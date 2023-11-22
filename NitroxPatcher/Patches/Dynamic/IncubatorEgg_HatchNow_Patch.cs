using System.Reflection;
using NitroxClient.GameLogic;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.DataStructures;
using NitroxModel.DataStructures.GameLogic.Entities;
using NitroxModel.Helper;
using NitroxModel_Subnautica.DataStructures;
using UnityEngine;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class IncubatorEgg_HatchNow_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((IncubatorEgg t) => t.HatchNow());

    public static bool Prefix(IncubatorEgg __instance)
    {
        StartHatchingVisuals(__instance);

        SpawnBabiesIfSimulating(__instance);

        return false;
    }

    private static void StartHatchingVisuals(IncubatorEgg egg)
    {
        egg.fxControl.Play();
        Utils.PlayFMODAsset(egg.hatchSound, egg.transform, 30f);

        SafeAnimator.SetBool(egg.animationController.eggAnimator, egg.animParameter, true);
    }

    private static void SpawnBabiesIfSimulating(IncubatorEgg egg)
    {
        SimulationOwnership simulationOwnership = NitroxServiceLocator.LocateService<SimulationOwnership>();

        NitroxEntity serverKnownParent = egg.GetComponentInParent<NitroxEntity>();
        Validate.NotNull(serverKnownParent, "Could not find a server known parent for incubator egg");

        // Only spawn the babies if we are simulating the main incubator platform.
        if (simulationOwnership.HasAnyLockType(serverKnownParent.Id))
        {
            GameObject baby = UnityEngine.Object.Instantiate<GameObject>(egg.seaEmperorBabyPrefab);
            baby.transform.position = egg.attachPoint.transform.position;
            baby.transform.localRotation = Quaternion.identity;

            NitroxId babyId = NitroxEntity.GenerateNewId(baby);

            WorldEntity entity = new(baby.transform.position.ToDto(), baby.transform.rotation.ToDto(), baby.transform.localScale.ToDto(), TechType.SeaEmperorBaby.ToDto(), 3, "09883a6c-9e78-4bbf-9561-9fa6e49ce766", false, babyId, null);
            Resolve<Entities>().BroadcastEntitySpawnedByClient(entity);
        }
    }
}
