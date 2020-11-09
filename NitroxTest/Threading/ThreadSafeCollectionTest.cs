﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxModel.DataStructures;

namespace NitroxTest.Threading
{
    [TestClass]
    public class ThreadSafeCollectionTest
    {
        [TestMethod]
        public void ReadAndWriteSimultanious()
        {
            int iterations = 500000;

            ThreadSafeCollection<string> comeGetMe = new ThreadSafeCollection<string>(iterations);
            List<long> countsRead = new List<long>();
            long addCount = 0;

            Random r = new Random();
            DoReaderWriter(() =>
                {
                    countsRead.Add(Interlocked.Read(ref addCount));
                },
                i =>
                {
                    comeGetMe.Add(new string(Enumerable.Repeat(' ', 10).Select(c => (char)r.Next('A', 'Z')).ToArray()));
                    Interlocked.Increment(ref addCount);
                },
                iterations);

            addCount.Should().Be(iterations);
            countsRead.Count.Should().BeGreaterThan(0);
            countsRead.Last().Should().Be(iterations);
            comeGetMe.Count.Should().Be(iterations);
        }

        [TestMethod]
        public void IterateAndAddSimultanious()
        {
            int iterations = 500000;

            ThreadSafeCollection<string> comeGetMe = new ThreadSafeCollection<string>(iterations);
            long addCount = 0;
            long iterationsReadMany = 0;

            Random r = new Random();
            DoReaderWriter(() =>
                {
                    foreach (string item in comeGetMe)
                    {
                        item.Length.Should().BeGreaterThan(0);
                        Interlocked.Increment(ref iterationsReadMany);
                    }
                },
                i =>
                {
                    comeGetMe.Add(new string(Enumerable.Repeat(' ', 10).Select(c => (char)r.Next('A', 'Z')).ToArray()));
                    Interlocked.Increment(ref addCount);
                },
                iterations);

            addCount.Should().Be(iterations);
            iterationsReadMany.Should().BePositive();
        }

        [TestMethod]
        public void IterateAndAdd()
        {
            ThreadSafeCollection<int> nums = new ThreadSafeCollection<int>()
            {
                1,2,3,4,5
            };

            foreach (int num in nums)
            {
                if (num == 3)
                {
                    nums.Add(10);
                }
            }

            nums.Count.Should().Be(6);
            nums.Last().Should().Be(10);
        }

        private void DoReaderWriter(Action reader, Action<int> writer, int iterators)
        {
            ManualResetEvent barrier = new ManualResetEvent(false);
            Thread readerThread = new Thread(() =>
            {
                while (!barrier.SafeWaitHandle.IsClosed)
                {
                    reader();
                    Thread.Yield();
                }

                // Read one last time after writer finishes
                reader();
            });
            Thread writerThread = new Thread(() =>
            {
                for (int i = 0; i < iterators; i++)
                {
                    writer(i);
                }
                barrier.Set(); // Signal done
            });

            readerThread.Start();
            writerThread.Start();
            barrier.WaitOne(); // Wait for signal
        }
    }
}
