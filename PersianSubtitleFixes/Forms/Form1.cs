using CustomControls;
using MsmhTools;
using Nikse.SubtitleEdit.Core.Common;
using Nikse.SubtitleEdit.Core.SubtitleFormats;
using PersianSubtitleFixes.ColumnSorter;
using PSFTools;
using System.Data;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;

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
        public static Subtitle? SubCurrent { get; set; } = new();
        private Subtitle? SubOriginal;
        private Subtitle? SubTemp;
        private List<Paragraph> SubParagraphs = new();
        private List<string> SubLines = new();
        private MemoryStream SubMemoryStream = new();
        public static string? SubName { get; set; }
        public static string? SubPath { get; set; }
        public static string? SubOriginalPath { get; set; }
        public static string? SubTempPath { get; set; }
        public static SubtitleFormat? SubFormat { get; set; }
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
        private readonly static CustomLabel WaitLabel = new();
        private readonly static PictureBox PictureBoxGuide = new();
        private static bool UndoRedoClicked = false;
        private static bool UnSavedWork = false;
        public static bool IsSubLoaded = false;

        public static DataSet DataSetSettings = new();

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

            // Initialize Settings
            PSFSettings.Initialize(this);

            // Load Theme
            Theme.LoadTheme(this, Controls);

            // Load XML Replace List
            MultipleReplace.LoadXmlRL(PSF.ReplaceListPath);
            MultipleReplace.LoadXmlRL(PSF.UserReplaceListPath);

            Size = new Size(1150, 632);
            MinimumSize = new Size(1000, 492); // Main form minimum size
            CustomMenuStrip1.Visible = true;
            CustomMenuStrip1.BringToFront();
            CustomDataGridView1.RowTemplate.Height = 20;

            MsgEmpty = "Subtitle file is empty.";
            MsgNotExist = "Subtitle file not exist.";
            MsgFormatNotSupported = "Subtitle format is not supported.";
            MsgEncodingNotSupported = "Subtitle encoding is not supported.";
            MsgOpenOrDrag = "Open or Drag-n-Drop a subtitle.";
            TitleInfo = Tools.Info.InfoExecutingAssembly.ProductName + " v" + Tools.Info.InfoExecutingAssembly.ProductVersion;
            TitleSuffix = " - " + TitleInfo;
            Text = TitleInfo;

            ShowStatus(MsgOpenOrDrag, ForeColor, 0);
            ToolStripLabelRight.Text = string.Empty;

            // Set ButtonApply ToolTip
            CustomButtonApply.SetToolTip("Info", "You Can Apply Multiple Times.");

            var dgv = CustomDataGridView1;
            PSF.LoadColumnsLoadState(dgv);

            DetectChanges();
            // Sort Column Line#
            lvwColumnSorter = new ListViewColumnSorter();

            EncodingTool.InitializeTextEncoding(CustomToolStripComboBoxEncoding.ComboBox);
            LoadCheckBoxes();

            // Load Settings
            PSFSettings.Load(this, PSFSettings.SettingsName.General);
            PSFSettings.Load(this, PSFSettings.SettingsName.Timing);
            PSFSettings.Load(this, PSFSettings.SettingsName.Others);

            // WaitLabel
            Rectangle screenRectangle = RectangleToScreen(ClientRectangle);
            int titleHeight = screenRectangle.Top - Top;
            WaitLabel.AutoSize = false;
            WaitLabel.Size = new Size(150, 75);
            WaitLabel.TextAlign = ContentAlignment.MiddleCenter;
            WaitLabel.Dock = DockStyle.None;
            WaitLabel.Text = "Please Wait\nمنتظر بمانید";
            WaitLabel.Font = new Font("Tahoma", 14, FontStyle.Regular);
            //WaitLabel.BackColor = Color.FromArgb(50, BackColor);
            WaitLabel.Border = true;
            WaitLabel.RoundedCorners = 10;
            WaitLabel.Top = (ClientSize.Height - WaitLabel.Size.Height) / 2 - titleHeight / 2;
            WaitLabel.Left = (ClientSize.Width - WaitLabel.Size.Width) / 2;
            WaitLabel.Anchor = AnchorStyles.Top;
            Controls.Add(WaitLabel);
            Theme.SetColors(WaitLabel);
            WaitLabel.SendToBack();
            WaitLabel.Hide();

            // PictureBox Guide
            PictureBoxGuide.Size = new Size(200, 200);
            PictureBoxGuide.Dock = DockStyle.None;
            PictureBoxGuide.Top = (ClientSize.Height - PictureBoxGuide.Size.Height) / 2 - titleHeight / 2;
            PictureBoxGuide.Left = (ClientSize.Width - PictureBoxGuide.Size.Width) / 2;
            PictureBoxGuide.Anchor = AnchorStyles.Top;
            Controls.Add(PictureBoxGuide);
            PictureBoxGuide.SendToBack();

            // Timing
            // Hide Horizontal ScrollBar
            TabPageCommonErrors.AutoScroll = false;
            TabPageCommonErrors.HorizontalScroll.Maximum = 0;
            //TabPageTexts.VerticalScroll.Visible = false;
            TabPageCommonErrors.AutoScroll = true;

            CustomLabelMinGap.SetToolTip("Gap", "Apply minimum gap between subtitles.");

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
            Debug.WriteLine("CPU Cores: " + cpuCount);
            Debug.WriteLine("Current Process Affinity: " + Affinity);

            // Set Process Priority to High.
            Tools.ProcessManager.SetProcessPriority(ProcessPriorityClass.High);
        }

        public void DetectChanges(int timesToRepeat = 1)
        {
            for (int n = 0; n < timesToRepeat; n++)
            {
                dc();
                Task.Delay(150).Wait();
            }

            void dc()
            {
                if (SubLoaded()) // Sub Loaded
                {
                    if (ApplyState()) // Sub Loaded and Apply State
                    {
                        ShowWaitLabel(false);

                        ComboBoxes(false);
                        Checkboxes(true);
                        ButtonCheck(true);
                        TabPages(CustomTabControl1, true);
                        ChangesCheckSub();
                        ChangesUndoRedo();
                        ChangesNotUnicode();

                        CustomButtonApply.Enabled = true;
                        CustomButtonStop.Enabled = false;
                        CustomButtonCancel.Enabled = true;
                    }
                    else
                    {
                        if (IsApplyTaskRunning()) // Sub Loaded and Task is running.
                        {
                            ShowWaitLabel(true);

                            ComboBoxes(false);
                            Checkboxes(false);
                            ButtonCheck(false);
                            TabPages(CustomTabControl1, false);

                            if (Text.StartsWith('*'))
                                Text = Text[1..];
                            ButtonSave(false);
                            ButtonUndo(false);
                            ButtonRedo(false);
                            ChangesNotUnicode();
                            
                            CustomButtonApply.Enabled = false;
                            CustomButtonStop.Enabled = true;
                            CustomButtonCancel.Enabled = false;
                        }
                        else // Sub Loaded and Normal State
                        {
                            ShowWaitLabel(false);

                            CustomProgressBar1.Value = 0;
                            ComboBoxes(true);
                            Checkboxes(true);
                            ButtonCheck(false);
                            TabPages(CustomTabControl1, true);

                            ChangesCheckSub();
                            ChangesUndoRedo();
                            ChangesNotUnicode();

                            CustomButtonApply.Enabled = true;
                            CustomButtonStop.Enabled = false;
                            CustomButtonCancel.Enabled = false;
                        }
                    }
                }
                else
                {
                    // Sub Is Not Loaded
                    ChangesNotLoaded();
                }
            }
        }

        private void ChangesNotLoaded()
        {
            CustomProgressBar1.Value = 0;
            ButtonSave(false);
            ButtonUndo(false);
            ButtonRedo(false);
            ComboBoxes(false);
            ButtonCheck(false);
            CustomButtonApply.Enabled = false;
            CustomButtonStop.Enabled = false;
            CustomButtonCancel.Enabled = false;
            CustomButtonApplyTiming.Enabled = false;
            CustomButtonSyncEarlier.Enabled = false;
            CustomButtonSyncLater.Enabled = false;
            CustomButtonChangeFrameRate.Enabled = false;
            CustomButtonChangeSpeed.Enabled = false;
            CustomButtonApplyOthers.Enabled = false;
        }

        private void ChangesUndoRedo()
        {
            if (PSFUndoRedo.Undo)
                ButtonUndo(true);
            else
                ButtonUndo(false);

            if (PSFUndoRedo.Redo)
                ButtonRedo(true);
            else
                ButtonRedo(false);
        }

        private void ChangesNotUnicode()
        {
            if (CustomToolStripComboBoxEncoding.SelectedItem != null)
                if (EncodingTool.GetEncoding(CustomToolStripComboBoxEncoding) == Encoding.GetEncoding(1256))
                {
                    FindCheckBoxByGroupName(PSF.Group_FixUnicodeControlChar, false);
                    FindCheckBoxByGroupName(PSF.Group_ChangeArabicCharsToPersian, false);
                    CheckboxHandler(null, null);
                }
                else
                {
                    FindCheckBoxByGroupName(PSF.Group_FixUnicodeControlChar, true);
                    FindCheckBoxByGroupName(PSF.Group_ChangeArabicCharsToPersian, true);
                    CheckboxHandler(null, null);
                }
        }

        private bool ChangesCheckSub()
        {
            if (SubOriginal.ToText(SubFormat).Compare(SubCurrent.ToText(SubFormat)) == true &&
                EncodingTool.GetEncodingDisplayName(SubOriginalPath.IsNotNull()) == EncodingTool.GetEncodingDisplayName(CustomToolStripComboBoxEncoding) &&
                SubOriginalFormat == PSF.SubFormat.GetFormat(CustomToolStripComboBoxFormat.SelectedItem.ToString().IsNotNull()))
            {
                if (Text.StartsWith('*'))
                    Text = Text[1..];
                ButtonSave(false);
                UnSavedWork = false;
            }
            else
            {
                if (!Text.StartsWith('*'))
                    Text = '*' + Text;
                ButtonSave(true);
                UnSavedWork = true;
            }
            return UnSavedWork;
        }

        private static bool SubContentChanged()
        {
            Subtitle subToCheck = new(PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item1);
            return !subToCheck.ToText(SubFormat).Compare(SubCurrent.ToText(SubFormat));
        }

        private void ShowWaitLabel(bool state)
        {
            if (state == true)
            {
                WaitLabel.BringToFront();
                WaitLabel.Visible = true;
            }
            else
            {
                WaitLabel.SendToBack();
                WaitLabel.Visible = false;
                WaitLabel.Hide();
                CustomDataGridView1.Invalidate();
            }
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

        private void TabPages(TabControl tabControl, bool state)
        {
            for (int n = 0; n < tabControl.TabPages.Count; n++)
            {
                TabPage tabPage = tabControl.TabPages[n];
                var list = Tools.Controllers.GetAllChildControls(tabPage);
                for (int i = 0; i < list.Count; i++)
                {
                    Control c = list.ToList()[i];
                    if (c.Name != CustomButtonApply.Name ||
                        c.Name != CustomButtonStop.Name ||
                        c.Name != CustomButtonCancel.Name)
                        c.Enabled = state;
                }
            }
        }

        private void ShowStatus(string message, Color color, int clearAfterSeconds)
        {
            if (!string.IsNullOrEmpty(message))
            {
                if (timerClearStatus.Enabled)
                    timerClearStatus.Stop();
                CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = color);
                CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = message);
                if (clearAfterSeconds > 0)
                {
                    timerClearStatus.Interval = clearAfterSeconds * 1000;
                    timerClearStatus.Tick += (s, e) =>
                    {
                        CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = string.Empty);
                        timerClearStatus.Stop();
                    };
                    timerClearStatus.Start();
                }
            }
            else
            {
                if (timerClearStatus.Enabled)
                    timerClearStatus.Stop();
                CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.Text = string.Empty);
            }
        }
        
        public void LoadCheckBoxes()
        {
            while (TabPageCommonErrors.Controls.Count != 3)
            {
                for (int n = 0; n < TabPageCommonErrors.Controls.Count; n++)
                {
                    var control = TabPageCommonErrors.Controls[n];
                    if (control is CustomCheckBox box)
                        TabPageCommonErrors.Controls.Remove(box);
                }
                if (TabPageCommonErrors.Controls.Count == 3)
                    break;
            }

            DataSet ds;
            ds = DataSetSettings;

            // Load Default Replace List
            List<string> items = PSF.ListGroupNames(PSF.ReplaceListPath);
            int groupsCount = 0;

            if (items != null)
                groupsCount = items.Count;

            int i = 0;
            for (int n = 0; n < groupsCount; n++)
            {
                string item = items[n];
                i++;
                CustomCheckBox box = new();
                bool state = true;
                if (ds.Tables[PSFSettings.SettingsName.CheckBoxes] != null)
                {
                    string s = string.Empty;
                    if (ds.Tables[PSFSettings.SettingsName.CheckBoxes].Columns.Contains(item))
                        s = (string)ds.Tables[PSFSettings.SettingsName.CheckBoxes].Rows[0][item];
                    if (s.IsBool() == true)
                        state = bool.Parse(s);
                    else
                        state = true;
                }
                box.Checked = state;
                box.Tag = item;
                box.Text = item;
                box.Name = "Default" + i.ToString();
                box.Click += new EventHandler(CheckboxHandler);
                box.Location = new Point(10, (i - 1) * 20 + 2);
                box.Anchor = AnchorStyles.Top; // To Avoid Having Horizontal ScrollBar
                TabPageCommonErrors.Controls.Add(box);

                if (box.Text == PSF.Group_FixUnicodeControlChar)
                    box.SetToolTip("Info", "It's readonly.");
                else if (box.Text == PSF.Group_ChangeArabicCharsToPersian)
                    box.SetToolTip("Info", "It's readonly.");

                Theme.SetColors(box);
                PSFTools.Guide.Help(box, PictureBoxGuide, ShowPopupGuideToolStripMenuItem);
            }
            int y1 = groupsCount * 20;

            // Load User Replace List
            List<string> userItems = PSF.ListGroupNames(PSF.UserReplaceListPath);
            int userGroupsCount = 0;

            if (userItems != null)
                userGroupsCount = userItems.Count;

            int j = 0;
            for (int n = 0; n < userGroupsCount; n++)
            {
                string userItem = userItems[n];
                j++;
                CustomCheckBox userBox = new();
                bool state = true;
                if (ds.Tables[PSFSettings.SettingsName.CheckBoxes] != null)
                {
                    string s = string.Empty;
                    if (ds.Tables[PSFSettings.SettingsName.CheckBoxes].Columns.Contains(userItem))
                        s = (string)ds.Tables[PSFSettings.SettingsName.CheckBoxes].Rows[0][userItem];
                    if (s.IsBool() == true)
                        state = bool.Parse(s);
                    else
                        state = true;
                }
                userBox.Checked = state;
                userBox.Tag = userItem;
                userBox.Text = userItem;
                userBox.Name = "User" + j.ToString();
                userBox.Click += new EventHandler(CheckboxHandler);
                userBox.Location = new Point(10, ((j - 1) * 20 + 2) + y1);
                userBox.Anchor = AnchorStyles.Top; // To Avoid Having Horizontal ScrollBar
                TabPageCommonErrors.Controls.Add(userBox);

                Theme.SetColors(userBox);
            }
            int y2 = ((userGroupsCount * 20) + 10) + y1;

            // Set Apply, Stop and Cancel Button Location
            CustomButtonApply.Location = new(26, y2);
            CustomButtonStop.Location = new(121, y2);
            CustomButtonCancel.Location = new(74, y2 + CustomButtonApply.Height + 10);

            CheckboxHandler(null, null);
        }

        public void FindCheckBoxByGroupName(string groupName, bool state)
        {
            for (int n = 0; n < TabPageCommonErrors.Controls.Count; n++)
            {
                var c = TabPageCommonErrors.Controls[n];
                if (c is CustomCheckBox box)
                {
                    if (box.Text == groupName)
                    {
                        box.Checked = state;
                        box.AutoCheck = false;
                    }
                }
            }
        }

        private void Checkboxes(bool state)
        {
            for (int n = 0; n < TabPageCommonErrors.Controls.Count; n++)
            {
                var c = TabPageCommonErrors.Controls[n];
                if (c is CustomCheckBox box)
                    box.Enabled = state;
            }
        }

        private int GetFirstDisplayedRowindex()
        {
            var dgv = CustomDataGridView1;
            if (dgv.RowCount > 0)
                return dgv.FirstDisplayedScrollingRowIndex;
            else
                return 0;
        }

        private int SetFirstDisplayedRowindex(int firstVisible)
        {
            var dgv = CustomDataGridView1;
            if (dgv.RowCount > 0)
            {
                if (firstVisible > dgv.RowCount - 1)
                    return dgv.RowCount - 1 - dgv.DisplayedRowCount(false);
                else
                    return firstVisible;
            }
            else
                return 0;
        }

        private int GetSelectedRow()
        {
            var dgv = CustomDataGridView1;
            if (dgv.SelectedRows.Count > 0)
                return dgv.SelectedRows[0].Index;
            else
                return 0;
        }

        private int SetSelectedRow(int selectedRow)
        {
            var dgv = CustomDataGridView1;
            if (dgv.RowCount > 0)
            {
                if (selectedRow > dgv.RowCount - 1)
                    return dgv.RowCount - 1;
                else
                    return selectedRow;
            }
            else
                return 0;
        }

        private string GetCurrentRow()
        {
            var dgv = CustomDataGridView1;
            int colIndex = ApplyState() ? 1 : 0;
            string CurrentRow = "0";
            try
            {
                if (dgv.RowCount > 0 && dgv.SelectedRows[0].Cells.Count > 1)
                {
                    if (dgv.SelectedRows[0].Cells[colIndex].ValueType == typeof(string))
                        CurrentRow = (string)dgv.SelectedRows[0].Cells[colIndex].Value;
                }
            }
            catch (ArgumentOutOfRangeException msg)
            {
                Debug.WriteLine("Error: " + msg.Message);
            }
            return CurrentRow;
        }

        private static bool SubExist(string? subPath)
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
                            {
                                IsSubLoaded = true;
                                return true;
                            }
            IsSubLoaded = false;
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
            SubPath = subPath;
            CustomDataGridView1.Rows.Clear();
            ToolStripLabelRight.Text = string.Empty;
            if (!string.IsNullOrEmpty(SubPath))
            {
                if (File.Exists(SubPath))
                {
                    if (new FileInfo(SubPath).Length <= 0)
                    {
                        Debug.WriteLine(MsgEmpty);
                        ShowStatus(MsgEmpty.IsNotNull(), ForeColor, 0);
                        using (new Tools.CenterWinDialog(this))
                            CustomMessageBox.Show(MsgEmpty, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    else
                    {
                        if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null)
                        {
                            Debug.WriteLine(MsgEncodingNotSupported);
                            ShowStatus(MsgEncodingNotSupported.IsNotNull(), ForeColor, 0);
                            using (new Tools.CenterWinDialog(this))
                                CustomMessageBox.Show(MsgEncodingNotSupported, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else if (SubFormat == null)
                        {
                            Debug.WriteLine(MsgFormatNotSupported);
                            ShowStatus(MsgFormatNotSupported.IsNotNull(), ForeColor, 0);
                            using (new Tools.CenterWinDialog(this))
                                CustomMessageBox.Show(MsgFormatNotSupported, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
                else
                {
                    Debug.WriteLine(MsgNotExist);
                    ShowStatus(MsgNotExist.IsNotNull(), ForeColor, 0);
                    using (new Tools.CenterWinDialog(this))
                        CustomMessageBox.Show(MsgNotExist, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                Debug.WriteLine(MsgOpenOrDrag);
                ShowStatus(MsgOpenOrDrag.IsNotNull(), ForeColor, 0);
                using (new Tools.CenterWinDialog(this))
                    CustomMessageBox.Show(MsgOpenOrDrag, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            DetectChanges();
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

            if (!SubExist(filePath))
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
            string updateSub = UpdateSub(filePath);
            if (updateSub == null) return;
            UpdateSubOriginal(filePath);
            // Set ButtonOpen ToolTip
            ToolStripButtonOpen.ToolTipText = Path.GetFullPath(SubOriginalPath.IsNotNull());
            ShowStatus("New subtitle loaded: " + SubName, ForeColor, 5);
            Debug.WriteLine("==================================================");
            Debug.WriteLine("New Subtitle Loaded: " + SubOriginalPath);
            Debug.WriteLine("Subtitle Format: " + SubFormat.Name);
            Debug.WriteLine("Subtitle Encoding Name: " + SubEncoding.EncodingName);

            PSFUndoRedo.UndoRedoList.Clear();
            SubTemp = new(SubCurrent);
            PSFUndoRedo.UndoRedo(SubTemp, SubOriginalEncodingDisplayName, SubFormat, "New Subtitle Loaded: " + SubOriginalPath);
            Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
            LoadSubtitle(SubEncoding, 0, 0);
        }

        private string? UpdateSub(string newPath)
        {
            // Update Sub Name, Sub Path
            SubName = Path.GetFileName(newPath);
            SubPath = newPath;

            // Update SubTemp Path
            SubTempPath = Path.GetTempPath() + Path.GetFileName(SubPath);

            // Update Sub Encoding Display Name
            SubEncodingDisplayName = EncodingTool.GetEncodingDisplayName(SubPath);
            if (string.IsNullOrWhiteSpace(SubEncodingDisplayName))
            {
                OpenFileError(SubPath);
                return null;
            }

            // Update Sub Encoding
            SubEncoding = EncodingTool.GetEncoding(SubEncodingDisplayName);
            if (SubEncoding == null)
            {
                OpenFileError(SubPath);
                return null;
            }

            // Reload
            SubCurrent = new Subtitle();
            SubFormat = SubCurrent.LoadSubtitle(SubPath, out _, SubEncoding);
            if (SubFormat == null)
            {
                OpenFileError(SubPath);
                return null;
            }

            // Update SubTemp
            SubTemp = new Subtitle(SubCurrent);

            // Initialize and Set Format to ComboBox
            PSF.InitializeSubtitleFormat(CustomToolStripComboBoxFormat, SubFormat);

            // Update Text
            Text = Path.ChangeExtension(SubName, SubFormat.Extension) + TitleSuffix;

            DetectChanges();
            return "NotNull";
        }

        private void UpdateSubOriginal(string newPath)
        {
            // Update SubOriginal Path
            SubOriginalPath = newPath;

            // Update SubOriginal Encoding Display Name
            SubOriginalEncodingDisplayName = SubEncodingDisplayName;

            // Update SubOriginal Encoding
            SubOriginalEncoding = SubEncoding;

            // Update SubOriginal Format
            SubOriginalFormat = SubFormat;

            // Update ButtonOpen ToolTip
            ToolStripButtonOpen.ToolTipText = Path.GetFullPath(SubOriginalPath);

            // Update SubOriginal
            SubOriginal = new Subtitle(SubCurrent);

            DetectChanges();
        }

        private void LoadSubtitle(Encoding? previewEncoding, int firstVisible, int selectRow)
        {
            if (!SubLoaded()) return;
            previewEncoding ??= Encoding.UTF8;

            var dgv = CustomDataGridView1;

            PSF.LoadColumnsLoadState(dgv);

            List<DataGridViewRow> pList = new();
            if (string.IsNullOrWhiteSpace(SubEncodingDisplayName) || SubEncoding == null || SubFormat == null)
            {
                OpenFileError(SubPath.IsNotNull());
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
            Text = GetStar() + Path.ChangeExtension(SubName, SubFormat.Extension) + TitleSuffix;
            static string GetStar()
            {
                //if (UnSavedWork) return "*"; else return string.Empty;
                return UnSavedWork ? "*" : string.Empty;
            }
            SubParagraphs.Clear();
            SubContainsPersianChars = false;

            for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = SubCurrent.Paragraphs[pn];
                if (p.Text.ContainsPersianChars())
                    SubContainsPersianChars = true;
                //var textPreview = previewEncoding.GetString(previewEncoding.GetBytes(p.Text));
                string textPreview = previewEncoding.GetString(Encoding.Convert(SubEncoding, previewEncoding, SubEncoding.GetBytes(p.Text)));

                SubParagraphs.Add(p);

                // Add one by one
                //int rowId = CustomDataGridView1.Rows.Add();
                //DataGridViewRow row = CustomDataGridView1.Rows[rowId];

                // Add by AddRange
                DataGridViewRow row = new();
                row.CreateCells(CustomDataGridView1, "cell0", "cell1", "cell2", "cell3", "cell4");
                row.Height = 20;
                row.Cells[0].Value = p.Number.ToString();
                row.Cells[1].Value = p.StartTime.ToString();
                row.Cells[2].Value = p.EndTime.ToString();
                row.Cells[3].Value = p.Duration.ToShortDisplayString();
                row.Cells[4].Value = textPreview.Replace(Environment.NewLine, PSF.LineSeparator);
                pList.Add(row);
            }
            CustomDataGridView1.Rows.AddRange(pList.ToArray());
            CustomDataGridView1.AutoSizeLastColumn();

            HighlightTimingError();

            dgv.FirstDisplayedScrollingRowIndex = SetFirstDisplayedRowindex(firstVisible);
            dgv.Rows[SetSelectedRow(selectRow)].Selected = true;

            TotalRows = SubCurrent.Paragraphs.Count;
            ToolStripLabelRight.Text = CustomDataGridView1.SelectedRows.Count + "/" + TotalRows.ToString();
            Debug.WriteLine("Subtitle Contains Persian Chars: " + SubContainsPersianChars);

            DetectChanges();

            //// Sort Column Line#
            //ListView1.ListViewItemSorter = lvwColumnSorter;
            //lvwColumnSorter.SortColumn = 0;
            //ListView1.Sorting = SortOrder.Ascending;
            //ListView1.AutoArrange = true;
            //lvwColumnSorter._SortModifier = ListViewColumnSorter.SortModifiers.SortByText;
        }

        private void HighlightTimingError()
        {
            var dgv = CustomDataGridView1;
            if (dgv.RowCount == 0) return;

            dgv.ShowCellToolTips = true;
            for (int a = 0; a < dgv.RowCount; a++)
            {
                var row = dgv.Rows[a];
                for (int b = 0; b < row.Cells.Count; b++)
                {
                    var cell = row.Cells[b];
                    cell.Style.BackColor = Color.Empty;
                    cell.Style.SelectionBackColor = Color.Empty;
                    cell.ToolTipText = null;
                }
            }

            double minDL = (double)CustomNumericUpDownMinDL.Value;
            double maxDL = (double)CustomNumericUpDownMaxDL.Value;

            static void ChangeCellStyle(DataGridViewCell cell, Color color, string toolTipText)
            {
                cell.Style.BackColor = color;
                cell.Style.SelectionBackColor = color.ChangeBrightness(-0.3f);
                cell.ToolTipText = toolTipText;
            }

            for (int pn = 0; pn < SubCurrent.Paragraphs.Count; pn++)
            {
                Paragraph p = SubCurrent.Paragraphs[pn];
                double duration = p.Duration.TotalMilliseconds;
                var row = dgv.Rows[p.Number - 1];
                var lineNumberCell = dgv.Rows[pn].Cells[0];
                var startTimeCell = dgv.Rows[pn].Cells[1];
                var endTimeCell = dgv.Rows[pn].Cells[2];
                var durationCell = row.Cells[3];
                var textCell = dgv.Rows[pn].Cells[4];

                // Highlight Incorrect Time Order
                if (pn < SubCurrent.Paragraphs.Count - 1)
                {
                    string incorrectTimeOrderMSG = "Incorrect time order.";
                    Paragraph pNext = SubCurrent.Paragraphs[pn + 1];

                    if (pn == 0)
                    {
                        if (p.StartTime.TotalMilliseconds > pNext.StartTime.TotalMilliseconds)
                        {
                            ChangeCellStyle(lineNumberCell, Color.DarkRed, incorrectTimeOrderMSG);
                        }
                    }
                    else
                    {
                        Paragraph pPrev = SubCurrent.Paragraphs[pn - 1];
                        if (p.StartTime.TotalMilliseconds > pNext.StartTime.TotalMilliseconds ||
                            p.StartTime.TotalMilliseconds < pPrev.StartTime.TotalMilliseconds)
                        {
                            double diffWithPrev = 0;
                            if (p.StartTime.TotalMilliseconds > pPrev.StartTime.TotalMilliseconds)
                                diffWithPrev = p.StartTime.TotalMilliseconds - pPrev.StartTime.TotalMilliseconds;
                            if (pPrev.StartTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                                diffWithPrev = pPrev.StartTime.TotalMilliseconds - p.StartTime.TotalMilliseconds;

                            double diffWithNext = 0;
                            if (p.StartTime.TotalMilliseconds > pNext.StartTime.TotalMilliseconds)
                                diffWithNext = p.StartTime.TotalMilliseconds - pNext.StartTime.TotalMilliseconds;
                            if (pNext.StartTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                                diffWithNext = pNext.StartTime.TotalMilliseconds - p.StartTime.TotalMilliseconds;

                            double setDifference = 60000;
                            if (diffWithPrev > setDifference && diffWithNext <= setDifference)
                            {
                                var lineNumberCellPrev = dgv.Rows[pn - 1].Cells[0];
                                ChangeCellStyle(lineNumberCellPrev, Color.DarkRed, incorrectTimeOrderMSG);
                            }
                            else if (diffWithPrev <= setDifference && diffWithNext > setDifference)
                            {
                                var lineNumberCellNext = dgv.Rows[pn + 1].Cells[0];
                                ChangeCellStyle(lineNumberCellNext, Color.DarkRed, incorrectTimeOrderMSG);
                            }
                            else
                            {
                                if (p.StartTime.TotalMilliseconds > pNext.StartTime.TotalMilliseconds)
                                {
                                    ChangeCellStyle(lineNumberCell, Color.DarkRed, incorrectTimeOrderMSG);
                                }
                            }
                        }
                    }
                }

                // Highlight Adjust Duration
                // If Duration is Not Negative
                if (p.EndTime.TotalMilliseconds > p.StartTime.TotalMilliseconds)
                {
                    if (minDL > duration)
                    {
                        string msg = "Smaller than minimum duration limit.";
                        ChangeCellStyle(durationCell, Color.DarkRed, msg);
                    }
                    else if (duration > maxDL)
                    {
                        string msg = "Bigger than maximum duration limit.";
                        ChangeCellStyle(durationCell, Color.DarkRed, msg);
                    }
                }
                else
                {
                    string msg = "It's negative.";
                    ChangeCellStyle(durationCell, Color.DarkRed, msg);
                }

                // Highlight Overlapped Lines (Minimum Gap)
                if (pn < SubCurrent.Paragraphs.Count - 1)
                {
                    Paragraph pNext = SubCurrent.Paragraphs[pn + 1];
                    if (p.StartTime.TotalMilliseconds < pNext.StartTime.TotalMilliseconds)
                    {
                        if (p.EndTime.TotalMilliseconds > pNext.StartTime.TotalMilliseconds)
                        {
                            var nextStartTimeCell = dgv.Rows[pn + 1].Cells[1];
                            string msg = "Time overlapped.\r\nMight be fixed by applying minimum gap.";
                            ChangeCellStyle(endTimeCell, Color.DarkRed, msg);
                            ChangeCellStyle(nextStartTimeCell, Color.DarkRed, msg);
                        }
                    }
                }

                // Highlight Same Time Code
                if (pn < SubCurrent.Paragraphs.Count - 1)
                {
                    double difference = (double)CustomNumericUpDownSameTimeCode.Value;
                    Paragraph pNext = SubCurrent.Paragraphs[pn + 1];
                    if (p.StartTime.TotalMilliseconds <= pNext.StartTime.TotalMilliseconds)
                    {
                        if (p.StartTime.TotalMilliseconds + difference >= pNext.StartTime.TotalMilliseconds)
                        {
                            string msg = "Same time code with next line.";
                            ChangeCellStyle(startTimeCell, Color.DarkRed, msg);
                        }
                    }
                }

                // Highlight Empty Lines
                if (p.Text.IsEmpty())
                {
                    string msg = "Line is empty.";
                    ChangeCellStyle(textCell, Color.DarkRed, msg);
                }

                // Highlight Same Text
                if (pn < SubCurrent.Paragraphs.Count - 1)
                {
                    double difference = (double)CustomNumericUpDownSameText.Value;
                    Paragraph pNext = SubCurrent.Paragraphs[pn + 1];
                    if (p.StartTime.TotalMilliseconds <= pNext.StartTime.TotalMilliseconds)
                    {
                        if (p.EndTime.TotalMilliseconds + difference >= pNext.StartTime.TotalMilliseconds)
                        {
                            if (!p.Text.IsEmpty() && !pNext.Text.IsEmpty())
                            {
                                string text1 = Tools.HTML.RemoveHtmlTags(p.Text);
                                text1 = text1.IsNotNull().RemoveControlChars().Trim();
                                string text2 = Tools.HTML.RemoveHtmlTags(pNext.Text);
                                text2 = text2.IsNotNull().RemoveControlChars().Trim();
                                if (text1 == text2)
                                {
                                    string msgNL = "Same text with next line.";
                                    ChangeCellStyle(textCell, Color.DarkRed, msgNL);

                                    var textCellNext = dgv.Rows[pn + 1].Cells[4];
                                    string msgPL = "Same text with previous line.";
                                    ChangeCellStyle(textCellNext, Color.DarkRed, msgPL);
                                }
                            }
                        }
                    }
                }

            }
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
                    Debug.WriteLine("TaskApply: " + TaskApply.Status);
                }, TaskScheduler.FromCurrentSynchronizationContext());

                DetectChanges(2);
                var t = new System.Windows.Forms.Timer();
                t.Interval = 100;
                t.Tick += (s, e) =>
                {
                    if (TaskApply.IsCompleted)
                    {
                        DetectChanges();
                        // Auto Sort Column Line#
                        //if (lvwColumnSorter != null)
                        //{
                        //    lvwColumnSorter.SortColumn = 0;
                        //    lvwColumnSorter.Order = SortOrder.Ascending;
                        //    ListView1.Sort();
                        //}
                        t.Stop();
                    }
                };
                t.Start();
            }
            catch (OperationCanceledException ex)
            {
                Debug.WriteLine("Error: " + ex.Message);
                using (new Tools.CenterWinDialog(this))
                    CustomMessageBox.Show("Error: " + ex.Message, "Message", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public void FindAndListFixes(CancellationToken TokenApply)
        {
            DateTime startTime = DateTime.Now;
            CustomStatusStrip1.InvokeIt(() => CustomProgressBar1.Value = 0);
            CustomStatusStrip1.InvokeIt(() => CustomProgressBar1.Minimum = 0);
            CustomStatusStrip1.InvokeIt(() => CustomProgressBar1.Maximum = 100);
            CustomStatusStrip1.InvokeIt(() => CustomProgressBar1.Step = 1);
            TotalFixes = 0;
            //========== Creating List ============================================
            List<Tuple<string, string, string>> ReplaceExpressionList = new();
            ReplaceExpressionList.Clear();

            //var fileContent = Tools.Resource.GetResourceTextFile(PSF.ResourcePath); // Load from Embedded Resource
            XDocument doc = XDocument.Load(PSF.ReplaceListPath, LoadOptions.None);

            if (File.Exists(PSF.UserReplaceListPath))
                doc.Root.Element("MultipleSearchAndReplaceList").Add(XDocument.Load(PSF.UserReplaceListPath, LoadOptions.None).Root.Elements().Elements());

            var groups = doc.Root.Elements().Elements();

            for (int n1 = 0; n1 < TabPageCommonErrors.Controls.Count; n1++)
            {
                if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                    return;
                var cc = TabPageCommonErrors.Controls[n1];
                if ((cc is CustomCheckBox box) && box.Checked)
                {
                    for (int a = 0; a < groups.Count(); a++)
                    {
                        var group = groups.ToList()[a];
                        string groupName = group.Element("Name").Value;

                        if (groupName == box.Text)
                        {
                            var rules = group.Elements("MultipleSearchAndReplaceItem");
                            for (int b = 0; b < rules.Count(); b++)
                            {
                                var rule = rules.ToList()[b];
                                bool ruleEnabled = Convert.ToBoolean(rule.Element("Enabled").Value);
                                if (ruleEnabled)
                                {
                                    string findWhat = rule.Element("FindWhat").Value;
                                    string replaceWith = rule.Element("ReplaceWith").Value;
                                    string searchType = rule.Element("SearchType").Value;

                                    findWhat = MultipleReplace.FindWhatRule(findWhat);
                                    replaceWith = MultipleReplace.ReplaceWithRule(replaceWith);

                                    if (searchType == "RegularExpression" && !CompiledRegExList.ContainsKey(findWhat))
                                    {
                                        CompiledRegExList.Add(findWhat, new Regex(findWhat, RegexOptions.Compiled | RegexOptions.Multiline,
                                            TimeSpan.FromMilliseconds(2000)));
                                    }

                                    ReplaceExpressionList.Add(new Tuple<string, string, string>(findWhat, replaceWith, searchType));
                                }
                            }
                        }
                    }
                }
            }



            //========== Replacing List ===========================================
            List<DataGridViewRow> fixes = new();
            fixes.Clear();

            // Set a timeout interval of 2 seconds.
            AppDomain domain = AppDomain.CurrentDomain;
            domain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(2000));

            //Parallel.ForEach(_subtitle.Paragraphs, p =>
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

                for (int i = 0; i < ReplaceExpressionList.Count; i++)
                {
                    var list = ReplaceExpressionList[i];
                    string findWhat = list.Item1;
                    string replaceWith = list.Item2;
                    string searchType = list.Item3;

                    if (searchType == MultipleReplace.SearchType.CaseSensitive)
                    {
                        if (After.Contains(findWhat))
                            After = After.Replace(findWhat, replaceWith);
                    }
                    else if (searchType == MultipleReplace.SearchType.RegularExpression)
                    {
                        Regex regExFindWhat = CompiledRegExList[findWhat];

                        try
                        {
                            if (regExFindWhat.IsMatch(After))
                            {
                                After = regExFindWhat.Replace(After, replaceWith);
                            }
                        }
                        catch (RegexMatchTimeoutException ex)
                        {
                            Debug.WriteLine("Regex timed out!");
                            Debug.WriteLine("- Timeout interval specified: " + ex.MatchTimeout.TotalMilliseconds);
                            Debug.WriteLine("- Pattern: " + ex.Pattern);
                            Debug.WriteLine("- Input: " + ex.Input);
                        }

                    }
                    else if (searchType == MultipleReplace.SearchType.Normal)
                    {
                        After = After.Replace(findWhat, replaceWith, StringComparison.OrdinalIgnoreCase);
                    }
                }

                After = After.Trim();
                if (After != Before)
                {
                    //Before = Tools.HTML.RemoveHtmlTags(Before);
                    //After = Tools.HTML.RemoveHtmlTags(After);
                    if (TokenApply.IsCancellationRequested == true || CloseForm == true)
                        return;

                    CustomDataGridView1.InvokeIt(() =>
                    {
                        // Add one by one
                        //DataGridViewRow row = CustomDataGridView1.Rows[CustomDataGridView1.Rows.Add()];

                        // Add by AddRange
                        DataGridViewRow row = new();
                        row.CreateCells(CustomDataGridView1, "cell0", "cell1", "cell2", "cell3");
                        row.Height = 20;
                        row.Cells[0].Value = true;
                        CustomDataGridView1.EndEdit();
                        row.Cells[1].Value = p.Number.ToString();
                        row.Cells[2].Value = Before.Replace(Environment.NewLine, PSF.LineSeparator);
                        row.Cells[3].Value = After.Replace(Environment.NewLine, PSF.LineSeparator);
                        fixes.Add(row);
                    });

                    TotalFixes++;
                    ShowStatus(string.Format("Total fixes: {0}", TotalFixes), ForeColor, 0);
                    CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = TotalFixes <= 0 ? ControlPaint.Light(ForeColor) : ForeColor);
                }
            }
            CustomDataGridView1.InvokeIt(() => CustomDataGridView1.Rows.AddRange(fixes.ToArray()));
            //=============================================================================
            CustomProgressBar1.InvokeIt(() => CustomProgressBar1.CustomText = string.Empty);
            if (TotalFixes > 0)
                ShowStatus(string.Format("Total fixes: {0}", TotalFixes), ForeColor, 0);
            CustomStatusStrip1.InvokeIt(() => ToolStripLabelLeft.ForeColor = TotalFixes <= 0 ? ControlPaint.Light(ForeColor) : ForeColor);
        }

        private void ApplyPreview()
        {
            if (SubLoaded())
            {
                if (ApplyState())
                {
                    ShowStatus("Please wait...", ForeColor, 0);
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
                                    p.Text = p.Text.Replace(PSF.LineSeparator, Environment.NewLine);
                                }
                                else if ((bool)row.Cells[0].Value == true)
                                {
                                    count++;
                                    p.Text = row.Cells[3].Value.ToString();
                                    p.Text = p.Text.Replace(PSF.LineSeparator, Environment.NewLine);
                                }
                            }
                        }
                    }
                    //----------------------------------------------------------------
                    if (count > 0)
                    {
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, "Common errors fixed: " + count);
                        Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
                    }
                }
            }
        }

        private void Apply()
        {
            if (SubPath != null)
                if (SubExist(SubPath))
                {
                    var dgv = CustomDataGridView1;

                    PSF.LoadColumnsApplyState(dgv);

                    dgv.Rows.Clear();
                    dgv.Columns[dgv.Columns.Count - 2].Width = 400;
                    dgv.AutoSizeLastColumn();

                    PleaseWait();
                }
                else
                {
                    ShowStatus(MsgOpenOrDrag.IsNotNull(), ForeColor, 0);
                    return;
                }
        }

        private void ApplyToTemp()
        {
            var t = new System.Windows.Forms.Timer();
            t.Interval = 100;
            t.Tick += (s, e) =>
            {
                if (TaskApply.IsCompleted)
                {
                    if (TotalFixes == 0)
                    {
                        LoadSubtitle(SubEncoding, 0, 0);
                        ShowStatus("There is nothing to fix.", ForeColor, 7);
                    }

                    ToolStripLabelRight.Text = GetCurrentRow() + "/" + TotalRows.ToString();
                    t.Stop();
                }
                CustomDataGridView1.AutoSizeLastColumn();
            };
            t.Start();
        }

        private void CustomButtonApply_Click(object sender, EventArgs e)
        {
            if (!SubLoaded()) return;
            if (IsApplyTaskRunning()) return;
            CustomButtonApply.Enabled = false;
            CloseForm = false;
            ApplyPreview();
            Apply();
            CustomDataGridView1.AutoSizeLastColumn();
            ApplyToTemp();
        }

        private void CustomButtonStop_Click(object sender, EventArgs e)
        {
            if (IsApplyTaskRunning())
            {
                CustomButtonStop.Enabled = false;
                SourceApply.Cancel();
                CloseForm = true;
                TaskApply.ContinueWith(t =>
                {
                    this.InvokeIt(() =>
                    {
                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        TaskScheduler.FromCurrentSynchronizationContext();
                    });
                });
            }
        }

        private void CustomButtonCancel_Click(object sender, EventArgs e)
        {
            if (!SubLoaded()) return;
            LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
        }

        private async Task<string?> SaveSubtitleToFileAsync(string savePath, string saveAction)
        {
            string successConsole = "File Saved: ";

            int codePage = SubOriginalEncoding.CodePage;
            byte[] sourceByte = Encoding.GetEncoding(codePage).GetBytes(SubCurrent.ToText(SubFormat));
            byte[] outputByte = Encoding.Convert(Encoding.GetEncoding(codePage), SubEncoding.IsNotNull(), sourceByte);
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
                                return null;
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
            Debug.WriteLine(successConsole + newPath);
            UpdateSub(newPath);
            SubTemp = new(SubCurrent);
            PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, successConsole + newPath);
            Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
            return newPath;
        }

        private async Task Save(string SavePath, string saveAction)
        {
            if (SubLoaded() == true && SubTempPath != null)
            {
                if (ApplyState() == true)
                {
                    ApplyPreview();
                }
                //-------------------------------------------------------------------
                SavePath = await SaveSubtitleToFileAsync(SavePath, saveAction);
                if (SavePath == null)
                    return;
                //-------------------------------------------------------------------
                if (File.Exists(SubTempPath) == true)
                    File.Delete(SubTempPath);
                // Reset ComboBox Encoding
                EncodingTool.InitializeTextEncoding(CustomToolStripComboBoxEncoding.ComboBox);
                UpdateSub(SavePath);
                UpdateSubOriginal(SavePath);
                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                if (saveAction == "Save")
                    ShowStatus("File saved: " + SubName, ForeColor, 5);
                if (saveAction == "SaveAs")
                    ShowStatus("File saved as: " + SubName, ForeColor, 5);
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
                if (SavePath != null)
                    await Save(SavePath, "SaveAs");
            }
        }

        private void ToolStripButtonUndo_Click(object sender, EventArgs e)
        {
            if (PSFUndoRedo.CurrentIndex > 0)
            {
                string msg = "Undo: Before " + PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item4;
                PSFUndoRedo.CurrentIndex--;
                SubCurrent = new(PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item1);
                SubEncodingDisplayName = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item2;
                SubEncoding = EncodingTool.GetEncoding(SubEncodingDisplayName);
                SubFormat = PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item3;
                UndoRedoClicked = true;
                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 7);
            }
            Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
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
                string msg = "Redo: " + PSFUndoRedo.UndoRedo(PSFUndoRedo.CurrentIndex).Item4;
                UndoRedoClicked = true;
                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 7);
            }
            Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
        }

        private void ShowPopupGuideToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ShowPopupGuideToolStripMenuItem.Checked = !ShowPopupGuideToolStripMenuItem.Checked;

            // Save
            PSFSettings.Save(PSFSettings.SettingsName.General, ShowPopupGuideToolStripMenuItem);
        }

        private void CustomToolStripComboBoxFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SubLoaded() == true && CustomToolStripComboBoxFormat.SelectedItem != null && UndoRedoClicked == false)
            {
                var newFormat = PSF.SubFormat.GetFormat(CustomToolStripComboBoxFormat.SelectedItem.ToString().IsNotNull());
                if (SubFormat == newFormat)
                {
                    string info = "No Need To Change Format.";
                    Debug.WriteLine(info);
                    ShowStatus(info, ForeColor, 5);
                }
                else
                {
                    string msg = "Subtitle Format Changed To: " + newFormat.FriendlyName;
                    Debug.WriteLine(msg);
                    ShowStatus(msg, ForeColor, 5);
                    SubFormat = newFormat;
                    LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                    SubTemp = new(SubCurrent);
                    PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, newFormat, msg);
                    Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
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
                Debug.WriteLine("Subtitle Encoding From ComboBox: " + subEncodingFromComboBox.WebName);
                if (SubEncodingDisplayName == dstDisplayName)
                {
                    string info = "No Need To Convert.";
                    Debug.WriteLine(info);
                    ShowStatus(info, ForeColor, 5);
                }
                else
                {
                    string msg = "Converted to: " + dstDisplayName;
                    ShowStatus(msg, ForeColor, 5);
                    SubEncodingDisplayName = dstDisplayName;
                    SubEncoding = subEncodingFromComboBox;
                    LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                    SubTemp = new(SubCurrent);
                    PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);
                    Debug.WriteLine("Current UndoRedo Index: " + PSFUndoRedo.CurrentIndex);
                }
            }
            UndoRedoClicked = false;
        }
        //public static event FormClosedEventHandler? MultipleReplaceFormClosed;
        private void ToolStripButtonEdit_Click(object sender, EventArgs e)
        {
            Form multipleReplace = new MultipleReplace(PSF.UserReplaceListPath);
            multipleReplace.StartPosition = FormStartPosition.CenterParent;
            multipleReplace.Text = "Multiple Replace Edit - User";
            multipleReplace.FormClosed -= MultipleReplace_FormClosed;
            multipleReplace.FormClosed += MultipleReplace_FormClosed;
            void MultipleReplace_FormClosed(object? sender, FormClosedEventArgs e)
            {
                LoadCheckBoxes();
                CompiledRegExList.Clear();
                ChangesNotUnicode();
            }
            multipleReplace.ShowDialog(this);
        }

        private void ToolStripButtonEditAdmin_Click(object? sender, EventArgs? e)
        {
            Form multipleReplace = new MultipleReplace(PSF.ReplaceListPath);
            multipleReplace.StartPosition = FormStartPosition.CenterParent;
            multipleReplace.Text = "Multiple Replace Edit - Admin";
            multipleReplace.FormClosed -= MultipleReplace_FormClosed;
            multipleReplace.FormClosed += MultipleReplace_FormClosed;
            void MultipleReplace_FormClosed(object? sender, FormClosedEventArgs e)
            {
                LoadCheckBoxes();
                CompiledRegExList.Clear();
                ChangesNotUnicode();
            }
            multipleReplace.ShowDialog(this);
        }

        private void ToolStripButtonSettings_Click(object sender, EventArgs e)
        {
            Form settings = new Settings();
            settings.StartPosition = FormStartPosition.CenterParent;
            settings.ShowDialog(this);
        }

        private void ToolStripButtonAbout_Click(object sender, EventArgs e)
        {
            Form about = new About();
            about.StartPosition = FormStartPosition.CenterParent;
            about.ShowDialog(this);
        }

        private void CheckboxHandler(object? sender, EventArgs? e)
        {
            // Search and find Checkboxes
            int n = 0;
            for (int a = 0; a < TabPageCommonErrors.Controls.Count; a++)
            {
                Control c = TabPageCommonErrors.Controls[a];

                if (c is CustomCheckBox box)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.CheckBoxes, box);
                    if (box.Checked == true)
                        n++;
                }
            }
            if (n > 0 && SubExist(SubPath))
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
                for (int n = 0; n < CustomDataGridView1.Rows.Count; n++)
                {
                    var row = CustomDataGridView1.Rows[n];
                    row.Cells[0].Value = true;
                }
            }
            else
            {
                for (int n = 0; n < CustomDataGridView1.Rows.Count; n++)
                {
                    var row = CustomDataGridView1.Rows[n];
                    row.Cells[0].Value = !(bool)row.Cells[0].Value;
                }
            }
        }

        private void CustomDataGridView1_SelectionChanged(object sender, EventArgs e)
        {
            var dgv = CustomDataGridView1;
            if (dgv.SelectedRows.Count > 0)
            {
                var t = new System.Windows.Forms.Timer();
                t.Interval = 10;
                t.Tick += (s, e) =>
                {
                    ToolStripLabelRight.Text = GetCurrentRow() + "/" + TotalRows.ToString();
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

        //================================================== Timing, Others ==========================================
        private void Fixes_ValueChanged(object sender, EventArgs e)
        {
            if (sender is CustomCheckBox box)
            {
                if (box.Name == CustomCheckBoxAdjustDurations.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, box);
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, CustomRadioButtonCharsPerSec);
                    CustomRadioButtonCharsPerSec.Enabled = CustomCheckBoxAdjustDurations.Checked;
                    CustomNumericUpDownCharsPerSec.Enabled = CustomCheckBoxAdjustDurations.Checked;
                }
                else if (box.Name == CustomCheckBoxMergeSameTime.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, box);
                    CustomNumericUpDownSameTimeCode.Enabled = CustomCheckBoxMergeSameTime.Checked;
                }
                else if (box.Name == CustomCheckBoxFixNegative.Name ||
                         box.Name == CustomCheckBoxFixIncorrectTimeOrder.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, box);
                }
                else if (box.Name == CustomCheckBoxRemoveEmptyLines.Name ||
                         box.Name == CustomCheckBoxRemoveUCChars.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Others, box);
                }
                else if (box.Name == CustomCheckBoxMergeSameText.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Others, box);
                    CustomNumericUpDownSameText.Enabled = CustomCheckBoxMergeSameText.Checked;
                }
            }
            else if (sender is CustomComboBox cb)
            {
                if (cb.Name == CustomComboBoxFromFrameRate.Name ||
                    cb.Name == CustomComboBoxToFrameRate.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, cb);
                }
            }
            else if (sender is CustomNumericUpDown cn)
            {
                if (cn.Name == CustomNumericUpDownCharsPerSec.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, cn);
                }
                else if (cn.Name == CustomNumericUpDownMinDL.Name ||
                        cn.Name == CustomNumericUpDownMaxDL.Name ||
                        cn.Name == CustomNumericUpDownMinGap.Name ||
                        cn.Name == CustomNumericUpDownSameTimeCode.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, cn);
                    if (SubLoaded())
                        HighlightTimingError();
                }
                else if (cn.Name == CustomNumericUpDownSameText.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Others, cn);
                    if (SubLoaded())
                        HighlightTimingError();
                }
                else if (cn.Name == CustomNumericUpDownChangeSpeed.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, cn);
                }
            }
            else if (sender is CustomRadioButton rb)
            {
                if (rb.Name == CustomRadioButtonCharsPerSec.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, rb);
                }
                else if (rb.Name == CustomRadioButtonChangeSpeedC.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, rb);
                    CustomNumericUpDownChangeSpeed.Enabled = true;
                }
                else if (rb.Name == CustomRadioButtonChangeSpeedFDF.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, rb);
                    CustomNumericUpDownChangeSpeed.Value = 99.98887M;
                    CustomNumericUpDownChangeSpeed.Enabled = false;
                }
                else if (rb.Name == CustomRadioButtonChangeSpeedTDF.Name)
                {
                    PSFSettings.Save(PSFSettings.SettingsName.Timing, rb);
                    CustomNumericUpDownChangeSpeed.Value = 100.10010M;
                    CustomNumericUpDownChangeSpeed.Enabled = false;
                }
            }
            else if (sender is CustomTimeUpDown ct)
            {
                PSFSettings.Save(PSFSettings.SettingsName.Timing, ct);
            }
        }

        private void CustomButtonApplyTiming_Click(object sender, EventArgs e)
        {
            if (CustomCheckBoxFixNegative.Checked)
                FixTiming.FixNegative();

            if (CustomCheckBoxFixIncorrectTimeOrder.Checked)
                FixTiming.FixIncorrectTimeOrder();

            if (CustomCheckBoxAdjustDurations.Checked)
                FixTiming.AdjustDurations((double)CustomNumericUpDownCharsPerSec.Value);

            FixTiming.FixMinimumDurationLimit((double)CustomNumericUpDownMinDL.Value);
            FixTiming.FixMaximumDurationLimit((double)CustomNumericUpDownMaxDL.Value);
            FixTiming.ApplyMinimumGap((double)CustomNumericUpDownMinGap.Value);

            if (CustomCheckBoxMergeSameTime.Checked)
                FixTiming.MergeLinesWithSameTimeCode((double)CustomNumericUpDownSameTimeCode.Value);

            if (SubContentChanged())
            {
                string msg = "Timing correction applied.";
                SubTemp = new(SubCurrent);
                PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 5);
            }
            else
                ShowStatus("There are no timing issues or cannot be fix.", ForeColor, 7);
        }

        private void CustomButtonResetTiming_Click(object sender, EventArgs e)
        {
            CustomCheckBoxFixNegative.Checked = false;
            CustomCheckBoxFixIncorrectTimeOrder.Checked = false;

            CustomCheckBoxAdjustDurations.Checked = false;
            CustomRadioButtonCharsPerSec.Checked = true;
            CustomNumericUpDownCharsPerSec.Value = (decimal)6.5M;

            CustomNumericUpDownMinDL.Value = 1000;
            CustomNumericUpDownMaxDL.Value = 8000;
            CustomNumericUpDownMinGap.Value = 42;

            CustomCheckBoxMergeSameTime.Checked = true;
            CustomNumericUpDownSameTimeCode.Value = 250;
        }

        private void CustomButtonSyncEarlier_Click(object sender, EventArgs e)
        {
            decimal value = CustomTimeUpDownAdjustTime.Value;
            if (SubLoaded() && value != 0)
            {
                FixTiming.ShowEarlier((double)value);

                string msg = "Went backward for " + value.ToString() + " miliseconds.";
                SubTemp = new(SubCurrent);
                PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 5);
            }
        }

        private void CustomButtonSyncLater_Click(object sender, EventArgs e)
        {
            decimal value = CustomTimeUpDownAdjustTime.Value;
            if (SubLoaded() && value != 0)
            {
                FixTiming.ShowLater((double)value);

                string msg = "Went forward for " + value.ToString() + " miliseconds.";
                SubTemp = new(SubCurrent);
                PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 5);
            }
        }

        private void CustomButtonChangeFrameRate_Click(object sender, EventArgs e)
        {
            if (CustomComboBoxFromFrameRate.SelectedItem != null || CustomComboBoxToFrameRate.SelectedItem != null)
            {
                double fromFR = double.Parse(CustomComboBoxFromFrameRate.SelectedItem.ToString().IsNotNull());
                double toFR = double.Parse(CustomComboBoxToFrameRate.SelectedItem.ToString().IsNotNull());
                if (fromFR == toFR)
                {
                    string msg = "Frame rate is the same.";
                    CustomMessageBox.Show(msg, "Frame rate", MessageBoxButtons.OK);
                    return;
                }
                else
                {
                    FixTiming.ChangeFrameRate(fromFR, toFR);

                    if (SubContentChanged())
                    {
                        string msg = "Frame rate changed from " + fromFR.ToString() + " to " + toFR.ToString() + ".";
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        ShowStatus(msg, ForeColor, 7);
                    }
                    else
                        ShowStatus("No need to change.", ForeColor, 7);
                }
            }
        }

        private void CustomButtonChangeSpeed_Click(object sender, EventArgs e)
        {
            double percentage = double.Parse(CustomNumericUpDownChangeSpeed.Value.ToString());
            if (percentage == 100)
            {
                string msg = "100% is the same as original.";
                CustomMessageBox.Show(msg, "Change speed", MessageBoxButtons.OK);
            }
            else
            {
                FixTiming.ChangeSpeed(percentage);

                if (SubContentChanged())
                {
                    string msg = "Speed changed by " + percentage.ToString() + "%.";
                    SubTemp = new(SubCurrent);
                    PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                    LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                    ShowStatus(msg, ForeColor, 7);
                }
                else
                    ShowStatus("No need to change.", ForeColor, 7);
            }
        }

        private void CustomButtonApplyOthers_Click(object sender, EventArgs e)
        {
            if (CustomCheckBoxRemoveEmptyLines.Checked)
                FixOthers.RemoveEmptylines();

            if (CustomCheckBoxMergeSameText.Checked)
                FixOthers.MergeLinesWithSameText((double)CustomNumericUpDownSameText.Value);

            if (CustomCheckBoxRemoveUCChars.Checked)
                FixOthers.RemoveUnicodeControlChars();

            if (SubContentChanged())
            {
                string msg = "Fixes applied: Others.";
                SubTemp = new(SubCurrent);
                PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                ShowStatus(msg, ForeColor, 4);
            }
            else
                ShowStatus("No need to fix.", ForeColor, 7);
        }

        private void CustomButtonResetOthers_Click(object sender, EventArgs e)
        {
            CustomCheckBoxRemoveEmptyLines.Checked = true;
            CustomCheckBoxMergeSameText.Checked = false;
            CustomNumericUpDownSameText.Value = 250;
            CustomCheckBoxRemoveUCChars.Checked = false;
        }

        private void ToolsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!SubLoaded()) return;

            if (sender is ToolStripMenuItem mi)
            {
                if (mi.Name == FixNegativeTimingToolStripMenuItem.Name)
                {
                    FixTiming.FixNegative();
                    if (SubContentChanged())
                    {
                        string msg = "Negative timing fixed.";
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        ShowStatus(msg, ForeColor, 7);
                    }
                    else
                        ShowStatus("No need to fix.", ForeColor, 7);
                }
                else if (mi.Name == FixIncorrectTimeOrderToolStripMenuItem.Name)
                {
                    FixTiming.FixIncorrectTimeOrder();
                    if (SubContentChanged())
                    {
                        string msg = "Incorrect time order fixed.";
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        ShowStatus(msg, ForeColor, 7);
                    }
                    else
                        ShowStatus("No need to fix.", ForeColor, 7);
                }
                else if (mi.Name == RemoveEmptyLinesToolStripMenuItem.Name)
                {
                    FixOthers.RemoveEmptylines();
                    if (SubContentChanged())
                    {
                        string msg = "Empty lines removed.";
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        ShowStatus(msg, ForeColor, 7);
                    }
                    else
                        ShowStatus("There are no empty lines.", ForeColor, 7);
                }
                else if (mi.Name == RemoveUnicodeControlCharsToolStripMenuItem.Name)
                {
                    FixOthers.RemoveUnicodeControlChars();
                    if (SubContentChanged())
                    {
                        string msg = "Unicode control chars removed.";
                        SubTemp = new(SubCurrent);
                        PSFUndoRedo.UndoRedo(SubTemp, SubEncodingDisplayName, SubFormat, msg);

                        LoadSubtitle(SubEncoding, GetFirstDisplayedRowindex(), GetSelectedRow());
                        ShowStatus(msg, ForeColor, 7);
                    }
                    else
                        ShowStatus("There are no unicode control chars.", ForeColor, 7);
                }
            }

        }

        //================================================= Exit ==============================================
        private void ToolStripButtonExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private async void Form_Closing(object sender, FormClosingEventArgs ex)
        {
            if (ex.CloseReason == CloseReason.UserClosing || ex.CloseReason == CloseReason.WindowsShutDown)
            {
                ex.Cancel = true;

                await PSFSettings.SaveToFile();

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

        //=====================================================================================================
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
            // Form.KeyPreview Property must be set to True.
            // Set Shortcuts
            if (e.Control && e.Shift && e.KeyCode == Keys.M)
            {
                ToolStripButtonEditAdmin_Click(null, null);
            }
        }

        
    }
}