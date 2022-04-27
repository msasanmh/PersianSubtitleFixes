using MsmhTools;
using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace CustomControls
{
    public class CustomListView : ListView
    {
        public CustomListView() : base()
        {
            OwnerDraw = true;
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
            DrawItem += ListView_DrawItem;
            DrawSubItem += ListView_DrawSubItem;
            DrawColumnHeader += ListView_DrawColumnHeader;
            EnabledChanged += ListView_EnabledChanged;
            HandleCreated += ListView_HandleCreated;
        }
        private static void ListView_DrawItem(object? sender, DrawListViewItemEventArgs e)
        {
            // do nothing
        }

        private void ListView_DrawSubItem(object? sender, DrawListViewSubItemEventArgs e)
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

        private void ListView_DrawColumnHeader(object? sender, DrawListViewColumnHeaderEventArgs e)
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
            var control = (Control)sender;
            _ = NativeMethods.SetWindowTheme(control.Handle, "DarkMode_Explorer", null);
            var lv = sender as ListView;
            lv.GridLines = false;
        }
    }
}
