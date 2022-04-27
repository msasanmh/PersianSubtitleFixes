using System;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Windows.Forms.Design;
using System.Diagnostics;

namespace CustomControls
{
    [Designer(typeof(ScrollBarControlDesigner))]
    public class CustomVScrollBar : UserControl
    {
        protected int mLargeChange = 10;
        protected int mSmallChange = 1;
        protected int mMinimum = 0;
        protected int mMaximum = 100;
        protected int mValue = 0;
        protected int mNewValue = 0;
        private int nClickPoint;

        protected int mThumbTop = 0;

        protected bool mAutoSize = false;

        private bool mThumbDown = false;
        private bool mThumbDragging = false;
        private bool mDown = false;

        public new event EventHandler<EventArgs> Scroll = null;
        public event EventHandler? ValueChanged = null;

        private static readonly int ScrollBarWidth = 15;
        private static readonly int ThumbMinHeight = 20;
        private static Size UpArrow = new(ScrollBarWidth, ScrollBarWidth);
        private static Size Thumb = new(ScrollBarWidth, ThumbMinHeight);
        private static Size DownArrow = new(ScrollBarWidth, ScrollBarWidth);

        private static Rectangle rect;
        private static Rectangle rectUpArrow;
        private static Rectangle rectThumb;
        private static Rectangle rectDownArrow;
        private bool once = true;

        public CustomVScrollBar() : base()
        {
            InitializeComponent();
            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.AllPaintingInWmPaint, true);
            SetStyle(ControlStyles.DoubleBuffer, true);
            
