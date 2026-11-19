using System.Reflection;
using UnityEngine;
using Verse;


namespace CharacterEditorSkillFix
{

    public static class CE_AddExpertiseTabButton
    {
        public static MethodBase TargetMethod() => CEReflection.DrawTabsMethod;

        private static Rect cachedRect;
        private static bool rectValid;
        private static object lastInstance;

        public static void Postfix(object __instance, int x, int y, int w, int h)
        {
            var currentTab = CEReflection.CurrentTabField.GetValue(__instance);

            if (!Equals(currentTab, CEReflection.Sentinel))
            {
                ExpertiseUI.ExpertiseTabState.Active = false;
            }

            if (Equals(currentTab, CEReflection.NoneTab))
                return;

            if (!rectValid || __instance != lastInstance)
            {
                lastInstance = __instance;

                int tabWidth = w / 9;
                int buttonX = x + tabWidth;

                cachedRect = new Rect(buttonX, y, tabWidth, h);
                rectValid = true;
            }

            GUI.color = ExpertiseUI.ExpertiseTabState.Active ? Color.yellow : Color.white;

            if (Widgets.ButtonText(cachedRect, "Expertise"))
            {
                ExpertiseUI.ExpertiseTabState.Active = true;
                CEReflection.CurrentTabField.SetValue(__instance, CEReflection.Sentinel);
            }

            GUI.color = Color.white;
        }
    }
}