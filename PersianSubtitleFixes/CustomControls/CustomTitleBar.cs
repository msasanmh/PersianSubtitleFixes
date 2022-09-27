using PersianSubtitleFixes;
using System;
using System.Drawing;
using System.Reflection;
using System.Windows.Forms;
using MsmhTools;
using System.Runtime.InteropServices;
using PSFTools;

namespace CustomControls
{
	public class CustomTitleBar : Form
	{
		private readonly PictureBox title = new(); // create a PictureBox
		private readonly Label minimise = new(); // this doesn't even have to be a label!
		private readonly Label maximise = new(); // this will simulate our this.maximise box
		private readonly Label close = new(); // simulates the this.close box


		private bool drag = false; // determine if we should be moving the form
		private Point offset = new(0, 0); // also for the moving
		private Point original = new(0, 0); // also for the moving

		private const int height = 30; // Caption bar height;









		// Add App Icon
		private static readonly PictureBox IconBox = new();
		// Add Form Text
		private static readonly Label LabelText = new();
		public static string? TitleText { get; set; }
		// Add Close Button
		private static readonly Button ButtonClose = new();
		private static bool ButtonCloseMouseHover { get; set; }
		private static bool ButtonCloseMouseDown { get; set; }
		// Add Maximize Button
		private static readonly Button ButtonMax = new();
		private static bool ButtonMaxMouseHover { get; set; }
		private static bool ButtonMaxMouseDown { get; set; }
		// Add Minimize Button
		private static readonly Button ButtonMin = new();
		private static bool ButtonMinMouseHover { get; set; }
		private static bool ButtonMinMouseDown { get; set; }
		public CustomTitleBar() : base()
		{

			SetStyle(ControlStyles.OptimizedDoubleBuffer |
					 ControlStyles.ResizeRedraw |
					 ControlStyles.UserPaint, true);

			FormBorderStyle = FormBorderStyle.None;
			Height = 30;
			Width = 1000;
			
			
            SizeChanged += CustomTitleBar_SizeChanged;
            Paint += CustomTitleBar_Paint;

            MouseUp += CustomTitleBar_MouseUp;
            MouseDown += CustomTitleBar_MouseDown;
            MouseMove += CustomTitleBar_MouseMove;

			Dock = DockStyle.Top;
			if (DesignMode)
				BackColor = Color.LightBlue;
			else
				BackColor = Colors.TitleBarBackColor;

			// Form Icon
			//Icon appIcon = Icon.ExtractAssociatedIcon(Assembly.GetExecutingAssembly().Location);
			//Icon appIcon = Icon.ExtractAssociatedIcon(Application.ExecutablePath);
			Icon appIcon = (Icon)typeof(Form).GetProperty("DefaultIcon", BindingFlags.NonPublic | BindingFlags.Static).GetValue(null, null);
			IconBox.Image = appIcon.ToBitmap();
			IconBox.Parent = this;
			IconBox.Width = appIcon.Width;
			IconBox.Height = appIcon.Height;
			int xI = 7;
			int yI = (Height / 2) - (IconBox.Height / 2);
			IconBox.Location = new Point(xI, yI);
			
			// Form Text
			var t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (s, e) =>
            {
                if (TitleText != null)
					LabelText.Text = TitleText;
            };
            t.Start();
			LabelText.Parent = this;
			LabelText.AutoSize = true;
			LabelText.BringToFront();
			int xT = xI + appIcon.Width + 2;
			int yT = (Height / 2) - (LabelText.Height / 2);
			LabelText.Location = new Point(xT, yT);
			Controls.Add(LabelText);

            // Close Button
            ButtonClose.MouseDown += ButtonClose_MouseDown;
            ButtonClose.MouseUp += ButtonClose_MouseUp;
            ButtonClose.MouseEnter += ButtonClose_MouseEnter;
            ButtonClose.MouseLeave += ButtonClose_MouseLeave;
            ButtonClose.MouseHover += ButtonClose_MouseHover;
            ButtonClose.Paint += ButtonClose_Paint;

			// Maximize Button
			ButtonMax.MouseDown += ButtonMax_MouseDown;
			ButtonMax.MouseUp += ButtonMax_MouseUp;
			ButtonMax.MouseEnter += ButtonMax_MouseEnter;
			ButtonMax.MouseLeave += ButtonMax_MouseLeave;
			ButtonMax.MouseHover += ButtonMax_MouseHover;
			ButtonMax.Paint += ButtonMax_Paint;

			// Minimize Button
			ButtonMin.MouseDown += ButtonMin_MouseDown;
			ButtonMin.MouseUp += ButtonMin_MouseUp;
			ButtonMin.MouseEnter += ButtonMin_MouseEnter;
			ButtonMin.MouseLeave += ButtonMin_MouseLeave;
			ButtonMin.MouseHover += ButtonMin_MouseHover;
			ButtonMin.Paint += ButtonMin_Paint;

			//title.Location = Location; // assign the location to the form location
			//title.Width = Width; // make it the same width as the form
			//title.Height = 50; // give it a default height (you may want it taller/shorter)
			//title.BackColor = Color.Black; // give it a default colour (or load an image)
			//Controls.Add(title); // add it to the form's controls, so it gets displayed
			//							   // if you have an image to display, you can load it, instead of assigning a bg colour
			//							   // this.title.Image = new Bitmap(System.Environment.CurrentDirectory + "\\title.jpg");
			//							   // if you displayed an image, alter the SizeMode to get it to display as you want it to
			//							   // examples:
			//							   // this.title.SizeMode = PictureBoxSizeMode.StretchImage;
			//							   // this.title.SizeMode = PictureBoxSizeMode.CenterImage;
			//							   // this.title.SizeMode = PictureBoxSizeMode.Zoom;
			//							   // etc			

			//// you may want to use PictureBoxes and display images
			//// or use buttons, there are many alternatives. This is a mere example.
			//minimise.Text = "Minimise"; // Doesn't have to be
			//minimise.Location = new Point(Location.X + 5, Location.Y + 5); // give it a default location
			//minimise.ForeColor = Color.Red; // Give it a colour that will make it stand out
			//									 // this is why I didn't use an image, just to keep things simple:
			//minimise.BackColor = Color.Black; // make it the same as the PictureBox
			//Controls.Add(minimise); // add it to the form's controls
			//minimise.BringToFront(); // bring it to the front, to display it above the picture box

			//maximise.Text = "Maximise";
			//// remember to make sure it's far enough away so as not to overlap our minimise option
			//maximise.Location = new Point(Location.X + 60, Location.Y + 5);
			//maximise.ForeColor = Color.Red;
			//maximise.BackColor = Color.Black; // remember, we want it to match the background
			//maximise.Width = 50;
			//Controls.Add(maximise); // add it to the form
			//maximise.BringToFront();

			//close.Text = "Close";
			//close.Location = new Point(Location.X + 120, Location.Y + 5);
			//close.ForeColor = Color.Red;
			//close.BackColor = Color.Black;
			//close.Width = 37; // this is just to make it fit nicely
			//Controls.Add(close);
			//close.BringToFront();

			//// now we need to add some functionality. First off, let's give those labels
			//// MouseHover and MouseLeave events, so they change colour
			//// Since they're all going to change to the same colour, we can give them the same
			//// event handler, which saves time of writing out all those extra functions
			//minimise.MouseEnter += new EventHandler(Control_MouseEnter);
			//maximise.MouseEnter += new EventHandler(Control_MouseEnter);
			//close.MouseEnter += new EventHandler(Control_MouseEnter);

			//// and we need to do the same for MouseLeave events, to change it back
			//minimise.MouseLeave += new EventHandler(Control_MouseLeave);
			//maximise.MouseLeave += new EventHandler(Control_MouseLeave);
			//close.MouseLeave += new EventHandler(Control_MouseLeave);

			//// and lastly, for these controls, we need to add some functionality
			//minimise.MouseClick += new MouseEventHandler(Control_MouseClick);
			//maximise.MouseClick += new MouseEventHandler(Control_MouseClick);
			//close.MouseClick += new MouseEventHandler(Control_MouseClick);

			//// finally, wouldn't it be nice to get some moveability on this control?
			//title.MouseDown += new MouseEventHandler(Title_MouseDown);
			//title.MouseUp += new MouseEventHandler(Title_MouseUp);
			//title.MouseMove += new MouseEventHandler(Title_MouseMove);
			
		}
		//------------------------------------------------------------------------------
		private void CustomTitleBar_MouseUp(object? sender, MouseEventArgs e)
        {
			drag = false;
			Capture = false;
        }

