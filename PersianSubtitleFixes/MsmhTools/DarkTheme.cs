using CustomControls;
using Nikse.SubtitleEdit.Core.Common;
using PSFTools;
using System.Diagnostics;
using System.Drawing.Drawing2D;
using System.Reflection;
using System.Runtime.InteropServices;

namespace MsmhTools
{
    public class DarkTheme
    {
        public static Color BackColor { get; private set; }
        public static Color ForeColor { get; private set; }
        private static bool ButtonMouseHover { get; set; }
        private static bool ButtonMouseDown { get; set; }
        private static readonly CustomToolStripRenderer customToolStripRenderer = new();

        private const int DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1 = 19;
        private const int DWMWA_USE_IMMERSIVE_DARK_MODE = 20;
        private static bool UseImmersiveDarkMode(IntPtr handle, bool enabled)
        {
            if (IsWindows10OrGreater(17763))
            {
                var attribute = DWMWA_USE_IMMERSIVE_DARK_MODE_BEFORE_20H1;
                if (IsWindows10OrGreater(18985))
                {
                    attribute = DWMWA_USE_IMMERSIVE_DARK_MODE;
                }

                int useImmersiveDarkMode = enabled ? 1 : 0;
                return NativeMethods.DwmSetWindowAttribute(handle, attribute, ref useImmersiveDarkMode, sizeof(int)) == 0;
            }
            return false;
        }

        private static bool IsWindows10OrGreater(int build = -1)
        {
            return Environment.OSVersion.Version.Major >= 10 && Environment.OSVersion.Version.Build >= build;
        }

        private static void SetWindowThemeDark(Control control)
        {
            _ = NativeMethods.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
        }

        private static List<T> GetSubControls<T>(Control c)
        {
            var type = c.GetType();
            var fields = type.GetFields(BindingFlags.NonPublic | BindingFlags.Instance);
            var contextMenus = fields.Where(f => f.GetValue(c) != null &&
            (f.GetValue(c).GetType().IsSubclassOf(typeof(T)) || f.GetValue(c).GetType() == typeof(T)));
            var menus = contextMenus.Select(f => f.GetValue(c));
            return menus.Cast<T>().ToList();
        }

        public static void SetDarkTheme(Control ctrl, int iterations = 5)
        {
            if (iterations < 1)
            {
                // note: no need to restore the colors set are constants
                return;
            }

            BackColor = Colors.BackColor;
            ForeColor = Colors.ForeColor;
            customToolStripRenderer.BackColor = Colors.BackColor;
            customToolStripRenderer.ForeColor = Colors.ForeColor;
            customToolStripRenderer.BorderColor = Colors.Border;
            customToolStripRenderer.SelectionColor = Colors.Selection;

            if (ctrl is Form form)
            {
                // Set Title Bar Color (ImmersiveDarkMode)
                UseImmersiveDarkMode(ctrl.Handle, true);
                var contextMenus = GetSubControls<ContextMenuStrip>(form);
                foreach (ContextMenuStrip cms in contextMenus)
                {
                    cms.BackColor = BackColor;
                    cms.ForeColor = ForeColor;
                    cms.Renderer = customToolStripRenderer;
                    foreach (Control inner in cms.Controls)
                    {
                        SetDarkTheme(inner, iterations - 1);
                    }
                }
                
                var toolStrips = GetSubControls<ToolStrip>(form);
                foreach (ToolStrip c in toolStrips)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                    c.Renderer = customToolStripRenderer;
                }
                
                var toolStripComboBox = GetSubControls<ToolStripComboBox>(form);
                foreach (ToolStripComboBox c in toolStripComboBox)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                    c.FlatStyle = FlatStyle.Flat;
                }
                
                var toolStripContentPanels = GetSubControls<ToolStripContentPanel>(form);
                foreach (ToolStripContentPanel c in toolStripContentPanels)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                }
                
