using System.Reflection;
using UnityEngine;
using Verse;

namespace CharacterEditorSkillFix
{
    public static class Patch_Draw
    {
        public static MethodBase TargetMethod() => CEReflection.DrawMethod;

        private static Vector2 scrollPos = Vector2.zero;

        public static bool Prefix(object __instance, object c)
        {
            int skillCount = SkillOrderManager.SkillCount;
            if (skillCount <= 13)
            {
                return true; //No patching needed, just use the original method
            }

            if (CEReflection.CurrentTabField == null ||
                CEReflection.APIProp == null ||
                CEReflection.PawnProp == null)
            {
                return false;
            }

            int x = (int)c.GetType().GetField("x", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            int y = (int)c.GetType().GetField("y", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            int w = (int)c.GetType().GetField("w", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);
            int h = (int)c.GetType().GetField("h", BindingFlags.Instance | BindingFlags.NonPublic).GetValue(c);


            CEReflection.DrawNameMethod.Invoke(__instance, new object[] { x + 20, y + 30, w, 30 });
            CEReflection.DrawDescMethod.Invoke(__instance, new object[] { x + 20, y + 60, w, 30 });
            CEReflection.DrawBackstoryMethod.Invoke(__instance, new object[] { x + 20, y + 100 });
            CEReflection.DrawIncabableOfMethod.Invoke(__instance, new object[] { x + 20, y + 205 });
            CEReflection.DrawTraitsMethod.Invoke(__instance, new object[] { x + 20, y + 305, h });

            Rect viewRect = new Rect(x + 330, y + 100, 260f, 350f);
            Rect innerRect = new Rect(0f, 0f, viewRect.width - 16f, (float)skillCount * 30);
            Widgets.BeginScrollView(viewRect, ref scrollPos, innerRect);
            GUI.BeginGroup(innerRect);
            CEReflection.DrawSkillsMethod.Invoke(__instance, new object[] { 0, 0 });
            GUI.EndGroup();
            Widgets.EndScrollView();

            CEReflection.DrawAbilitiesMethod.Invoke(__instance, new object[] { x + 330, y + 500 });
            CEReflection.DrawPsycastsMethod.Invoke(__instance, new object[] { x + 590, y + 380 });
            CEReflection.DrawTrainingMethod.Invoke(__instance, new object[] { x + 20, y + 100, w, h - 100 });
            CEReflection.DrawFactionMethod.Invoke(__instance, new object[] { x + 600, y + 30, 250 });
            CEReflection.DrawIdeoMethod.Invoke(__instance, new object[] { x + 600, y + 60, 250 });
            CEReflection.DrawXenoMethod.Invoke(__instance, new object[] { x + 600, y + 90, 250 });
            int oY = 0;
            CEReflection.DrawFavoriteMethod.Invoke(__instance, new object[] { x + 600, y + 120 + oY, 250 });
            CEReflection.DrawMutantMethod.Invoke(__instance, new object[] { x + 600, y + 120 + oY, 250 });
            CEReflection.DrawTitleMethod.Invoke(__instance, new object[] { x + 600, y + 120 + oY, 250 });
            CEReflection.DrawExtendablesMethod.Invoke(__instance, new object[] { x + 600, y + 120 + oY, 30 });
            CEReflection.DrawCapsuleMethod.Invoke(__instance, new object[] { x + 590, y + 220, 200, 144 });
            CEReflection.DrawLowerButtonsMethod.Invoke(__instance, new object[] { x, y, w, h });

            return false;
        }
    }
}
