using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using D3Helper.A_Collection;
using D3Helper.A_Enums;
using MouseEventArgs = System.Windows.Forms.MouseEventArgs;

namespace D3Helper
{

    public partial class Window_SkillEditor : Form
    {
        public class ComboboxItem
        {
            public string Text { get; set; }
            public object Value { get; set; }

            public override string ToString()
            {
                return Text;
            }
        }

        /*----------------------------------------------------------------*/

        BindingSource conditionsBinding = new BindingSource();
        BindingSource conditionsSetBinding = new BindingSource();
        BindingSource skillsBinding = new BindingSource();


        /*----------------------------------------------------------------*/

        //contextmenu - conditions listbox;
        ContextMenuStrip contextMenuListBox = new ContextMenuStrip();
        ToolStripMenuItem ts_add_new;
        ToolStripMenuItem ts_copy;
        ToolStripMenuItem ts_paste;
        ToolStripMenuItem ts_delete;

        //contextmenu - condition group listbox;
        ContextMenuStrip contextMenuListBoxConditionGroups = new ContextMenuStrip();
        ToolStripMenuItem group_add_new;
        ToolStripMenuItem group_delete;

        //contextmenu - skill listbox
        ContextMenuStrip contextMenuListBoxSkills = new ContextMenuStrip();
        ToolStripMenuItem skills_add_new;
        ToolStripMenuItem skills_delete;

        /*----------------------------------------------------------------*/


        public static SkillData _SelectedSkill = null;
        private static CastCondition _SelectedCondition = null;

        public static bool _PreselectedDefinition = false;
        public static bool _CreateNewDefinition = false;
        public static int _NewDefinitionPowerSNO;

        public static Window_SkillEditor _this = null;

        /*----------------------------------------------------------------*/


        public new void Show()
        {
            //Skill Editor will crash if Diablo is not running
            if (!Window_Main.d3helperform.SupportedProcessVersion())
            {
                MessageBox.Show("Please run Diablo III first!");
                return;
            }
            base.Show();
        }


        public Window_SkillEditor()
        {

            if (!Window_Main.d3helperform.SupportedProcessVersion())
            {
                return;
            }

            InitializeComponent();

            this.FormClosed += Window_SkillEditor_FormClosed;

            this.CB_ConditionSelection.DrawMode = DrawMode.OwnerDrawFixed;
            this.CB_ConditionSelection.DrawItem += new DrawItemEventHandler(CB_ConditionSelection_DrawItem);

            toolTip1.ShowAlways = false;



            //contextmenu conditions
            ts_add_new = new ToolStripMenuItem { Text = "Add new condition" };
            ts_add_new.Click += Ts_add_new_Click;

            ts_copy = new ToolStripMenuItem { Text = "Copy" };
            ts_copy.Click += Ts_copy_Click;

            ts_paste = new ToolStripMenuItem { Text = "Paste" };
            ts_paste.Click += Ts_paste_Click;

            ts_delete = new ToolStripMenuItem { Text = "Delete" };
            ts_delete.Click += Ts_delete_Click;

            contextMenuListBox.Items.Add(ts_add_new);
            contextMenuListBox.Items.Add(ts_copy);
            contextMenuListBox.Items.Add(ts_paste);
            contextMenuListBox.Items.Add(ts_delete);

            listBox_conditions.MouseDown += ListBox_conditions_MouseDown;



            //contextmenu condition groups
            group_add_new = new ToolStripMenuItem { Text = "Add new Group" };
            group_add_new.Click += Group_add_new_Click;

            group_delete= new ToolStripMenuItem { Text = "Delete Group" };
            group_delete.Click += Group_delete_Click;

            contextMenuListBoxConditionGroups.Items.Add(group_add_new);
            contextMenuListBoxConditionGroups.Items.Add(group_delete);

            listBox_conditionsets.MouseDown += ListBox_conditionsets_MouseDown;


            //contextmenu skills
            skills_add_new = new ToolStripMenuItem { Text = "Add new Skill Definition" };
            skills_add_new.Click += Skills_add_new_Click;

            skills_delete = new ToolStripMenuItem { Text = "Delete" };
            skills_delete.Click += Skills_delete_Click;

            contextMenuListBoxSkills.Items.Add(skills_add_new);
            contextMenuListBoxSkills.Items.Add(skills_delete);

            listBox_skills.MouseDown += ListBox_skills_MouseDown;

        }

        private void ListBox_skills_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBox_skills.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                skills_delete.Enabled = true;

                listBox_skills.SelectedIndex = index;
            }
            else
            {
                skills_delete.Enabled = false;
            }