            Width = ScrollBarWidth;
            base.MinimumSize = new Size(ScrollBarWidth, UpArrow.Height + Thumb.Height + DownArrow.Height);
            MouseMove += CustomVScrollBar_MouseMove;
            LocationChanged += CustomVScrollBar_LocationChanged;
            Move += CustomVScrollBar_Move;
            EnabledChanged += CustomScrollBar_EnabledChanged;
            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object? sender, EventArgs e)
        {
            if (Parent != null)
            {
                if (once == true)
                {
                    Parent.MouseWheel -= Parent_MouseWheel;
                    Parent.MouseWheel += Parent_MouseWheel;
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

        private void Parent_MouseWheel(object? sender, MouseEventArgs e)
        {
            CustomScrollBar_MouseWheel(sender, e);
        }

        private void CustomVScrollBar_MouseMove(object? sender, MouseEventArgs e)
        {
            var csb = sender as CustomVScrollBar;
            csb.Invalidate();
        }

        private void CustomVScrollBar_LocationChanged(object? sender, EventArgs e)
        {
            var csb = sender as CustomVScrollBar;
            csb.Invalidate();
        }

        private void CustomVScrollBar_Move(object? sender, EventArgs e)
        {
            var csb = sender as CustomVScrollBar;
            csb.Invalidate();
        }

        private void CustomScrollBar_EnabledChanged(object? sender, EventArgs e)
        {
            var csb = sender as CustomVScrollBar;
            csb.Invalidate();
        }

        private float GetThumbHeight()
        {
            int nTrackHeight = Height - (UpArrow.Height + DownArrow.Height);
            //float eachRowHeight = nTrackHeight / (float)LargeChange;
            float fThumbHeight = (float)LargeChange / Maximum * nTrackHeight;
            int nThumbHeight = (int)fThumbHeight;
            if (nThumbHeight < ThumbMinHeight)
            {
                nThumbHeight = ThumbMinHeight;
                fThumbHeight = (float)ThumbMinHeight;
            }
            else if (nThumbHeight > nTrackHeight)
            {
                nThumbHeight = nTrackHeight;
                fThumbHeight = (float)nTrackHeight;
            }
            Thumb.Height = nThumbHeight;
            return fThumbHeight;
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("LargeChange")]
        public int LargeChange
        {
            get { return mLargeChange; }
            set
            {
                mLargeChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("SmallChange")]
        public int SmallChange
        {
            get { return mSmallChange; }
            set
            {
                mSmallChange = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Minimum")]
        public int Minimum
        {
            get { return mMinimum; }
            set
            {
                mMinimum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Maximum")]
        public int Maximum
        {
            get { return mMaximum; }
            set
            {
                mMaximum = value;
                Invalidate();
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true), DefaultValue(false), Category("Behavior"), Description("Value")]
        public int Value
        {
            get
            {
                if (mValue < Minimum)
                    mValue = Minimum;
                else if (mValue > Maximum)
                    mValue = Maximum;
                return mValue;
            }
            set
            {
                mValue = value;

                int nTrackHeight = Height - (UpArrow.Height + DownArrow.Height);
                int nThumbHeight = (int)GetThumbHeight();

                //Compute Value
                int nPixelRange = nTrackHeight - nThumbHeight;
                int nRealRange = Maximum - Minimum - LargeChange;
                float fPerc = 0.0f;
                if (nRealRange != 0)
                {
                    fPerc = (float)mValue / nRealRange;
                }

                float fTop = fPerc * nPixelRange;
                mThumbTop = (int)fTop;

                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Thumb.Height = (int)GetThumbHeight();

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;

            Color BorderColor;
            if (Enabled)
            {
                BackColor = Colors.BackColor;
                ForeColor = Colors.ForeColor;
                BorderColor = Colors.Border;
            }
            else
            {
                BackColor = Colors.BackColorDisabled;
                ForeColor = Colors.ForeColorDisabled;
                BorderColor = Colors.BorderDisabled;
            }

            // Fill Background
            using Brush sb0 = new SolidBrush(BackColor);
            rect = new(0, 0, Width, Height);
            e.Graphics.FillRectangle(sb0, rect);

            // Draw Up Arrow BG
            using SolidBrush sbUpArrow = new(ForeColor);
            rectUpArrow = new(rect.X + 2, rect.Y, rect.Width - 4, UpArrow.Height);
            e.Graphics.FillRectangle(sbUpArrow, rectUpArrow);
            // Draw Spaces
            using Pen penUpArrow = new(BackColor);
            e.Graphics.DrawLine(penUpArrow, rectUpArrow.X, rectUpArrow.Y, rectUpArrow.X + rectUpArrow.Width - 1, rectUpArrow.Y);
            e.Graphics.DrawLine(penUpArrow, rectUpArrow.X, rectUpArrow.Y + 1, rectUpArrow.X + rectUpArrow.Width - 1, rectUpArrow.Y + 1);
            e.Graphics.DrawLine(penUpArrow, rectUpArrow.X, rectUpArrow.Y + rectUpArrow.Height - 1, rectUpArrow.X + rectUpArrow.Width - 1, rectUpArrow.Y + rectUpArrow.Height - 1);
            // Draw Arrow Button Icon
            var pthUpArrow = new System.Drawing.Drawing2D.GraphicsPath();
            var TopUpArrow = new PointF(rectUpArrow.X + rectUpArrow.Width / 2, rectUpArrow.Y + rectUpArrow.Height * 2 / 5);
            var ButtomLeftUpArrow = new PointF(rectUpArrow.X + rectUpArrow.Width * 1 / 5, rectUpArrow.Y + rectUpArrow.Height * 3 / 5);
            var BottomRightUpArrow = new PointF(rectUpArrow.X + rectUpArrow.Width * 4 / 5, rectUpArrow.Y + rectUpArrow.Height * 3 / 5);
            pthUpArrow.AddLine(TopUpArrow, ButtomLeftUpArrow);
            pthUpArrow.AddLine(ButtomLeftUpArrow, BottomRightUpArrow);
            // Determine the arrow's color.
            using SolidBrush arrowBrushUpArrow = new(BackColor);
            // Draw the arrow
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(arrowBrushUpArrow, pthUpArrow);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            //Thumb Location
            int nTop = mThumbTop;
            nTop += UpArrow.Height;

            //Draw Thumb
            using SolidBrush sb3 = new(ForeColor);
            rectThumb = new(rectUpArrow.X, nTop, rectUpArrow.Width, Thumb.Height);
            e.Graphics.FillRectangle(sb3, rectThumb);

            // Draw Down Arrow BG
            using SolidBrush sbDownArrow = new(ForeColor);
            rectDownArrow = new(rectUpArrow.X, Height - DownArrow.Height, rectUpArrow.Width, DownArrow.Height);
            e.Graphics.FillRectangle(sbDownArrow, rectDownArrow);
            // Draw Spaces
            using Pen penDownArrow = new(BackColor);
            e.Graphics.DrawLine(penDownArrow, rectDownArrow.X, rectDownArrow.Y, rectDownArrow.X + rectDownArrow.Width - 1, rectDownArrow.Y);
            e.Graphics.DrawLine(penDownArrow, rectDownArrow.X, rectDownArrow.Y + rectDownArrow.Height - 1, rectDownArrow.X + rectDownArrow.Width - 1, rectDownArrow.Y + rectDownArrow.Height - 1);
            e.Graphics.DrawLine(penDownArrow, rectDownArrow.X, rectDownArrow.Y + rectDownArrow.Height - 2, rectDownArrow.X + rectDownArrow.Width - 1, rectDownArrow.Y + rectDownArrow.Height - 2);
            // Draw Arrow Button Icon
            var pthDownArrow = new System.Drawing.Drawing2D.GraphicsPath();
            var TopLeftDownArrow = new PointF(rectDownArrow.X + rectDownArrow.Width * 1 / 5, rectDownArrow.Y + rectDownArrow.Height * 2 / 5);
            var TopRightDownArrow = new PointF(rectDownArrow.X + rectDownArrow.Width * 4 / 5, rectDownArrow.Y + rectDownArrow.Height * 2 / 5);
            var BottomDownArrow = new PointF(rectDownArrow.X + rectDownArrow.Width / 2, rectDownArrow.Y + rectDownArrow.Height * 3 / 5);
            pthDownArrow.AddLine(TopLeftDownArrow, TopRightDownArrow);
            pthDownArrow.AddLine(TopRightDownArrow, BottomDownArrow);
            // Determine the arrow's color.
            using SolidBrush arrowBrushDownArrow = new(BackColor);
            // Draw the arrow
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            e.Graphics.FillPath(arrowBrushDownArrow, pthDownArrow);
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.Default;

            // Draw Border
            using Pen penBorder = new(BorderColor);
            Rectangle rectBorder = new(rect.X, rect.Y, rect.Width - 1, rect.Height - 1);
            e.Graphics.DrawRectangle(penBorder, rectBorder);
        }

        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
                if (base.AutoSize)
                {
                    Width = UpArrow.Width;
                }
            }
        }

        private void InitializeComponent()
        {
            SuspendLayout();
            Name = "CustomScrollBar";
            MouseWheel += new MouseEventHandler(CustomScrollBar_MouseWheel);
            MouseDown += new MouseEventHandler(CustomScrollBar_MouseDown);
            MouseMove += new MouseEventHandler(CustomScrollBar_MouseMove);
            MouseUp += new MouseEventHandler(CustomScrollBar_MouseUp);
            ResumeLayout(false);
        }

        private void CustomScrollBar_MouseWheel(object? sender, MouseEventArgs e)
        {
            int nTrackHeight = Height - (UpArrow.Height + DownArrow.Height);
            int nThumbHeight = (int)GetThumbHeight();

            int nRealRange = Maximum - Minimum - LargeChange;
            int nPixelRange = nTrackHeight - nThumbHeight;
            if (e.Delta > 0)
            {
                Value -= SmallChange;
                Value = Math.Max(Value, 0);
                MoveThumb(e.Y);
                Debug.WriteLine(Value);
            }
            else
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        Value += SmallChange;
                        if ((mThumbTop + SmallChange) > nPixelRange)
                        {
                            mThumbTop = nPixelRange;
                            // Compute Value
                            float fPerc = mThumbTop / nPixelRange;
                            float fValue = fPerc * (Maximum - LargeChange);
                            Value = (int)fValue;
                        }
                        MoveThumb(e.Y);
                        Debug.WriteLine(Value);
                    }
                }
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());
        }

        private void CustomScrollBar_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right || e.Button == MouseButtons.Middle)
                return;
            Debug.WriteLine("Mouse Down");
            mDown = true;
            Point ptPoint = PointToClient(Cursor.Position);
            
            int nTrackHeight = Height - (UpArrow.Height + DownArrow.Height);
            int nThumbHeight = (int)GetThumbHeight();

            int nTop = mThumbTop;
            nTop += UpArrow.Height;

            int nRealRange = Maximum - Minimum - LargeChange;
            int nPixelRange = nTrackHeight - nThumbHeight;

            // rectUpArrow Action
            if (rectUpArrow.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        while (mDown == true)
                        {
                            Value -= SmallChange;
                            Value = Math.Max(Value, 0);
                            MoveThumb(e.Y);
                            Debug.WriteLine(Value);

                            if (ValueChanged != null)
                                ValueChanged(this, new EventArgs());

                            if (Scroll != null)
                                Scroll(this, new EventArgs());

                            Invalidate();
                            Task.Delay(10).Wait();
                            Application.DoEvents();
                            if (mDown == false)
                                break;
                        }
                    }
                }
            }

            // rectThumbBefore Action
            Rectangle rectThumbBefore = new(new Point(1, UpArrow.Height), new Size(Thumb.Width, nTop - (UpArrow.Height)));
            if (rectThumbBefore.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        while (mDown == true)
                        {
                            Value -= LargeChange;
                            if (mThumbTop + Thumb.Height < e.Y)
                            {
                                if (mThumbTop <= 0)
                                {
                                    mThumbTop = 0;
                                }
                                return;
                            }
                            Value = Math.Max(Value, 0);
                            MoveThumb(e.Y);
                            Debug.WriteLine(Value);
                            Task.Delay(10).Wait();
                            Application.DoEvents();
                            if (mDown == false)
                                break;
                        }
                    }
                }
            }

            // rectThumb Action
            if (rectThumb.Contains(ptPoint))
            {
                // Hit the thumb
                nClickPoint = ptPoint.Y - nTop;
                mThumbDown = true;
            }

            // rectThumbAfter Action
            Rectangle rectThumbAfter = new(1, nTop + Thumb.Height, Thumb.Width, Height - (nTop + Thumb.Height) - (DownArrow.Height));
            if (rectThumbAfter.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        while (mDown == true)
                        {
                            Value += LargeChange;
                            if (mThumbTop + Thumb.Height > e.Y)
                            {
                                if (mThumbTop >= nPixelRange)
                                {
                                    mThumbTop = nPixelRange;
                                }
                                return;
                            }
                            if ((mThumbTop + SmallChange) > nPixelRange)
                            {
                                mThumbTop = nPixelRange;
                                // Compute Value
                                float fPerc = mThumbTop / nPixelRange;
                                float fValue = fPerc * (Maximum - LargeChange);
                                Value = (int)fValue;
                            }
                            MoveThumb(e.Y);
                            Debug.WriteLine(Value);
                            Task.Delay(10).Wait();
                            Application.DoEvents();
                            if (mDown == false)
                                break;
                        }
                    }
                }
            }

            // rectDownArrow Action
            if (rectDownArrow.Contains(ptPoint))
            {
                if (nRealRange > 0)
                {
                    if (nPixelRange > 0)
                    {
                        while (mDown == true)
                        {
                            Value += SmallChange;
                            if ((mThumbTop + SmallChange) > nPixelRange)
                            {
                                mThumbTop = nPixelRange;
                                // Compute Value
                                float fPerc = mThumbTop / nPixelRange;
                                float fValue = fPerc * (Maximum - LargeChange);
                                Value = (int)fValue;
                            }
                            MoveThumb(e.Y);
                            Debug.WriteLine(Value);

                            if (ValueChanged != null)
                                ValueChanged(this, new EventArgs());

                            if (Scroll != null)
                                Scroll(this, new EventArgs());

                            Invalidate();
                            Task.Delay(10).Wait();
                            Application.DoEvents();
                            if (mDown == false)
                                break;
                        }
                    }
                }
            }
        }

        private void CustomScrollBar_MouseUp(object? sender, MouseEventArgs e)
        {
            Debug.WriteLine("Mouse Up");
            mThumbDown = false;
            mThumbDragging = false;
            mDown = false;
        }

        private void MoveThumb(int y)
        {
            int nRealRange = Maximum - Minimum;
            int nTrackHeight = Height - (UpArrow.Height + DownArrow.Height);
            int nThumbHeight = (int)GetThumbHeight();
            
            int nPixelRange = nTrackHeight - nThumbHeight;
            if (mThumbDown && nRealRange > 0)
            {
                //Debug.WriteLine(nPixelRange);
                if (nPixelRange > 0)
                {
                    int nNewThumbTop = y - (UpArrow.Height + nClickPoint);

                    if (nNewThumbTop < 0)
                    {
                        mThumbTop = 0;
                    }
                    else if (nNewThumbTop > nPixelRange)
                    {
                        mThumbTop = nPixelRange;
                    }
                    else
                    {
                        mThumbTop = y - (UpArrow.Height + nClickPoint);
                    }

                    //Compute Value
                    float fPerc = (float)mThumbTop / nPixelRange;
                    float fValue = fPerc * (Maximum - LargeChange);
                    mValue = (int)fValue;
                    Debug.WriteLine(mValue.ToString());
                    
                    Application.DoEvents();

                    Invalidate();
                }
            }
        }

        private void CustomScrollBar_MouseMove(object? sender, MouseEventArgs e)
        {
            if (mThumbDown == true)
            {
                mThumbDragging = true;
            }

            if (mThumbDragging)
            {
                MoveThumb(e.Y);
            }

            if (ValueChanged != null)
                ValueChanged(this, new EventArgs());

            if (Scroll != null)
                Scroll(this, new EventArgs());
        }
    }

    internal class ScrollBarControlDesigner : ControlDesigner
    {
        public override SelectionRules SelectionRules
        {
            get
            {
                SelectionRules selectionRules = base.SelectionRules;
                PropertyDescriptor propDescriptor = TypeDescriptor.GetProperties(Component)["AutoSize"];
                if (propDescriptor != null)
                {
                    bool autoSize = (bool)propDescriptor.GetValue(Component);
                    if (autoSize)
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.Moveable | SelectionRules.BottomSizeable | SelectionRules.TopSizeable;
                    }
                    else
                    {
                        selectionRules = SelectionRules.Visible | SelectionRules.AllSizeable | SelectionRules.Moveable;
                    }
                }
                return selectionRules;
            }
        }
    }
}