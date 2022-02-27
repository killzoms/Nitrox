using System;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.Unity;

namespace NitroxServer.GameLogic.Entities.Spawning
{
    public class DeterministicBatchGenerator
    {
        private readonly Random random;

        public DeterministicBatchGenerator(string seed, NitroxInt3 batchId)
        {
            random = new Random(seed.GetHashCode() + batchId.GetHashCode());
        }

        /// <returns>Double ranging from 0 to 1</returns>
        public double NextDouble()
        {
            return random.NextDouble();
        }

        /// <returns>Double ranging from min to max</returns>
        public double NextRange(double min, double max)
        {
            return NextDouble() * (max - min) + min;
        }

        public NitroxVector3 NextInsideUnitSphere()
        {
            double u = NextDouble();
            double x1 = NextRange(-1f, 1f);
            double x2 = NextRange(-1f, 1f);
            double x3 = NextRange(-1f, 1f);

            double mag = Math.Sqrt(x1 * x1 + x2 * x2 + x3 * x3);
            x1 /= mag;
            x2 /= mag;
            x3 /= mag;

            // Math.cbrt is cube root
            double c = Math.Pow(u, 1d/3d);

            return new NitroxVector3((float)(x1 * c), (float)(x2 * c), (float)(x3 * c));
        }

        public int NextInt(int min, int max)
        {
            return random.Next(min, max);
        }

        public NitroxId NextId()
        {
            byte[] bytes = new byte[16];
            random.NextBytes(bytes);
            return new NitroxId(bytes);
        }
    }
}
