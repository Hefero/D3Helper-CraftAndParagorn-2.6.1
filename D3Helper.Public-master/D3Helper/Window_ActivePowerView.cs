using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Forms;

using D3Helper.A_Collector;
using D3Helper.A_Enums;
using Enigma.D3;
using Enigma.D3.Enums;

namespace D3Helper
{
    public partial class Window_ActivePowerView : Form
    {
       private static List<ActivePower> Buffer = new List<ActivePower>();
        private static int _SelectedPowerSNO;
        private static int _TVWitdhOffset = 45;
        private static int _TVHeightOffset = 115;

        public static Window_ActivePowerView _this = null;


        public Window_ActivePowerView()
        {
           

            this.Closed += Window_ActivePowerView_Closed;
            this.Load += Window_ActivePowerView_Load;
            this.Resize += Window_ActivePowerView_Resize;

            InitializeComponent();
                        
        }

        private void Window_ActivePowerView_Resize(object sender, EventArgs e)
        {
            TV_PowerView.Width = this.Width - _TVWitdhOffset;
            TV_PowerView.Height = this.Height - _TVHeightOffset;
        }

        private void Window_ActivePowerView_Load(object sender, EventArgs e)
        {
            _this = this;

            typeof(TreeView).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, TV_PowerView, new object[] { true });

            TV_PowerView.Width = this.Width - _TVWitdhOffset;
            TV_PowerView.Height = this.Height - _TVHeightOffset;
        }

        private void Window_ActivePowerView_Closed(object sender, EventArgs e)
        {

            Store_UpdatedPowers();
            _this = null;
        }

        public static void Get_LatestPowers()
        {
            //List<string> RawPowers = A_TCPClient.TCPClient.send_Instruction(TCPInstructions.GetPowers, "") as List<string>;

            //A_Collection.Presets.SNOPowers.CustomPowerNames = A_TCPClient.TCPClient.ConvertToCustomPowerNames(RawPowers);

            A_Collection.Presets.SNOPowers.CustomPowerNames = new Dictionary<int, string>();
        }

        private void Store_UpdatedPowers()
        {
            //List<string> RawPowers = A_TCPClient.TCPClient.send_Instruction(TCPInstructions.GetPowers, "") as List<string>;

            //var CleanedPowers = A_TCPClient.TCPClient.ConvertToCustomPowerNames(RawPowers);
            var CleanedPowers = new Dictionary<int, string>();

            List<string> Buffer = new List<string>();

            foreach (var power in A_Collection.Presets.SNOPowers.CustomPowerNames)
            {
                if (!CleanedPowers.ContainsKey(power.Key))
                    Buffer.Add(power.Key.ToString() + "\t" + power.Value);
            }

            //foreach (var tosend in Buffer)
            //{
            //    A_TCPClient.TCPClient.send_Instruction(TCPInstructions.StorePower, tosend);
            //}
        }


        private void Window_SkillEditor_Info_Load(object sender, EventArgs e)
        {
            Get_LatestPowers();

            TB_PowerSNO.Visible = false;
            TB_AttribID.Visible = false;
            TB_SelectedPowerName.Visible = false;
            BTN_SavePowerName.Visible = false;

            TV_PowerView.AfterSelect += onNodeSelection;
            
            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

            this.SetStyle(ControlStyles.OptimizedDoubleBuffer, true);

            

            Update_PowerView();
        }

