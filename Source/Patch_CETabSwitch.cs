using System.Reflection;

namespace CharacterEditorSkillFix
{
    public static class CE_TabSwitch
    {
        public static MethodBase TargetMethod() => CEReflection.ATabwechsel;

        public static void Postfix()
        {
            ExpertiseUI.ExpertiseTabState.Active = false;
        }
    }
}