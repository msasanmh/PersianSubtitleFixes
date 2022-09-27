using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.IsolatedStorage;
using MsmhTools;
using System.Xml;
using PersianSubtitleFixes;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using Nikse.SubtitleEdit.Core.Common;
using System.Text.RegularExpressions;
using System.Data;
using CustomControls;
using System.Resources;
using System.Diagnostics;
using System.Xml.Linq;

namespace PSFTools
{
    public static class PSF
    {
        public static readonly string ResourcePath = "PersianSubtitleFixes.ReplaceList.multiple_replace.xml";
        public static readonly string ReplaceListPath = Path.Combine(Tools.Info.CurrentPath, "ReplaceList", "multiple_replace.xml");
        public static readonly string LineSeparator = "<br />";
        public static readonly string Group_FixUnicodeControlChar = "Fix Unicode Control Char";
        public static readonly string Group_ChangeArabicCharsToPersian = "Change Arabic Chars to Persian";
        private static readonly string exception1 = "Options";
        private static readonly string exception2 = "Formal to Slang";
        private static readonly string exception3 = "Guide";
        private static readonly string delimiter = "|";
        public static readonly string suffixN = "_Normal";
        public static readonly string suffixR = "_RegularExpression";
        public static readonly Lazy<string> SubtitleExtensionFilter = new(FileDlgFilter);
        
