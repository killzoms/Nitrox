using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using Harmony;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxPatcher.Patches.Dynamic;
using NitroxTest.Patcher.Test;

namespace NitroxTest.Patcher.Patches
{
    [TestClass]
    public class BuilderPatchTest
    {
        [TestMethod]
        public void Sanity()
        {
            List<CodeInstruction> instructions = PatchTestHelper.GenerateDummyInstructions(100);
            instructions.Add(new CodeInstruction(Builder_TryPlace_Patch.placeBaseInjectionOpCode, Builder_TryPlace_Patch.placeBaseInjectionOperand));
            instructions.Add(new CodeInstruction(Builder_TryPlace_Patch.placeFurnitureInjectionOpCode, Builder_TryPlace_Patch.placeFurnitureInjectionOperand));

            IEnumerable<CodeInstruction> result = Builder_TryPlace_Patch.Transpiler(null, instructions);
            Assert.AreEqual(120, result.Count());
        }

        [TestMethod]
        public void InjectionSanity()
        {
            MethodInfo targetMethod = AccessTools.Method(typeof(Builder), "TryPlace");
            ReadOnlyCollection<CodeInstruction> beforeInstructions = PatchTestHelper.GetInstructionsFromMethod(targetMethod);

            IEnumerable<CodeInstruction> result = Builder_TryPlace_Patch.Transpiler(targetMethod, beforeInstructions);
            Assert.IsTrue(beforeInstructions.Count < result.Count());
        }
    }
}
