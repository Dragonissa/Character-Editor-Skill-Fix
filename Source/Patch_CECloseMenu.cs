using System.Reflection;

namespace CharacterEditorSkillFix
{
    public static class Patch_EditorUI_Close
    {
        public static MethodBase TargetMethod() => CEReflection.Close;
        public static void Postfix()
        {
            ExpertiseCache.Valid = false;
            ExpertiseUI.ExpertiseTabState.Active = false;
        }
    }
}
