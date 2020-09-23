﻿using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxClient.GameLogic.Helper;
using NitroxClient.MonoBehaviours;
using NitroxModel.Core;
using NitroxModel.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    public class PingManager_NotifyRename_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(PingManager).GetMethod(nameof(PingManager.NotifyRename), BindingFlags.Public | BindingFlags.Static);

        public static void Postfix(PingInstance instance)
        {
            // Only beacons are synced here (not mission, vehicle or other signals) because spawning is handled differently for non-droppable entities
            if (!instance || !instance.GetComponent<Beacon>())
            {
                return;
            }

            PingRenamed packet = new PingRenamed(NitroxEntity.GetId(instance.gameObject), instance.GetLabel(), SerializationHelper.GetBytes(instance.gameObject));
            NitroxServiceLocator.LocateService<IPacketSender>().Send(packet);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
