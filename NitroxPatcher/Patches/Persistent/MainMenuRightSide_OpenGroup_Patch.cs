using System.Reflection;
using NitroxClient.MonoBehaviours.Gui.MainMenu;
using NitroxModel.Helper;

namespace NitroxPatcher.Patches.Persistent
{
    public partial class MainMenuRightSide_OpenGroup_Patch : NitroxPatch, IPersistentPatch
    {
        public override MethodInfo targetMethod { get; } = Reflect.Method((MainMenuRightSide t) => t.OpenGroup(default(string)));

        public static void Prefix(string target)
        {
            // Don't stop the client if the client is trying to connect (in the case: target = "Join Server")
            // We can detect that the Join Server tab is still available because the gameobject is activated only when the tab is opened
            if (MainMenuMultiplayerPanel.Main.JoinServer.gameObject.activeSelf && !target.Equals(MainMenuMultiplayerPanel.Main.JoinServer.MenuName))
            {
                MainMenuMultiplayerPanel.Main.JoinServer.StopMultiplayerClient();
            }
        }
    }
}
