using RimWorld;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Verse;

namespace CharacterEditorSkillFix
{
    internal class ExpertiseUI
    {

        public static void RebuildCache(Pawn pawn, Rect inRect)
        {
            ExpertiseCache.Valid = false;
            ExpertiseCache.Pawn = pawn;
            object tracker = GetVSETracker(pawn);

            ExpertiseCache.Tracker = tracker;

            var allExpertiseProp = tracker.GetType().GetProperty("AllExpertise");
            if (allExpertiseProp == null)
            {
                    return;
            }
                
            var recsObj = allExpertiseProp.GetValue(tracker);
            var recs = ((IEnumerable<object>)recsObj).ToList();

            ExpertiseCache.Records = new List<object>();
            ExpertiseCache.Labels = new List<string>();
            ExpertiseCache.LevelStrings = new List<string>();
            ExpertiseCache.Tooltips = new List<string>();
            ExpertiseCache.RowRects = new List<Rect>();
            ExpertiseCache.RecordTypes = new List<Type>();
            ExpertiseCache.LevelProps = new List<PropertyInfo>();
            ExpertiseCache.LabelRects = new List<Rect>();
            ExpertiseCache.LevelRects = new List<Rect>();
            ExpertiseCache.MinusRects = new List<Rect>();
            ExpertiseCache.PlusRects = new List<Rect>();

            int count = recs.Count;

            float rowHeight = 24f;
            float curY = inRect.y + 32f;

            for (int i = 0; i < count; i++)
            {
                var rec = recs[i];
                if (rec == null)
                {
                    Log.Error($"[ExpertiseUI] rec[{i}] is NULL");
                    continue;
                }

                var recType = rec.GetType();

                var defField = recType.GetField("def");
                if (defField == null)
                    continue;

                var def = defField.GetValue(rec);
                if (def == null)
                {
                    Log.Error($"[ExpertiseUI] rec[{i}] def is NULL");
                    continue;
                }

                var defType = def.GetType();

                var labelCapProp = defType.GetProperty("LabelCap");
                if (labelCapProp == null)
                {
                    Log.Error($"[ExpertiseUI] rec[{i}] def has NO 'LabelCap' property");
                    continue;
                }

                var labelObj = labelCapProp.GetValue(def);
                string label = labelObj?.ToString() ?? "";

                // rec.Level
                var levelProp = recType.GetProperty("Level");
                int level = Convert.ToInt32(levelProp.GetValue(rec));

                ExpertiseCache.Records.Add(rec);
                ExpertiseCache.RecordTypes.Add(recType);
                ExpertiseCache.LevelProps.Add(levelProp);

                ExpertiseCache.Labels.Add(label);
                ExpertiseCache.LevelStrings.Add(level.ToString());
                ExpertiseCache.Tooltips.Add(BuildTooltip(rec));

                var row = new Rect(inRect.x, curY, inRect.width - 20f, rowHeight);
                ExpertiseCache.RowRects.Add(row);

                ExpertiseCache.LabelRects.Add(new Rect(row.x + 6f, row.y, 160f, row.height));
                ExpertiseCache.LevelRects.Add(new Rect(row.x + 170f, row.y, 30f, row.height));
                ExpertiseCache.MinusRects.Add(new Rect(row.x + 170f + 30f + 4f, row.y + 2f, 22f, row.height - 4f));
                ExpertiseCache.PlusRects.Add(new Rect(row.x + 170f + 30f + 4f + 22f + 2f, row.y + 2f, 22f, row.height - 4f));

                curY += rowHeight;
            }

            ExpertiseCache.Valid = true;
        }

        public static void UpdateRecord(object rec)
        {
            int index = ExpertiseCache.Records.IndexOf(rec);
            if (index < 0)
                return;

            var recType = rec.GetType();

            // rec.Level
            var levelProp = recType.GetProperty("Level");
            int level = Convert.ToInt32(levelProp.GetValue(rec));

            ExpertiseCache.LevelStrings[index] = level.ToString();
            ExpertiseCache.Tooltips[index] = BuildTooltip(rec);
        }


        private static string BuildTooltip(object rec)
        {
            var recType = rec.GetType();

            var fullDescMethod = recType.GetMethod("FullDescription");
            if (fullDescMethod != null)
            {
                return (string)fullDescMethod.Invoke(rec, null);
            }

            return "<color=#D9D948>Expertise</color>\n<color=#999999>No description available.</color>";
        }
        public static void Draw(Rect inRect)
        {
            if (!ExpertiseCache.Valid)
                return;

            Text.Font = GameFont.Medium;
            Widgets.Label(new Rect(inRect.x, inRect.y, inRect.width, 30f), "Expertise");
            Text.Font = GameFont.Small;

            if (ExpertiseCache.Records.Count == 0)
            {
                Widgets.Label(new Rect(inRect.x, inRect.y + 40f, inRect.width, 30f),
                    "This pawn has no expertises yet.");
                return;
            }

            int count = ExpertiseCache.Labels.Count;
            for (int i = 0; i < count; i++)
            {
                Text.Anchor = TextAnchor.MiddleLeft;

                Rect row = ExpertiseCache.RowRects[i]; // ⭐ Already in correct space

                Widgets.DrawHighlightIfMouseover(row);

                Widgets.Label(ExpertiseCache.LabelRects[i], ExpertiseCache.Labels[i]);
                Widgets.Label(ExpertiseCache.LevelRects[i], ExpertiseCache.LevelStrings[i]);

                object rec = ExpertiseCache.Records[i];
                var levelProp = ExpertiseCache.LevelProps[i];
                int level = int.Parse(ExpertiseCache.LevelStrings[i]);

                if (Widgets.ButtonText(ExpertiseCache.MinusRects[i], "-") && level > 0)
                {
                    levelProp.SetValue(rec, level - 1);
                    UpdateRecord(rec);
                }

                if (Widgets.ButtonText(ExpertiseCache.PlusRects[i], "+") && level < 20)
                {
                    levelProp.SetValue(rec, level + 1);
                    UpdateRecord(rec);
                }

                TooltipHandler.TipRegion(row, ExpertiseCache.Tooltips[i]);

                Text.Anchor = TextAnchor.UpperLeft;
            }
        }
    
        public static object GetVSETracker(Pawn pawn)
        {
            if (pawn == null)
                return null;

            // Find the ExpertiseTrackers class
            var extType = GenTypes.AllTypes
                .FirstOrDefault(t => t.Name == "ExpertiseTrackers");

            if (extType == null)
            {
                Log.Warning("[CharacterEditorSkillFix] ExpertiseTrackers class not found.");
                return null;
            }

            // Try the overload that takes Pawn
            var pawnMethod = extType.GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "Expertise" &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Pawn)
                );

            if (pawnMethod != null)
                return pawnMethod.Invoke(null, new object[] { pawn });

            // Try the overload that takes Pawn_SkillTracker
            var skillMethod = extType.GetMethods()
                .FirstOrDefault(m =>
                    m.Name == "Expertise" &&
                    m.GetParameters().Length == 1 &&
                    m.GetParameters()[0].ParameterType == typeof(Pawn_SkillTracker)
                );

            if (skillMethod != null)
                return skillMethod.Invoke(null, new object[] { pawn.skills });

            Log.Warning("[CharacterEditorSkillFix] No Expertise() method found.");
            return null;
        }
        public static class ExpertiseTabState
        {
            public static bool Active;
        }
    }

}

