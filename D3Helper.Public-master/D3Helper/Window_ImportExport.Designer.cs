namespace D3Helper
{
    partial class Window_ImportExport
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Window_ImportExport));
            this.RTB_ImportExport = new System.Windows.Forms.RichTextBox();
            this.BTN_Copy = new System.Windows.Forms.Button();
            this.BTN_Load = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // RTB_ImportExport
            // 
            this.RTB_ImportExport.Location = new System.Drawing.Point(12, 12);
            this.RTB_ImportExport.Name = "RTB_ImportExport";
            this.RTB_ImportExport.Size = new System.Drawing.Size(311, 347);
            this.RTB_ImportExport.TabIndex = 0;
            this.RTB_ImportExport.Text = "";
            // 
            // BTN_Copy
            // 
            this.BTN_Copy.AutoSize = true;
            this.BTN_Copy.Location = new System.Drawing.Point(13, 372);
            this.BTN_Copy.Name = "BTN_Copy";
            this.BTN_Copy.Size = new System.Drawing.Size(100, 23);
            this.BTN_Copy.TabIndex = 1;
            this.BTN_Copy.Text = "Copy to Clipboard";
            this.BTN_Copy.UseVisualStyleBackColor = true;
            this.BTN_Copy.Click += new System.EventHandler(this.BTN_Copy_Click);
            // 
            // BTN_Load
            // 
            this.BTN_Load.AutoSize = true;
            this.BTN_Load.Location = new System.Drawing.Point(223, 372);
            this.BTN_Load.Name = "BTN_Load";
            this.BTN_Load.Size = new System.Drawing.Size(100, 23);
            this.BTN_Load.TabIndex = 2;
            this.BTN_Load.Text = "Load";
            this.BTN_Load.UseVisualStyleBackColor = true;
            this.BTN_Load.Click += new System.EventHandler(this.BTN_Load_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(119, 377);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "label1";
            // 
            // Window_ImportExport
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(335, 407);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.BTN_Load);
            this.Controls.Add(this.BTN_Copy);
            this.Controls.Add(this.RTB_ImportExport);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Window_ImportExport";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Window_ImportExport";
            this.Load += new System.EventHandler(this.Window_ImportExport_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox RTB_ImportExport;
        private System.Windows.Forms.Button BTN_Copy;
        private System.Windows.Forms.Button BTN_Load;
        private System.Windows.Forms.Label label1;
    }
}