        private void CustomTitleBar_MouseDown(object? sender, MouseEventArgs e)
        {
			drag = true;
			Capture = true;
			offset = MousePosition;
			original = Location;
        }

        private void CustomTitleBar_MouseMove(object? sender, MouseEventArgs e)
        {
			if (!drag)
				return;
			int x = original.X + MousePosition.X - offset.X;
			int y = original.Y + MousePosition.Y - offset.Y;
			Location = new Point(x, y);
		}
        //------------------------------------------------------------------------------

        //------------------------------------------------------------------------------
        //protected override void WndProc(ref Message m)
        //{
        //    if (m.Msg == 0x0313)
        //    {
        //        //ContextMenuStrip contextMenu = new();
        //        // contMenu is the ContextMenu which you wish to display
        //        int contMenuX = Cursor.Position.X - Location.X;
        //        int contMenuY = Cursor.Position.Y - Location.Y;
        //        Point contMenuPos = new(contMenuX, contMenuY);
        //        this.ContextMenuStrip.Show(this, contMenuPos);
        //    }
        //    base.WndProc(ref m);
        //}
        //------------------------------------------------------------------------------
        private void ButtonClose_MouseDown(object? sender, MouseEventArgs e)
        {
			if (sender is Button button)
			{
				ButtonCloseMouseDown = true;
				button.Invalidate();
			}
		}

