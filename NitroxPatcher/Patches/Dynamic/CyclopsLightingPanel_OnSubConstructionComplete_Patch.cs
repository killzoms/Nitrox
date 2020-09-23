using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.Util;
using NitroxModel.Logger;
using NitroxModel_Subnautica.DataStructures.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    class CyclopsLightingPanel_OnSubConstructionComplete_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(CyclopsLightingPanel).GetMethod(nameof(CyclopsLightingPanel.SubConstructionComplete), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(CyclopsLightingPanel __instance)
        {
            // Suppress powered on if a cyclops´s floodlight is set to false            
            NitroxId id = NitroxEntity.GetId(__instance.gameObject.transform.parent.gameObject);// gameObject = LightsControl, Parent = main cyclops game object
            Optional<CyclopsModel> model = NitroxServiceLocator.LocateService<Vehicles>().TryGetVehicle<CyclopsModel>(id);
            if (!model.HasValue)
            {
                Log.Error($"[{nameof(CyclopsLightingPanel_OnSubConstructionComplete_Patch)}] Could not find {nameof(CyclopsModel)} by Nitrox id {id}.\nGO containing wrong id: {__instance.GetHierarchyPath()}");
                return false;
            }

            return model.Value.FloodLightsOn;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
