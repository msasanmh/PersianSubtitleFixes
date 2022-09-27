using CustomControls;
using MsmhTools;
using PSFTools;
using Nikse.SubtitleEdit.Core.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.XPath;

namespace PersianSubtitleFixes
{
    public partial class MultipleReplace : Form
    {
        private static Subtitle? subPreview;
        private static string? CurrentTheme;
        private static XDocument? ReplaceList;
        private static readonly List<string> ListGroupNames = new();

        // Save Paragraph Number and Applied Regexes
        private static Dictionary<int, List<Tuple<string, int>>> AppliedRules = new();

        // Context Menu Group
        private static CustomContextMenuStrip MG = new();
        private static ToolStripMenuItem MenuGroupNew = new();
        private static ToolStripMenuItem MenuGroupRename = new();
        private static ToolStripMenuItem MenuGroupRemove = new();
        private static ToolStripMenuItem MenuGroupMoveUp = new();
        private static ToolStripMenuItem MenuGroupMoveDown = new();
        private static ToolStripMenuItem MenuGroupMoveToTop = new();
        private static ToolStripMenuItem MenuGroupMoveToBottom = new();
        private static ToolStripMenuItem MenuGroupImport = new();
        private static ToolStripMenuItem MenuGroupExport = new();

        // Context Menu Rules
        private static CustomContextMenuStrip MR = new();
        private static ToolStripMenuItem MenuRuleRemove = new();
        private static ToolStripMenuItem MenuRuleRemoveAll = new();
        private static ToolStripMenuItem MenuRuleMoveSelectedRulesToGroup = new();
        private static ToolStripMenuItem MenuRuleMoveUp = new();
        private static ToolStripMenuItem MenuRuleMoveDown = new();
        private static ToolStripMenuItem MenuRuleMoveToTop = new();
        private static ToolStripMenuItem MenuRuleMoveToBottom = new();
        private static ToolStripMenuItem MenuRuleImport = new();
        private static ToolStripMenuItem MenuRuleExport = new();
        private static ToolStripMenuItem MenuRuleSelectAll = new();
        private static ToolStripMenuItem MenuRuleInvertSelection = new();

        public static string FindWhatRule(string findWhat)
        {
            findWhat = findWhat.Replace("\"", "\\\"");
            findWhat = findWhat.Replace("\\\\\"", "\\\"");
            findWhat = findWhat.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
            return findWhat;
        }

        public static string ReplaceWithRule(string replaceWith)
        {
            //if (replaceWith == "") replaceWith = " ";
            replaceWith = replaceWith.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);
            return replaceWith;
        }

        public MultipleReplace()
        {
            InitializeComponent();
            Size = new(1200, 700);
            StartPosition = FormStartPosition.CenterScreen;

            CurrentTheme = Theme.GetTheme();

            Text = "Multiple Replace Edit";

            if (FormMain.SubCurrent != null)
                subPreview = new(FormMain.SubCurrent);

            // Initialize Menu
            CreateMenuGroup();
            CreateMenuRule();

            // Load Theme
            Theme.LoadTheme(this, Controls);

            // Load XML Settings
            LoadXmlSettings();

            ReadGroups(null, false);
        }

