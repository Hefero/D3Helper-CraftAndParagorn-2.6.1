namespace D3Helper
{
    partial class Window_Outdated
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
            this.BTN_UpdateNow = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // BTN_UpdateNow
            // 
            this.BTN_UpdateNow.Location = new System.Drawing.Point(35, 31);
            this.BTN_UpdateNow.Name = "BTN_UpdateNow";
            this.BTN_UpdateNow.Size = new System.Drawing.Size(75, 23);
            this.BTN_UpdateNow.TabIndex = 0;
            this.BTN_UpdateNow.Text = "Update Now";
            this.BTN_UpdateNow.UseVisualStyleBackColor = true;
            this.BTN_UpdateNow.Click += new System.EventHandler(this.BTN_UpdateNow_Click);
            // 
            // Window_Outdated
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(165, 92);
            this.Controls.Add(this.BTN_UpdateNow);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Window_Outdated";
            this.Text = "Window_Outdated";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button BTN_UpdateNow;
    }
}