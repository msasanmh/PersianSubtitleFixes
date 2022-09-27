﻿using MsmhTools;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.Design;
/*
* Copyright MSasanMH, June 20, 2022.
*/

namespace CustomControls
{
    public class CustomMenuStrip : MenuStrip
    {
        private readonly CustomToolStripRenderer MyRenderer = new();

        private bool mBorder = false;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Appearance"), Description("Border")]
        public bool Border
        {
            get { return mBorder; }
            set
            {
                if (mBorder != value)
                {
                    mBorder = value;
                    Invalidate();
                }
            }
        }

        private Color mBorderColor = Color.Blue;
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
                    BorderColorChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        private Color mSelectionColor = Color.LightBlue;
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
                    SelectionColorChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        private bool mSameColorForSubItems = true;
        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Appearance"), Description("Same Color For Sub Items")]
        public bool SameColorForSubItems
        {
            get { return mSameColorForSubItems; }
            set
            {
                if (mSameColorForSubItems != value)
                {
                    mSameColorForSubItems = value;
                    SameColorForSubItemsChanged?.Invoke(this, EventArgs.Empty);
                    Invalidate();
                }
            }
        }

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Action"), Description("Same Color For Sub Items Changed Event")]
        public event EventHandler? SameColorForSubItemsChanged;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Property Changed"), Description("Border Color Changed Event")]
        public event EventHandler? BorderColorChanged;

        [EditorBrowsable(EditorBrowsableState.Always), Browsable(true)]
        [Category("Property Changed"), Description("Selection Color Changed Event")]
        public event EventHandler? SelectionColorChanged;

        public CustomMenuStrip() : base()
        {
            SetStyle(ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            // Default
            BackColor = Color.DimGray;
            ForeColor = Color.White;

            MyRenderer.BackColor = GetBackColor();
            MyRenderer.ForeColor = GetForeColor();
            MyRenderer.BorderColor = GetBorderColor();
            MyRenderer.SelectionColor = SelectionColor;
            Renderer = MyRenderer;

            BackColorChanged += CustomMenuStrip_BackColorChanged;
            ForeColorChanged += CustomMenuStrip_ForeColorChanged;
            BorderColorChanged += CustomMenuStrip_BorderColorChanged;
            SelectionColorChanged += CustomMenuStrip_SelectionColorChanged;
            SameColorForSubItemsChanged += CustomMenuStrip_SameColorForSubItemsChanged;
            ItemAdded += CustomMenuStrip_ItemAdded;
            Paint += CustomMenuStrip_Paint;

            var timer = new System.Windows.Forms.Timer();
            timer.Interval = 500;
            timer.Tick += (s, e) =>
            {
                if (SameColorForSubItems)
                    ColorForSubItems();
            };
            timer.Start();
        }

        private void ColorForSubItems()
        {
            for (int a = 0; a < Items.Count; a++)
            {
                ToolStripItem toolStripItem = Items[a];
                var toolStripItems = Tools.Controllers.GetAllToolStripItems(toolStripItem);
                for (int b = 0; b < toolStripItems.Count(); b++)
                {
                    ToolStripItem tsi = toolStripItems.ToList()[b];
                    if (tsi is ToolStripMenuItem)
                    {
                        ToolStripMenuItem tsmi = tsi as ToolStripMenuItem;
                        tsmi.BackColor = GetBackColor();
                        tsmi.ForeColor = GetForeColor();
                    }
                    else if (tsi is ToolStripSeparator)
                    {
                        ToolStripSeparator tss = tsi as ToolStripSeparator;
                        tss.BackColor = GetBackColor();
                        tss.ForeColor = BorderColor;
                    }
                }
            }
            
        }

        private void CustomMenuStrip_BackColorChanged(object? sender, EventArgs e)
        {
            MyRenderer.BackColor = GetBackColor();
            Invalidate();
        }

        private void CustomMenuStrip_ForeColorChanged(object? sender, EventArgs e)
        {
            MyRenderer.ForeColor = GetForeColor();
            Invalidate();
        }

        private void CustomMenuStrip_BorderColorChanged(object? sender, EventArgs e)
        {
            MyRenderer.BorderColor = GetBorderColor();
            Invalidate();
        }

        private void CustomMenuStrip_SelectionColorChanged(object? sender, EventArgs e)
        {
            MyRenderer.SelectionColor = SelectionColor;
            Invalidate();
        }

        private void CustomMenuStrip_SameColorForSubItemsChanged(object? sender, EventArgs e)
        {
            if (SameColorForSubItems)
                ColorForSubItems();
        }

        private void CustomMenuStrip_ItemAdded(object? sender, ToolStripItemEventArgs e)
        {
            if (SameColorForSubItems)
                ColorForSubItems();
            Invalidate();
        }

        private void CustomMenuStrip_Paint(object? sender, PaintEventArgs e)
        {
            if (Border)
            {
                Color borderColor = GetBorderColor();
                ControlPaint.DrawBorder(e.Graphics, ClientRectangle, borderColor, ButtonBorderStyle.Solid);
            }
        }

        private Color GetBackColor()
        {
            if (Enabled)
                return BackColor;
            else
            {
                if (BackColor.DarkOrLight() == "Dark")
                    return BackColor.ChangeBrightness(0.2f);
                else
                    return BackColor.ChangeBrightness(-0.2f);
            }
        }

        private Color GetForeColor()
        {
            if (Enabled)
                return ForeColor;
            else
            {
                if (ForeColor.DarkOrLight() == "Dark")
                    return ForeColor.ChangeBrightness(0.2f);
                else
                    return ForeColor.ChangeBrightness(-0.2f);
            }
        }

        private Color GetBorderColor()
        {
            if (Enabled)
                return BorderColor;
            else
            {
                if (BorderColor.DarkOrLight() == "Dark")
                    return BorderColor.ChangeBrightness(0.3f);
                else
                    return BorderColor.ChangeBrightness(-0.3f);
            }
        }
    }
}
