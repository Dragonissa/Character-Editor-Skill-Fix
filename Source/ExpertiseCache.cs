using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using Verse;


namespace CharacterEditorSkillFix
{
    internal static class ExpertiseCache
    {
        public static bool Valid;

        public static Pawn Pawn;
        public static object Tracker;

        public static List<object> Records;
        public static List<string> Labels;
        public static List<string> LevelStrings;
        public static List<string> Tooltips;
        public static List<Rect> LabelRects;
        public static List<Rect> LevelRects;
        public static List<Rect> MinusRects;
        public static List<Rect> PlusRects;
        public static List<Rect> RowRects;
        
        public static List<Type> RecordTypes;
        public static List<PropertyInfo> LevelProps;
    }
}
