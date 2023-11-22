using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using FluentAssertions;
using HarmonyLib;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NitroxTest.Patcher;
using static NitroxPatcher.Patches.Dynamic.SpawnOnKill_OnKill_Patch;

namespace NitroxPatcher.Patches.Dynamic;

[TestClass]
public class SpawnOnKill_OnKill_PatchTest
{
    [TestMethod]
    public void Sanity()
    {
        ReadOnlyCollection<CodeInstruction> originalIl = PatchTestHelper.GetInstructionsFromMethod(new SpawnOnKill_OnKill_Patch().targetMethod);
        IEnumerable<CodeInstruction> transformedIl = Transpiler(new SpawnOnKill_OnKill_Patch().targetMethod, originalIl);
        originalIl.Count.Should().Be(transformedIl.Count() - 3);
    }
}
