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
    
    public partial class Window_Info : Form
    {
        public static Window_Info _this = null;

        public Window_Info()
        {
           

            this.Closed += Window_Info_Closed;
            InitializeComponent();
        }

        private void Window_Info_Closed(object sender, EventArgs e)
        {
            _this = null;
        }

        private void SkillInfoForm_Load(object sender, EventArgs e)
        {
            _this = this;

            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

                        
        }
    }
}
