using PersianSubtitleFixes.msmh;
using MsmhTools;
using CustomControls;
using System.Text;
using System.IO;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Xml;
using System.Data;

namespace PersianSubtitleFixes
{
    public partial class FormMain : Form
    {
        // Enable Double-Buffering For The Entire Form (When There Are Many Controls) (Makes DataGridView Slow).
        //protected override CreateParams CreateParams
        //{
        //    get
        //    {
        //        CreateParams cp = base.CreateParams;
        //        cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
        //        return cp;
        //    }
        //}

        public bool CloseForm = false;
        public static Subtitle? SubCurrent = new();
        private Subtitle? SubOriginal;
        private Subtitle? SubTemp;
        private List<Paragraph> SubParagraphs = new();
        private List<string> SubLines = new();
        private MemoryStream SubMemoryStream = new();
        public static string? SubName { get; set; }
        public static string? SubPath { get; set; }
        public static string? SubOriginalPath { get; set; }
        public static string? SubTempPath { get; set; }
        public static SubtitleFormat? SubFormat;
        public static SubtitleFormat? SubOriginalFormat { get; set; }
        public static string? SubEncodingDisplayName { get; set; }
        public static string? SubOriginalEncodingDisplayName { get; set; }
        public static Encoding? SubEncoding { get; set; }
        public static Encoding? SubOriginalEncoding { get; set; }
        public static bool SubContainsPersianChars { get; set; }
        public static int TotalRows { get; set; }
        public static int TotalFixes { get; set; }
        // Sort Column Line#
        private readonly ListViewColumnSorter? lvwColumnSorter = null;
        private static CancellationTokenSource? SourceApply = null;
        private static Task? TaskApply { get; set; }
        private readonly System.Windows.Forms.Timer timerClearStatus = new();

        private readonly Dictionary<string, Regex> CompiledRegExList = new();
        private readonly static Label WaitLabel = new();
        private static bool UndoRedoClicked = false;
        private static bool UnSavedWork = false;

        public static DataSet DataSetSettings = new();

        private static readonly string ListViewLineSeparator = "<br />";
        private static string? MsgEmpty;
        private static string? MsgNotExist;
        private static string? MsgFormatNotSupported;
        private static string? MsgEncodingNotSupported;
        private static string? MsgOpenOrDrag;
        private static string? TitleInfo;
        private static string? TitleSuffix;
        public FormMain()
        {
            InitializeComponent();
            Controls.EnableDoubleBuffer();
            
            // DataSet Name
            DataSetSettings.DataSetName = "Settings";

            // DataSet Import
            string settingsFile = Tools.Info.ApplicationFullPathWithoutExtension + ".xml";
            if (PSF.IsSettingsValid(settingsFile) == true)
            {
                Tools.Xml.RemoveNodesWithoutChild(settingsFile);
                DataSetSettings.ToDataSet(settingsFile, XmlReadMode.Auto);
            }

            PSF.LoadTheme(this, Controls);

            Size = new Size(1150, 607);
            MinimumSize = new Size(1000, 547); // Main form minimum size
            MenuStrip1.Visible = false;
            CustomPanel1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            CustomDataGridView1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            CustomDataGridView1.RowTemplate.Height = 20;

            CustomVScrollBar vs1 = new();
            CustomDataGridView1.AddVScrollBar(vs1);

            MsgEmpty = "Subtitle File is Empty.";
            MsgNotExist = "Subtitle File Not Exist.";
            MsgFormatNotSupported = "Subtitle Format Is Not Supported.";
            MsgEncodingNotSupported = "Subtitle Encoding Is Not Supported.";
            MsgOpenOrDrag = "Open or Drag and Drop a Subtitle.";
            TitleInfo = Tools.Info.InfoExecutingAssembly.ProductName + " v" + Tools.Info.InfoExecutingAssembly.ProductVersion;
            TitleSuffix = " - " + TitleInfo;
            Text = TitleInfo;
            CustomTitleBar.TitleText = Text;
            ShowStatus(MsgOpenOrDrag, ForeColor, 0);

            DetectChanges();
            if (CustomDataGridView1.Columns.Count > 0)
            {
                CustomDataGridView1.Columns[2].HeaderText = "Text";
                CustomDataGridView1.Columns[2].Width = 800;
                CustomDataGridView1.Columns.RemoveAt(3);
                CustomDataGridView1.AutoSizeLastColumn();
            }
            ResetBegin();
            // Sort Column Line#
            lvwColumnSorter = new ListViewColumnSorter();

            EncodingTool.InitializeTextEncoding(CustomToolStripComboBoxEncoding.ComboBox);
            LoadCheckBoxes();

            // WaitLabel
            Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
            int titleHeight = screenRectangle.Top - Top;
            WaitLabel.AutoSize = false;
            WaitLabel.Size = new Size(150, 75);
            WaitLabel.TextAlign = ContentAlignment.MiddleCenter;
            WaitLabel.Dock = DockStyle.None;
            WaitLabel.Text = "Please Wait\nمنتظر بمانید";
            WaitLabel.Font = new Font("Tahoma", 14, FontStyle.Regular);
            Color bgColorWL = Color.FromArgb(50, BackColor);
            WaitLabel.BackColor = bgColorWL;
            WaitLabel.Top = (ClientSize.Height - WaitLabel.Size.Height) / 2 - titleHeight / 2;
            WaitLabel.Left = (ClientSize.Width - WaitLabel.Size.Width) / 2;
            WaitLabel.Anchor = AnchorStyles.Top;
            Controls.Add(WaitLabel);
            WaitLabel.SendToBack();
            WaitLabel.Hide();

            // Set CPU Affinity (Default)
            Process Proc = Process.GetCurrentProcess();
            IntPtr Affinity;
            int cpuCount = Environment.ProcessorCount;
            if (cpuCount == 1)
                Affinity = (IntPtr)0x0001; // Core 1
            else if (cpuCount == 2)
                Affinity = (IntPtr)0x0003; // Core 1 or 2
            else if (cpuCount == 3)
                Affinity = (IntPtr)0x0007; // Core 1, 2, or 3
            else if (cpuCount == 4)
                Affinity = (IntPtr)0x000F; // Core 1, 2, 3, or 4
            else
                Affinity = Proc.ProcessorAffinity;
            Proc.ProcessorAffinity = Affinity;
            Console.WriteLine("CPU Cores: " + cpuCount);
            Console.WriteLine("Current Process Affinity: " + Affinity);

            // Set Process Priority to High.
            Tools.ProcessManager.SetProcessPriority(ProcessPriorityClass.High);
        }

