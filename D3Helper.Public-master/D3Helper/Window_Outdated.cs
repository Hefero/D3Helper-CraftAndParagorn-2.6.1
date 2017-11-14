using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3Helper
{
    public partial class Window_Outdated : Form
    {
        public Window_Outdated()
        {
            InitializeComponent();

            this.Closed += Window_Outdated_Closed;
        }

        private void Window_Outdated_Closed(object sender, EventArgs e)
        {
            System.Environment.Exit(1);
        }

        private void BTN_UpdateNow_Click(object sender, EventArgs e)
        {
            Process.Start("http://d3helper.freeforums.net/board/3/releases");
            System.Environment.Exit(1);
        }
    }
}
