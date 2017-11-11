using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Timers;
using System.IO;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Resources;
using System.Collections;
using System.Diagnostics;
using System.Threading;
using System.Runtime.InteropServices;
using System.Drawing.Text;
using System.Reflection;
using Enigma.D3;
using Enigma.Memory;
using D3Helper.A_Collection;
using D3Helper;
using D3Helper.A_WPFOverlay;


using SlimDX.DirectInput;
using D3Helper.A_Tools;
using D3Helper.A_Handler.AutoCube;

namespace D3Helper
{
    
    public partial class Window_Main : Form
    {
        public static Window_Main d3helperform;
        public static DateTime Start = DateTime.Now;
        public static PrivateFontCollection _FontCollection = new PrivateFontCollection();

        public static readonly Version SupportedVersion = Engine.SupportedVersion;

        System.Timers.Timer waitForOverlay;

        public bool isSimpleCastEnabled = false;
        public string selectedSimpleCastName = null;

        public Window_Main()
        {

            try
            {

                InitializeComponent();

                
                //-- attach Events
                this.FormClosed += FormClose;
                this.FormClosing += Form1_FormClosing;

                this.bt_update.Visible = false;

                d3helperform = this;

                //new Thread(delegate ()
                //{

                //    //wait till d3 is running
                //    while (!isDiabloRunning())
                //    {
                //        try
                //        {
                //            Thread.Sleep(1000);
                //        }
                //        catch { }
                //    }


                //    if (SupportedProcessVersion())
                //    {

                //        //-- Initialize Collector and Handler Thread
                //        if(!Program.SingleThreaded)
                //            A_Initialize.Th_ICollector.New_ICollector();
                //        A_Initialize.Th_Handler.New_Handler();
                //        //
                //        if (A_Tools.Version.AppVersion.isOutdated()) // !!!!! REENABLE THIS!!!!!!
                //        {
                //            Window_Outdated WO = new Window_Outdated();
                //            WO.ShowDialog();
                //        }

                //        System.Timers.Timer UpdateUI = new System.Timers.Timer(250);
                //        UpdateUI.Elapsed += RefreshUI;
                //        UpdateUI.Start();

                //        Load_CustomFonts();

                //    }else
                //    {
                //        this.Text = "You are running a not supported D3Client(" + GetFileVersion() +
                //                    ") Supported Version is " + SupportedVersion;
                //    }

                //}).Start();


            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.MainWindow);
            }
        }


        public bool SupportedProcessVersion()
        {
            Version fileVersion = GetFileVersion();

            if (fileVersion != null)
            {
                if (fileVersion.Major == SupportedVersion.Major
                    && fileVersion.Minor == SupportedVersion.Minor
                    && fileVersion.Build == SupportedVersion.Build
                )
                {
                    return true;
                }
            }

            return false;
        }


        private bool isDiabloRunning()
        {
            if(GetFileVersion() == null)
            {
                return false;
            }
            return true;
        }


        private static Version GetFileVersion()
        {
            var process = Process.GetProcessesByName("Diablo III")
                .FirstOrDefault();
            if (process != null)
            {

                var fileVersionInfo = process.MainModule.FileVersionInfo;
                return new Version(
                    fileVersionInfo.FileMajorPart,
                    fileVersionInfo.FileMinorPart,
                    fileVersionInfo.FileBuildPart,
                    fileVersionInfo.FilePrivatePart);
            }
            return default(Version);
        }


        private void Load_CustomFonts()
        {
            try
            {
                //Create your private font collection object.
                PrivateFontCollection pfc = new PrivateFontCollection();

                //Select your font from the resources.
                //My font here is "Digireu.ttf"
                int fontLength = Properties.Resources.EXL_____.Length;

                // create a buffer to read in to
                byte[] fontdata = Properties.Resources.EXL_____;

                // create an unsafe memory block for the font data
                System.IntPtr data = Marshal.AllocCoTaskMem(fontLength);

                // copy the bytes to the unsafe memory block
                Marshal.Copy(fontdata, 0, data, fontLength);

                // pass the font to the font collection
                pfc.AddMemoryFont(data, fontLength);

                // free up the unsafe memory
                Marshal.FreeCoTaskMem(data);

                _FontCollection = pfc;
            }
            catch (Exception)
            {
                
                throw;
            }
        }


