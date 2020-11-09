using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxClient.Unity.Helper;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.Logger;
using NitroxModel.Subnautica.DataStructures.GameLogic;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Seamoth_SubConstructionComplete_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(SeaMoth).GetMethod(nameof(SeaMoth.SubConstructionComplete), BindingFlags.Public | BindingFlags.Instance);

        public static bool Prefix(SeaMoth __instance)
        {
            // Suppress powered on if a seamoth´s default is set to false            
            NitroxId id = NitroxEntity.GetId(__instance.gameObject);
            Optional<SeamothModel> model = NitroxServiceLocator.LocateService<Vehicles>().TryGetVehicle<SeamothModel>(id);

            if (!model.HasValue)
            {
                Log.Error($"{nameof(Seamoth_SubConstructionComplete_Patch)}: Could not find {nameof(CyclopsModel)} by Nitrox id {id}.\nGO containing wrong id: {__instance.GetHierarchyPath()}");
                return false;
            }

            // Set lights of seamoth            
            ToggleLights toggleLights = __instance.gameObject.RequireComponentInChildren<ToggleLights>();
            toggleLights.lightsActive = model.Value.LightOn;
            return model.Value.LightOn;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
