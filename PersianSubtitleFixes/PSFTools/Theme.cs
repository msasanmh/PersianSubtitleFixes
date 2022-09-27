using PersianSubtitleFixes;
using MsmhTools;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using CustomControls;

namespace PSFTools
{
    public static class Theme
    {
        public static readonly string DefaultTheme = Themes.Light;
        //=======================================================================================
        public struct Themes
        {
            public const string Light = "Light";
            public const string Dark = "Dark";
        }
        //=======================================================================================
        public static string GetTheme()
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;

            if (!ds.Tables.Contains(PSFSettings.SettingsName.General))
                return DefaultTheme;
            if (ds.Tables[PSFSettings.SettingsName.General].Columns.Contains("Theme"))
            {
                if (ds.Tables[PSFSettings.SettingsName.General].Rows[0] == null)
                    return DefaultTheme;
                else
                    return (string)ds.Tables[PSFSettings.SettingsName.General].Rows[0]["Theme"];
            }
            else
                return DefaultTheme;
        }
        //=======================================================================================
        public static void LoadTheme(Form form, Control.ControlCollection controlCollection)
        {
            string currentTheme = GetTheme();
            ResourceManager rm = new("PersianSubtitleFixes.Properties.Resources", typeof(PersianSubtitleFixes.Properties.Resources).Assembly);

            if (currentTheme == Themes.Light)
            {
                // Load Light Theme
                Colors.InitializeLight();
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var ctscb in Tools.Controllers.GetAllControlsByType<CustomToolStripComboBox>(form))
                {
                    ctscb.BackColor = Colors.BackColor;
                    ctscb.ForeColor = Colors.ForeColor;
                    ctscb.BorderColor = Colors.Border;
                    ctscb.SelectionColor = Colors.Selection;
                }
            }
            else if (currentTheme == Themes.Dark)
            {
                // Load Dark Theme
                Colors.InitializeDark();
                DarkTheme.SetDarkTheme(form); // Make TitleBar Black
                foreach (Control c in Tools.Controllers.GetAllControls(form))
                {
                    DarkTheme.SetDarkTheme(c);
                    SetColors(c);
                }
                // Find ToolStrip Controls
                foreach (var b in Tools.Controllers.GetAllControlsByType<ToolStripButton>(form))
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
                    else if (b.Name.Contains("Edit"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Edit_Blue;
                    else if (b.Name.Contains("Settings"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Settings_Blue;
                    else if (b.Name.Contains("About"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.About_Blue;
                    else if (b.Name.Contains("Exit"))
                        b.Image = PersianSubtitleFixes.Properties.Resources.Exit_Blue;
                }
                foreach (var ctscb in Tools.Controllers.GetAllControlsByType<CustomToolStripComboBox>(form))
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
            if (c is TabPage)
                return;

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
            else if (c is CustomContextMenuStrip customContextMenuStrip)
            {
                customContextMenuStrip.BorderColor = Colors.Border;
                customContextMenuStrip.SelectionColor = Colors.Selection;
            }
            else if (c is CustomDataGridView customDataGridView)
            {
                customDataGridView.BorderColor = Colors.Border;
                customDataGridView.SelectionColor = Colors.Selection;
                customDataGridView.GridColor = Colors.GridLines;
                customDataGridView.CheckColor = Colors.Tick;
            }
            else if (c is CustomGroupBox customGroupBox)
            {
                customGroupBox.BorderColor = Colors.Border;
            }
            else if (c is CustomLabel customLabel)
            {
                customLabel.BorderColor = Colors.Border;
                customLabel.BackColor = Colors.BackColor;
                customLabel.ForeColor = Colors.ForeColor;
            }
            else if (c is CustomMenuStrip customMenuStrip)
            {
                customMenuStrip.BorderColor = Colors.Border;
                customMenuStrip.SelectionColor = Colors.Selection;
            }
            else if (c is CustomNumericUpDown customNumericUpDown)
            {
                customNumericUpDown.BorderColor = Colors.Border;
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
            else if (c is CustomRadioButton customRadioButton)
            {
                customRadioButton.BorderColor = Colors.Border;
                customRadioButton.CheckColor = Colors.Tick;
                customRadioButton.SelectionColor = Colors.SelectionRectangle;
            }
            else if (c is CustomStatusStrip customStatusStrip)
            {
                customStatusStrip.BorderColor = Colors.Border;
                customStatusStrip.SelectionColor = Colors.Selection;
            }
            else if (c is CustomTabControl customTabControl)
            {
                customTabControl.BackColor = Colors.BackColor;
                customTabControl.ForeColor = Colors.ForeColor;
                customTabControl.BorderColor = Colors.Border;
            }
            else if (c is CustomTextBox customTextBox)
            {
                customTextBox.BorderColor = Colors.Border;
            }
            else if (c is CustomTimeUpDown customTimeUpDown)
            {
                customTimeUpDown.BorderColor = Colors.Border;
            }
            else if (c is CustomToolStrip customToolStrip)
            {
                customToolStrip.BorderColor = Colors.Border;
                customToolStrip.SelectionColor = Colors.Selection;
                foreach (ToolStripItem item in customToolStrip.Items)
                {
                    var items = Tools.Controllers.GetAllToolStripItems(item);
                    foreach (ToolStripItem toolItem in items)
                        if (toolItem is ToolStripSeparator)
                        {
                            ToolStripSeparator tss = toolItem as ToolStripSeparator;
                            tss.ForeColor = Colors.Border;
                        }
                }
            }
            else if (c is CustomVScrollBar customVScrollBar)
            {
                customVScrollBar.BorderColor = Colors.Border;
            }
            else if (c is Label label)
            {
                if (label.Name.Contains("LabelAppliedRegex"))
                {
                    label.BackColor = Colors.BackColorDarker;
                    label.ForeColor = Colors.ForeColor;
                }
            }
        }
        //=======================================================================================
    }

    public sealed class Colors
    {
        public static Color BackColor { get; set; }
        public static Color BackColorDisabled { get; set; }
        public static Color BackColorDarker { get; set; }
        public static Color BackColorDarkerDisabled { get; set; }
        public static Color BackColorMouseHover { get; set; }
        public static Color BackColorMouseDown { get; set; }
        public static Color ForeColor { get; set; }
        public static Color ForeColorDisabled { get; set; }
        public static Color Border { get; set; }
        public static Color BorderDisabled { get; set; }
        public static Color Chunks { get; set; }
        public static Color GridLines { get; set; }
        public static Color GridLinesDisabled { get; set; }
        public static Color Selection { get; set; }
        public static Color SelectionRectangle { get; set; }
        public static Color SelectionUnfocused { get; set; }
        public static Color Tick { get; set; }
        public static Color TickDisabled { get; set; }
        public static Color TitleBarBackColor { get; set; }
        public static Color TitleBarForeColor { get; set; }
        public static void InitializeLight()
        {
            BackColor = SystemColors.Control;
            BackColorDisabled = BackColor.ChangeBrightness(-0.3f);
            BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            BackColorDarkerDisabled = BackColorDarker.ChangeBrightness(-0.3f);
            BackColorMouseHover = BackColor.ChangeBrightness(-0.1f);
            BackColorMouseDown = BackColorMouseHover.ChangeBrightness(-0.1f);
            ForeColor = Color.Black;
            ForeColorDisabled = ForeColor.ChangeBrightness(0.3f);
            Border = Color.FromArgb(52, 152, 219);
            BorderDisabled = Border.ChangeBrightness(0.3f);
            Chunks = Color.DodgerBlue;
            GridLines = ForeColor.ChangeBrightness(0.5f);
            GridLinesDisabled = GridLines.ChangeBrightness(0.3f);
            Selection = Color.FromArgb(104, 151, 187);
            SelectionRectangle = Selection;
            SelectionUnfocused = Selection.ChangeBrightness(0.3f);
            Tick = Border;
            TickDisabled = Tick.ChangeBrightness(0.3f);
            TitleBarBackColor = Color.LightBlue;
            TitleBarForeColor = Color.Black;
            CC();
        }
        public static void InitializeDark()
        {
            BackColor = Color.DarkGray.ChangeBrightness(-0.8f);
            BackColorDisabled = BackColor.ChangeBrightness(0.3f);
            BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            BackColorDarkerDisabled = BackColorDarker.ChangeBrightness(0.3f);
            BackColorMouseHover = BackColor.ChangeBrightness(0.1f);
            BackColorMouseDown = BackColorMouseHover.ChangeBrightness(0.1f);
            ForeColor = Color.LightGray;
            ForeColorDisabled = ForeColor.ChangeBrightness(-0.3f);
            Border = Color.FromArgb(52, 152, 219);
            BorderDisabled = Border.ChangeBrightness(-0.3f);
            Chunks = Color.DodgerBlue;
            GridLines = ForeColor.ChangeBrightness(-0.5f);
            GridLinesDisabled = GridLines.ChangeBrightness(-0.3f);
            Selection = Color.Black;
            SelectionRectangle = Selection;
            SelectionUnfocused = Selection.ChangeBrightness(0.3f);
            Tick = Border;
            TickDisabled = Tick.ChangeBrightness(-0.3f);
            TitleBarBackColor = Color.DarkBlue;
            TitleBarForeColor = Color.White;
            CC();
        }

        private static void CC()
        {
            // MessageBox
            CustomMessageBox.BackColor = BackColor;
            CustomMessageBox.ForeColor = ForeColor;
            CustomMessageBox.BorderColor = Border;

            // InputBox
            CustomInputBox.BackColor = BackColor;
            CustomInputBox.ForeColor = ForeColor;
            CustomInputBox.BorderColor = Border;
        }

    }
}
