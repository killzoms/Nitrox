using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NitroxServer.GameLogic
{
    public static class TickTime
    {
        private static Stopwatch watch = new Stopwatch();

        public static float DeltaTime => watch.ElapsedMilliseconds / 1000f;



        internal static void Restart()
        {
            watch.Restart();
        }
    }
}
