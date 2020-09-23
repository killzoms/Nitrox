using System;
using System.Reflection;
using Harmony;
using NitroxClient.MonoBehaviours.Gui.Input;
using NitroxClient.MonoBehaviours.Gui.Input.KeyBindings;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic
{
    public class Player_Update_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(Player).GetMethod(nameof(Player.Update), BindingFlags.Public | BindingFlags.Instance);
        private static DevConsole devConsoleInstance;

        public static void Postfix(Player __instance)
        {
            // TODO: Use proper way to check if input is free, because players can be editing labels etc.
            if (!devConsoleInstance)
            {
                devConsoleInstance = (DevConsole)ReflectionHelper.ReflectionGet<DevConsole>(null, "instance", false, true);
            }

            if ((bool)devConsoleInstance.ReflectionGet("state", false, false))
            {
                return;
            }

            KeyBindingManager keyBindingManager = new KeyBindingManager();

            foreach (KeyBinding keyBinding in keyBindingManager.KeyboardKeyBindings)
            {
                bool isButtonDown = (bool)ReflectionHelper.ReflectionCall<GameInput>(null, "GetButtonDown", new Type[] { typeof(GameInput.Button) }, true, true, keyBinding.Button);

                if (isButtonDown)
                {
                    keyBinding.Action.Execute();
                }
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchPostfix(harmony, targetMethod);
        }
    }
}
