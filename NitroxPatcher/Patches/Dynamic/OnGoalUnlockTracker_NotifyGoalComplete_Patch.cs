using System.Collections.Generic;
using System.Reflection;
using Harmony;
using NitroxClient.Communication.Abstract;
using NitroxModel.Core;
using NitroxModel.DataStructures.GameLogic;
using NitroxModel.Packets;
using Story;

namespace NitroxPatcher.Patches.Dynamic
{
    public class OnGoalUnlockTracker_NotifyGoalComplete_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(OnGoalUnlockTracker).GetMethod("NotifyGoalComplete", BindingFlags.Public | BindingFlags.Instance);

        public static void Prefix(OnGoalUnlockData __instance, string completedGoal, Dictionary<string, OnGoalUnlock> ___goalUnlocks)
        {
            if (___goalUnlocks.TryGetValue(completedGoal, out OnGoalUnlock _))
            {
                StoryEventSend packet = new StoryEventSend(StoryEventType.GOAL_UNLOCK, completedGoal);
                NitroxServiceLocator.LocateService<IPacketSender>().Send(packet);
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPrefix(harmony, targetMethod);
        }
    }
}