        //=======================================================================================
        public static bool IsSettingsValid(string filePath)
        {
            if (File.Exists(filePath))
            {
                bool isXmlValid = Tools.Xml.IsValidXML(File.ReadAllText(filePath));
                if (isXmlValid == true)
                {
                    Console.WriteLine("Settings File Is Valid.");
                    return true;
                }
                else
                {
                    Console.WriteLine("Settings File Is Not Valid.");
                    return false;
                }
            }
            else
            {
                Console.WriteLine("Settings File Not Exist.");
                return false;
            }
        }
        //=======================================================================================
        public static class SubFormat
        {
            public static List<Tuple<SubtitleFormat, string, string, string>> Formats()
            {
                int count = 0;
                List<Tuple<SubtitleFormat, string, string, string>> subFormats = new();
                // Get Text Based Formats
                //foreach (var format in SubtitleFormat.AllSubtitleFormats.Where(format => !format.IsVobSubIndexFile))
                foreach (var format in SubtitleFormat.AllSubtitleFormats.Where(format => !format.IsVobSubIndexFile))
                {
                    //Item1 = Format, Item2 = Format Name, Item3 = Format Friendly Name, Item4 = Format Extension
                    if (format.IsTextBased == true)
                    {
                        SubtitleFormat Format = format;
                        string FName = format.Name;
                        string FFriendlyName = format.FriendlyName;
                        string FExt = format.Extension;
                        subFormats.Add(new Tuple<SubtitleFormat, string, string, string>(Format, FName, FFriendlyName, FExt));
                        count++;
                    }
                }
                //Console.WriteLine("Available Formats: " + count);
                return subFormats;
            }
            public static List<SubtitleFormat> GetFormat()
            {
                List<SubtitleFormat> formats = new();
                for (int n = 0; n < Formats().Count; n++)
                {
                    SubtitleFormat format = Formats()[n].Item1;
                    formats.Add(format);
                }
                return formats;
            }
            public static SubtitleFormat? GetFormat(string friendlyName)
            {
                for (int n = 0; n < Formats().Count; n++)
                {
                    if (friendlyName == Formats()[n].Item3)
                        return Formats()[n].Item1;
                }
                return null;
            }
            public static List<string> GetName()
            {
                List<string> names = new();
                for (int n = 0; n < Formats().Count; n++)
                {
                    string name = Formats()[n].Item2;
                    names.Add(name);
                }
                return names;
            }
            public static string? GetName(string friendlyName)
            {
                for (int n = 0; n < Formats().Count; n++)
                {
                    if (friendlyName == Formats()[n].Item3)
                        return Formats()[n].Item2;
                }
                return null;
            }
            public static List<string> GetFriendlyName()
            {
                List<string> friendlyNames = new();
                for (int n = 0; n < Formats().Count; n++)
                {
                    string friendlyName = Formats()[n].Item3;
                    friendlyNames.Add(friendlyName);
                }
                return friendlyNames;
            }
            public static string? GetFriendlyName(string extension)
            {
                for (int n = 0; n < Formats().Count; n++)
                {
                    if (extension == Formats()[n].Item4)
                        return Formats()[n].Item3;
                }
                return null;
            }
            public static List<string> GetAllExtensions()
            {
                List<string> extensions = new();
                foreach (var s in SubtitleFormat.AllSubtitleFormats.Concat(SubtitleFormat.GetTextOtherFormats()))
                {
                    if (!extensions.Contains(s.Extension))
                    {
                        extensions.Add(s.Extension);
                        foreach (var ext in s.AlternateExtensions)
                        {
                            if (!string.IsNullOrEmpty(ext))
                                if (!extensions.Contains(ext))
                                    extensions.Add(ext);
                        }
                    }
                }
                return extensions;
            }
            public static List<string> GetExtension()
            {
                List<string> extensions = new();
                for (int n = 0; n < Formats().Count; n++)
                {
                    string ext = Formats()[n].Item4;
                    if (!extensions.Contains(ext))
                        extensions.Add(ext);
                }
                return extensions;
            } // Supported Extensions Without Duplicates
        }
        //=======================================================================================
        public static void InitializeSubtitleFormat(CustomToolStripComboBox comboBox, SubtitleFormat format)
        {
            InitializeSubtitleFormat(comboBox.ComboBox, format);
        }
        public static void InitializeSubtitleFormat(ToolStripComboBox comboBox, SubtitleFormat format)
        {
            InitializeSubtitleFormat(comboBox.ComboBox, format);
        }
        public static void InitializeSubtitleFormat(ComboBox comboBox, SubtitleFormat format)
        {
            var friendlyNames = SubFormat.GetFriendlyName();
            var selectedIndex = 0;
            using (var graphics = comboBox.CreateGraphics())
            {
                var maxWidth = (float)comboBox.DropDownWidth;
                int max = friendlyNames.Count;
                for (var index = 0; index < max; index++)
                {
                    var name = friendlyNames[index];
                    if (name.Equals(format.FriendlyName, StringComparison.OrdinalIgnoreCase))
                    {
                        // Use Index Only Formats List Is Not Sorted.
                        selectedIndex = index;
                    }
                    if (name.Length > 30)
                    {
                        var width = graphics.MeasureString(name, comboBox.Font).Width;
                        if (width > maxWidth)
                        {
                            maxWidth = width;
                        }
                    }
                }
                comboBox.DropDownWidth = (int)Math.Round(maxWidth + 7.5);
            }
            comboBox.BeginUpdate();
            comboBox.Items.Clear();
            comboBox.Items.AddRange(friendlyNames.ToArray<object>());
            comboBox.SelectedIndex = selectedIndex;
            comboBox.EndUpdate();
        }
        //=======================================================================================
        public static void UpdateComboBoxFormat(SubtitleFormat format, CustomToolStripComboBox comboBox)
        {
            UpdateComboBoxFormat(format, comboBox.ComboBox);
        }
        public static void UpdateComboBoxFormat(SubtitleFormat format, ToolStripComboBox comboBox)
        {
            UpdateComboBoxFormat(format, comboBox.ComboBox);
        }
        public static void UpdateComboBoxFormat(SubtitleFormat format, ComboBox comboBox)
        {
            comboBox.InvokeIt(() =>
            {
                var friendlyNames = SubFormat.GetFriendlyName();
                for (int n = 0; n < friendlyNames.Count; n++)
                {
                    if (friendlyNames[n] == format.FriendlyName)
                    {
                        comboBox.SelectedItem = friendlyNames[n];
                        return;
                    }
                }
                Console.WriteLine("Subtitle Format Is Not Supported.");
            });
        }
        //=======================================================================================
        public static List<string>? ListGroupNames()
        {
            //string fileContent = Tools.Resource.GetResourceTextFile(ResourcePath);
            //string fileContent = File.ReadAllText(ReplaceListPath);
            List<string> listGN = new();
            XDocument replaceList = new();
            replaceList = XDocument.Load(ReplaceListPath, LoadOptions.None);
            var nodesGroups = replaceList.Root.Elements().Elements();

            if (!nodesGroups.Any())
                return null;
            else
            {
                for (int a = 0; a < nodesGroups.Count(); a++)
                {
                    var node = nodesGroups.ToList()[a];
                    bool groupEnabled = node.Element("Enabled") == null || Convert.ToBoolean(node.Element("Enabled").Value);
                    if (groupEnabled)
                        listGN.Add(node.Element("Name").Value);
                }
            }

            return listGN;
        }
        //=======================================================================================
        public static string FileDlgFilter()
        {
            StringBuilder sb = new();
            sb.Append("Subtitles|");
            for (int n = 0; n < SubFormat.GetAllExtensions().Count; n++)
            {
                var ext = SubFormat.GetAllExtensions()[n];
                if (!sb.ToString().Contains("*" + ext + ";", StringComparison.OrdinalIgnoreCase))
                {
                    sb.Append('*');
                    sb.Append(ext.TrimStart('*'));
                    sb.Append(';');
                }
            }
            //sb.Append('|');
            //sb.Append("All Files");
            //sb.Append("|*.*");
            return sb.ToString();
        }
        //=======================================================================================
        public static OpenFileDialog OpenFileDlg
        {
            get
            {
                using OpenFileDialog ofd = new();
                //ofd.Filter = "Subtitles|*.srt;*.sup";
                ofd.Filter = SubtitleExtensionFilter.Value;
                ofd.RestoreDirectory = true;
                return ofd;
            }
            // Using: var FileDialog1 = PSF.OpenFileDlg;
        }
        //=======================================================================================
        public static SaveFileDialog SaveFileDlg
        {
            get
            {
                using SaveFileDialog sfd = new();
                sfd.Filter = "Subtitles|*" + FormMain.SubFormat.Extension;
                sfd.DefaultExt = ".srt";
                sfd.AddExtension = true;
                sfd.RestoreDirectory = true;
                return sfd;
            }
            // Using: var FileDialog1 = PSF.SaveFileDlg;
        }
        //=======================================================================================
        public static void LoadColumnsLoadState(CustomDataGridView dgv)
        {
            dgv.InvokeIt(() =>
            {
                // Remove All Columns
                dgv.Columns.Clear();

                // Add Line# Column
                DataGridViewTextBoxColumn LineColumn = new();
                LineColumn.HeaderText = "Line#";
                LineColumn.ReadOnly = true;
                LineColumn.Resizable = DataGridViewTriState.False;
                LineColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                LineColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                LineColumn.CellTemplate = new DataGridViewTextBoxCell();
                LineColumn.ValueType = typeof(string);
                dgv.Columns.Add(LineColumn);

                // Add StartTime Column
                DataGridViewTextBoxColumn StartTimeColumn = new();
                StartTimeColumn.HeaderText = "Start time";
                StartTimeColumn.ReadOnly = true;
                StartTimeColumn.Resizable = DataGridViewTriState.False;
                StartTimeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                StartTimeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                StartTimeColumn.CellTemplate = new DataGridViewTextBoxCell();
                StartTimeColumn.ValueType = typeof(string);
                dgv.Columns.Add(StartTimeColumn);

                // Add EndTime Column
                DataGridViewTextBoxColumn EndTimeColumn = new();
                EndTimeColumn.HeaderText = "End time";
                EndTimeColumn.ReadOnly = true;
                EndTimeColumn.Resizable = DataGridViewTriState.False;
                EndTimeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                EndTimeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                EndTimeColumn.CellTemplate = new DataGridViewTextBoxCell();
                EndTimeColumn.ValueType = typeof(string);
                dgv.Columns.Add(EndTimeColumn);

                // Add Duration Column
                DataGridViewTextBoxColumn DurationColumn = new();
                DurationColumn.HeaderText = "Duration";
                DurationColumn.ReadOnly = true;
                DurationColumn.Resizable = DataGridViewTriState.False;
                DurationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                DurationColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                DurationColumn.CellTemplate = new DataGridViewTextBoxCell();
                DurationColumn.ValueType = typeof(string);
                dgv.Columns.Add(DurationColumn);

                // Add Text Column
                DataGridViewTextBoxColumn TextColumn = new();
                TextColumn.HeaderText = "Text";
                TextColumn.ReadOnly = true;
                TextColumn.Resizable = DataGridViewTriState.False;
                TextColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                TextColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                TextColumn.CellTemplate = new DataGridViewTextBoxCell();
                TextColumn.ValueType = typeof(string);
                dgv.Columns.Add(TextColumn);
            });
            
        }
        //---------------------------------------------------------------------------------------
        public static void LoadColumnsApplyState(CustomDataGridView dgv)
        {
            dgv.InvokeIt(() =>
            {
                // Remove All Columns
                dgv.Columns.Clear();

                // Add Apply Column
                DataGridViewCheckBoxColumn ApplyColumn = new();
                ApplyColumn.HeaderText = "Apply";
                ApplyColumn.ReadOnly = false;
                ApplyColumn.Resizable = DataGridViewTriState.False;
                ApplyColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                ApplyColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.ColumnHeader;
                ApplyColumn.CellTemplate = new DataGridViewCheckBoxCell();
                ApplyColumn.ValueType = typeof(bool);
                dgv.Columns.Add(ApplyColumn);

                // Add Line# Column
                DataGridViewTextBoxColumn LineColumn = new();
                LineColumn.HeaderText = "Line#";
                LineColumn.ReadOnly = true;
                LineColumn.Resizable = DataGridViewTriState.False;
                LineColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                LineColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.AllCells;
                LineColumn.CellTemplate = new DataGridViewTextBoxCell();
                LineColumn.ValueType = typeof(string);
                dgv.Columns.Add(LineColumn);

                // Add Before Column
                DataGridViewTextBoxColumn BeforeColumn = new();
                BeforeColumn.HeaderText = "Before";
                BeforeColumn.ReadOnly = true;
                BeforeColumn.Resizable = DataGridViewTriState.True;
                BeforeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                BeforeColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.None;
                BeforeColumn.Width = 400;
                BeforeColumn.CellTemplate = new DataGridViewTextBoxCell();
                BeforeColumn.ValueType = typeof(string);
                dgv.Columns.Add(BeforeColumn);

                // Add After Column
                DataGridViewTextBoxColumn AfterColumn = new();
                AfterColumn.HeaderText = "After";
                AfterColumn.ReadOnly = true;
                AfterColumn.Resizable = DataGridViewTriState.False;
                AfterColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
                AfterColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
                AfterColumn.CellTemplate = new DataGridViewTextBoxCell();
                AfterColumn.ValueType = typeof(string);
                dgv.Columns.Add(AfterColumn);
            });
        }
        //=======================================================================================
        public static int FramesToMillisecondsMax999(double frames)
        {
            int ms = (int)Math.Round(frames * (TimeCode.BaseUnit / GetFrameForCalculation(Configuration.Settings.General.CurrentFrameRate)));
            return Math.Min(ms, 999);
        }

