using MsmhTools;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersianSubtitleFixes.msmh
{
    public static class PSFUndoRedo
    {
        public static bool Undo { get; set; }
        public static bool Redo { get; set; }
        public static List<Tuple<Subtitle, string, SubtitleFormat>> UndoRedoList { get; set; } = new List<Tuple<Subtitle, string, SubtitleFormat>>();
        public static int CurrentIndex = 0;
        public static void UndoRedo(Subtitle subCurrent, string subEncodingDisplayName, SubtitleFormat subtitleFormat)
        {
            if (string.IsNullOrWhiteSpace(subEncodingDisplayName) || subtitleFormat == null)
                return;
            //for (int ci = CurrentIndex; ci < UndoRedoList.Count; ci++)
            int MaxIndex = UndoRedoList.Count - 1;
            if (MaxIndex > CurrentIndex)
                UndoRedoList.RemoveRange(CurrentIndex + 1, MaxIndex - CurrentIndex);
            UndoRedoList.Add(new Tuple<Subtitle, string, SubtitleFormat>(subCurrent, subEncodingDisplayName, subtitleFormat));
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
        }
        public static Tuple<Subtitle, string, SubtitleFormat> UndoRedo(int currentIndex)
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
