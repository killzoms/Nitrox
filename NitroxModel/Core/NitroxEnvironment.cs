using System;

namespace NitroxModel.Core
{
    public static class NitroxEnvironment
    {
        private static bool hasBeenSet;
        private static NitroxEnvironmentType type = NitroxEnvironmentType.NORMAL;

        public static bool IsNormal => type == NitroxEnvironmentType.NORMAL;
        public static bool IsTesting => type == NitroxEnvironmentType.TESTING;

        public static void Set(NitroxEnvironmentType value)
        {
            if (hasBeenSet)
            {
                throw new InvalidOperationException("NitroxEnvironmentTypes can only be set once.");
            }

            type = value;
            hasBeenSet = true;
        }
    }

    public enum NitroxEnvironmentType
    {
        NORMAL,
        TESTING
    }
}
