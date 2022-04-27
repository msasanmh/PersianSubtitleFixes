namespace PersianSubtitleFixes
{
    partial class FormMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.StatusStrip1 = new System.Windows.Forms.StatusStrip();
            this.ToolStripLabelLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabelSpace1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabelRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.MenuStrip1 = new System.Windows.Forms.MenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.ToolStrip1 = new System.Windows.Forms.ToolStrip();
            this.ToolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabelFormat = new System.Windows.Forms.ToolStripLabel();
            this.CustomToolStripComboBoxFormat = new CustomControls.CustomToolStripComboBox();
            this.ToolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabelEncoding = new System.Windows.Forms.ToolStripLabel();
            this.CustomToolStripComboBoxEncoding = new CustomControls.CustomToolStripComboBox();
            this.ToolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonExit = new System.Windows.Forms.ToolStripButton();
            this.CustomButtonStop = new CustomControls.CustomButton();
            this.CustomButtonApply = new CustomControls.CustomButton();
            this.CustomButtonInvertCheck = new CustomControls.CustomButton();
            this.CustomButtonCheckAll = new CustomControls.CustomButton();
            this.CustomProgressBar1 = new CustomControls.CustomProgressBar();
            this.CustomPanel1 = new CustomControls.CustomPanel();
            this.CustomDataGridView1 = new CustomControls.CustomDataGridView();
            this.ColumnApply = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ColumnLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnBefore = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnAfter = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.StatusStrip1.SuspendLayout();
            this.MenuStrip1.SuspendLayout();
            this.ToolStrip1.SuspendLayout();
            this.CustomPanel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomDataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // StatusStrip1
            // 
            this.StatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabelLeft,
            this.ToolStripLabelSpace1,
            this.ToolStripLabelRight});
            this.StatusStrip1.Location = new System.Drawing.Point(0, 389);
            this.StatusStrip1.Name = "StatusStrip1";
            this.StatusStrip1.Size = new System.Drawing.Size(884, 22);
            this.StatusStrip1.TabIndex = 0;
            this.StatusStrip1.Text = "statusStrip1";
            // 
            // ToolStripLabelLeft
            // 
            this.ToolStripLabelLeft.AutoSize = false;
            this.ToolStripLabelLeft.Name = "ToolStripLabelLeft";
            this.ToolStripLabelLeft.Size = new System.Drawing.Size(749, 17);
            this.ToolStripLabelLeft.Spring = true;
            this.ToolStripLabelLeft.Text = "LabelLeft";
            this.ToolStripLabelLeft.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // ToolStripLabelSpace1
            // 
            this.ToolStripLabelSpace1.AutoSize = false;
            this.ToolStripLabelSpace1.Name = "ToolStripLabelSpace1";
            this.ToolStripLabelSpace1.Size = new System.Drawing.Size(20, 17);
            // 
            // ToolStripLabelRight
            // 
            this.ToolStripLabelRight.AutoSize = false;
            this.ToolStripLabelRight.Name = "ToolStripLabelRight";
            this.ToolStripLabelRight.Size = new System.Drawing.Size(100, 17);
            this.ToolStripLabelRight.Text = "LabelRight";
            this.ToolStripLabelRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // MenuStrip1
            // 
            this.MenuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.MenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.EditToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.MenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.MenuStrip1.Name = "MenuStrip1";
            this.MenuStrip1.Size = new System.Drawing.Size(136, 24);
            this.MenuStrip1.TabIndex = 1;
            this.MenuStrip1.Text = "menuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.ToolStripSeparator5,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonOpen_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonSave_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.SaveAsToolStripMenuItem.Text = "Save as...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonSaveAs_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(181, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.Name = "ExitToolStripMenuItem";
            this.ExitToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
            this.ExitToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.ExitToolStripMenuItem.Text = "Exit";
            this.ExitToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonExit_Click);
            // 
            // EditToolStripMenuItem
            // 
            this.EditToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.UndoToolStripMenuItem,
            this.RedoToolStripMenuItem});
            this.EditToolStripMenuItem.Name = "EditToolStripMenuItem";
            this.EditToolStripMenuItem.Size = new System.Drawing.Size(39, 20);
            this.EditToolStripMenuItem.Text = "Edit";
            // 
            // UndoToolStripMenuItem
            // 
            this.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
            this.UndoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.UndoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.UndoToolStripMenuItem.Text = "Undo";
            this.UndoToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonUndo_Click);
            // 
            // RedoToolStripMenuItem
            // 
            this.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
            this.RedoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.RedoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.RedoToolStripMenuItem.Text = "Redo";
            this.RedoToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonRedo_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.AboutToolStripMenuItem.Text = "About";
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // ToolStrip1
            // 
            this.ToolStrip1.AutoSize = false;
            this.ToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.ToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonOpen,
            this.ToolStripButtonSave,
            this.ToolStripButtonSaveAs,
            this.ToolStripSeparator1,
            this.ToolStripButtonUndo,
            this.ToolStripButtonRedo,
            this.ToolStripSeparator2,
            this.ToolStripLabelFormat,
            this.CustomToolStripComboBoxFormat,
            this.ToolStripSeparator3,
            this.ToolStripLabelEncoding,
            this.CustomToolStripComboBoxEncoding,
            this.ToolStripSeparator4,
            this.ToolStripButtonSettings,
            this.ToolStripButtonAbout,
            this.ToolStripButtonExit});
            this.ToolStrip1.Location = new System.Drawing.Point(0, 0);
            this.ToolStrip1.Name = "ToolStrip1";
            this.ToolStrip1.Size = new System.Drawing.Size(884, 40);
            this.ToolStrip1.TabIndex = 11;
            this.ToolStrip1.Text = "ToolStrip1";
            // 
            // ToolStripButtonOpen
            // 
            this.ToolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonOpen.Image = global::PersianSubtitleFixes.Properties.Resources.Open_Black;
            this.ToolStripButtonOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonOpen.Name = "ToolStripButtonOpen";
            this.ToolStripButtonOpen.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonOpen.Text = "ToolStripButtonOpen";
            this.ToolStripButtonOpen.ToolTipText = "Open";
            this.ToolStripButtonOpen.Click += new System.EventHandler(this.ToolStripButtonOpen_Click);
            // 
            // ToolStripButtonSave
            // 
            this.ToolStripButtonSave.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonSave.Image = global::PersianSubtitleFixes.Properties.Resources.Save_Black;
            this.ToolStripButtonSave.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonSave.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonSave.Name = "ToolStripButtonSave";
            this.ToolStripButtonSave.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonSave.Text = "ToolStripButtonSave";
            this.ToolStripButtonSave.ToolTipText = "Save";
            this.ToolStripButtonSave.Click += new System.EventHandler(this.ToolStripButtonSave_Click);
            // 
            // ToolStripButtonSaveAs
            // 
            this.ToolStripButtonSaveAs.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonSaveAs.Image = global::PersianSubtitleFixes.Properties.Resources.Save_as_Black;
            this.ToolStripButtonSaveAs.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonSaveAs.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonSaveAs.Name = "ToolStripButtonSaveAs";
            this.ToolStripButtonSaveAs.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonSaveAs.Text = "ToolStripButtonSaveAs";
            this.ToolStripButtonSaveAs.ToolTipText = "SaveAs";
            this.ToolStripButtonSaveAs.Click += new System.EventHandler(this.ToolStripButtonSaveAs_Click);
            // 
            // ToolStripSeparator1
            // 
            this.ToolStripSeparator1.Name = "ToolStripSeparator1";
            this.ToolStripSeparator1.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripButtonUndo
            // 
            this.ToolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonUndo.Image = global::PersianSubtitleFixes.Properties.Resources.Undo_Black;
            this.ToolStripButtonUndo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonUndo.Name = "ToolStripButtonUndo";
            this.ToolStripButtonUndo.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonUndo.Text = "ToolStripButtonUndo";
            this.ToolStripButtonUndo.ToolTipText = "Undo";
            this.ToolStripButtonUndo.Click += new System.EventHandler(this.ToolStripButtonUndo_Click);
            // 
            // ToolStripButtonRedo
            // 
            this.ToolStripButtonRedo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonRedo.Image = global::PersianSubtitleFixes.Properties.Resources.Redo_Black;
            this.ToolStripButtonRedo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonRedo.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonRedo.Name = "ToolStripButtonRedo";
            this.ToolStripButtonRedo.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonRedo.Text = "ToolStripButtonRedo";
            this.ToolStripButtonRedo.ToolTipText = "Redo";
            this.ToolStripButtonRedo.Click += new System.EventHandler(this.ToolStripButtonRedo_Click);
            // 
            // ToolStripSeparator2
            // 
            this.ToolStripSeparator2.Name = "ToolStripSeparator2";
            this.ToolStripSeparator2.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripLabelFormat
            // 
            this.ToolStripLabelFormat.Name = "ToolStripLabelFormat";
            this.ToolStripLabelFormat.Size = new System.Drawing.Size(45, 37);
            this.ToolStripLabelFormat.Text = "Format";
            // 
            // CustomToolStripComboBoxFormat
            // 
            this.CustomToolStripComboBoxFormat.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CustomToolStripComboBoxFormat.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.CustomToolStripComboBoxFormat.BorderColor = System.Drawing.Color.Blue;
            this.CustomToolStripComboBoxFormat.DropDownHeight = 106;
            this.CustomToolStripComboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CustomToolStripComboBoxFormat.DropDownWidth = 130;
            this.CustomToolStripComboBoxFormat.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.CustomToolStripComboBoxFormat.IntegralHeight = false;
            this.CustomToolStripComboBoxFormat.MaxDropDownItems = 8;
            this.CustomToolStripComboBoxFormat.Name = "CustomToolStripComboBoxFormat";
            this.CustomToolStripComboBoxFormat.SelectedIndex = -1;
            this.CustomToolStripComboBoxFormat.SelectedItem = null;
            this.CustomToolStripComboBoxFormat.SelectedText = "";
            this.CustomToolStripComboBoxFormat.SelectionColor = System.Drawing.Color.Blue;
            this.CustomToolStripComboBoxFormat.SelectionLength = 0;
            this.CustomToolStripComboBoxFormat.SelectionStart = 0;
            this.CustomToolStripComboBoxFormat.Size = new System.Drawing.Size(130, 37);
            this.CustomToolStripComboBoxFormat.Sorted = false;
            this.CustomToolStripComboBoxFormat.CustomToolStripComboBox_SelectionIndexChanged += new System.EventHandler(this.CustomToolStripComboBoxFormat_SelectedIndexChanged);
            // 
            // ToolStripSeparator3
            // 
            this.ToolStripSeparator3.Name = "ToolStripSeparator3";
            this.ToolStripSeparator3.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripLabelEncoding
            // 
            this.ToolStripLabelEncoding.Name = "ToolStripLabelEncoding";
            this.ToolStripLabelEncoding.Size = new System.Drawing.Size(57, 37);
            this.ToolStripLabelEncoding.Text = "Encoding";
            // 
            // CustomToolStripComboBoxEncoding
            // 
            this.CustomToolStripComboBoxEncoding.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.None;
            this.CustomToolStripComboBoxEncoding.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.None;
            this.CustomToolStripComboBoxEncoding.BorderColor = System.Drawing.Color.Blue;
            this.CustomToolStripComboBoxEncoding.DropDownHeight = 106;
            this.CustomToolStripComboBoxEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CustomToolStripComboBoxEncoding.DropDownWidth = 130;
            this.CustomToolStripComboBoxEncoding.FlatStyle = System.Windows.Forms.FlatStyle.Standard;
            this.CustomToolStripComboBoxEncoding.IntegralHeight = false;
            this.CustomToolStripComboBoxEncoding.MaxDropDownItems = 8;
            this.CustomToolStripComboBoxEncoding.Name = "CustomToolStripComboBoxEncoding";
            this.CustomToolStripComboBoxEncoding.SelectedIndex = -1;
            this.CustomToolStripComboBoxEncoding.SelectedItem = null;
            this.CustomToolStripComboBoxEncoding.SelectedText = "";
            this.CustomToolStripComboBoxEncoding.SelectionColor = System.Drawing.Color.Red;
            this.CustomToolStripComboBoxEncoding.SelectionLength = 0;
            this.CustomToolStripComboBoxEncoding.SelectionStart = 0;
            this.CustomToolStripComboBoxEncoding.Size = new System.Drawing.Size(121, 37);
            this.CustomToolStripComboBoxEncoding.Sorted = false;
            this.CustomToolStripComboBoxEncoding.CustomToolStripComboBox_SelectionIndexChanged += new System.EventHandler(this.CustomToolStripComboBoxEncoding_SelectedIndexChanged);
            // 
            // ToolStripSeparator4
            // 
            this.ToolStripSeparator4.Name = "ToolStripSeparator4";
            this.ToolStripSeparator4.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripButtonSettings
            // 
            this.ToolStripButtonSettings.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonSettings.Image = global::PersianSubtitleFixes.Properties.Resources.Settings_Black;
            this.ToolStripButtonSettings.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonSettings.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonSettings.Name = "ToolStripButtonSettings";
            this.ToolStripButtonSettings.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonSettings.Text = "Settings";
            this.ToolStripButtonSettings.Click += new System.EventHandler(this.ToolStripButtonSettings_Click);
            // 
            // ToolStripButtonAbout
            // 
            this.ToolStripButtonAbout.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonAbout.Image = global::PersianSubtitleFixes.Properties.Resources.About_Black;
            this.ToolStripButtonAbout.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonAbout.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonAbout.Name = "ToolStripButtonAbout";
            this.ToolStripButtonAbout.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonAbout.Text = "About";
            this.ToolStripButtonAbout.Click += new System.EventHandler(this.ToolStripButtonAbout_Click);
            // 
            // ToolStripButtonExit
            // 
            this.ToolStripButtonExit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonExit.Image = global::PersianSubtitleFixes.Properties.Resources.Exit_Black;
            this.ToolStripButtonExit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonExit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonExit.Name = "ToolStripButtonExit";
            this.ToolStripButtonExit.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonExit.Text = "Exit";
            this.ToolStripButtonExit.Click += new System.EventHandler(this.ToolStripButtonExit_Click);
            // 
            // CustomButtonStop
            // 
            this.CustomButtonStop.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomButtonStop.BorderColor = System.Drawing.Color.Red;
            this.CustomButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonStop.Location = new System.Drawing.Point(808, 365);
            this.CustomButtonStop.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonStop.Name = "CustomButtonStop";
            this.CustomButtonStop.RoundedCorners = 0;
            this.CustomButtonStop.SelectionColor = System.Drawing.Color.Blue;
            this.CustomButtonStop.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonStop.TabIndex = 14;
            this.CustomButtonStop.Text = "Stop";
            this.CustomButtonStop.UseVisualStyleBackColor = false;
            this.CustomButtonStop.Click += new System.EventHandler(this.CustomButtonStop_Click);
            // 
            // CustomButtonApply
            // 
            this.CustomButtonApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomButtonApply.BorderColor = System.Drawing.Color.Red;
            this.CustomButtonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonApply.Location = new System.Drawing.Point(731, 365);
            this.CustomButtonApply.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonApply.Name = "CustomButtonApply";
            this.CustomButtonApply.RoundedCorners = 0;
            this.CustomButtonApply.SelectionColor = System.Drawing.Color.Blue;
            this.CustomButtonApply.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonApply.TabIndex = 15;
            this.CustomButtonApply.Text = "Apply";
            this.CustomButtonApply.UseVisualStyleBackColor = false;
            this.CustomButtonApply.Click += new System.EventHandler(this.CustomButtonApply_Click);
            // 
            // CustomButtonInvertCheck
            // 
            this.CustomButtonInvertCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomButtonInvertCheck.BorderColor = System.Drawing.Color.Red;
            this.CustomButtonInvertCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonInvertCheck.Location = new System.Drawing.Point(295, 365);
            this.CustomButtonInvertCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonInvertCheck.Name = "CustomButtonInvertCheck";
            this.CustomButtonInvertCheck.RoundedCorners = 0;
            this.CustomButtonInvertCheck.SelectionColor = System.Drawing.Color.Blue;
            this.CustomButtonInvertCheck.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonInvertCheck.TabIndex = 16;
            this.CustomButtonInvertCheck.Text = "Invert";
            this.CustomButtonInvertCheck.UseVisualStyleBackColor = false;
            this.CustomButtonInvertCheck.Click += new System.EventHandler(this.SelectionHandler);
            // 
            // CustomButtonCheckAll
            // 
            this.CustomButtonCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomButtonCheckAll.BorderColor = System.Drawing.Color.Red;
            this.CustomButtonCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonCheckAll.Location = new System.Drawing.Point(218, 365);
            this.CustomButtonCheckAll.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonCheckAll.Name = "CustomButtonCheckAll";
            this.CustomButtonCheckAll.RoundedCorners = 0;
            this.CustomButtonCheckAll.SelectionColor = System.Drawing.Color.Blue;
            this.CustomButtonCheckAll.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonCheckAll.TabIndex = 17;
            this.CustomButtonCheckAll.Text = "Select All";
            this.CustomButtonCheckAll.UseVisualStyleBackColor = false;
            this.CustomButtonCheckAll.Click += new System.EventHandler(this.SelectionHandler);
            // 
            // CustomProgressBar1
            // 
            this.CustomProgressBar1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomProgressBar1.BorderColor = System.Drawing.Color.Red;
            this.CustomProgressBar1.ChunksColor = System.Drawing.Color.LightBlue;
            this.CustomProgressBar1.CustomText = "MSasanMH";
            this.CustomProgressBar1.ForeColor = System.Drawing.Color.Black;
            this.CustomProgressBar1.Location = new System.Drawing.Point(372, 365);
            this.CustomProgressBar1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomProgressBar1.Name = "CustomProgressBar1";
            this.CustomProgressBar1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomProgressBar1.Size = new System.Drawing.Size(357, 23);
            this.CustomProgressBar1.StartTime = null;
            this.CustomProgressBar1.TabIndex = 18;
            this.CustomProgressBar1.Value = 50;
            // 
            // CustomPanel1
            // 
            this.CustomPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomPanel1.Border = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomPanel1.BorderColor = System.Drawing.Color.Red;
            this.CustomPanel1.ButtonBorderStyle = System.Windows.Forms.ButtonBorderStyle.Solid;
            this.CustomPanel1.Controls.Add(this.MenuStrip1);
            this.CustomPanel1.Location = new System.Drawing.Point(1, 41);
            this.CustomPanel1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomPanel1.Name = "CustomPanel1";
            this.CustomPanel1.Size = new System.Drawing.Size(215, 322);
            this.CustomPanel1.TabIndex = 20;
            // 
            // CustomDataGridView1
            // 
            this.CustomDataGridView1.AllowUserToAddRows = false;
            this.CustomDataGridView1.AllowUserToDeleteRows = false;
            this.CustomDataGridView1.AllowUserToResizeRows = false;
            this.CustomDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomDataGridView1.BorderColor = System.Drawing.Color.Red;
            this.CustomDataGridView1.CheckColor = System.Drawing.Color.Blue;
            this.CustomDataGridView1.ColumnHeadersBorder = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle1.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(73)))), ((int)(((byte)(73)))), ((int)(((byte)(73)))));
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.CustomDataGridView1.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.CustomDataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.CustomDataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ColumnApply,
            this.ColumnLine,
            this.ColumnBefore,
            this.ColumnAfter});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CustomDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.CustomDataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.CustomDataGridView1.Location = new System.Drawing.Point(218, 41);
            this.CustomDataGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomDataGridView1.MultiSelect = false;
            this.CustomDataGridView1.Name = "CustomDataGridView1";
            this.CustomDataGridView1.RowHeadersVisible = false;
            this.CustomDataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CustomDataGridView1.RowTemplate.Height = 25;
            this.CustomDataGridView1.SelectionColor = System.Drawing.Color.Blue;
            this.CustomDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CustomDataGridView1.ShowCellToolTips = false;
            this.CustomDataGridView1.Size = new System.Drawing.Size(665, 322);
            this.CustomDataGridView1.TabIndex = 21;
            this.CustomDataGridView1.SelectionChanged += new System.EventHandler(this.CustomDataGridView1_SelectionChanged);
            // 
            // ColumnApply
            // 
            this.ColumnApply.HeaderText = "Apply";
            this.ColumnApply.Name = "ColumnApply";
            this.ColumnApply.ReadOnly = true;
            this.ColumnApply.Width = 45;
            // 
            // ColumnLine
            // 
            this.ColumnLine.HeaderText = "Line#";
            this.ColumnLine.Name = "ColumnLine";
            this.ColumnLine.ReadOnly = true;
            this.ColumnLine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnLine.Width = 45;
            // 
            // ColumnBefore
            // 
            this.ColumnBefore.HeaderText = "Before";
            this.ColumnBefore.Name = "ColumnBefore";
            this.ColumnBefore.ReadOnly = true;
            this.ColumnBefore.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnBefore.Width = 400;
            // 
            // ColumnAfter
            // 
            this.ColumnAfter.HeaderText = "After";
            this.ColumnAfter.Name = "ColumnAfter";
            this.ColumnAfter.ReadOnly = true;
            this.ColumnAfter.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnAfter.Width = 400;
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.ClientSize = new System.Drawing.Size(884, 411);
            this.Controls.Add(this.CustomDataGridView1);
            this.Controls.Add(this.CustomPanel1);
            this.Controls.Add(this.CustomProgressBar1);
            this.Controls.Add(this.CustomButtonCheckAll);
            this.Controls.Add(this.CustomButtonInvertCheck);
            this.Controls.Add(this.CustomButtonApply);
            this.Controls.Add(this.CustomButtonStop);
            this.Controls.Add(this.ToolStrip1);
            this.Controls.Add(this.StatusStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MenuStrip1;
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.StatusStrip1.ResumeLayout(false);
            this.StatusStrip1.PerformLayout();
            this.MenuStrip1.ResumeLayout(false);
            this.MenuStrip1.PerformLayout();
            this.ToolStrip1.ResumeLayout(false);
            this.ToolStrip1.PerformLayout();
            this.CustomPanel1.ResumeLayout(false);
            this.CustomPanel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomDataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private StatusStrip StatusStrip1;
        private MenuStrip MenuStrip1;
        private ToolStripMenuItem FileToolStripMenuItem;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private ToolStripMenuItem EditToolStripMenuItem;
        private ToolStripMenuItem UndoToolStripMenuItem;
        private ToolStripMenuItem RedoToolStripMenuItem;
        private ToolStripMenuItem AboutToolStripMenuItem;
        private ToolStripStatusLabel ToolStripLabelLeft;
        private ToolStripStatusLabel ToolStripLabelSpace1;
        private ToolStripStatusLabel ToolStripLabelRight;
        private System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private ToolStrip ToolStrip1;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripButton ToolStripButtonOpen;
        private ToolStripButton ToolStripButtonSave;
        private ToolStripButton ToolStripButtonSaveAs;
        private ToolStripSeparator ToolStripSeparator1;
        private ToolStripButton ToolStripButtonUndo;
        private ToolStripButton ToolStripButtonRedo;
        private ToolStripSeparator ToolStripSeparator2;
        private ToolStripLabel ToolStripLabelEncoding;
        private ToolStripMenuItem SaveAsToolStripMenuItem;
        private ToolStripSeparator ToolStripSeparator4;
        private ToolStripButton ToolStripButtonExit;
        private ToolStripButton ToolStripButtonAbout;
        private ToolStripLabel ToolStripLabelFormat;
        private ToolStripSeparator ToolStripSeparator3;
        private ToolStripButton ToolStripButtonSettings;
        private CustomControls.CustomToolStripComboBox CustomToolStripComboBoxEncoding;
        private CustomControls.CustomToolStripComboBox CustomToolStripComboBoxFormat;
        private ToolStripSeparator ToolStripSeparator5;
        private CustomControls.CustomButton CustomButtonStop;
        private CustomControls.CustomButton CustomButtonApply;
        private CustomControls.CustomButton CustomButtonInvertCheck;
        private CustomControls.CustomButton CustomButtonCheckAll;
        private CustomControls.CustomProgressBar CustomProgressBar1;
        private CustomControls.CustomCheckBox customCheckBox1;
        private CustomControls.CustomPanel CustomPanel1;
        private CustomControls.CustomDataGridView CustomDataGridView1;
        private DataGridViewCheckBoxColumn ColumnApply;
        private DataGridViewTextBoxColumn ColumnLine;
        private DataGridViewTextBoxColumn ColumnBefore;
        private DataGridViewTextBoxColumn ColumnAfter;
    }
}