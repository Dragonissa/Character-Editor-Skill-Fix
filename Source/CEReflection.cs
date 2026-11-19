using HarmonyLib;
using System;
using System.Reflection;
using Verse;

namespace CharacterEditorSkillFix
{
    public static class CEReflection
    {
        public static bool Initialized;
        public static Type EditorUIType;
        public static Type EditorType;
        public static Type BlockBioType;
        public static Type SkillToolType;
        public static FieldInfo CurrentTabField;
        public static PropertyInfo APIProp;
        public static PropertyInfo PawnProp;
        public static object Sentinel;
        public static object NoneTab;
        public static Type ExpertiseTrackerType;
        public static Type ExpertiseRecordType;
        public static MethodInfo DrawTabsMethod;
        public static MethodInfo DrawSelectedTabMethod;
        public static MethodInfo ATabwechsel;
        public static MethodInfo Close;
        public static MethodInfo DrawMethod;
        public static MethodInfo DrawNameMethod;
        public static MethodInfo DrawDescMethod;
        public static MethodInfo DrawBackstoryMethod;
        public static MethodInfo DrawIncabableOfMethod;
        public static MethodInfo DrawTraitsMethod;
        public static MethodInfo DrawSkillsMethod;
        public static MethodInfo DrawAbilitiesMethod;
        public static MethodInfo DrawPsycastsMethod;
        public static MethodInfo DrawTrainingMethod;
        public static MethodInfo DrawFactionMethod;
        public static MethodInfo DrawIdeoMethod;
        public static MethodInfo DrawXenoMethod;
        public static MethodInfo DrawFavoriteMethod;
        public static MethodInfo DrawMutantMethod;
        public static MethodInfo DrawTitleMethod;
        public static MethodInfo DrawExtendablesMethod;
        public static MethodInfo DrawCapsuleMethod;
        public static MethodInfo DrawLowerButtonsMethod;
        public static MethodInfo GetSkillIndexMethod;

