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

namespace PersianSubtitleFixes.msmh
{
    public static class PSF
    {
        public static readonly string ResourcePath = "PersianSubtitleFixes.ReplaceList.multiple_replace.xml";
        public static readonly string Group_ChangeArabicCharsToPersian = "Change Arabic Char to Persian";
        private static readonly string exception1 = "Options";
        private static readonly string exception2 = "Formal to Slang";
        private static readonly string exception3 = "Guide";
        private static readonly string delimiter = "|";
        public static readonly string suffixN = "_Normal";
        public static readonly string suffixR = "_RegularExpression";
        public static readonly Lazy<string> SubtitleExtensionFilter = new(FileDlgFilter);
        public static readonly string DefaultTheme = "Light";
        //=======================================================================================
        public static string GetTheme()
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;

            if (!ds.Tables.Contains("General"))
                return DefaultTheme;
            if (ds.Tables["General"].Columns.Contains("Theme"))
            {
                if (ds.Tables["General"].Rows[0] == null)
                    return DefaultTheme;
                else
                    return (string)ds.Tables["General"].Rows[0]["Theme"];
            }
            else
                return DefaultTheme;
        }
        //=======================================================================================
        public static void LoadTheme(Form form, Control.ControlCollection controlCollection)
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;
            ResourceManager rm = new("PersianSubtitleFixes.Properties.Resources", typeof(Properties.Resources).Assembly);

            if (ds.Tables["General"] == null)
            {
                // Load Default Theme (Light)
                Colors.InitializeLight();
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var ctscb in Tools.Controllers.GetSubControls<CustomToolStripComboBox>(form))
                {
                    ctscb.BackColor = Colors.BackColor;
                    ctscb.ForeColor = Colors.ForeColor;
                    ctscb.BorderColor = Colors.Border;
                    ctscb.SelectionColor = Colors.Selection;
                }
                return;
            }

            string s = string.Empty;
            if (ds.Tables["General"].Columns.Contains("Theme"))
                s = (string)ds.Tables["General"].Rows[0]["Theme"];
            if (s == "Dark")
            {
                // Load Dark Theme
                Colors.InitializeDark();
                DarkTheme.SetDarkTheme(form); // Makes TitleBar Black
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var ctscb in Tools.Controllers.GetSubControls<CustomToolStripComboBox>(form))
                {
                    ctscb.BackColor = Colors.BackColor;
                    ctscb.ForeColor = Colors.ForeColor;
                    ctscb.BorderColor = Colors.Border;
                    ctscb.SelectionColor = Colors.Selection;
                }
                foreach (var b in Tools.Controllers.GetSubControls<ToolStripButton>(form))
                {

                    if (b.Name.Contains("Open"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Open_Blue;
                    else if (b.Name.Contains("Save"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Save_Blue;
                    else if (b.Name.Contains("SaveAs"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Save_as_Blue;
                    else if (b.Name.Contains("Undo"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Undo_Blue;
                    else if (b.Name.Contains("Redo"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Redo_Blue;
                    else if (b.Name.Contains("Settings"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Settings_Blue;
                    else if (b.Name.Contains("About"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.About_Blue;
                    else if (b.Name.Contains("Exit"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Exit_Blue;
                }
            }
            else if (s == "Light")
            {
                // Load Light Theme
                Colors.InitializeLight();
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var ctscb in Tools.Controllers.GetSubControls<CustomToolStripComboBox>(form))
                {
                    ctscb.BackColor = Colors.BackColor;
                    ctscb.ForeColor = Colors.ForeColor;
                    ctscb.BorderColor = Colors.Border;
                    ctscb.SelectionColor = Colors.Selection;
                }
            }
            else
            {
                // Load Default Theme (Light)
                Colors.InitializeLight();
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var ctscb in Tools.Controllers.GetSubControls<CustomToolStripComboBox>(form))
                {
                    ctscb.BackColor = Colors.BackColor;
                    ctscb.ForeColor = Colors.ForeColor;
                    ctscb.BorderColor = Colors.Border;
                    ctscb.SelectionColor = Colors.Selection;
                }
            }
        }
        //=======================================================================================
        public static void SetColors(Control c)
        {
            c.BackColor = Colors.BackColor;
            c.ForeColor = Colors.ForeColor;
            if (c is CustomButton customButton)
            {
                customButton.BorderColor = Colors.Border;
                customButton.SelectionColor = Colors.SelectionRectangle;
            }
            else if (c is CustomCheckBox customCheckBox)
            {
                customCheckBox.BorderColor = Colors.Border;
                customCheckBox.CheckColor = Colors.Tick;
                customCheckBox.SelectionColor = Colors.SelectionRectangle;
            }
            else if (c is CustomComboBox customComboBox)
            {
                customComboBox.BorderColor = Colors.Border;
                customComboBox.SelectionColor = Colors.Selection;
            }
            else if (c is CustomDataGridView customDataGridView)
            {
                customDataGridView.BorderColor = Colors.Border;
                customDataGridView.SelectionColor = Colors.Selection;
                customDataGridView.GridColor = Colors.GridLines;
                customDataGridView.CheckColor = Colors.Tick;
            }
            else if (c is CustomPanel customPanel)
            {
                customPanel.BorderColor = Colors.Border;
            }
            else if (c is CustomProgressBar customProgressBar)
            {
                customProgressBar.BorderColor = Colors.Border;
                customProgressBar.ChunksColor = Colors.Chunks;
            }
        }
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
        }
        //=======================================================================================
        public static List<string>? ListGroupNames()
        {
            string fileContent = Tools.Resource.GetResourceTextFile(ResourcePath);
            //var listGN = new List<string>();
            List<string> listGN = new();
            XmlDocument docGN = new();
            if (fileContent != null)
            {
                docGN.LoadXml(fileContent);
                XmlNodeList nodesGN = docGN.GetElementsByTagName("Group");
                foreach (XmlNode node in nodesGN)
                {
                    foreach (XmlNode childN in node.SelectNodes("Name"))
                    {
                        if (childN.InnerText != exception1 || childN.InnerText != exception2 || childN.InnerText != exception3)
                            listGN.Add(childN.InnerText);
                    }
                }
                return listGN;
            }
            else
                return null;
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