        private void onNodeSelection(object sender, TreeViewEventArgs e)
        {
            TreeView _this = sender as TreeView;
            //TreeNode _parentselected = _this.Nodes.OfType<TreeNode>().First().Nodes.OfType<TreeNode>().FirstOrDefault(x => x.IsSelected);
            //TreeNode _subselected = _this.Nodes.OfType<TreeNode>().First().Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Nodes.OfType<TreeNode>().FirstOrDefault(y => y.IsSelected) != null);

            TreeNode _selected = null;

            var AllNodes = _this.Nodes.OfType<TreeNode>();

            foreach (var parentnode in AllNodes)
            {
                if (parentnode.IsSelected)
                {
                    _selected = parentnode;
                    break;
                }

                var childnodes = parentnode.Nodes.OfType<TreeNode>();

                foreach (var child in childnodes)
                {
                    if (child.IsSelected)
                    {
                        _selected = child;
                        break;
                    }

                    var _childnodes = child.Nodes.OfType<TreeNode>();

                    foreach (var _child in _childnodes)
                    {
                        if (_child.IsSelected)
                        {
                            _selected = _child;
                            break;
                        }

                        var __childnodes = _child.Nodes.OfType<TreeNode>();

                        foreach (var __child in __childnodes)
                        {
                            if (__child.IsSelected)
                            {
                                _selected = __child;
                                break;
                            }
                        }
                    }
                }

                if (_selected != null)
                    break;
            }

            if (_selected != null)
            {
                if (_selected.Text.Contains("PowerSNO"))
                {
                    int PowerSNO = int.Parse(_selected.Text.Split(' ')[1]);

                    if (!A_Collection.Presets.SNOPowers.CustomPowerNames.ContainsKey(PowerSNO))
                    {
                        TB_SelectedPowerName.Visible = true;
                        BTN_SavePowerName.Visible = true;

                        _SelectedPowerSNO = PowerSNO;

                        TB_SelectedPowerName.Text = getPowerName(PowerSNO);
                    }
                    else
                    {
                        TB_SelectedPowerName.Visible = false;
                        BTN_SavePowerName.Visible = false;
                    }
                }

                //-- Fille Selected PowerSNO / AttribID Textboxes

                if (_selected.Text.Contains("PowerSNO"))
                {
                    int PowerSNO = int.Parse(_selected.Text.Split(' ')[1]);

                    TB_PowerSNO.Visible = true;
                    TB_PowerSNO.Text = PowerSNO.ToString();
                    TB_AttribID.Visible = false;
                    
                }
                else if (_selected.Text.Contains("AttribID"))
                {
                    int PowerSNO = int.Parse(_selected.Parent.Text.Split(' ')[1]);
                    int AttribID = int.Parse(_selected.Text.Split(' ')[1]);

                    TB_PowerSNO.Visible = true;
                    TB_PowerSNO.Text = PowerSNO.ToString();

                    TB_AttribID.Visible = true;
                    TB_AttribID.Text = AttribID.ToString();
                }
                else
                {
                    TB_PowerSNO.Visible = false;
                    TB_AttribID.Visible = false;
                }

                //
            }
            else
            {
                TB_SelectedPowerName.Visible = false;
                BTN_SavePowerName.Visible = false;
            }
        }

        private string getPowerName(int PowerSNO)
        {
            return A_Collection.Presets.SNOPowers.getPowerName(PowerSNO);
        }
        