        public static void LoadXmlSettings()
        {
            if (!File.Exists(PSF.ReplaceListPath))
            {
                File.Create(PSF.ReplaceListPath).Dispose();
                ReplaceList = CreateXmlSettings();
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
            else if (string.IsNullOrWhiteSpace(File.ReadAllText(PSF.ReplaceListPath)))
            {
                ReplaceList = CreateXmlSettings();
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
            else if (!Tools.Xml.IsValidXML(File.ReadAllText(PSF.ReplaceListPath)))
            {
                CustomMessageBox.Show("Settings file is not valid. Returned to default.", "Not Valid", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ReplaceList = CreateXmlSettings();
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
            ReplaceList = XDocument.Load(PSF.ReplaceListPath, LoadOptions.None);
        }

        private static XDocument CreateXmlSettings()
        {
            XDocument doc = new();
            XElement group = new("Settings");
            group.Add(new XElement("MultipleSearchAndReplaceList"));
            doc.Add(group);
            return doc;
        }

        private void ReadGroups(string? groupNameToSelect, bool showRow)
        {
            // Read Groups
            ListGroupNames.Clear();
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            int firstVisible = dgvG.FirstDisplayedScrollingRowIndex;

            if (dgvG.SelectedRows.Count > 0)
            {
                int selectedRow = dgvG.SelectedRows[0].Index;
                int displayedRowCount = dgvG.DisplayedRowCount(false);
                if (selectedRow - firstVisible == displayedRowCount)
                    firstVisible++;
            }

            dgvG.Rows.Clear();

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            if (!nodesGroups.Any())
            {
                dgvR.Rows.Clear();
                CustomGroupBoxRules.Text = "Rules";
                return;
            }

            dgvG.Rows.Add(nodesGroups.Count());
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var node = nodesGroups.ToList()[a];
                bool cell0Value = node.Element("Enabled") == null || Convert.ToBoolean(node.Element("Enabled").Value);
                dgvG.Rows[a].Cells[0].Value = cell0Value;
                dgvG.Rows[a].Cells[1].Value = node.Element("Name").Value;
                dgvG.Rows[a].Height = 20;
                dgvG.EndEdit();

                // Add Names to List
                ListGroupNames.Add(node.Element("Name").Value);
            }

            // Select Group
            int rowIndex = 0;

            if (groupNameToSelect == null)
            {
                groupNameToSelect = dgvG.Rows[0].Cells[1].Value.ToString();
            }

            for (int n = 0; n < dgvG.Rows.Count; n++)
            {
                var row = dgvG.Rows[n];
                if (row.Cells[1].Value.ToString().Equals(groupNameToSelect))
                {
                    rowIndex = row.Index;
                    break;
                }
            }

            if (rowIndex == 0 && showRow == false)
                ReadRules(groupNameToSelect.IsNotNull()); // Because SelectionChanged won't fire.

            if (showRow)
                ShowRow(dgvG, rowIndex, firstVisible);
            else
            {
                dgvG.Rows[rowIndex].Cells[0].Selected = true;
                dgvG.Rows[rowIndex].Selected = true;
            }
        }

        private XElement GetXmlGroupByName(XDocument xDoc, string groupName)
        {
            XElement group = new("Default");

            XDocument doc = xDoc;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    group = nodeG;
                    break;
                }
            }

            return group;
        }

        private void RenameGroup(XDocument xDoc, string oldName, string newName)
        {
            var nodesGroups = xDoc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == oldName)
                {
                    nodeG.Element("Name").Value = newName;
                }
            }
        }

        private void RemoveGroup(string groupName)
        {
            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    nodeG.Remove();
                    break;
                }
            }
            //// Or (For loop is faster than linq).
            //var group = from groupElement in nodes
            //            where groupElement.Element("Name").Value == groupName
            //            select groupElement;
            //foreach (var groupElement in group)
            //    groupElement.Remove();

            // Find Previous Group Name Otherwise Next
            int index = ListGroupNames.GetIndex(groupName);
            if (index == -1 || index == 0)
            {
                // Refresh Groups
                ReadGroups(null, true);
                return;
            }
            else
            {
                string previousGroup = ListGroupNames[index - 1];
                // Refresh Groups
                ReadGroups(previousGroup, true);
            }
        }

        private void ReadRules(string? groupName)
        {
            Debug.WriteLine("ReadRules Fired: " + groupName);
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (string.IsNullOrEmpty(groupName))
            {
                dgvR.Rows.Clear();
                return;
            }

            if (dgvG.Rows.Count == 0) return;

            List<DataGridViewRow> pList = new();

            XDocument doc = ReplaceList;
            var nodes = doc.Root.Elements().Elements();

            for (int a = 0; a < nodes.Count(); a++)
            {
                var nodeG = nodes.ToList()[a];
                if (groupName == nodeG.Element("Name").Value)
                {
                    int count = nodeG.Elements("MultipleSearchAndReplaceItem").Count();
                    dgvR.Rows.Clear();
                    //dgvR.Rows.Add(count); // Add one by one
                    for (int b = 0; b < count; b++)
                    {
                        var node = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[b];
                        
                        if (node.Element("Enabled") == null)
                        {
                            // Clear TextBoxes
                            CustomTextBoxFindWhat.Texts = string.Empty;
                            CustomTextBoxReplaceWith.Texts = string.Empty;
                            CustomComboBoxSearchType.SelectedItem = null;
                            CustomTextBoxDescription.Texts = string.Empty;
                            return;
                        }

                        // Add by AddRange
                        DataGridViewRow row = new();
                        row.CreateCells(dgvR, "cell0", "cell1", "cell2", "cell3", "cell4");
                        row.Height = 20;
                        row.Cells[0].Value = Convert.ToBoolean(node.Element("Enabled").Value);
                        row.Cells[1].Value = node.Element("FindWhat").Value;
                        row.Cells[2].Value = node.Element("ReplaceWith").Value;
                        string searchType;
                        if (node.Element("SearchType").Value == "Normal")
                            searchType = "Normal";
                        else if (node.Element("SearchType").Value == "CaseSensitive")
                            searchType = "Case Sensitive";
                        else if (node.Element("SearchType").Value == "RegularExpression")
                            searchType = "Regular Expression";
                        else
                            searchType = node.Element("SearchType").Value;
                        row.Cells[3].Value = searchType;
                        row.Cells[4].Value = node.Element("Description").Value;
                        pList.Add(row);

                        //// Add one by one (Slow)
                        //dgvR.Rows[b].Cells[0].Value = Convert.ToBoolean(node.Element("Enabled").Value);
                        //dgvR.Rows[b].Cells[1].Value = node.Element("FindWhat").Value;
                        //dgvR.Rows[b].Cells[2].Value = node.Element("ReplaceWith").Value;
                        //dgvR.Rows[b].Cells[3].Value = node.Element("SearchType").Value;
                        //dgvR.Rows[b].Cells[4].Value = node.Element("Description").Value;
                        //dgvR.Rows[b].Height = 20;
                        //dgvR.EndEdit();
                    }
                    dgvR.Rows.AddRange(pList.ToArray());
                }
            }
        }

        private void RemoveRule(string groupName)
        {
            var dgvR = CustomDataGridViewRules;

            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }
            Array.Sort(ruleRows);

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    int less = 0;
                    for (int b = 0; b < ruleRows.Length; b++)
                    {
                        int row = ruleRows[b];
                        if (b != 0)
                        {
                            less++;
                            row = ruleRows[b] - less;
                        }
                        var node = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[row];
                        node.Remove();
                        Debug.WriteLine("Rule Removed At " + row);
                    }
                    break;
                }
            }
        }

        private void MoveRulesToGroup(string groupToSend, int[] ruleRows, int rowToInsert, bool addBeforeSelf)
        {
            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();
            List<XElement> selectedRules = new();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupToSend)
                {
                    // Add Selected Rules To List
                    for (int b = 0; b < ruleRows.Length; b++)
                    {
                        int row = ruleRows[b];
                        var rule = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[row];
                        selectedRules.Add(rule);
                    }

                    // Remove Selected Rules
                    int less = 0;
                    for (int b = 0; b < ruleRows.Length; b++)
                    {
                        int row = ruleRows[b];
                        if (b != 0)
                        {
                            less++;
                            row = ruleRows[b] - less;
                        }
                        var node = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[row];
                        node.Remove();
                    }

                    // Copy Selected Rules
                    selectedRules.Reverse();
                    for (int b = 0; b < selectedRules.Count; b++)
                    {
                        var rule = selectedRules[b];
                        CopyRuleToGroup(groupToSend, rule, rowToInsert, addBeforeSelf);
                    }

                    break;
                }
            }
        }

        private void CopyRuleToGroup(string groupToSend, XElement ruleToCopy, int? rowToInsert, bool? addBeforeSelf)
        {
            var dgvG = CustomDataGridViewGroups;

            if (dgvG.RowCount == 0) return;
            
            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupToSend)
                {
                    if (rowToInsert == null)
                    {
                        nodeG.Add(ruleToCopy);
                    }
                    else
                    {
                        var rules = nodeG.Elements("MultipleSearchAndReplaceItem").ToList();
                        for (int b = 0; b < rules.Count; b++)
                        {
                            var rule = rules[b];
                            if (rowToInsert == b)
                            {
                                if (addBeforeSelf != null)
                                {
                                    if ((bool)addBeforeSelf)
                                        rule.AddBeforeSelf(ruleToCopy);
                                    else
                                        rule.AddAfterSelf(ruleToCopy);
                                    break;
                                }
                            }
                        }
                    }
                }
            }
        }

        private static void ShowRow(DataGridView dgv, int rowToSelect, int firstVisibleRow)
        {
            if (0 <= rowToSelect && rowToSelect < dgv.RowCount)
            {
                dgv.Rows[0].Cells[0].Selected = false;
                dgv.Rows[0].Selected = false;
                dgv.Rows[rowToSelect].Cells[0].Selected = true;
                dgv.Rows[rowToSelect].Selected = true;
                if (rowToSelect < firstVisibleRow || firstVisibleRow == -1)
                    firstVisibleRow = rowToSelect;
                dgv.FirstDisplayedScrollingRowIndex = firstVisibleRow;
            }
        }

        private static void ShowRow(DataGridView dgv, int[] rowsToSelect, int firstVisibleRow)
        {
            bool zero = false;
            for (int n = 0; n < rowsToSelect.Length; n++)
            {
                int rowToSelect = rowsToSelect[n];
                if (rowToSelect == -1) return;
                if (0 <= rowToSelect && rowToSelect < dgv.RowCount)
                {
                    if (rowToSelect != 0 && zero == false)
                    {
                        dgv.Rows[0].Cells[0].Selected = false;
                        dgv.Rows[0].Selected = false;
                    }
                    else
                    {
                        dgv.Rows[0].Cells[0].Selected = true;
                        dgv.Rows[0].Selected = true;
                        zero = true;
                    }
                    dgv.Rows[rowToSelect].Cells[0].Selected = true;
                    dgv.Rows[rowToSelect].Selected = true;
                }
                if (rowToSelect < firstVisibleRow || firstVisibleRow == -1)
                    firstVisibleRow = rowToSelect;
                dgv.FirstDisplayedScrollingRowIndex = firstVisibleRow;
            }
        }

        private XElement CreateRule()
        {
            string searchTypeString = CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull();
            if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Case Sensitive")
                searchTypeString = "CaseSensitive";
            else if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Regular Expression")
                searchTypeString = "RegularExpression";

            XElement rule = new("MultipleSearchAndReplaceItem");

            XElement enabled = new("Enabled");
            enabled.Value = "True";
            rule.Add(enabled);

            XElement findWhat = new("FindWhat");
            findWhat.Value = CustomTextBoxFindWhat.Texts;
            rule.Add(findWhat);

            XElement replaceWith = new("ReplaceWith");
            replaceWith.Value = CustomTextBoxReplaceWith.Texts;
            rule.Add(replaceWith);

            XElement searchType = new("SearchType");
            searchType.Value = searchTypeString;
            rule.Add(searchType);

            XElement description = new("Description");
            description.Value = CustomTextBoxDescription.Texts;
            rule.Add(description);

            return rule;
        }

        #region Groups
        //======================================= Groups ======================================
        private void CustomDataGridViewGroups_SelectionChanged(object sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.RowCount == 0) return;
            if (dgvG.SelectedCells.Count <= 0) return;

            int currentRow = dgvG.SelectedCells[0].RowIndex;

            if (dgvG.Rows[currentRow].Cells[1].Value == null) return;

            string group = dgvG.Rows[currentRow].Cells[1].Value.ToString();

            string ruleNoMsg = string.Empty;
            if (dgvR.RowCount > 0 && dgvR.SelectedCells.Count > 0)
            {
                int ruleNo = dgvR.SelectedCells[0].RowIndex + 1;
                ruleNoMsg = " - Rule No: " + ruleNo.ToString();
            }

            string msg = "Rules for group \"" + group + "\"" + ruleNoMsg;
            CustomGroupBoxRules.Text = msg;

            ReadRules(group.IsNotNull());
        }

        private void CustomDataGridViewGroups_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            if (dgvG.Rows.Count == 0) return;
            if (dgvG.SelectedCells.Count <= 0) return;
            if (dgvG.Rows[e.RowIndex].Cells[1].Value == null) return;

            string group = dgvG.Rows[e.RowIndex].Cells[1].Value.ToString();
            //Debug.WriteLine(group);

            // Save CheckBox State for Groups
            if (e.ColumnIndex == 0)
            {
                XDocument doc = ReplaceList;
                var nodesGroups = doc.Root.Elements().Elements();

                for (int a = 0; a < nodesGroups.Count(); a++)
                {
                    var nodeG = nodesGroups.ToList()[a];
                    if (group == nodeG.Element("Name").Value)
                    {
                        nodeG.Element("Enabled").Value = dgvG.Rows[a].Cells[0].Value.ToString().IsNotNull();

                        // Save xDocument to File
                        ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
                    }
                }
            }
        }

        private void CustomDataGridViewGroups_KeyDown(object sender, KeyEventArgs e)
        {
            // Assign Menu Shortcuts to KeyDown (Use Shortcuts When Menu is Not Displayed)
            void checkShortcut(ToolStripMenuItem item)
            {
                if (item.ShortcutKeys == e.KeyData)
                {
                    //item.PerformClick(); // Doesn't work correctly
                    if (item.Text.Contains("New group..."))
                        MenuGroupNew_Click(null, null);
                    else if (item.Text.Contains("Rename group..."))
                        MenuGroupRename_Click(null, null);
                    else if (item.Text.Contains("Remove"))
                        MenuGroupRemove_Click(null, null);
                    else if (item.Text.Contains("Move up"))
                        MenuGroupMoveUp_Click(null, null);
                    else if (item.Text.Contains("Move down"))
                        MenuGroupMoveDown_Click(null, null);
                    else if (item.Text.Contains("Move to top"))
                        MenuGroupMoveToTop_Click(null, null);
                    else if (item.Text.Contains("Move to bottom"))
                        MenuGroupMoveToBottom_Click(null, null);
                    else if (item.Text.Contains("Import"))
                        MenuGroupImport_Click(null, null);
                    else if (item.Text.Contains("Export"))
                        MenuGroupExport_Click(null, null);
                    else
                        item.PerformClick();
                    return;
                }

                foreach (ToolStripMenuItem child in item.DropDownItems.OfType<ToolStripMenuItem>())
                {
                    checkShortcut(child);
                }
            }

            foreach (ToolStripMenuItem item in MG.Items.OfType<ToolStripMenuItem>())
            {
                checkShortcut(item);
            }

            // Make ContextMenu Shortcuts Work (Move up and Move Down)
            if (e.Control && e.KeyCode == Keys.Up || e.Control && e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void CreateMenuGroup()
        {
            // Context Menu
            MG = new();

            MenuGroupNew = new();
            MenuGroupNew.Text = "New group...";
            MenuGroupNew.ShortcutKeys = Keys.Control | Keys.Shift | Keys.N;
            MenuGroupNew.Click += MenuGroupNew_Click;
            MG.Items.Add(MenuGroupNew);

            MenuGroupRename = new();
            MenuGroupRename.Text = "Rename group...";
            MenuGroupRename.ShortcutKeys = Keys.F2;
            MenuGroupRename.Click += MenuGroupRename_Click;
            MG.Items.Add(MenuGroupRename);

            MenuGroupRemove = new();
            MenuGroupRemove.Text = "Remove";
            MenuGroupRemove.ShortcutKeys = Keys.Delete;
            MenuGroupRemove.Click += MenuGroupRemove_Click;
            MG.Items.Add(MenuGroupRemove);

            MG.Items.Add("-");

            MenuGroupMoveUp = new();
            MenuGroupMoveUp.Text = "Move up";
            MenuGroupMoveUp.ShortcutKeys = Keys.Control | Keys.Up;
            MenuGroupMoveUp.Click += MenuGroupMoveUp_Click;
            MG.Items.Add(MenuGroupMoveUp);

            MenuGroupMoveDown = new();
            MenuGroupMoveDown.Text = "Move down";
            MenuGroupMoveDown.ShortcutKeys = Keys.Control | Keys.Down;
            MenuGroupMoveDown.Click += MenuGroupMoveDown_Click;
            MG.Items.Add(MenuGroupMoveDown);

            MenuGroupMoveToTop = new();
            MenuGroupMoveToTop.Text = "Move to top";
            MenuGroupMoveToTop.ShortcutKeys = Keys.Control | Keys.Home;
            MenuGroupMoveToTop.Click += MenuGroupMoveToTop_Click;
            MG.Items.Add(MenuGroupMoveToTop);

            MenuGroupMoveToBottom = new();
            MenuGroupMoveToBottom.Text = "Move to bottom";
            MenuGroupMoveToBottom.ShortcutKeys = Keys.Control | Keys.End;
            MenuGroupMoveToBottom.Click += MenuGroupMoveToBottom_Click;
            MG.Items.Add(MenuGroupMoveToBottom);

            MG.Items.Add("-");

            MenuGroupImport = new();
            MenuGroupImport.Text = "Import";
            MenuGroupImport.ShortcutKeys = Keys.Control | Keys.I;
            MenuGroupImport.Click += MenuGroupImport_Click;
            MG.Items.Add(MenuGroupImport);

            MenuGroupExport = new();
            MenuGroupExport.Text = "Export";
            MenuGroupExport.ShortcutKeys = Keys.Control | Keys.E;
            MenuGroupExport.Click += MenuGroupExport_Click;
            MG.Items.Add(MenuGroupExport);

            Theme.SetColors(MG);
        }

        private void CustomDataGridViewGroups_MouseDown(object? sender, MouseEventArgs? e)
        {
            // Context Menu
            if (e.Button == MouseButtons.Right)
            {
                var dgvG = CustomDataGridViewGroups;
                dgvG.Select(); // Set Focus on Control
                CreateMenuGroup();

                int currentMouseOverRow = dgvG.HitTest(e.X, e.Y).RowIndex;
                int totalRows = dgvG.Rows.Count;

                if (currentMouseOverRow != -1)
                {
                    dgvG.Rows[currentMouseOverRow].Cells[0].Selected = true;
                    dgvG.Rows[currentMouseOverRow].Selected = true;
                }

                if (currentMouseOverRow == -1)
                {
                    MG.Items.Remove(MenuGroupRename);
                    MG.Items.Remove(MenuGroupRemove);
                    MG.Items.Remove(MenuGroupMoveUp);
                    MG.Items.Remove(MenuGroupMoveDown);
                    MG.Items.Remove(MenuGroupMoveToTop);
                    MG.Items.Remove(MenuGroupMoveToBottom);
                    MG.Items.RemoveAt(2);
                }
                else if (currentMouseOverRow == 0)
                {
                    MG.Items.Remove(MenuGroupMoveUp);
                    MG.Items.Remove(MenuGroupMoveToTop);
                }
                else if (currentMouseOverRow == totalRows - 1)
                {
                    MG.Items.Remove(MenuGroupMoveDown);
                    MG.Items.Remove(MenuGroupMoveToBottom);
                }

                if (totalRows == 0)
                {
                    MG.Items.Remove(MenuGroupExport);
                }
                else if (totalRows == 1)
                {
                    MG.Items.Remove(MenuGroupMoveDown);
                    MG.Items.Remove(MenuGroupMoveToBottom);
                    if (currentMouseOverRow != -1)
                        MG.Items.RemoveAt(3);
                }

                DarkTheme.SetDarkTheme(MG);
                MG.Show(dgvG, new Point(e.X, e.Y));
            }
        }

        private void MenuGroupNew_Click(object? sender, EventArgs? e)
        {
            string newGroupName = string.Empty;
            switch (CustomInputBox.Show(ref newGroupName, "New Group Name:", false, "Create group"))
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    return;
                default:
                    return;
            }

            if (string.IsNullOrEmpty(newGroupName) || string.IsNullOrWhiteSpace(newGroupName))
            {
                string msg = "Name cannot be empty or white space.";
                CustomMessageBox.Show(msg, "Message");
                return;
            }

            if (ListGroupNames.Contains(newGroupName))
            {
                string msg = "\"" + newGroupName + "\"" + " is already exist, choose another name.";
                CustomMessageBox.Show(msg, "Message");
                return;
            }

            XDocument doc = ReplaceList;
            var nodes = doc.Root.Elements();
            for (int a = 0; a < nodes.Count(); a++)
            {
                var node = nodes.ToList()[a];

                XElement group = new("Group");
                group.Add(new XElement("Name", newGroupName));
                group.Add(new XElement("Enabled", "True"));
                //group.Add(new XElement("MultipleSearchAndReplaceItem"));
                //group.Element("MultipleSearchAndReplaceItem").Add(new XElement("Enabled", "True"));
                node.Add(group);
            }

            // Refresh Groups
            ReadGroups(newGroupName, false);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("New Group Created: " + newGroupName);
        }

        private void MenuGroupRename_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;
            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupNameOld = groupNameNull.IsNotNull();
            string groupNameNew = groupNameNull.IsNotNull();

            switch (CustomInputBox.Show(ref groupNameNew, "New Group Name:", false, "Rename group"))
            {
                case DialogResult.OK:
                    break;
                case DialogResult.Cancel:
                    return;
                default:
                    return;
            }

            if (string.IsNullOrEmpty(groupNameNew) || string.IsNullOrWhiteSpace(groupNameNew))
            {
                string msg = "Name cannot be empty or white space.";
                CustomMessageBox.Show(msg, "Message");
                return;
            }

            if (ListGroupNames.Contains(groupNameNew))
            {
                string msg = "\"" + groupNameNew + "\"" + " is already exist, choose another name.";
                CustomMessageBox.Show(msg, "Message");
                return;
            }

            XDocument doc = ReplaceList;

            RenameGroup(doc, groupNameOld, groupNameNew);

            // Refresh Groups
            ReadGroups(groupNameNew, false);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Renamed From \"" + groupNameOld + "\" To \"" + groupNameNew + "\"");
        }

        private void MenuGroupRemove_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;

            if (dgv.SelectedCells.Count < 1) return;

            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            RemoveGroup(groupName);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Removed: " + groupName);
        }

        private void MenuGroupMoveUp_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;

            if (dgv.SelectedCells.Count < 1) return;

            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            XDocument doc = ReplaceList;

            // Find Current Group
            var nodesGroups = doc.Root.Elements().Elements();
            XElement currentElement = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    currentElement = nodeG;
                    break;
                }
            }

            // Find Previous Group Name
            int index = ListGroupNames.FindIndex(a => a.Equals(groupName));
            if (index == -1 || index == 0)
            {
                return;
            }
            string previousGroup = ListGroupNames[index - 1];

            // Find Previous Location
            XElement newLocation = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == previousGroup)
                {
                    newLocation = nodeG;
                    break;
                }
            }

            if (currentElement.Name == "Default" || newLocation.Name == "Default") return;

            // Remove Current Group
            currentElement.Remove();

            // Copy to New Location
            newLocation.AddBeforeSelf(currentElement);

            // Refresh Groups
            ReadGroups(groupName, true);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Moved Up: " + groupName);
        }

        private void MenuGroupMoveDown_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;

            if (dgv.SelectedCells.Count < 1) return;

            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            XDocument doc = ReplaceList;

            // Find Current Group
            var nodesGroups = doc.Root.Elements().Elements();
            XElement currentElement = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    currentElement = nodeG;
                    break;
                }
            }

            // Find Next Group Name
            int index = ListGroupNames.FindIndex(a => a.Equals(groupName));
            if (index == -1 || index + 1 >= ListGroupNames.Count)
            {
                return;
            }
            string nextGroup = ListGroupNames[index + 1];

            // Find Previous Location
            XElement newLocation = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == nextGroup)
                {
                    newLocation = nodeG;
                    break;
                }
            }

            if (currentElement.Name == "Default" || newLocation.Name == "Default") return;

            // Remove Current Group
            currentElement.Remove();

            // Copy to New Location
            newLocation.AddAfterSelf(currentElement);

            // Refresh Groups
            ReadGroups(groupName, true);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Moved Down: " + groupName);
        }

        private void MenuGroupMoveToTop_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;

            if (dgv.SelectedCells.Count < 1) return;

            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            XDocument doc = ReplaceList;

            // Find Current Group
            var nodesGroups = doc.Root.Elements().Elements();
            XElement currentElement = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    currentElement = nodeG;
                    break;
                }
            }

            // Find Top Group Name
            int index = ListGroupNames.FindIndex(a => a.Equals(groupName));
            if (index == -1 || index == 0)
            {
                return;
            }
            string topGroup = ListGroupNames[0];

            // Find Top Location
            XElement newLocation = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == topGroup)
                {
                    newLocation = nodeG;
                    break;
                }
            }

            if (currentElement.Name == "Default" || newLocation.Name == "Default") return;

            // Remove Current Group
            currentElement.Remove();

            // Copy to New Location
            newLocation.AddBeforeSelf(currentElement);

            // Refresh Groups
            ReadGroups(groupName, false);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Moved To Top: " + groupName);
        }

        private void MenuGroupMoveToBottom_Click(object? sender, EventArgs? e)
        {
            var dgv = CustomDataGridViewGroups;

            if (dgv.SelectedCells.Count < 1) return;

            int currentRow = dgv.SelectedCells[0].RowIndex;
            string groupNameNull = dgv.Rows[currentRow].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            XDocument doc = ReplaceList;

            // Find Current Group
            var nodesGroups = doc.Root.Elements().Elements();
            XElement currentElement = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    currentElement = nodeG;
                    break;
                }
            }

            // Find Bottom Group Name
            int index = ListGroupNames.FindIndex(a => a.Equals(groupName));
            if (index == -1 || index + 1 >= ListGroupNames.Count)
            {
                return;
            }
            string bottomGroup = ListGroupNames[ListGroupNames.Count - 1];

            // Find Bottom Location
            XElement newLocation = new("Default");
            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == bottomGroup)
                {
                    newLocation = nodeG;
                    break;
                }
            }

            if (currentElement.Name == "Default" || newLocation.Name == "Default") return;

            // Remove Current Group
            currentElement.Remove();

            // Copy to New Location
            newLocation.AddAfterSelf(currentElement);

            // Refresh Groups
            ReadGroups(groupName, false);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Group Moved To Buttom: " + groupName);
        }

        private void MenuGroupImport_Click(object? sender, EventArgs? e)
        {
            using OpenFileDialog ofd = new();
            ofd.Filter = "Find and replace rules (*.template)|*.template|Find and replace rules (*.xml)|*.xml";
            ofd.DefaultExt = ".template";
            ofd.AddExtension = true;
            ofd.RestoreDirectory = true;
            //ofd.FileName = "multiple_replace_groups";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string filePath = ofd.FileName;

                XDocument importedReplaceList = new();
                importedReplaceList = XDocument.Load(filePath);

                Form Import = new()
                {
                    Size = new(300, 400),
                    Text = "Import...",
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.CenterParent
                };

                Label text = new()
                {
                    Text = "Choose groups to import",
                    AutoSize = true,
                    Location = new(5, 5)
                };
                Import.Controls.Add(text);
                int textHeight = text.GetPreferredSize(Size.Empty).Height;

                CustomButton buttonCancel = new()
                {
                    Text = "Cancel",
                    DialogResult = DialogResult.Cancel,
                };
                buttonCancel.Location = new(Import.ClientRectangle.Width - buttonCancel.Width - 5, Import.ClientRectangle.Height - buttonCancel.Height - 5);
                Import.Controls.Add(buttonCancel);

                CustomButton buttonOK = new()
                {
                    Text = "OK",
                    DialogResult = DialogResult.OK,
                };
                buttonOK.Location = new(Import.ClientRectangle.Width - buttonOK.Width - 5 - buttonCancel.Width - 5, Import.ClientRectangle.Height - buttonOK.Height - 5);
                Import.Controls.Add(buttonOK);

                CustomPanel panel = new()
                {
                    AutoScroll = true,
                    Border = BorderStyle.FixedSingle,
                    ButtonBorderStyle = ButtonBorderStyle.Solid,
                    Location = new(5, 10 + textHeight),
                    Width = Import.ClientRectangle.Width - 10,
                    Height = Import.ClientRectangle.Height - textHeight - buttonCancel.Height - 20
                };
                Import.Controls.Add(panel);

                var nodesGroups = importedReplaceList.Root.Elements().Elements();

                if (!nodesGroups.Any())
                    return;

                for (int a = 0; a < nodesGroups.Count(); a++)
                {
                    var nodeG = nodesGroups.ToList()[a];
                    string group = nodeG.Element("Name").Value;
                    //bool enabled = Convert.ToBoolean(nodeG.Element("Enabled").Value);

                    CustomCheckBox box = new();
                    box.Checked = true;
                    box.Text = group;
                    box.Location = new(5, 5 + (a * 20));
                    panel.Controls.Add(box);
                    box.MouseDown += (s, e) =>
                    {
                        if (e.Button == MouseButtons.Right)
                        {
                            ImportMenu(e.X + 5, box.Location.Y + box.GetPreferredSize(Size.Empty).Height / 3);
                        }
                    };
                }

                if (CurrentTheme == "Dark")
                    panel.SetDarkControl();

                buttonOK.Click += ButtonOK_Click;

                void ButtonOK_Click(object? sender, EventArgs e)
                {
                    XDocument doc = ReplaceList;
                    var insert = doc.Root.Element("MultipleSearchAndReplaceList");

                    string firstGroup = string.Empty;
                    bool once = true;
                    int count = 0;
                    int countName = 1;

                    for (int a = 0; a < panel.Controls.Count; a++)
                    {
                        Control c = panel.Controls[a];
                        foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>())
                        {
                            if (ch.Checked)
                            {
                                string group = ch.Text;

                                // Check for Duplicate Name
                                while (ListGroupNames.GetIndex(group) != -1)
                                {
                                    // Rename Variable
                                    string groupNew = string.Format("{0} ({1})", ch.Text, countName++);
                                    // Rename Group in Imported xDoc
                                    RenameGroup(importedReplaceList, group, groupNew);
                                    group = groupNew;
                                }

                                count++;
                                if (once)
                                {
                                    firstGroup = group;
                                    once = false;
                                }
                                insert.Add(GetXmlGroupByName(importedReplaceList, group));
                                countName = 1;
                            }
                        }
                    }

                    if (!doc.Root.Elements().Elements().Any()) return;
                    if (count == 0) return;

                    ReadGroups(firstGroup, false);

                    // Save xDocument to File
                    ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
                }

                // Context Menu Import (Selection)
                panel.MouseDown += Panel_MouseDown;
                void Panel_MouseDown(object? sender, MouseEventArgs e)
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ImportMenu(e.X, e.Y);
                    }
                }

                void ImportMenu(int eX, int eY)
                {
                    CustomContextMenuStrip MGE = new();

                    ToolStripMenuItem MenuSA = new();
                    MenuSA.Text = "Select All";
                    MenuSA.Click += MenuSA_Click;
                    MGE.Items.Add(MenuSA);

                    ToolStripMenuItem MenuIS = new();
                    MenuIS.Text = "Invert selection";
                    MenuIS.Click += MenuIS_Click;
                    MGE.Items.Add(MenuIS);

                    Theme.SetColors(MGE);
                    MGE.Show(panel, new Point(eX, eY));
                }

                void MenuSA_Click(object? sender, EventArgs e)
                {
                    for (int a = 0; a < panel.Controls.Count; a++)
                    {
                        Control c = panel.Controls[a];
                        foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>().Reverse())
                        {
                            ch.Checked = true;
                        }
                    }
                }

                void MenuIS_Click(object? sender, EventArgs e)
                {
                    for (int a = 0; a < panel.Controls.Count; a++)
                    {
                        Control c = panel.Controls[a];
                        foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>().Reverse())
                        {
                            if (ch.Checked)
                                ch.Checked = false;
                            else
                                ch.Checked = true;
                        }
                    }
                }

                Import.AcceptButton = buttonOK;
                Import.CancelButton = buttonCancel;

                Theme.LoadTheme(Import, Import.Controls);
                Import.ShowDialog(this);
            }
        }

        private void MenuGroupExport_Click(object? sender, EventArgs? e)
        {
            if (CustomDataGridViewGroups.Rows.Count == 0) return;

            Form Export = new()
            {
                Size = new(300, 400),
                Text = "Export...",
                FormBorderStyle = FormBorderStyle.FixedToolWindow,
                ShowInTaskbar = false,
                StartPosition = FormStartPosition.CenterParent
            };

            Label text = new()
            {
                Text = "Choose groups to export",
                AutoSize = true,
                Location = new(5, 5)
            };
            Export.Controls.Add(text);
            int textHeight = text.GetPreferredSize(Size.Empty).Height;

            CustomButton buttonCancel = new()
            {
                Text = "Cancel",
                DialogResult = DialogResult.Cancel,
            };
            buttonCancel.Location = new(Export.ClientRectangle.Width - buttonCancel.Width - 5, Export.ClientRectangle.Height - buttonCancel.Height - 5);
            Export.Controls.Add(buttonCancel);

            CustomButton buttonOK = new()
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
            };
            buttonOK.Location = new(Export.ClientRectangle.Width - buttonOK.Width - 5 - buttonCancel.Width - 5, Export.ClientRectangle.Height - buttonOK.Height - 5);
            Export.Controls.Add(buttonOK);

            CustomPanel panel = new()
            {
                AutoScroll = true,
                Border = BorderStyle.FixedSingle,
                ButtonBorderStyle = ButtonBorderStyle.Solid,
                Location = new(5, 10 + textHeight),
                Width = Export.ClientRectangle.Width - 10,
                Height = Export.ClientRectangle.Height - textHeight - buttonCancel.Height - 20
            };
            Export.Controls.Add(panel);

            for (int n = 0; n < ListGroupNames.Count; n++)
            {
                string group = ListGroupNames[n];
                CustomCheckBox box = new();
                box.Checked = true;
                box.Text = group;
                box.Location = new(5, 5 + (n * 20));
                panel.Controls.Add(box);
                box.MouseDown += (s, e) =>
                {
                    if (e.Button == MouseButtons.Right)
                    {
                        ExportMenu(e.X + 5, box.Location.Y + box.GetPreferredSize(Size.Empty).Height / 3);
                    }
                };
            }

            if (CurrentTheme == "Dark")
                panel.SetDarkControl();

            buttonOK.Click += ButtonOK_Click;

            void ButtonOK_Click(object? sender, EventArgs e)
            {
                List<int> ints = new();
                for (int a = 0; a < panel.Controls.Count; a++)
                {
                    Control c = panel.Controls[a];
                    foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>().Reverse())
                    {
                        if (ch.Checked)
                        {
                            ints.Add(ListGroupNames.GetIndex(ch.Text));
                        }
                    }
                }

                XDocument doc = CreateXmlSettings();
                var insert = doc.Root.Element("MultipleSearchAndReplaceList");

                ints.Sort();
                for (int b = 0; b < ints.Count; b++)
                {
                    int i = ints[b];
                    insert.Add(GetXmlGroupByName(ReplaceList, ListGroupNames[i]));
                }

                using SaveFileDialog sfd = new();
                sfd.Filter = "Find and replace rules (*.template)|*.template|Find and replace rules (*.xml)|*.xml";
                sfd.DefaultExt = ".template";
                sfd.AddExtension = true;
                sfd.RestoreDirectory = true;
                sfd.FileName = "multiple_replace_groups";
                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    doc.Save(sfd.FileName, SaveOptions.None);
                }
            }

            // Context Menu Export (Selection)
            panel.MouseDown += Panel_MouseDown;
            void Panel_MouseDown(object? sender, MouseEventArgs e)
            {
                if (e.Button == MouseButtons.Right)
                {
                    ExportMenu(e.X, e.Y);
                }
            }

            void ExportMenu(int eX, int eY)
            {
                CustomContextMenuStrip MGE = new();

                ToolStripMenuItem MenuSA = new();
                MenuSA.Text = "Select All";
                MenuSA.Click += MenuSA_Click;
                MGE.Items.Add(MenuSA);

                ToolStripMenuItem MenuIS = new();
                MenuIS.Text = "Invert selection";
                MenuIS.Click += MenuIS_Click;
                MGE.Items.Add(MenuIS);

                Theme.SetColors(MGE);
                MGE.Show(panel, new Point(eX, eY));
            }

            void MenuSA_Click(object? sender, EventArgs e)
            {
                for (int a = 0; a < panel.Controls.Count; a++)
                {
                    Control c = panel.Controls[a];
                    foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>().Reverse())
                    {
                        ch.Checked = true;
                    }
                }
            }

            void MenuIS_Click(object? sender, EventArgs e)
            {
                for (int a = 0; a < panel.Controls.Count; a++)
                {
                    Control c = panel.Controls[a];
                    foreach (CustomCheckBox ch in c.Controls.OfType<CustomCheckBox>().Reverse())
                    {
                        if (ch.Checked)
                            ch.Checked = false;
                        else
                            ch.Checked = true;
                    }
                }
            }

            Export.AcceptButton = buttonOK;
            Export.CancelButton = buttonCancel;

            Theme.LoadTheme(Export, Export.Controls);
            Export.ShowDialog(this);
        }
        #endregion

        #region Rules
        //======================================= Rules ======================================
        private void CustomDataGridViewRules_SelectionChanged(object sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.RowCount == 0) return;
            if (dgvG.SelectedCells.Count <= 0) return;
            if (dgvR.RowCount == 0) return;
            if (dgvR.SelectedCells.Count <= 0) return;

            int currentRow = dgvR.SelectedCells[0].RowIndex;
            string findWhat = dgvR.Rows[currentRow].Cells[1].Value.ToString();
            string replaceWith = dgvR.Rows[currentRow].Cells[2].Value.ToString();
            string searchType = dgvR.Rows[currentRow].Cells[3].Value.ToString();
            string description = dgvR.Rows[currentRow].Cells[4].Value.ToString();

            int currentGRow = dgvG.SelectedCells[0].RowIndex;
            if (dgvG.Rows[currentGRow].Cells[1].Value == null) return;
            string group = dgvG.Rows[currentGRow].Cells[1].Value.ToString();
            int ruleNo = currentRow + 1;
            string ruleNoMsg = " - Rule No: " + ruleNo.ToString();
            string msg = "Rules for group \"" + group + "\"" + ruleNoMsg;
            CustomGroupBoxRules.Text = msg;

            if (dgvR.SelectedRows.Count > 1)
            {
                CustomButtonAdd.Enabled = false;
                CustomButtonUpdate.Enabled = false;
            }
            else
            {
                CustomButtonAdd.Enabled = true;
                CustomButtonUpdate.Enabled = true;
            }

            CustomTextBoxFindWhat.Texts = findWhat.IsNotNull();
            CustomTextBoxReplaceWith.Texts = replaceWith.IsNotNull();

            if (searchType == "Normal")
                CustomComboBoxSearchType.SelectedIndex = 0;
            else if (searchType == "Case Sensitive")
                CustomComboBoxSearchType.SelectedIndex = 1;
            else if (searchType == "Regular Expression")
                CustomComboBoxSearchType.SelectedIndex = 2;
            else
                CustomComboBoxSearchType.SelectedItem = null;

            CustomTextBoxDescription.Texts = description.IsNotNull();
        }

        private void CustomDataGridViewRules_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Save CheckBox State for Rules
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            if (dgvG.Rows.Count == 0) return;
            if (dgvR.Rows.Count == 0) return;
            if (e.ColumnIndex != 0) return;

            int groupIndex = dgvG.SelectedCells[0].RowIndex;
            string group = dgvG.Rows[groupIndex].Cells[1].Value.ToString();

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (group == nodeG.Element("Name").Value)
                {
                    int count = nodeG.Elements("MultipleSearchAndReplaceItem").Count();
                    for (int b = 0; b < count; b++)
                    {
                        if (b == e.RowIndex)
                        {
                            var nodeItem = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[b];
                            nodeItem.Element("Enabled").Value = dgvR.Rows[b].Cells[0].Value.ToString().IsNotNull();

                            // Save xDocument to File
                            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
                        }
                    }
                }
            }
        }

        private void CustomDataGridViewRules_KeyDown(object sender, KeyEventArgs e)
        {
            // Assign Menu Shortcuts to KeyDown (Use Shortcuts When Menu is Not Displayed)
            void checkShortcut(ToolStripMenuItem item)
            {
                if (item.ShortcutKeys == e.KeyData)
                {
                    //item.PerformClick(); // Doesn't work correctly
                    if (item.Text.Contains("Remove"))
                        MenuRuleRemove_Click(null, null);
                    else if (item.Text.Contains("Move up"))
                        MenuRuleMoveUp_Click(null, null);
                    else if (item.Text.Contains("Move down"))
                        MenuRuleMoveDown_Click(null, null);
                    else if (item.Text.Contains("Move to top"))
                        MenuRuleMoveToTop_Click(null, null);
                    else if (item.Text.Contains("Move to bottom"))
                        MenuRuleMoveToBottom_Click(null, null);
                    else if (item.Text.Contains("Import"))
                        MenuRuleImport_Click(null, null);
                    else if (item.Text.Contains("Export"))
                        MenuRuleExport_Click(null, null);
                    else
                        item.PerformClick();
                    return;
                }

                foreach (ToolStripMenuItem child in item.DropDownItems.OfType<ToolStripMenuItem>())
                {
                    checkShortcut(child);
                }
            }

            foreach (ToolStripMenuItem item in MR.Items.OfType<ToolStripMenuItem>())
            {
                checkShortcut(item);
            }

            // Make ContextMenu Shortcuts Work (Move up and Move Down)
            if (e.Control && e.KeyCode == Keys.Up || e.Control && e.KeyCode == Keys.Down)
            {
                e.SuppressKeyPress = true;
            }
        }

        private void CreateMenuRule()
        {
            var dgvG = CustomDataGridViewGroups;
            if (dgvG.Rows.Count == 0) return;

            int groupIndex = dgvG.SelectedCells[0].RowIndex;
            string group = dgvG.Rows[groupIndex].Cells[1].Value.ToString();

            // Context Menu
            MR = new();

            MenuRuleRemove = new();
            MenuRuleRemove.Text = "Remove";
            MenuRuleRemove.ShortcutKeys = Keys.Delete;
            MenuRuleRemove.Click += MenuRuleRemove_Click;
            MR.Items.Add(MenuRuleRemove);

            MenuRuleRemoveAll = new();
            MenuRuleRemoveAll.Text = "Remove all";
            MenuRuleRemoveAll.Click += MenuRuleRemoveAll_Click;
            MR.Items.Add(MenuRuleRemoveAll);

            MR.Items.Add("-");

            MenuRuleMoveSelectedRulesToGroup = new();
            MenuRuleMoveSelectedRulesToGroup.Text = "Move selected rules to group";
            MR.Items.Add(MenuRuleMoveSelectedRulesToGroup);

            // SubMenu for Move Selected Rules To Group
            List<string> ListGroupsToSend = new(ListGroupNames);
            ListGroupsToSend.Remove(group.IsNotNull()); // Remove Current Group From Menu
            ToolStripItem[] subMenuItems = new ToolStripItem[ListGroupsToSend.Count];
            for (int n = 0; n < ListGroupsToSend.Count; n++)
            {
                string groupName = ListGroupsToSend[n];
                subMenuItems[n] = new ToolStripMenuItem(groupName);
                subMenuItems[n].Click += MenuRuleMoveSelectedRulesToGroup_Click;
            }
            MenuRuleMoveSelectedRulesToGroup.DropDownItems.AddRange(subMenuItems);

            MR.Items.Add("-");

            MenuRuleMoveUp = new();
            MenuRuleMoveUp.Text = "Move up";
            MenuRuleMoveUp.ShortcutKeys = Keys.Control | Keys.Up;
            MenuRuleMoveUp.Click += MenuRuleMoveUp_Click;
            MR.Items.Add(MenuRuleMoveUp);

            MenuRuleMoveDown = new();
            MenuRuleMoveDown.Text = "Move down";
            MenuRuleMoveDown.ShortcutKeys = Keys.Control | Keys.Down;
            MenuRuleMoveDown.Click += MenuRuleMoveDown_Click;
            MR.Items.Add(MenuRuleMoveDown);

            MenuRuleMoveToTop = new();
            MenuRuleMoveToTop.Text = "Move to top";
            MenuRuleMoveToTop.ShortcutKeys = Keys.Control | Keys.Home;
            MenuRuleMoveToTop.Click += MenuRuleMoveToTop_Click;
            MR.Items.Add(MenuRuleMoveToTop);

            MenuRuleMoveToBottom = new();
            MenuRuleMoveToBottom.Text = "Move to bottom";
            MenuRuleMoveToBottom.ShortcutKeys = Keys.Control | Keys.End;
            MenuRuleMoveToBottom.Click += MenuRuleMoveToBottom_Click;
            MR.Items.Add(MenuRuleMoveToBottom);

            MR.Items.Add("-");

            MenuRuleImport = new();
            MenuRuleImport.Text = "Import";
            MenuRuleImport.ShortcutKeys = Keys.Control | Keys.I;
            MenuRuleImport.Click += MenuRuleImport_Click;
            MR.Items.Add(MenuRuleImport);

            MenuRuleExport = new();
            MenuRuleExport.Text = "Export";
            MenuRuleExport.ShortcutKeys = Keys.Control | Keys.E;
            MenuRuleExport.Click += MenuRuleExport_Click;
            MR.Items.Add(MenuRuleExport);

            MR.Items.Add("-");

            MenuRuleSelectAll = new();
            MenuRuleSelectAll.Text = "Select all";
            MenuRuleSelectAll.Click += MenuRuleSelectAll_Click;
            MR.Items.Add(MenuRuleSelectAll);

            MenuRuleInvertSelection = new();
            MenuRuleInvertSelection.Text = "Invert selection";
            MenuRuleInvertSelection.Click += MenuRuleInvertSelection_Click;
            MR.Items.Add(MenuRuleInvertSelection);

            Theme.SetColors(MR);
        }

        private void CustomDataGridViewRules_MouseDown(object sender, MouseEventArgs e)
        {
            // Context Menu
            if (e.Button == MouseButtons.Right)
            {
                var dgvG = CustomDataGridViewGroups;
                if (dgvG.RowCount == 0) return;
                if (dgvG.SelectedRows.Count == 0) return;
                var dgvR = CustomDataGridViewRules;
                CustomDataGridViewRules.Select(); // Set Focus on Control

                int currentMouseOverRow = dgvR.HitTest(e.X, e.Y).RowIndex;

                // Disable MultiSelect by RightClick
                if (currentMouseOverRow != -1)
                {
                    if (dgvR.Rows[currentMouseOverRow].Selected == false)
                    {
                        dgvR.ClearSelection();
                        dgvR.Rows[currentMouseOverRow].Cells[0].Selected = true;
                        dgvR.Rows[currentMouseOverRow].Selected = true;
                    }

                    dgvR.Rows[currentMouseOverRow].Cells[0].Selected = true;
                    dgvR.Rows[currentMouseOverRow].Selected = true;
                }

                CreateMenuRule();

                int[] ruleRows = new int[dgvR.SelectedRows.Count];
                for (int a = 0; a < dgvR.SelectedRows.Count; a++)
                {
                    ruleRows[a] = dgvR.SelectedRows[a].Index;
                }
                Array.Sort(ruleRows);

                int firstSelectedCell;
                if (ruleRows.Length == 0)
                    firstSelectedCell = -1;
                else
                    firstSelectedCell = ruleRows[0];

                int totalRows = dgvR.Rows.Count;
                int totalSelectedRows = dgvR.SelectedRows.Count;

                // Remove Menus On Conditions
                if (currentMouseOverRow == -1)
                    showMenu1();
                else
                    showMenu1();

                void showMenu1()
                {
                    if (totalRows == 0)
                    {
                        MR.Items.Remove(MenuRuleRemove);
                        MR.Items.Remove(MenuRuleRemoveAll);
                        MR.Items.Remove(MenuRuleMoveSelectedRulesToGroup);
                        MR.Items.Remove(MenuRuleMoveUp);
                        MR.Items.Remove(MenuRuleMoveDown);
                        MR.Items.Remove(MenuRuleMoveToTop);
                        MR.Items.Remove(MenuRuleMoveToBottom);
                        MR.Items.Remove(MenuRuleExport);
                        MR.Items.Remove(MenuRuleSelectAll);
                        MR.Items.Remove(MenuRuleInvertSelection);
                        MR.Items.RemoveAt(0);
                        MR.Items.RemoveAt(0);
                        MR.Items.RemoveAt(0);
                        MR.Items.RemoveAt(1);
                    }
                    else if (totalRows == 1)
                    {
                        if (totalSelectedRows <= 0)
                        {
                            MR.Items.Remove(MenuRuleRemove);
                            MR.Items.Remove(MenuRuleMoveSelectedRulesToGroup);
                            MR.Items.RemoveAt(1);
                            MR.Items.Remove(MenuRuleMoveUp);
                            MR.Items.Remove(MenuRuleMoveDown);
                            MR.Items.Remove(MenuRuleMoveToTop);
                            MR.Items.Remove(MenuRuleMoveToBottom);
                            MR.Items.RemoveAt(1);
                        }
                        else //if (totalSelectedRows > 0)
                        {
                            MR.Items.Remove(MenuRuleMoveUp);
                            MR.Items.Remove(MenuRuleMoveDown);
                            MR.Items.Remove(MenuRuleMoveToTop);
                            MR.Items.Remove(MenuRuleMoveToBottom);
                            MR.Items.RemoveAt(4);
                        }
                    }
                    else // if (totalRows > 1)
                    {
                        if (totalSelectedRows <= 0)
                        {
                            MR.Items.Remove(MenuRuleRemove);
                            MR.Items.Remove(MenuRuleMoveSelectedRulesToGroup);
                            MR.Items.RemoveAt(1);
                            MR.Items.Remove(MenuRuleMoveUp);
                            MR.Items.Remove(MenuRuleMoveDown);
                            MR.Items.Remove(MenuRuleMoveToTop);
                            MR.Items.Remove(MenuRuleMoveToBottom);
                            MR.Items.RemoveAt(1);
                        }
                        else //if (totalSelectedRows > 1)
                        {
                            movesMenus();
                        }
                    }
                }

                void movesMenus()
                {
                    if (currentMouseOverRow == 0)
                    {
                        MR.Items.Remove(MenuRuleMoveUp);
                        MR.Items.Remove(MenuRuleMoveToTop);
                    }
                    else if (currentMouseOverRow == totalRows - 1)
                    {
                        MR.Items.Remove(MenuRuleMoveDown);
                        MR.Items.Remove(MenuRuleMoveToBottom);
                    }

                    if (totalSelectedRows == totalRows)
                    {
                        MR.Items.Remove(MenuRuleMoveUp);
                        MR.Items.Remove(MenuRuleMoveToTop);
                        MR.Items.Remove(MenuRuleMoveDown);
                        MR.Items.Remove(MenuRuleMoveToBottom);
                        MR.Items.RemoveAt(4);
                    }
                    else if (firstSelectedCell - 1 < 0)
                    {
                        MR.Items.Remove(MenuRuleMoveUp);
                        MR.Items.Remove(MenuRuleMoveToTop);
                    }
                    else if (firstSelectedCell > dgvR.RowCount - 1 - dgvR.SelectedRows.Count)
                    {
                        MR.Items.Remove(MenuRuleMoveDown);
                        MR.Items.Remove(MenuRuleMoveToBottom);
                    }

                    for (int a = 0; a < ruleRows.Length; a++)
                    {
                        if (a != 0)
                        {
                            if (ruleRows[a - 1] + 1 != ruleRows[a])
                            {
                                MR.Items.Remove(MenuRuleMoveUp);
                                MR.Items.Remove(MenuRuleMoveToTop);
                                MR.Items.Remove(MenuRuleMoveDown);
                                MR.Items.Remove(MenuRuleMoveToBottom);
                                MR.Items.RemoveAt(4);
                                break; // Remove MovesMenus If Rows are not selected in order
                            }
                        }
                    }
                }

                DarkTheme.SetDarkTheme(MR);
                MR.Show(dgvR, new Point(e.X, e.Y));
            }
        }

        private void MenuRuleRemove_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            // Find a Row to Select
            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }
            int select = ruleRows[0];
            if (select != 0)
                select--;
            int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;

            // Remove Selected Rules
            RemoveRule(groupName);

            // Refresh Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, select, firstVisible);

            // Clear Selection
            dgvR.ClearSelection();

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
        }

        private void MenuRuleRemoveAll_Click(object? sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.RowCount == 0) return;
            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.RowCount == 0) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    var nodes = nodeG.Elements("MultipleSearchAndReplaceItem");
                    nodes.Remove();
                    break;
                }
            }

            // Refresh Rules
            ReadRules(groupName);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("All Rules Removed From " + groupName);
        }

        private void MenuRuleMoveSelectedRulesToGroup_Click(object? sender, EventArgs e)
        {
            var item = sender as ToolStripItem;
            string groupToSend = item.Text;

            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }

            int select = ruleRows[0];
            if (select != 0)
                select--;

            Array.Sort(ruleRows);

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();
            List<XElement> selectedRules = new();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (nodeG.Element("Name").Value == groupName)
                {
                    // Add Selected Rules To List
                    for (int b = 0; b < ruleRows.Length; b++)
                    {
                        int row = ruleRows[b];
                        var rule = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[row];
                        selectedRules.Add(rule);
                    }

                    // Remove Selected Rules
                    int less = 0;
                    for (int b = 0; b < ruleRows.Length; b++)
                    {
                        int row = ruleRows[b];
                        if (b != 0)
                        {
                            less++;
                            row = ruleRows[b] - less;
                        }
                        var node = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[row];
                        node.Remove();
                    }

                    // Copy Selected Rules
                    for (int b = 0; b < selectedRules.Count; b++)
                    {
                        var rule = selectedRules[b];
                        CopyRuleToGroup(groupToSend, rule, null, null);
                    }

                    break;
                }
            }

            // Read Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, select, firstVisible);

            // Clear Selection
            dgvR.ClearSelection();

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("All Selected Rules Moved to " + groupToSend);
        }

        private void MenuRuleMoveUp_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            // Find Prevoius Rule
            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }

            Array.Sort(ruleRows);

            int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;
            int firstSelectedCell = ruleRows[0];
            int insert = firstSelectedCell - 1;

            if (insert < 0) return; // Return When Reaches Top

            for (int a = 0; a < ruleRows.Length; a++)
            {
                if (a != 0)
                {
                    if (ruleRows[a - 1] + 1 != ruleRows[a])
                        return; // Return If Rows are not selected in order
                }
            }

            MoveRulesToGroup(groupName, ruleRows, insert, true);

            // Find New Location Of Selected Rules To Select
            int[] selectRules = new int[ruleRows.Length];
            for (int n = 0; n < ruleRows.Length; n++)
            {
                if (n == 0)
                    selectRules[n] = firstSelectedCell - 1;
                else
                    selectRules[n] = selectRules[n - 1] + 1;
            }

            // Read Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, selectRules, firstVisible);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("Selected Rules Moved up");
        }

        private void MenuRuleMoveDown_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            // Find Next Rule
            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }

            Array.Sort(ruleRows);

            int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;
            int firstSelectedCell = ruleRows[0];
            int insert = firstSelectedCell;

            int lastSelectedCell = ruleRows[ruleRows.Length - 1];
            int displayedRowCount = dgvR.DisplayedRowCount(false);
            if (lastSelectedCell - firstVisible == displayedRowCount)
                firstVisible++;

            if (insert > dgvR.RowCount - 1 - dgvR.SelectedRows.Count) return; // Return When Reaches Bottom

            for (int a = 0; a < ruleRows.Length; a++)
            {
                if (a != 0)
                {
                    if (ruleRows[a - 1] + 1 != ruleRows[a])
                        return; // Return When Rows are not selected in order
                }
            }

            MoveRulesToGroup(groupName, ruleRows, insert, false);

            // Find New Location Of Selected Rules To Select
            int[] selectRules = new int[ruleRows.Length];
            for (int n = 0; n < ruleRows.Length; n++)
            {
                if (n == 0)
                    selectRules[n] = firstSelectedCell + 1;
                else
                    selectRules[n] = selectRules[n - 1] + 1;
            }

            // Read Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, selectRules, firstVisible);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("All Selected Rules Moved Down");
        }

        private void MenuRuleMoveToTop_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            // Find Top Rule
            int insert = 0;

            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }

            Array.Sort(ruleRows);

            int firstVisible = 0;
            int firstSelectedCell = ruleRows[0];

            if (firstSelectedCell - 1 < 0) return; // Return When Reaches Top

            for (int a = 0; a < ruleRows.Length; a++)
            {
                if (a != 0)
                {
                    if (ruleRows[a - 1] + 1 != ruleRows[a])
                        return; // Return If Rows are not selected in order
                }
            }

            MoveRulesToGroup(groupName, ruleRows, insert, true);

            // Find New Location Of Selected Rules To Select
            int[] selectRules = new int[ruleRows.Length];
            for (int n = 0; n < ruleRows.Length; n++)
            {
                if (n == 0)
                    selectRules[n] = 0;
                else
                    selectRules[n] = selectRules[n - 1] + 1;
            }

            // Read Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, selectRules, firstVisible);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("All Selected Rules Moved to Top");
        }

        private void MenuRuleMoveToBottom_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.SelectedCells.Count < 1) return;
            if (dgvR.SelectedCells.Count < 1) return;

            int currentRowG = dgvG.SelectedRows[0].Index;
            string groupNameNull = dgvG.Rows[currentRowG].Cells[1].Value.ToString();
            string groupName = groupNameNull.IsNotNull();

            // Find Bottom Rule
            int insert = dgvR.RowCount - 1 - dgvR.SelectedRows.Count;

            int[] ruleRows = new int[dgvR.SelectedRows.Count];
            for (int a = 0; a < dgvR.SelectedRows.Count; a++)
            {
                ruleRows[a] = dgvR.SelectedRows[a].Index;
            }

            Array.Sort(ruleRows);

            int firstVisible = dgvR.RowCount - dgvR.DisplayedRowCount(false);
            int firstSelectedCell = ruleRows[0];

            if (firstSelectedCell > insert) return; // Return When Reaches Bottom

            for (int a = 0; a < ruleRows.Length; a++)
            {
                if (a != 0)
                {
                    if (ruleRows[a - 1] + 1 != ruleRows[a])
                        return; // Return If Rows are not selected in order
                }
            }

            MoveRulesToGroup(groupName, ruleRows, insert, false);

            // Find New Location Of Selected Rules To Select
            int[] selectRules = new int[ruleRows.Length];
            for (int n = 0; n < ruleRows.Length; n++)
            {
                if (n == 0)
                    selectRules[n] = dgvR.RowCount - ruleRows.Length;
                else
                    selectRules[n] = selectRules[n - 1] + 1;
            }

            // Read Rules
            ReadRules(groupName);

            // Select and Make it Visible
            ShowRow(dgvR, selectRules, firstVisible);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);

            Debug.WriteLine("All Selected Rules Moved to Bottom");
        }

        private void MenuRuleImport_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            if (dgvG.SelectedRows.Count == 0) return;

            using OpenFileDialog ofd = new();
            ofd.Filter = "Find and replace rules (*.template)|*.template|Find and replace rules (*.xml)|*.xml";
            ofd.DefaultExt = ".template";
            ofd.AddExtension = true;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string groupToSend = dgvG.SelectedRows[0].Cells[1].Value.ToString();

                int selectRule;
                if (dgvR.RowCount > 0)
                    selectRule = dgvR.RowCount;
                else
                    selectRule = 0;

                string filePath = ofd.FileName;

                XDocument importedReplaceList = XDocument.Load(filePath);
                var nodesGroups = importedReplaceList.Root.Elements().Elements();
                
                if (!nodesGroups.Any())
                    return;

                for (int a = 0; a < nodesGroups.Count(); a++)
                {
                    var nodeG = nodesGroups.ToList()[a];
                    var rules = nodeG.Elements("MultipleSearchAndReplaceItem");
                    
                    if (!rules.Any())
                        return;

                    for (int b = 0; b < rules.Count(); b++)
                    {
                        var rule = rules.ToList()[b];
                        CopyRuleToGroup(groupToSend.IsNotNull(), rule, null, null);
                    }
                }

                // Read Rules
                ReadRules(groupToSend.IsNotNull());

                // Select and Make it Visible
                ShowRow(dgvR, selectRule, selectRule);

                // Save xDocument to File
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
        }

        private void MenuRuleExport_Click(object? sender, EventArgs? e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            if (dgvG.SelectedRows.Count == 0) return;
            if (dgvR.RowCount == 0) return;

            string groupToExport = dgvG.SelectedRows[0].Cells[1].Value.ToString();

            XDocument doc = CreateXmlSettings();
            var insert = doc.Root.Element("MultipleSearchAndReplaceList");

            insert.Add(GetXmlGroupByName(ReplaceList, groupToExport.IsNotNull()));

            using SaveFileDialog sfd = new();
            sfd.Filter = "Find and replace rules (*.template)|*.template|Find and replace rules (*.xml)|*.xml";
            sfd.DefaultExt = ".template";
            sfd.AddExtension = true;
            sfd.RestoreDirectory = true;
            sfd.FileName = "multiple_replace_" + groupToExport.IsNotNull();
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                doc.Save(sfd.FileName, SaveOptions.None);
            }
        }

        private void MenuRuleSelectAll_Click(object? sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            if (dgvG.RowCount == 0) return;
            if (dgvR.RowCount == 0) return;

            for (int a = 0; a < dgvR.RowCount; a++)
            {
                var row = dgvR.Rows[a];
                var cellCheck = row.Cells[0];

                cellCheck.Value = true;
                dgvR.EndEdit();
            }

            // Save CheckBox State for Rules
            int groupIndex = dgvG.SelectedCells[0].RowIndex;
            string group = dgvG.Rows[groupIndex].Cells[1].Value.ToString();

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (group == nodeG.Element("Name").Value)
                {
                    int count = nodeG.Elements("MultipleSearchAndReplaceItem").Count();
                    for (int b = 0; b < count; b++)
                    {
                        var row = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[b];
                        row.Element("Enabled").Value = dgvR.Rows[b].Cells[0].Value.ToString().IsNotNull();
                    }
                }
            }
        }

        private void MenuRuleInvertSelection_Click(object? sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;
            if (dgvG.RowCount == 0) return;
            if (dgvR.RowCount == 0) return;

            for (int a = 0; a < dgvR.RowCount; a++)
            {
                var row = dgvR.Rows[a];
                var cellCheck = row.Cells[0];
                bool cellCheckValue = Convert.ToBoolean(cellCheck.Value);

                if (cellCheckValue)
                {
                    cellCheck.Value = false;
                    dgvR.EndEdit();
                }
                else
                {
                    cellCheck.Value = true;
                    dgvR.EndEdit();
                }
            }

            // Save CheckBox State for Rules
            int groupIndex = dgvG.SelectedCells[0].RowIndex;
            string group = dgvG.Rows[groupIndex].Cells[1].Value.ToString();

            XDocument doc = ReplaceList;
            var nodesGroups = doc.Root.Elements().Elements();

            for (int a = 0; a < nodesGroups.Count(); a++)
            {
                var nodeG = nodesGroups.ToList()[a];
                if (group == nodeG.Element("Name").Value)
                {
                    int count = nodeG.Elements("MultipleSearchAndReplaceItem").Count();
                    for (int b = 0; b < count; b++)
                    {
                        var row = nodeG.Elements("MultipleSearchAndReplaceItem").ToList()[b];
                        row.Element("Enabled").Value = dgvR.Rows[b].Cells[0].Value.ToString().IsNotNull();
                    }
                }
            }
        }

        #endregion


        //======================================= Add & Update Rules ======================================
        private void CustomButtonAdd_Click(object sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.RowCount == 0) return;
            if (dgvG.SelectedRows.Count == 0) return;

            // Context Menu
            CustomContextMenuStrip AR = new();

            ToolStripMenuItem MenuAddBelowSelected = new();
            MenuAddBelowSelected.Text = "Add below selected";
            MenuAddBelowSelected.Click += MenuAddBelowSelected_Click;
            AR.Items.Add(MenuAddBelowSelected);

            ToolStripMenuItem MenuAddToEnd = new();
            MenuAddToEnd.Text = "Add to end";
            MenuAddToEnd.Click += MenuAddToEnd_Click;
            AR.Items.Add(MenuAddToEnd);

            if (dgvR.SelectedRows.Count == 0 || dgvR.RowCount == 0)
                AR.Items.Remove(MenuAddBelowSelected);

            if (dgvR.SelectedRows.Count > 0)
                if (dgvR.SelectedRows[0].Index == dgvR.RowCount - 1)
                    AR.Items.Remove(MenuAddBelowSelected);

            Theme.SetColors(AR);
            AR.Show(new Point(Location.X + (ClientSize.Width - AR.Width) + 4, Location.Y + SplitContainer1.Panel1.Height + 1));

            void MenuAddBelowSelected_Click(object? sender, EventArgs e)
            {
                if (CustomComboBoxSearchType.SelectedItem == null)
                {
                    CustomMessageBox.Show("Select search type.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (dgvR.SelectedRows.Count == 0)
                {
                    CustomMessageBox.Show("Select a rule.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var groupToAdd = dgvG.SelectedRows[0].Cells[1].Value.ToString();

                int rowToInsert = dgvR.SelectedRows[0].Index;

                int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;
                if (rowToInsert - firstVisible == dgvR.DisplayedRowCount(false) - 1)
                    firstVisible++;

                // Check If Regex is Valid
                if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Regular Expression")
                {
                    if (Tools.Texts.IsValidRegex(CustomTextBoxFindWhat.Texts) == false)
                    {
                        CustomMessageBox.Show("Regex is not valid.", "Regex");
                        return;
                    }
                }

                XElement rule = CreateRule();

                CopyRuleToGroup(groupToAdd.IsNotNull(), rule, rowToInsert, false);

                // Read Rules
                ReadRules(groupToAdd);

                // Select and Make it Visible
                ShowRow(dgvR, rowToInsert + 1, firstVisible);

                // Save xDocument to File
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }

            void MenuAddToEnd_Click(object? sender, EventArgs e)
            {
                if (CustomComboBoxSearchType.SelectedItem == null)
                {
                    CustomMessageBox.Show("Select search type.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                var groupToAdd = dgvG.SelectedRows[0].Cells[1].Value.ToString();

                int rowToInsert;
                if (dgvR.RowCount > 0)
                    rowToInsert = dgvR.RowCount;
                else
                    rowToInsert = 0;

                int firstVisible = rowToInsert - dgvR.DisplayedRowCount(false) + 1;
                if (firstVisible < 0) firstVisible = 0;

                // Check If Regex is Valid
                if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Regular Expression")
                {
                    if (Tools.Texts.IsValidRegex(CustomTextBoxFindWhat.Texts) == false)
                    {
                        CustomMessageBox.Show("Regex is not valid.", "Regex");
                        return;
                    }
                }

                XElement rule = CreateRule();

                CopyRuleToGroup(groupToAdd.IsNotNull(), rule, null, null);

                // Read Rules
                ReadRules(groupToAdd);

                // Select and Make it Visible
                ShowRow(dgvR, rowToInsert, firstVisible);

                // Save xDocument to File
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
        }

        private void CustomButtonUpdate_Click(object sender, EventArgs e)
        {
            var dgvG = CustomDataGridViewGroups;
            var dgvR = CustomDataGridViewRules;

            if (dgvG.RowCount == 0) return;
            if (dgvG.SelectedRows.Count == 0) return;
            if (dgvR.RowCount == 0) return;

            if (CustomComboBoxSearchType.SelectedItem == null)
            {
                CustomMessageBox.Show("Select search type.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (dgvR.SelectedRows.Count == 0)
            {
                CustomMessageBox.Show("Select a rule to update.", "Message", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string searchTypeString = CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull();
            if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Case Sensitive")
            {
                searchTypeString = "CaseSensitive";
            }
            else if (CustomComboBoxSearchType.SelectedItem.ToString().IsNotNull() == "Regular Expression")
            {
                searchTypeString = "RegularExpression";

                // Check If Regex is Valid
                if (Tools.Texts.IsValidRegex(CustomTextBoxFindWhat.Texts) == false)
                {
                    CustomMessageBox.Show("Regex is not valid.", "Regex");
                    return;
                }
            }

            var groupToUpdate = dgvG.SelectedRows[0].Cells[1].Value.ToString();
            int rowToSelect = dgvR.SelectedRows[0].Index;
            int firstVisible = dgvR.FirstDisplayedScrollingRowIndex;

            XDocument doc = ReplaceList;
            var groups = doc.Root.Elements().Elements();

            for (int a = 0; a < groups.Count(); a++)
            {
                var group = groups.ToList()[a];
                if (group.Element("Name").Value == groupToUpdate)
                {
                    var rules = group.Elements("MultipleSearchAndReplaceItem").ToList();
                    for (int b = 0; b < rules.Count; b++)
                    {
                        var rule = rules[b];
                        if (rowToSelect == b)
                        {
                            rule.Element("FindWhat").Value = CustomTextBoxFindWhat.Texts;
                            rule.Element("ReplaceWith").Value = CustomTextBoxReplaceWith.Texts;
                            rule.Element("SearchType").Value = searchTypeString;
                            rule.Element("Description").Value = CustomTextBoxDescription.Texts;
                            break;
                        }
                    }
                }
            }

            // Read Rules
            ReadRules(groupToUpdate);

            // Select and Make it Visible
            ShowRow(dgvR, rowToSelect, firstVisible);

            // Save xDocument to File
            ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
        }

        //======================================= Preview ======================================
        private void CustomButtonPreview_Click(object sender, EventArgs e)
        {
            if (!FormMain.IsSubLoaded || subPreview == null)
            {
                CustomMessageBox.Show("No Subtitle Loaded.", "Message");
                return;
            }

            // Context Menu
            CustomContextMenuStrip PV = new();

            ToolStripMenuItem MenuFromBeginning = new();
            MenuFromBeginning.Text = "From beginning";
            MenuFromBeginning.Click += MenuFromBeginning_Click;
            PV.Items.Add(MenuFromBeginning);

            ToolStripMenuItem MenuContinue = new();
            MenuContinue.Text = "Continue";
            MenuContinue.Click += MenuContinue_Click;
            PV.Items.Add(MenuContinue);

            Theme.SetColors(PV);
            PV.Show(new Point(Location.X + CustomGroupBoxGroups.Width - CustomButtonPreview.Width + 6,
                                Location.Y + SplitContainer1.Panel1.Height + CustomButtonPreview.Height + 3));

            void MenuFromBeginning_Click(object? sender, EventArgs e)
            {
                subPreview = new(FormMain.SubCurrent);
                ApplyPreview();
            }

            void MenuContinue_Click(object? sender, EventArgs e)
            {
                ApplyPreview();
            }
        }

        private void ApplyPreview()
        {
            Task.Run(() =>
            {
                var stopwatch = Stopwatch.StartNew();
                CustomButtonPreview.InvokeIt(() => CustomButtonPreview.Enabled = false);
                CustomGroupBoxPreview.InvokeIt(() => CustomGroupBoxPreview.Text = "Please Wait...");
                int totalFixes = 0;
                //========== Creating List ============================================
                List<Tuple<string, string, string, string, int>> ReplaceExpressionList = new();
                ReplaceExpressionList.Clear();

                Dictionary<string, Regex> CompiledRegExList = new();
                CompiledRegExList.Clear();

                XDocument doc = ReplaceList;
                var groups = doc.Root.Elements().Elements();

                for (int a = 0; a < groups.Count(); a++)
                {
                    var group = groups.ToList()[a];
                    string groupName = group.Element("Name").Value;
                    bool groupEnabled = Convert.ToBoolean(group.Element("Enabled").Value);
                    if (groupEnabled)
                    {
                        var rules = group.Elements("MultipleSearchAndReplaceItem");
                        for (int b = 0; b < rules.Count(); b++)
                        {
                            var rule = rules.ToList()[b];
                            int ruleNumber = b;
                            bool ruleEnabled = Convert.ToBoolean(rule.Element("Enabled").Value);
                            if (ruleEnabled)
                            {
                                string findWhat = rule.Element("FindWhat").Value;
                                string replaceWith = rule.Element("ReplaceWith").Value;
                                string searchType = rule.Element("SearchType").Value;

                                findWhat = FindWhatRule(findWhat);
                                replaceWith = ReplaceWithRule(replaceWith);

                                if (searchType == "RegularExpression" && !CompiledRegExList.ContainsKey(findWhat))
                                {
                                    CompiledRegExList.Add(findWhat, new Regex(findWhat, RegexOptions.Compiled | RegexOptions.Multiline,
                                        TimeSpan.FromMilliseconds(2000)));
                                }

                                ReplaceExpressionList.Add(new Tuple<string, string, string, string, int>(findWhat, replaceWith, searchType, groupName, ruleNumber));
                            }
                        }
                    }
                }

                //========== Replacing List ===========================================
                List<DataGridViewRow> fixes = new();
                fixes.Clear();

                AppliedRules.Clear();

                // Set a timeout interval of 2 seconds.
                AppDomain domain = AppDomain.CurrentDomain;
                domain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromMilliseconds(2000));

                //Parallel.ForEach(_subtitle.Paragraphs, p =>
                for (int pn = 0; pn < subPreview.Paragraphs.Count; pn++)
                {
                    List<Tuple<string, int>> tempAppliedRegex = new();

                    Paragraph p = subPreview.Paragraphs[pn];

                    p.Text = p.Text.Replace("<br />", Environment.NewLine).Replace("</ br>", Environment.NewLine);
                    string Before = @p.Text;
                    string After = @p.Text;

                    for (int i = 0; i < ReplaceExpressionList.Count; i++)
                    {
                        var list = ReplaceExpressionList[i];
                        string findWhat = list.Item1;
                        string replaceWith = list.Item2;
                        string searchType = list.Item3;
                        string groupName = list.Item4;
                        int ruleNumber = list.Item5;

                        if (searchType == "CaseSensitive")
                        {
                            if (After.Contains(findWhat))
                                After = After.Replace(findWhat, replaceWith);
                        }
                        else if (searchType == "RegularExpression")
                        {
                            Regex regExFindWhat = CompiledRegExList[findWhat];

                            try
                            {
                                if (regExFindWhat.IsMatch(After))
                                {
                                    if (regExFindWhat.Replace(After, replaceWith) != After)
                                        tempAppliedRegex.Add(new Tuple<string, int>(groupName, ruleNumber));
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
                        else if (searchType == "Normal")
                        {
                            After = After.Replace(findWhat, replaceWith, StringComparison.OrdinalIgnoreCase);
                        }
                    }

                    AppliedRules.Add(pn, tempAppliedRegex);

                    After = After.Trim();
                    if (After != Before)
                    {
                        // Add by AddRange
                        DataGridViewRow row = new();
                        row.CreateCells(CustomDataGridViewPreview, "cell0", "cell1", "cell2");
                        row.Height = 20;
                        row.Cells[0].Value = p.Number.ToString();
                        row.Cells[1].Value = Before.Replace(Environment.NewLine, PSF.LineSeparator);
                        row.Cells[2].Value = After.Replace(Environment.NewLine, PSF.LineSeparator);
                        fixes.Add(row);

                        totalFixes++;

                        p.Text = After;
                    }
                }

                if (totalFixes == 0)
                    subPreview = new(FormMain.SubCurrent);

                CustomGroupBoxPreview.InvokeIt(() => CustomGroupBoxPreview.Text = string.Format("Total Fixes: {0}", totalFixes));
                CustomDataGridViewPreview.InvokeIt(() =>
                {
                    CustomDataGridViewPreview.Rows.Clear();
                    CustomDataGridViewPreview.Rows.AddRange(fixes.ToArray());
                });

                stopwatch.Stop();
                Debug.WriteLine(stopwatch.Elapsed);
                CustomButtonPreview.InvokeIt(() => CustomButtonPreview.Enabled = true);
            });
        }

        private void AppliedRegex(int paragraphNumber)
        {
            for (int i = 0; i < CustomPanelAppliedRegex.Controls.Count; i++)
            {
                Control c1 = CustomPanelAppliedRegex.Controls[i];
                if (c1 is Panel panel)
                {
                    while (panel.Controls.Count > 1)
                    {
                        for (int j = 0; j < panel.Controls.Count; j++)
                        {
                            Control c2 = panel.Controls[j];
                            if (!c2.Name.Equals(LabelAppliedRegex.Name))
                                panel.Controls.Remove(c2);
                        }
                    }
                }
            }

            int countG = 0;
            int countR = 0;
            int topSpace = 30;
            
            var appliedRulesList = AppliedRules[paragraphNumber];
            for (int a = 0; a < appliedRulesList.Count; a++)
            {
                var appliedRule = appliedRulesList[a];
                string appliedGroupName = appliedRule.Item1;
                int appliedRuleNumber = appliedRule.Item2;

                if (a == 0)
                {
                    Label groupL = new();
                    groupL.Location = new(5, (countG * 20) + (countR * 20) + topSpace);
                    groupL.Text = appliedGroupName;
                    groupL.AutoSize = true;
                    CustomPanelAppliedRegex.Controls.Add(groupL);
                    countG++;

                    Label ruleL = new();
                    ruleL.Location = new(20, (countG * 20) + (countR * 20) + topSpace);
                    ruleL.Text = "Rule No. " + (appliedRuleNumber + 1);
                    ruleL.Tag = appliedGroupName + "|" + appliedRuleNumber;
                    ruleL.AutoSize = true;
                    CustomPanelAppliedRegex.Controls.Add(ruleL);
                    ruleL.Name = "RuleLabel" + a;
                    ruleL.MouseEnter += (s, e) => { ruleL.ForeColor = Color.DodgerBlue; };
                    ruleL.MouseLeave += (s, e) => { ruleL.ForeColor = Colors.ForeColor; };
                    ruleL.Click += RuleL_Click;
                    countR++;
                }
                else
                {
                    if (appliedGroupName == appliedRulesList[a - 1].Item1)
                    {
                        Label ruleL = new();
                        ruleL.Location = new(20, (countG * 20) + (countR * 20) + topSpace);
                        ruleL.Text = "Rule No. " + (appliedRuleNumber + 1);
                        ruleL.Tag = appliedGroupName + "|" + appliedRuleNumber;
                        ruleL.AutoSize = true;
                        CustomPanelAppliedRegex.Controls.Add(ruleL);
                        ruleL.Name = "RuleLabel" + a;
                        ruleL.Click += RuleL_Click;
                        ruleL.MouseEnter += (s, e) => { ruleL.ForeColor = Color.DodgerBlue; };
                        ruleL.MouseLeave += (s, e) => { ruleL.ForeColor = Colors.ForeColor; };
                        countR++;
                    }
                    else
                    {
                        Label groupL = new();
                        groupL.Location = new(5, (countG * 20) + (countR * 20) + topSpace);
                        groupL.Text = appliedGroupName;
                        groupL.AutoSize = true;
                        CustomPanelAppliedRegex.Controls.Add(groupL);
                        countG++;

                        Label ruleL = new();
                        ruleL.Location = new(20, (countG * 20) + (countR * 20) + topSpace);
                        ruleL.Text = "Rule No. " + (appliedRuleNumber + 1);
                        ruleL.Tag = appliedGroupName + "|" + appliedRuleNumber;
                        ruleL.AutoSize = true;
                        CustomPanelAppliedRegex.Controls.Add(ruleL);
                        ruleL.Name = "RuleLabel" + a;
                        ruleL.Click += RuleL_Click;
                        ruleL.MouseEnter += (s, e) => { ruleL.ForeColor = Color.DodgerBlue; };
                        ruleL.MouseLeave += (s, e) => { ruleL.ForeColor = Colors.ForeColor; };
                        countR++;
                    }
                }
            }

            if (appliedRulesList.Count == 0)
            {
                Label noRegex = new();
                noRegex.Text = "No Regular Expression Applied.";
                int x = (CustomPanelAppliedRegex.Width - noRegex.GetPreferredSize(Size.Empty).Width) / 2;
                noRegex.Location = new(x, topSpace);
                noRegex.AutoSize = true;
                CustomPanelAppliedRegex.Controls.Add(noRegex);
            }

            CustomPanelAppliedRegex.AutoScroll = true;

            void RuleL_Click(object? sender, EventArgs e)
            {
                var label = sender as Label;
                var split = label.Tag.ToString().Split('|');

                string groupName = split[0];
                int ruleNumber = int.Parse(split[1]);

                var dgvR = CustomDataGridViewRules;
                if (dgvR.RowCount == 0) return;

                int firstVisible = ruleNumber - (dgvR.DisplayedRowCount(false) / 2);
                if (firstVisible < 0) firstVisible = 0;

                // Select Group
                ReadGroups(groupName, true);

                // Select and Show Rule
                ShowRow(dgvR, ruleNumber, firstVisible);
            }
        }

        private void CustomDataGridViewPreview_SelectionChanged(object sender, EventArgs e)
        {
            var dgvP = CustomDataGridViewPreview;

            if (dgvP.RowCount == 0) return;
            if (dgvP.SelectedRows.Count == 0) // Clear Applied Regex Panel and Return.
            {
                for (int i = 0; i < CustomPanelAppliedRegex.Controls.Count; i++)
                {
                    Control c1 = CustomPanelAppliedRegex.Controls[i];
                    if (c1 is Panel panel)
                    {
                        while (panel.Controls.Count > 1)
                        {
                            for (int j = 0; j < panel.Controls.Count; j++)
                            {
                                Control c2 = panel.Controls[j];
                                if (!c2.Name.Equals(LabelAppliedRegex.Name))
                                    panel.Controls.Remove(c2);
                            }
                        }
                    }
                }
                return;
            }

            int currentRow = dgvP.SelectedRows[0].Index;
            int pNumber = int.Parse(dgvP.Rows[currentRow].Cells[0].Value.ToString().IsNotNull());
            pNumber--;

            AppliedRegex(pNumber);
        }

        private void CustomDataGridViewPreview_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var dgvP = CustomDataGridViewPreview;
                dgvP.Select(); // Set Focus on Control
                if (dgvP.RowCount == 0) return;

                int currentMouseOverRow = dgvP.HitTest(e.X, e.Y).RowIndex;
                int currentMouseOverColumn = dgvP.HitTest(e.X, e.Y).ColumnIndex;
                if (currentMouseOverRow == -1 || currentMouseOverColumn == -1) return;
                if (currentMouseOverColumn == 0) return;
                int pNumber = int.Parse(dgvP.Rows[currentMouseOverRow].Cells[0].Value.ToString().IsNotNull());
                pNumber--;

                var currentCell = dgvP.Rows[currentMouseOverRow].Cells[currentMouseOverColumn];
                if (currentCell == null) return;

                string currentValue = currentCell.Value.ToString().IsNotNull();
                currentValue = currentValue.Replace("<br />", Environment.NewLine).Replace("</ br>", Environment.NewLine);

                // Select Current Row
                dgvP.Rows[currentMouseOverRow].Selected = true;
                dgvP.Rows[currentMouseOverRow].Cells[0].Selected = true;

                // Create New Window
                Form TestRegex = new()
                {
                    Size = new(500, 304),
                    Text = "Test Regex",
                    FormBorderStyle = FormBorderStyle.FixedToolWindow,
                    ShowInTaskbar = false,
                    StartPosition = FormStartPosition.CenterParent
                };

                Label inputLabel = new();
                CustomTextBox input = new();
                Label findWhatLabel = new();
                CustomTextBox findWhatRegex = new();
                Label replaceWithLabel = new();
                CustomTextBox replaceWithRegex = new();
                CustomButton btnClear = new();
                CustomButton btnApply = new();
                Label outputLabel = new();
                CustomTextBox output = new();

                inputLabel.AutoSize = true;
                inputLabel.Text = "Input:";
                inputLabel.Location = new(5, 5);
                TestRegex.Controls.Add(inputLabel);

                input.Multiline = true;
                input.RightToLeft = RightToLeft.Yes;
                input.Texts = currentValue;
                input.Width = TestRegex.ClientSize.Width - 10;
                input.Height = 55;
                input.Location = new(5, 5 + inputLabel.GetPreferredSize(Size.Empty).Height + 2);
                TestRegex.Controls.Add(input);

                findWhatLabel.AutoSize = true;
                findWhatLabel.Text = "Find what:";
                findWhatLabel.Location = new(5, input.Bottom + 2);
                TestRegex.Controls.Add(findWhatLabel);

                findWhatRegex.Width = TestRegex.ClientSize.Width - 10;
                findWhatRegex.Location = new(5, findWhatLabel.Bottom + 2);
                TestRegex.Controls.Add(findWhatRegex);

                replaceWithLabel.AutoSize = true;
                replaceWithLabel.Text = "Replace with:";
                replaceWithLabel.Location = new(5, findWhatRegex.Bottom + 2);
                TestRegex.Controls.Add(replaceWithLabel);

                replaceWithRegex.Width = TestRegex.ClientSize.Width - 10;
                replaceWithRegex.Location = new(5, replaceWithLabel.Bottom + 2);
                TestRegex.Controls.Add(replaceWithRegex);

                btnClear.Text = "Clear";
                btnClear.Location = new(TestRegex.ClientSize.Width - btnClear.Width - 5, replaceWithRegex.Bottom + 2);
                btnClear.Click += BtnClear_Click;
                TestRegex.Controls.Add(btnClear);

                btnApply.Text = "Apply";
                btnApply.Location = new(TestRegex.ClientSize.Width - btnClear.Width - 5 - btnApply.Width - 2, replaceWithRegex.Bottom + 2);
                btnApply.Click += BtnApply_Click;
                TestRegex.Controls.Add(btnApply);

                outputLabel.AutoSize = true;
                outputLabel.Text = "Output:";
                outputLabel.Location = new(5, btnApply.Bottom + 2);
                TestRegex.Controls.Add(outputLabel);

                output.Multiline = true;
                output.RightToLeft = RightToLeft.Yes;
                output.Width = TestRegex.ClientSize.Width - 10;
                output.Height = 55;
                output.Location = new(5, outputLabel.Bottom + 2);
                TestRegex.Controls.Add(output);

                void BtnClear_Click(object? sender, EventArgs e)
                {
                    findWhatRegex.Texts = string.Empty;
                    replaceWithRegex.Texts = string.Empty;
                }

                void BtnApply_Click(object? sender, EventArgs e)
                {
                    string inputText = input.Texts;
                    string findWhat = findWhatRegex.Texts;
                    string replaceWith = replaceWithRegex.Texts;

                    if (!Tools.Texts.IsValidRegex(findWhat))
                    {
                        CustomMessageBox.Show("Regex is not valid.", "Regex");
                        return;
                    }

                    findWhat = findWhat.Replace("\"", "\\\"");
                    findWhat = findWhat.Replace("\\\\\"", "\\\"");
                    findWhat = findWhat.Replace("\\r\\n", Environment.NewLine).Replace("\\n", Environment.NewLine);

                    string outputText = Regex.Replace(inputText, findWhat, replaceWith, RegexOptions.Multiline);

                    output.Texts = outputText;
                }

                Theme.LoadTheme(TestRegex, TestRegex.Controls);
                TestRegex.ShowDialog(this);
            }
        }
        
        private void MultipleReplace_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing || e.CloseReason == CloseReason.WindowsShutDown)
            {
                // Save xDocument to File
                ReplaceList.Save(PSF.ReplaceListPath, SaveOptions.None);
            }
        }

        
    }
}
