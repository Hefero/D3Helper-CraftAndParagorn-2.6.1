using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using D3Helper.A_Collection;
using Newtonsoft.Json;

namespace D3Helper
{
    public partial class Window_ImportExport : Form
    {
        public SkillData _SelectedSkill;
        public bool isImport;
        public bool isExport;

        public static Window_ImportExport _this = null;

        public Window_ImportExport()
        {
            

            this.Closed += Window_ImportExport_Closed;
            InitializeComponent();
        }

        private void Window_ImportExport_Closed(object sender, EventArgs e)
        {
            _this = null;
        }

        private void Window_ImportExport_Load(object sender, EventArgs e)
        {
            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

            this.label1.AutoSize = true;
            this.label1.Visible = false;

            if (isExport)
            {
                this.RTB_ImportExport.ReadOnly = true;
                BTN_Load.Visible = false;

                List<CastCondition> CastConditions =
                    _SelectedSkill.CastConditions.Where(x => x.ConditionGroup != -1).ToList();

                string output = JsonConvert.SerializeObject(CastConditions);

                RTB_ImportExport.Text = output;
            }
            else if (isImport)
            {
                BTN_Copy.Visible = false;
                BTN_Load.Visible = false;
                    
                label1.Visible = true;
                label1.Text = "Paste in Data";

                RTB_ImportExport.TextChanged += Import_TextChanged;
            }
        }

        private void Import_TextChanged(object sender, EventArgs e)
        {
            label1.Text = "Press Load to Import";
            BTN_Load.Visible = true;
        }

        private void BTN_Copy_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Clipboard.SetText(RTB_ImportExport.Text);

            this.label1.Visible = true;
            this.label1.Text = "Data copied to Clipboard";
        }

        private void BTN_Load_Click(object sender, EventArgs e)
        {
            try
            {
                List<CastCondition> castConditions = new List<CastCondition>();

                castConditions = JsonConvert.DeserializeObject<List<CastCondition>>(RTB_ImportExport.Text);

                _SelectedSkill.CastConditions = castConditions;

                Window_SkillEditor._SelectedSkill.CastConditions = castConditions;

                this.Close();
            }
            catch
            {
                MessageBox.Show("Data is invalid! Please enter valid Data!");
            }
            
        }
    }
}
