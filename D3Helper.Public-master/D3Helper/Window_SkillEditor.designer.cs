namespace D3Helper
{
    partial class Window_SkillEditor
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window_SkillEditor));
            this.Panel_Add_Edit_Skill = new System.Windows.Forms.Panel();
            this.button1 = new System.Windows.Forms.Button();
            this.BNT_DeleteSelection = new System.Windows.Forms.Button();
            this.BTN_Update = new System.Windows.Forms.Button();
            this.BTN_Add = new System.Windows.Forms.Button();
            this.LBL_Rune = new System.Windows.Forms.Label();
            this.CB_SelectedRune = new System.Windows.Forms.ComboBox();
            this.CB_PowerSelection = new System.Windows.Forms.ComboBox();
            this.LBL_Power = new System.Windows.Forms.Label();
            this.LBL_SkillName = new System.Windows.Forms.Label();
            this.TB_SkillName = new System.Windows.Forms.TextBox();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.BTN_Condition_Add = new System.Windows.Forms.Button();
            this.BTN_ConditionEdit = new System.Windows.Forms.Button();
            this.BTN_ContitionRemove = new System.Windows.Forms.Button();
            this.BTN_Export = new System.Windows.Forms.Button();
            this.BTN_Import = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.textBox_condition_comment = new System.Windows.Forms.TextBox();
            this.checkBox_condition_enabled = new System.Windows.Forms.CheckBox();
            this.Panel_ConditionEditor = new System.Windows.Forms.Panel();
            this.label4 = new System.Windows.Forms.Label();
            this.Panel_ConditionEditor_Values = new System.Windows.Forms.Panel();
            this.CB_ConditionSelection = new System.Windows.Forms.ComboBox();
            this.listBox_conditionsets = new System.Windows.Forms.ListBox();
            this.listBox_conditions = new System.Windows.Forms.ListBox();
            this.button_skillimage = new System.Windows.Forms.Button();
            this.listBox_skills = new System.Windows.Forms.ListBox();
            this.Panel_Add_Edit_Skill.SuspendLayout();
            this.Panel_ConditionEditor.SuspendLayout();
            this.SuspendLayout();
            // 
            // Panel_Add_Edit_Skill
            // 
            this.Panel_Add_Edit_Skill.BackColor = System.Drawing.Color.Transparent;
            this.Panel_Add_Edit_Skill.Controls.Add(this.button1);
            this.Panel_Add_Edit_Skill.Controls.Add(this.BNT_DeleteSelection);
            this.Panel_Add_Edit_Skill.Controls.Add(this.BTN_Update);
            this.Panel_Add_Edit_Skill.Controls.Add(this.BTN_Add);
            this.Panel_Add_Edit_Skill.Controls.Add(this.LBL_Rune);
            this.Panel_Add_Edit_Skill.Controls.Add(this.CB_SelectedRune);
            this.Panel_Add_Edit_Skill.Controls.Add(this.CB_PowerSelection);
            this.Panel_Add_Edit_Skill.Controls.Add(this.LBL_Power);
            this.Panel_Add_Edit_Skill.Controls.Add(this.LBL_SkillName);
            this.Panel_Add_Edit_Skill.Controls.Add(this.TB_SkillName);
            this.Panel_Add_Edit_Skill.Location = new System.Drawing.Point(12, 415);
            this.Panel_Add_Edit_Skill.Name = "Panel_Add_Edit_Skill";
            this.Panel_Add_Edit_Skill.Size = new System.Drawing.Size(281, 123);
            this.Panel_Add_Edit_Skill.TabIndex = 1;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(179, 93);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 23);
            this.button1.TabIndex = 12;
            this.button1.Text = "ActivePowerView";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // BNT_DeleteSelection
            // 
            this.BNT_DeleteSelection.FlatAppearance.BorderSize = 0;
            this.BNT_DeleteSelection.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BNT_DeleteSelection.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BNT_DeleteSelection.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BNT_DeleteSelection.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BNT_DeleteSelection.Image = global::D3Helper.Properties.Resources._120px_Crystal_128_trashcan_full;
            this.BNT_DeleteSelection.Location = new System.Drawing.Point(3, 86);
            this.BNT_DeleteSelection.Name = "BNT_DeleteSelection";
            this.BNT_DeleteSelection.Size = new System.Drawing.Size(30, 30);
            this.BNT_DeleteSelection.TabIndex = 7;
            this.toolTip1.SetToolTip(this.BNT_DeleteSelection, "Delete selected SkillDefinition");
            this.BNT_DeleteSelection.UseVisualStyleBackColor = true;
            this.BNT_DeleteSelection.Click += new System.EventHandler(this.BNT_DeleteSelection_Click);
            // 
            // BTN_Update
            // 
            this.BTN_Update.FlatAppearance.BorderSize = 0;
            this.BTN_Update.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_Update.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_Update.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Update.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Update.Image = global::D3Helper.Properties.Resources._0pvBq;
            this.BTN_Update.Location = new System.Drawing.Point(121, 86);
            this.BTN_Update.Name = "BTN_Update";
            this.BTN_Update.Size = new System.Drawing.Size(30, 30);
            this.BTN_Update.TabIndex = 6;
            this.toolTip1.SetToolTip(this.BTN_Update, "Updates current selected Skills Properties (Name , Power , Rune)");
            this.BTN_Update.UseVisualStyleBackColor = true;
            this.BTN_Update.Click += new System.EventHandler(this.BTN_Update_Click);
            // 
            // BTN_Add
            // 
            this.BTN_Add.FlatAppearance.BorderSize = 0;
            this.BTN_Add.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_Add.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Add.Image = global::D3Helper.Properties.Resources.add;
            this.BTN_Add.Location = new System.Drawing.Point(85, 86);
            this.BTN_Add.Name = "BTN_Add";
            this.BTN_Add.Size = new System.Drawing.Size(30, 30);
            this.BTN_Add.TabIndex = 2;
            this.toolTip1.SetToolTip(this.BTN_Add, "Adds a new SkillDefinition");
            this.BTN_Add.UseVisualStyleBackColor = true;
            this.BTN_Add.Click += new System.EventHandler(this.BTN_Add_Click);
            // 
            // LBL_Rune
            // 
            this.LBL_Rune.AutoSize = true;
            this.LBL_Rune.Location = new System.Drawing.Point(3, 62);
            this.LBL_Rune.Name = "LBL_Rune";
            this.LBL_Rune.Size = new System.Drawing.Size(33, 13);
            this.LBL_Rune.TabIndex = 5;
            this.LBL_Rune.Text = "Rune";
            // 
            // CB_SelectedRune
            // 
            this.CB_SelectedRune.FormattingEnabled = true;
            this.CB_SelectedRune.Location = new System.Drawing.Point(44, 59);
            this.CB_SelectedRune.Name = "CB_SelectedRune";
            this.CB_SelectedRune.Size = new System.Drawing.Size(184, 21);
            this.CB_SelectedRune.TabIndex = 4;
            this.CB_SelectedRune.SelectedIndexChanged += new System.EventHandler(this.CB_SelectedRune_SelectedIndexChanged);
            // 
            // CB_PowerSelection
            // 
            this.CB_PowerSelection.FormattingEnabled = true;
            this.CB_PowerSelection.Location = new System.Drawing.Point(44, 32);
            this.CB_PowerSelection.Name = "CB_PowerSelection";
            this.CB_PowerSelection.Size = new System.Drawing.Size(184, 21);
            this.CB_PowerSelection.TabIndex = 0;
            this.CB_PowerSelection.SelectedIndexChanged += new System.EventHandler(this.CB_PowerSelection_SelectedIndexChanged);
            // 
            // LBL_Power
            // 
            this.LBL_Power.AutoSize = true;
            this.LBL_Power.BackColor = System.Drawing.Color.Transparent;
            this.LBL_Power.Location = new System.Drawing.Point(3, 35);
            this.LBL_Power.Name = "LBL_Power";
            this.LBL_Power.Size = new System.Drawing.Size(37, 13);
            this.LBL_Power.TabIndex = 3;
            this.LBL_Power.Text = "Power";
            // 
            // LBL_SkillName
            // 
            this.LBL_SkillName.AutoSize = true;
            this.LBL_SkillName.Location = new System.Drawing.Point(3, 9);
            this.LBL_SkillName.Name = "LBL_SkillName";
            this.LBL_SkillName.Size = new System.Drawing.Size(35, 13);
            this.LBL_SkillName.TabIndex = 0;
            this.LBL_SkillName.Text = "Name";
            // 
            // TB_SkillName
            // 
            this.TB_SkillName.Location = new System.Drawing.Point(44, 6);
            this.TB_SkillName.Name = "TB_SkillName";
            this.TB_SkillName.Size = new System.Drawing.Size(184, 20);
            this.TB_SkillName.TabIndex = 2;
            // 
            // BTN_Condition_Add
            // 
            this.BTN_Condition_Add.FlatAppearance.BorderSize = 0;
            this.BTN_Condition_Add.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_Condition_Add.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_Condition_Add.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Condition_Add.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Condition_Add.Image = global::D3Helper.Properties.Resources.add;
            this.BTN_Condition_Add.Location = new System.Drawing.Point(136, 115);
            this.BTN_Condition_Add.Name = "BTN_Condition_Add";
            this.BTN_Condition_Add.Size = new System.Drawing.Size(30, 30);
            this.BTN_Condition_Add.TabIndex = 8;
            this.toolTip1.SetToolTip(this.BTN_Condition_Add, "Adds a new SkillDefinition");
            this.BTN_Condition_Add.UseVisualStyleBackColor = true;
            this.BTN_Condition_Add.Click += new System.EventHandler(this.BTN_Condition_Add_Click);
            // 
            // BTN_ConditionEdit
            // 
            this.BTN_ConditionEdit.FlatAppearance.BorderSize = 0;
            this.BTN_ConditionEdit.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_ConditionEdit.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_ConditionEdit.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ConditionEdit.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_ConditionEdit.Image = global::D3Helper.Properties.Resources._0pvBq;
            this.BTN_ConditionEdit.Location = new System.Drawing.Point(172, 115);
            this.BTN_ConditionEdit.Name = "BTN_ConditionEdit";
            this.BTN_ConditionEdit.Size = new System.Drawing.Size(30, 30);
            this.BTN_ConditionEdit.TabIndex = 8;
            this.toolTip1.SetToolTip(this.BTN_ConditionEdit, "Updates current selected Condition");
            this.BTN_ConditionEdit.UseVisualStyleBackColor = true;
            this.BTN_ConditionEdit.Click += new System.EventHandler(this.BTN_ConditionEdit_Click);
            // 
            // BTN_ContitionRemove
            // 
            this.BTN_ContitionRemove.FlatAppearance.BorderSize = 0;
            this.BTN_ContitionRemove.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_ContitionRemove.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_ContitionRemove.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_ContitionRemove.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_ContitionRemove.Image = global::D3Helper.Properties.Resources._120px_Crystal_128_trashcan_full;
            this.BTN_ContitionRemove.Location = new System.Drawing.Point(6, 115);
            this.BTN_ContitionRemove.Name = "BTN_ContitionRemove";
            this.BTN_ContitionRemove.Size = new System.Drawing.Size(30, 30);
            this.BTN_ContitionRemove.TabIndex = 8;
            this.toolTip1.SetToolTip(this.BTN_ContitionRemove, "Delete selected Condition");
            this.BTN_ContitionRemove.UseVisualStyleBackColor = true;
            this.BTN_ContitionRemove.Click += new System.EventHandler(this.BTN_ContitionRemove_Click);
            // 
            // BTN_Export
            // 
            this.BTN_Export.BackColor = System.Drawing.Color.Transparent;
            this.BTN_Export.FlatAppearance.BorderSize = 0;
            this.BTN_Export.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_Export.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_Export.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Export.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Export.Image = global::D3Helper.Properties.Resources.Upload;
            this.BTN_Export.Location = new System.Drawing.Point(780, 501);
            this.BTN_Export.Name = "BTN_Export";
            this.BTN_Export.Size = new System.Drawing.Size(30, 30);
            this.BTN_Export.TabIndex = 10;
            this.toolTip1.SetToolTip(this.BTN_Export, "Export Conditions of current selected Skill");
            this.BTN_Export.UseVisualStyleBackColor = false;
            this.BTN_Export.Click += new System.EventHandler(this.BTN_Export_Click);
            // 
            // BTN_Import
            // 
            this.BTN_Import.BackColor = System.Drawing.Color.Transparent;
            this.BTN_Import.FlatAppearance.BorderSize = 0;
            this.BTN_Import.FlatAppearance.MouseDownBackColor = System.Drawing.Color.Transparent;
            this.BTN_Import.FlatAppearance.MouseOverBackColor = System.Drawing.Color.Transparent;
            this.BTN_Import.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.BTN_Import.Font = new System.Drawing.Font("Microsoft Sans Serif", 20F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.BTN_Import.Image = global::D3Helper.Properties.Resources.Download;
            this.BTN_Import.Location = new System.Drawing.Point(816, 501);
            this.BTN_Import.Name = "BTN_Import";
            this.BTN_Import.Size = new System.Drawing.Size(30, 30);
            this.BTN_Import.TabIndex = 11;
            this.toolTip1.SetToolTip(this.BTN_Import, "Import Conditions from XML Text to current selected SkillDefinition");
            this.BTN_Import.UseVisualStyleBackColor = false;
            this.BTN_Import.Click += new System.EventHandler(this.BTN_Import_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(525, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 15;
            this.label1.Text = "Cast Conditions";
            this.toolTip1.SetToolTip(this.label1, "all cast conditions are connected by logical AND operator");
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(372, 19);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(104, 13);
            this.label2.TabIndex = 16;
            this.label2.Text = "Condition Groups";
            this.toolTip1.SetToolTip(this.label2, "All Condition Groups are connected with logical OR operator");
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(12, 19);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(37, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Skills";
            this.toolTip1.SetToolTip(this.label3, "All Condition Groups are connected with logical OR operator");
            // 
            // textBox_condition_comment
            // 
            this.textBox_condition_comment.Location = new System.Drawing.Point(3, 77);
            this.textBox_condition_comment.Name = "textBox_condition_comment";
            this.textBox_condition_comment.Size = new System.Drawing.Size(254, 20);
            this.textBox_condition_comment.TabIndex = 10;
            this.toolTip1.SetToolTip(this.textBox_condition_comment, "comment for this condition");
            // 
            // checkBox_condition_enabled
            // 
            this.checkBox_condition_enabled.AutoSize = true;
            this.checkBox_condition_enabled.Checked = true;
            this.checkBox_condition_enabled.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBox_condition_enabled.Location = new System.Drawing.Point(3, 30);
            this.checkBox_condition_enabled.Name = "checkBox_condition_enabled";
            this.checkBox_condition_enabled.Size = new System.Drawing.Size(120, 17);
            this.checkBox_condition_enabled.TabIndex = 11;
            this.checkBox_condition_enabled.Text = "condition is enabled";
            this.toolTip1.SetToolTip(this.checkBox_condition_enabled, "uncheck if this condition should be ignored");
            this.checkBox_condition_enabled.UseVisualStyleBackColor = true;
            // 
            // Panel_ConditionEditor
            // 
            this.Panel_ConditionEditor.BackColor = System.Drawing.Color.Transparent;
            this.Panel_ConditionEditor.Controls.Add(this.label4);
            this.Panel_ConditionEditor.Controls.Add(this.checkBox_condition_enabled);
            this.Panel_ConditionEditor.Controls.Add(this.textBox_condition_comment);
            this.Panel_ConditionEditor.Controls.Add(this.BTN_ContitionRemove);
            this.Panel_ConditionEditor.Controls.Add(this.BTN_ConditionEdit);
            this.Panel_ConditionEditor.Controls.Add(this.BTN_Condition_Add);
            this.Panel_ConditionEditor.Controls.Add(this.Panel_ConditionEditor_Values);
            this.Panel_ConditionEditor.Controls.Add(this.CB_ConditionSelection);
            this.Panel_ConditionEditor.Location = new System.Drawing.Point(372, 386);
            this.Panel_ConditionEditor.Name = "Panel_ConditionEditor";
            this.Panel_ConditionEditor.Size = new System.Drawing.Size(609, 152);
            this.Panel_ConditionEditor.TabIndex = 2;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 61);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(50, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "comment";
            // 
            // Panel_ConditionEditor_Values
            // 
            this.Panel_ConditionEditor_Values.Location = new System.Drawing.Point(260, 0);
            this.Panel_ConditionEditor_Values.Name = "Panel_ConditionEditor_Values";
            this.Panel_ConditionEditor_Values.Size = new System.Drawing.Size(349, 152);
            this.Panel_ConditionEditor_Values.TabIndex = 9;
            // 
            // CB_ConditionSelection
            // 
            this.CB_ConditionSelection.FormattingEnabled = true;
            this.CB_ConditionSelection.Location = new System.Drawing.Point(3, 3);
            this.CB_ConditionSelection.Name = "CB_ConditionSelection";
            this.CB_ConditionSelection.Size = new System.Drawing.Size(254, 21);
            this.CB_ConditionSelection.TabIndex = 8;
            this.CB_ConditionSelection.SelectedIndexChanged += new System.EventHandler(this.CB_ConditionSelection_SelectedIndexChanged);
            // 
            // listBox_conditionsets
            // 
            this.listBox_conditionsets.FormattingEnabled = true;
            this.listBox_conditionsets.Location = new System.Drawing.Point(375, 38);
            this.listBox_conditionsets.Name = "listBox_conditionsets";
            this.listBox_conditionsets.Size = new System.Drawing.Size(147, 342);
            this.listBox_conditionsets.TabIndex = 12;
            this.listBox_conditionsets.SelectedIndexChanged += new System.EventHandler(this.listBox_conditionsets_SelectedIndexChanged);
            // 
            // listBox_conditions
            // 
            this.listBox_conditions.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
            this.listBox_conditions.FormattingEnabled = true;
            this.listBox_conditions.Location = new System.Drawing.Point(528, 38);
            this.listBox_conditions.Name = "listBox_conditions";
            this.listBox_conditions.Size = new System.Drawing.Size(453, 342);
            this.listBox_conditions.TabIndex = 13;
            this.listBox_conditions.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.listBox_conditions_DrawItem);
            this.listBox_conditions.SelectedIndexChanged += new System.EventHandler(this.listBox_conditions_SelectedIndexChanged);
            // 
            // button_skillimage
            // 
            this.button_skillimage.Location = new System.Drawing.Point(305, 38);
            this.button_skillimage.Name = "button_skillimage";
            this.button_skillimage.Size = new System.Drawing.Size(64, 64);
            this.button_skillimage.TabIndex = 14;
            this.button_skillimage.UseVisualStyleBackColor = true;
            // 
            // listBox_skills
            // 
            this.listBox_skills.FormattingEnabled = true;
            this.listBox_skills.Location = new System.Drawing.Point(12, 38);
            this.listBox_skills.Name = "listBox_skills";
            this.listBox_skills.Size = new System.Drawing.Size(281, 368);
            this.listBox_skills.TabIndex = 17;
            this.listBox_skills.SelectedIndexChanged += new System.EventHandler(this.listBox_skills_SelectedIndexChanged);
            // 
            // Window_SkillEditor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = global::D3Helper.Properties.Resources.Background_Monk_50_SplitLine_Wide;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.ClientSize = new System.Drawing.Size(1184, 543);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox_skills);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.button_skillimage);
            this.Controls.Add(this.listBox_conditions);
            this.Controls.Add(this.listBox_conditionsets);
            this.Controls.Add(this.BTN_Import);
            this.Controls.Add(this.BTN_Export);
            this.Controls.Add(this.Panel_ConditionEditor);
            this.Controls.Add(this.Panel_Add_Edit_Skill);
            this.DoubleBuffered = true;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Window_SkillEditor";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "D3Helper - Skill Editor";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Panel_Add_Edit_Skill.ResumeLayout(false);
            this.Panel_Add_Edit_Skill.PerformLayout();
            this.Panel_ConditionEditor.ResumeLayout(false);
            this.Panel_ConditionEditor.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Panel Panel_Add_Edit_Skill;
        private System.Windows.Forms.Label LBL_SkillName;
        private System.Windows.Forms.TextBox TB_SkillName;
        private System.Windows.Forms.Label LBL_Rune;
        private System.Windows.Forms.ComboBox CB_SelectedRune;
        private System.Windows.Forms.ComboBox CB_PowerSelection;
        private System.Windows.Forms.Label LBL_Power;
        private System.Windows.Forms.Button BTN_Update;
        private System.Windows.Forms.Button BTN_Add;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button BNT_DeleteSelection;
        private System.Windows.Forms.Panel Panel_ConditionEditor;
        private System.Windows.Forms.ComboBox CB_ConditionSelection;
        private System.Windows.Forms.Panel Panel_ConditionEditor_Values;
        private System.Windows.Forms.Button BTN_Condition_Add;
        private System.Windows.Forms.Button BTN_ContitionRemove;
        private System.Windows.Forms.Button BTN_ConditionEdit;
        private System.Windows.Forms.Button BTN_Import;
        private System.Windows.Forms.Button BTN_Export;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListBox listBox_conditionsets;
        private System.Windows.Forms.ListBox listBox_conditions;
        private System.Windows.Forms.Button button_skillimage;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ListBox listBox_skills;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.CheckBox checkBox_condition_enabled;
        private System.Windows.Forms.TextBox textBox_condition_comment;
        private System.Windows.Forms.Label label4;
    }
}

