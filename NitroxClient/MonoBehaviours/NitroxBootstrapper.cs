using NitroxClient.MonoBehaviours.Gui.MainMenu;
using NitroxModel.Logger;
using UnityEngine;

namespace NitroxClient.MonoBehaviours
{
    public class NitroxBootstrapper : MonoBehaviour
    {
        public void Awake()
        {
            DontDestroyOnLoad(gameObject);
            gameObject.AddComponent<SceneCleanerPreserve>();
            gameObject.AddComponent<MainMenuMods>();

#if DEBUG
            EnableDeveloperFeatures();
#endif

            CreateDebugger();
        }

        private static void EnableDeveloperFeatures()
        {
            Log.Info("Enabling developer console.");
            DevConsole.disableConsole = false;
            Application.runInBackground = true;
            Log.Info($"Unity run in background set to \"{Application.runInBackground}\"");
        }

        private void CreateDebugger()
        {
            Instantiate(new GameObject("Debug manager"), transform).AddComponent<NitroxDebugManager>();
        }
    }
}
