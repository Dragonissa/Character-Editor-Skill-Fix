using HarmonyLib;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;
using Verse;

namespace CharacterEditorSkillFix
{

    public static class CE_ModifyTabWidth
    {
        public static MethodBase TargetMethod() => CEReflection.DrawTabsMethod;

        public static IEnumerable<CodeInstruction> Transpiler(IEnumerable<CodeInstruction> instructions)
        {
            var codes = new List<CodeInstruction>(instructions);
            bool patched = false;

            for (int i = 0; i < codes.Count - 3; i++)
            {
                // Look for: ldarg.3 → ldc.i4.8 → div → stloc.0
                if (codes[i].opcode == OpCodes.Ldarg_3 &&
                    codes[i + 1].opcode == OpCodes.Ldc_I4_8 &&
                    codes[i + 2].opcode == OpCodes.Div &&
                    codes[i + 3].opcode == OpCodes.Stloc_0)
                {
                    Log.Warning("[CharacterEditorSkillFix] Found tab width divisor, patching 8 → 9");

                    codes[i + 1] = new CodeInstruction(OpCodes.Ldc_I4, 9);
                    patched = true;
                    break;
                }
            }

            if (!patched)
                Log.Error("[CharacterEditorSkillFix] ERROR: Could not find tab width divisor!");

            return codes;
        }
    }
}