        public void DetectChanges()
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (s, e) =>
            {
                if (SubLoaded() == true)
                {
                    if (IsApplyTaskRunning() == false)
                    {
                        if (PSFUndoRedo.Undo == true)
                            ButtonUndo(true);
                        else
                            ButtonUndo(false);
                        if (PSFUndoRedo.Redo == true)
                            ButtonRedo(true);
                        else
                            ButtonRedo(false);
                        if (CustomToolStripComboBoxEncoding.SelectedItem != null)
                            if (EncodingTool.GetEncoding(CustomToolStripComboBoxEncoding) == Encoding.GetEncoding(1256))
                            {
                                FindCheckBoxByGroupName(PSF.Group_ChangeArabicCharsToPersian, false);
                                CheckboxHandler(null, null);
                            }
                            else
                            {
                                FindCheckBoxByGroupName(PSF.Group_ChangeArabicCharsToPersian, true);
                                CheckboxHandler(null, null);
                            }
                    }
                    else
                    {
                        // Disable everything when Task is running.
                        if (Text.StartsWith('*') == true)
                            Text = Text[1..];
                        ButtonSave(false);
                        ButtonUndo(false);
                        ButtonRedo(false);
                        ComboBoxes(false);
                        ButtonCheck(false);
                    }

                    if (ApplyState() == true)
                    {
                        if (Text.StartsWith('*') == false)
                            Text = '*' + Text;
                        ButtonSave(true);
                        ComboBoxes(false);
                        ButtonCheck(true);
                        UnSavedWork = true;
                    }
                    else
                    {
                        if (SubOriginal.ToText(SubFormat).Compare(SubCurrent.ToText(SubFormat)) == true
                            && EncodingTool.GetEncodingDisplayName(SubOriginalPath) == EncodingTool.GetEncodingDisplayName(CustomToolStripComboBoxEncoding)
                            && SubOriginalFormat == PSF.SubFormat.GetFormat(CustomToolStripComboBoxFormat.SelectedItem.ToString()))
                        {
                            if (IsApplyTaskRunning() == false)
                            {
                                if (Text.StartsWith('*') == true)
                                    Text = Text[1..];
                                ButtonSave(false);
                                ComboBoxes(true);
                                ButtonCheck(false);
                                CustomProgressBar1.Value = 0;
                            }
                            UnSavedWork = false;
                        }
                        else
                        {
                            if (IsApplyTaskRunning() == false)
                            {
                                if (Text.StartsWith('*') == false)
                                    Text = '*' + Text;
                                ButtonSave(true);
                                ComboBoxes(true);
                                ButtonCheck(false);
                                CustomProgressBar1.Value = 0;
                            }
                            UnSavedWork = true;
                        }
                    }
                }

                //if (Text.StartsWith('*'))
                //    UnSavedWork = true;
                //else
                //    UnSavedWork = false;
            };
            t.Start();
        }

        private bool ApplyState()
        {
            if (TaskApply != null)
                if (TaskApply.Status != TaskStatus.Running)
                    if (CustomDataGridView1.Columns.Count > 2)
                        if (CustomDataGridView1.Columns[2].HeaderText == "Before")
                        return true;
            return false;
        }

        private static bool IsApplyTaskRunning()
        {
            if (TaskApply != null)
                if (TaskApply.Status == TaskStatus.Running)
                    return true;
            return false;
        }

        private void ButtonSave(bool state)
        {
            ToolStripButtonSave.Enabled = state;
            ToolStripButtonSaveAs.Enabled = state;
            SaveToolStripMenuItem.Enabled = state;
            SaveAsToolStripMenuItem.Enabled = state;
        }

        private void ButtonCheck(bool state)
        {
            CustomButtonCheckAll.Enabled = state;
            CustomButtonInvertCheck.Enabled = state;
        }

        private void ButtonUndo(bool state)
        {
            ToolStripButtonUndo.Enabled = state;
            UndoToolStripMenuItem.Enabled = state;
        }

        private void ButtonRedo(bool state)
        {
            ToolStripButtonRedo.Enabled = state;
            RedoToolStripMenuItem.Enabled = state;
        }

        private void ComboBoxes(bool state)
        {
            CustomToolStripComboBoxEncoding.Enabled = state;
            CustomToolStripComboBoxFormat.Enabled = state;
        }

        private void ResetBegin()
        {
            CustomButtonStop.Enabled = false;
            if (TaskApply == null)
            { // At First Run
                CustomProgressBar1.Value = 0;
                if (SubPath == null)
                    CustomButtonApply.Enabled = false;
                ButtonSave(false);
                ButtonUndo(false);
                ButtonRedo(false);
                ComboBoxes(false);
                ButtonCheck(false);
            }
            if (SubLoaded() == false)
            { // At New Sub Opened But Its Empty Or Wrong
                ToolStripLabelRight.Text = string.Empty;
                CustomProgressBar1.Value = 0;
                CustomButtonApply.Enabled = false;
                ButtonSave(false);
                ButtonUndo(false);
                ButtonRedo(false);
                ComboBoxes(false);
                ButtonCheck(false);
            }
            else
            {
                if (ApplyState() == false)
                { // At New Sub Opened Successfully
                    //customProgressBar1.Value = 0;
                    ButtonSave(false);
                    ButtonUndo(false);
                    ButtonRedo(false);
                    ComboBoxes(true);
                    ButtonCheck(false);
                }
                else
                {
                    ComboBoxes(false);
                }
            }
        }

