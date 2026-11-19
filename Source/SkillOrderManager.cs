using RimWorld;
using Verse;
using System.Collections.Generic;

namespace CharacterEditorSkillFix
{
    [StaticConstructorOnStartup]
    public static class SkillOrderManager
    {
        public static readonly Dictionary<SkillDef, int> NormalizedOrder;
        public static readonly int SkillCount;

        static SkillOrderManager()
        {
            var skills = DefDatabase<SkillDef>.AllDefsListForReading;
            int count = skills.Count;
            SkillCount = count;

            List<(SkillDef skill, int order)> ordered = new List<(SkillDef, int)>(count);

            foreach (var s in skills)
            {
                int order = s.GetModExtension<SkillOrderExtension>()?.order ?? int.MaxValue;
                if (order == int.MaxValue)
                {
                    Log.Error($"[CharacterEditorSkillFix] Skill {s.defName} missing SkillOrderExtension and will break Character Editor Skilllist.");
                    continue;
                }
                ordered.Add((s, order));
            }

            ordered.Sort((a, b) => b.order.CompareTo(a.order));

            NormalizedOrder = new Dictionary<SkillDef, int>(count);

            int index = 0;
            foreach (var entry in ordered)
            {
                NormalizedOrder[entry.skill] = index++;
            }
        }
    }
}
