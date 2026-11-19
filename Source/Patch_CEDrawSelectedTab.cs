using HarmonyLib;
using System;
using System.Reflection;
using UnityEngine;
using Verse;


namespace CharacterEditorSkillFix
{
    public static class Patch_DrawSelectedTab
    {
        public static MethodBase TargetMethod() => CEReflection.DrawSelectedTabMethod;

        private static Pawn cachedPawn;

        public static void Postfix(object __instance, int x, int y, int w, int h)
        {
            if (Event.current.type == EventType.Layout)
                return;

            if (!ExpertiseUI.ExpertiseTabState.Active)
            {
                return;
            }

            if (CEReflection.CurrentTabField == null ||
                CEReflection.APIProp == null ||
                CEReflection.PawnProp == null)
            {
                return;
            }

            var currentTab = CEReflection.CurrentTabField.GetValue(__instance);
            if (!Equals(currentTab, CEReflection.Sentinel))
            {
                return;
            }

            var api = CEReflection.APIProp.GetValue(null);
            if (api == null)
            {
                return;
            }

            Pawn pawn = CEReflection.PawnProp.GetValue(api) as Pawn;
            if (pawn == null)
            {
                return;
            }

            Rect rect = new Rect(x, y, w, h);

            if (pawn != cachedPawn || !ExpertiseCache.Valid)
            {
                cachedPawn = pawn;
                Log.Warning($"Pawn changed to {pawn.Name}. Rebuilding ExpertiseUI cache.");
                ExpertiseUI.RebuildCache(pawn, rect);
            }

            GUI.color = Color.white;
            ExpertiseUI.Draw(rect);
        }

    }
}
