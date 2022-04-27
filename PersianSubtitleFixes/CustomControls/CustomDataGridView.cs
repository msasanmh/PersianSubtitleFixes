using MsmhTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;

namespace CustomControls
{
    public class CustomDataGridView : DataGridView
    {
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new Color BackgroundColor { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new bool EnableHeadersVisualStyles { get; set; }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public new DataGridViewHeaderBorderStyle ColumnHeadersBorderStyle { get; set; }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Appearance"), Description("Column Headers Border")]
        public bool ColumnHeadersBorder { get; set; }

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

        private Color mGridColor = Color.LightBlue;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Grid Lines Color")]
        public new Color GridColor
        {
            get { return mGridColor; }
            set
            {
                if (mGridColor != value)
                {
                    mGridColor = value;
                    Invalidate();
                }
            }
        }

        private Color mCheckColor = Color.Blue;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Editor(typeof(WindowsFormsComponentEditor), typeof(Color))]
        [Category("Appearance"), Description("Check Color for CheckBox Cell Type")]
        public Color CheckColor
        {
            get { return mCheckColor; }
            set
            {
                if (mCheckColor != value)
                {
                    mCheckColor = value;
                    Invalidate();
                }
            }
        }

        private static Color[]? OriginalColors;
        private static Color BackColorDarker { get; set; }
        private static Color SelectionUnfocused { get; set; }

        private static Color BackColorDisabled;
        private static Color ForeColorDisabled;
        private static Color BorderColorDisabled;
        private static Color GridColorDisabled;
        private static Color CheckColorDisabled;
        private static Color BackColorDarkerDisabled;

        private static bool ApplicationIdle = false;
        private bool once = true;

        public CustomDataGridView() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            BorderStyle = BorderStyle.FixedSingle;
            EnableHeadersVisualStyles = false;
            ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;

            ColumnHeadersBorder = true;

            Application.Idle += Application_Idle;
            HandleCreated += DataGridView_HandleCreated;
            MouseUp += CustomDataGridView_MouseUp;
            MouseDown += CustomDataGridView_MouseDown;
            MouseMove += CustomDataGridView_MouseMove;
            LocationChanged += CustomDataGridView_LocationChanged;
            Move += CustomDataGridView_Move;
            Scroll += DataGridView_Scroll;
            GotFocus += DataGridView_GotFocus;
            LostFocus += DataGridView_LostFocus;
            EnabledChanged += DataGridView_EnabledChanged;
            CellClick -= DataGridView_CellClick;
            CellClick += DataGridView_CellClick;
            CellPainting += DataGridView_CellPainting;
            Paint += DataGridView_Paint;
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

        private void DataGridView_HandleCreated(object? sender, EventArgs e)
        {
            if (sender is null || e is null)
                return;
            BackColorDarker = mBackColor.ChangeBrightness(-0.3f);
            SelectionUnfocused = mSelectionColor.ChangeBrightness(0.3f);
            OriginalColors = new Color[] { mBackColor, mForeColor, mBorderColor, mSelectionColor, mGridColor, mCheckColor, BackColorDarker, SelectionUnfocused };
            var gv = sender as DataGridView;
            DataGridViewColor(gv);
            gv.Invalidate();
        }

