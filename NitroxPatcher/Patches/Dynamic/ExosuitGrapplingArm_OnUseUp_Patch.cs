using System;
using System.Reflection;
using Harmony;
using NitroxClient.GameLogic;
using NitroxModel.Core;
using NitroxModel.Subnautica.Packets;

namespace NitroxPatcher.Patches.Dynamic
{
    public class ExosuitGrapplingArm_OnUseUp_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethodInterface = typeof(IExosuitArm).GetMethod(nameof(IExosuitArm.OnUseUp));

        public static void Prefix(ExosuitGrapplingArm __instance)
        {
            NitroxServiceLocator.LocateService<ExosuitModuleEvent>().BroadcastArmAction(TechType.ExosuitGrapplingArmModule, __instance, ExosuitArmAction.END_USE_TOOL);
        }

        public override void Patch(HarmonyInstance harmony)
        {
            InterfaceMapping interfaceMap = typeof(ExosuitGrapplingArm).GetInterfaceMap(typeof(IExosuitArm));
            MethodInfo targetMethod = interfaceMap.TargetMethods[Array.IndexOf(interfaceMap.InterfaceMethods, targetMethodInterface)];
            PatchPrefix(harmony, targetMethod);
        }
    }
}
