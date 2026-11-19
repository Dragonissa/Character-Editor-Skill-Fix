using HarmonyLib;
using RimWorld;
using System.Runtime.CompilerServices;
using Verse;

namespace CharacterEditorSkillFix
{
    [HarmonyPatch(typeof(CharacterEditor.SkillTool), nameof(CharacterEditor.SkillTool.GetSkillIndex))]
    public class Patch_GetSkillIndex
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Prefix(SkillDef skill, ref int __result)
        {
            if (SkillOrderManager.NormalizedOrder.TryGetValue(skill, out int index))
            {
                Log.Message($"[CharacterEditorSkillFix] GetSkillIndex: {skill.defName} -> {index}");
                __result = index;
                return false;
            }

            Log.WarningOnce($"[CharacterEditorSkillFix] Skill {skill.defName} missing from NormalizedOrder", skill.shortHash);
            __result = 999;
            return false;
        }
    }

}