            contextMenuListBoxSkills.Show(MousePosition);
            contextMenuListBoxSkills.Visible = true;
        }

        private void Skills_delete_Click(object sender, EventArgs e)
        {
            SkillData_Delete();
        }

        private void Skills_add_new_Click(object sender, EventArgs e)
        {
            SkillData_Add();
        }

        private void Ts_add_new_Click(object sender, EventArgs e)
        {
            Condition_Add(-1);
        }

        private void ListBox_conditionsets_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBox_conditionsets.IndexFromPoint(e.Location);

            if (index != ListBox.NoMatches)
            {
                group_delete.Enabled = true;

                listBox_conditionsets.SelectedIndex = index;
            }
            else
            {
                group_delete.Enabled = false;
            }

            contextMenuListBoxConditionGroups.Show(MousePosition);
            contextMenuListBoxConditionGroups.Visible = true;
        }


        private void Group_delete_Click(object sender, EventArgs e)
        {
            ConditionGroup selected_group = (ConditionGroup)listBox_conditionsets.SelectedItem;

            if(selected_group != null)
            {
                _SelectedSkill.removeGroup(selected_group.ConditionGroupValue);

                Update_PanelSelectedSkillDetails();
            }
        }

        private void Group_add_new_Click(object sender, EventArgs e)
        {
            Condition_Add(_SelectedSkill.getNewGroupId());
        }

        private void Ts_delete_Click(object sender, EventArgs e)
        {
            Condition_Delete();
        }

        private void Ts_paste_Click(object sender, EventArgs e)
        {
            Condition_Paste();
        }

        private void Ts_copy_Click(object sender, EventArgs e)
        {
            Condition_Copy();
        }

        private void ListBox_conditions_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Right) return;
            var index = listBox_conditions.IndexFromPoint(e.Location);


            if (global_castCondition_to_copy != null)
            {
                ts_paste.Enabled = true;
            }else
            {
                ts_paste.Enabled = false;
            }

            
            if (index != ListBox.NoMatches)
            {
                ts_copy.Enabled = true;
                ts_delete.Enabled = true;

                listBox_conditions.SelectedIndex = index;
            }
            else
            {
                ts_delete.Enabled = false;
                ts_copy.Enabled = false;
            }

            contextMenuListBox.Show(MousePosition);
            contextMenuListBox.Visible = true;
        }

        void CB_ConditionSelection_DrawItem(object sender, DrawItemEventArgs e)
        {
            string text = CB_ConditionSelection.GetItemText(CB_ConditionSelection.Items[e.Index]);
            string t_text = ConditionTypeHelper.getTooltip(text);

            e.DrawBackground();
            using (SolidBrush br = new SolidBrush(e.ForeColor))
            { e.Graphics.DrawString(text, e.Font, br, e.Bounds); }

            if ((e.State & DrawItemState.Selected) == DrawItemState.Selected && CB_ConditionSelection.DroppedDown)
            {
                
                this.toolTip1.Show(t_text, CB_ConditionSelection, CB_ConditionSelection.Bounds.Right,
                    CB_ConditionSelection.Bounds.Top - (CB_ConditionSelection.Bounds.Height*2));
            }
            else
            {
                this.toolTip1.Hide(CB_ConditionSelection);
            }
            
            e.DrawFocusRectangle();
        }





        private void Window_SkillEditor_FormClosed(object sender, FormClosedEventArgs e)
        {
            Remove_UnassignedConditions();

            A_Tools.T_ExternalFile.CustomSkillDefinitions.Save(A_Collection.SkillCastConditions.Custom.CustomDefinitions);

            _this = null;
        }

        private void Remove_UnassignedConditions()
        {
            foreach (var definition in A_Collection.SkillCastConditions.Custom.CustomDefinitions)
            {
                var tryGetEmptyAssign = definition.CastConditions.FirstOrDefault(x => x.ConditionGroup == -1);

                if (tryGetEmptyAssign != null)
                    definition.CastConditions.Remove(tryGetEmptyAssign);
            }
        }

        private void remove_PowerNamePrefixed()
        {
            var outdatedEntries =
                A_Collection.SkillCastConditions.Custom.CustomDefinitions.Where(
                    x => x.Power.Name.ToLower().StartsWith("x1_"));

            foreach (var entry in outdatedEntries)
            {
                entry.Power.Name = entry.Power.Name.TrimStart(new char[] {'X', '1', '_'});
            }
        }


        private void Form1_Load(object sender, EventArgs e)
        {
            
            remove_PowerNamePrefixed();

            _this = this;

            SkillData _SelectedSkill_parameter = _SelectedSkill;


            Window_ActivePowerView.Get_LatestPowers();



            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;



            CB_PowerSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            CB_PowerSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            CB_PowerSelection.AutoCompleteSource = AutoCompleteSource.ListItems;

            CB_SelectedRune.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDown;
            CB_SelectedRune.AutoCompleteMode = AutoCompleteMode.Suggest;
            CB_SelectedRune.AutoCompleteSource = AutoCompleteSource.ListItems;

            CB_ConditionSelection.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            //CB_ConditionSelection.AutoCompleteMode = AutoCompleteMode.Suggest;
            //CB_ConditionSelection.AutoCompleteSource = AutoCompleteSource.None;
            
            if (_SelectedSkill == null)
            {
                BTN_Update.Visible = false;
                Panel_ConditionEditor.Visible = false;
                BTN_Import.Visible = false;
                BTN_Export.Visible = false;

            }
            if(_SelectedCondition == null)
            {
                BTN_ConditionEdit.Visible = false;
                BTN_ContitionRemove.Visible = false;
            }

            Populate_ComboBox_PowerSelection();
            Populate_ComboBox_ConditionSelection();

            Update_View();





            //-----------------------------------------------------------------------------
            // Proceed Parameters
            //-----------------------------------------------------------------------------

            //select skill by parameter
            if (_SelectedSkill_parameter != null)
            {
                listBox_skills.SelectedIndex = listBox_skills.Items.IndexOf(_SelectedSkill_parameter);
            }


            //create new Skill Definition by parameter
            if (_CreateNewDefinition)
            {

                //Fill text and select correct rune
                string PowerName = A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(x => x.PowerSNO == _NewDefinitionPowerSNO).Name;

                TB_SkillName.Text = PowerName;

                ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == PowerName);

                CB_PowerSelection.SelectedItem = _selection;

                int index = CB_PowerSelection.Items.IndexOf(_selection);
                CB_PowerSelection.SelectedIndex = index;

                _CreateNewDefinition = false;

                //press Add Button
                SkillData_Add();
            }


        }

        private void Populate_ComboBox_PowerSelection()
        {
            CB_PowerSelection.Items.Clear();

            foreach (var power in Presets.SkillPowers.AllSkillPowers)
            {
                ComboboxItem NewItem = new ComboboxItem();
                NewItem.Text = power.Name;
                NewItem.Value = power;

                CB_PowerSelection.Items.Add(NewItem);
            }

            if(_SelectedSkill == null)
                CB_PowerSelection.SelectedIndex = 0;
        }

        private void Populate_ComboBox_ConditionSelection()
        {
            foreach (var condition in Enum.GetValues(typeof(ConditionType)).Cast<ConditionType>().OrderBy(x => x.ToString()).ToList())
            {
                ComboboxItem NewItem = new ComboboxItem();
                NewItem.Text = condition.ToString();
                NewItem.Value = condition;
                
                CB_ConditionSelection.Items.Add(NewItem);
                
            }

            CB_ConditionSelection.SelectedIndex = 0;
            
        }

        private void Update_View()
        {
            if (_SelectedSkill != null)
            {
                BTN_Update.Visible = true;
                Panel_ConditionEditor.Visible = true;
                BTN_Import.Visible = false;
                BTN_Export.Visible = false;

            }
            else
            {
                BTN_Update.Visible = false;
                Panel_ConditionEditor.Visible = false;
                BTN_Import.Visible = true;
                BTN_Export.Visible = true;

            }

            Update_PanelSkillOverview();
            Update_PanelSelectedSkillDetails();
        }

        private void Update_PanelSkillOverview()
        {
            skillsBinding.DataSource = SkillCastConditions.Custom.CustomDefinitions;
            listBox_skills.DataSource = skillsBinding;

            listBox_skills.DisplayMember = "Name";
            listBox_skills.ValueMember = "Name";

            skillsBinding.ResetBindings(false);
        }



        private void Mark_SelectedCondition()
        {

        }


        private void Update_PanelSelectedSkillDetails(bool IsEdit = false)
        {
            try
            {
                if (_SelectedSkill != null)
                {        
                    conditionsSetBinding.DataSource = _SelectedSkill.getConditionGroups();
                    listBox_conditionsets.DataSource = conditionsSetBinding;

                    listBox_conditionsets.DisplayMember = "Name";
                    listBox_conditionsets.ValueMember = "Name";

                    conditionsSetBinding.ResetBindings(false);

                    refresh_listbox_conditions();

                    //skill icon
                    Image icon = _SelectedSkill.getIcon();
                    button_skillimage.Name = "SkillIcon";
                    button_skillimage.Width = icon.Width;
                    button_skillimage.Height = icon.Height;
                    button_skillimage.FlatStyle = FlatStyle.Flat;
                    button_skillimage.BackgroundImage = icon;
                }
            }catch(Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.MainWindow);
            }



            if (!_CreateNewDefinition)
            {
                if (_SelectedSkill == null)
                    return;

                if (_SelectedCondition != null)
                {
                    BTN_ConditionEdit.Visible = true;
                    BTN_ContitionRemove.Visible = true;
                }

                BTN_Import.Visible = true;
                BTN_Export.Visible = true;
                BTN_Update.Visible = true;


                TB_SkillName.Text = _SelectedSkill.Name;

                ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == _SelectedSkill.Power.Name);

                CB_PowerSelection.SelectedItem = _selection;

                int index = CB_PowerSelection.Items.IndexOf(_selection);
                CB_PowerSelection.SelectedIndex = index;

                CB_SelectedRune.SelectedItem =
                    CB_SelectedRune.Items.OfType<ComboboxItem>()
                        .FirstOrDefault(x => x.Text == _SelectedSkill.SelectedRune.Name);

            }
            else
            {
                //string PowerName = A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(x => x.PowerSNO == _NewDefinitionPowerSNO).Name;

                //TB_SkillName.Text = PowerName;

                //ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
                //        .FirstOrDefault(x => x.Text == PowerName);

                //CB_PowerSelection.SelectedItem = _selection;

                //int index = CB_PowerSelection.Items.IndexOf(_selection);
                //CB_PowerSelection.SelectedIndex = index;

                //_CreateNewDefinition = false;
            }








            ///*-----old code-----------------------------*/

            //Panel_SelectedSkillDetails.Controls.Clear();

            //if (!_CreateNewDefinition)
            //{
            //    if (_SelectedSkill == null)
            //        return;

            //    if (_SelectedCondition != null)
            //    {
            //        BTN_ConditionEdit.Visible = true;
            //        BTN_ContitionRemove.Visible = true;
            //    }

            //    BTN_Import.Visible = true;
            //    BTN_Export.Visible = true;
            //    BTN_Update.Visible = true;
            //    Panel_ConditionEditor.Visible = true;
            //    TB_SkillName.Text = _SelectedSkill.Name;

            //    ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
            //            .FirstOrDefault(x => x.Text == _SelectedSkill.Power.Name);

            //    CB_PowerSelection.SelectedItem = _selection;

            //    int index = CB_PowerSelection.Items.IndexOf(_selection);
            //    CB_PowerSelection.SelectedIndex = index;

            //    CB_SelectedRune.SelectedItem =
            //        CB_SelectedRune.Items.OfType<ComboboxItem>()
            //            .FirstOrDefault(x => x.Text == _SelectedSkill.SelectedRune.Name);


            //    //-- Add Skill Image
            //    Image icon =
            //        Properties.Resources.ResourceManager.GetObject(_SelectedSkill.Power.Name.ToLower()) as Image;



            //    Button SkillIcon = new Button();
            //    SkillIcon.Name = "SkillIcon";
            //    SkillIcon.Width = icon.Width;
            //    SkillIcon.Height = icon.Height;
            //    SkillIcon.FlatStyle = FlatStyle.Flat;
            //    SkillIcon.BackgroundImage = icon;
            //    SkillIcon.FlatAppearance.BorderSize = 0;
            //    SkillIcon.Left = SkillIcon.Left + 10;

            //    Panel_SelectedSkillDetails.Controls.Add(SkillIcon);
            //    //
            //    //-- Add Info Label
            //    SkillPower PowerFromFile =
            //        A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(
            //            x => x.PowerSNO == _SelectedSkill.Power.PowerSNO);
            //    if (PowerFromFile != null)
            //    {
            //        Label Info = new Label();
            //        Info.Font = new Font(Window_Main._FontCollection.Families[0], (float) 8.3, FontStyle.Bold);
            //        Info.ForeColor = Color.Black;
            //        Info.AutoSize = true;
            //        Info.Top =
            //            Panel_SelectedSkillDetails.Controls.OfType<Button>()
            //                .FirstOrDefault(x => x.Name == "SkillIcon")
            //                .Top;
            //        Info.Left =
            //            Panel_SelectedSkillDetails.Controls.OfType<Button>()
            //                .FirstOrDefault(x => x.Name == "SkillIcon")
            //                .Right;


            //        if (PowerFromFile.IsCooldownSpell)
            //            Info.Text += "This is a COOLDOWN Skill!" + System.Environment.NewLine +
            //                         "Add Player_Skill_IsNotOnCooldown!" + System.Environment.NewLine;
            //        if (PowerFromFile.ResourceCost < -1 || PowerFromFile.ResourceCost > 0)
            //            Info.Text += "This Skill requires RESOURCE!" + System.Environment.NewLine +
            //                         "Add atleast Player_Skill_MinResource!" +
            //                         System.Environment.NewLine;

            //        Panel_SelectedSkillDetails.Controls.Add(Info);
            //    }





            //    //
            //    //-- load stored Conditions



            //    foreach (var condition in _SelectedSkill.CastConditions)
            //    {


            //        Button NewCondition = new Button();
            //        NewCondition.AutoSize = true;
            //        NewCondition.MouseClick += Condition_Click;
            //        NewCondition.MouseMove += Condition_onMouseMove;
            //        NewCondition.MouseDown += Condition_onMouseDown;
            //        NewCondition.MouseUp += Condition_onMouseUp;

            //        //-- add Tooltip to ConditonButtons which have a PowerSNO Value input
            //        //if (condition.ValueNames.Contains(ConditionValueName.PowerSNO))
            //        //{
            //        //    var defaultPower =
            //        //        A_Collection.Presets.SNOPowers.AllPowers.FirstOrDefault(
            //        //            x => x.Key == condition.Values.First());
            //        //    var customPower =
            //        //        A_Collection.Presets.SNOPowers.CustomPowerNames.FirstOrDefault(
            //        //            x => x.Key == condition.Values.First());

            //        //    string text = "";

            //        //    if (customPower.Value != null)
            //        //        text = customPower.Value;
            //        //    else if (defaultPower.Value != null)
            //        //        text = defaultPower.Value;

            //        //    ToolTip t = new ToolTip();
            //        //    t.SetToolTip(NewCondition, text);
            //        //}
            //        ToolTip t = new ToolTip();
            //        t.SetToolTip(NewCondition, condition.getSNOTooltipText());
            //        //

            //        NewCondition.Name = condition.ConditionGroup.ToString() + "|";

            //        for (int i = 0; i < condition.Values.Count(); i++)
            //        {
            //            NewCondition.Name += condition.Values[i].ToString() + "|";
            //        }

            //        NewCondition.Name = NewCondition.Name.TrimEnd('|');

            //        NewCondition.Text = condition.Type.ToString();

            //        //Set Bounds

            //        if (condition.ConditionGroup != -1)
            //        {
            //            var tryGetExisting =
            //                Panel_SelectedSkillDetails.Controls.OfType<Button>()
            //                    .Where(x => x.Name.Split('|')[0].Contains(condition.ConditionGroup.ToString()));

            //            if (tryGetExisting.Count() == 0)
            //            {
            //                NewCondition.Top = Panel_SelectedSkillDetails.Controls.OfType<Button>().Last().Bottom + 20;
            //                NewCondition.Left = Panel_SelectedSkillDetails.Controls.OfType<Button>().First().Left;

            //            }
            //            else
            //            {
            //                NewCondition.Top = tryGetExisting.Last().Top;
            //                NewCondition.Left = tryGetExisting.Last().Right;

            //                // Update Bounds if outside the panel

            //                if (
            //                    !Panel_SelectedSkillDetails.ClientRectangle.Contains(NewCondition.Right,
            //                        NewCondition.Top))
            //                {
            //                    NewCondition.Top = Panel_SelectedSkillDetails.Controls.OfType<Button>().Last().Bottom;
            //                    NewCondition.Left = Panel_SelectedSkillDetails.Controls.OfType<Button>().First().Left;
            //                }

            //                //
            //            }


            //        }
            //        else
            //        {
            //            NewCondition.Top = Panel_SelectedSkillDetails.Bottom - NewCondition.Height - 15;
            //            NewCondition.Left = Panel_SelectedSkillDetails.Left;
            //            NewCondition.BackColor = Color.LightGreen;
            //            NewCondition.FlatStyle = FlatStyle.Flat;


            //            Label DragMe = new Label();
            //            DragMe.AutoSize = true;
            //            DragMe.Font = new Font(Window_Main._FontCollection.Families[0], (float) 9, FontStyle.Bold);
            //            DragMe.Text = "Drag and Drop!";
            //            DragMe.ForeColor = Color.Green;
            //            DragMe.Left = NewCondition.Left + 5;
            //            DragMe.Top = NewCondition.Top - 15;


            //            Panel_SelectedSkillDetails.Controls.Add(DragMe);
            //        }
            //        //

            //        Panel_SelectedSkillDetails.Controls.Add(NewCondition);

            //    }
            //    //
            //    //-- set Group Split Labels
            //    var groups =
            //        Panel_SelectedSkillDetails.Controls.OfType<Button>()
            //            .Where(x => x.Name != "SkillIcon" && x.Name.Split('|')[0] != "-1")
            //            .GroupBy(x => int.Parse(x.Name.Split('|')[0]))
            //            .ToList();

            //    if (groups.Count() > 1)
            //    {
            //        for (int i = 0; i < groups.Count() - 1; i++)
            //        {
            //            var minLeft = groups[i].OrderByDescending(x => x.Left).Last().Left;
            //            var maxRight = groups[i].OrderByDescending(x => x.Right).First().Right;
            //            var minTop = groups[i].OrderByDescending(x => x.Top).Last().Top;
            //            var maxBottom = groups[i].OrderByDescending(x => x.Bottom).First().Bottom;

            //            Label OrSplit = new Label();
            //            OrSplit.AutoSize = true;
            //            OrSplit.BackColor = Color.Transparent;
            //            OrSplit.Text = "OR";
            //            OrSplit.Font = new Font(Window_Main._FontCollection.Families[0], (float) 8.25, FontStyle.Bold);
            //            OrSplit.Top = maxBottom + 3;
            //            OrSplit.Left = minLeft + ((maxRight - minLeft)/2) - 12;

            //            Panel_SelectedSkillDetails.Controls.Add(OrSplit);
            //        }
            //    }
            //    //
            //}
            //else
            //{
            //    string PowerName = A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(x => x.PowerSNO == _NewDefinitionPowerSNO).Name;

            //    //Populate_ComboBox_PowerSelection();

            //    TB_SkillName.Text = PowerName;

            //    ComboboxItem _selection = CB_PowerSelection.Items.OfType<ComboboxItem>()
            //            .FirstOrDefault(x => x.Text == PowerName);

            //    CB_PowerSelection.SelectedItem = _selection;

            //    int index = CB_PowerSelection.Items.IndexOf(_selection);
            //    CB_PowerSelection.SelectedIndex = index;

            //    _CreateNewDefinition = false;
            //}

        }


        private CastCondition global_castCondition_to_copy = null;

        private void Condition_Copy()
        {

            global_castCondition_to_copy = (CastCondition) listBox_conditions.SelectedItem;
        }

        private void Condition_Paste()
        {

            if (global_castCondition_to_copy != null)
            {

                int newGroupId = -1;

                if (listBox_conditionsets.Items.Count == 0)
                {
                    newGroupId = 0;
                }

                ConditionGroup conditionGroup = (ConditionGroup)listBox_conditionsets.SelectedItem;
                if (conditionGroup != null)
                {
                    newGroupId = conditionGroup.ConditionGroupValue;
                }

                CastCondition newCastCond = new CastCondition(newGroupId, global_castCondition_to_copy.Type, global_castCondition_to_copy.Values.ToArray(),
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(
                        x => x.Type == global_castCondition_to_copy.Type).ValueNames);

                newCastCond.enabled = global_castCondition_to_copy.enabled;
                newCastCond.comment = global_castCondition_to_copy.comment;

                _SelectedSkill.CastConditions.Add(newCastCond);

                Update_PanelSelectedSkillDetails();

                global_castCondition_to_copy = null;
            }
        }
      

        private void CB_PowerSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            ComboboxItem SelectedItem = CB_PowerSelection.SelectedItem as ComboboxItem;
            if (SelectedItem != null)
            {

                SkillPower Power = SelectedItem.Value as SkillPower;

                CB_SelectedRune.Items.Clear();

                foreach (var rune in Power.Runes)
                {
                    ComboboxItem NewItem = new ComboboxItem();
                    NewItem.Text = rune.Name;
                    NewItem.Value = rune;

                    CB_SelectedRune.Items.Add(NewItem);
                }

                CB_SelectedRune.SelectedIndex = 0;
                CB_PowerSelection.Update();
            }
        }


        private void BTN_Add_Click(object sender, EventArgs e)
        {
            SkillData_Add();
        }

        /// <summary>
        /// Add new SkillData to Listbox
        /// </summary>
        private void SkillData_Add()
        {
            if (TB_SkillName.Text.Length < 3)
            {
                MessageBox.Show("Name too short. Must contain atleast 3 chars!");
                return;
            }
            var tryGetEntry = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == TB_SkillName.Text);
            if (tryGetEntry != null)
            {
                MessageBox.Show("Name already exists. Please choose another!", TB_SkillName.Text);
                return;
            }

            ComboboxItem SelectedPower = CB_PowerSelection.SelectedItem as ComboboxItem;
            ComboboxItem SelectedRune = CB_SelectedRune.SelectedItem as ComboboxItem;

            string SkillDefinitionName = TB_SkillName.Text;
            SkillPower Power = SelectedPower.Value as SkillPower;
            Rune _Rune = SelectedRune.Value as Rune;

            SkillData newSkillData = new SkillData(Power, SkillDefinitionName, _Rune, new List<CastCondition>());

            SkillCastConditions.Custom.CustomDefinitions.Add(newSkillData);

            _SelectedSkill = null;
            TB_SkillName.Text = "";

            Update_View();

            //select new created item
            listBox_skills.SelectedIndex = listBox_skills.Items.IndexOf(newSkillData);
        }


        private void Skill_Copy()
        {
            //SkillData ToCopy =
            //    A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(
            //        x => x.Name == _clickedSkill.Text);

            //int counter = 0;

            //string newName = ToCopy.Name + "_" + counter.ToString();

            //while (true)
            //{
            //    if (A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(
            //        x => x.Name == newName) == null)
            //        break;

            //    counter++;

            //    newName = ToCopy.Name + "_" + counter.ToString();
            //}

            //SkillData NewData = new SkillData(ToCopy.Power, newName, ToCopy.SelectedRune, ToCopy.CastConditions);
            
            //A_Collection.SkillCastConditions.Custom.CustomDefinitions.Add(NewData);

            //Update_View();
        }


        private void BTN_Update_Click(object sender, EventArgs e)
        {
            
            SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _SelectedSkill.Name && x.Power.PowerSNO == _SelectedSkill.Power.PowerSNO);

            ComboboxItem SelectedPower = CB_PowerSelection.SelectedItem as ComboboxItem;
            ComboboxItem SelectedRune = CB_SelectedRune.SelectedItem as ComboboxItem;

            if (TB_SkillName.Text.Length < 3)
            {
                MessageBox.Show("Name too short. Must contain atleast 3 chars!");
                return;
            }
            
            _selected.Name = TB_SkillName.Text;
            _selected.Power = SelectedPower.Value as SkillPower;
            _selected.SelectedRune = SelectedRune.Value as Rune;

            Update_View();
        }


        private void BNT_DeleteSelection_Click(object sender, EventArgs e)
        {
            SkillData_Delete();
        }

        /// <summary>
        /// Delete selected SkillData
        /// </summary>
        private void SkillData_Delete()
        {
            if (_SelectedSkill != null)
            {
                SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _SelectedSkill.Name && x.Power.PowerSNO == _SelectedSkill.Power.PowerSNO);

                SkillCastConditions.Custom.CustomDefinitions.Remove(_selected);

                _SelectedSkill = null;

                Update_View();
            }
        }


        private ToolTip CB_ConditionSelection_tooltip = new ToolTip();

        private void CB_ConditionSelection_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            ComboBox _this = sender as ComboBox;
            ComboboxItem _selected = _this.SelectedItem as ComboboxItem;

            ConditionType _type = (ConditionType)_selected.Value;

            CastCondition _default = Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(x => x.Type == _type);





            //assign tooltip;
            string text = CB_ConditionSelection.GetItemText(_selected);
            string t_text = ConditionTypeHelper.getTooltip(text);
            CB_ConditionSelection_tooltip.SetToolTip(CB_ConditionSelection, t_text);



            Panel_ConditionEditor_Values.Controls.Clear();

            for(int i = 0; i < _default.Values.Count(); i++)
            {
                if (_default.ValueNames[i] != ConditionValueName.Bool)
                {
                    Label ValueName = new Label();
                    ValueName.Text = _default.ValueNames[i].ToString();
                    ValueName.Top = (Panel_ConditionEditor_Values.Top + 5) + (i*ValueName.Height);

                    Panel_ConditionEditor_Values.Controls.Add(ValueName);

                    TextBox Value = new TextBox();
                    Value.Text = _default.Values[i].ToString();
                    Value.Top = ValueName.Top;
                    Value.Left = ValueName.Right;

                    Panel_ConditionEditor_Values.Controls.Add(Value);
                }
                else
                {
                    CheckBox BoolValue = new CheckBox();
                    BoolValue.Top = (Panel_ConditionEditor_Values.Top + 5) + (i * BoolValue.Height);
                    BoolValue.Checked = true;
                    if (_default.Values[i] == 0)
                        BoolValue.Checked = false;

                    BoolValue.AutoSize = true;
                    BoolValue.CheckedChanged += BoolValue_CheckedChanged;

                    if (
                        _default.Type.ToString().Contains("StandStillTime") ||
                        _default.Type.ToString().Contains("MonstersInRange") || 
                        _default.Type.ToString().Contains("EliteInRange") || 
                        _default.Type.ToString().Contains("BossInRange") ||
                        _default.Type.ToString().Contains("RiftProgress") ||
                        _default.Type.ToString().Contains("PartyMember_InRange")
                        )
                    {
                        BoolValue.Text = "greater then or equal";

                        if (_default.Values[i] == 0)
                            BoolValue.Text = "less then or equal";
                    }

                    Panel_ConditionEditor_Values.Controls.Add(BoolValue);
                }
            }

            
        }

        private void BoolValue_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox _this = sender as CheckBox;

            if (_this.Text == "greater then or equal" || _this.Text == "less then or equal")
            {
                switch (_this.Text)
                {
                    case "greater then or equal":
                        _this.Text = "less then or equal";
                        break;

                    case "less then or equal":
                        _this.Text = "greater then or equal";
                        break;
                }
            }
        }

        private void Load_ConditionValues()
        {
            Panel_ConditionEditor_Values.Controls.Clear();

            checkBox_condition_enabled.Checked = _SelectedCondition.enabled;
            textBox_condition_comment.Text = _SelectedCondition.comment;


            for (int i = 0; i < _SelectedCondition.Values.Count(); i++)
            {
                if (_SelectedCondition.ValueNames[i] != ConditionValueName.Bool)
                {
                    Label ValueName = new Label();
                    ValueName.Text = _SelectedCondition.ValueNames[i].ToString();
                    ValueName.Top = (Panel_ConditionEditor_Values.Top + 5) + (i*ValueName.Height);

                    Panel_ConditionEditor_Values.Controls.Add(ValueName);

                    TextBox Value = new TextBox();
                    Value.Text = _SelectedCondition.Values[i].ToString();
                    Value.Top = ValueName.Top;
                    Value.Left = ValueName.Right;

                    //PowerSNO value to Text if possible
                    if(i == 0)
                    {
                        string powername =  A_Collection.Presets.SNOPowers.getPowerName((int)_SelectedCondition.Values[i]);
                        if(powername != null && powername.Length > 0)
                        {
                            Label SnoTextLabel = new Label();
                            SnoTextLabel.Text  = powername;
                            SnoTextLabel.Left  = Value.Right + 2;
                            SnoTextLabel.Top   = Value.Top;
                            SnoTextLabel.Width = 200;

                            Panel_ConditionEditor_Values.Controls.Add(SnoTextLabel);
                        }
                    }

                    Panel_ConditionEditor_Values.Controls.Add(Value);
                }
                else
                {
                    CheckBox BoolValue = new CheckBox();
                    BoolValue.Top = (Panel_ConditionEditor_Values.Top + 5) + (i * BoolValue.Height);
                    BoolValue.Checked = true;
                    if (_SelectedCondition.Values[i] == 0)
                        BoolValue.Checked = false;

                    BoolValue.AutoSize = true;
                    BoolValue.CheckedChanged += BoolValue_CheckedChanged;

                    if (
                        _SelectedCondition.Type.ToString().Contains("StandStillTime") || 
                        _SelectedCondition.Type.ToString().Contains("MonstersInRange") || 
                        _SelectedCondition.Type.ToString().Contains("EliteInRange") || 
                        _SelectedCondition.Type.ToString().Contains("BossInRange") ||
                        _SelectedCondition.Type.ToString().Contains("RiftProgress") ||
                        _SelectedCondition.Type.ToString().Contains("PartyMember_InRange")
                        )
                    {
                        BoolValue.Text = "greater then or equal";

                        if (_SelectedCondition.Values[i] == 0)
                            BoolValue.Text = "less then or equal";
                    }


                    Panel_ConditionEditor_Values.Controls.Add(BoolValue);

                }
            }
        }


        private void BTN_Condition_Add_Click(object sender, EventArgs e)
        {
            Condition_Add(-1); //-1 add to selected group;

        }

        /// <summary>
        /// p_new_group_id : -1 = add to current selected group
        /// </summary>
        /// <param name="p_new_group_id"></param>
        private void Condition_Add(int p_new_group_id)
        {
            try
            {
                if (!Validated_ConditionsInput())
                    return;

                //if (IsUnassignedConditionLeft())
                //    return;


                var EmptyValueFields = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().FirstOrDefault(x => x.Text.Length == 0);

                if (EmptyValueFields != null)
                { MessageBox.Show("Not all Conditions Values are set. Please enter a Value!"); return; }

                ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
                ConditionType Type = (ConditionType)_selected.Value;

                CastCondition _default = Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(x => x.Type == Type);

                var TextBoxes = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().ToList();
                var CheckBoxes = Panel_ConditionEditor_Values.Controls.OfType<CheckBox>().ToList();

                List<double> Values = new List<double>();

                if (CheckBoxes.Count == 0)
                {
                    for (int i = 0; i < _default.Values.Count(); i++)
                    {
                        Values.Add(double.Parse(TextBoxes[i].Text));
                    }
                }
                else
                {
                    if (_default.Type == ConditionType.Add_Property_Channeling)
                    {
                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }

                        Values.Add(double.Parse(TextBoxes[0].Text));


                    }
                    else if (_default.Type == ConditionType.Player_StandStillTime)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type == ConditionType.PartyMember_InRangeIsBuff || _default.Type == ConditionType.PartyMember_InRangeIsNotBuff)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));
                        Values.Add(double.Parse(TextBoxes[2].Text));
                        Values.Add(double.Parse(TextBoxes[3].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type == ConditionType.MonstersInRange_RiftProgress ||
                        _default.Type == ConditionType.SelectedMonster_MonstersInRange_RiftProgress ||
                        _default.Type == ConditionType.SelectedMonster_RiftProgress)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type != ConditionType.Add_Property_Channeling &&
                             (!_default.Type.ToString().Contains("MonstersInRange") ||
                              !_default.Type.ToString().Contains("EliteInRange") ||
                              !_default.Type.ToString().Contains("BossInRange")) && _default.ValueNames.Count() < 3)
                    {
                        for (int i = 0; i < _default.Values.Count(); i++)
                        {
                            if (CheckBoxes[i].Checked)
                                Values.Add(1);
                            else
                            {
                                Values.Add(0);
                            }
                        }
                    }
                    else
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                }



                //get current selected conditiongroup
                int newGroupId = -1;

                if (listBox_conditionsets.Items.Count == 0)
                {
                    newGroupId = 0;
                }

                ConditionGroup conditionGroup = (ConditionGroup)listBox_conditionsets.SelectedItem;
                if (conditionGroup != null)
                {
                    newGroupId = conditionGroup.ConditionGroupValue;
                }

                if(p_new_group_id > -1)
                {
                    newGroupId = p_new_group_id;
                }


                CastCondition newCastCondition = new CastCondition(newGroupId, Type, Values.ToArray(), _default.ValueNames);

                _SelectedSkill.CastConditions.Add(newCastCondition);


                Update_PanelSelectedSkillDetails();
                //Mark_SelectedCondition();


                //mark new group
                if (p_new_group_id > -1)
                {
                    listBox_conditionsets.SelectedIndex = listBox_conditionsets.Items.Count - 1;
                }

                //mark last=new entry
                listBox_conditions.SelectedIndex = listBox_conditions.Items.Count - 1;
            }
            catch(Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.MainWindow);
            }
        }






        private void BTN_ContitionRemove_Click(object sender, EventArgs e)
        {
            Condition_Delete();
        }

        /// <summary>
        /// delete selected condition
        /// </summary>
        private void Condition_Delete()
        {
            _SelectedSkill.CastConditions.Remove(_SelectedCondition);
            _SelectedCondition = null;

            Update_PanelSelectedSkillDetails();
        }


        //private bool IsUnassignedConditionLeft()
        //{
        //    var emptyGroup =
        //        Panel_SelectedSkillDetails.Controls.OfType<Button>()
        //            .FirstOrDefault(x => x.Name != "SkillIcon" && int.Parse(x.Name.Split('|')[0]) == -1);

        //    if (emptyGroup == null)
        //        return false;
        //    else
        //    {
        //        MessageBox.Show(
        //            "There is an unassigned Condition already. Assign it to a ConditionGroup before you add a new Condition");
        //    }

        //    return true;
        //}


        private bool Validated_ConditionsInput()
        {
            ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
            ConditionType Type = (ConditionType)_selected.Value;

            ConditionType TypeRequired;
            ConditionType[] TypesRequired;

            switch (Type)
            {
                case ConditionType.SelectedMonster_MinDistance:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MaxDistance:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_IsBuffActive:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_IsBuffNotActive:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.World_MonstersInRange:
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.Player_IsMonsterSelected:
                    return true;

                case ConditionType.PartyMember_InRangeIsBuff:
                    return true;

                case ConditionType.PartyMember_InRangeIsNotBuff:
                    return true;

                case ConditionType.PartyMember_InRangeMinHitpoints:
                    return true;

                case ConditionType.Party_AllInRange:
                    return true;

                case ConditionType.Party_AllAlive:
                    return true;

                case ConditionType.Party_NotAllInRange:
                    return true;

                case ConditionType.Player_BuffTicksLeft:
                    return true;

                case ConditionType.Player_IsBuffActive:
                    return true;

                case ConditionType.Player_IsBuffNotActive:
                    return true;

                case ConditionType.Player_IsBuffCount:
                    return true;

                case ConditionType.Player_IsNotBuffCount:
                    return true;

                case ConditionType.Player_MaxHitpointsPercentage:
                    return true;

                case ConditionType.Player_MinPrimaryResource:
                    return true;

                case ConditionType.Player_MinPrimaryResourcePercentage:
                    return true;

                case ConditionType.Player_MinSecondaryResource:
                    return true;

                case ConditionType.Player_MinSecondaryResourcePercentage:
                    return true;

                case ConditionType.Player_Skill_MinCharges:
                    return true;

                case ConditionType.Player_Skill_MinResource:
                    return true;

                case ConditionType.Player_Skill_IsNotOnCooldown:
                    return true;

                case ConditionType.SelectedMonster_IsBuffActive:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_IsBuffNotActive:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.World_BossInRange:
                    return true;

                case ConditionType.World_EliteInRange:
                    return true;

                case ConditionType.World_IsGRift:
                    return true;

                case ConditionType.World_IsRift:
                    return true;

                case ConditionType.Player_MaxPrimaryResource:
                    return true;

                case ConditionType.Player_MaxSecondaryResource:
                    return true;

                case ConditionType.Player_MaxPrimaryResourcePercentage:
                    return true;

                case ConditionType.Player_MaxSecondaryResourcePercentage:
                    return true;

                case ConditionType.Player_IsMoving:
                    return true;

                case ConditionType.Player_Pet_MinFetishesCount:
                    return true;

                case ConditionType.Player_Pet_MinZombieDogsCount:
                    return true;

                case ConditionType.Player_Pet_MinGargantuanCount:
                    return true;

                case ConditionType.Player_Pet_MaxFetishesCount:
                    return true;

                case ConditionType.Player_Pet_MaxZombieDogsCount:
                    return true;

                case ConditionType.Player_Pet_MaxGargantuanCount:
                    return true;

                case ConditionType.Player_Pet_MinSkeletalMageCount:
                    return true;

                case ConditionType.Player_Pet_MaxSkeletalMageCount:
                    return true;

                case ConditionType.SelectedMonster_IsElite:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_IsBoss:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_Power_IsNotOnCooldown:
                    return true;

                    case ConditionType.Player_Power_IsOnCooldown:
                        return true;

                    case ConditionType.Player_HasSkillEquipped:
                    return true;

                case ConditionType.MonstersInRange_HaveArcaneEnchanted:
                    TypesRequired = new ConditionType[] {ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange};
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveAvenger:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveDesecrator:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveElectrified:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveExtraHealth:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFast:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFrozen:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveHealthlink:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveIllusionist:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveJailer:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveKnockback:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveFirechains:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveMolten:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveMortar:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveNightmarish:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HavePlagued:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveReflectsDamage:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveShielding:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveTeleporter:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveThunderstorm:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveVortex:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.MonstersInRange_HaveWaller:
                    TypesRequired = new ConditionType[] { ConditionType.World_MonstersInRange, ConditionType.World_EliteInRange };
                    if (!IsConditionAvailable(TypesRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypesRequired[0].ToString() + " or " + TypesRequired[1].ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.Player_HasSkillNotEquipped:
                    return true;

                    case ConditionType.SelectedMonster_IsBuffCount:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_IsNotBuffCount:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_IsBuffCount:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_IsNotBuffCount:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_IsDestructableSelected:
                    return true;

                    case ConditionType.Key_ForceStandStill:
                    return true;

                    case ConditionType.Key_ForceMove:
                    return true;

                case ConditionType.Add_Property_TimedUse:
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_IsBuffActive:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_IsBuffNotActive:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_MinAPS:
                    return true;

                    case ConditionType.Add_Property_Channeling:
                    return true;

                    case ConditionType.Add_Property_APSSnapShot:
                    TypeRequired = ConditionType.Player_MinAPS;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_MinHitpointsPercentage:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.MonstersInRange_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_MonstersInRange_MinHitpointsPercentage:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.SelectedMonster_MinHitpointsPercentage:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MaxHitpointsPercentage:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                    case ConditionType.Player_StandStillTime:
                    return true;

                case ConditionType.MonstersInRange_RiftProgress:
                    TypeRequired = ConditionType.World_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_MonstersInRange_RiftProgress:
                    TypeRequired = ConditionType.SelectedMonster_MonstersInRange;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                case ConditionType.SelectedMonster_RiftProgress:
                    TypeRequired = ConditionType.Player_IsMonsterSelected;
                    if (!IsConditionAvailable(TypeRequired))
                    {
                        MessageBox.Show("Add a Conditions of Type (" + TypeRequired.ToString() +
                                        ") before you add this Condition!");
                        return false;
                    }
                    return true;

                default:
                    MessageBox.Show(
                        "No Validation Definition added for the selected Condition. Please send a BugReport!");
                    return false;
            }

        }

        private bool IsConditionAvailable(ConditionType Type)
        {
            //foreach (var button in Panel_SelectedSkillDetails.Controls.OfType<Button>().Where(x => x.Name != "SkillIcon"))
            //{
            //    ConditionType _Type = (ConditionType)Enum.Parse(typeof(ConditionType), button.Text.Split(' ').Last());

            //    if (_Type == Type)
            //        return true;
            //}
            //return false;

            foreach(CastCondition c in _SelectedSkill.CastConditions)
            {
                if (Type == c.Type)
                    return true;
            }
            return false;


        }

        private bool IsConditionAvailable(ConditionType[] Types)
        {
            //foreach (var button in Panel_SelectedSkillDetails.Controls.OfType<Button>().Where(x => x.Name != "SkillIcon"))
            //{
            //    ConditionType _Type = (ConditionType)Enum.Parse(typeof(ConditionType), button.Text.Split(' ').Last());

            //    if (Types.Contains(_Type))
            //        return true;
            //}
            //return false;


            foreach (CastCondition c in _SelectedSkill.CastConditions)
            {
                if (Types.Contains(c.Type))
                    return true;
            }
            return false;
        }


        private void BTN_ConditionEdit_Click(object sender, EventArgs e)
        {
            if (_SelectedCondition != null)
            {
                if (!Validated_ConditionsInput())
                    return;

                var EmptyValueFields =
                    Panel_ConditionEditor_Values.Controls.OfType<TextBox>().FirstOrDefault(x => x.Text.Length == 0);

                if (EmptyValueFields != null)
                {
                    MessageBox.Show("Not all Conditions Values are set. Please enter a Value!");
                    return;
                }

                ComboboxItem _selected = CB_ConditionSelection.SelectedItem as ComboboxItem;
                ConditionType Type = (ConditionType) _selected.Value;

                var _default =
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.FirstOrDefault(
                        x => x.Type == Type);

                var TextBoxes = Panel_ConditionEditor_Values.Controls.OfType<TextBox>().ToList();
                var CheckBoxes = Panel_ConditionEditor_Values.Controls.OfType<CheckBox>().ToList();

                List<double> Values = new List<double>();

                if (CheckBoxes.Count == 0)
                {
                    for (int i = 0; i < TextBoxes.Count(); i++)
                    {
                        Values.Add(double.Parse(TextBoxes[i].Text));
                    }
                }
                else
                {
                    if (_default.Type == ConditionType.Add_Property_Channeling)
                    {
                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }

                        Values.Add(double.Parse(TextBoxes[0].Text));


                    }
                    else if (_default.Type == ConditionType.PartyMember_InRangeIsBuff || _default.Type == ConditionType.PartyMember_InRangeIsNotBuff)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));
                        Values.Add(double.Parse(TextBoxes[2].Text));
                        Values.Add(double.Parse(TextBoxes[3].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type == ConditionType.Player_StandStillTime)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }




                    }
                    else if (_default.Type == ConditionType.MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_MonstersInRange_RiftProgress ||
                    _default.Type == ConditionType.SelectedMonster_RiftProgress)
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                    else if (_default.Type != ConditionType.Add_Property_Channeling && (!Type.ToString().Contains("MonstersInRange") || !Type.ToString().Contains("EliteInRange") || !Type.ToString().Contains("BossInRange")) && _default.ValueNames.Count() < 3)
                    {
                        for (int i = 0; i < _default.Values.Count(); i++)
                        {
                            if (i < CheckBoxes.Count)
                            {
                                if (CheckBoxes[i].Checked)
                                    Values.Add(1);
                                else
                                {
                                    Values.Add(0);
                                }
                            }
                            
                        }
                    }
                    else
                    {
                        Values.Add(double.Parse(TextBoxes[0].Text));
                        Values.Add(double.Parse(TextBoxes[1].Text));

                        if (CheckBoxes[0].Checked)
                            Values.Add(1);
                        else
                        {
                            Values.Add(0);
                        }
                    }
                }

                _SelectedCondition.Type = Type;
                _SelectedCondition.Values = Values.ToArray();
                _SelectedCondition.ValueNames =
                    A_Collection.Presets.DefaultCastConditions._Default_CastConditions.First(x => x.Type == Type)
                        .ValueNames;

                _SelectedCondition.enabled = checkBox_condition_enabled.Checked;
                _SelectedCondition.comment = textBox_condition_comment.Text;

                Update_PanelSelectedSkillDetails();
            }
        }
        
        private void CB_SelectedRune_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void BTN_Export_Click(object sender, EventArgs e)
        {
            if (Window_ImportExport._this == null)
            {
                Window_ImportExport IE = new Window_ImportExport();
                IE._SelectedSkill = _SelectedSkill;
                IE.isExport = true;
                IE.isImport = false;
                IE.Show();
            }
        }

        private void BTN_Import_Click(object sender, EventArgs e)
        {
            if (Window_ImportExport._this == null)
            {
                Window_ImportExport IE = new Window_ImportExport();
                IE._SelectedSkill = _SelectedSkill;
                IE.isExport = false;
                IE.isImport = true;
                IE.Closed += IE_AfterImport;
                IE.Show();
            }
        }

        private void IE_AfterImport(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.CustomSkillDefinitions.Fix();
            Update_PanelSelectedSkillDetails();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Window_ActivePowerView._this == null)
            {
                Window_ActivePowerView APV = new Window_ActivePowerView();
                APV.Show();
            }
        }




        private void listBox_conditionsets_SelectedIndexChanged(object sender, EventArgs e)
        {
            refresh_listbox_conditions();


            //select first item
            listBox_conditions_SelectedIndexChanged(this.listBox_conditions, null);
        }

        private void refresh_listbox_conditions()
        {


            ConditionGroup conditionGroup = (ConditionGroup)listBox_conditionsets.SelectedItem;
            if (conditionGroup != null)
            {
                conditionsBinding.DataSource = conditionGroup.CastConditions;
                listBox_conditions.DataSource = conditionsBinding;

                listBox_conditions.DisplayMember = "DisplayText";
                listBox_conditions.ValueMember = "Type";

                conditionsBinding.ResetBindings(false);
            }
            else
            {
                listBox_conditions.DataSource = null;
            }
        }




        private void listBox_conditions_SelectedIndexChanged(object sender, EventArgs e)
        {
            _SelectedCondition = (CastCondition)listBox_conditions.SelectedItem;

            if (_SelectedCondition != null)
            {
                var SelectionItem = CB_ConditionSelection.Items.OfType<ComboboxItem>()
                                    .ToList()
                                    .FirstOrDefault(x => x.Text == _SelectedCondition.Type.ToString());

                CB_ConditionSelection.SelectedItem = SelectionItem;

                Update_PanelSelectedSkillDetails(true);
                Load_ConditionValues();
                //Mark_SelectedCondition();
            }
        }


        private void listBox_skills_SelectedIndexChanged(object sender, EventArgs e)
        {
            //SkillData _selected = SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _this.Text && x.Power.PowerSNO == int.Parse(_this.Name));

            _SelectedSkill = (SkillData)listBox_skills.SelectedItem;
            _SelectedCondition = null;

            BTN_ConditionEdit.Visible = false;
            BTN_ContitionRemove.Visible = false;

            Update_View();

            //select first item in condition group listbox
            listBox_conditionsets_SelectedIndexChanged(this.listBox_conditionsets, null);
        }


        private void listBox_conditions_DrawItem(object sender, DrawItemEventArgs e)
        {
            try
            {
                if (e.Index >= 0)
                {
                    e.DrawBackground();

                    CastCondition item = listBox_conditions.Items[e.Index] as CastCondition;
                    if (item != null)
                    {
                        Brush brush = Brushes.Black;

                        if (!item.enabled)
                        {
                            CastCondition s = listBox_conditions.SelectedItem as CastCondition;
                            if (s != null && s.Equals(item))
                            {
                                brush = Brushes.White;
                            }
                            else
                            {
                                brush = Brushes.Gray;
                            }
                        }

                        e.Graphics.DrawString(
                            item.DisplayText,
                            listBox_conditions.Font,
                            brush,
                            e.Bounds
                        );
                    }
                }
            }
            catch (Exception ex)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(ex, A_Enums.ExceptionThread.MainWindow);
            }
        }
    }


}
