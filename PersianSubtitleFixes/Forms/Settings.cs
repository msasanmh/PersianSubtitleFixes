using CustomControls;
using MsmhTools;
using PersianSubtitleFixes.msmh;
using System.Data;
using System.Text;
using System.Windows.Forms;

namespace PersianSubtitleFixes
{
    public partial class Settings : Form
    {
        private FormMain? MainForm;
        private static string? CurrentTheme;
        public Settings()
        {
            InitializeComponent();
            CurrentTheme = PSF.GetTheme();
            PSF.LoadTheme(this, Controls);
            
            // Initialize Default Encoding
            EncodingTool.InitializeTextEncoding(CustomComboBoxEncoding);
            CustomComboBoxEncoding.SelectedItem = EncodingTool.GetDefaultEncodingDisplayName();
            
            // Update ComboBox Theme
            UpdateComboBoxTheme(CustomComboBoxTheme);
        }

        private static void UpdateComboBoxTheme(ComboBox comboBox)
        {
            if (comboBox.SelectedItem == null)
            {
                string theme = PSF.GetTheme();
                if (theme != null)
                    comboBox.SelectedItem = theme;
                else
                    comboBox.SelectedItem = PSF.DefaultTheme;
            }
        }

        private void CustomComboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void SaveComboBoxEncoding()
        {
            if (CustomComboBoxEncoding.SelectedItem == null)
                return;

            string tableName = "General";
            if (!FormMain.DataSetSettings.Tables.Contains(tableName))
            {
                DataTable dataTable = new();
                dataTable.TableName = tableName;
                FormMain.DataSetSettings.Tables.Add(dataTable);
            }
            var dt = FormMain.DataSetSettings.Tables[tableName];

            if (!dt.Columns.Contains("DefaultEncodingDisplayName"))
                dt.Columns.Add("DefaultEncodingDisplayName");

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1["DefaultEncodingDisplayName"] = CustomComboBoxEncoding.SelectedItem.ToString();
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                dataRow1["DefaultEncodingDisplayName"] = CustomComboBoxEncoding.SelectedItem.ToString();
            }
        }

        private void CustomComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void SaveComboBoxTheme()
        {
            if (CustomComboBoxTheme.SelectedItem == null)
                return;

            string tableName = "General";
            if (!FormMain.DataSetSettings.Tables.Contains(tableName))
            {
                DataTable dataTable = new();
                dataTable.TableName = tableName;
                FormMain.DataSetSettings.Tables.Add(dataTable);
            }
            var dt = FormMain.DataSetSettings.Tables[tableName];

            if (!dt.Columns.Contains("Theme"))
                dt.Columns.Add("Theme");

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1["Theme"] = CustomComboBoxTheme.SelectedItem.ToString();
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                dataRow1["Theme"] = CustomComboBoxTheme.SelectedItem.ToString();
            }
        }

        private async void CustomButtonSave_Click(object sender, EventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                SaveComboBoxEncoding();
                SaveComboBoxTheme();
                await Tools.Files.WriteAllTextAsync(Tools.Info.ApplicationFullPathWithoutExtension + ".xml", FormMain.DataSetSettings.ToXmlWithWriteMode(XmlWriteMode.IgnoreSchema), new UTF8Encoding(false));

                Close();

                if (CurrentTheme != CustomComboBoxTheme.SelectedItem.ToString())
                {
                    string restart = "Restart application for theme changes to take effect.";
                    switch (CustomMessageBox.Show(restart, "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            {
                                Application.Restart();
                            }
                            break;
                        case DialogResult.No:
                            break;
                        default:
                            break;
                    }
                }

            }

        }
    }
}