        private void Update_PowerView()
        {
            Dictionary<ActorCommonData,List<Monster_ActivePower>> MonstersPlayersInRangePowers = new Dictionary<ActorCommonData, List<Monster_ActivePower>>();
            List<ACD> InRange;
            lock (A_Collection.Environment.Actors.AllActors)
                InRange =
                    A_Collection.Environment.Actors.AllActors.Where(x => x.Distance <= 100).ToList();
            foreach (var acd in InRange)
            {
                if ((acd.IsMonster || acd._ACD.x17C_ActorType == ActorType.Player) && acd._ACD.x000_Id != A_Collection.Me.HeroGlobals.LocalACD.x000_Id)
                {
                    List<Monster_ActivePower> ActivePowers = A_Tools.T_ACD.get_MonsterActivePowers(acd._ACD);

                    MonstersPlayersInRangePowers.Add(acd._ACD, ActivePowers);
                }
            }


            List<ActivePower> AllPowers;
            lock(A_Collection.Me.HeroDetails.ActivePowers) AllPowers = A_Collection.Me.HeroDetails.ActivePowers.Where(x => A_Tools.T_LocalPlayer.PowerCollection.isBuffCount(x.AttribId) || A_Tools.T_LocalPlayer.PowerCollection.isBuffStartTick(x.AttribId) || A_Tools.T_LocalPlayer.PowerCollection.isBuffEndTick(x.AttribId) || A_Tools.T_LocalPlayer.PowerCollection.isSkillOverride(x.AttribId)).ToList();
            Dictionary<int, int> AllSkills;
            lock (A_Collection.Me.HeroDetails.ActiveSkills) AllSkills = A_Collection.Me.HeroDetails.ActiveSkills.ToDictionary(x => x.Key, y => y.Value);

            //-- adds Potion to EquippedSkills
            AllSkills.Add(30211, 0);
            //

            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.BeginUpdate()));

            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.Clear()));

            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.Add("ActivePowers")));
            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.Add("EquippedSkills")));
            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.Add("MonstersAndPlayersInRange")));

            //-- add Active Powers
            foreach (var power in AllPowers)
            {
                if (TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains(power.PowerSnoId.ToString())) == null)
                {
                    string powertext = getPowerName(power.PowerSnoId);

                    TreeNode newNode = new TreeNode();
                    newNode.Text = "PowerSNO: " + power.PowerSnoId.ToString() + " " + powertext;

                    if (Buffer.FirstOrDefault(x => x.PowerSnoId == power.PowerSnoId) == null ||
                        Buffer.FirstOrDefault(x => x.PowerSnoId == power.PowerSnoId && x.AttribId == power.AttribId) == null ||
                        Buffer.FirstOrDefault(x => x.PowerSnoId == power.PowerSnoId && x.AttribId == power.AttribId && x.Value == power.Value) == null)
                        newNode.ForeColor = Color.Green;

                    TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.Add(newNode)));
                }

                TreeNode newSubNode = new TreeNode();

                string powervalue = "Value";
                if (A_Tools.T_LocalPlayer.PowerCollection.isSkillOverride(power.AttribId))
                    powervalue = "SkillOverride";
                else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffCount(power.AttribId))
                    powervalue = "BuffCount";
                else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffStartTick(power.AttribId))
                    powervalue = "BuffStartTick";
                else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffEndTick(power.AttribId))
                    powervalue = "BuffEndTick";

                newSubNode.Text = "AttribID: " + power.AttribId.ToString() + " || " + powervalue + ": " + power.Value.ToString();

                if (Buffer.FirstOrDefault(x => x.PowerSnoId == power.PowerSnoId && x.AttribId == power.AttribId) == null ||
                    Buffer.FirstOrDefault(
                        x =>
                            x.PowerSnoId == power.PowerSnoId && x.AttribId == power.AttribId &&
                            x.Value == power.Value) == null)
                {
                    newSubNode.ForeColor = Color.Green;

                    TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains(power.PowerSnoId.ToString())).ForeColor = Color.Green));
                }

                TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains(power.PowerSnoId.ToString())).Nodes.Add(newSubNode)));
                
            }
            //
            //-- add Equipped Skill
            foreach (var power in AllSkills)
            {
                if (TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "EquippedSkills").Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains(power.Key.ToString())) == null)
                {
                    string powertext = A_Collection.Presets.SNOPowers.AllPowers.FirstOrDefault(x => x.Key == power.Key).Value;

                    TreeNode newNode = new TreeNode();
                    newNode.Text = "PowerSNO: " + power.Key.ToString() + " " + powertext;
                    
                    TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "EquippedSkills").Nodes.Add(newNode)));
                }
                
            }
            //
            //-- add SelectedMonster Active Powers
            foreach (var monster in MonstersPlayersInRangePowers)
            {
                TreeNode MonsterNode = new TreeNode();
                MonsterNode.Text = monster.Key.x004_Name;

                TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "MonstersAndPlayersInRange").Nodes.Add(MonsterNode)));

                foreach (var power in monster.Value)
                {
                    if (
                        MonsterNode.Nodes.OfType<TreeNode>()
                            .FirstOrDefault(x => x.Text.Contains(power.Modifier.ToString())) == null)
                    {
                        string powertext = getPowerName(power.Modifier);

                        TreeNode PowerNode = new TreeNode();
                        PowerNode.Text = "PowerSNO: " + power.Modifier.ToString() + " " + powertext;

                        MonsterNode.Nodes.Add(PowerNode);
                    }

                    TreeNode AttribNode = new TreeNode();
                    string powervalue = "Value";
                    if (A_Tools.T_LocalPlayer.PowerCollection.isSkillOverride(power.AttribID))
                        powervalue = "SkillOverride";
                    else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffCount(power.AttribID))
                        powervalue = "BuffCount";
                    else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffStartTick(power.AttribID))
                        powervalue = "BuffStartTick";
                    else if (A_Tools.T_LocalPlayer.PowerCollection.isBuffEndTick(power.AttribID))
                        powervalue = "BuffEndTick";

                    AttribNode.Text = "AttribID: " + power.AttribID.ToString() + " || " + powervalue + ": " + power.Value.ToString();

                    MonsterNode.Nodes.OfType<TreeNode>()
                        .FirstOrDefault(x => x.Text.Contains(power.Modifier.ToString()))
                        .Nodes.Add(AttribNode);


                }
            }
        
            //
            TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.EndUpdate()));

            Buffer = AllPowers;
        }

        private void Refresh_View()
        {
            List<string> ExpandedNodes = new List<string>();
            List<int> ExpandedSubNodes = new List<int>();
            
            var AllNodes = TV_PowerView.Nodes.OfType<TreeNode>().ToList();
            var AllSubNodes = TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.OfType<TreeNode>().ToList();
            

            foreach (var node in AllSubNodes)
            {
                int PowerSNO = int.Parse(node.Text.Split(' ')[1]);


                if (node.IsExpanded)
                    ExpandedSubNodes.Add(PowerSNO);
            }
            
            foreach (var node in AllNodes)
            {

                if (node.IsExpanded)
                    ExpandedNodes.Add(node.Text);
            }

            Update_PowerView();

            foreach (var index in ExpandedSubNodes)
            {
                var node = TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == "ActivePowers").Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text.Contains(index.ToString()));

                if (node != null)
                    TV_PowerView.Invoke((MethodInvoker)(() => node.Expand()));
            }
            
            foreach (var index in ExpandedNodes)
            {
                var node = TV_PowerView.Nodes.OfType<TreeNode>().FirstOrDefault(x => x.Text == index);

                if (node != null)
                    TV_PowerView.Invoke((MethodInvoker)(() => node.Expand()));
            }
        }
        private void BTN_RefreshPowerView_Click(object sender, EventArgs e)
        {
            Refresh_View();
        }

        private static bool AllExpanded = false;

        private void BTN_PowerView_ExpandAll_Click(object sender, EventArgs e)
        {
            if (!AllExpanded)
            {
                TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.ExpandAll()));

                AllExpanded = true;
            }
            else
            {
                TV_PowerView.Invoke((MethodInvoker)(() => TV_PowerView.CollapseAll()));

                AllExpanded = false;
            }

           
        }

        private void BTN_SavePowerName_Click(object sender, EventArgs e)
        {
            if (!A_Collection.Presets.SNOPowers.CustomPowerNames.ContainsKey(_SelectedPowerSNO))
            {
                DialogResult Result = MessageBox.Show(
                "You can set a custom name for the selected default Power's name. " +
                "The custom name will be stored on server and will be available for other users too, " +
                "so choose a readable and unique name!" + "\n\n" + "Do you rly want to save this name?", "", MessageBoxButtons.YesNo);

                if (Result == DialogResult.Yes)
                {
                    A_Collection.Presets.SNOPowers.CustomPowerNames.Add(_SelectedPowerSNO, TB_SelectedPowerName.Text);

                    Refresh_View();
                }
            }

        }

        private void TB_SelectedPowerName_TextChanged(object sender, EventArgs e)
        {
            
            
        }
    }
}
