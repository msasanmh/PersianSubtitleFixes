using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomControls;
using MsmhTools;
using PersianSubtitleFixes;

namespace PSFTools
{
    public class PSFSettings
    {
        private static readonly string SettingsFile = Tools.Info.ApplicationFullPathWithoutExtension + ".xml";

        public static void Initialize(FormMain formMain)
        {
            // DataSet Name
            FormMain.DataSetSettings.DataSetName = "Settings";
            
            // DataSet Import
            string settingsFile = Tools.Info.ApplicationFullPathWithoutExtension + ".xml";
            if (PSF.IsSettingsValid(settingsFile) == true)
            {
                Tools.Xml.RemoveNodesWithoutChild(settingsFile);
                FormMain.DataSetSettings.ToDataSet(settingsFile, XmlReadMode.Auto);
            }

            // Set Timing Names for Settings
            formMain.CustomCheckBoxFixNegative.Tag = "FixNegative";
            formMain.CustomCheckBoxFixIncorrectTimeOrder.Tag = "FixIncorrectTimeOrder";
            formMain.CustomCheckBoxAdjustDurations.Tag = "AdjustDurations";
            formMain.CustomRadioButtonCharsPerSec.Tag = "RadioButtonCharsPerSec";
            formMain.CustomNumericUpDownCharsPerSec.Tag = "NumericUpDownCharsPerSec";
            formMain.CustomNumericUpDownMinDL.Tag = "MinimumDurationLimit";
            formMain.CustomNumericUpDownMaxDL.Tag = "MaximumDurationLimit";
            formMain.CustomNumericUpDownMinGap.Tag = "MinimumGap";
            formMain.CustomCheckBoxMergeSameTime.Tag = "CheckBoxMergeLinesWithSameTimeCode";
            formMain.CustomNumericUpDownSameTimeCode.Tag = "NumericUpDownMergeLinesWithSameTimeCode";
            formMain.CustomTimeUpDownAdjustTime.Tag = "TimeUpDownAdjustTime";
            formMain.CustomComboBoxFromFrameRate.Tag = "FromFrameRate";
            InitializeComboBoxFrameRate(formMain.CustomComboBoxFromFrameRate);
            formMain.CustomComboBoxToFrameRate.Tag = "ToFrameRate";
            InitializeComboBoxFrameRate(formMain.CustomComboBoxToFrameRate);
            formMain.CustomRadioButtonChangeSpeedC.Tag = "ChangeSpeedCustom";
            formMain.CustomRadioButtonChangeSpeedFDF.Tag = "ChangeSpeedFromDropFrame";
            formMain.CustomRadioButtonChangeSpeedTDF.Tag = "ChangeSpeedToDropFrame";
            formMain.CustomNumericUpDownChangeSpeed.Tag = "ChangeSpeedPercentage";

            static void InitializeComboBoxFrameRate(CustomComboBox customComboBox)
            {
                customComboBox.Items.Clear();
                customComboBox.BeginUpdate();
                customComboBox.Items.Add(FrameRate.f23976.ToString());
                customComboBox.Items.Add(FrameRate.f24.ToString());
                customComboBox.Items.Add(FrameRate.f25.ToString());
                customComboBox.Items.Add(FrameRate.f2997.ToString());
                customComboBox.Items.Add(FrameRate.f30.ToString());
                customComboBox.EndUpdate();
            }

            // Set Others Names for Settings
            formMain.CustomCheckBoxRemoveEmptyLines.Tag = "RemoveEmptyLines";
            formMain.CustomCheckBoxMergeSameText.Tag = "CheckBoxMergeLinesWithSameText";
            formMain.CustomNumericUpDownSameText.Tag = "NumericUpDownMergeLinesWithSameText";
            formMain.CustomCheckBoxRemoveUCChars.Tag = "RemoveUnicodeControlChars";

            // Set General Names for Settings
            formMain.ShowPopupGuideToolStripMenuItem.Tag = "ShowPopupGuide";
        }

        public static void Initialize(Settings formSettings)
        {
            formSettings.CustomComboBoxEncoding.Tag = "DefaultEncodingDisplayName";
            formSettings.CustomComboBoxTheme.Tag = "Theme";
        }

