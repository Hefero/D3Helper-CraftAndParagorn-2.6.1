namespace D3Helper
{
    partial class Window_ActivePowerView
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window_ActivePowerView));
            this.TV_PowerView = new System.Windows.Forms.TreeView();
            this.BTN_RefreshPowerView = new System.Windows.Forms.Button();
            this.BTN_PowerView_ExpandAll = new System.Windows.Forms.Button();
            this.TB_SelectedPowerName = new System.Windows.Forms.TextBox();
            this.BTN_SavePowerName = new System.Windows.Forms.Button();
            this.TB_PowerSNO = new System.Windows.Forms.TextBox();
            this.TB_AttribID = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TV_PowerView
            // 
            this.TV_PowerView.Location = new System.Drawing.Point(13, 64);
            this.TV_PowerView.Name = "TV_PowerView";
            this.TV_PowerView.Size = new System.Drawing.Size(587, 589);
            this.TV_PowerView.TabIndex = 0;
            // 
            // BTN_RefreshPowerView
            // 
            this.BTN_RefreshPowerView.Location = new System.Drawing.Point(13, 13);
            this.BTN_RefreshPowerView.Name = "BTN_RefreshPowerView";
            this.BTN_RefreshPowerView.Size = new System.Drawing.Size(75, 23);
            this.BTN_RefreshPowerView.TabIndex = 1;
            this.BTN_RefreshPowerView.Text = "Refresh";
            this.BTN_RefreshPowerView.UseVisualStyleBackColor = true;
            this.BTN_RefreshPowerView.Click += new System.EventHandler(this.BTN_RefreshPowerView_Click);
            // 
            // BTN_PowerView_ExpandAll
            // 
            this.BTN_PowerView_ExpandAll.Location = new System.Drawing.Point(94, 13);
            this.BTN_PowerView_ExpandAll.Name = "BTN_PowerView_ExpandAll";
            this.BTN_PowerView_ExpandAll.Size = new System.Drawing.Size(75, 23);
            this.BTN_PowerView_ExpandAll.TabIndex = 2;
            this.BTN_PowerView_ExpandAll.Text = "Expand All";
            this.BTN_PowerView_ExpandAll.UseVisualStyleBackColor = true;
            this.BTN_PowerView_ExpandAll.Click += new System.EventHandler(this.BTN_PowerView_ExpandAll_Click);
            // 
            // TB_SelectedPowerName
            // 
            this.TB_SelectedPowerName.Location = new System.Drawing.Point(294, 15);
            this.TB_SelectedPowerName.Name = "TB_SelectedPowerName";
            this.TB_SelectedPowerName.Size = new System.Drawing.Size(229, 20);
            this.TB_SelectedPowerName.TabIndex = 3;
            this.TB_SelectedPowerName.TextChanged += new System.EventHandler(this.TB_SelectedPowerName_TextChanged);
            // 
            // BTN_SavePowerName
            // 
            this.BTN_SavePowerName.Location = new System.Drawing.Point(525, 13);
            this.BTN_SavePowerName.Name = "BTN_SavePowerName";
            this.BTN_SavePowerName.Size = new System.Drawing.Size(75, 23);
            this.BTN_SavePowerName.TabIndex = 4;
            this.BTN_SavePowerName.Text = "Save";
            this.BTN_SavePowerName.UseVisualStyleBackColor = true;
            this.BTN_SavePowerName.Click += new System.EventHandler(this.BTN_SavePowerName_Click);
            // 
            // TB_PowerSNO
            // 
            this.TB_PowerSNO.Location = new System.Drawing.Point(185, 15);
            this.TB_PowerSNO.Name = "TB_PowerSNO";
            this.TB_PowerSNO.Size = new System.Drawing.Size(93, 20);
            this.TB_PowerSNO.TabIndex = 5;
            // 
            // TB_AttribID
            // 
            this.TB_AttribID.Location = new System.Drawing.Point(185, 41);
            this.TB_AttribID.Name = "TB_AttribID";
            this.TB_AttribID.Size = new System.Drawing.Size(93, 20);
            this.TB_AttribID.TabIndex = 6;
            // 
            // Window_ActivePowerView
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(612, 665);
            this.Controls.Add(this.TB_AttribID);
            this.Controls.Add(this.TB_PowerSNO);
            this.Controls.Add(this.BTN_SavePowerName);
            this.Controls.Add(this.TB_SelectedPowerName);
            this.Controls.Add(this.BTN_PowerView_ExpandAll);
            this.Controls.Add(this.BTN_RefreshPowerView);
            this.Controls.Add(this.TV_PowerView);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Window_ActivePowerView";
            this.Text = "Active Power View";
            this.Load += new System.EventHandler(this.Window_SkillEditor_Info_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TreeView TV_PowerView;
        private System.Windows.Forms.Button BTN_RefreshPowerView;
        private System.Windows.Forms.Button BTN_PowerView_ExpandAll;
        private System.Windows.Forms.TextBox TB_SelectedPowerName;
        private System.Windows.Forms.Button BTN_SavePowerName;
        private System.Windows.Forms.TextBox TB_PowerSNO;
        private System.Windows.Forms.TextBox TB_AttribID;
    }
}