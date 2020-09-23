using System.Reflection;
using Harmony;
using NitroxModel.Helper;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Stalker_CheckLoseTooth_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Stalker).GetMethod("CheckLoseTooth", BindingFlags.NonPublic | BindingFlags.Instance);

        //GetComponent<HardnessMixin> was returning null for everything instead of a HardnessMixin with a hardness value. Since this component 
        //isn't used for anything else than the stalker teeth drop, we hard-code the values and bingo.
        public static bool Prefix(Stalker __instance, GameObject target)
        {
            TechType techType = CraftData.GetTechType(target);
            float dropProbability = techType == TechType.ScrapMetal ? 0.15f : 0f;

            if (UnityEngine.Random.value < dropProbability)
            {
                __instance.ReflectionCall("LoseTooth");
            }
            return false;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
