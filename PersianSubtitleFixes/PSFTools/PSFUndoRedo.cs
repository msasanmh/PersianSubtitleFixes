using MsmhTools;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using PersianSubtitleFixes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFTools
{
    public static class PSFUndoRedo
    {
        public static bool Undo { get; private set; }
        public static bool Redo { get; private set; }
        public static List<Tuple<Subtitle, string, SubtitleFormat, string>> UndoRedoList { get; set; } = new List<Tuple<Subtitle, string, SubtitleFormat, string>>();
        public static int CurrentIndex { get; set; } = 0;

        public static void UndoRedo(Subtitle subCurrent, string? subEncodingDisplayName, SubtitleFormat? subtitleFormat, string message)
        {
            if (string.IsNullOrWhiteSpace(subEncodingDisplayName) || subtitleFormat == null)
                return;

            int MaxIndex = UndoRedoList.Count - 1;
            if (MaxIndex > CurrentIndex)
                UndoRedoList.RemoveRange(CurrentIndex + 1, MaxIndex - CurrentIndex);
            UndoRedoList.Add(new Tuple<Subtitle, string, SubtitleFormat, string>(subCurrent, subEncodingDisplayName, subtitleFormat, message));
            int LC = UndoRedoList.Count;
            CurrentIndex = LC - 1;
            UpdateIndex(CurrentIndex);

            // Remove state if current state is equal to previous one.
            if (CurrentIndex > 0)
            {
                Subtitle previousSubCurrent = UndoRedoList[CurrentIndex - 1].Item1;
                string previousSubEncodingDisplayName = UndoRedoList[CurrentIndex - 1].Item2;
                SubtitleFormat previousSubtitleFormat = UndoRedoList[CurrentIndex - 1].Item3;
                if (subCurrent.ToText(FormMain.SubFormat).Equals(previousSubCurrent.ToText(FormMain.SubFormat))
                    && subEncodingDisplayName.Equals(previousSubEncodingDisplayName)
                    && subtitleFormat.Equals(previousSubtitleFormat))
                {
                    UndoRedoList.RemoveRange(CurrentIndex - 1, 1);
                    CurrentIndex = UndoRedoList.Count - 1;
                    UpdateIndex(CurrentIndex);
                }
            }

            Debug.WriteLine("Current UndoRedo Index: " + CurrentIndex);
        }

        public static Tuple<Subtitle, string, SubtitleFormat, string> UndoRedo(int currentIndex)
        {
            UpdateIndex(currentIndex);
            return UndoRedoList[currentIndex];
        }

        private static void UpdateIndex(int currentIndex)
        {
            int LC = UndoRedoList.Count;
            if (LC == 0)
            {
                Undo = false;
                Redo = false;
            }
            else
            {
                if (currentIndex == 0)
                    Undo = false;
                else if (0 < currentIndex)
                {
                    Undo = true;
                }
                if (currentIndex < LC - 1)
                {
                    Redo = true;
                }
                else if (currentIndex == LC - 1)
                    Redo = false;
            }
        }
    }
}