        public static int FramesToMilliseconds(double frames)
        {
            return (int)Math.Round(frames * (TimeCode.BaseUnit / GetFrameForCalculation(Configuration.Settings.General.CurrentFrameRate)));
            
        }

        public static int MillisecondsToFrames(double milliseconds)
        {
            return MillisecondsToFrames(milliseconds, Configuration.Settings.General.CurrentFrameRate);
        }

        public static int MillisecondsToFrames(double milliseconds, double frameRate)
        {
            return (int)Math.Round(milliseconds / (TimeCode.BaseUnit / GetFrameForCalculation(frameRate)), MidpointRounding.AwayFromZero);
        }

        private static double GetFrameForCalculation(double frameRate)
        {
            if (Math.Abs(frameRate - 23.976) < 0.01)
            {
                return 24000.0 / 1001.0;
            }
            if (Math.Abs(frameRate - 29.97) < 0.01)
            {
                return 30000.0 / 1001.0;
            }
            if (Math.Abs(frameRate - 59.94) < 0.01)
            {
                return 60000.0 / 1001.0;
            }

            return frameRate;
        }
        //=======================================================================================
        public static int CountGroupNames()
        {
            int count = 0;
            // Get file content from Embedded Resource
            string fileContents = Tools.Resource.GetResourceTextFile("PersianSubtitleFixes.multiple_replace.xml");
            XmlDocument docGN = new();
            docGN.LoadXml(fileContents);
            XmlNodeList nodesGN = docGN.GetElementsByTagName("Group");
            foreach (XmlNode node in nodesGN)
            {
                foreach (XmlNode childN in node.SelectNodes("Name"))
                {
                    if (childN.InnerText != exception1 && childN.InnerText != exception2 && childN.InnerText != exception3)
                    {
                        count++;
                    }
                }
            }
            return count;
        }
        //=======================================================================================
        public static List<string> ListGroupReplaceNames()
        {
            return ListGroupReplaceNamesOrHashcodes(0);
        }
        public static List<string> ListGroupReplaceHashcodes()
        {
            return ListGroupReplaceNamesOrHashcodes(1);
        }
        private static List<string> ListGroupReplaceNamesOrHashcodes(int index)
        {
            var name = "GroupsHashcodes";
            if (Tools.IsolatedStorage.IsFileExist(name))
            {
                string GT = Tools.IsolatedStorage.ReadIsolatedTextFile(name);
                var split1 = GT.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
                List<string> listC = new();
                foreach (var s1 in split1)
                {
                    var split2 = s1.Split(new[] { delimiter }, StringSplitOptions.None)[index];
                    listC.Add(split2);
                }
                return listC;
            }
            Console.WriteLine("File not exist: " + name);
            return string.Empty.ToCharArray().Select(c => c.ToString()).ToList();
        }
        //=======================================================================================
        
        //=======================================================================================
    }
}
