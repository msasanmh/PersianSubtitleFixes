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
    public class FixTiming
    {
        public FixTiming() { }

        public static void FixIncorrectTimeOrder()
        {
            FormMain.SubCurrent.Sort(Nikse.SubtitleEdit.Core.Enums.SubtitleSortCriteria.StartTime);
            FormMain.SubCurrent.Renumber();
        }

        public static void AdjustDurations(double charsPerSec = 6.5)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                string cleanText = Tools.HTML.RemoveHtmlTags(p.Text);
                int countChars = cleanText.Length;
                double charPerSec = 1 / (double)charsPerSec;
                double timeMs = (charPerSec * countChars) * 1000;
                double endTime = p.StartTime.TotalMilliseconds + timeMs;
                p.EndTime.TotalMilliseconds = endTime;
            }
        }


        public static void FixMinimumDurationLimit(double milliseconds = 1000)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                // If Duration is Not Negative
                if (p.EndTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                {
                    int pLastNumber = FormMain.SubCurrent.Paragraphs.Count - 1;
                    bool nextParagraph = pn != pLastNumber;

                    if (p.Duration.TotalMilliseconds < (double)milliseconds)
                    {
                        double addMilliseconds = (double)milliseconds - p.Duration.TotalMilliseconds;
                        TimeCode endTime = new();
                        endTime.TotalMilliseconds = p.EndTime.TotalMilliseconds + addMilliseconds;

                        p.EndTime = endTime;

                        if (nextParagraph)
                        {
                            Paragraph pNext = FormMain.SubCurrent.Paragraphs[pn + 1];
                            if (p.EndTime.TotalSeconds > pNext.StartTime.TotalSeconds)
                            {
                                endTime.TotalMilliseconds = pNext.StartTime.TotalMilliseconds;
                                p.EndTime = endTime;
                            }
                        }
                    }
                }
            }
        }

        public static void FixMaximumDurationLimit(double milliseconds = 8000)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                // If Duration is Not Negative
                if (p.EndTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                {
                    if (p.Duration.TotalMilliseconds > (double)milliseconds)
                    {
                        double removeMilliseconds = p.Duration.TotalMilliseconds - (double)milliseconds;
                        TimeCode endTime = new();
                        endTime.TotalMilliseconds = p.EndTime.TotalMilliseconds - removeMilliseconds;

                        p.EndTime = endTime;
                    }
                }
            }
        }

        public static void ApplyMinimumGap(double milliseconds = 42)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                // If Duration is Not Negative
                if (p.EndTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                {
                    int pLastNumber = FormMain.SubCurrent.Paragraphs.Count - 1;
                    bool nextParagraph = pn != pLastNumber;

                    if (nextParagraph)
                    {
                        Paragraph pNext = FormMain.SubCurrent.Paragraphs[pn + 1];
                        if (p.StartTime.TotalMilliseconds < pNext.StartTime.TotalMilliseconds)
                        {
                            if (p.EndTime.TotalMilliseconds + (double)milliseconds > pNext.StartTime.TotalMilliseconds)
                            {
                                double removeMilliseconds = (p.EndTime.TotalMilliseconds + (double)milliseconds) - pNext.StartTime.TotalMilliseconds;
                                TimeCode endTime = new();
                                endTime.TotalMilliseconds = p.EndTime.TotalMilliseconds - removeMilliseconds;
                                // Don't Make It Negative
                                if (endTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                                    p.EndTime = endTime;
                            }
                        }
                    }
                }
            }
        }

        public static void MergeLinesWithSameTimeCode(double difference = 250)
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
                        if (p.StartTime.TotalMilliseconds + difference >= pNext.StartTime.TotalMilliseconds)
                        {
                            renumber = true;
                            double startTime = p.StartTime.TotalMilliseconds;
                            double endTime = pNext.EndTime.TotalMilliseconds;
                            string text1 = p.Text;
                            string text2 = pNext.Text;
                            FormMain.SubCurrent.Paragraphs.Remove(pNext);
                            p.StartTime.TotalMilliseconds = startTime;
                            p.EndTime.TotalMilliseconds = endTime;
                            if (text1 != text2)
                            {
                                if (text1.Contains(Environment.NewLine))
                                    text1 = text1.Replace(Environment.NewLine, " ");
                                if (text2.Contains(Environment.NewLine))
                                    text2 = text2.Replace(Environment.NewLine, " ");
                                string text = text1 + Environment.NewLine + text2;
                                p.Text = text;
                            }
                            else
                                p.Text = text1;
                        }
                    }
                }
            }

            if (renumber)
                FormMain.SubCurrent.Renumber();
        }

        public static void FixNegative()
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];
                // If Duration is Negative
                if (p.StartTime.TotalMilliseconds > p.EndTime.TotalMilliseconds)
                {
                    var startTime = p.StartTime;
                    var endTime = p.EndTime;

                    p.StartTime = endTime;
                    p.EndTime = startTime;
                }
            }
        }

        public static void ShowEarlier(double milliseconds)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];

                p.StartTime.TotalMilliseconds -= milliseconds;
                p.EndTime.TotalMilliseconds -= milliseconds;
            }
        }

        public static void ShowLater(double milliseconds)
        {
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];

                p.StartTime.TotalMilliseconds += milliseconds;
                p.EndTime.TotalMilliseconds += milliseconds;
            }
        }

        public static void ChangeFrameRate(double fromFrameRate, double toFrameRate)
        {
            FormMain.SubCurrent.ChangeFrameRate(fromFrameRate, toFrameRate);
        }

        public static void ChangeSpeed(double percentage)
        {
            double adjust = percentage / 100.0;
            if (adjust == 1) return;
            for (int pn = 0; pn < FormMain.SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = FormMain.SubCurrent.Paragraphs[pn];

                p.StartTime.TotalMilliseconds *= adjust;
                p.EndTime.TotalMilliseconds *= adjust;
            }
        }


    }
}
