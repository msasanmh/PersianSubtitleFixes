using System;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Text;
using System.Windows.Forms.Design;
using MsmhTools;
/*
 * Copyright MSasanMH, April 16, 2022.
 */

namespace CustomControls
{
    public class CustomComboBox : ComboBox
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new FlatStyle FlatStyle { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new ComboBoxStyle DropDownStyle { get; set; }

        private Color mBackColor = Color.DimGray;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Back Color")]
        public override Color BackColor
        {
            get { return mBackColor; }
            set
            {
                if (mBackColor != value)
                {
                    mBackColor = value;
                    Invalidate();
                }
            }
        }

        private Color mForeColor = Color.White;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Fore Color")]
        public override Color ForeColor
        {
            get { return mForeColor; }
            set
            {
                if (mForeColor != value)
                {
                    mForeColor = value;
                    Invalidate();
                }
            }
        }

        private Color mBorderColor = Color.Red;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Border Color")]
        public Color BorderColor
        {
            get { return mBorderColor; }
            set
            {
                if (mBorderColor != value)
                {
                    mBorderColor = value;
                    Invalidate();
                }
            }
        }

        private Color mSelectionColor = Color.Blue;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Selection Color")]
        public Color SelectionColor
        {
            get { return mSelectionColor; }
            set
            {
                if (mSelectionColor != value)
                {
                    mSelectionColor = value;
                    Invalidate();
                }
            }
        }

        private static Color[]? OriginalColors;
        private static Color BackColorDarker { get; set; }

        private static Color BackColorDisabled;
        private static Color ForeColorDisabled;
        private static Color BorderColorDisabled;
        private static Color BackColorDarkerDisabled;

        private static readonly int MyPadding = 10;
        private static bool ApplicationIdle = false;
        private bool once = true;

        public CustomComboBox() : base()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            DrawMode = DrawMode.OwnerDrawVariable;

            base.FlatStyle = FlatStyle.Flat;
            base.DropDownStyle = ComboBoxStyle.DropDownList;
            Application.Idle += Application_Idle;
            HandleCreated += CustomComboBox_HandleCreated;
            MouseMove += CustomComboBox_MouseMove;
            LocationChanged += CustomComboBox_LocationChanged;
            Move += CustomComboBox_Move;
            EnabledChanged += CustomComboBox_EnabledChanged;
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            ApplicationIdle = true;
            if (Parent != null)
            {
                if (once == true)
                {
                    Parent.Move -= Parent_Move;
                    Parent.Move += Parent_Move;
                    Invalidate();
                    once = false;
                }
            }
        }

        private void Parent_Move(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void CustomComboBox_HandleCreated(object? sender, EventArgs e)
        {
            BackColorDarker = mBackColor.ChangeBrightness(-0.3f);
            OriginalColors = new Color[] { mBackColor, mForeColor, mBorderColor, mSelectionColor, BackColorDarker };
            Invalidate();
        }

        private void CustomComboBox_MouseMove(object? sender, MouseEventArgs e)
        {
            Invalidate();
        }

        private void CustomComboBox_LocationChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void CustomComboBox_Move(object? sender, EventArgs e)
        {
            Invalidate();
        }

        private void CustomComboBox_EnabledChanged(object? sender, EventArgs e)
        {
            Invalidate();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
        }

        protected override void OnTabStopChanged(EventArgs e)
        {
            base.OnTabStopChanged(e);
            Invalidate();
        }

        protected override void OnTabIndexChanged(EventArgs e)
        {
            base.OnTabIndexChanged(e);
            Invalidate();
        }

        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            Invalidate();
        }

        protected override void OnLostFocus(EventArgs e)
        {
            base.OnLostFocus(e);
            Invalidate();
        }

        protected override void OnTextChanged(EventArgs e)
        {
            base.OnTextChanged(e);
            Invalidate();
        }

        protected override void OnTextUpdate(EventArgs e)
        {
            base.OnTextUpdate(e);
            Invalidate();
        }

        protected override void OnSelectedValueChanged(EventArgs e)
        {
            base.OnSelectedValueChanged(e);
            Invalidate();
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            Invalidate();
        }