        private void ButtonClose_MouseUp(object? sender, MouseEventArgs e)
        {
			if (sender is Button button)
			{
				ButtonCloseMouseDown = false;
				button.Invalidate();
			}
		}

        private void ButtonClose_MouseEnter(object? sender, EventArgs e)
        {
			if (sender is Button button)
			{
				ButtonCloseMouseHover = true;
				button.Invalidate();
			}
		}

        private void ButtonClose_MouseLeave(object? sender, EventArgs e)
        {
			if (sender is Button button)
			{
				ButtonCloseMouseHover = false;
				button.Invalidate();
			}
		}

		private void ButtonClose_MouseHover(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonCloseMouseHover = true;
				button.Invalidate();
			}
		}

		private void ButtonClose_Paint(object? sender, PaintEventArgs e)
        {
            if (sender is Button button)
            {
				button.BackColor = Colors.TitleBarBackColor;
				if (ButtonCloseMouseHover == true)
					button.BackColor = Color.FromArgb(232, 17, 35);
				if (ButtonCloseMouseDown == true)
					button.BackColor = Color.FromArgb(139, 10, 20);
				button.ForeColor = Colors.TitleBarForeColor;
				Rectangle rect = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, height);

				// Draw Button Background
				e.Graphics.Clear(button.BackColor);

				// Draw X
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				using Pen pen = new(button.ForeColor, 1.6f);
				Rectangle rectX = new(rect.X + (rect.Width - rect.Height) / 2, rect.Y, rect.Height, rect.Height);
				rectX.Inflate(-20, -20);
				Point[] point1 = new Point[] { new Point(rectX.Left, rectX.Top), new Point(rectX.Right, rectX.Bottom) };
				e.Graphics.DrawLines(pen, point1);
				Point[] point2 = new Point[] { new Point(rectX.Right, rectX.Top), new Point(rectX.Left, rectX.Bottom) };
				e.Graphics.DrawLines(pen, point2);
			}
		}
		//------------------------------------------------------------------------------
		private void ButtonMax_MouseDown(object? sender, MouseEventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMaxMouseDown = true;
				button.Invalidate();
			}
		}

		private void ButtonMax_MouseUp(object? sender, MouseEventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMaxMouseDown = false;
				button.Invalidate();
			}
		}

		private void ButtonMax_MouseEnter(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMaxMouseHover = true;
				button.Invalidate();
			}
		}

		private void ButtonMax_MouseLeave(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMaxMouseHover = false;
				button.Invalidate();
			}
		}

		private void ButtonMax_MouseHover(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMaxMouseHover = true;
				button.Invalidate();
			}
		}

		private void ButtonMax_Paint(object? sender, PaintEventArgs e)
		{
			if (sender is Button button)
			{
				button.BackColor = Colors.TitleBarBackColor;
				if (ButtonMaxMouseHover == true)
					button.BackColor = button.BackColor.ChangeBrightness(0.1f);
				if (ButtonMaxMouseDown == true)
					button.BackColor = button.BackColor.ChangeBrightness(0.1f);
				button.ForeColor = Colors.TitleBarForeColor;
				Rectangle rect = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, height);

				// Draw Button Background
				e.Graphics.Clear(button.BackColor);

				// Draw Rectangle
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				using Pen pen = new(button.ForeColor, 1.6f);
				Rectangle rectMax = new(rect.X + (rect.Width - rect.Height) / 2, rect.Y, rect.Height, rect.Height);
				rectMax.Inflate(-10, -10);
				e.Graphics.DrawRectangle(pen, rectMax);
			}
		}
		//------------------------------------------------------------------------------
		private void ButtonMin_MouseDown(object? sender, MouseEventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMinMouseDown = true;
				button.Invalidate();
			}
		}

		private void ButtonMin_MouseUp(object? sender, MouseEventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMinMouseDown = false;
				button.Invalidate();
			}
		}

		private void ButtonMin_MouseEnter(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMinMouseHover = true;
				button.Invalidate();
			}
		}

		private void ButtonMin_MouseLeave(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMinMouseHover = false;
				button.Invalidate();
			}
		}

		private void ButtonMin_MouseHover(object? sender, EventArgs e)
		{
			if (sender is Button button)
			{
				ButtonMinMouseHover = true;
				button.Invalidate();
			}
		}

		private void ButtonMin_Paint(object? sender, PaintEventArgs e)
		{
			if (sender is Button button)
			{
				button.BackColor = Colors.TitleBarBackColor;
				if (ButtonMinMouseHover == true)
					button.BackColor = button.BackColor.ChangeBrightness(0.1f);
				if (ButtonMinMouseDown == true)
					button.BackColor = button.BackColor.ChangeBrightness(0.1f);
				button.ForeColor = Colors.TitleBarForeColor;
				Rectangle rect = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, height);

				// Draw Button Background
				e.Graphics.Clear(button.BackColor);

				// Draw Rectangle
				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
				using Pen pen = new(button.ForeColor, 1.6f);
				Rectangle rectMin = new(rect.X + (rect.Width - rect.Height) / 2, rect.Y, rect.Height, rect.Height);
				rectMin.Inflate(-10, -10);
				e.Graphics.DrawLine(pen, new Point(rectMin.X, rectMin.Y + (rectMin.Width / 2)), new Point(rectMin.X + rectMin.Width, rectMin.Y + (rectMin.Width / 2)));
			}
		}
		//------------------------------------------------------------------------------
		private void CustomTitleBar_SizeChanged(object? sender, EventArgs e)
        {
			Invalidate();
		}

        private void CustomTitleBar_Paint(object? sender, PaintEventArgs e)
        {
			Rectangle rect = new(e.ClipRectangle.X, e.ClipRectangle.Y, e.ClipRectangle.Width, height);
			
			if (DesignMode)
            {
                // Fill Background
                using SolidBrush sbBG = new(Color.LightBlue);
                e.Graphics.FillRectangle(sbBG, rect);
				// Fill Icon Background
				IconBox.BackColor = Color.LightBlue;
				// Fill Text Background
				LabelText.BackColor = Color.LightBlue;
				// Default Title Text
				LabelText.Text = "TitleBar";
			}
			else
            {
				// Fill Background
				BackColor = Colors.TitleBarBackColor;
				// Fill Icon Background
				IconBox.BackColor = Colors.TitleBarBackColor;
				// Fill Text Background
				LabelText.BackColor = Colors.TitleBarBackColor;

				// Close Button
				ButtonClose.FlatStyle = FlatStyle.Flat;
				ButtonClose.AutoSize = false;
				ButtonClose.Parent = this;
				ButtonClose.BringToFront();
				ButtonClose.Width = 45;
				ButtonClose.Height = height;
				int xCB = Right - ButtonClose.Width - 100;
				ButtonClose.Location = new Point(xCB, 0);

				// Maximize Button
				ButtonMax.FlatStyle = FlatStyle.Flat;
				ButtonMax.AutoSize = false;
				ButtonMax.Parent = this;
				ButtonMax.BringToFront();
				ButtonMax.Width = 45;
				ButtonMax.Height = height;
				int xMaxB = xCB - ButtonMax.Width - 1;
				ButtonMax.Location = new Point(xMaxB, 0);

				// Maximize Button
				ButtonMin.FlatStyle = FlatStyle.Flat;
				ButtonMin.AutoSize = false;
				ButtonMin.Parent = this;
				ButtonMin.BringToFront();
				ButtonMin.Width = 45;
				ButtonMin.Height = height;
				int xMinB = xMaxB - ButtonMax.Width - 1;
				ButtonMin.Location = new Point(xMinB, 0);
			}
			
		}
		//------------------------------------------------------------------------------
		private void Control_MouseEnter(object? sender, EventArgs e)
		{
			if (sender.Equals(close))
				close.ForeColor = Color.White;
			else if (sender.Equals(maximise))
				maximise.ForeColor = Color.White;
			else // it's the minimise label
				minimise.ForeColor = Color.White;
		}

		private void Control_MouseLeave(object? sender, EventArgs e)
		{ // return them to their default colours
			if (sender.Equals(close))
				close.ForeColor = Color.Red;
			else if (sender.Equals(this.maximise))
				maximise.ForeColor = Color.Red;
			else // it's the minimise label
				minimise.ForeColor = Color.Red;
		}

		private void Control_MouseClick(object? sender, MouseEventArgs e)
		{
			if (sender.Equals(close))
				Application.Exit(); // close the form
			else if (sender.Equals(maximise))
			{ // maximise is more interesting. We need to give it different functionality,
			  // depending on the window state (Maximise/Restore)
				if (maximise.Text == "Maximise")
				{
					//WindowState = FormWindowState.Maximized; // maximise the form
					maximise.Text = "Restore"; // change the text
					title.Width = Width; // stretch the title bar
				}
				else // we need to restore
				{
					//WindowState = FormWindowState.Normal;
					maximise.Text = "Maximise";
				}
			}
			//else // it's the minimise label
				//WindowState = FormWindowState.Minimized; // minimise the form
		}

		
	}
}