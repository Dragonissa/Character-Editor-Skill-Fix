using HarmonyLib;
using Verse;

namespace CharacterEditorSkillFix
{
    [StaticConstructorOnStartup]
    public static class CEReflectionBootstrap
    {
        static CEReflectionBootstrap()
        {
            CEReflection.Initialize();

            var harmony = new Harmony("dragonissa.CharacterEditorSkillFix");

            Log.Message("[CharacterEditorSkillFix] Patching Character Editor...");

            if (SkillOrderManager.SkillCount > 13)
            {
                var originalMethod = CEReflection.DrawMethod;
                var prefix = typeof(Patch_Draw).GetMethod(nameof(Patch_Draw.Prefix));
                if (originalMethod != null && prefix != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching Draw Method.");
                    harmony.Patch(originalMethod, prefix: new HarmonyMethod(prefix));
                }
            }

            var originalMethod1 = CEReflection.GetSkillIndexMethod;
            var prefix1 = typeof(Patch_GetSkillIndex).GetMethod(nameof(Patch_GetSkillIndex.Prefix));
            if (originalMethod1 != null && prefix1 != null)
            {
                Log.Message("[CharacterEditorSkillFix] Patching GetSkillIndex Method.");
                harmony.Patch(originalMethod1, prefix: new HarmonyMethod(prefix1));
            }

            var modVSE = ModLister.GetActiveModWithIdentifier("vanillaexpanded.skills");
            if (modVSE != null)
            {
                Log.Warning("[CharacterEditorSkillFix] Detected Vanilla Expanded - Skills. Patching...");

                var originalMethod = CEReflection.DrawSelectedTabMethod;
                var postfix = typeof(Patch_DrawSelectedTab)
                .GetMethod(nameof(Patch_DrawSelectedTab.Postfix));

                if (originalMethod != null && postfix != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching DrawSelectedTab for VSE compatibility.");
                    harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfix));
                }

                originalMethod = CEReflection.DrawTabsMethod;
                postfix = typeof(CE_AddExpertiseTabButton).GetMethod(nameof(CE_AddExpertiseTabButton.Postfix));

                if (originalMethod != null && postfix != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching DrawTabs for VSE compatibility.");
                    harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfix));
                }

                var transpiler = typeof(CE_ModifyTabWidth).GetMethod(nameof(CE_ModifyTabWidth.Transpiler));
                if (originalMethod != null && transpiler != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching DrawTabs transpiler for VSE compatibility.");
                    harmony.Patch(originalMethod, transpiler: new HarmonyMethod(transpiler));
                }

                originalMethod = CEReflection.ATabwechsel;
                postfix = typeof(CE_TabSwitch).GetMethod(nameof(CE_TabSwitch.Postfix));
                if (originalMethod != null && postfix != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching ATabwechsel for VSE compatibility.");
                    harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfix));
                }

                originalMethod = CEReflection.Close;
                postfix = typeof(Patch_EditorUI_Close).GetMethod(nameof(Patch_EditorUI_Close.Postfix));
                if (originalMethod != null && postfix != null)
                {
                    Log.Message("[CharacterEditorSkillFix] Patching Close for VSE compatibility.");
                    harmony.Patch(originalMethod, postfix: new HarmonyMethod(postfix));
                }
            } else {
                Log.Warning("[CharacterEditorSkillFix] Vanilla Expanded - Skills not detected. Skipping VSE patches.");
            }
        }
    }
}