        private struct FrameRate
        {
            public const double f23976 = 23.976;
            public const double f24 = 24;
            public const double f25 = 25;
            public const double f2997 = 29.97;
            public const double f30 = 30;
        }

        public struct SettingsName
        {
            public const string General = "General";
            public const string CheckBoxes = "CheckBoxes";
            public const string Timing = "Timing";
            public const string Others = "Others";
        }

        // Load
        public static void Load(FormMain formMain, string settingsName)
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;
            if (settingsName.ToString() == SettingsName.General)
            {
                ReadItemToolStripMenuItem(ds, settingsName, formMain.ShowPopupGuideToolStripMenuItem, true);
            }
            else if (settingsName.ToString() == SettingsName.Timing)
            {
                // Timing
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxFixNegative, false);
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxFixIncorrectTimeOrder, false);
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxAdjustDurations, false);
                ReadItemRadioButton(ds, settingsName, formMain.CustomRadioButtonCharsPerSec, true);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownCharsPerSec, 6.5);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownMinDL, 1000);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownMaxDL, 8000);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownMinGap, 42);
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxMergeSameTime, true);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownSameTimeCode, 250);

                // Synchronization
                ReadItemTimeUpDown(ds, settingsName, formMain.CustomTimeUpDownAdjustTime, 0);
                ReadItemComboBox(ds, settingsName, formMain.CustomComboBoxFromFrameRate, FrameRate.f23976.ToString());
                ReadItemComboBox(ds, settingsName, formMain.CustomComboBoxToFrameRate, FrameRate.f23976.ToString());
                ReadItemRadioButton(ds, settingsName, formMain.CustomRadioButtonChangeSpeedC, true);
                ReadItemRadioButton(ds, settingsName, formMain.CustomRadioButtonChangeSpeedFDF, false);
                ReadItemRadioButton(ds, settingsName, formMain.CustomRadioButtonChangeSpeedTDF, false);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownChangeSpeed, 100.0);
            }
            else if (settingsName.ToString() == SettingsName.Others)
            {
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxRemoveEmptyLines, true);
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxMergeSameText, false);
                ReadItemNumericUpDown(ds, settingsName, formMain.CustomNumericUpDownSameText, 250);
                ReadItemCheckBox(ds, settingsName, formMain.CustomCheckBoxRemoveUCChars, false);
            }
            else
                return;

            void ReadItemToolStripMenuItem(DataSet ds, string tableName, ToolStripMenuItem toolStripMenuItem, bool defaultValue)
            {
                string name = toolStripMenuItem.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue = bool.Parse(ds.Tables[tableName].Rows[0][name].ToString().IsNotNull());
                    }
                }

                toolStripMenuItem.Checked = defaultValue;
            }

            void ReadItemNumericUpDown(DataSet ds, string tableName, CustomNumericUpDown customNumeric, double defaultValue)
            {
                string name = customNumeric.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue = double.Parse(ds.Tables[tableName].Rows[0][name].ToString().IsNotNull());
                    }
                }

                customNumeric.Value = (decimal)defaultValue;
            }

            void ReadItemTimeUpDown(DataSet ds, string tableName, CustomTimeUpDown customTime, double defaultValue)
            {
                string name = customTime.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue = double.Parse(ds.Tables[tableName].Rows[0][name].ToString().IsNotNull());
                    }
                }

                customTime.Value = (decimal)defaultValue;
            }

            void ReadItemCheckBox(DataSet ds, string tableName, CustomCheckBox customCheckBox, bool defaultValue)
            {
                string name = customCheckBox.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue = bool.Parse(ds.Tables[tableName].Rows[0][name].ToString().IsNotNull());
                    }
                }

                customCheckBox.Checked = defaultValue;
            }

            void ReadItemRadioButton(DataSet ds, string tableName, CustomRadioButton customRadioButton, bool defaultValue)
            {
                string name = customRadioButton.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue = bool.Parse(ds.Tables[tableName].Rows[0][name].ToString().IsNotNull());
                    }
                }

                customRadioButton.Checked = defaultValue;
            }

            void ReadItemComboBox(DataSet ds, string tableName, CustomComboBox customComboBox, string defaultValue)
            {
                string name = customComboBox.Tag.ToString().IsNotNull();
                if (ds.Tables[tableName] != null)
                {
                    if (ds.Tables[tableName].Columns.Contains(name))
                    {
                        defaultValue =ds.Tables[tableName].Rows[0][name].ToString().IsNotNull();
                    }
                }

                customComboBox.SelectedItem = defaultValue;
            }
        }

        public static void Load(Settings formSettings, string settingsName)
        {
            DataSet ds;
            ds = FormMain.DataSetSettings;
            if (settingsName.ToString() == SettingsName.General)
            {
                ReadItemComboBoxEncoding(formSettings.CustomComboBoxEncoding);
                ReadItemComboBoxTheme(formSettings.CustomComboBoxTheme, Theme.DefaultTheme);

            }
            else
                return;

            void ReadItemComboBoxEncoding(CustomComboBox customComboBox)
            {
                customComboBox.SelectedItem = EncodingTool.GetDefaultEncodingDisplayName();
            }

            void ReadItemComboBoxTheme(CustomComboBox customComboBox, string defaultValue)
            {
                if (customComboBox.SelectedItem == null)
                {
                    string theme = Theme.GetTheme();
                    if (theme != null)
                        customComboBox.SelectedItem = theme;
                    else
                        customComboBox.SelectedItem = defaultValue;
                }
            }

        }

        // Save
        private static DataTable GetSettingsDataTable(string tableName)
        {
            if (!FormMain.DataSetSettings.Tables.Contains(tableName))
            {
                DataTable dataTable = new();
                dataTable.TableName = tableName;
                FormMain.DataSetSettings.Tables.Add(dataTable);
            }
            return FormMain.DataSetSettings.Tables[tableName].IsNotNull();
        }

        public static void Save(string settingsName, ToolStripMenuItem toolStripMenuItem)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), toolStripMenuItem.Tag.ToString().IsNotNull(), toolStripMenuItem.Checked);
        }

        public static void Save(string settingsName, CustomComboBox customComboBox)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), customComboBox.Tag.ToString().IsNotNull(), customComboBox.SelectedItem.ToString().IsNotNull());
        }

        public static void Save(string settingsName, CustomNumericUpDown customNumeric)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), customNumeric.Tag.ToString().IsNotNull(), (double)customNumeric.Value);
        }

        public static void Save(string settingsName, CustomTimeUpDown customTimeUpDown)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), customTimeUpDown.Tag.ToString().IsNotNull(), (double)customTimeUpDown.Value);
        }

        public static void Save(string settingsName, CustomCheckBox customCheckBox)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), customCheckBox.Tag.ToString().IsNotNull(), customCheckBox.Checked);
        }

        public static void Save(string settingsName, CustomRadioButton customRadioButton)
        {
            DataTable dt = GetSettingsDataTable(settingsName);
            AddOrUpdateSettings(dt.IsNotNull(), customRadioButton.Tag.ToString().IsNotNull(), customRadioButton.Checked);
        }

        private static void AddOrUpdateSettings(DataTable dt, string name, string value)
        {
            if (!dt.Columns.Contains(name))
                dt.Columns.Add(name);

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1[name] = value;
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                if (dataRow1 != null)
                    dataRow1[name] = value;
            }
        }

        private static void AddOrUpdateSettings(DataTable dt, string name, double value)
        {
            if (!dt.Columns.Contains(name))
                dt.Columns.Add(name);

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1[name] = (double)value;
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                if (dataRow1 != null)
                    dataRow1[name] = (double)value;
            }
        }

        private static void AddOrUpdateSettings(DataTable dt, string name, bool value)
        {
            if (!dt.Columns.Contains(name))
                dt.Columns.Add(name);

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1[name] = value;
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                if (dataRow1 != null)
                    dataRow1[name] = value;
            }
        }

        public static async Task SaveToFile()
        {
            await Tools.Files.WriteAllTextAsync(SettingsFile,
                    FormMain.DataSetSettings.ToXmlWithWriteMode(XmlWriteMode.IgnoreSchema), new UTF8Encoding(false));
        }

    }
}
