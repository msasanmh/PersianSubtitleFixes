using CustomControls;
using PersianSubtitleFixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PSFTools
{
    public static class Guide
    {
        public static void Help(Control c, PictureBox pictureBox, ToolStripMenuItem viewGuide)
        {
            var box = c as CustomCheckBox;

            box.MouseHover -= Box_MouseHover;
            box.MouseHover += Box_MouseHover;

            box.MouseLeave -= Box_MouseLeave;
            box.MouseLeave += Box_MouseLeave;

            void Box_MouseHover(object? sender, EventArgs e)
            {
                if (!viewGuide.Checked)
                {
                    HidePictureBox(pictureBox);
                    return;
                }

                pictureBox.BackColor = Color.LightGray;
                //pictureBox.Location = new(box.Location.X + 0, box.Location.Y + 20);
                pictureBox.Visible = true;
                pictureBox.BringToFront();

                if (box.Tag.Equals("Fix Unicode Control Char"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.FixUnicodeControlChar;
                else if (box.Tag.Equals("Change Arabic Chars to Persian"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.ChangeArabicCharsToPersian;
                else if (box.Tag.Equals("Remove Unneeded Spaces"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.RemoveUnneededSpaces;
                else if (box.Tag.Equals("Add Missing Spaces"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.AddMissingSpaces;
                else if (box.Tag.Equals("Fix Dialog Hyphen"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.FixDialogHyphen;
                else if (box.Tag.Equals("Fix Wrong Chars"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.FixWrongChars;
                else if (box.Tag.Equals("Fix Misplaced Chars"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.FixMisplacedChars;
                else if (box.Tag.Equals("Fix Abbreviations"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.FixAbbreviations;
                else if (box.Tag.Equals("Space to Invisible Space"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.SpaceToInvisibleSpace;
                else if (box.Tag.Equals("OCR"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.OCR;
                else if (box.Tag.Equals("Remove Leading Dots"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.RemoveLeadingDots;
                else if (box.Tag.Equals("Remove Dot from the End of Line"))
                    pictureBox.Image = global::PersianSubtitleFixes.Guide.ResourceGuide.RemoveDotFromTheEndOfLine;
                else
                {
                    HidePictureBox(pictureBox);
                }


            }

            void Box_MouseLeave(object? sender, EventArgs e)
            {
                HidePictureBox(pictureBox);
            }

            void HidePictureBox(PictureBox pictureBox)
            {
                pictureBox.Image = null;
                pictureBox.Visible = false;
                pictureBox.SendToBack();
            }
        }


    }
}
