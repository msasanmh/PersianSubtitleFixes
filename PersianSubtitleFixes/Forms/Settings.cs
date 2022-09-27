using CustomControls;
using MsmhTools;
using PSFTools;
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

            // Initialize Settings
            PSFSettings.Initialize(this);

            // Load Theme
            CurrentTheme = Theme.GetTheme();
            Theme.LoadTheme(this, Controls);

            // Initialize Default Encoding
            EncodingTool.InitializeTextEncoding(CustomComboBoxEncoding);

            PSFSettings.Load(this, PSFSettings.SettingsName.General);
        }

        private void CustomComboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private void CustomComboBoxTheme_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        private async void CustomButtonSave_Click(object sender, EventArgs e)
        {
            if (DialogResult == DialogResult.OK)
            {
                if (CustomComboBoxEncoding != null)
                    PSFSettings.Save(PSFSettings.SettingsName.General, CustomComboBoxEncoding);
                if (CustomComboBoxTheme != null)
                    PSFSettings.Save(PSFSettings.SettingsName.General, CustomComboBoxTheme);

                await PSFSettings.SaveToFile();

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