        protected override void OnInvalidated(InvalidateEventArgs e)
        {
            base.OnInvalidated(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            Invalidate();
        }

        // Item's Box Border
        //protected override void OnDropDown(EventArgs e)
        //{
        //    if (_buffer == null)
        //        _buffer = new Bitmap(DropDownWidth, DropDownHeight);

        //    using var g = Graphics.FromImage(_buffer);
        //    // Draw DropDownList Border
        //    Rectangle modRect2 = new(0, 10, DropDownWidth, DropDownHeight);
        //    g.DrawRectangle(new Pen(Color.Red), modRect2);
        //}

        protected override void OnPaint(PaintEventArgs e)
        {
            // Update Colors
            BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            OriginalColors = new Color[] { BackColor, ForeColor, BorderColor, SelectionColor, BackColorDarker };

            if (ApplicationIdle == false)
                return;

            if (DesignMode)
            {
                BackColor = mBackColor;
                ForeColor = mForeColor;
                BorderColor = mBorderColor;
                SelectionColor = mSelectionColor;
            }
            else
            {
                if (OriginalColors != null)
                {
                    if (Enabled == true)
                    {
                        BackColor = OriginalColors[0];
                        ForeColor = OriginalColors[1];
                        BorderColor = OriginalColors[2];
                        SelectionColor = OriginalColors[3];
                        BackColorDarker = OriginalColors[4];
                    }
                    else
                    {
                        // Disabled Colors
                        if (OriginalColors[0].DarkOrLight() == "Dark")
                            BackColorDisabled = OriginalColors[0].ChangeBrightness(0.3f);
                        else if (OriginalColors[0].DarkOrLight() == "Light")
                            BackColorDisabled = OriginalColors[0].ChangeBrightness(-0.3f);

                        if (OriginalColors[1].DarkOrLight() == "Dark")
                            ForeColorDisabled = OriginalColors[1].ChangeBrightness(0.2f);
                        else if (OriginalColors[1].DarkOrLight() == "Light")
                            ForeColorDisabled = OriginalColors[1].ChangeBrightness(-0.2f);

                        if (OriginalColors[2].DarkOrLight() == "Dark")
                            BorderColorDisabled = OriginalColors[2].ChangeBrightness(0.3f);
                        else if (OriginalColors[2].DarkOrLight() == "Light")
                            BorderColorDisabled = OriginalColors[2].ChangeBrightness(-0.3f);

                        if (OriginalColors[4].DarkOrLight() == "Dark")
                            BackColorDarkerDisabled = OriginalColors[4].ChangeBrightness(0.3f);
                        else if (OriginalColors[4].DarkOrLight() == "Light")
                            BackColorDarkerDisabled = OriginalColors[4].ChangeBrightness(-0.3f);
                    }
                }
            }

            var g = e.Graphics;
            Rectangle rect = new(0, 0, ClientSize.Width, ClientSize.Height);

            Color backColor;
            Color foreColor;
            Color borderColor;
            Color backColorDarker;

            if (Enabled == true)
            {
                backColor = BackColor;
                foreColor = ForeColor;
                borderColor = BorderColor;
                backColorDarker = BackColorDarker;
            }
            else
            {
                backColor = BackColorDisabled;
                foreColor = ForeColorDisabled;
                borderColor = BorderColorDisabled;
                backColorDarker = BackColorDarkerDisabled;
            }

            // Selected Border Color
            if (Focused && TabStop)
                borderColor = BorderColor;

            // Fill Background
            using SolidBrush sb = new(backColor);
            g.FillRectangle(sb, rect);

            // Draw Border
            using Pen p = new(borderColor, 1);
            Rectangle modRect1 = new(rect.Left, rect.Top, rect.Width - 1, rect.Height - 1);
            if (DesignMode || !DesignMode)
            {
                g.DrawRectangle(p, modRect1);
            }

            // Fill Arrow Button Back Color
            using SolidBrush sb2 = new(backColorDarker);
            int x = rect.Right - 15;
            int y = rect.Top + 1;
            int buttonWidth = rect.Width - x - 1;
            int buttonHeight = rect.Height - 2;
            Rectangle modRect2 = new(x, y, buttonWidth, buttonHeight);
            g.FillRectangle(sb2, modRect2);

            // Draw Arrow Button Icon
            var pth = new GraphicsPath();
            var TopLeft = new PointF(x + buttonWidth * 1 / 5, y + buttonHeight * 2 / 5);
            var TopRight = new PointF(x + buttonWidth * 4 / 5, y + buttonHeight * 2 / 5);
            var Bottom = new PointF(x + buttonWidth / 2, y + buttonHeight * 3 / 5);
            pth.AddLine(TopLeft, TopRight);
            pth.AddLine(TopRight, Bottom);
            // Determine the Arrow's Color.
            using SolidBrush arrowBrush = new(foreColor);
            // Draw the Arrow
            g.SmoothingMode = SmoothingMode.AntiAlias;
            g.FillPath(arrowBrush, pth);
            g.SmoothingMode = SmoothingMode.Default;

            var text = SelectedItem != null ? SelectedItem.ToString() : Text;

            using SolidBrush b = new(foreColor);
            int padding = 2;
            int arrowWidth = (int)(TopRight.X - TopLeft.X);
            Rectangle modRect3 = new(rect.Left + padding,
                                        rect.Top + padding,
                                        rect.Width - arrowWidth - (MyPadding / 2) - (padding * 2),
                                        rect.Height - (padding * 2));

            var stringFormat = new StringFormat
            {
                LineAlignment = StringAlignment.Center,
                Alignment = StringAlignment.Near,
                FormatFlags = StringFormatFlags.NoWrap,
                Trimming = StringTrimming.EllipsisCharacter
            };

            g.DrawString(text, Font, b, modRect3, stringFormat);
        }

        protected override void OnDrawItem(DrawItemEventArgs e)
        {
            Graphics g = e.Graphics;
            Rectangle rect = e.Bounds;

            Color textColor = ForeColor;
            Color fillColor = BackColor;

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected ||
                (e.State & DrawItemState.Focus) == DrawItemState.Focus)
                fillColor = SelectionColor;

            using SolidBrush sb = new(fillColor);
            g.FillRectangle(sb, rect);

            if (e.Index >= 0 && e.Index < Items.Count)
            {
                var text = Items[e.Index].ToString();

                using SolidBrush b = new(textColor);
                var padding = 2;

                Rectangle modRect = new(rect.Left + padding,
                    rect.Top + padding,
                    rect.Width - (padding * 2),
                    rect.Height - (padding * 2));

                var stringFormat = new StringFormat
                {
                    LineAlignment = StringAlignment.Center,
                    Alignment = StringAlignment.Near,
                    FormatFlags = StringFormatFlags.NoWrap,
                    Trimming = StringTrimming.EllipsisCharacter
                };

                g.DrawString(text, Font, b, modRect, stringFormat);
            }
        }
    }
}
