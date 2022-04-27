using MsmhTools;
using PersianSubtitleFixes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls
{
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
            //Border = ForeColor.ChangeBrightness(-0.3f);
            Border = Color.FromArgb(52, 152, 219);
            BorderDisabled = Border.ChangeBrightness(-0.3f);
            Chunks = Color.DodgerBlue;
            GridLines = ForeColor.ChangeBrightness(-0.5f);
            GridLinesDisabled = GridLines.ChangeBrightness(-0.3f);
            Selection = Color.FromArgb(104, 151, 187);
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
        }
        
    }
}