                var toolStripContainers = GetSubControls<ToolStripContainer>(form);
                foreach (ToolStripContainer c in toolStripContainers)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                }
                
                var toolStripDropDownMenus = GetSubControls<ToolStripDropDownMenu>(form);
                foreach (ToolStripDropDownMenu c in toolStripDropDownMenus)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                    foreach (ToolStripItem x in c.Items)
                    {
                        x.BackColor = BackColor;
                        x.ForeColor = ForeColor;
                    }
                }
                
                var toolStripMenuItems = GetSubControls<ToolStripMenuItem>(form);
                foreach (ToolStripMenuItem c in toolStripMenuItems)
                {
                    if (c.GetCurrentParent() is ToolStripDropDownMenu p)
                    {
                        p.BackColor = BackColor;
                        p.Renderer = customToolStripRenderer;
                    }

                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                }

                var toolStripSeparators = GetSubControls<ToolStripSeparator>(form);
                foreach (ToolStripSeparator c in toolStripSeparators)
                {
                    c.BackColor = BackColor;
                    c.ForeColor = ForeColor;
                }
            }

            FixControl(ctrl);
            foreach (Control c in GetSubControls<Control>(ctrl))
            {
                FixControl(c);
            }
        }

        private static void FixControl(Control c)
        {
            if (c is TabPage)
                return;

            c.BackColor = BackColor;
            c.ForeColor = ForeColor;

            if (c is Button b)
            {
                if (c is CustomButton)
                    return;

                b.FlatStyle = FlatStyle.Flat;
                b.EnabledChanged += Button_EnabledChanged;
                b.MouseDown += Button_MouseDown;
                b.MouseUp += Button_MouseUp;
                b.MouseEnter += Button_MouseEnter;
                b.MouseLeave += Button_MouseLeave;
                b.Paint += Button_Paint;
            }

            if (c is CheckBox cb)
            {
                if (c is CustomCheckBox)
                    return;

                cb.Paint += CheckBox_Paint;
            }

            if (c is RadioButton rb)
            {
                if (c is CustomRadioButton)
                    return;

                rb.Paint += RadioButton_Paint;
            }

            if (c is ComboBox cmBox)
            {
                if (c is CustomComboBox)
                    return;

                cmBox.FlatStyle = FlatStyle.Flat;
            }

            if (c is NumericUpDown numeric)
            {
                if (c is CustomNumericUpDown)
                    return;

                numeric.BorderStyle = BorderStyle.FixedSingle;
            }

            if (c is Panel p)
            {
                if (c is CustomPanel)
                    return;

                if (p.BorderStyle != BorderStyle.None)
                {
                    p.BorderStyle = BorderStyle.FixedSingle;
                }
                SetStyle(p, ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
                p.Paint += Panel_Paint;
                p.HandleCreated += Panel_HandleCreated;
                p.EnabledChanged += Panel_EnabledChanged;
                p.ControlAdded += Panel_ControlAdded;
                p.ControlRemoved += Panel_ControlRemoved;
                p.Enter += Panel_Enter;
                p.MouseEnter += Panel_MouseEnter;
                p.ParentChanged += Panel_ParentChanged;
                p.Resize += Panel_Resize;
                p.Scroll += Panel_Scroll;
                p.SizeChanged += Panel_SizeChanged;
            }

            if (c is ContextMenuStrip cms)
            {
                if (c is CustomContextMenuStrip)
                    return;

                cms.Renderer = customToolStripRenderer;
            }

            if (c is ToolStripDropDownMenu t)
            {
                t.BackColor = BackColor;
                t.ForeColor = ForeColor;
                t.Renderer = customToolStripRenderer;
                foreach (var x in t.Items)
                {
                    if (x is ToolStripMenuItem item)
                    {
                        item.BackColor = BackColor;
                        item.ForeColor = ForeColor;
                    }

                    if (x is ToolStripDropDownItem dropDownMenu && dropDownMenu.DropDownItems.Count > 0)
                    {
                        dropDownMenu.BackColor = BackColor;
                        dropDownMenu.ForeColor = ForeColor;
                        foreach (ToolStripItem dropDownItem in dropDownMenu.DropDownItems)
                        {
                            dropDownItem.ForeColor = ForeColor;
                            dropDownItem.BackColor = BackColor;
                        }
                    }
                }
            }

            if (c is LinkLabel linkLabel)
            {
                var linkColor = Color.FromArgb(0, 120, 215);
                linkLabel.ActiveLinkColor = linkColor;
                linkLabel.LinkColor = linkColor;
                linkLabel.VisitedLinkColor = linkColor;
                linkLabel.DisabledLinkColor = Color.FromArgb(0, 70, 170);
            }

            if (c is TreeView || c is ListBox || c is TextBox || c is RichTextBox)
            {
                SetWindowThemeDark(c);
            }

            if (c is TabControl tc)
            {
                if (c is CustomTabControl)
                    return;

                SetStyle(tc, ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
                tc.Paint += TabControl_Paint;
            }

            if (c is ListView lv)
            {
                if (c is CustomListView)
                    return;

                lv.OwnerDraw = true;
                lv.DrawItem += ListView_DrawItem;
                lv.DrawSubItem += ListView_DrawSubItem;
                lv.DrawColumnHeader += ListView_DrawColumnHeader;
                lv.EnabledChanged += ListView_EnabledChanged;
                lv.HandleCreated += ListView_HandleCreated;
            }

            if (c is DataGridView gv)
            {
                if (c is CustomDataGridView)
                    return;

                SetStyle(gv, ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
                gv.BorderStyle = BorderStyle.None;
                gv.RowsAdded += DataGridView_RowsAdded;
                gv.MouseMove += DataGridView_MouseMove;
                gv.Scroll += DataGridView_Scroll;
                gv.GotFocus += DataGridView_GotFocus;
                gv.LostFocus += DataGridView_LostFocus;
                gv.EnabledChanged += DataGridView_EnabledChanged;
                gv.HandleCreated += DataGridView_HandleCreated;
                gv.CellClick -= DataGridView_CellClick;
                gv.CellClick += DataGridView_CellClick;
                gv.CellPainting += DataGridView_CellPainting;
                gv.Paint += DataGridView_Paint;
            }
        }

        //==============================================================================
        private static void Button_EnabledChanged(object? sender, EventArgs e)
        {
            var button = (Button)sender;
            button.ForeColor = button.Enabled ? ForeColor : Colors.ForeColorDisabled;
        }

        private static void Button_MouseDown(object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                ButtonMouseDown = true;
                button.Invalidate();
            }
        }

        private static void Button_MouseUp(object? sender, MouseEventArgs e)
        {
            if (sender is Button button)
            {
                ButtonMouseDown = false;
                button.Invalidate();
            }
        }

        private static void Button_MouseEnter(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ButtonMouseHover = true;
                button.Invalidate();
            }
        }

        private static void Button_MouseLeave(object? sender, EventArgs e)
        {
            if (sender is Button button)
            {
                ButtonMouseHover = false;
                button.Invalidate();
            }
        }

        private static void Button_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is Button button)
            {
                Rectangle rect = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                
                SolidBrush sbBG;
                Pen penb;
                
                if (button.Enabled)
                {
                    sbBG = new(Colors.BackColor);
                    
                    if (button.PointToScreen(Point.Empty).X <= Cursor.Position.X
                            && Cursor.Position.X <= (button.PointToScreen(Point.Empty).X + rect.Width)
                            && button.PointToScreen(Point.Empty).Y <= Cursor.Position.Y
                            && Cursor.Position.Y <= (button.PointToScreen(Point.Empty).Y + rect.Height))
                    {
                        if (ButtonMouseHover)
                        {
                            sbBG = new(Colors.BackColorMouseHover);

                            if (ButtonMouseDown)
                            {
                                sbBG = new(Colors.BackColorMouseDown);
                            }
                        }
                    }

                    penb = new(Colors.Border);
                    button.ForeColor = Colors.ForeColor;
                }
                else
                {
                    sbBG = new(Colors.BackColorDisabled);
                    penb = new(Colors.BorderDisabled);
                    button.ForeColor = Colors.ForeColorDisabled;
                }

                // Draw Button Background
                e.Graphics.FillRectangle(sbBG, rect);
                sbBG.Dispose();

                if (button.Enabled && button.Focused)
                {
                    rect.Inflate(-2, -2);
                    using Pen pen = new(Colors.SelectionRectangle) { DashStyle = DashStyle.Dot };
                    e.Graphics.DrawRectangle(pen, rect);
                    rect.Inflate(+2, +2);
                }

                // Draw Button Border
                e.Graphics.DrawRectangle(penb, rect);
                penb.Dispose();

                // Draw Button Text
                TextFormatFlags flags = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter;
                TextRenderer.DrawText(e.Graphics, button.Text, button.Font, rect, button.ForeColor, flags);
                
            }
        }
        //==============================================================================
        private static void CheckBox_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is CheckBox checkBox)
            {
                BackColor = Colors.BackColor;
                Color BorderColor;
                Color TickColor;
                if (checkBox.Enabled)
                {
                    BackColor = Colors.BackColor;
                    ForeColor = Colors.ForeColor;
                    BorderColor = Colors.Border;
                    TickColor = Colors.Tick;
                }
                else
                {
                    if (checkBox.Parent != null)
                    {
                        if (checkBox.Parent.Enabled == false)
                            BackColor = Colors.BackColorDisabled;
                    }
                    ForeColor = Colors.ForeColorDisabled;
                    BorderColor = Colors.BorderDisabled;
                    TickColor = Colors.TickDisabled;
                }

                e.Graphics.Clear(BackColor);
                checkBox.Appearance = Appearance.Button;
                checkBox.FlatStyle = FlatStyle.Flat;
                checkBox.TextAlign = ContentAlignment.MiddleLeft;
                checkBox.FlatAppearance.BorderSize = 0;
                checkBox.AutoSize = false;
                checkBox.UseVisualStyleBackColor = false;
                SizeF sizeF = checkBox.CreateGraphics().MeasureString(checkBox.Text, checkBox.Font);
                checkBox.Height = (int)sizeF.Height;
                checkBox.Width = (int)(sizeF.Width + sizeF.Height * 1.2);
                int rectSize = (int)(sizeF.Height / 1.4);
                int x;
                float textX;

                if (checkBox.RightToLeft == RightToLeft.No)
                {
                    checkBox.TextAlign = ContentAlignment.MiddleLeft;
                    x = 1;
                    textX = (float)(rectSize * 1.3);
                }
                else
                {
                    checkBox.TextAlign = ContentAlignment.MiddleRight;
                    x = checkBox.Width - rectSize - 2;
                    textX = checkBox.Width - sizeF.Width - (float)(rectSize * 1.2);
                }

                int y = rectSize / 5;
                Point pt = new(x, y);
                Rectangle rectCheck = new(pt, new Size(rectSize, rectSize));

                // Draw Selection Border
                Rectangle cRect = new(checkBox.ClientRectangle.X, checkBox.ClientRectangle.Y, checkBox.ClientRectangle.Width - 1, checkBox.ClientRectangle.Height - 1);
                if (checkBox.Focused)
                {
                    //cRect.Inflate(-1, -1);
                    using Pen pen = new(Colors.SelectionRectangle) { DashStyle = DashStyle.Dot };
                    e.Graphics.DrawRectangle(pen, cRect);
                }

                // Draw Text
                using SolidBrush brush1 = new(ForeColor);
                e.Graphics.DrawString(checkBox.Text, checkBox.Font, brush1, textX, 0);

                // Fill Check Rect
                using SolidBrush brush2 = new(BackColor);
                e.Graphics.FillRectangle(brush2, rectCheck);

                // Draw Check
                if (checkBox.Checked)
                {
                    if (checkBox.CheckState == CheckState.Checked)
                    {
                        // Draw Check Using Font
                        //using SolidBrush brush3 = new(TickColor);
                        //using Font wing = new("Wingdings", rectSize - 1);
                        //e.Graphics.DrawString("ü", wing, brush3, x - 2, y);

                        // Draw Check
                        using Pen p = new(TickColor, 2);
                        rectCheck.Inflate(-2, -2);
                        e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                        e.Graphics.DrawLines(p, new Point[] { new Point(rectCheck.Left, rectCheck.Bottom - rectCheck.Height / 2), new Point(rectCheck.Left + rectCheck.Width / 3, rectCheck.Bottom), new Point(rectCheck.Right, rectCheck.Top) });
                        e.Graphics.SmoothingMode = SmoothingMode.Default;
                        rectCheck.Inflate(+2, +2);
                    }
                    else if (checkBox.CheckState == CheckState.Indeterminate)
                    {
                        // Draw Indeterminate
                        using SolidBrush sb = new(TickColor);
                        rectCheck.Inflate(-2, -2);
                        e.Graphics.FillRectangle(sb, rectCheck);
                        rectCheck.Inflate(+2, +2);
                    }
                }

                // Draw Check Rect (Check Border)
                ControlPaint.DrawBorder(e.Graphics, rectCheck, BorderColor, ButtonBorderStyle.Solid);
            }
        }
        //==============================================================================
        private static void RadioButton_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is RadioButton radioButton && !radioButton.Enabled)
            {
                var radioButtonWidth = RadioButtonRenderer.GetGlyphSize(e.Graphics, System.Windows.Forms.VisualStyles.RadioButtonState.UncheckedDisabled).Width;
                Rectangle textRectangleValue = new()
                {
                    X = e.ClipRectangle.X + radioButtonWidth,
                    Y = e.ClipRectangle.Y,
                    Width = e.ClipRectangle.X + e.ClipRectangle.Width - radioButtonWidth,
                    Height = e.ClipRectangle.Height
                };
                TextRenderer.DrawText(e.Graphics, radioButton.Text, radioButton.Font, textRectangleValue, radioButton.ForeColor, TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter);
            }
        }
        //==============================================================================
        private static void Panel_Paint(object? sender, PaintEventArgs e)
        {
            var p = sender as Panel;

            Rectangle rect = e.ClipRectangle;
            //rect = new(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);

            Color borderColor = Colors.Border;
            if (p.Enabled == false)
            {
                borderColor = Colors.BorderDisabled;
            }

            // Draw Border
            ControlPaint.DrawBorder(e.Graphics, rect, borderColor, ButtonBorderStyle.None);
        }

        private static void Panel_HandleCreated(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            int totalTime = 500;
            int elapsedTime = 0;
            var t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (s, e) =>
            {
                p.Invalidate();
                elapsedTime += t.Interval;
                if (elapsedTime > totalTime)
                    t.Stop();
            };
            t.Start();
        }

        private static void Panel_EnabledChanged(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_ControlAdded(object? sender, ControlEventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_ControlRemoved(object? sender, ControlEventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_Enter(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_MouseEnter(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_ParentChanged(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_Resize(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_Scroll(object? sender, ScrollEventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }

        private static void Panel_SizeChanged(object? sender, EventArgs e)
        {
            var p = sender as Panel;
            p.Invalidate();
        }
        //==============================================================================
        private static void TabControl_Paint(object? sender, PaintEventArgs e)
        {
            new TabControlRenderer(sender as TabControl, e).Paint();
        }
        //==============================================================================
        private static void ListView_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            // do nothing
        }

        private static void ListView_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
        {
            var lv = sender as ListView;
            e.Graphics.SmoothingMode = SmoothingMode.HighSpeed;
            // Draw GridLines
            e.Graphics.DrawRectangle(new Pen(Colors.GridLines), e.Bounds);

            if (e.Item.Selected)
            {
                if (lv.Focused)
                {
                    using var sb = new SolidBrush(Colors.Selection);
                    e.Graphics.FillRectangle(sb, e.Bounds);
                }
                else
                {
                    using var sb = new SolidBrush(Colors.SelectionUnfocused);
                    e.Graphics.FillRectangle(sb, e.Bounds);
                }
            }

            int addX = 0; // for later use
            if (lv.CheckBoxes)
            {
                if (e.ColumnIndex == 0)
                {
                    int columnMinWidth = 17;
                    if (lv.Columns[e.ColumnIndex].Width < columnMinWidth)
                    {
                        lv.Columns[e.ColumnIndex].Width = columnMinWidth;
                    }

                    addX = 16;
                    int rectCheckSize = 12;
                    int x = e.Bounds.X + 2;
                    int y = e.Bounds.Y + (e.Bounds.Height / 2) - (rectCheckSize / 2);
                    Point pt = new(x, y);
                    Rectangle rectCheck = new(pt, new Size(rectCheckSize, rectCheckSize));
                    // Draw Check Rect
                    using SolidBrush brush2 = new(BackColor);
                    e.Graphics.DrawRectangle(new Pen(Colors.Border), rectCheck);

                    if (e.Item.Checked)
                    {
                        // Draw Check
                        using SolidBrush brush3 = new(Colors.Tick);
                        using Font wing = new("Wingdings", rectCheckSize - 1);
                        //e.Graphics.DrawString("ü", wing, brush3, x - 2, y); // Slower
                        TextRenderer.DrawText(e.Graphics, "ü", wing, new Point(x - 2, y), Colors.Tick); // Faster
                    }

                    // Draw Text on first cell
                    var subtitleFont = e.Item.Font;
                    string text = e.Item.SubItems[e.ColumnIndex].Text;
                    if (!string.IsNullOrWhiteSpace(text))
                    {
                        int columnWidth = lv.Columns[e.ColumnIndex].Width - addX;
                        var stringWidth = (int)e.Graphics.MeasureString(e.Item.SubItems[e.ColumnIndex].Text, subtitleFont).Width;
                        if (lv.Columns[e.ColumnIndex].TextAlign == HorizontalAlignment.Right)
                        {
                            TextRenderer.DrawText(e.Graphics, text.Truncate(subtitleFont, columnWidth, true), subtitleFont, new Point(e.Bounds.Right - stringWidth - 7, e.Bounds.Top + 2), e.Item.ForeColor, TextFormatFlags.NoPrefix);
                        }
                        else
                        {
                            TextRenderer.DrawText(e.Graphics, text.Truncate(subtitleFont, columnWidth, false), subtitleFont, new Point(e.Bounds.Left + 3 + addX, e.Bounds.Top + 2), e.Item.ForeColor, TextFormatFlags.NoPrefix);
                        }
                    }
                }
                else
                {
                    e.DrawText(TextFormatFlags.NoPrefix);
                }
            }
            else
            {
                e.DrawText(TextFormatFlags.NoPrefix);
            }
        }

        private static void ListView_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
        {
            e.DrawDefault = false;

            if (sender is ListView lv && lv.RightToLeftLayout)
            {
                TextRenderer.DrawText(e.Graphics, $" {e.Header.Text}", e.Font, e.Bounds, ForeColor, TextFormatFlags.Left);
                return;
            }

            using (var slightlyDarkerBrush = new SolidBrush(Color.FromArgb(Math.Max(BackColor.R - 9, 0), Math.Max(BackColor.G - 9, 0), Math.Max(BackColor.B - 9, 0))))
            {
                e.Graphics.FillRectangle(slightlyDarkerBrush, e.Bounds);
            }

            int posY = Math.Abs(e.Bounds.Height - e.Font.Height) / 2;
            TextRenderer.DrawText(e.Graphics, e.Header.Text, e.Font, new Point(e.Bounds.X + 3, posY), ForeColor);

            // ListView Columns Header GridLines
            if (e.ColumnIndex != 0)
            {
                using var foreColorPen = new Pen(Colors.GridLines);
                e.Graphics.DrawLine(foreColorPen, e.Bounds.X, e.Bounds.Y, e.Bounds.X, e.Bounds.Height);
            }
        }

        // A hack to set the backcolor of a disabled ListView
        private static void ListView_EnabledChanged(object? sender, EventArgs e)
        {
            var listView = sender as ListView;
            if (!listView.Enabled)
            {
                Bitmap disabledBackgroundImage = new(listView.ClientSize.Width, listView.ClientSize.Height);
                Graphics.FromImage(disabledBackgroundImage).Clear(Colors.BackColorDisabled);
                listView.BackgroundImage = disabledBackgroundImage;
                listView.BackgroundImageTiled = true;
            }
            else if (listView.BackgroundImage != null)
            {
                listView.BackgroundImage = null;
            }
        }

        private static void ListView_HandleCreated(object? sender, EventArgs e)
        {
            SetWindowThemeDark((Control)sender);
            var lv = sender as ListView;
            lv.GridLines = false;
        }
        //==============================================================================

        private static void DataGridView_RowsAdded(object? sender, DataGridViewRowsAddedEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private static void DataGridView_MouseMove(object? sender, MouseEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private static void DataGridView_Scroll(object? sender, ScrollEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private static void DataGridView_GotFocus(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            DataGridViewColor(gv);
        }

        private static void DataGridView_LostFocus(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            DataGridViewColor(gv);
        }

        private static void DataGridView_EnabledChanged(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            DataGridViewColor(gv);
        }

        private static void DataGridView_HandleCreated(object? sender, EventArgs e)
        {
            if (sender is null || e is null)
                return;
            SetWindowThemeDark((Control)sender); // Doesn't work
            var gv = sender as DataGridView;
            DataGridViewColor(gv);
        }

        private static void DataGridViewColor(DataGridView? gv)
        {
            if (gv.Enabled == true)
            {
                gv.BackgroundColor = Colors.BackColor;
                gv.ForeColor = Colors.ForeColor;
                gv.GridColor = Colors.GridLines;
                gv.EnableHeadersVisualStyles = false;
                gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                gv.ColumnHeadersDefaultCellStyle.BackColor = Colors.BackColorDarker;
                gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.BackColorDarker;
                gv.ColumnHeadersDefaultCellStyle.ForeColor = Colors.ForeColor;
                gv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Colors.ForeColor;
                gv.DefaultCellStyle.BackColor = Colors.BackColor;
                gv.DefaultCellStyle.ForeColor = Colors.ForeColor;
                if (gv.Focused)
                    gv.DefaultCellStyle.SelectionBackColor = Colors.Selection;
                else
                    gv.DefaultCellStyle.SelectionBackColor = Colors.SelectionUnfocused;
                gv.DefaultCellStyle.SelectionForeColor = Colors.ForeColor;
            }
            else
            {
                gv.BackgroundColor = Colors.BackColorDisabled;
                gv.ForeColor = Colors.ForeColorDisabled;
                gv.GridColor = Colors.GridLinesDisabled;
                gv.EnableHeadersVisualStyles = false;
                gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
                gv.ColumnHeadersDefaultCellStyle.BackColor = Colors.BackColorDarkerDisabled;
                gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = Colors.BackColorDarkerDisabled;
                gv.ColumnHeadersDefaultCellStyle.ForeColor = Colors.ForeColorDisabled;
                gv.ColumnHeadersDefaultCellStyle.SelectionForeColor = Colors.ForeColorDisabled;
                gv.DefaultCellStyle.BackColor = Colors.BackColorDisabled;
                gv.DefaultCellStyle.ForeColor = Colors.ForeColorDisabled;
                gv.DefaultCellStyle.SelectionBackColor = Colors.SelectionUnfocused;
                gv.DefaultCellStyle.SelectionForeColor = Colors.ForeColorDisabled;
            }

            // Or
            //for (int rr = 0; rr < gv.Rows.Count; rr++)
            //{
            //    DataGridViewRow row = gv.Rows[rr];
            //    for (int cc = 0; cc < gv.Columns.Count; cc++)
            //    {
            //        DataGridViewColumn col = gv.Columns[cc];
            //        row.Cells[col.Index].Style.BackColor = Color.Green;
            //        // Or
            //        gv[col.Index, row.Index].Style.BackColor = Colors.BackColor;
            //        // Or
            //        gv.Rows[rr].Cells[cc].Style.BackColor = Colors.BackColor;
            //    }
            //}
        }

        private static void DataGridView_CellClick(object? sender, DataGridViewCellEventArgs e)
        {
            var gv = sender as DataGridView;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = gv[e.ColumnIndex, e.RowIndex];
                if (cell.GetType().ToString().Contains("CheckBox", StringComparison.OrdinalIgnoreCase))
                {
                    bool cellValue = Convert.ToBoolean(cell.Value);
                    if (cellValue == true)
                    {
                        cell.Value = false;
                        gv.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                        gv.EndEdit();
                        gv.InvalidateCell(cell);
                    }
                    else
                    {
                        cell.Value = true;
                        gv.UpdateCellValue(e.ColumnIndex, e.RowIndex);
                        gv.EndEdit();
                        gv.InvalidateCell(cell);
                    }
                }
            }
        }

        private static void DataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            var gv = sender as DataGridView;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = gv[e.ColumnIndex, e.RowIndex];
                if (cell.GetType().ToString().Contains("CheckBox", StringComparison.OrdinalIgnoreCase))
                {
                    Rectangle rectCell = gv.GetCellDisplayRectangle(e.ColumnIndex, e.RowIndex, true);

                    e.Handled = true;
                    e.PaintBackground(rectCell, true);

                    rectCell = new(rectCell.X, rectCell.Y, rectCell.Width - 2, rectCell.Height - 2);
                    int checkSize = 13;
                    checkSize = Math.Min(checkSize, rectCell.Width);
                    checkSize = Math.Min(checkSize, rectCell.Height);
                    int centerX = rectCell.X + rectCell.Width / 2 - checkSize / 2;
                    int centerY = rectCell.Y + rectCell.Height / 2 - checkSize / 2;

                    rectCell = new(centerX, centerY, checkSize, checkSize);

                    BackColor = Colors.BackColor;
                    Color borderColor = Colors.Border;
                    Color tickColor = Colors.Tick;
                    if (gv.Enabled == false)
                    {
                        BackColor = Colors.BackColorDisabled;
                        borderColor = Colors.BorderDisabled;
                        tickColor = Colors.TickDisabled;
                    }

                    // Fill Check Rect
                    using SolidBrush brush1 = new(BackColor);
                    e.Graphics.FillRectangle(brush1, rectCell);

                    // Draw Check Rect
                    using Pen pen1 = new(borderColor);
                    e.Graphics.DrawRectangle(pen1, rectCell);

                    // Draw Check
                    if (Convert.ToBoolean(cell.Value) == true)
                    {
                        //using SolidBrush brush2 = new(tickColor);
                        int rectCheck = rectCell.Height - 1;
                        if (rectCheck <= 0)
                            rectCheck = 1;
                        //using Font wing = new("Wingdings", rectCheck);
                        //e.Graphics.DrawString("ü", wing, brush2, rectCell.X - 3, rectCell.Y);

                        if (rectCheck > 1)
                        {
                            // Draw Check
                            using var p = new Pen(tickColor, 2);
                            rectCell.Inflate(-2, -2);
                            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                            e.Graphics.DrawLines(p, new Point[] { new Point(rectCell.Left, rectCell.Bottom - rectCheck / 2), new Point(rectCell.Left + rectCell.Width / 3, rectCell.Bottom), new Point(rectCell.Right, rectCell.Top) });
                            e.Graphics.SmoothingMode = SmoothingMode.Default;
                            rectCell.Inflate(+2, +2);
                        }
                    }
                }
            }
        }

        private static void DataGridView_Paint(object? sender, PaintEventArgs e)
        {
            var gv = sender as DataGridView;

            Rectangle rectGv = e.ClipRectangle;
            rectGv = new(rectGv.X, rectGv.Y, rectGv.Width - 1, rectGv.Height - 1);

            Color borderColor = Colors.Border;
            if (gv.Enabled == false)
            {
                borderColor = Colors.BorderDisabled;
            }

            // Draw Border
            using Pen pen1 = new(borderColor);
            e.Graphics.DrawRectangle(pen1, rectGv);
        }
        //==============================================================================
        /// <summary>
        /// A dark theme for TabControl.
        /// </summary>
        private class TabControlRenderer
        {
            private readonly Point _mouseCursor;
            private readonly Graphics _graphics;
            private readonly Rectangle _clipRectangle;
            private readonly int _selectedIndex;
            private readonly int _tabCount;
            private readonly Size _imageSize;
            private readonly Font _font;
            private readonly bool _enabled;
            private readonly Image[] _tabImages;
            private readonly Rectangle[] _tabRects;
            private readonly string[] _tabTexts;
            private readonly Size _size;
            private readonly bool _failed;

            private static readonly Color _selectedTabColor = Color.FromArgb(0, 122, 204);
            private static readonly Color _highlightedTabColor = Color.FromArgb(28, 151, 234);

            private static readonly int ImagePadding = 6;
            private static readonly int SelectedTabPadding = 2;
            private static readonly int BorderWidth = 1;

            public TabControlRenderer(TabControl? tabs, PaintEventArgs e)
            {
                _mouseCursor = tabs.PointToClient(Cursor.Position);
                _graphics = e.Graphics;
                _clipRectangle = e.ClipRectangle;
                _size = tabs.Size;
                _selectedIndex = tabs.SelectedIndex;
                _tabCount = tabs.TabCount;
                _font = tabs.Font;
                _imageSize = tabs.ImageList?.ImageSize ?? Size.Empty;
                _enabled = tabs.Enabled;

                try
                {
                    _tabTexts = Enumerable.Range(0, _tabCount)
                        .Select(i => tabs.TabPages[i].Text)
                        .ToArray();
                    _tabImages = Enumerable.Range(0, _tabCount)
                        .Select(i => GetTabImage(tabs, i))
                        .ToArray();
                    _tabRects = Enumerable.Range(0, _tabCount)
                        .Select(tabs.GetTabRect)
                        .ToArray();
                }
                catch (ArgumentOutOfRangeException)
                {
                    _failed = true;
                }
            }

            public void Paint()
            {
                if (_failed)
                {
                    return;
                }

                using (var canvasBrush = new SolidBrush(BackColor))
                {
                    _graphics.FillRectangle(canvasBrush, _clipRectangle);
                }

                RenderSelectedPageBackground();

                IEnumerable<int> pageIndices;
                if (_selectedIndex >= 0 && _selectedIndex < _tabCount)
                {
                    // Render tabs in pyramid order with selected on top
                    pageIndices = Enumerable.Range(0, _selectedIndex)
                        .Concat(Enumerable.Range(_selectedIndex, _tabCount - _selectedIndex).Reverse());
                }
                else
                {
                    pageIndices = Enumerable.Range(0, _tabCount);
                }

                foreach (var index in pageIndices)
                {
                    RenderTabBackground(index);
                    RenderTabImage(index);
                    RenderTabText(index, _tabImages[index] != null);
                }
            }

            private void RenderTabBackground(int index)
            {
                var outerRect = GetOuterTabRect(index);
                using (var sb = GetBackgroundBrush(index))
                {
                    _graphics.FillRectangle(sb, outerRect);
                }

                var points = new List<Point>(4);
                if (index <= _selectedIndex)
                {
                    points.Add(new Point(outerRect.Left, outerRect.Bottom - 1));
                }

                points.Add(new Point(outerRect.Left, outerRect.Top));
                points.Add(new Point(outerRect.Right - 1, outerRect.Top));

                if (index >= _selectedIndex)
                {
                    points.Add(new Point(outerRect.Right - 1, outerRect.Bottom - 1));
                }

                using var borderPen = GetBorderPen();
                _graphics.DrawLines(borderPen, points.ToArray());
            }

            private void RenderTabImage(int index)
            {
                var image = _tabImages[index];
                if (image is null)
                {
                    return;
                }

                var imgRect = GetTabImageRect(index);
                _graphics.DrawImage(image, imgRect);
            }

            private Rectangle GetTabImageRect(int index)
            {
                var innerRect = _tabRects[index];
                int imgHeight = _imageSize.Height;
                var imgRect = new Rectangle(
                    new Point(innerRect.X + ImagePadding,
                        innerRect.Y + ((innerRect.Height - imgHeight) / 2)),
                    _imageSize);

                if (index == _selectedIndex)
                {
                    imgRect.Offset(0, -SelectedTabPadding);
                }

                return imgRect;
            }

            private static Image GetTabImage(TabControl tabs, int index)
            {
                var images = tabs.ImageList?.Images;
                if (images is null)
                {
                    return null;
                }

                var page = tabs.TabPages[index];
                if (!string.IsNullOrEmpty(page.ImageKey))
                {
                    return images[page.ImageKey];
                }

                if (page.ImageIndex >= 0 && page.ImageIndex < images.Count)
                {
                    return images[page.ImageIndex];
                }

                return null;
            }

            private void RenderTabText(int index, bool hasImage)
            {
                if (string.IsNullOrEmpty(_tabTexts[index]))
                {
                    return;
                }

                var textRect = GetTabTextRect(index, hasImage);

                const TextFormatFlags format =
                    TextFormatFlags.NoClipping |
                    TextFormatFlags.NoPrefix |
                    TextFormatFlags.VerticalCenter |
                    TextFormatFlags.HorizontalCenter;

                var textColor = _enabled
                    ? ForeColor
                    : Colors.ForeColorDisabled;

                TextRenderer.DrawText(_graphics, _tabTexts[index], _font, textRect, textColor, format);
            }

            private Rectangle GetTabTextRect(int index, bool hasImage)
            {
                var innerRect = _tabRects[index];
                Rectangle textRect;
                if (hasImage)
                {
                    int deltaWidth = _imageSize.Width + ImagePadding;
                    textRect = new Rectangle(
                        innerRect.X + deltaWidth,
                        innerRect.Y,
                        innerRect.Width - deltaWidth,
                        innerRect.Height);
                }
                else
                {
                    textRect = innerRect;
                }

                if (index == _selectedIndex)
                {
                    textRect.Offset(0, -SelectedTabPadding);
                }

                return textRect;
            }

            private Rectangle GetOuterTabRect(int index)
            {
                var innerRect = _tabRects[index];

                if (index == _selectedIndex)
                {
                    return Rectangle.FromLTRB(
                        innerRect.Left - SelectedTabPadding,
                        innerRect.Top - SelectedTabPadding,
                        innerRect.Right + SelectedTabPadding,
                        innerRect.Bottom + 1); // +1 to overlap tabs bottom line
                }

                return Rectangle.FromLTRB(
                    innerRect.Left,
                    innerRect.Top + 1,
                    innerRect.Right,
                    innerRect.Bottom);
            }

            private void RenderSelectedPageBackground()
            {
                if (_selectedIndex < 0 || _selectedIndex >= _tabCount)
                {
                    return;
                }

                var tabRect = _tabRects[_selectedIndex];
                var pageRect = Rectangle.FromLTRB(0, tabRect.Bottom, _size.Width - 1,
                    _size.Height - 1);

                if (!_clipRectangle.IntersectsWith(pageRect))
                {
                    return;
                }

                using var borderPen = GetBorderPen();
                _graphics.DrawRectangle(borderPen, pageRect);
            }

            private Brush GetBackgroundBrush(int index)
            {
                if (index == _selectedIndex)
                {
                    return new SolidBrush(Colors.Selection);
                }

                bool isHighlighted = _tabRects[index].Contains(_mouseCursor);
                return isHighlighted
                    ? new SolidBrush(Colors.SelectionUnfocused)
                    : new SolidBrush(BackColor);
            }

            private static Pen GetBorderPen() =>
                new(Colors.Border, BorderWidth);
        }

        private static void SetStyle(Control control, ControlStyles styles, bool value) =>
            typeof(TabControl).GetMethod("SetStyle", BindingFlags.Instance | BindingFlags.NonPublic)
            .Invoke(control, new object[] { styles, value });

        internal static void SetDarkTheme(ToolStripItem item)
        {
            item.BackColor = BackColor;
            item.ForeColor = ForeColor;
            
            if (item is ToolStripDropDownItem dropDownMenu && dropDownMenu.DropDownItems.Count > 0)
            {
                foreach (ToolStripItem dropDownItem in dropDownMenu.DropDownItems)
                {
                    dropDownItem.ForeColor = ForeColor;
                    dropDownItem.BackColor = BackColor;
                }
            }
        }

    }
}