        private void ShowStatus(string message, Color color, int clearAfterSeconds)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (timerClearStatus.Enabled == true)
                    timerClearStatus.Stop();
                StatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = color);
                StatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = message);
                if (clearAfterSeconds > 0)
                {
                    timerClearStatus.Interval = clearAfterSeconds * 1000;
                    timerClearStatus.Tick += (s, e) =>
                    {
                        StatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = string.Empty);
                        timerClearStatus.Stop();
                    };
                    timerClearStatus.Start();
                }
                else
                {
                    if (timerClearStatus.Enabled == true)
                        timerClearStatus.Stop();
                }
            }
            else
            {
                if (timerClearStatus.Enabled == true)
                    timerClearStatus.Stop();
                StatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = string.Empty);
            }
        }

        private void LoadCheckBoxes()
        {
            DataSet ds;
            ds = DataSetSettings;

            List<string> items = PSF.ListGroupNames();
            int i = 0;
            for (int n = 0; n < items.Count; n++)
            {
                var item = items[n];
                i++;
                CustomCheckBox box = new();
                bool state = true;
                if (ds.Tables["CheckBoxes"] != null)
                {
                    string s = string.Empty;
                    if (ds.Tables["CheckBoxes"].Columns.Contains(item))
                        s = (string)ds.Tables["CheckBoxes"].Rows[0][item];
                    if (s.IsBool() == true)
                        state = bool.Parse(s);
                    else
                        state = true;
                }
                box.Checked = state;
                box.Tag = i.ToString();
                box.Text = item;
                box.Name = "checkBox" + i.ToString();
                box.Click += new EventHandler(CheckboxHandler);
                box.Location = new Point(10, (i - 1) * 20 + 2); //vertical
                                                            //box.Location = new Point(i * 50, 10); //horizontal
                CustomPanel1.Controls.Add(box); // Add CheckBoxes inside GroupBox1

                PSF.SetColors(box);
            }
            CheckboxHandler(null, null);
        }

        public void FindCheckBoxByGroupName(string groupName, bool TrueFalse)
        {
            //foreach (Control c in Panel1.Controls)
            for (int n = 0; n < CustomPanel1.Controls.Count; n++)
            {
                var c = CustomPanel1.Controls[n];
                if (c is CustomCheckBox box)
                {
                    if (box.Text == groupName)
                    {
                        box.Checked = TrueFalse;
                        box.Enabled = TrueFalse;
                    }
                }
            }
        }

        private void Checkboxes(bool state)
        {
            //foreach (Control c in Panel1.Controls)
            for (int n = 0; n < CustomPanel1.Controls.Count; n++)
            {
                var c = CustomPanel1.Controls[n];
                if (c is CustomCheckBox box)
                    box.Enabled = state;
            }
        }

        private void ApplyOnOff(bool state)
        {
            if (state == false)
            {
                WaitLabel.BringToFront();
                WaitLabel.Visible = true;
                if (Text.StartsWith('*') == true)
                    Text = Text[1..];
                if (TaskApply != null)
                    if (TaskApply.Status == TaskStatus.Running)
                        CustomButtonStop.Enabled = true;
            }
            else
            {
                WaitLabel.SendToBack();
                WaitLabel.Visible = false;
                WaitLabel.Hide();
                CustomButtonStop.Enabled = false;
                CustomDataGridView1.Invalidate();
            }
            CustomButtonApply.Enabled = state;
            ButtonSave(state);
            ButtonUndo(state);
            ButtonRedo(state);
            ButtonCheck(state);
            ComboBoxes(state);
            Checkboxes(state);
        }

        private void PleaseWait()
        {
            SourceApply = new CancellationTokenSource();
            var TokenApply = SourceApply.Token;
            try
            {
                TaskApply = Task.Run(() =>
                {
                    FindAndListFixes(TokenApply);
                }, TokenApply);
                //Process.GetCurrentProcess().Threads[Process.GetCurrentProcess().Threads.Count - 1].ProcessorAffinity = (IntPtr)0x000F;
                TaskApply.ContinueWith(_ =>
                {
                    Console.WriteLine("TaskApply: " + TaskApply.Status);
                }, TaskScheduler.FromCurrentSynchronizationContext());
                ApplyOnOff(false);
                var t = new System.Windows.Forms.Timer();
                t.Interval = 100;
                t.Tick += (s, e) =>
                {
                    if (TaskApply.IsCompleted == true)
                    {
                        ApplyOnOff(true);
                        ResetBegin();
                        // Auto Sort Column Line#
                        //if (lvwColumnSorter != null)
                        //{
                        //    lvwColumnSorter.SortColumn = 0;
                        //    lvwColumnSorter.Order = SortOrder.Ascending;
                        //    ListView1.Sort();
                        //}
                        //source.Dispose();
                        //TaskApply.Dispose();
                        t.Stop();
                    }
                };
                t.Start();
            }
            catch (OperationCanceledException ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                using (new Tools.CenterWinDialog(this))
                    CustomMessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private static bool SubExist(string subPath)
        {
            if (!string.IsNullOrEmpty(subPath))
                if (File.Exists(subPath))
                    if (new FileInfo(subPath).Length > 0)
                        return true;
            return false;
        }

        private bool SubLoaded()
        {
            if (!string.IsNullOrEmpty(SubPath))
                if (File.Exists(SubPath))
                    if (new FileInfo(SubPath).Length > 0)
                        if (SubCurrent != null && SubOriginal != null && SubFormat != null && SubOriginalFormat != null
                            && SubEncodingDisplayName != null && SubOriginalEncodingDisplayName != null
                            && SubEncoding != null && SubOriginalEncoding != null)
                            if (SubCurrent.Paragraphs.Count > 0)
                                return true;
            return false;
        }

        private void OpenFileError(OpenFileDialog fileDialog)
        {
            SubPath = fileDialog.FileName;
            OpenFileError(SubPath);
        }

        private void OpenFileError(string subPath)
        {
            Text = Path.GetFileName(subPath) + TitleSuffix;
            CustomTitleBar.TitleText = Text;
            SubPath = subPath;
            CustomDataGridView1.Rows.Clear();
            ResetBegin();
            if (!string.IsNullOrEmpty(SubPath))
            {
                if (File.Exists(SubPath))
                {
                    if (new FileInfo(SubPath).Length <= 0)
                    {
                        Console.WriteLine(MsgEmpty);
                        ShowStatus(MsgEmpty.IsNotNull(), ForeColor, 0);
                        using (new Tools.CenterWinDialog(this))
                            CustomMessageBox.Show(MsgEmpty, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null)
                        {
                            Console.WriteLine(MsgEncodingNotSupported);
                            ShowStatus(MsgEncodingNotSupported.IsNotNull(), ForeColor, 0);
                            using (new Tools.CenterWinDialog(this))
                                CustomMessageBox.Show(MsgEncodingNotSupported, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (SubFormat == null)
                        {
                            Console.WriteLine(MsgFormatNotSupported);
                            ShowStatus(MsgFormatNotSupported.IsNotNull(), ForeColor, 0);
                            using (new Tools.CenterWinDialog(this))
                                CustomMessageBox.Show(MsgFormatNotSupported, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    Console.WriteLine(MsgNotExist);
                    ShowStatus(MsgNotExist.IsNotNull(), ForeColor, 0);
                    using (new Tools.CenterWinDialog(this))
                        CustomMessageBox.Show(MsgNotExist, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Console.WriteLine(MsgOpenOrDrag);
                ShowStatus(MsgOpenOrDrag.IsNotNull(), ForeColor, 0);
                using (new Tools.CenterWinDialog(this))
                    CustomMessageBox.Show(MsgOpenOrDrag, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            ResetBegin(); // Just in case.
        }

        private void ToolStripButtonOpen_Click(object sender, EventArgs e)
        {
            var FileDialog1 = PSF.OpenFileDlg;
            if (FileDialog1.ShowDialog() == DialogResult.OK)
            {
                OpenFile(FileDialog1.FileName);
            }
        }

        private void FormMain_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
                e.Effect = DragDropEffects.Copy;
        }

        private void FormMain_DragDrop(object sender, DragEventArgs e)
        {
            string[] files = (string[])e.Data.GetData(DataFormats.FileDrop);
            if (files.Length > 1)
            {
                using (new Tools.CenterWinDialog(this))
                    CustomMessageBox.Show("Drag Only One File.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            foreach (string file in files)
            {
                if (Directory.Exists(file))
                {
                    using (new Tools.CenterWinDialog(this))
                        CustomMessageBox.Show("Drag A File Not A Directory.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                string fileExt = Path.GetExtension(file); // Ex: .srt
                // Check For Supported Subtitles
                if (!PSF.SubFormat.GetAllExtensions().Contains(fileExt))
                {
                    using (new Tools.CenterWinDialog(this))
                        CustomMessageBox.Show(MsgFormatNotSupported.IsNotNull(), "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                OpenFile(file);
            }
        }

        private void OpenFile(string filePath)
        {
            // Cancel Task If Its Running
            if (TaskApply != null)
            {
                if (!TaskApply.IsCompleted)
                {
                    SourceApply.Cancel();
                    CloseForm = true;
                    Task.Delay(100).Wait();
                }
            }

            if (SubExist(filePath) == false)
            {
                OpenFileError(filePath);
                return;
            }

            SubLines = File.ReadAllLines(filePath).ToList();

            if (SubTempPath != null)
                if (File.Exists(SubTempPath) == true)
                    File.Delete(SubTempPath);
            ShowStatus(string.Empty, ForeColor, 0);
            // Reset ComboBox Encoding
            EncodingTool.InitializeTextEncoding(CustomToolStripComboBoxEncoding.ComboBox);
            UpdateSub(filePath);
            UpdateSubOriginal(filePath);
            if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null || SubFormat == null)
                return;
            // Set ButtonOpen ToolTip
            ToolStripButtonOpen.ToolTipText = Path.GetFullPath(SubOriginalPath);
            CustomButtonApply.Enabled = true;
            Console.WriteLine("==================================================");
            Console.WriteLine("New Subtitle Loaded: " + SubOriginalPath);
            ShowStatus("New Subtitle Loaded: " + SubName, ForeColor, 5);
            // Set ButtonApply ToolTip
            CustomButtonApply.SetToolTip("Info", "You Can Apply Multiple Times.");
            Console.WriteLine("Subtitle Format: " + SubFormat.Name);
            Console.WriteLine("Subtitle Encoding Name: " + SubEncoding.EncodingName);

            PSFUndoRedo.UndoRedoList.Clear();
            SubTemp = new(SubCurrent);
            PSFUndoRedo.UndoRedo(SubTemp, SubOriginalEncodingDisplayName, SubFormat);
            Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
            LoadSubtitle(SubEncoding);
        }

        private void UpdateSub(string newPath)
        {
            // Update SubName, SubPath.
            SubName = Path.GetFileName(newPath);
            SubPath = newPath;
            // Update SubTempPath
            SubTempPath = Path.GetTempPath() + Path.GetFileName(SubPath);
            // Update Sub Encoding Display Name
            SubEncodingDisplayName = EncodingTool.GetEncodingDisplayName(SubPath);
            // Update Sub Encoding
            SubEncoding = EncodingTool.GetEncoding(SubEncodingDisplayName);
            if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null)
            {
                OpenFileError(SubPath);
                return;
            }
            // Reload
            SubCurrent = new Subtitle();
            SubFormat = SubCurrent.LoadSubtitle(SubPath, out _, SubEncoding);
            if (SubFormat == null)
            {
                OpenFileError(SubPath);
                return;
            }
            // Update SubTemp
            SubTemp = new Subtitle(SubCurrent);
            // Initialize and Set Format to ComboBox
            PSF.InitializeSubtitleFormat(CustomToolStripComboBoxFormat, SubFormat);
            // Update Text
            Text = Path.ChangeExtension(SubName, SubFormat.Extension) + TitleSuffix;
        }

        private void UpdateSubOriginal(string newPath)
        {
            SubOriginalPath = newPath;
            SubOriginalEncodingDisplayName = SubEncodingDisplayName;
            SubOriginalEncoding = SubEncoding;
            // Update SubOriginal Format
            SubOriginalFormat = SubFormat;
            // Update ButtonOpen ToolTip
            ToolStripButtonOpen.ToolTipText = Path.GetFullPath(SubOriginalPath);
            // Update SubOriginal
            SubOriginal = new Subtitle(SubCurrent);
        }

        private void LoadSubtitle(Encoding previewEncoding)
        {
            List<DataGridViewRow> pList = new();
            if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null)
            {
                OpenFileError(SubPath);
                return;
            }
            // Set Encoding to ComboBox
            EncodingTool.UpdateComboBoxEncoding(SubEncodingDisplayName, CustomToolStripComboBoxEncoding);
            // Set ComboBox Encoding ToolTip
            CustomToolStripComboBoxEncoding.ToolTipText = SubEncodingDisplayName;
            // Set Subtitle Format to ComboBox
            PSF.UpdateComboBoxFormat(SubFormat, CustomToolStripComboBoxFormat);
            UndoRedoClicked = false;
            // Set ComboBox Format ToolTip
            CustomToolStripComboBoxFormat.ToolTipText = CustomToolStripComboBoxFormat.SelectedItem.ToString();
            // Update Text
            Text = Path.ChangeExtension(SubName, SubFormat.Extension) + TitleSuffix;
            SubParagraphs.Clear();
            SubContainsPersianChars = false;

            if (CustomDataGridView1.Columns.Count > 0)
            {
                if (CustomDataGridView1.Columns.Count >= 3 && CustomDataGridView1.Columns[2].HeaderText == "Before")
                {
                    CustomDataGridView1.Columns[2].HeaderText = "Text";
                    CustomDataGridView1.Columns[2].Width = 800;
                    CustomDataGridView1.Columns.RemoveAt(3);
                }
                if (CustomDataGridView1.Columns[0].HeaderText == "Apply")
                    CustomDataGridView1.Columns.RemoveAt(0); // Remove Apply
            }
            CustomDataGridView1.Rows.Clear();
            CustomDataGridView1.Columns[0].ValueType = typeof(string); // CheckBox False

            //foreach (Paragraph p in SubCurrent.Paragraphs)
            for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
            {
                var p = SubCurrent.Paragraphs[pn];
                if (p.Text.ContainsPersianChars() == true)
                    SubContainsPersianChars = true;
                //var textPreview = previewEncoding.GetString(previewEncoding.GetBytes(p.Text));
                var textPreview = previewEncoding.GetString(Encoding.Convert(SubEncoding, previewEncoding, SubEncoding.GetBytes(p.Text)));
                
                SubParagraphs.Add(p);

                // Add one by one
                //int rowId = CustomDataGridView1.Rows.Add();
                //DataGridViewRow row = CustomDataGridView1.Rows[rowId];
                //row.Cells[0].Value = p.Number.ToString();
                //row.Cells[1].Value = textPreview.Replace(Environment.NewLine, ListViewLineSeparator);

                // Add by AddRange
                DataGridViewRow row = new();
                row.CreateCells(CustomDataGridView1, "cell0", "cell1");
                row.Height = 20;
                row.Cells[0].Value = p.Number.ToString();
                row.Cells[1].Value = textPreview.Replace(Environment.NewLine, ListViewLineSeparator);
                pList.Add(row);
            }
            CustomDataGridView1.Rows.AddRange(pList.ToArray());
            CustomDataGridView1.AutoSizeLastColumn();
            
            
            TotalRows = SubCurrent.Paragraphs.Count;
            ToolStripLabelRight.Text = CustomDataGridView1.SelectedRows.Count + "/" + TotalRows.ToString();
            Console.WriteLine("Subtitle Contains Persian Chars: " + SubContainsPersianChars);
            ResetBegin();
            //// Sort Column Line#
            //ListView1.ListViewItemSorter = lvwColumnSorter;
            //lvwColumnSorter.SortColumn = 0;
            //ListView1.Sorting = SortOrder.Ascending;
            //ListView1.AutoArrange = true;
            //lvwColumnSorter._SortModifier = ListViewColumnSorter.SortModifiers.SortByText;
        }

        public void FindAndListFixes(CancellationToken TokenApply)
        {
            DateTime startTime = DateTime.Now;
            StatusStrip1.InvokeIt(() => CustomProgressBar1.Value = 0);
            StatusStrip1.InvokeIt(() => CustomProgressBar1.Minimum = 0);
            StatusStrip1.InvokeIt(() => CustomProgressBar1.Maximum = 100);
            StatusStrip1.InvokeIt(() => CustomProgressBar1.Step = 1);
            TotalFixes = 0;
            //========== Creating List ============================================
            HashSet<ReplaceExpression> replaceExpressions = new();
            replaceExpressions.Clear();
            var fileContent = Tools.Resource.GetResourceTextFile(PSF.ResourcePath); // Load from Embedded Resource
            //foreach (Control cc in Panel1.Controls)
            for (int n = 0; n < CustomPanel1.Controls.Count; n++)
            {
                if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                    return;
                var cc = CustomPanel1.Controls[n];
                if ((cc is CustomCheckBox box) && box.Checked)
                {
                    XmlDocument doc = new();
                    if (fileContent != null)
                    {
                        doc.LoadXml(fileContent); // Load from String
                                                  //doc.Load(fileContent); // Load from URL
                        XmlNodeList nodes = doc.GetElementsByTagName("Group");
                        //foreach (XmlNode node in nodes)
                        for (int a = 0; a < nodes.Count; a++)
                        {
                            var node = nodes[a];
                            //Console.WriteLine(node.Name);
                            //foreach (XmlNode childN in node.SelectNodes("Name"))
                            for (int b = 0; b < node.SelectNodes("Name").Count; b++)
                            {
                                var childN = node.SelectNodes("Name")[b];
                                //Console.WriteLine("Group " + childN.Name + ": " + childN.InnerText);
                                if (childN.InnerText == cc.Text)
                                {
                                    //foreach (XmlNode child in node.SelectNodes("Enabled"))
                                    for (int c = 0; c < node.SelectNodes("Enabled").Count; c++)
                                    {
                                        var child = node.SelectNodes("Enabled")[c];
                                        //Console.WriteLine("Group " + child.Name + ": " + child.InnerText);
                                        if (child.InnerText == "True")
                                        {
                                            //foreach (XmlNode child1 in node.SelectNodes("MultipleSearchAndReplaceItem"))
                                            for (int d = 0; d < node.SelectNodes("MultipleSearchAndReplaceItem").Count; d++)
                                            {
                                                var child1 = node.SelectNodes("MultipleSearchAndReplaceItem")[d];
                                                //Console.WriteLine(child3.ChildNodes[0].Name); // Enabled
                                                //Console.WriteLine(child3.ChildNodes[1].Name); // FindWhat
                                                //Console.WriteLine(child3.ChildNodes[2].Name); // ReplaceWith
                                                //Console.WriteLine(child3.ChildNodes[3].Name); // SearchType
                                                //Console.WriteLine(child3.ChildNodes[4].Name); // Description
                                                if (child1.ChildNodes[0].InnerText == "True")
                                                {
                                                    string findWhat = @child1.ChildNodes[1].InnerText;
                                                    findWhat = findWhat.Replace("\"", "\\\"");
                                                    findWhat = findWhat.Replace("\\\\\"", "\\\"");
                                                    string replaceWith = @child1.ChildNodes[2].InnerText;
                                                    if (replaceWith == "")
                                                        replaceWith = " ";
                                                    string searchType = @child1.ChildNodes[3].InnerText;
                                                    if (!string.IsNullOrEmpty(findWhat)) // allow space or spaces
                                                    {
                                                        //findWhat = RegexUtils.FixNewLine(findWhat);
                                                        //replaceWith = RegexUtils.FixNewLine(replaceWith);
                                                        // Or
                                                        findWhat = findWhat.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                                        replaceWith = replaceWith.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
                                                        var mpi = new ReplaceExpression(findWhat, replaceWith, searchType);
                                                        replaceExpressions.Add(mpi);
                                                        if (mpi.SearchType == ReplaceExpression.SearchRegEx && !CompiledRegExList.ContainsKey(findWhat))
                                                        {
                                                            CompiledRegExList.Add(findWhat, new Regex(findWhat, RegexOptions.Compiled | RegexOptions.Multiline));
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }
            //========== Replacing List ===========================================
            List<DataGridViewRow> fixes = new();
            fixes.Clear();
            //Parallel.ForEach(_subtitle.Paragraphs, p =>
            //foreach (Paragraph p in SubCurrent.Paragraphs)
            //for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
            for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
            {
                if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                    return;
                Paragraph p = SubCurrent.Paragraphs[pn];
                // Progress bar
                int TC = SubCurrent.Paragraphs.Count;
                CustomProgressBar1.InvokeIt(() => CustomProgressBar1.Maximum = TC);
                int CC = p.Number;
                int VC = 0;
                if (TC != 0) // Solving: Attempted to divide by zero
                    VC = CC * 100 / TC;
                else
                    return;
                if (VC > 100)
                    return;
                CustomProgressBar1.InvokeIt(() => CustomProgressBar1.Value = CC);
                CustomProgressBar1.InvokeIt(() => CustomProgressBar1.CustomText = "Working On Line: " + CC + "/" + TC);
                CustomProgressBar1.InvokeIt(() => CustomProgressBar1.StartTime = startTime);

                //if (p.Text[0] == '"')
                //    p.Text = "\\\"" + p.Text.Substring(1);
                //if (p.Text[p.Text.Length - 1] == '"')
                //    p.Text = p.Text.Remove(p.Text.Length - 1, 1) + "\\\"";
                p.Text = p.Text.Replace("<br />", Environment.NewLine).Replace("</ br>", Environment.NewLine);
                string Before = @p.Text;
                string After = @p.Text;
                After = After.Replace("<br />", Environment.NewLine).Replace("</ br>", Environment.NewLine);

                foreach (ReplaceExpression item in replaceExpressions)
                {
                    if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                        return;
                    if (item.SearchType == ReplaceExpression.SearchRegEx)
                    {
                        Regex r = CompiledRegExList[item.FindWhat];
                        if (r.IsMatch(After))
                        {
                            After = RegexUtils.ReplaceNewLineSafe(r, After, item.ReplaceWith);
                        }
                    }
                    else if (item.SearchType == ReplaceExpression.SearchNormal)
                    {
                        After = After.Replace(item.FindWhat, item.ReplaceWith);
                    }
                }

                After = After.Trim();
                if (After != Before)
                {
                    //p.Text = After;
                    //SubCurrent.Paragraphs[pn].Text = After;

                    //Before = Tools.HTML.RemoveHtmlTags(Before);
                    //After = Tools.HTML.RemoveHtmlTags(After);
                    if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                        return;

                    CustomDataGridView1.InvokeIt(() =>
                    {
                        // Add one by one
                        //DataGridViewRow row = CustomDataGridView1.Rows[CustomDataGridView1.Rows.Add()];
                        //row.Cells[0].Value = true;
                        //CustomDataGridView1.EndEdit();
                        //row.Cells[1].Value = p.Number.ToString();
                        //row.Cells[2].Value = Before.Replace(Environment.NewLine, ListViewLineSeparator);
                        //row.Cells[3].Value = After.Replace(Environment.NewLine, ListViewLineSeparator);

                        // Add by AddRange
                        DataGridViewRow row = new();
                        row.CreateCells(CustomDataGridView1, "cell0", "cell1", "cell2", "cell3");
                        row.Height = 20;
                        row.Cells[0].Value = true;
                        CustomDataGridView1.EndEdit();
                        row.Cells[1].Value = p.Number.ToString();
                        row.Cells[2].Value = Before.Replace(Environment.NewLine, ListViewLineSeparator);
                        row.Cells[3].Value = After.Replace(Environment.NewLine, ListViewLineSeparator);
                        fixes.Add(row);
                    });

                    TotalFixes++;
                    ShowStatus(string.Format("Total Fixes: {0}", TotalFixes), ForeColor, 0);
                    StatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = TotalFixes <= 0 ? ControlPaint.Light(ForeColor) : ForeColor);
                }
            }
            CustomDataGridView1.InvokeIt(() => CustomDataGridView1.Rows.AddRange(fixes.ToArray()));
            //=============================================================================
            CustomProgressBar1.InvokeIt(() => CustomProgressBar1.CustomText = string.Empty);
            ShowStatus(string.Format("Total Fixes: {0}", TotalFixes), ForeColor, 0);
            StatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = TotalFixes <= 0 ? ControlPaint.Light(ForeColor) : ForeColor);
        }

        private void ApplyPreview()
        {
            if (SubLoaded() == true)
            {
                if (ApplyState() == true)
                {
                    ShowStatus("Please Wait...", ForeColor, 0);
                    Task.Delay(50).Wait(); // To Make ShowStatus Work.
                    int count = 0;
                    for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
                    {
                        Paragraph p = SubCurrent.Paragraphs[pn];
                        string ln = p.Number.ToString();
                        
                        for (int n = 0; n < CustomDataGridView1.Rows.Count; n++)
                        {
                            var row = CustomDataGridView1.Rows[n];
                            if (row.Cells[1].Value.ToString() == ln)
                            {
                                if ((bool)row.Cells[0].Value == false)
                                {
                                    p.Text = row.Cells[2].Value.ToString();
                                    p.Text = p.Text.Replace(ListViewLineSeparator, Environment.NewLine);
                                }
                                else if ((bool)row.Cells[0].Value == true)
                                {
                                    count++;
                                    p.Text = row.Cells[3].Value.ToString();
                                    p.Text = p.Text.Replace(ListViewLineSeparator, Environment.NewLine);
                                }
                            }
                        }
                    }
                    //----------------------------------------------------------------
                    if (count > 0)
                    {
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat);
                        Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
                    }
                }
            }
        }

        private void Apply()
        {
            if (SubPath != null)
                if (SubExist(SubPath) == true)
                {
                    if (CustomDataGridView1.Columns[0].HeaderText != "Apply")
                    {
                        DataGridViewCheckBoxColumn ApplyColumn = new();
                        ApplyColumn.HeaderText = "Apply";
                        ApplyColumn.CellTemplate = new DataGridViewCheckBoxCell();
                        ApplyColumn.ValueType = typeof(bool);
                        ApplyColumn.ReadOnly = false;
                        ApplyColumn.Width = 45;
                        CustomDataGridView1.Columns.Insert(0, ApplyColumn);
                    }
                    if (CustomDataGridView1.Columns.Count > 0)
                    {
                        CustomDataGridView1.Columns[2].HeaderText = "Before";
                        CustomDataGridView1.Columns[2].Width = 400;
                        if (CustomDataGridView1.Columns.Count <= 3)
                        {
                            DataGridViewColumn AfterColumn = new();
                            AfterColumn.HeaderText = "After";
                            AfterColumn.CellTemplate = new DataGridViewTextBoxCell();
                            AfterColumn.ValueType = typeof(string);
                            AfterColumn.ReadOnly = true;
                            AfterColumn.Width = 400;
                            CustomDataGridView1.Columns.Add(AfterColumn);
                        }
                    }
                    CustomDataGridView1.Rows.Clear();
                    CustomDataGridView1.Columns[CustomDataGridView1.Columns.Count - 2].Width = 400;
                    CustomDataGridView1.AutoSizeLastColumn();

                    PleaseWait();
                }
                else
                {
                    ShowStatus(MsgOpenOrDrag, ForeColor, 0);
                    return;
                }
        }

        private void ApplyToTemp()
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (s, e) =>
            {
                if (TaskApply.IsCompleted == true)
                {
                    if (TotalFixes == 0)
                    {
                        LoadSubtitle(SubEncoding);
                    }
                    t.Stop();
                }
                CustomDataGridView1.AutoSizeLastColumn();
            };
            t.Start();
        }

        private void CustomButtonApply_Click(object sender, EventArgs e)
        {
            CloseForm = false;
            ApplyOnOff(false);
            ApplyPreview();
            Apply();
            CustomDataGridView1.AutoSizeLastColumn();
            ApplyToTemp();
        }

        private void CustomButtonStop_Click(object sender, EventArgs e)
        {
            if (TaskApply != null)
                if (TaskApply.Status == TaskStatus.Running)
                {
                    SourceApply.Cancel();
                    CloseForm = true;
                    TaskApply.ContinueWith(t => LoadSubtitle(SubEncoding), TaskScheduler.FromCurrentSynchronizationContext());
                    //LoadSubtitle(SubEncoding);
                }
        }

        private async Task<string> SaveSubtitleToFileAsync(Subtitle subtitle, string savePath, string saveAction)
        {
            string successConsole = "File Saved: ";

            int codePage = SubOriginalEncoding.CodePage;
            byte[] sourceByte = Encoding.GetEncoding(codePage).GetBytes(SubCurrent.ToText(SubFormat));
            byte[] outputByte = Encoding.Convert(Encoding.GetEncoding(codePage), SubEncoding, sourceByte);
            string output = SubEncoding.GetString(outputByte);

            string newPath = Path.ChangeExtension(savePath, SubFormat.Extension);

            if (saveAction == "SaveAs")
                await Tools.Files.WriteAllTextAsync(newPath, output, SubEncoding);
            else if (saveAction == "Save")
            {
                if (SubFormat.Extension.Equals(SubOriginalFormat.Extension))
                    await Tools.Files.WriteAllTextAsync(newPath, output, SubEncoding);
                else
                {
                    if (!File.Exists(newPath))
                        await Tools.Files.WriteAllTextAsync(newPath, output, SubEncoding);
                    else
                    {
                        string msg = Path.GetFileName(newPath) + " already exists.\r\nDo you want to replace it?";
                        switch (CustomMessageBox.Show(msg, "Message", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                        {

                            case DialogResult.Yes:
                                await Tools.Files.WriteAllTextAsync(newPath, output, SubEncoding);
                                break;
                            case DialogResult.No:
                                return "Return";
                            default:
                                break;
                        }
                    }
                }
            }

            // Delete OldFile If Format Changed
            if (saveAction == "Save")
                if (SubFormat != SubOriginalFormat)
                    if (SubFormat.Extension != SubOriginalFormat.Extension)
                        if (File.Exists(savePath))
                            if (Tools.Files.IsFileLocked(savePath) == false) // and if (!File.Exists(newPath))
                                File.Delete(savePath);

            Task.Delay(50).Wait(); // To avoid process conflict.
            Console.WriteLine(successConsole + newPath);
            UpdateSub(newPath);
            SubTemp = new(SubCurrent);
            PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat);
            Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
            return newPath;
        }

        private async Task Save(string SavePath, string saveAction)
        {
            if (SubLoaded() == true && SubTempPath != null)
            {
                //var subRip = new Nikse.SubtitleEdit.Core.SubtitleFormats.SubRip();
                if (ApplyState() == true)
                {
                    ApplyPreview();
                }
                //-------------------------------------------------------------------
                SavePath = await SaveSubtitleToFileAsync(SubCurrent, SavePath, saveAction);
                if (SavePath == "Return")
                    return;
                //-------------------------------------------------------------------
                if (File.Exists(SubTempPath) == true)
                    File.Delete(SubTempPath);
                // Reset ComboBox Encoding
                EncodingTool.InitializeTextEncoding(CustomToolStripComboBoxEncoding.ComboBox);
                UpdateSub(SavePath);
                UpdateSubOriginal(SavePath);
                LoadSubtitle(SubEncoding);
                if (saveAction == "Save")
                    ShowStatus("File Saved: " + SubName, ForeColor, 5);
                if (saveAction == "SaveAs")
                    ShowStatus("File Saved As: " + SubName, ForeColor, 5);
            }
        }

        private async void ToolStripButtonSave_Click(object sender, EventArgs e)
        {
            if (SubOriginalPath != null)
            {
                string SavePath = SubOriginalPath;
                await Save(SavePath, "Save");
            }
        }

        private async void ToolStripButtonSaveAs_Click(object sender, EventArgs e)
        {
            var FileDialog1 = PSF.SaveFileDlg;
            FileDialog1.FileName = Path.ChangeExtension(SubName, SubFormat.Extension);
            if (FileDialog1.ShowDialog() == DialogResult.OK)
            {
                string SavePath = FileDialog1.FileName;
                await Save(SavePath, "SaveAs");
            }
        }

        private void ToolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (PSFUndoRedo.CurrentIndex > 0)
            {
                PSFUndoRedo.CurrentIndex--;
                SubCurrent = new(PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item1);
                SubEncodingDisplayName = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item2;
                SubEncoding = EncodingTool.GetEncoding(SubEncodingDisplayName);
                SubFormat = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item3;
                UndoRedoClicked = true;
                LoadSubtitle(SubEncoding);
            }
            Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
        }

        private void ToolStripButtonRedo_Click(object sender, EventArgs e)
        {
            if (PSFUndoRedo.CurrentIndex < PSFUndoRedo.UndoRedoList.Count - 1)
            {
                PSFUndoRedo.CurrentIndex++;
                SubCurrent = new(PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item1);
                SubEncodingDisplayName = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item2;
                SubEncoding = EncodingTool.GetEncoding(SubEncodingDisplayName);
                SubFormat = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item3;
                UndoRedoClicked = true;
                LoadSubtitle(SubEncoding);
            }
            Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
        }
        
        private void CustomToolStripComboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SubLoaded() == true && CustomToolStripComboBoxFormat.SelectedItem != null && UndoRedoClicked == false)
            {
                var newFormat = PSF.SubFormat.GetFormat(CustomToolStripComboBoxFormat.SelectedItem.ToString().IsNotNull());
                if (SubFormat == newFormat)
                {
                    string info = "No Need To Change Format.";
                    Console.WriteLine(info);
                    ShowStatus(info, ForeColor, 5);
                }
                else
                {
                    string msg = "Subtitle Format Changed To: " + newFormat.FriendlyName;
                    Console.WriteLine(msg);
                    ShowStatus(msg, ForeColor, 5);
                    SubFormat = newFormat;
                    LoadSubtitle(SubEncoding);
                    SubTemp = new(SubCurrent);
                    PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, newFormat);
                    Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
                }
            }
            UndoRedoClicked = false;
        }

        private void CustomToolStripComboBoxEncoding_SelectedIndexChanged(object sender, EventArgs e)
        {
            //CustomToolStripComboBoxEncoding.DroppedDown = true;
            if (SubLoaded() == true && CustomToolStripComboBoxEncoding.SelectedItem != null && UndoRedoClicked == false)
            {
                string dstDisplayName = EncodingTool.GetEncodingDisplayName(CustomToolStripComboBoxEncoding);
                Encoding subEncodingFromComboBox = EncodingTool.GetEncoding(CustomToolStripComboBoxEncoding);
                Console.WriteLine("Subtitle Encoding From ComboBox: " + subEncodingFromComboBox.WebName);
                if (SubEncodingDisplayName == dstDisplayName)
                {
                    string info = "No Need To Convert.";
                    Console.WriteLine(info);
                    ShowStatus(info, ForeColor, 5);
                }
                else
                {
                    ShowStatus("Converted To: " + dstDisplayName, ForeColor, 5);
                    SubEncodingDisplayName = dstDisplayName;
                    SubEncoding = subEncodingFromComboBox;
                    LoadSubtitle(SubEncoding);
                    SubTemp = new(SubCurrent);
                    PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat);
                    Console.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
                }
            }
            UndoRedoClicked = false;
        }

        private static void SettingsSaveCheckBoxes(string checkBoxName, string value)
        {
            string tableName = "CheckBoxes";
            if (!DataSetSettings.Tables.Contains(tableName))
            {
                DataTable dataTable = new();
                dataTable.TableName = tableName;
                DataSetSettings.Tables.Add(dataTable);
            }
            var dt = DataSetSettings.Tables[tableName];

            if (!dt.Columns.Contains(checkBoxName))
                dt.Columns.Add(checkBoxName);

            if (dt.Rows.Count == 0)
            {
                DataRow dataRow1 = dt.NewRow();
                dataRow1[checkBoxName] = value;
                dt.Rows.Add(dataRow1);
            }
            else
            {
                DataRow dataRow1 = dt.Rows[0];
                if (dataRow1 != null)
                    dataRow1[checkBoxName] = value;
            }
        }

        private void ToolStripButtonSettings_Click(object sender, EventArgs e)
        {
            Form settings = new Settings();
            settings.StartPosition = FormStartPosition.CenterParent;
            settings.ShowDialog(this);
            
        }

        private async void ToolStripButtonAbout_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog(this);
        }

        private void ToolStripButtonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Form_Closing(object sender, FormClosingEventArgs ex)
        {
            if (ex.CloseReason == CloseReason.UserClosing || ex.CloseReason == CloseReason.WindowsShutDown)
            {
                ex.Cancel = true;

                await Tools.Files.WriteAllTextAsync(Tools.Info.ApplicationFullPathWithoutExtension + ".xml",
                DataSetSettings.ToXmlWithWriteMode(XmlWriteMode.IgnoreSchema), new UTF8Encoding(false));

                if (UnSavedWork)
                {
                    string msg = "You have unsaved work.\r\nDo you really want to quit?";
                    switch (CustomMessageBox.Show(msg, "Unsaved work", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation))
                    {
                        case DialogResult.Yes:
                            {
                                if (TaskApply != null)
                                {
                                    if (!TaskApply.IsCompleted)
                                    {
                                        SourceApply.Cancel();
                                        CloseForm = true;
                                        _ = TaskApply.ContinueWith(t => Application.Exit(), TaskScheduler.FromCurrentSynchronizationContext());
                                    }
                                    else
                                        Application.Exit();
                                }
                                else
                                    Application.Exit();
                            }
                            break;
                        case DialogResult.No:
                            return;
                        default:
                            break;
                    }
                }
                else
                {
                    if (TaskApply != null)
                    {
                        if (!TaskApply.IsCompleted)
                        {
                            SourceApply.Cancel();
                            CloseForm = true;
                            _ = TaskApply.ContinueWith(t => Application.Exit(), TaskScheduler.FromCurrentSynchronizationContext());
                        }
                        else
                            Application.Exit();
                    }
                    else
                        Application.Exit();
                }
            }
        }

        private void CheckboxHandler(object? sender, EventArgs? e)
        {
            // Search and find Checkboxes
            int n = 0;
            //foreach (Control c in Panel1.Controls)
            for (int i = 0; i < CustomPanel1.Controls.Count; i++)
            {
                var c = CustomPanel1.Controls[i];
                if (c is CustomCheckBox box)
                {
                    SettingsSaveCheckBoxes(box.Text, box.Checked.ToString());
                    if (box.Checked == true)
                        n++;
                }
            }
            if (n > 0 && SubExist(SubPath) == true)
                CustomButtonApply.Enabled = true;
            else
                CustomButtonApply.Enabled = false;
        }

        private void SelectionHandler(object sender, EventArgs e)
        {
            if (CustomDataGridView1.Rows.Count <= 0)
                return;
            if ((sender as Button).Text == "Select All")
            {
                //foreach (ListViewItem item in ListView1.Items)
                for (int n = 0; n < CustomDataGridView1.Rows.Count; n++)
                {
                    var row = CustomDataGridView1.Rows[n];
                    row.Cells[0].Value = true;
                }
            }
            else
            {
                //foreach (ListViewItem item in ListView1.Items)
                for (int n = 0; n < CustomDataGridView1.Rows.Count; n++)
                {
                    var row = CustomDataGridView1.Rows[n];
                    row.Cells[0].Value = !(bool)row.Cells[0].Value;
                }
            }
        }

        private void CustomDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            if (CustomDataGridView1.SelectedRows.Count > 0)
            {
                var t = new System.Windows.Forms.Timer();
                t.Interval = 10;
                t.Tick += (s, e) =>
                {
                    int colIndex;
                    if (ApplyState() == true)
                        colIndex = 1;
                    else
                        colIndex = 0;
                    string CurrentRow = string.Empty;
                    if (CustomDataGridView1.SelectedRows[0].Cells[colIndex].ValueType == typeof(string))
                        CurrentRow = (string)CustomDataGridView1.SelectedRows[0].Cells[colIndex].Value;
                    ToolStripLabelRight.Text = CurrentRow + "/" + TotalRows.ToString();
                    t.Stop();
                };
                t.Start();
            }
        }

        private void ListView1_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            ListView myListView = (ListView)sender;
            if (lvwColumnSorter != null)
            {
                // Determine if clicked column is already the column that is being sorted.
                if (e.Column == lvwColumnSorter.ColumnToSort)
                {
                    // Reverse the current sort direction for this column.
                    if (lvwColumnSorter.OrderOfSort == SortOrder.Ascending)
                        lvwColumnSorter.OrderOfSort = SortOrder.Descending;
                    else
                        lvwColumnSorter.OrderOfSort = SortOrder.Ascending;
                }
                else
                {
                    // Set the column number that is to be sorted; default to ascending.
                    lvwColumnSorter.ColumnToSort = e.Column;
                    lvwColumnSorter.OrderOfSort = SortOrder.Ascending;
                }
                // Perform the sort with these new sort options.
                myListView.Sort();
            }
        }

        private void FormMain_SizeChanged(object sender, EventArgs e)
        {
            CustomDataGridView1.AutoSizeLastColumn();
        }

        private void BackgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            //var t = new System.Windows.Forms.Timer();
            //t.Interval = 500;
            //t.Tick += (s, e) =>
            //{
            //    if (ToolStripLabelLeft.Text == string.Empty)
            //        ToolStripLabelLeft.Text = OpenOrDrag;
            //    else
            //        ToolStripLabelLeft.Text = string.Empty;
            //    if (SubExist(SubPath) == true)
            //        t.Stop();
            //};
            //t.Start();
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            // Set Shortcuts
            //if (e.Control && e.KeyCode == Keys.S)
            //{
            //    // Do Work
            //}
        }

        
    }
}