        private void CustomDataGridView_MouseUp(object? sender, MouseEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void CustomDataGridView_MouseDown(object? sender, MouseEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void CustomDataGridView_MouseMove(object? sender, MouseEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void CustomDataGridView_LocationChanged(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void CustomDataGridView_Move(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private static void DataGridView_Scroll(object? sender, ScrollEventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void DataGridView_GotFocus(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void DataGridView_LostFocus(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void DataGridView_EnabledChanged(object? sender, EventArgs e)
        {
            var gv = sender as DataGridView;
            gv.Invalidate();
        }

        private void DataGridViewColor(DataGridView? gv)
        {
            // Update Colors
            BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            SelectionUnfocused = SelectionColor.ChangeBrightness(0.3f);
            OriginalColors = new Color[] { BackColor, ForeColor, BorderColor, SelectionColor, GridColor, CheckColor, BackColorDarker, SelectionUnfocused };
            
            if (DesignMode)
            {
                BackColor = mBackColor;
                ForeColor = mForeColor;
                BorderColor = mBorderColor;
                SelectionColor = mSelectionColor;
                GridColor = mGridColor;
                CheckColor = mCheckColor;
            }
            else
            {
                if (OriginalColors != null)
                {
                    if (gv.Enabled == true)
                    {
                        BackColor = OriginalColors[0];
                        ForeColor = OriginalColors[1];
                        BorderColor = OriginalColors[2];
                        SelectionColor = OriginalColors[3];
                        GridColor = OriginalColors[4];
                        CheckColor = OriginalColors[5];
                        BackColorDarker = OriginalColors[6];
                        SelectionUnfocused = OriginalColors[7];
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
                            GridColorDisabled = OriginalColors[4].ChangeBrightness(0.3f);
                        else if (OriginalColors[4].DarkOrLight() == "Light")
                            GridColorDisabled = OriginalColors[4].ChangeBrightness(-0.3f);

                        if (OriginalColors[5].DarkOrLight() == "Dark")
                            CheckColorDisabled = OriginalColors[5].ChangeBrightness(0.3f);
                        else if (OriginalColors[5].DarkOrLight() == "Light")
                            CheckColorDisabled = OriginalColors[5].ChangeBrightness(-0.3f);

                        if (OriginalColors[6].DarkOrLight() == "Dark")
                            BackColorDarkerDisabled = OriginalColors[6].ChangeBrightness(0.3f);
                        else if (OriginalColors[6].DarkOrLight() == "Light")
                            BackColorDarkerDisabled = OriginalColors[6].ChangeBrightness(-0.3f);
                    }
                }
            }

            Color backColor;
            Color foreColor;
            Color gridColor;
            Color backColorDarker;

            if (gv.Enabled == true)
            {
                backColor = BackColor;
                foreColor = ForeColor;
                gridColor = GridColor;
                backColorDarker = BackColorDarker;
            }
            else
            {
                backColor = BackColorDisabled;
                foreColor = ForeColorDisabled;
                gridColor = GridColorDisabled;
                backColorDarker = BackColorDarkerDisabled;
            }
            
            gv.BackgroundColor = backColor;
            gv.GridColor = gridColor;
            gv.ColumnHeadersDefaultCellStyle.BackColor = backColorDarker;
            gv.ColumnHeadersDefaultCellStyle.SelectionBackColor = backColorDarker;
            gv.ColumnHeadersDefaultCellStyle.ForeColor = foreColor;
            gv.ColumnHeadersDefaultCellStyle.SelectionForeColor = foreColor;
            gv.DefaultCellStyle.BackColor = backColor;
            gv.DefaultCellStyle.ForeColor = foreColor;
            if (gv.Focused)
                gv.DefaultCellStyle.SelectionBackColor = SelectionColor;
            else
                gv.DefaultCellStyle.SelectionBackColor = SelectionUnfocused;
            gv.DefaultCellStyle.SelectionForeColor = foreColor;

            gv.EnableHeadersVisualStyles = false;
            if (ColumnHeadersBorder)
                gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.Single;
            else
                gv.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            
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

        private void DataGridView_CellPainting(object? sender, DataGridViewCellPaintingEventArgs e)
        {
            var gv = sender as DataGridView;
            //// Update Colors
            //BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            //SelectionUnfocused = SelectionColor.ChangeBrightness(0.3f);
            //OriginalColors = new Color[] { BackColor, ForeColor, BorderColor, SelectionColor, GridColor, CheckColor, BackColorDarker, SelectionUnfocused };
            DataGridViewColor(gv);

            Color backColor;
            Color checkColor;
            Color borderColor;

            if (gv.Enabled)
            {
                backColor = BackColor;
                checkColor = CheckColor;
                borderColor = BorderColor;
            }
            else
            {
                backColor = BackColorDisabled;
                checkColor = CheckColorDisabled;
                borderColor = BorderColorDisabled;
            }

            if (ApplicationIdle == false)
                return;

            if (DesignMode || !DesignMode)
            {
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

                        // Fill Check Rect
                        using SolidBrush brush1 = new(backColor);
                        e.Graphics.FillRectangle(brush1, rectCell);

                        // Draw Check
                        if (Convert.ToBoolean(cell.Value) == true)
                        {
                            int rectCheck = rectCell.Height - 1;
                            if (rectCheck <= 0)
                                rectCheck = 1;

                            // Draw Check Using Font
                            //using SolidBrush brush2 = new(checkColor);
                            //using Font wing = new("Wingdings", rectCheck);
                            //e.Graphics.DrawString("ü", wing, brush2, rectCell.X - 3, rectCell.Y);

                            if (rectCheck > 1)
                            {
                                // Draw Check
                                using var p = new Pen(checkColor, 2);
                                rectCell.Inflate(-2, -2);
                                e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
                                e.Graphics.DrawLines(p, new Point[] { new Point(rectCell.Left, rectCell.Bottom - rectCell.Height / 2), new Point(rectCell.Left + rectCell.Width / 3, rectCell.Bottom), new Point(rectCell.Right, rectCell.Top) });
                                e.Graphics.SmoothingMode = SmoothingMode.Default;
                                rectCell.Inflate(+2, +2);
                            }  
                        }

                        // Draw Check Rect (Check Border)
                        using Pen pen1 = new(borderColor);
                        e.Graphics.DrawRectangle(pen1, rectCell);
                    }
                }
            }
        }

        private void DataGridView_Paint(object? sender, PaintEventArgs e)
        {
            var gv = sender as DataGridView;
            //// Update Colors
            //BackColorDarker = BackColor.ChangeBrightness(-0.3f);
            //SelectionUnfocused = SelectionColor.ChangeBrightness(0.3f);
            //OriginalColors = new Color[] { BackColor, ForeColor, BorderColor, SelectionColor, GridColor, CheckColor, BackColorDarker, SelectionUnfocused };
            DataGridViewColor(gv);

            if (ApplicationIdle == false)
                return;

            if (DesignMode || !DesignMode)
            {
                Rectangle rectGv;

                Color borderColor;
                if (gv.Enabled)
                    borderColor = BorderColor;
                else
                    borderColor = BorderColorDisabled;

                if (BorderStyle == BorderStyle.FixedSingle)
                    ControlPaint.DrawBorder(e.Graphics, e.ClipRectangle, borderColor, ButtonBorderStyle.Solid);
                else if (BorderStyle == BorderStyle.Fixed3D)
                {
                    Color secondBorderColor;
                    if (borderColor.DarkOrLight() == "Dark")
                        secondBorderColor = borderColor.ChangeBrightness(0.6f);
                    else
                        secondBorderColor = borderColor.ChangeBrightness(-0.6f);

                    rectGv = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, e.ClipRectangle.Height);
                    ControlPaint.DrawBorder(e.Graphics, rectGv, secondBorderColor, ButtonBorderStyle.Solid);

                    rectGv = new(e.ClipRectangle.X + 1, e.ClipRectangle.Y + 1, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                    ControlPaint.DrawBorder(e.Graphics, rectGv, secondBorderColor, ButtonBorderStyle.Solid);

                    rectGv = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width - 1, e.ClipRectangle.Height - 1);
                    ControlPaint.DrawBorder(e.Graphics, rectGv, borderColor, ButtonBorderStyle.Solid);
                }
            }
        }
    }
}
