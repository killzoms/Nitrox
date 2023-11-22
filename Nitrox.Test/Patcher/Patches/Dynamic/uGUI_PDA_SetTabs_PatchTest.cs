using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HarmonyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxTest.Patcher;

namespace NitroxPatcher.Patches.Dynamic;

[TestClass]
public class uGUI_PDA_SetTabs_PatchTest
{
    [TestMethod]
    public void Sanity()
    {
        List<CodeInstruction> instructions = PatchTestHelper.GenerateDummyInstructions(100);
        instructions.Add(new CodeInstruction(uGUI_PDA_SetTabs_Patch.INJECTION_OPCODE));

        IEnumerable<CodeInstruction> result = uGUI_PDA_SetTabs_Patch.Transpiler(new uGUI_PDA_SetTabs_Patch().targetMethod, instructions);
        Assert.AreEqual(instructions.Count + 3, result.Count());
    }

    [TestMethod]
    public void InjectionSanity()
    {
        ReadOnlyCollection<CodeInstruction> beforeInstructions = PatchTestHelper.GetInstructionsFromMethod(new uGUI_PDA_SetTabs_Patch().targetMethod);
        IEnumerable<CodeInstruction> result = uGUI_PDA_SetTabs_Patch.Transpiler(new uGUI_PDA_SetTabs_Patch().targetMethod, beforeInstructions);

        Assert.IsTrue(beforeInstructions.Count < result.Count());
    }
}
