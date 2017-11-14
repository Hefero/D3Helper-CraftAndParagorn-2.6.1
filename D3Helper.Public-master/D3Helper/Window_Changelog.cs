using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3Helper
{
    public partial class Window_Changelog : Form
    {
        public static Window_Changelog _this = null;

        public Window_Changelog()
        {
            

            this.Closed += Window_Changelog_Closed;
            InitializeComponent();
        }

        private void Window_Changelog_Closed(object sender, EventArgs e)
        {
            _this = null;
        }

        //private void Changelog_Load(object sender, EventArgs e)
        //{
        //    _this = this;

        //    this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
        //    this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

        //    this.VerticalScroll.Enabled = true;

        //    try
        //    {

        //        //List<string> allLogs = A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.GetChangelog, "") as List<string>;

        //        this.lb_changelog.Text = "Changelog" + Environment.NewLine + Environment.NewLine;

        //        //foreach (var entry in allLogs)
        //        //{
                    
        //        //    this.lb_changelog.Text += entry + Environment.NewLine + Environment.NewLine;
        //        //}


        //        this.Width = this.lb_changelog.Width;
        //    }
        //    catch { }
        //}

        private void lb_changelog_Click(object sender, EventArgs e)
        {

        }
    }
}
