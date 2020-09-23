using System.Collections.Generic;
using System.Linq;
using Harmony;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxPatcher.Patches.Dynamic;
using NitroxTest.Patcher.Test;

namespace NitroxTest.Patcher.Patches
{
    [TestClass]
    public class BaseGhost_Finish_PatchTest
    {
        [TestMethod]
        public void Sanity()
        {
            List<CodeInstruction> instructions = PatchTestHelper.GenerateDummyInstructions(100);
            instructions.Add(new CodeInstruction(BaseGhost_Finish_Patch.injectionOpCode, BaseGhost_Finish_Patch.injectionOperand));

            IEnumerable<CodeInstruction> result = BaseGhost_Finish_Patch.Transpiler(null, instructions);

            Assert.AreEqual(instructions.Count + 3, result.Count());
        }

        [TestMethod]
        public void InjectionSanity()
        {
            IEnumerable<CodeInstruction> beforeInstructions = PatchTestHelper.GetInstructionsFromMethod(BaseGhost_Finish_Patch.targetMethod);
            IEnumerable<CodeInstruction> result = BaseGhost_Finish_Patch.Transpiler(BaseGhost_Finish_Patch.targetMethod, beforeInstructions);

            Assert.IsTrue(beforeInstructions.Count() < result.Count());
        }
    }
}
