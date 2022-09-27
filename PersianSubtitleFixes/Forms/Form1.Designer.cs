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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.BackgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.CustomButtonStop = new CustomControls.CustomButton();
            this.CustomButtonApply = new CustomControls.CustomButton();
            this.CustomButtonInvertCheck = new CustomControls.CustomButton();
            this.CustomButtonCheckAll = new CustomControls.CustomButton();
            this.CustomProgressBar1 = new CustomControls.CustomProgressBar();
            this.CustomDataGridView1 = new CustomControls.CustomDataGridView();
            this.ColumnLine = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnStartTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnEndTime = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnDuration = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ColumnText = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.CustomTabControl1 = new CustomControls.CustomTabControl();
            this.TabPageCommonErrors = new System.Windows.Forms.TabPage();
            this.CustomButtonCancel = new CustomControls.CustomButton();
            this.TabPageTiming = new System.Windows.Forms.TabPage();
            this.CustomGroupBoxFixTiming = new CustomControls.CustomGroupBox();
            this.CustomLabelSameTimeDifference = new CustomControls.CustomLabel();
            this.CustomCheckBoxFixIncorrectTimeOrder = new CustomControls.CustomCheckBox();
            this.CustomLabelMinGap = new CustomControls.CustomLabel();
            this.CustomLabelMaxDurationLimit = new CustomControls.CustomLabel();
            this.CustomLabelMinDurationlimit = new CustomControls.CustomLabel();
            this.CustomCheckBoxMergeSameTime = new CustomControls.CustomCheckBox();
            this.CustomNumericUpDownSameTimeCode = new CustomControls.CustomNumericUpDown();
            this.CustomNumericUpDownMinDL = new CustomControls.CustomNumericUpDown();
            this.CustomRadioButtonCharsPerSec = new CustomControls.CustomRadioButton();
            this.CustomNumericUpDownMaxDL = new CustomControls.CustomNumericUpDown();
            this.CustomNumericUpDownCharsPerSec = new CustomControls.CustomNumericUpDown();
            this.CustomNumericUpDownMinGap = new CustomControls.CustomNumericUpDown();
            this.CustomCheckBoxAdjustDurations = new CustomControls.CustomCheckBox();
            this.CustomButtonApplyTiming = new CustomControls.CustomButton();
            this.CustomCheckBoxFixNegative = new CustomControls.CustomCheckBox();
            this.CustomButtonResetTiming = new CustomControls.CustomButton();
            this.TabPageSync = new System.Windows.Forms.TabPage();
            this.CustomGroupBoxSync = new CustomControls.CustomGroupBox();
            this.CustomGroupBoxChangeSpeed = new CustomControls.CustomGroupBox();
            this.CustomButtonChangeSpeed = new CustomControls.CustomButton();
            this.CustomNumericUpDownChangeSpeed = new CustomControls.CustomNumericUpDown();
            this.CustomRadioButtonChangeSpeedTDF = new CustomControls.CustomRadioButton();
            this.CustomRadioButtonChangeSpeedFDF = new CustomControls.CustomRadioButton();
            this.CustomRadioButtonChangeSpeedC = new CustomControls.CustomRadioButton();
            this.CustomGroupBoxChangeFrameRate = new CustomControls.CustomGroupBox();
            this.CustomLabelToFrameRate = new CustomControls.CustomLabel();
            this.CustomLabelFromFrameRate = new CustomControls.CustomLabel();
            this.CustomComboBoxFromFrameRate = new CustomControls.CustomComboBox();
            this.CustomComboBoxToFrameRate = new CustomControls.CustomComboBox();
            this.CustomButtonChangeFrameRate = new CustomControls.CustomButton();
            this.CustomGroupBoxAdjustAllTimes = new CustomControls.CustomGroupBox();
            this.CustomLabelAdjustAllTimes = new CustomControls.CustomLabel();
            this.CustomTimeUpDownAdjustTime = new CustomControls.CustomTimeUpDown();
            this.CustomButtonSyncEarlier = new CustomControls.CustomButton();
            this.CustomButtonSyncLater = new CustomControls.CustomButton();
            this.TabPageOthers = new System.Windows.Forms.TabPage();
            this.CustomGroupBoxOthers = new CustomControls.CustomGroupBox();
            this.CustomLabelSameTextDifference = new CustomControls.CustomLabel();
            this.CustomCheckBoxRemoveUCChars = new CustomControls.CustomCheckBox();
            this.CustomNumericUpDownSameText = new CustomControls.CustomNumericUpDown();
            this.CustomCheckBoxMergeSameText = new CustomControls.CustomCheckBox();
            this.CustomButtonResetOthers = new CustomControls.CustomButton();
            this.CustomButtonApplyOthers = new CustomControls.CustomButton();
            this.CustomCheckBoxRemoveEmptyLines = new CustomControls.CustomCheckBox();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.BottomToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.TopToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.RightToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.LeftToolStripPanel = new System.Windows.Forms.ToolStripPanel();
            this.ContentPanel = new System.Windows.Forms.ToolStripContentPanel();
            this.CustomMenuStrip1 = new CustomControls.CustomMenuStrip();
            this.FileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.OpenToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.SaveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
            this.ExitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.EditToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.UndoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RedoToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ViewToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ShowPopupGuideToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ToolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FixNegativeTimingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.FixIncorrectTimeOrderToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveEmptyLinesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.RemoveUnicodeControlCharsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.AboutToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.CustomToolStrip1 = new CustomControls.CustomToolStrip();
            this.ToolStripButtonOpen = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSave = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSaveAs = new System.Windows.Forms.ToolStripButton();
            this.ToolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonUndo = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonRedo = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabelFormat = new System.Windows.Forms.ToolStripLabel();
            this.CustomToolStripComboBoxFormat = new CustomControls.CustomToolStripComboBox();
            this.ToolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripLabelEncoding = new System.Windows.Forms.ToolStripLabel();
            this.CustomToolStripComboBoxEncoding = new CustomControls.CustomToolStripComboBox();
            this.ToolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButtonEdit = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonSettings = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonAbout = new System.Windows.Forms.ToolStripButton();
            this.ToolStripButtonExit = new System.Windows.Forms.ToolStripButton();
            this.CustomStatusStrip1 = new CustomControls.CustomStatusStrip();
            this.ToolStripLabelLeft = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabelSpace1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.ToolStripLabelRight = new System.Windows.Forms.ToolStripStatusLabel();
            this.customContextMenuStrip1 = new CustomControls.CustomContextMenuStrip();
            this.test1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.test2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item1ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item2ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.item3ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            ((System.ComponentModel.ISupportInitialize)(this.CustomDataGridView1)).BeginInit();
            this.CustomTabControl1.SuspendLayout();
            this.TabPageCommonErrors.SuspendLayout();
            this.TabPageTiming.SuspendLayout();
            this.CustomGroupBoxFixTiming.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownSameTimeCode)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMinDL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMaxDL)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownCharsPerSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMinGap)).BeginInit();
            this.TabPageSync.SuspendLayout();
            this.CustomGroupBoxSync.SuspendLayout();
            this.CustomGroupBoxChangeSpeed.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownChangeSpeed)).BeginInit();
            this.CustomGroupBoxChangeFrameRate.SuspendLayout();
            this.CustomGroupBoxAdjustAllTimes.SuspendLayout();
            this.TabPageOthers.SuspendLayout();
            this.CustomGroupBoxOthers.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownSameText)).BeginInit();
            this.CustomMenuStrip1.SuspendLayout();
            this.CustomToolStrip1.SuspendLayout();
            this.CustomStatusStrip1.SuspendLayout();
            this.customContextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BackgroundWorker1
            // 
            this.BackgroundWorker1.DoWork += new System.ComponentModel.DoWorkEventHandler(this.BackgroundWorker1_DoWork);
            // 
            // CustomButtonStop
            // 
            this.CustomButtonStop.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonStop.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonStop.Location = new System.Drawing.Point(121, 246);
            this.CustomButtonStop.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonStop.Name = "CustomButtonStop";
            this.CustomButtonStop.RoundedCorners = 0;
            this.CustomButtonStop.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonStop.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonStop.TabIndex = 14;
            this.CustomButtonStop.Text = "Stop";
            this.CustomButtonStop.UseVisualStyleBackColor = false;
            this.CustomButtonStop.Click += new System.EventHandler(this.CustomButtonStop_Click);
            // 
            // CustomButtonApply
            // 
            this.CustomButtonApply.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonApply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonApply.Location = new System.Drawing.Point(26, 246);
            this.CustomButtonApply.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonApply.Name = "CustomButtonApply";
            this.CustomButtonApply.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomButtonApply.RoundedCorners = 0;
            this.CustomButtonApply.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonApply.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonApply.TabIndex = 15;
            this.CustomButtonApply.Text = "Apply";
            this.CustomButtonApply.UseVisualStyleBackColor = false;
            this.CustomButtonApply.Click += new System.EventHandler(this.CustomButtonApply_Click);
            // 
            // CustomButtonInvertCheck
            // 
            this.CustomButtonInvertCheck.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomButtonInvertCheck.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonInvertCheck.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonInvertCheck.Location = new System.Drawing.Point(310, 415);
            this.CustomButtonInvertCheck.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonInvertCheck.Name = "CustomButtonInvertCheck";
            this.CustomButtonInvertCheck.RoundedCorners = 0;
            this.CustomButtonInvertCheck.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonInvertCheck.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonInvertCheck.TabIndex = 16;
            this.CustomButtonInvertCheck.Text = "Invert";
            this.CustomButtonInvertCheck.UseVisualStyleBackColor = false;
            this.CustomButtonInvertCheck.Click += new System.EventHandler(this.SelectionHandler);
            // 
            // CustomButtonCheckAll
            // 
            this.CustomButtonCheckAll.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomButtonCheckAll.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonCheckAll.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonCheckAll.Location = new System.Drawing.Point(233, 415);
            this.CustomButtonCheckAll.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonCheckAll.Name = "CustomButtonCheckAll";
            this.CustomButtonCheckAll.RoundedCorners = 0;
            this.CustomButtonCheckAll.SelectionColor = System.Drawing.Color.LightBlue;
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
            this.CustomProgressBar1.BackColor = System.Drawing.Color.DimGray;
            this.CustomProgressBar1.BorderColor = System.Drawing.Color.Blue;
            this.CustomProgressBar1.ChunksColor = System.Drawing.Color.LightBlue;
            this.CustomProgressBar1.CustomText = "MSasanMH";
            this.CustomProgressBar1.ForeColor = System.Drawing.Color.Black;
            this.CustomProgressBar1.Location = new System.Drawing.Point(387, 415);
            this.CustomProgressBar1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomProgressBar1.Name = "CustomProgressBar1";
            this.CustomProgressBar1.Size = new System.Drawing.Size(496, 23);
            this.CustomProgressBar1.StartTime = null;
            this.CustomProgressBar1.TabIndex = 18;
            this.CustomProgressBar1.Value = 50;
            // 
            // CustomDataGridView1
            // 
            this.CustomDataGridView1.AllowUserToAddRows = false;
            this.CustomDataGridView1.AllowUserToDeleteRows = false;
            this.CustomDataGridView1.AllowUserToResizeRows = false;
            this.CustomDataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomDataGridView1.BorderColor = System.Drawing.Color.Blue;
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
            this.ColumnLine,
            this.ColumnStartTime,
            this.ColumnEndTime,
            this.ColumnDuration,
            this.ColumnText});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.Color.DimGray;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            dataGridViewCellStyle2.ForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(76)))), ((int)(((byte)(76)))), ((int)(((byte)(255)))));
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.Color.White;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.CustomDataGridView1.DefaultCellStyle = dataGridViewCellStyle2;
            this.CustomDataGridView1.GridColor = System.Drawing.Color.LightBlue;
            this.CustomDataGridView1.Location = new System.Drawing.Point(233, 65);
            this.CustomDataGridView1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomDataGridView1.MultiSelect = false;
            this.CustomDataGridView1.Name = "CustomDataGridView1";
            this.CustomDataGridView1.RowHeadersVisible = false;
            this.CustomDataGridView1.RowHeadersWidthSizeMode = System.Windows.Forms.DataGridViewRowHeadersWidthSizeMode.DisableResizing;
            this.CustomDataGridView1.RowTemplate.Height = 25;
            this.CustomDataGridView1.SelectionColor = System.Drawing.Color.Blue;
            this.CustomDataGridView1.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.CustomDataGridView1.SelectionModeFocus = true;
            this.CustomDataGridView1.ShowCellToolTips = false;
            this.CustomDataGridView1.Size = new System.Drawing.Size(650, 348);
            this.CustomDataGridView1.TabIndex = 21;
            this.CustomDataGridView1.SelectionChanged += new System.EventHandler(this.CustomDataGridView1_SelectionChanged);
            // 
            // ColumnLine
            // 
            this.ColumnLine.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnLine.HeaderText = "Line#";
            this.ColumnLine.Name = "ColumnLine";
            this.ColumnLine.ReadOnly = true;
            this.ColumnLine.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnLine.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnLine.Width = 41;
            // 
            // ColumnStartTime
            // 
            this.ColumnStartTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnStartTime.HeaderText = "Start time";
            this.ColumnStartTime.Name = "ColumnStartTime";
            this.ColumnStartTime.ReadOnly = true;
            this.ColumnStartTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnStartTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnStartTime.Width = 63;
            // 
            // ColumnEndTime
            // 
            this.ColumnEndTime.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnEndTime.HeaderText = "End time";
            this.ColumnEndTime.Name = "ColumnEndTime";
            this.ColumnEndTime.ReadOnly = true;
            this.ColumnEndTime.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnEndTime.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnEndTime.Width = 59;
            // 
            // ColumnDuration
            // 
            this.ColumnDuration.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ColumnDuration.HeaderText = "Duration";
            this.ColumnDuration.Name = "ColumnDuration";
            this.ColumnDuration.ReadOnly = true;
            this.ColumnDuration.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnDuration.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            this.ColumnDuration.Width = 58;
            // 
            // ColumnText
            // 
            this.ColumnText.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill;
            this.ColumnText.HeaderText = "Text";
            this.ColumnText.Name = "ColumnText";
            this.ColumnText.ReadOnly = true;
            this.ColumnText.Resizable = System.Windows.Forms.DataGridViewTriState.False;
            this.ColumnText.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // CustomTabControl1
            // 
            this.CustomTabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomTabControl1.BorderColor = System.Drawing.Color.Blue;
            this.CustomTabControl1.Controls.Add(this.TabPageCommonErrors);
            this.CustomTabControl1.Controls.Add(this.TabPageTiming);
            this.CustomTabControl1.Controls.Add(this.TabPageSync);
            this.CustomTabControl1.Controls.Add(this.TabPageOthers);
            this.CustomTabControl1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CustomTabControl1.HideTabHeader = false;
            this.CustomTabControl1.Location = new System.Drawing.Point(1, 65);
            this.CustomTabControl1.Margin = new System.Windows.Forms.Padding(1);
            this.CustomTabControl1.Multiline = true;
            this.CustomTabControl1.Name = "CustomTabControl1";
            this.CustomTabControl1.Padding = new System.Drawing.Point(6, 1);
            this.CustomTabControl1.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomTabControl1.SelectedIndex = 0;
            this.CustomTabControl1.Size = new System.Drawing.Size(230, 373);
            this.CustomTabControl1.SizeMode = System.Windows.Forms.TabSizeMode.FillToRight;
            this.CustomTabControl1.TabIndex = 22;
            this.CustomTabControl1.Tag = 0;
            // 
            // TabPageCommonErrors
            // 
            this.TabPageCommonErrors.AutoScroll = true;
            this.TabPageCommonErrors.BackColor = System.Drawing.Color.Transparent;
            this.TabPageCommonErrors.Controls.Add(this.CustomButtonCancel);
            this.TabPageCommonErrors.Controls.Add(this.CustomButtonApply);
            this.TabPageCommonErrors.Controls.Add(this.CustomButtonStop);
            this.TabPageCommonErrors.Location = new System.Drawing.Point(4, 42);
            this.TabPageCommonErrors.Margin = new System.Windows.Forms.Padding(1);
            this.TabPageCommonErrors.Name = "TabPageCommonErrors";
            this.TabPageCommonErrors.Padding = new System.Windows.Forms.Padding(1);
            this.TabPageCommonErrors.Size = new System.Drawing.Size(222, 327);
            this.TabPageCommonErrors.TabIndex = 0;
            this.TabPageCommonErrors.Tag = 0;
            this.TabPageCommonErrors.Text = "Common errors";
            // 
            // CustomButtonCancel
            // 
            this.CustomButtonCancel.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonCancel.Location = new System.Drawing.Point(74, 283);
            this.CustomButtonCancel.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonCancel.Name = "CustomButtonCancel";
            this.CustomButtonCancel.RoundedCorners = 0;
            this.CustomButtonCancel.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonCancel.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonCancel.TabIndex = 16;
            this.CustomButtonCancel.Text = "Cancel";
            this.CustomButtonCancel.UseVisualStyleBackColor = true;
            this.CustomButtonCancel.Click += new System.EventHandler(this.CustomButtonCancel_Click);
            // 
            // TabPageTiming
            // 
            this.TabPageTiming.BackColor = System.Drawing.Color.Transparent;
            this.TabPageTiming.Controls.Add(this.CustomGroupBoxFixTiming);
            this.TabPageTiming.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TabPageTiming.ImageKey = "(none)";
            this.TabPageTiming.Location = new System.Drawing.Point(4, 42);
            this.TabPageTiming.Margin = new System.Windows.Forms.Padding(1);
            this.TabPageTiming.Name = "TabPageTiming";
            this.TabPageTiming.Padding = new System.Windows.Forms.Padding(1);
            this.TabPageTiming.Size = new System.Drawing.Size(222, 327);
            this.TabPageTiming.TabIndex = 1;
            this.TabPageTiming.Tag = 1;
            this.TabPageTiming.Text = "Timing";
            // 
            // CustomGroupBoxFixTiming
            // 
            this.CustomGroupBoxFixTiming.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomGroupBoxFixTiming.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomLabelSameTimeDifference);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomCheckBoxFixIncorrectTimeOrder);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomLabelMinGap);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomLabelMaxDurationLimit);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomLabelMinDurationlimit);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomCheckBoxMergeSameTime);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomNumericUpDownSameTimeCode);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomNumericUpDownMinDL);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomRadioButtonCharsPerSec);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomNumericUpDownMaxDL);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomNumericUpDownCharsPerSec);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomNumericUpDownMinGap);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomCheckBoxAdjustDurations);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomButtonApplyTiming);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomCheckBoxFixNegative);
            this.CustomGroupBoxFixTiming.Controls.Add(this.CustomButtonResetTiming);
            this.CustomGroupBoxFixTiming.Location = new System.Drawing.Point(1, 1);
            this.CustomGroupBoxFixTiming.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxFixTiming.Name = "CustomGroupBoxFixTiming";
            this.CustomGroupBoxFixTiming.Size = new System.Drawing.Size(220, 325);
            this.CustomGroupBoxFixTiming.TabIndex = 18;
            this.CustomGroupBoxFixTiming.TabStop = false;
            this.CustomGroupBoxFixTiming.Text = "Fix timing";
            // 
            // CustomLabelSameTimeDifference
            // 
            this.CustomLabelSameTimeDifference.AutoSize = true;
            this.CustomLabelSameTimeDifference.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelSameTimeDifference.Border = false;
            this.CustomLabelSameTimeDifference.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelSameTimeDifference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelSameTimeDifference.Location = new System.Drawing.Point(20, 217);
            this.CustomLabelSameTimeDifference.Name = "CustomLabelSameTimeDifference";
            this.CustomLabelSameTimeDifference.RoundedCorners = 0;
            this.CustomLabelSameTimeDifference.Size = new System.Drawing.Size(121, 15);
            this.CustomLabelSameTimeDifference.TabIndex = 21;
            this.CustomLabelSameTimeDifference.Text = "Max. MSec difference";
            // 
            // CustomCheckBoxFixIncorrectTimeOrder
            // 
            this.CustomCheckBoxFixIncorrectTimeOrder.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxFixIncorrectTimeOrder.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxFixIncorrectTimeOrder.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxFixIncorrectTimeOrder.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxFixIncorrectTimeOrder.Location = new System.Drawing.Point(5, 40);
            this.CustomCheckBoxFixIncorrectTimeOrder.Name = "CustomCheckBoxFixIncorrectTimeOrder";
            this.CustomCheckBoxFixIncorrectTimeOrder.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxFixIncorrectTimeOrder.Size = new System.Drawing.Size(147, 17);
            this.CustomCheckBoxFixIncorrectTimeOrder.TabIndex = 20;
            this.CustomCheckBoxFixIncorrectTimeOrder.Text = "Fix incorrect time order";
            this.CustomCheckBoxFixIncorrectTimeOrder.UseVisualStyleBackColor = false;
            this.CustomCheckBoxFixIncorrectTimeOrder.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomLabelMinGap
            // 
            this.CustomLabelMinGap.AutoSize = true;
            this.CustomLabelMinGap.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelMinGap.Border = false;
            this.CustomLabelMinGap.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelMinGap.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelMinGap.Location = new System.Drawing.Point(2, 170);
            this.CustomLabelMinGap.Name = "CustomLabelMinGap";
            this.CustomLabelMinGap.RoundedCorners = 0;
            this.CustomLabelMinGap.Size = new System.Drawing.Size(110, 15);
            this.CustomLabelMinGap.TabIndex = 19;
            this.CustomLabelMinGap.Text = "Minimum gap (ms)";
            // 
            // CustomLabelMaxDurationLimit
            // 
            this.CustomLabelMaxDurationLimit.AutoSize = true;
            this.CustomLabelMaxDurationLimit.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelMaxDurationLimit.Border = false;
            this.CustomLabelMaxDurationLimit.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelMaxDurationLimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelMaxDurationLimit.Location = new System.Drawing.Point(2, 140);
            this.CustomLabelMaxDurationLimit.Name = "CustomLabelMaxDurationLimit";
            this.CustomLabelMaxDurationLimit.RoundedCorners = 0;
            this.CustomLabelMaxDurationLimit.Size = new System.Drawing.Size(132, 15);
            this.CustomLabelMaxDurationLimit.TabIndex = 18;
            this.CustomLabelMaxDurationLimit.Text = "Max duration limit (ms)";
            // 
            // CustomLabelMinDurationlimit
            // 
            this.CustomLabelMinDurationlimit.AutoSize = true;
            this.CustomLabelMinDurationlimit.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelMinDurationlimit.Border = false;
            this.CustomLabelMinDurationlimit.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelMinDurationlimit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelMinDurationlimit.Location = new System.Drawing.Point(2, 110);
            this.CustomLabelMinDurationlimit.Name = "CustomLabelMinDurationlimit";
            this.CustomLabelMinDurationlimit.RoundedCorners = 0;
            this.CustomLabelMinDurationlimit.Size = new System.Drawing.Size(130, 15);
            this.CustomLabelMinDurationlimit.TabIndex = 17;
            this.CustomLabelMinDurationlimit.Text = "Min duration limit (ms)";
            // 
            // CustomCheckBoxMergeSameTime
            // 
            this.CustomCheckBoxMergeSameTime.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxMergeSameTime.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxMergeSameTime.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxMergeSameTime.Checked = true;
            this.CustomCheckBoxMergeSameTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomCheckBoxMergeSameTime.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxMergeSameTime.Location = new System.Drawing.Point(5, 195);
            this.CustomCheckBoxMergeSameTime.Margin = new System.Windows.Forms.Padding(1);
            this.CustomCheckBoxMergeSameTime.Name = "CustomCheckBoxMergeSameTime";
            this.CustomCheckBoxMergeSameTime.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxMergeSameTime.Size = new System.Drawing.Size(200, 17);
            this.CustomCheckBoxMergeSameTime.TabIndex = 14;
            this.CustomCheckBoxMergeSameTime.Text = "Merge lines with same time code";
            this.CustomCheckBoxMergeSameTime.UseVisualStyleBackColor = false;
            this.CustomCheckBoxMergeSameTime.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownSameTimeCode
            // 
            this.CustomNumericUpDownSameTimeCode.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownSameTimeCode.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownSameTimeCode.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownSameTimeCode.Enabled = false;
            this.CustomNumericUpDownSameTimeCode.Location = new System.Drawing.Point(160, 215);
            this.CustomNumericUpDownSameTimeCode.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownSameTimeCode.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.CustomNumericUpDownSameTimeCode.Name = "CustomNumericUpDownSameTimeCode";
            this.CustomNumericUpDownSameTimeCode.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownSameTimeCode.TabIndex = 15;
            this.CustomNumericUpDownSameTimeCode.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownMinDL
            // 
            this.CustomNumericUpDownMinDL.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownMinDL.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownMinDL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownMinDL.Location = new System.Drawing.Point(160, 108);
            this.CustomNumericUpDownMinDL.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownMinDL.Maximum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.CustomNumericUpDownMinDL.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CustomNumericUpDownMinDL.Name = "CustomNumericUpDownMinDL";
            this.CustomNumericUpDownMinDL.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomNumericUpDownMinDL.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownMinDL.TabIndex = 3;
            this.CustomNumericUpDownMinDL.Value = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.CustomNumericUpDownMinDL.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomRadioButtonCharsPerSec
            // 
            this.CustomRadioButtonCharsPerSec.BackColor = System.Drawing.Color.DimGray;
            this.CustomRadioButtonCharsPerSec.BorderColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonCharsPerSec.CheckColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonCharsPerSec.Checked = true;
            this.CustomRadioButtonCharsPerSec.Enabled = false;
            this.CustomRadioButtonCharsPerSec.ForeColor = System.Drawing.Color.White;
            this.CustomRadioButtonCharsPerSec.Location = new System.Drawing.Point(16, 80);
            this.CustomRadioButtonCharsPerSec.Margin = new System.Windows.Forms.Padding(1);
            this.CustomRadioButtonCharsPerSec.Name = "CustomRadioButtonCharsPerSec";
            this.CustomRadioButtonCharsPerSec.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomRadioButtonCharsPerSec.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomRadioButtonCharsPerSec.Size = new System.Drawing.Size(75, 17);
            this.CustomRadioButtonCharsPerSec.TabIndex = 13;
            this.CustomRadioButtonCharsPerSec.TabStop = true;
            this.CustomRadioButtonCharsPerSec.Text = "Chars/Sec";
            this.CustomRadioButtonCharsPerSec.UseVisualStyleBackColor = false;
            this.CustomRadioButtonCharsPerSec.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownMaxDL
            // 
            this.CustomNumericUpDownMaxDL.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownMaxDL.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownMaxDL.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownMaxDL.Location = new System.Drawing.Point(160, 138);
            this.CustomNumericUpDownMaxDL.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownMaxDL.Maximum = new decimal(new int[] {
            50000,
            0,
            0,
            0});
            this.CustomNumericUpDownMaxDL.Minimum = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.CustomNumericUpDownMaxDL.Name = "CustomNumericUpDownMaxDL";
            this.CustomNumericUpDownMaxDL.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownMaxDL.TabIndex = 4;
            this.CustomNumericUpDownMaxDL.Value = new decimal(new int[] {
            3000,
            0,
            0,
            0});
            this.CustomNumericUpDownMaxDL.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownCharsPerSec
            // 
            this.CustomNumericUpDownCharsPerSec.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownCharsPerSec.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownCharsPerSec.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownCharsPerSec.DecimalPlaces = 1;
            this.CustomNumericUpDownCharsPerSec.Enabled = false;
            this.CustomNumericUpDownCharsPerSec.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.CustomNumericUpDownCharsPerSec.Location = new System.Drawing.Point(160, 78);
            this.CustomNumericUpDownCharsPerSec.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownCharsPerSec.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.CustomNumericUpDownCharsPerSec.Name = "CustomNumericUpDownCharsPerSec";
            this.CustomNumericUpDownCharsPerSec.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownCharsPerSec.TabIndex = 11;
            this.CustomNumericUpDownCharsPerSec.Value = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.CustomNumericUpDownCharsPerSec.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownMinGap
            // 
            this.CustomNumericUpDownMinGap.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownMinGap.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownMinGap.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownMinGap.Location = new System.Drawing.Point(160, 168);
            this.CustomNumericUpDownMinGap.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownMinGap.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.CustomNumericUpDownMinGap.Name = "CustomNumericUpDownMinGap";
            this.CustomNumericUpDownMinGap.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownMinGap.TabIndex = 5;
            this.CustomNumericUpDownMinGap.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomCheckBoxAdjustDurations
            // 
            this.CustomCheckBoxAdjustDurations.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxAdjustDurations.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxAdjustDurations.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxAdjustDurations.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxAdjustDurations.Location = new System.Drawing.Point(5, 60);
            this.CustomCheckBoxAdjustDurations.Margin = new System.Windows.Forms.Padding(1);
            this.CustomCheckBoxAdjustDurations.Name = "CustomCheckBoxAdjustDurations";
            this.CustomCheckBoxAdjustDurations.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.CustomCheckBoxAdjustDurations.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxAdjustDurations.Size = new System.Drawing.Size(110, 17);
            this.CustomCheckBoxAdjustDurations.TabIndex = 9;
            this.CustomCheckBoxAdjustDurations.Text = "Adjust durations";
            this.CustomCheckBoxAdjustDurations.UseVisualStyleBackColor = false;
            this.CustomCheckBoxAdjustDurations.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomButtonApplyTiming
            // 
            this.CustomButtonApplyTiming.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonApplyTiming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonApplyTiming.Location = new System.Drawing.Point(26, 255);
            this.CustomButtonApplyTiming.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonApplyTiming.Name = "CustomButtonApplyTiming";
            this.CustomButtonApplyTiming.RoundedCorners = 0;
            this.CustomButtonApplyTiming.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonApplyTiming.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonApplyTiming.TabIndex = 6;
            this.CustomButtonApplyTiming.Text = "Apply";
            this.CustomButtonApplyTiming.UseVisualStyleBackColor = true;
            this.CustomButtonApplyTiming.Click += new System.EventHandler(this.CustomButtonApplyTiming_Click);
            // 
            // CustomCheckBoxFixNegative
            // 
            this.CustomCheckBoxFixNegative.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxFixNegative.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxFixNegative.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxFixNegative.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxFixNegative.Location = new System.Drawing.Point(5, 20);
            this.CustomCheckBoxFixNegative.Margin = new System.Windows.Forms.Padding(1);
            this.CustomCheckBoxFixNegative.Name = "CustomCheckBoxFixNegative";
            this.CustomCheckBoxFixNegative.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxFixNegative.Size = new System.Drawing.Size(124, 17);
            this.CustomCheckBoxFixNegative.TabIndex = 8;
            this.CustomCheckBoxFixNegative.Text = "Fix negative timing";
            this.CustomCheckBoxFixNegative.UseVisualStyleBackColor = false;
            this.CustomCheckBoxFixNegative.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomButtonResetTiming
            // 
            this.CustomButtonResetTiming.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonResetTiming.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonResetTiming.Location = new System.Drawing.Point(120, 255);
            this.CustomButtonResetTiming.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonResetTiming.Name = "CustomButtonResetTiming";
            this.CustomButtonResetTiming.RoundedCorners = 0;
            this.CustomButtonResetTiming.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonResetTiming.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonResetTiming.TabIndex = 7;
            this.CustomButtonResetTiming.Text = "Reset";
            this.CustomButtonResetTiming.UseVisualStyleBackColor = true;
            this.CustomButtonResetTiming.Click += new System.EventHandler(this.CustomButtonResetTiming_Click);
            // 
            // TabPageSync
            // 
            this.TabPageSync.BackColor = System.Drawing.Color.Transparent;
            this.TabPageSync.Controls.Add(this.CustomGroupBoxSync);
            this.TabPageSync.Location = new System.Drawing.Point(4, 42);
            this.TabPageSync.Name = "TabPageSync";
            this.TabPageSync.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageSync.Size = new System.Drawing.Size(222, 327);
            this.TabPageSync.TabIndex = 2;
            this.TabPageSync.Tag = 2;
            this.TabPageSync.Text = "Synchronization";
            // 
            // CustomGroupBoxSync
            // 
            this.CustomGroupBoxSync.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomGroupBoxSync.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxSync.Controls.Add(this.CustomGroupBoxChangeSpeed);
            this.CustomGroupBoxSync.Controls.Add(this.CustomGroupBoxChangeFrameRate);
            this.CustomGroupBoxSync.Controls.Add(this.CustomGroupBoxAdjustAllTimes);
            this.CustomGroupBoxSync.Location = new System.Drawing.Point(1, 1);
            this.CustomGroupBoxSync.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxSync.Name = "CustomGroupBoxSync";
            this.CustomGroupBoxSync.Size = new System.Drawing.Size(220, 325);
            this.CustomGroupBoxSync.TabIndex = 19;
            this.CustomGroupBoxSync.TabStop = false;
            this.CustomGroupBoxSync.Text = "Synchronization";
            // 
            // CustomGroupBoxChangeSpeed
            // 
            this.CustomGroupBoxChangeSpeed.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxChangeSpeed.Controls.Add(this.CustomButtonChangeSpeed);
            this.CustomGroupBoxChangeSpeed.Controls.Add(this.CustomNumericUpDownChangeSpeed);
            this.CustomGroupBoxChangeSpeed.Controls.Add(this.CustomRadioButtonChangeSpeedTDF);
            this.CustomGroupBoxChangeSpeed.Controls.Add(this.CustomRadioButtonChangeSpeedFDF);
            this.CustomGroupBoxChangeSpeed.Controls.Add(this.CustomRadioButtonChangeSpeedC);
            this.CustomGroupBoxChangeSpeed.Location = new System.Drawing.Point(3, 198);
            this.CustomGroupBoxChangeSpeed.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxChangeSpeed.Name = "CustomGroupBoxChangeSpeed";
            this.CustomGroupBoxChangeSpeed.Size = new System.Drawing.Size(214, 109);
            this.CustomGroupBoxChangeSpeed.TabIndex = 24;
            this.CustomGroupBoxChangeSpeed.TabStop = false;
            this.CustomGroupBoxChangeSpeed.Text = "Change speed (percent)";
            // 
            // CustomButtonChangeSpeed
            // 
            this.CustomButtonChangeSpeed.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonChangeSpeed.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonChangeSpeed.Location = new System.Drawing.Point(68, 81);
            this.CustomButtonChangeSpeed.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonChangeSpeed.Name = "CustomButtonChangeSpeed";
            this.CustomButtonChangeSpeed.RoundedCorners = 0;
            this.CustomButtonChangeSpeed.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonChangeSpeed.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonChangeSpeed.TabIndex = 4;
            this.CustomButtonChangeSpeed.Text = "Apply";
            this.CustomButtonChangeSpeed.UseVisualStyleBackColor = true;
            this.CustomButtonChangeSpeed.Click += new System.EventHandler(this.CustomButtonChangeSpeed_Click);
            // 
            // CustomNumericUpDownChangeSpeed
            // 
            this.CustomNumericUpDownChangeSpeed.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownChangeSpeed.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownChangeSpeed.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownChangeSpeed.DecimalPlaces = 5;
            this.CustomNumericUpDownChangeSpeed.Location = new System.Drawing.Point(135, 40);
            this.CustomNumericUpDownChangeSpeed.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownChangeSpeed.Maximum = new decimal(new int[] {
            200,
            0,
            0,
            0});
            this.CustomNumericUpDownChangeSpeed.Minimum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.CustomNumericUpDownChangeSpeed.Name = "CustomNumericUpDownChangeSpeed";
            this.CustomNumericUpDownChangeSpeed.Size = new System.Drawing.Size(75, 23);
            this.CustomNumericUpDownChangeSpeed.TabIndex = 3;
            this.CustomNumericUpDownChangeSpeed.Value = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.CustomNumericUpDownChangeSpeed.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomRadioButtonChangeSpeedTDF
            // 
            this.CustomRadioButtonChangeSpeedTDF.BackColor = System.Drawing.Color.DimGray;
            this.CustomRadioButtonChangeSpeedTDF.BorderColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedTDF.CheckColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedTDF.ForeColor = System.Drawing.Color.White;
            this.CustomRadioButtonChangeSpeedTDF.Location = new System.Drawing.Point(6, 60);
            this.CustomRadioButtonChangeSpeedTDF.Margin = new System.Windows.Forms.Padding(1);
            this.CustomRadioButtonChangeSpeedTDF.Name = "CustomRadioButtonChangeSpeedTDF";
            this.CustomRadioButtonChangeSpeedTDF.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomRadioButtonChangeSpeedTDF.Size = new System.Drawing.Size(99, 17);
            this.CustomRadioButtonChangeSpeedTDF.TabIndex = 2;
            this.CustomRadioButtonChangeSpeedTDF.Text = "To drop frame";
            this.CustomRadioButtonChangeSpeedTDF.UseVisualStyleBackColor = false;
            this.CustomRadioButtonChangeSpeedTDF.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomRadioButtonChangeSpeedFDF
            // 
            this.CustomRadioButtonChangeSpeedFDF.BackColor = System.Drawing.Color.DimGray;
            this.CustomRadioButtonChangeSpeedFDF.BorderColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedFDF.CheckColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedFDF.ForeColor = System.Drawing.Color.White;
            this.CustomRadioButtonChangeSpeedFDF.Location = new System.Drawing.Point(6, 40);
            this.CustomRadioButtonChangeSpeedFDF.Margin = new System.Windows.Forms.Padding(1);
            this.CustomRadioButtonChangeSpeedFDF.Name = "CustomRadioButtonChangeSpeedFDF";
            this.CustomRadioButtonChangeSpeedFDF.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomRadioButtonChangeSpeedFDF.Size = new System.Drawing.Size(113, 17);
            this.CustomRadioButtonChangeSpeedFDF.TabIndex = 1;
            this.CustomRadioButtonChangeSpeedFDF.Text = "From drop frame";
            this.CustomRadioButtonChangeSpeedFDF.UseVisualStyleBackColor = false;
            this.CustomRadioButtonChangeSpeedFDF.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomRadioButtonChangeSpeedC
            // 
            this.CustomRadioButtonChangeSpeedC.BackColor = System.Drawing.Color.DimGray;
            this.CustomRadioButtonChangeSpeedC.BorderColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedC.CheckColor = System.Drawing.Color.Blue;
            this.CustomRadioButtonChangeSpeedC.Checked = true;
            this.CustomRadioButtonChangeSpeedC.ForeColor = System.Drawing.Color.White;
            this.CustomRadioButtonChangeSpeedC.Location = new System.Drawing.Point(6, 20);
            this.CustomRadioButtonChangeSpeedC.Margin = new System.Windows.Forms.Padding(1);
            this.CustomRadioButtonChangeSpeedC.Name = "CustomRadioButtonChangeSpeedC";
            this.CustomRadioButtonChangeSpeedC.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomRadioButtonChangeSpeedC.Size = new System.Drawing.Size(62, 17);
            this.CustomRadioButtonChangeSpeedC.TabIndex = 0;
            this.CustomRadioButtonChangeSpeedC.TabStop = true;
            this.CustomRadioButtonChangeSpeedC.Text = "Custom";
            this.CustomRadioButtonChangeSpeedC.UseVisualStyleBackColor = false;
            this.CustomRadioButtonChangeSpeedC.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomGroupBoxChangeFrameRate
            // 
            this.CustomGroupBoxChangeFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxChangeFrameRate.Controls.Add(this.CustomLabelToFrameRate);
            this.CustomGroupBoxChangeFrameRate.Controls.Add(this.CustomLabelFromFrameRate);
            this.CustomGroupBoxChangeFrameRate.Controls.Add(this.CustomComboBoxFromFrameRate);
            this.CustomGroupBoxChangeFrameRate.Controls.Add(this.CustomComboBoxToFrameRate);
            this.CustomGroupBoxChangeFrameRate.Controls.Add(this.CustomButtonChangeFrameRate);
            this.CustomGroupBoxChangeFrameRate.Location = new System.Drawing.Point(3, 93);
            this.CustomGroupBoxChangeFrameRate.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxChangeFrameRate.Name = "CustomGroupBoxChangeFrameRate";
            this.CustomGroupBoxChangeFrameRate.Size = new System.Drawing.Size(214, 101);
            this.CustomGroupBoxChangeFrameRate.TabIndex = 23;
            this.CustomGroupBoxChangeFrameRate.TabStop = false;
            this.CustomGroupBoxChangeFrameRate.Text = "Change frame rate";
            // 
            // CustomLabelToFrameRate
            // 
            this.CustomLabelToFrameRate.AutoSize = true;
            this.CustomLabelToFrameRate.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelToFrameRate.Border = false;
            this.CustomLabelToFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelToFrameRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelToFrameRate.Location = new System.Drawing.Point(6, 48);
            this.CustomLabelToFrameRate.Name = "CustomLabelToFrameRate";
            this.CustomLabelToFrameRate.RoundedCorners = 0;
            this.CustomLabelToFrameRate.Size = new System.Drawing.Size(76, 15);
            this.CustomLabelToFrameRate.TabIndex = 11;
            this.CustomLabelToFrameRate.Text = "To frame rate";
            // 
            // CustomLabelFromFrameRate
            // 
            this.CustomLabelFromFrameRate.AutoSize = true;
            this.CustomLabelFromFrameRate.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelFromFrameRate.Border = false;
            this.CustomLabelFromFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelFromFrameRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelFromFrameRate.Location = new System.Drawing.Point(6, 23);
            this.CustomLabelFromFrameRate.Name = "CustomLabelFromFrameRate";
            this.CustomLabelFromFrameRate.RoundedCorners = 0;
            this.CustomLabelFromFrameRate.Size = new System.Drawing.Size(92, 15);
            this.CustomLabelFromFrameRate.TabIndex = 10;
            this.CustomLabelFromFrameRate.Text = "From frame rate";
            // 
            // CustomComboBoxFromFrameRate
            // 
            this.CustomComboBoxFromFrameRate.BackColor = System.Drawing.Color.DimGray;
            this.CustomComboBoxFromFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomComboBoxFromFrameRate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxFromFrameRate.ForeColor = System.Drawing.Color.White;
            this.CustomComboBoxFromFrameRate.FormattingEnabled = true;
            this.CustomComboBoxFromFrameRate.ItemHeight = 17;
            this.CustomComboBoxFromFrameRate.Location = new System.Drawing.Point(140, 20);
            this.CustomComboBoxFromFrameRate.Margin = new System.Windows.Forms.Padding(1);
            this.CustomComboBoxFromFrameRate.Name = "CustomComboBoxFromFrameRate";
            this.CustomComboBoxFromFrameRate.SelectionColor = System.Drawing.Color.DodgerBlue;
            this.CustomComboBoxFromFrameRate.Size = new System.Drawing.Size(70, 23);
            this.CustomComboBoxFromFrameRate.TabIndex = 6;
            this.CustomComboBoxFromFrameRate.SelectedIndexChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomComboBoxToFrameRate
            // 
            this.CustomComboBoxToFrameRate.BackColor = System.Drawing.Color.DimGray;
            this.CustomComboBoxToFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomComboBoxToFrameRate.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawVariable;
            this.CustomComboBoxToFrameRate.ForeColor = System.Drawing.Color.White;
            this.CustomComboBoxToFrameRate.FormattingEnabled = true;
            this.CustomComboBoxToFrameRate.ItemHeight = 17;
            this.CustomComboBoxToFrameRate.Location = new System.Drawing.Point(140, 45);
            this.CustomComboBoxToFrameRate.Margin = new System.Windows.Forms.Padding(1);
            this.CustomComboBoxToFrameRate.Name = "CustomComboBoxToFrameRate";
            this.CustomComboBoxToFrameRate.SelectionColor = System.Drawing.Color.DodgerBlue;
            this.CustomComboBoxToFrameRate.Size = new System.Drawing.Size(70, 23);
            this.CustomComboBoxToFrameRate.TabIndex = 8;
            this.CustomComboBoxToFrameRate.SelectedIndexChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomButtonChangeFrameRate
            // 
            this.CustomButtonChangeFrameRate.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonChangeFrameRate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonChangeFrameRate.Location = new System.Drawing.Point(68, 73);
            this.CustomButtonChangeFrameRate.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonChangeFrameRate.Name = "CustomButtonChangeFrameRate";
            this.CustomButtonChangeFrameRate.RoundedCorners = 0;
            this.CustomButtonChangeFrameRate.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonChangeFrameRate.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonChangeFrameRate.TabIndex = 9;
            this.CustomButtonChangeFrameRate.Text = "Apply";
            this.CustomButtonChangeFrameRate.UseVisualStyleBackColor = true;
            this.CustomButtonChangeFrameRate.Click += new System.EventHandler(this.CustomButtonChangeFrameRate_Click);
            // 
            // CustomGroupBoxAdjustAllTimes
            // 
            this.CustomGroupBoxAdjustAllTimes.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxAdjustAllTimes.Controls.Add(this.CustomLabelAdjustAllTimes);
            this.CustomGroupBoxAdjustAllTimes.Controls.Add(this.CustomTimeUpDownAdjustTime);
            this.CustomGroupBoxAdjustAllTimes.Controls.Add(this.CustomButtonSyncEarlier);
            this.CustomGroupBoxAdjustAllTimes.Controls.Add(this.CustomButtonSyncLater);
            this.CustomGroupBoxAdjustAllTimes.Location = new System.Drawing.Point(3, 20);
            this.CustomGroupBoxAdjustAllTimes.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxAdjustAllTimes.Name = "CustomGroupBoxAdjustAllTimes";
            this.CustomGroupBoxAdjustAllTimes.Size = new System.Drawing.Size(214, 71);
            this.CustomGroupBoxAdjustAllTimes.TabIndex = 11;
            this.CustomGroupBoxAdjustAllTimes.TabStop = false;
            this.CustomGroupBoxAdjustAllTimes.Text = "Adjust all times";
            // 
            // CustomLabelAdjustAllTimes
            // 
            this.CustomLabelAdjustAllTimes.AutoSize = true;
            this.CustomLabelAdjustAllTimes.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelAdjustAllTimes.Border = false;
            this.CustomLabelAdjustAllTimes.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelAdjustAllTimes.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelAdjustAllTimes.Location = new System.Drawing.Point(4, 16);
            this.CustomLabelAdjustAllTimes.Name = "CustomLabelAdjustAllTimes";
            this.CustomLabelAdjustAllTimes.RoundedCorners = 0;
            this.CustomLabelAdjustAllTimes.Size = new System.Drawing.Size(96, 15);
            this.CustomLabelAdjustAllTimes.TabIndex = 4;
            this.CustomLabelAdjustAllTimes.Text = "HH:MM:SS.Msec";
            // 
            // CustomTimeUpDownAdjustTime
            // 
            this.CustomTimeUpDownAdjustTime.BackColor = System.Drawing.Color.DimGray;
            this.CustomTimeUpDownAdjustTime.Border = true;
            this.CustomTimeUpDownAdjustTime.BorderColor = System.Drawing.Color.Blue;
            this.CustomTimeUpDownAdjustTime.ForeColor = System.Drawing.Color.White;
            this.CustomTimeUpDownAdjustTime.Location = new System.Drawing.Point(120, 13);
            this.CustomTimeUpDownAdjustTime.Margin = new System.Windows.Forms.Padding(1);
            this.CustomTimeUpDownAdjustTime.Name = "CustomTimeUpDownAdjustTime";
            this.CustomTimeUpDownAdjustTime.Size = new System.Drawing.Size(90, 23);
            this.CustomTimeUpDownAdjustTime.TabIndex = 0;
            this.CustomTimeUpDownAdjustTime.Value = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.CustomTimeUpDownAdjustTime.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomButtonSyncEarlier
            // 
            this.CustomButtonSyncEarlier.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonSyncEarlier.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonSyncEarlier.Location = new System.Drawing.Point(20, 43);
            this.CustomButtonSyncEarlier.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonSyncEarlier.Name = "CustomButtonSyncEarlier";
            this.CustomButtonSyncEarlier.RoundedCorners = 0;
            this.CustomButtonSyncEarlier.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonSyncEarlier.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonSyncEarlier.TabIndex = 2;
            this.CustomButtonSyncEarlier.Text = "Earlier";
            this.CustomButtonSyncEarlier.UseVisualStyleBackColor = true;
            this.CustomButtonSyncEarlier.Click += new System.EventHandler(this.CustomButtonSyncEarlier_Click);
            // 
            // CustomButtonSyncLater
            // 
            this.CustomButtonSyncLater.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonSyncLater.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonSyncLater.Location = new System.Drawing.Point(120, 43);
            this.CustomButtonSyncLater.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonSyncLater.Name = "CustomButtonSyncLater";
            this.CustomButtonSyncLater.RoundedCorners = 0;
            this.CustomButtonSyncLater.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonSyncLater.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonSyncLater.TabIndex = 3;
            this.CustomButtonSyncLater.Text = "Later";
            this.CustomButtonSyncLater.UseVisualStyleBackColor = true;
            this.CustomButtonSyncLater.Click += new System.EventHandler(this.CustomButtonSyncLater_Click);
            // 
            // TabPageOthers
            // 
            this.TabPageOthers.BackColor = System.Drawing.Color.Transparent;
            this.TabPageOthers.Controls.Add(this.CustomGroupBoxOthers);
            this.TabPageOthers.Location = new System.Drawing.Point(4, 42);
            this.TabPageOthers.Name = "TabPageOthers";
            this.TabPageOthers.Padding = new System.Windows.Forms.Padding(3);
            this.TabPageOthers.Size = new System.Drawing.Size(222, 327);
            this.TabPageOthers.TabIndex = 3;
            this.TabPageOthers.Tag = 3;
            this.TabPageOthers.Text = "Others";
            // 
            // CustomGroupBoxOthers
            // 
            this.CustomGroupBoxOthers.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.CustomGroupBoxOthers.BorderColor = System.Drawing.Color.Blue;
            this.CustomGroupBoxOthers.Controls.Add(this.CustomLabelSameTextDifference);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomCheckBoxRemoveUCChars);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomNumericUpDownSameText);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomCheckBoxMergeSameText);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomButtonResetOthers);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomButtonApplyOthers);
            this.CustomGroupBoxOthers.Controls.Add(this.CustomCheckBoxRemoveEmptyLines);
            this.CustomGroupBoxOthers.Location = new System.Drawing.Point(1, 1);
            this.CustomGroupBoxOthers.Margin = new System.Windows.Forms.Padding(1);
            this.CustomGroupBoxOthers.Name = "CustomGroupBoxOthers";
            this.CustomGroupBoxOthers.Size = new System.Drawing.Size(220, 325);
            this.CustomGroupBoxOthers.TabIndex = 0;
            this.CustomGroupBoxOthers.TabStop = false;
            this.CustomGroupBoxOthers.Text = "Others";
            // 
            // CustomLabelSameTextDifference
            // 
            this.CustomLabelSameTextDifference.AutoSize = true;
            this.CustomLabelSameTextDifference.BackColor = System.Drawing.Color.DimGray;
            this.CustomLabelSameTextDifference.Border = false;
            this.CustomLabelSameTextDifference.BorderColor = System.Drawing.Color.Blue;
            this.CustomLabelSameTextDifference.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomLabelSameTextDifference.Location = new System.Drawing.Point(20, 62);
            this.CustomLabelSameTextDifference.Name = "CustomLabelSameTextDifference";
            this.CustomLabelSameTextDifference.RoundedCorners = 0;
            this.CustomLabelSameTextDifference.Size = new System.Drawing.Size(121, 15);
            this.CustomLabelSameTextDifference.TabIndex = 6;
            this.CustomLabelSameTextDifference.Text = "Max. MSec difference";
            // 
            // CustomCheckBoxRemoveUCChars
            // 
            this.CustomCheckBoxRemoveUCChars.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxRemoveUCChars.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxRemoveUCChars.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxRemoveUCChars.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxRemoveUCChars.Location = new System.Drawing.Point(5, 85);
            this.CustomCheckBoxRemoveUCChars.Name = "CustomCheckBoxRemoveUCChars";
            this.CustomCheckBoxRemoveUCChars.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxRemoveUCChars.Size = new System.Drawing.Size(188, 17);
            this.CustomCheckBoxRemoveUCChars.TabIndex = 5;
            this.CustomCheckBoxRemoveUCChars.Text = "Remove unicode control Chars";
            this.CustomCheckBoxRemoveUCChars.UseVisualStyleBackColor = false;
            this.CustomCheckBoxRemoveUCChars.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomNumericUpDownSameText
            // 
            this.CustomNumericUpDownSameText.BackColor = System.Drawing.Color.DimGray;
            this.CustomNumericUpDownSameText.BorderColor = System.Drawing.Color.Blue;
            this.CustomNumericUpDownSameText.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.CustomNumericUpDownSameText.Enabled = false;
            this.CustomNumericUpDownSameText.Location = new System.Drawing.Point(160, 60);
            this.CustomNumericUpDownSameText.Margin = new System.Windows.Forms.Padding(1);
            this.CustomNumericUpDownSameText.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.CustomNumericUpDownSameText.Name = "CustomNumericUpDownSameText";
            this.CustomNumericUpDownSameText.Size = new System.Drawing.Size(55, 23);
            this.CustomNumericUpDownSameText.TabIndex = 4;
            this.CustomNumericUpDownSameText.ValueChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomCheckBoxMergeSameText
            // 
            this.CustomCheckBoxMergeSameText.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxMergeSameText.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxMergeSameText.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxMergeSameText.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxMergeSameText.Location = new System.Drawing.Point(5, 40);
            this.CustomCheckBoxMergeSameText.Margin = new System.Windows.Forms.Padding(1);
            this.CustomCheckBoxMergeSameText.Name = "CustomCheckBoxMergeSameText";
            this.CustomCheckBoxMergeSameText.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxMergeSameText.Size = new System.Drawing.Size(167, 17);
            this.CustomCheckBoxMergeSameText.TabIndex = 3;
            this.CustomCheckBoxMergeSameText.Text = "Merge lines with same text";
            this.CustomCheckBoxMergeSameText.UseVisualStyleBackColor = false;
            this.CustomCheckBoxMergeSameText.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // CustomButtonResetOthers
            // 
            this.CustomButtonResetOthers.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonResetOthers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonResetOthers.Location = new System.Drawing.Point(120, 140);
            this.CustomButtonResetOthers.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonResetOthers.Name = "CustomButtonResetOthers";
            this.CustomButtonResetOthers.RoundedCorners = 0;
            this.CustomButtonResetOthers.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonResetOthers.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonResetOthers.TabIndex = 2;
            this.CustomButtonResetOthers.Text = "Reset";
            this.CustomButtonResetOthers.UseVisualStyleBackColor = true;
            this.CustomButtonResetOthers.Click += new System.EventHandler(this.CustomButtonResetOthers_Click);
            // 
            // CustomButtonApplyOthers
            // 
            this.CustomButtonApplyOthers.BorderColor = System.Drawing.Color.Blue;
            this.CustomButtonApplyOthers.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomButtonApplyOthers.Location = new System.Drawing.Point(26, 140);
            this.CustomButtonApplyOthers.Margin = new System.Windows.Forms.Padding(1);
            this.CustomButtonApplyOthers.Name = "CustomButtonApplyOthers";
            this.CustomButtonApplyOthers.RoundedCorners = 0;
            this.CustomButtonApplyOthers.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomButtonApplyOthers.Size = new System.Drawing.Size(75, 23);
            this.CustomButtonApplyOthers.TabIndex = 1;
            this.CustomButtonApplyOthers.Text = "Apply";
            this.CustomButtonApplyOthers.UseVisualStyleBackColor = true;
            this.CustomButtonApplyOthers.Click += new System.EventHandler(this.CustomButtonApplyOthers_Click);
            // 
            // CustomCheckBoxRemoveEmptyLines
            // 
            this.CustomCheckBoxRemoveEmptyLines.BackColor = System.Drawing.Color.DimGray;
            this.CustomCheckBoxRemoveEmptyLines.BorderColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxRemoveEmptyLines.CheckColor = System.Drawing.Color.Blue;
            this.CustomCheckBoxRemoveEmptyLines.Checked = true;
            this.CustomCheckBoxRemoveEmptyLines.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CustomCheckBoxRemoveEmptyLines.ForeColor = System.Drawing.Color.White;
            this.CustomCheckBoxRemoveEmptyLines.Location = new System.Drawing.Point(5, 15);
            this.CustomCheckBoxRemoveEmptyLines.Margin = new System.Windows.Forms.Padding(1);
            this.CustomCheckBoxRemoveEmptyLines.Name = "CustomCheckBoxRemoveEmptyLines";
            this.CustomCheckBoxRemoveEmptyLines.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomCheckBoxRemoveEmptyLines.Size = new System.Drawing.Size(131, 17);
            this.CustomCheckBoxRemoveEmptyLines.TabIndex = 0;
            this.CustomCheckBoxRemoveEmptyLines.Text = "Remove empty lines";
            this.CustomCheckBoxRemoveEmptyLines.UseVisualStyleBackColor = false;
            this.CustomCheckBoxRemoveEmptyLines.CheckedChanged += new System.EventHandler(this.Fixes_ValueChanged);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            this.imageList1.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imageList1.ImageStream")));
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            this.imageList1.Images.SetKeyName(0, "apple.ico");
            this.imageList1.Images.SetKeyName(1, "bread1.ico");
            this.imageList1.Images.SetKeyName(2, "coffeecup_red.ico");
            this.imageList1.Images.SetKeyName(3, "coffeepot.ico");
            // 
            // BottomToolStripPanel
            // 
            this.BottomToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.BottomToolStripPanel.Name = "BottomToolStripPanel";
            this.BottomToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.BottomToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.BottomToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // TopToolStripPanel
            // 
            this.TopToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.TopToolStripPanel.Name = "TopToolStripPanel";
            this.TopToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.TopToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.TopToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // RightToolStripPanel
            // 
            this.RightToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.RightToolStripPanel.Name = "RightToolStripPanel";
            this.RightToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.RightToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.RightToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // LeftToolStripPanel
            // 
            this.LeftToolStripPanel.Location = new System.Drawing.Point(0, 0);
            this.LeftToolStripPanel.Name = "LeftToolStripPanel";
            this.LeftToolStripPanel.Orientation = System.Windows.Forms.Orientation.Horizontal;
            this.LeftToolStripPanel.RowMargin = new System.Windows.Forms.Padding(3, 0, 0, 0);
            this.LeftToolStripPanel.Size = new System.Drawing.Size(0, 0);
            // 
            // ContentPanel
            // 
            this.ContentPanel.Size = new System.Drawing.Size(150, 150);
            // 
            // CustomMenuStrip1
            // 
            this.CustomMenuStrip1.BackColor = System.Drawing.Color.DimGray;
            this.CustomMenuStrip1.Border = false;
            this.CustomMenuStrip1.BorderColor = System.Drawing.Color.Blue;
            this.CustomMenuStrip1.ForeColor = System.Drawing.Color.White;
            this.CustomMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FileToolStripMenuItem,
            this.EditToolStripMenuItem,
            this.ViewToolStripMenuItem,
            this.ToolsToolStripMenuItem,
            this.AboutToolStripMenuItem});
            this.CustomMenuStrip1.Location = new System.Drawing.Point(0, 0);
            this.CustomMenuStrip1.Name = "CustomMenuStrip1";
            this.CustomMenuStrip1.SameColorForSubItems = true;
            this.CustomMenuStrip1.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomMenuStrip1.Size = new System.Drawing.Size(884, 24);
            this.CustomMenuStrip1.TabIndex = 23;
            this.CustomMenuStrip1.Text = "MenuStrip1";
            // 
            // FileToolStripMenuItem
            // 
            this.FileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.OpenToolStripMenuItem,
            this.SaveToolStripMenuItem,
            this.SaveAsToolStripMenuItem,
            this.toolStripSeparator6,
            this.ExitToolStripMenuItem});
            this.FileToolStripMenuItem.Name = "FileToolStripMenuItem";
            this.FileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.FileToolStripMenuItem.Text = "File";
            // 
            // OpenToolStripMenuItem
            // 
            this.OpenToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.OpenToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.OpenToolStripMenuItem.Name = "OpenToolStripMenuItem";
            this.OpenToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
            this.OpenToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.OpenToolStripMenuItem.Text = "Open";
            this.OpenToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonOpen_Click);
            // 
            // SaveToolStripMenuItem
            // 
            this.SaveToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.SaveToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.SaveToolStripMenuItem.Name = "SaveToolStripMenuItem";
            this.SaveToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
            this.SaveToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.SaveToolStripMenuItem.Text = "Save";
            this.SaveToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonSave_Click);
            // 
            // SaveAsToolStripMenuItem
            // 
            this.SaveAsToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.SaveAsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.SaveAsToolStripMenuItem.Name = "SaveAsToolStripMenuItem";
            this.SaveAsToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)(((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Alt) 
            | System.Windows.Forms.Keys.S)));
            this.SaveAsToolStripMenuItem.Size = new System.Drawing.Size(184, 22);
            this.SaveAsToolStripMenuItem.Text = "Save as...";
            this.SaveAsToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonSaveAs_Click);
            // 
            // toolStripSeparator6
            // 
            this.toolStripSeparator6.BackColor = System.Drawing.Color.DimGray;
            this.toolStripSeparator6.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator6.Name = "toolStripSeparator6";
            this.toolStripSeparator6.Size = new System.Drawing.Size(181, 6);
            // 
            // ExitToolStripMenuItem
            // 
            this.ExitToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.ExitToolStripMenuItem.ForeColor = System.Drawing.Color.White;
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
            this.UndoToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.UndoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.UndoToolStripMenuItem.Name = "UndoToolStripMenuItem";
            this.UndoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Z)));
            this.UndoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.UndoToolStripMenuItem.Text = "Undo";
            this.UndoToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonUndo_Click);
            // 
            // RedoToolStripMenuItem
            // 
            this.RedoToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.RedoToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.RedoToolStripMenuItem.Name = "RedoToolStripMenuItem";
            this.RedoToolStripMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.Y)));
            this.RedoToolStripMenuItem.Size = new System.Drawing.Size(144, 22);
            this.RedoToolStripMenuItem.Text = "Redo";
            this.RedoToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonRedo_Click);
            // 
            // ViewToolStripMenuItem
            // 
            this.ViewToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ShowPopupGuideToolStripMenuItem});
            this.ViewToolStripMenuItem.Name = "ViewToolStripMenuItem";
            this.ViewToolStripMenuItem.Size = new System.Drawing.Size(44, 20);
            this.ViewToolStripMenuItem.Text = "View";
            // 
            // ShowPopupGuideToolStripMenuItem
            // 
            this.ShowPopupGuideToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.ShowPopupGuideToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.ShowPopupGuideToolStripMenuItem.Name = "ShowPopupGuideToolStripMenuItem";
            this.ShowPopupGuideToolStripMenuItem.Size = new System.Drawing.Size(174, 22);
            this.ShowPopupGuideToolStripMenuItem.Text = "Show popup guide";
            this.ShowPopupGuideToolStripMenuItem.Click += new System.EventHandler(this.ShowPopupGuideToolStripMenuItem_Click);
            // 
            // ToolsToolStripMenuItem
            // 
            this.ToolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.FixNegativeTimingToolStripMenuItem,
            this.FixIncorrectTimeOrderToolStripMenuItem,
            this.RemoveEmptyLinesToolStripMenuItem,
            this.RemoveUnicodeControlCharsToolStripMenuItem});
            this.ToolsToolStripMenuItem.Name = "ToolsToolStripMenuItem";
            this.ToolsToolStripMenuItem.Size = new System.Drawing.Size(46, 20);
            this.ToolsToolStripMenuItem.Text = "Tools";
            // 
            // FixNegativeTimingToolStripMenuItem
            // 
            this.FixNegativeTimingToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.FixNegativeTimingToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.FixNegativeTimingToolStripMenuItem.Name = "FixNegativeTimingToolStripMenuItem";
            this.FixNegativeTimingToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.FixNegativeTimingToolStripMenuItem.Text = "Fix negative timing";
            this.FixNegativeTimingToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolStripMenuItem_Click);
            // 
            // FixIncorrectTimeOrderToolStripMenuItem
            // 
            this.FixIncorrectTimeOrderToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.FixIncorrectTimeOrderToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.FixIncorrectTimeOrderToolStripMenuItem.Name = "FixIncorrectTimeOrderToolStripMenuItem";
            this.FixIncorrectTimeOrderToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.FixIncorrectTimeOrderToolStripMenuItem.Text = "Fix incorrect time order";
            this.FixIncorrectTimeOrderToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolStripMenuItem_Click);
            // 
            // RemoveEmptyLinesToolStripMenuItem
            // 
            this.RemoveEmptyLinesToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.RemoveEmptyLinesToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.RemoveEmptyLinesToolStripMenuItem.Name = "RemoveEmptyLinesToolStripMenuItem";
            this.RemoveEmptyLinesToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.RemoveEmptyLinesToolStripMenuItem.Text = "Remove empty lines";
            this.RemoveEmptyLinesToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolStripMenuItem_Click);
            // 
            // RemoveUnicodeControlCharsToolStripMenuItem
            // 
            this.RemoveUnicodeControlCharsToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.RemoveUnicodeControlCharsToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.RemoveUnicodeControlCharsToolStripMenuItem.Name = "RemoveUnicodeControlCharsToolStripMenuItem";
            this.RemoveUnicodeControlCharsToolStripMenuItem.Size = new System.Drawing.Size(235, 22);
            this.RemoveUnicodeControlCharsToolStripMenuItem.Text = "Remove unicode control chars";
            this.RemoveUnicodeControlCharsToolStripMenuItem.Click += new System.EventHandler(this.ToolsToolStripMenuItem_Click);
            // 
            // AboutToolStripMenuItem
            // 
            this.AboutToolStripMenuItem.Name = "AboutToolStripMenuItem";
            this.AboutToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.AboutToolStripMenuItem.Text = "About";
            this.AboutToolStripMenuItem.Click += new System.EventHandler(this.ToolStripButtonAbout_Click);
            // 
            // CustomToolStrip1
            // 
            this.CustomToolStrip1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.CustomToolStrip1.AutoSize = false;
            this.CustomToolStrip1.BackColor = System.Drawing.Color.DimGray;
            this.CustomToolStrip1.Border = false;
            this.CustomToolStrip1.BorderColor = System.Drawing.Color.Blue;
            this.CustomToolStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.CustomToolStrip1.ForeColor = System.Drawing.Color.White;
            this.CustomToolStrip1.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden;
            this.CustomToolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripButtonOpen,
            this.ToolStripButtonSave,
            this.ToolStripButtonSaveAs,
            this.ToolStripSeparator5,
            this.ToolStripButtonUndo,
            this.ToolStripButtonRedo,
            this.toolStripSeparator7,
            this.ToolStripLabelFormat,
            this.CustomToolStripComboBoxFormat,
            this.ToolStripSeparator8,
            this.ToolStripLabelEncoding,
            this.CustomToolStripComboBoxEncoding,
            this.ToolStripSeparator9,
            this.ToolStripButtonEdit,
            this.ToolStripButtonSettings,
            this.ToolStripButtonAbout,
            this.ToolStripButtonExit});
            this.CustomToolStrip1.Location = new System.Drawing.Point(0, 24);
            this.CustomToolStrip1.Name = "CustomToolStrip1";
            this.CustomToolStrip1.SelectionColor = System.Drawing.Color.LightCoral;
            this.CustomToolStrip1.Size = new System.Drawing.Size(884, 40);
            this.CustomToolStrip1.TabIndex = 24;
            this.CustomToolStrip1.Text = "CustomToolStrip1";
            // 
            // ToolStripButtonOpen
            // 
            this.ToolStripButtonOpen.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonOpen.Image = global::PersianSubtitleFixes.Properties.Resources.Open_Black;
            this.ToolStripButtonOpen.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonOpen.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonOpen.Name = "ToolStripButtonOpen";
            this.ToolStripButtonOpen.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonOpen.Text = "Open";
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
            this.ToolStripButtonSave.Text = "Save";
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
            this.ToolStripButtonSaveAs.Text = "Save as";
            this.ToolStripButtonSaveAs.Click += new System.EventHandler(this.ToolStripButtonSaveAs_Click);
            // 
            // ToolStripSeparator5
            // 
            this.ToolStripSeparator5.Name = "ToolStripSeparator5";
            this.ToolStripSeparator5.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripButtonUndo
            // 
            this.ToolStripButtonUndo.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonUndo.Image = global::PersianSubtitleFixes.Properties.Resources.Undo_Black;
            this.ToolStripButtonUndo.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonUndo.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonUndo.Name = "ToolStripButtonUndo";
            this.ToolStripButtonUndo.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonUndo.Text = "Undo";
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
            this.ToolStripButtonRedo.Text = "Redo";
            this.ToolStripButtonRedo.Click += new System.EventHandler(this.ToolStripButtonRedo_Click);
            // 
            // toolStripSeparator7
            // 
            this.toolStripSeparator7.Name = "toolStripSeparator7";
            this.toolStripSeparator7.Size = new System.Drawing.Size(6, 40);
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
            this.CustomToolStripComboBoxFormat.BackColor = System.Drawing.Color.DimGray;
            this.CustomToolStripComboBoxFormat.BorderColor = System.Drawing.Color.Blue;
            this.CustomToolStripComboBoxFormat.DropDownHeight = 106;
            this.CustomToolStripComboBoxFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CustomToolStripComboBoxFormat.DropDownWidth = 130;
            this.CustomToolStripComboBoxFormat.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomToolStripComboBoxFormat.ForeColor = System.Drawing.Color.White;
            this.CustomToolStripComboBoxFormat.IntegralHeight = false;
            this.CustomToolStripComboBoxFormat.MaxDropDownItems = 8;
            this.CustomToolStripComboBoxFormat.Name = "CustomToolStripComboBoxFormat";
            this.CustomToolStripComboBoxFormat.SelectedIndex = -1;
            this.CustomToolStripComboBoxFormat.SelectedItem = null;
            this.CustomToolStripComboBoxFormat.SelectedText = "";
            this.CustomToolStripComboBoxFormat.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomToolStripComboBoxFormat.SelectionLength = 0;
            this.CustomToolStripComboBoxFormat.SelectionStart = 0;
            this.CustomToolStripComboBoxFormat.Size = new System.Drawing.Size(121, 37);
            this.CustomToolStripComboBoxFormat.Sorted = false;
            this.CustomToolStripComboBoxFormat.SelectionIndexChanged += new System.EventHandler(this.CustomToolStripComboBoxFormat_SelectedIndexChanged);
            // 
            // ToolStripSeparator8
            // 
            this.ToolStripSeparator8.Name = "ToolStripSeparator8";
            this.ToolStripSeparator8.Size = new System.Drawing.Size(6, 40);
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
            this.CustomToolStripComboBoxEncoding.BackColor = System.Drawing.Color.DimGray;
            this.CustomToolStripComboBoxEncoding.BorderColor = System.Drawing.Color.Blue;
            this.CustomToolStripComboBoxEncoding.DropDownHeight = 106;
            this.CustomToolStripComboBoxEncoding.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CustomToolStripComboBoxEncoding.DropDownWidth = 130;
            this.CustomToolStripComboBoxEncoding.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.CustomToolStripComboBoxEncoding.ForeColor = System.Drawing.Color.White;
            this.CustomToolStripComboBoxEncoding.IntegralHeight = false;
            this.CustomToolStripComboBoxEncoding.MaxDropDownItems = 8;
            this.CustomToolStripComboBoxEncoding.Name = "CustomToolStripComboBoxEncoding";
            this.CustomToolStripComboBoxEncoding.SelectedIndex = -1;
            this.CustomToolStripComboBoxEncoding.SelectedItem = null;
            this.CustomToolStripComboBoxEncoding.SelectedText = "";
            this.CustomToolStripComboBoxEncoding.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomToolStripComboBoxEncoding.SelectionLength = 0;
            this.CustomToolStripComboBoxEncoding.SelectionStart = 0;
            this.CustomToolStripComboBoxEncoding.Size = new System.Drawing.Size(121, 37);
            this.CustomToolStripComboBoxEncoding.Sorted = false;
            this.CustomToolStripComboBoxEncoding.SelectionIndexChanged += new System.EventHandler(this.CustomToolStripComboBoxEncoding_SelectedIndexChanged);
            // 
            // ToolStripSeparator9
            // 
            this.ToolStripSeparator9.Name = "ToolStripSeparator9";
            this.ToolStripSeparator9.Size = new System.Drawing.Size(6, 40);
            // 
            // ToolStripButtonEdit
            // 
            this.ToolStripButtonEdit.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButtonEdit.Enabled = false;
            this.ToolStripButtonEdit.Image = global::PersianSubtitleFixes.Properties.Resources.Edit_Black;
            this.ToolStripButtonEdit.ImageScaling = System.Windows.Forms.ToolStripItemImageScaling.None;
            this.ToolStripButtonEdit.ImageTransparentColor = System.Drawing.Color.Transparent;
            this.ToolStripButtonEdit.Name = "ToolStripButtonEdit";
            this.ToolStripButtonEdit.Size = new System.Drawing.Size(36, 37);
            this.ToolStripButtonEdit.Text = "Edit";
            this.ToolStripButtonEdit.Click += new System.EventHandler(this.ToolStripButtonEdit_Click);
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
            // CustomStatusStrip1
            // 
            this.CustomStatusStrip1.BackColor = System.Drawing.Color.DimGray;
            this.CustomStatusStrip1.Border = false;
            this.CustomStatusStrip1.BorderColor = System.Drawing.Color.Blue;
            this.CustomStatusStrip1.ForeColor = System.Drawing.Color.White;
            this.CustomStatusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.ToolStripLabelLeft,
            this.ToolStripLabelSpace1,
            this.ToolStripLabelRight});
            this.CustomStatusStrip1.Location = new System.Drawing.Point(0, 439);
            this.CustomStatusStrip1.Name = "CustomStatusStrip1";
            this.CustomStatusStrip1.SelectionColor = System.Drawing.Color.LightBlue;
            this.CustomStatusStrip1.Size = new System.Drawing.Size(884, 22);
            this.CustomStatusStrip1.TabIndex = 25;
            this.CustomStatusStrip1.Text = "customStatusStrip1";
            // 
            // ToolStripLabelLeft
            // 
            this.ToolStripLabelLeft.AutoSize = false;
            this.ToolStripLabelLeft.Name = "ToolStripLabelLeft";
            this.ToolStripLabelLeft.Size = new System.Drawing.Size(749, 17);
            this.ToolStripLabelLeft.Spring = true;
            this.ToolStripLabelLeft.Text = "Label Left";
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
            this.ToolStripLabelRight.Text = "Label Right";
            this.ToolStripLabelRight.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // customContextMenuStrip1
            // 
            this.customContextMenuStrip1.BackColor = System.Drawing.Color.DimGray;
            this.customContextMenuStrip1.BorderColor = System.Drawing.Color.Blue;
            this.customContextMenuStrip1.ForeColor = System.Drawing.Color.White;
            this.customContextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.test1ToolStripMenuItem,
            this.test2ToolStripMenuItem});
            this.customContextMenuStrip1.Name = "customContextMenuStrip1";
            this.customContextMenuStrip1.SameColorForSubItems = true;
            this.customContextMenuStrip1.SelectionColor = System.Drawing.Color.LightBlue;
            this.customContextMenuStrip1.Size = new System.Drawing.Size(103, 48);
            // 
            // test1ToolStripMenuItem
            // 
            this.test1ToolStripMenuItem.Name = "test1ToolStripMenuItem";
            this.test1ToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.test1ToolStripMenuItem.Text = "test 1";
            // 
            // test2ToolStripMenuItem
            // 
            this.test2ToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.item1ToolStripMenuItem,
            this.item2ToolStripMenuItem,
            this.item3ToolStripMenuItem,
            this.toolStripSeparator1});
            this.test2ToolStripMenuItem.Name = "test2ToolStripMenuItem";
            this.test2ToolStripMenuItem.Size = new System.Drawing.Size(102, 22);
            this.test2ToolStripMenuItem.Text = "test 2";
            // 
            // item1ToolStripMenuItem
            // 
            this.item1ToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.item1ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.item1ToolStripMenuItem.Name = "item1ToolStripMenuItem";
            this.item1ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.item1ToolStripMenuItem.Text = "item 1";
            // 
            // item2ToolStripMenuItem
            // 
            this.item2ToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.item2ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.item2ToolStripMenuItem.Name = "item2ToolStripMenuItem";
            this.item2ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.item2ToolStripMenuItem.Text = "item 2";
            // 
            // item3ToolStripMenuItem
            // 
            this.item3ToolStripMenuItem.BackColor = System.Drawing.Color.DimGray;
            this.item3ToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.item3ToolStripMenuItem.Name = "item3ToolStripMenuItem";
            this.item3ToolStripMenuItem.Size = new System.Drawing.Size(107, 22);
            this.item3ToolStripMenuItem.Text = "item 3";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.BackColor = System.Drawing.Color.DimGray;
            this.toolStripSeparator1.ForeColor = System.Drawing.Color.Blue;
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(104, 6);
            // 
            // FormMain
            // 
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Cyan;
            this.ClientSize = new System.Drawing.Size(884, 461);
            this.Controls.Add(this.CustomStatusStrip1);
            this.Controls.Add(this.CustomToolStrip1);
            this.Controls.Add(this.CustomTabControl1);
            this.Controls.Add(this.CustomDataGridView1);
            this.Controls.Add(this.CustomProgressBar1);
            this.Controls.Add(this.CustomButtonCheckAll);
            this.Controls.Add(this.CustomButtonInvertCheck);
            this.Controls.Add(this.CustomMenuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FormMain";
            this.Text = "FormMain";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form_Closing);
            this.SizeChanged += new System.EventHandler(this.FormMain_SizeChanged);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.FormMain_DragDrop);
            this.DragEnter += new System.Windows.Forms.DragEventHandler(this.FormMain_DragEnter);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.CustomDataGridView1)).EndInit();
            this.CustomTabControl1.ResumeLayout(false);
            this.TabPageCommonErrors.ResumeLayout(false);
            this.TabPageTiming.ResumeLayout(false);
            this.CustomGroupBoxFixTiming.ResumeLayout(false);
            this.CustomGroupBoxFixTiming.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownSameTimeCode)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMinDL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMaxDL)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownCharsPerSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownMinGap)).EndInit();
            this.TabPageSync.ResumeLayout(false);
            this.CustomGroupBoxSync.ResumeLayout(false);
            this.CustomGroupBoxChangeSpeed.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownChangeSpeed)).EndInit();
            this.CustomGroupBoxChangeFrameRate.ResumeLayout(false);
            this.CustomGroupBoxChangeFrameRate.PerformLayout();
            this.CustomGroupBoxAdjustAllTimes.ResumeLayout(false);
            this.CustomGroupBoxAdjustAllTimes.PerformLayout();
            this.TabPageOthers.ResumeLayout(false);
            this.CustomGroupBoxOthers.ResumeLayout(false);
            this.CustomGroupBoxOthers.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.CustomNumericUpDownSameText)).EndInit();
            this.CustomMenuStrip1.ResumeLayout(false);
            this.CustomMenuStrip1.PerformLayout();
            this.CustomToolStrip1.ResumeLayout(false);
            this.CustomToolStrip1.PerformLayout();
            this.CustomStatusStrip1.ResumeLayout(false);
            this.CustomStatusStrip1.PerformLayout();
            this.customContextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.ComponentModel.BackgroundWorker BackgroundWorker1;
        private CustomControls.CustomButton CustomButtonStop;
        private CustomControls.CustomButton CustomButtonApply;
        private CustomControls.CustomButton CustomButtonInvertCheck;
        private CustomControls.CustomButton CustomButtonCheckAll;
        private CustomControls.CustomProgressBar CustomProgressBar1;
        private CustomControls.CustomDataGridView CustomDataGridView1;
        private DataGridViewTextBoxColumn ColumnLine;
        private DataGridViewTextBoxColumn ColumnStartTime;
        private DataGridViewTextBoxColumn ColumnEndTime;
        private DataGridViewTextBoxColumn ColumnDuration;
        private DataGridViewTextBoxColumn ColumnText;
        private CustomControls.CustomTabControl CustomTabControl1;
        private TabPage TabPageCommonErrors;
        private TabPage TabPageTiming;
        private CustomControls.CustomButton CustomButtonResetTiming;
        private CustomControls.CustomButton CustomButtonApplyTiming;
        private CustomControls.CustomGroupBox CustomGroupBoxFixTiming;
        private CustomControls.CustomGroupBox CustomGroupBoxSync;
        private CustomControls.CustomButton CustomButtonSyncLater;
        private CustomControls.CustomButton CustomButtonSyncEarlier;
        private TabPage TabPageSync;
        private TabPage TabPageOthers;
        private CustomControls.CustomGroupBox CustomGroupBoxOthers;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxAdjustDurations;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownMinDL;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownMaxDL;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownMinGap;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxFixNegative;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownCharsPerSec;
        protected internal CustomControls.CustomRadioButton CustomRadioButtonCharsPerSec;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxMergeSameTime;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownSameTimeCode;
        protected internal CustomControls.CustomTimeUpDown CustomTimeUpDownAdjustTime;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxRemoveEmptyLines;
        private CustomControls.CustomButton CustomButtonResetOthers;
        private CustomControls.CustomButton CustomButtonApplyOthers;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownSameText;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxMergeSameText;
        protected internal CustomControls.CustomComboBox CustomComboBoxFromFrameRate;
        protected internal CustomControls.CustomComboBox CustomComboBoxToFrameRate;
        private CustomControls.CustomButton CustomButtonChangeFrameRate;
        private CustomControls.CustomGroupBox CustomGroupBoxChangeSpeed;
        private CustomControls.CustomGroupBox CustomGroupBoxChangeFrameRate;
        private CustomControls.CustomGroupBox CustomGroupBoxAdjustAllTimes;
        private CustomControls.CustomButton CustomButtonChangeSpeed;
        protected internal CustomControls.CustomRadioButton CustomRadioButtonChangeSpeedTDF;
        protected internal CustomControls.CustomRadioButton CustomRadioButtonChangeSpeedFDF;
        protected internal CustomControls.CustomRadioButton CustomRadioButtonChangeSpeedC;
        protected internal CustomControls.CustomNumericUpDown CustomNumericUpDownChangeSpeed;
        private ImageList imageList1;
        protected internal CustomControls.CustomLabel CustomLabelMinDurationlimit;
        protected internal CustomControls.CustomLabel CustomLabelMaxDurationLimit;
        protected internal CustomControls.CustomLabel CustomLabelMinGap;
        protected internal CustomControls.CustomLabel CustomLabelAdjustAllTimes;
        private CustomControls.CustomLabel CustomLabelToFrameRate;
        protected internal CustomControls.CustomLabel CustomLabelFromFrameRate;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxFixIncorrectTimeOrder;
        private CustomControls.CustomButton CustomButtonCancel;
        protected internal CustomControls.CustomCheckBox CustomCheckBoxRemoveUCChars;
        private ToolStripPanel BottomToolStripPanel;
        private ToolStripPanel TopToolStripPanel;
        private ToolStripPanel RightToolStripPanel;
        private ToolStripPanel LeftToolStripPanel;
        private ToolStripContentPanel ContentPanel;
        private CustomControls.CustomMenuStrip CustomMenuStrip1;
        private ToolStripMenuItem FileToolStripMenuItem;
        private ToolStripMenuItem OpenToolStripMenuItem;
        private ToolStripMenuItem SaveToolStripMenuItem;
        private ToolStripMenuItem SaveAsToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator6;
        private ToolStripMenuItem ExitToolStripMenuItem;
        private ToolStripMenuItem EditToolStripMenuItem;
        private ToolStripMenuItem UndoToolStripMenuItem;
        private ToolStripMenuItem RedoToolStripMenuItem;
        private ToolStripMenuItem ToolsToolStripMenuItem;
        private ToolStripMenuItem AboutToolStripMenuItem;
        private CustomControls.CustomToolStrip CustomToolStrip1;
        private ToolStripButton ToolStripButtonOpen;
        private ToolStripButton ToolStripButtonSave;
        private ToolStripButton ToolStripButtonSaveAs;
        private ToolStripSeparator ToolStripSeparator5;
        private ToolStripButton ToolStripButtonUndo;
        private ToolStripButton ToolStripButtonRedo;
        private ToolStripSeparator toolStripSeparator7;
        private ToolStripLabel ToolStripLabelFormat;
        private CustomControls.CustomToolStripComboBox CustomToolStripComboBoxFormat;
        private ToolStripSeparator ToolStripSeparator8;
        private ToolStripLabel ToolStripLabelEncoding;
        private CustomControls.CustomToolStripComboBox CustomToolStripComboBoxEncoding;
        private ToolStripSeparator ToolStripSeparator9;
        private ToolStripButton ToolStripButtonEdit;
        private ToolStripButton ToolStripButtonSettings;
        private ToolStripButton ToolStripButtonAbout;
        private ToolStripButton ToolStripButtonExit;
        private CustomControls.CustomStatusStrip CustomStatusStrip1;
        private ToolStripStatusLabel ToolStripLabelLeft;
        private ToolStripStatusLabel ToolStripLabelSpace1;
        private ToolStripStatusLabel ToolStripLabelRight;
        private CustomControls.CustomContextMenuStrip customContextMenuStrip1;
        private ToolStripMenuItem test1ToolStripMenuItem;
        private ToolStripMenuItem test2ToolStripMenuItem;
        private ToolStripMenuItem item1ToolStripMenuItem;
        private ToolStripMenuItem item2ToolStripMenuItem;
        private ToolStripMenuItem item3ToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private CustomControls.CustomLabel CustomLabelSameTimeDifference;
        private CustomControls.CustomLabel CustomLabelSameTextDifference;
        private ToolStripMenuItem FixNegativeTimingToolStripMenuItem;
        private ToolStripMenuItem FixIncorrectTimeOrderToolStripMenuItem;
        private ToolStripMenuItem RemoveEmptyLinesToolStripMenuItem;
        private ToolStripMenuItem RemoveUnicodeControlCharsToolStripMenuItem;
        private ToolStripMenuItem ViewToolStripMenuItem;
        internal ToolStripMenuItem ShowPopupGuideToolStripMenuItem;
    }
}