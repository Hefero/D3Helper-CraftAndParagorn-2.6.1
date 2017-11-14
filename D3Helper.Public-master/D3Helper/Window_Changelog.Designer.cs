namespace D3Helper
{
    partial class Window_Changelog
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window_Changelog));
            this.lb_changelog = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // lb_changelog
            // 
            this.lb_changelog.AutoSize = true;
            this.lb_changelog.Location = new System.Drawing.Point(12, 9);
            this.lb_changelog.Name = "lb_changelog";
            this.lb_changelog.Size = new System.Drawing.Size(35, 13);
            this.lb_changelog.TabIndex = 0;
            this.lb_changelog.Text = "label1";
            this.lb_changelog.Click += new System.EventHandler(this.lb_changelog_Click);
            // 
            // Window_Changelog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoScroll = true;
            this.ClientSize = new System.Drawing.Size(508, 261);
            this.Controls.Add(this.lb_changelog);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Window_Changelog";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Changelog";
            //this.Load += new System.EventHandler(this.Changelog_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lb_changelog;
    }
}