        public static void Initialize()
        {
            if (Initialized) return;

            EditorUIType = AccessTools.TypeByName("CharacterEditor.CEditor+EditorUI");
            if (EditorUIType == null)
            {
                Log.Error("[CharacterEditorSkillFix] Could not find CE EditorUI type.");
                return;
            }

            EditorType = AccessTools.TypeByName("CharacterEditor.CEditor");
            if (EditorType == null)
            {
                Log.Error("[CharacterEditorSkillFix] Could not find CE Editor type.");
                return;
            }

            BlockBioType = AccessTools.TypeByName("CharacterEditor.CEditor+EditorUI+BlockBio");
            if (BlockBioType == null)
            {
                Log.Error("[CharacterEditorSkillFix] Could not find CE BlockBio type.");
                return;
            }

            SkillToolType = AccessTools.TypeByName("CharacterEditor.SkillTool");
            if (SkillToolType == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find SkillTool type.");
                return;
            }

            DrawTabsMethod = AccessTools.Method(EditorUIType, "DrawTabs");
            if (DrawTabsMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawTabs on CE EditorUI. CE may have updated.");
                return;
            }

            DrawSelectedTabMethod = AccessTools.Method(EditorUIType, "DrawSelectedTab");
            if (DrawSelectedTabMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawSelectedTab on CE EditorUI. CE may have updated.");
                return;
            }

            ATabwechsel = AccessTools.Method(EditorUIType, "ATabwechsel");
            if (ATabwechsel == null)            {
                Log.Warning("[CharacterEditorSkillFix] Could not find ATabwechsel on CE EditorUI. CE may have updated.");
                return;
            }

            Close = AccessTools.Method(EditorUIType, "Close");
            if (Close == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find Close on CE EditorUI. CE may have updated.");
                return;
            }

            DrawMethod = AccessTools.Method(BlockBioType, "Draw");
            if (DrawMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find Draw on CE BlockBio. CE may have updated.");
                return;
            }

            DrawNameMethod = AccessTools.Method(BlockBioType, "DrawName");
            if (DrawNameMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawName on CE BlockBio. CE may have updated.");
                return;
            }

            DrawDescMethod = AccessTools.Method(BlockBioType, "DrawDesc");
            if (DrawDescMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawDesc on CE BlockBio. CE may have updated.");
                return;
            }

            DrawBackstoryMethod = AccessTools.Method(BlockBioType, "DrawBackstory");
            if (DrawBackstoryMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawBackstory on CE BlockBio. CE may have updated.");
                return;
            }

            DrawIncabableOfMethod = AccessTools.Method(BlockBioType, "DrawIncapableOf");
            if (DrawIncabableOfMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawIncapableOf on CE BlockBio. CE may have updated.");
                return;
            }

            DrawTraitsMethod = AccessTools.Method(BlockBioType, "DrawTraits");
            if (DrawTraitsMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawTraits on CE BlockBio. CE may have updated.");
                return;
            }

            DrawSkillsMethod = AccessTools.Method(BlockBioType, "DrawSkills");
            if (DrawSkillsMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawSkills on CE BlockBio. CE may have updated.");
                return;
            }

            DrawAbilitiesMethod = AccessTools.Method(BlockBioType, "DrawAbilities");
            if (DrawAbilitiesMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawAbilities on CE BlockBio. CE may have updated.");
                return;
            }

            DrawPsycastsMethod = AccessTools.Method(BlockBioType, "DrawPsycasts");
            if (DrawPsycastsMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawPsycasts on CE BlockBio. CE may have updated.");
                return;
            }

            DrawTrainingMethod = AccessTools.Method(BlockBioType, "DrawTraining");
            if (DrawTrainingMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawTraining on CE BlockBio. CE may have updated.");
                return;
            }

            DrawFactionMethod = AccessTools.Method(BlockBioType, "DrawFaction");
            if (DrawFactionMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawFaction on CE BlockBio. CE may have updated.");
                return;
            }

            DrawIdeoMethod = AccessTools.Method(BlockBioType, "DrawIdeo");
            if (DrawIdeoMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawIdeo on CE BlockBio. CE may have updated.");
                return;
            }

            DrawXenoMethod = AccessTools.Method(BlockBioType, "DrawXeno");
            if (DrawXenoMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawXeno on CE BlockBio. CE may have updated.");
                return;
            }

            DrawFavoriteMethod = AccessTools.Method(BlockBioType, "DrawFavorite");
            if (DrawFavoriteMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawFavorite on CE BlockBio. CE may have updated.");
                return;
            }

            DrawMutantMethod = AccessTools.Method(BlockBioType, "DrawMutant");
            if (DrawMutantMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawMutant on CE BlockBio. CE may have updated.");
                return;
            }

            DrawTitleMethod = AccessTools.Method(BlockBioType, "DrawTitle");
            if (DrawTitleMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawTitle on CE BlockBio. CE may have updated.");
                return;
            }

            DrawExtendablesMethod = AccessTools.Method(BlockBioType, "DrawExtendables");
            if (DrawExtendablesMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawExtendables on CE BlockBio. CE may have updated.");
                return;
            }

            DrawCapsuleMethod = AccessTools.Method(BlockBioType, "DrawCapsule");
            if (DrawCapsuleMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawCapsule on CE BlockBio. CE may have updated.");
                return;
            }

            DrawLowerButtonsMethod = AccessTools.Method(BlockBioType, "DrawLowerButtons");
            if (DrawLowerButtonsMethod == null) {
                Log.Warning("[CharacterEditorSkillFix] Could not find DrawLowerButtons on CE BlockBio. CE may have updated.");
                return;
            }

            GetSkillIndexMethod = AccessTools.Method(SkillToolType, "GetSkillIndex");
            if (GetSkillIndexMethod == null)
            {
                Log.Warning("[CharacterEditorSkillFix] Could not find GetSkillIndex on CE SkillTool. CE may have updated.");
                return;
            }

            CurrentTabField = AccessTools.Field(EditorUIType, "currentTab");
            APIProp = AccessTools.Property(EditorType, "API");
            PawnProp = AccessTools.Property(EditorType, "Pawn");
            Type TabEnum = EditorUIType.GetNestedType("TabType", BindingFlags.NonPublic);
            NoneTab = Enum.ToObject(TabEnum, 10);
            Sentinel = Enum.ToObject(TabEnum, -1);

            if (CurrentTabField == null || APIProp == null || PawnProp == null || Sentinel == null || NoneTab == null)
            {
                Log.Error("[CharacterEditorSkillFix] CEReflection failed to bind one or more fields.");
                Log.Error($"CurrentTabField: {(CurrentTabField == null ? "null" : "found")}");
                Log.Error($"APIProp: {(APIProp == null ? "null" : "found")}");
                Log.Error($"PawnProp: {(PawnProp == null ? "null" : "found")}");
                Log.Error($"Sentinel: {(Sentinel == null ? "null" : "found")}");
                Log.Error($"NoneTab: {(NoneTab == null ? "null" : "found")}");
               
                return;
            }

            Initialized = true;
            Log.Message("[CharacterEditorSkillFix] CEReflection initialized successfully.");
        }
    }
}