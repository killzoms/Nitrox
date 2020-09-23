﻿using System.Reflection;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class CrashHome_Spawn_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CrashHome).GetMethod("Spawn", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix() // Disables Crashfish automatic spawning on the client
        {
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
