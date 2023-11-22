using System.Reflection;
using NitroxClient.MonoBehaviours.Gui.Input;
using NitroxClient.MonoBehaviours.Gui.Input.KeyBindings;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Dynamic;

public sealed partial class Player_Update_Patch : NitroxPatch, IDynamicPatch
{
    public override MethodInfo targetMethod { get; } = Reflect.Method((Player t) => t.Update());

    public static void Postfix(Player __instance)
    {
        // TODO: Use proper way to check if input is free, because players can be editing labels etc.
        if (DevConsole.instance.state)
        {
            return;
        }

        KeyBindingManager keyBindingManager = new();
        foreach (KeyBinding keyBinding in keyBindingManager.KeyboardKeyBindings)
        {
            if (GameInput.GetButtonDown(keyBinding.Button))
            {
                keyBinding.Action.Execute();
            }
        }
    }
}
