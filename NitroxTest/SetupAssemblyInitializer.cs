using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxModel.Core;
using NitroxModel.Logger;

namespace NitroxTest
{
    [TestClass]
    public static class SetupAssemblyInitializer
    {
        [AssemblyInitialize]
        public static void AssemblyInit(TestContext context)
        {
            NitroxEnvironment.Set(NitroxEnvironmentType.TESTING);
            Log.Setup();
        }
    }
}