        void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.D3Helper_MainForm_StartPosition = new Point(this.Left, this.Top);
            Properties.Settings.Default.Save();
        }
        
                        
        public static SlimDX.DirectInput.DirectInput directInput;
        public static SlimDX.DirectInput.Keyboard keyboard;
        public static SlimDX.DirectInput.Mouse mouse;

        private void Window_Main_Load(object sender, EventArgs e)
        {

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill1, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill2, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill3, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_Skill4, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_SkillLmb, new object[] { true });

            typeof(Button).InvokeMember("DoubleBuffered",
    BindingFlags.SetProperty | BindingFlags.Instance | BindingFlags.NonPublic,
    null, bt_SkillRmb, new object[] { true });


            directInput = new DirectInput();
            keyboard = new SlimDX.DirectInput.Keyboard(directInput);
            keyboard.Acquire();

            mouse = new SlimDX.DirectInput.Mouse(directInput);
            mouse.Acquire();


            Point d3helpermainwindowpos = Properties.Settings.Default.D3Helper_MainForm_StartPosition;
            var screens = Screen.AllScreens.OrderByDescending(x => x.Bounds.X);

            if (d3helpermainwindowpos.X >= screens.Last().Bounds.Left && d3helpermainwindowpos.X <= screens.First().Bounds.Right && d3helpermainwindowpos.Y <= screens.First().Bounds.Bottom && d3helpermainwindowpos.Y >= screens.First().Bounds.Top)
            {
                this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
                this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;
            }
            else
            {
                this.Top = 0;
                this.Left = 0;
            }


            //this.btn_donate.Image = new Bitmap(Properties.Resources.paypal_donate_button11, new Size(this.btn_donate.Width, this.btn_donate.Height));
            
            this.btn_info.Image = new Bitmap(Properties.Resources._480px_Info_icon_002_svg, new Size(this.btn_info.Width, this.btn_info.Height));
            this.btn_settings.Image = new Bitmap(Properties.Resources.pignon, new Size(this.btn_settings.Width, this.btn_settings.Height));




            DateTime latestOnlineVersion = A_Tools.Version.AppVersion.LatestOnlineVersion;
            DateTime currentVersion = A_Tools.Version.AppVersion.get_CurrentVersionDate();

            if (latestOnlineVersion > currentVersion)
            {
                this.lb_versionlb.Text = "New Version Available!" + System.Environment.NewLine + latestOnlineVersion.ToString("yy.MM.d.H");
                //this.lb_versionlb.Invoke((MethodInvoker)(() => this.lb_versionlb.Text = "New Version Available!"));
                this.bt_update.Visible = true;
            }
            else
            {
                this.lb_versionlb.Text = "";
                //this.lb_versionlb.Invoke((MethodInvoker)(() => this.lb_versionlb.Text = ""));
            }

            this.Text = "D3Helper - V" + A_Tools.Version.AppVersion.version;



            //Use SimpleCast!
            T_SimpleCast.StartSimpleCastThread();

            //fill combobox for simple cast
            loadSimeCastListToCombobox();






            //create thread waiting for d3 start!
            Thread t = new Thread(delegate ()
            {
                //wait till d3 is running
                while (!isDiabloRunning())
                {
                    try
                    {
                        Thread.Sleep(1000);
                    }
                    catch { }
                }

                if (SupportedProcessVersion())
                {

                    //-- Initialize Collector and Handler Thread
                    //if (!Program.SingleThreaded)
                    A_Initialize.Th_ICollector.New_ICollector();
                    A_Initialize.Th_Handler.New_Handler();


                    System.Timers.Timer UpdateUI = new System.Timers.Timer(1000);
                    UpdateUI.Elapsed += RefreshUI;
                    UpdateUI.Start();

                    Load_CustomFonts();
                }
                else
                {
                    this.Text = "You are running a not supported D3Client(" + GetFileVersion() +
                                ") Supported Version is " + SupportedVersion;

                }


            });
            t.SetApartmentState(ApartmentState.STA); //must be STA Thread to access GUI
            t.Start();




            //overlay
            if (SupportedProcessVersion())
            {
                A_WPFOverlay.Overlay o = new A_WPFOverlay.Overlay();
                o.Show();
            }
            try
            {
                if (Me.ParagonPointSpender.RosBotUpgradeKadalaThread.ThreadState == System.Threading.ThreadState.Unstarted)
                {
                    Me.ParagonPointSpender.RosBotUpgradeKadalaThread = new Thread(() =>
                    {
                        RosBotUpgradeKadala rosBotUpgradeKadala = new RosBotUpgradeKadala();
                    });
                    Me.ParagonPointSpender.RosBotUpgradeKadalaThread.Start();
                }
            }
            catch
            {
                Me.ParagonPointSpender.RosBotUpgradeKadalaThread = new Thread(() =>
                {
                    RosBotUpgradeKadala rosBotUpgradeKadala = new RosBotUpgradeKadala();
                });
                Me.ParagonPointSpender.RosBotUpgradeKadalaThread.Start();
            }
        }


        private void WaitForOverlay_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (SupportedProcessVersion())
            {
                A_WPFOverlay.Overlay o = new A_WPFOverlay.Overlay();
                o.Show();

                waitForOverlay.Stop();
            }
        }

        private void FormClose(Object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            System.Environment.Exit(1);
        }


        private void Populate_SkillButtonContextMenu()
        {
            try
            {
                List<Button> MainWindow_SkillButtons = new List<Button>()
                {
                    bt_Skill1,
                    bt_Skill2,
                    bt_Skill3,
                    bt_Skill4,
                    bt_SkillLmb,
                    bt_SkillRmb
                };
                List<SkillPower> AllSkillPowers = new List<SkillPower>()
                {
                    Skills.SkillInfos._HotBar1Skill,
                    Skills.SkillInfos._HotBar2Skill,
                    Skills.SkillInfos._HotBar3Skill,
                    Skills.SkillInfos._HotBar4Skill,
                    Skills.SkillInfos._HotBarLeftClickSkill,
                    Skills.SkillInfos._HotBarRightClickSkill
                };

                for (int i = 0; i < MainWindow_SkillButtons.Count; i++)
                {
                    Button _this = MainWindow_SkillButtons[i];
                    SkillPower AssignedPower = AllSkillPowers[i];

                    if (AssignedPower != null)
                    {
                        List<SkillData> AllDefinitions =
                            A_Collection.SkillCastConditions.Custom.CustomDefinitions.Where(
                                x => x.Power.PowerSNO == AssignedPower.PowerSNO).ToList();

                        ContextMenuStrip CMS = new ContextMenuStrip();


                        foreach (var definition in AllDefinitions)
                            {
                                ToolStripItem newItem = new ToolStripMenuItem();
                                newItem.Text = definition.Name;
                                newItem.Name = AssignedPower.PowerSNO.ToString();
                                newItem.AutoSize = true;

                                CMS.Items.Add(newItem);
                            }
                       ToolStripItem _newItem = new ToolStripMenuItem();
                        _newItem.Text = "Create New Definition";
                        _newItem.Name = AssignedPower.PowerSNO.ToString();
                        _newItem.AutoSize = true;

                            CMS.Items.Add(_newItem);

                        
                        CMS.ItemClicked += CMS_ItemClicked;

                        _this.ContextMenuStrip = CMS;
                    }
                }
            }
            catch (Exception)
            {
                
            }
        }


        private void CMS_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ToolStripItem _this = e.ClickedItem;

            SkillData _selected =
                A_Collection.SkillCastConditions.Custom.CustomDefinitions.FirstOrDefault(x => x.Name == _this.Text);

            if (_selected != null)
            {
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor._PreselectedDefinition = true;
                    Window_SkillEditor._SelectedSkill = _selected;
                    Window_SkillEditor Editor = new Window_SkillEditor();
                    Editor.Show();
                }
            }
            else
            {
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor._CreateNewDefinition = true;
                    Window_SkillEditor._NewDefinitionPowerSNO = int.Parse(_this.Name);
                    Window_SkillEditor Editor = new Window_SkillEditor();
                    Editor.Show();
                }
            }
        }


        private void RefreshUI(object sender, ElapsedEventArgs e)
        {
            try
            {
                List<Button> MainWindow_SkillButtons = new List<Button>()
                {
                    bt_Skill1,
                    bt_Skill2,
                    bt_Skill3,
                    bt_Skill4,
                    bt_SkillLmb,
                    bt_SkillRmb
                };
                List<SkillPower> AllSkillPowers = new List<SkillPower>()
                {
                    Skills.SkillInfos._HotBar1Skill,
                    Skills.SkillInfos._HotBar2Skill,
                    Skills.SkillInfos._HotBar3Skill,
                    Skills.SkillInfos._HotBar4Skill,
                    Skills.SkillInfos._HotBarLeftClickSkill,
                    Skills.SkillInfos._HotBarRightClickSkill
                };
                List<bool> AutoCastOverrides = new List<bool>()
                {
                    Me.AutoCastOverrides.AutoCast1Override,
                    Me.AutoCastOverrides.AutoCast2Override,
                    Me.AutoCastOverrides.AutoCast3Override,
                    Me.AutoCastOverrides.AutoCast4Override,
                    Me.AutoCastOverrides.AutoCastLMBOverride,
                    Me.AutoCastOverrides.AutoCastRMBOverride
                };

                for (int i = 0; i < MainWindow_SkillButtons.Count; i++)
                {
                    Button control = MainWindow_SkillButtons[i];
                    SkillPower power = AllSkillPowers[i];
                    bool autocastoverride = AutoCastOverrides[i];
                    Image SkillIcon = null;
                    if (power != null)
                        SkillIcon = Properties.Resources.ResourceManager.GetObject(power.Name.ToLower()) as Image;

                    //-- Set SkillIcon
                    if (power != null)
                    {
                        if(control.Image != SkillIcon)
                            control.Invoke((MethodInvoker) (() => control.Image = SkillIcon));
                    }
                    else
                    {
                        if (control.Image != null)
                            control.Invoke((MethodInvoker) (() => control.Image = null));

                    }
                    //
                    //-- Set BackColor
                    if (power != null)
                    {
                        if (autocastoverride)
                        {
                            if (control.BackColor != Color.Red)
                                control.Invoke((MethodInvoker) (() => control.BackColor = Color.Red));
                        }

                        else
                        {
                            if (control.BackColor != Color.Green)
                                control.Invoke((MethodInvoker) (() => control.BackColor = Color.Green));
                        }
                    }
                    else
                    {
                        if (control.BackColor != Color.Transparent)
                            control.Invoke((MethodInvoker) (() => control.BackColor = Color.Transparent));
                    }
                    //
                }

                Populate_SkillButtonContextMenu();
            }
            catch { }
        }
                
        private void bt_Skill1_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(0);
        }

        private void bt_Skill2_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(1);
        }

        private void bt_Skill3_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(2);
        }

        private void bt_Skill4_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(3);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (Window_Info._this == null)
            {
                Window_Info s = new Window_Info();
                s.Show();
            }
        }
        
        private void bt_hotkeys_Click(object sender, EventArgs e)
        {
            if (Window_Settings._this == null)
            {
                Window_Settings hf = new Window_Settings();
                hf.Show();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (Window_Changelog._this == null)
            {
                Window_Changelog cl = new Window_Changelog();
                cl.Show();
            }
        }

        private void bt_update_Click(object sender, EventArgs e)
        {
            Process.Start("http://d3helper.freeforums.net/board/3/releases");
        }

        private void bt_SkillRmb_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(4);
        }
        
        private void bt_SkillLmb_Click(object sender, EventArgs e)
        {
            A_Tools.T_ExternalFile.AutoCastOverrides.ChangeOverrides(5);
        }

        private void BTN_Forum_Click(object sender, EventArgs e)
        {
            Process.Start("http://d3helper.freeforums.net/");
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            Window_SkillEditor se = new Window_SkillEditor();
            se.Show();
        }

        private void comboBox_simplecast_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedSimpleCastName = comboBox_simplecast.Text;
        }

        private void checkBox_simpleCast_CheckedChanged(object sender, EventArgs e)
        {
            isSimpleCastEnabled = checkBox_simpleCast.Checked;
        }

        public void loadSimeCastListToCombobox()
        {
            List<string> simpleCasteNames = T_SimpleCast.getSimpleCastNames();
            T_SimpleCast.bindListToCombobox(comboBox_simplecast, simpleCasteNames);
            if (simpleCasteNames.Any())
            {
                selectedSimpleCastName = simpleCasteNames[0];
            }
        }

    }
}
