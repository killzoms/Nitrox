using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Harmony;

namespace NitroxPatcher.Patches.Dynamic
{
    public class IngameMenu_OnSelect_Patch : NitroxPatch, IDynamicPatch
    {
        private static readonly MethodInfo targetMethod = typeof(IngameMenu).GetMethod(nameof(IngameMenu.OnSelect), BindingFlags.Public | BindingFlags.Instance);
        private static readonly MethodInfo gameModeUtilsIsPermadeathMethod = typeof(GameModeUtils).GetMethod(nameof(GameModeUtils.IsPermadeath), BindingFlags.Public | BindingFlags.Static);

        public static void Postfix()
        {
            IngameMenu.main.saveButton.gameObject.SetActive(false);
            IngameMenu.main.quitToMainMenuButton.interactable = true;
        }

        public static IEnumerable<CodeInstruction> Transpiler(MethodBase original, IEnumerable<CodeInstruction> instructions)
        {
            /* Early return cuts out
             * if (GameModeUtils.IsPermadeath())
		     * {
			 *    this.quitToMainMenuText.text = Language.main.Get("SaveAndQuitToMainMenu");
			 *    this.saveButton.gameObject.SetActive(false);
		     * }
		     * else
		     * {
			 *     this.saveButton.interactable = this.GetAllowSaving();
			 *     this.quitToMainMenuButton.interactable = true;
		     * }
             * if (PlatformUtils.isXboxOnePlatform)
		     * {
			 *      this.helpButton.gameObject.SetActive(true);
		     * }
             */
            foreach (CodeInstruction instruction in instructions)
            {
                if (gameModeUtilsIsPermadeathMethod.Equals(instruction.operand))
                {
                    yield return new CodeInstruction(OpCodes.Ret);
                    break;
                }

                yield return instruction;
            }
        }

        public override void Patch(HarmonyInstance harmony)
        {
            PatchMultiple(harmony, targetMethod, false, true, true);
        }
    }
}
