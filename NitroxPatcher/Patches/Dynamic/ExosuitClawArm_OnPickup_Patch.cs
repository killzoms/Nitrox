using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;
using NitroxModel.Logger;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ExosuitClawArm_OnPickup_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(ExosuitClawArm).GetMethod(nameof(ExosuitClawArm.OnPickup));

        public static bool Prefix(ExosuitClawArm __instance)
        {
            Exosuit componentInParent = __instance.GetComponentInParent<Exosuit>();
            if (componentInParent && componentInParent.GetActiveTarget())
            {
                Pickupable pickupable = componentInParent.GetActiveTarget().GetComponent<Pickupable>();
                PickPrefab component = componentInParent.GetActiveTarget().GetComponent<PickPrefab>();
                if (pickupable && pickupable.isPickupable && componentInParent.storageContainer.container.HasRoomFor(pickupable))
                {
                    NitroxServiceLocator.LocateService<Item>().PickedUp(pickupable.gameObject, pickupable.GetTechType());
                }
                else if (component)
                {
                    Log.Debug("Delete Pickprefab for exosuit claw arm");
                    NitroxServiceLocator.LocateService<Item>().PickedUp(component.gameObject, component.pickTech);
                }
            }
            return true;
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
