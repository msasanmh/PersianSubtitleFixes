using MsmhTools;
using Nikse.SubtitleEdit.Core.Common;
using PersianSubtitleFixes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace PSFTools
{
    public class FixOthers
    {
        public FixOthers() { }

        public static void RemoveEmptylines()
        {
            bool renumber = false;
            for (int pn = FormMain.SubCurrent.Paragraphs.Count - 1; pn >= 0; pn--)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];

                if (p.Text.IsEmpty())
                {
                    renumber = true;
                    FormMain.SubCurrent.Paragraphs.Remove(p);
                }
            }

            if (renumber)
                FormMain.SubCurrent.Renumber();
        }

        public static void MergeLinesWithSameText(double difference = 250)
        {
            bool renumber = false;
            for (int pn = FormMain.SubCurrent.Paragraphs.Count - 1; pn >= 0; pn--)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];

                int pLastNumber = FormMain.SubCurrent.Paragraphs.Count - 1;
                bool nextParagraph = pn != pLastNumber;

                if (nextParagraph)
                {
                    Paragraph pNext = FormMain.SubCurrent.Paragraphs[pn + 1];
                    if (p.StartTime.TotalMilliseconds <= pNext.StartTime.TotalMilliseconds)
                    {
                        if (p.EndTime.TotalMilliseconds + difference >= pNext.StartTime.TotalMilliseconds)
                        {
                            if (!p.Text.IsEmpty() && !pNext.Text.IsEmpty())
                            {
                                double startTime = p.StartTime.TotalMilliseconds;
                                double endTime = pNext.EndTime.TotalMilliseconds;
                                string text = p.Text;
                                string text1 = Tools.HTML.RemoveHtmlTags(p.Text);
                                text1 = text1.IsNotNull().RemoveControlChars().Trim();
                                string text2 = Tools.HTML.RemoveHtmlTags(pNext.Text);
                                text2 = text2.IsNotNull().RemoveControlChars().Trim();
                                if (text1 == text2)
                                {
                                    renumber = true;
                                    FormMain.SubCurrent.Paragraphs.Remove(pNext);
                                    p.StartTime.TotalMilliseconds = startTime;
                                    p.EndTime.TotalMilliseconds = endTime;
                                    p.Text = text;
                                }
                            }
                        }
                    }
                }
            }

            if (renumber)
                FormMain.SubCurrent.Renumber();
        }

        public static void RemoveUnicodeControlChars()
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                p.Text = p.Text.RemoveUnicodeControlChars();
            }
        }


    }
}
