using MsmhTools;
using PersianSubtitleFixes.msmh;
using System.Windows.Forms;

namespace PersianSubtitleFixes
{
    public partial class About : Form
    {
        private static string? CurrentTheme;

        public About()
        {
            InitializeComponent();
            CurrentTheme = PSF.GetTheme();
            PSF.LoadTheme(this, Controls);

            string productName = Tools.Info.InfoExecutingAssembly.ProductName;
            var productVersion = Tools.Info.InfoExecutingAssembly.ProductVersion;

            Text = "About " + productName;

            // Product Name
            LabelProduct.Text = productName + " v" + productVersion.ToString();

            // Product Details
            LabelDetails.Text = productName + " is a free software to enhance Persian subtitles.\r\nIt's under the GNU GPLv3 License.";

            // Product Homepage
            LabelHomePage.Text = "Homepage:";
            LinkLabelHomePage.Text = "Github Page";
            LinkLabelHomePage.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) =>
            {
                Tools.Openlinks.OpenUrl("https://github.com/msasanmh/PersianSubtitleFixes");
            };

            // Subtitle Library
            LabelLib.Text = "Subtitle Library:";
            LinkLabelLib.Text = "LibSE by Nikse";
            LinkLabelLib.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) =>
            {
                Tools.Openlinks.OpenUrl("https://github.com/SubtitleEdit/subtitleedit/tree/master/src/libse");
            };

            // Iconset
            LabelIconSet.Text = "Iconset:";
            LinkLabelIconSet.Text = "Icons8.com";
            LinkLabelIconSet.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) =>
            {
                Tools.Openlinks.OpenUrl("https://icons8.com");
            };

            // Email
            LabelEmail.Text = "Email:";
            LinkLabelEmail.Text = "msasanmh@gmail.com";
            LinkLabelEmail.LinkClicked += (object sender, LinkLabelLinkClickedEventArgs e) =>
            {
                Tools.Openlinks.OpenUrl("mailto:msasanmh@gmail.com");
            };

        }

        private void CustomButtonOK_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
