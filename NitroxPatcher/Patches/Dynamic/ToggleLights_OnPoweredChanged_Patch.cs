using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel_Subnautica.DataStructures.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ToggleLights_OnPoweredChanged_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ToggleLights).GetMethod("OnPoweredChanged", BindingFlags.NonPublic | BindingFlags.Instance);

        public static bool Prefix(ToggleLights __instance, bool powered)
        {
            // Suppress powered on if a seamoth´s default is set to false            
            if (__instance.GetComponentInParent<SeaMoth>() && powered)
            {
                NitroxId id = NitroxEntity.GetId(__instance.transform.parent.gameObject);
                SeamothModel model = NitroxServiceLocator.LocateService<Vehicles>().GetVehicles<SeamothModel>(id);
                return (model.LightOn == __instance.lightsActive);
            }

            return true;
        }


        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
