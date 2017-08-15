using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using D3Helper.A_Collector;
using D3Helper.A_Enums;
using D3Helper.A_Tools;
using Key = SlimDX.DirectInput.Key;
using KeyEventArgs = System.Windows.Forms.KeyEventArgs;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;

namespace D3Helper
{
    public partial class Window_Settings : Form
    {
        public static DataGridView Setup1 = new DataGridView();
        public static DataGridView Setup2 = new DataGridView();
        public static DataGridView Setup3 = new DataGridView();
        public static DataGridView Setup4 = new DataGridView();

        public static Window_Settings _this = null;

        public static List<UXControl> visible_uxcontrols = null;

        public Window_Settings()
        {
            InitializeComponent();

            this.FormClosed += FormClose;
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.SupportsTransparentBackColor, true);
        }

        private void FormClose(Object sender, FormClosedEventArgs e)
        {
            _this = null;
            A_WPFOverlay.Overlay.selected_uxcontrol = null;
        }

        private void Settings_Load(object sender, EventArgs e)
        {
            _this = this;

            Load_Setups();

            TabControl_ParagonPoints.BackColor = Color.Transparent;

            tabControl1.SelectedIndexChanged += TabControl1_SelectedIndexChanged;

            this.Top = Properties.Settings.Default.D3Helper_MainForm_StartPosition.Y;
            this.Left = Properties.Settings.Default.D3Helper_MainForm_StartPosition.X;

            // -----------
            this.tb_assignedSkill1.ReadOnly = true;
            this.tb_assignedSkill2.ReadOnly = true;
            this.tb_assignedSkill3.ReadOnly = true;
            this.tb_assignedSkill4.ReadOnly = true;
            this.tb_assignedSkillRMB.ReadOnly = true;
            this.tb_assignedSkillLMB.ReadOnly = true;
            
            
            this.tb_assignedParagonPoints1.ReadOnly = true;
            this.tb_assignedParagonPoints2.ReadOnly = true;
            this.tb_assignedParagonPoints3.ReadOnly = true;
            this.tb_assignedParagonPoints4.ReadOnly = true;
            this.tb_assignedAutoGambleHotkey.ReadOnly = true;

            this.tb_assignedAutoPick.ReadOnly = true;
            this.tb_assignedAutoCube_UpgradeRare.ReadOnly = true;
            this.tb_assignedAutoCube_ConvertMaterial.ReadOnly = true;

            this.tb_assignedSkill1.KeyDown += tb_assignedSkill1_KeyDown;
            this.tb_assignedSkill2.KeyDown += tb_assignedSkill2_KeyDown;
            this.tb_assignedSkill3.KeyDown += tb_assignedSkill3_KeyDown;
            this.tb_assignedSkill4.KeyDown += tb_assignedSkill4_KeyDown;
            this.tb_assignedSkillRMB.KeyDown += tb_assignedSkillRMB_KeyDown;
            this.tb_assignedSkillLMB.KeyDown += tb_assignedSkillLMB_KeyDown;

            this.tb_assignedParagonPoints1.KeyDown += tb_assignedParagonPoints1_KeyDown;
            this.tb_assignedParagonPoints2.KeyDown += tb_assignedParagonPoints2_KeyDown;
            this.tb_assignedParagonPoints3.KeyDown += tb_assignedParagonPoints3_KeyDown;
            this.tb_assignedParagonPoints4.KeyDown += tb_assignedParagonPoints4_KeyDown;
            this.tb_assignedAutoGambleHotkey.KeyDown += Tb_assignedAutoGambleHotkey_KeyDown;

            this.tb_assignedAutoPick.KeyDown += Tb_assignedAutoPick_KeyDown;
            this.tb_assignedAutoCube_UpgradeRare.KeyDown += Tb_assignedAutoCube_UpgradeRare_KeyDown;
            this.tb_assignedAutoCube_ConvertMaterial.KeyDown += Tb_assignedAutoCube_ConvertMaterial_KeyDown;

            this.tb_assignedSkill1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot1).Modifiers);
            this.tb_assignedSkill2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot2).Modifiers);
            this.tb_assignedSkill3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot3).Modifiers);
            this.tb_assignedSkill4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlot4).Modifiers);
            this.tb_assignedSkillRMB.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotRmb).Modifiers);
            this.tb_assignedSkillLMB.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeySlotLmb).Modifiers);

            this.tb_assignedParagonPoints1.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints1).Modifiers);
            this.tb_assignedParagonPoints2.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints2).Modifiers);
            this.tb_assignedParagonPoints3.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints3).Modifiers);
            this.tb_assignedParagonPoints4.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyParagonPoints4).Modifiers);
            this.tb_assignedAutoGambleHotkey.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoGamble).Modifiers);

            this.tb_assignedAutoPick.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoPick).Modifiers);
            this.tb_assignedAutoCube_UpgradeRare.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_UpgradeRare).Modifiers);
            this.tb_assignedAutoCube_ConvertMaterial.Text = get_HotkeyText(H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_ConvertMaterial).Key, H_Keyboard.get_HotkeyFromSettingsString(Properties.Settings.Default.HotkeyAutoCube_ConvertMaterial).Modifiers);

            this.cb_autopotion.Checked = Properties.Settings.Default.AutoPotionBool;
            this.tb_autopotionhpvalue.Text = Properties.Settings.Default.AutoPotionValue.ToString();

            this.cb_fps.Checked = Properties.Settings.Default.overlayfps;
            this.cb_xptracker.Checked = Properties.Settings.Default.overlayxptracker;
            this.cb_skillbuttons.Checked = Properties.Settings.Default.overlayskillbuttons;
            this.cb_riftprogressinrange.Checked = Properties.Settings.Default.overlayriftprogress;

            this.cb_autogamble.Checked = Properties.Settings.Default.AutoGambleBool;
            this.CB_ExtendedLogging.Checked = Properties.Settings.Default.Logger_extendedLog;
            this.cb_skillbuttonsastext.Checked = Properties.Settings.Default.overlayskillbuttonsastext;
            this.cb_conventionelements.Checked = Properties.Settings.Default.overlayconventiondraws;
            this.CB_ApsAndSnapShotAPs.Checked = Properties.Settings.Default.Overlay_APS;
            this.CB_EliteCircles.Checked = Properties.Settings.Default.Overlay_EliteCircles;

            this.tb_updaterate.Text = Properties.Settings.Default.D3Helper_UpdateRate.ToString();
            this.tb_riftprogressradius.Text = Properties.Settings.Default.riftprogress_radius.ToString();

            this.CB_AutoPick_Gem.Checked = Properties.Settings.Default.AutoPickSettings_Gem;
            this.CB_AutoPick_Material.Checked = Properties.Settings.Default.AutoPickSettings_Material;
            this.CB_AutoPick_Legendary.Checked = Properties.Settings.Default.AutoPickSettings_Legendary;
            this.CB_AutoPick_LegendaryAncient.Checked = Properties.Settings.Default.AutoPickSettings_LegendaryAncient;
            this.CB_AutoPick_GreaterRiftKeystone.Checked = Properties.Settings.Default.AutoPickSettings_GreaterRiftKeystone;
            this.CB_AutoPick_Whites.Checked = Properties.Settings.Default.AutoPickSettings_Whites;
            this.CB_AutoPick_Magics.Checked = Properties.Settings.Default.AutoPickSettings_Magics;
            this.CB_AutoPick_Rares.Checked = Properties.Settings.Default.AutoPickSettings_Rares;
            this.TB_AutoPick_PickupRadius.Text = Properties.Settings.Default.AutoPickSettings_PickupRadius.ToString();

            this.cb_DisableAutocastOnNoOverride.Checked = Properties.Settings.Default.DisableAutocastOnNoOverride;
            this.cbox_ConvertMaterialFromTo.Text = Properties.Settings.Default.ConvertMaterialText;

            //--------------------

            // load simpleCaste settings
            T_SimpleCast.load_to_datagrid(dataGridView_simpleCast);


        }

        private void Tb_assignedAutoCube_UpgradeRare_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedAutoCube_UpgradeRare.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoCube_UpgradeRare = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedAutoCube_ConvertMaterial_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedAutoCube_ConvertMaterial.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoCube_ConvertMaterial = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void Tb_assignedAutoPick_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedAutoPick.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoPick = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }


        private void Tb_assignedAutoGambleHotkey_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedAutoGambleHotkey.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyAutoGamble = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        private void TabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (tabControl1.SelectedIndex == 4)
            {
                tabControl1.SelectedIndex = 0;
                if (Window_SkillEditor._this == null)
                {
                    Window_SkillEditor SE = new Window_SkillEditor();
                    SE.Show();
                }
            }
        }

        void tb_assignedSkillLMB_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkillLMB.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlotLmb = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }

        void tb_assignedParagonPoints4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedParagonPoints4.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyParagonPoints4 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }

        void tb_assignedParagonPoints3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedParagonPoints3.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyParagonPoints3 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }

        void tb_assignedParagonPoints2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedParagonPoints2.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyParagonPoints2 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }

        void tb_assignedParagonPoints1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedParagonPoints1.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeyParagonPoints1 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }


        private string get_HotkeyText(Key key, List<Key> modifiers)
        {
            string text = "";

            foreach (var modifier in modifiers)
            {
                if (modifier == Key.LeftControl)
                    text += "CTRL+";
                if (modifier == Key.LeftAlt)
                    text += "ALT+";
                if (modifier == Key.LeftShift)
                    text += "SHIFT+";
            }
            text += key.ToString();

            return text;
        }
        void tb_assignedSkillRMB_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkillRMB.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlotRmb = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        

        void tb_assignedSkill4_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkill4.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlot4 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        void tb_assignedSkill3_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkill3.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlot3 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        void tb_assignedSkill2_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkill2.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlot2 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();
        }

        void tb_assignedSkill1_KeyDown(object sender, KeyEventArgs e)
        {
            Keys Key = e.KeyCode;

            SlimDX.DirectInput.Key key = A_Tools.InputSimulator.IS_Keyboard.convert_KeysToKey(Key);

            List<SlimDX.DirectInput.Key> Modifiers = new List<Key>();

            if (e.Alt)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftAlt);
            if (e.Control)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftControl);
            if (e.Shift)
                Modifiers.Add(SlimDX.DirectInput.Key.LeftShift);

            this.tb_assignedSkill1.Text = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.HotkeySlot1 = get_HotkeyText(key, Modifiers);
            Properties.Settings.Default.Save();

        }

        private void cb_autopotion_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPotionBool = this.cb_autopotion.Checked;
            Properties.Settings.Default.Save();
        }

        private void tb_autopotionhpvalue_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(this.tb_autopotionhpvalue.Text, out value))
            {
                if (value > 0 && value <= 100)
                {
                    Properties.Settings.Default.AutoPotionValue = value;
                    Properties.Settings.Default.Save();
                }
            }
        }


        private void cb_fps_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayfps = this.cb_fps.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_xptracker_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayxptracker = this.cb_xptracker.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_skillbuttons_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_skillbuttonsastext.Checked)
                cb_skillbuttonsastext.Checked = false;

            Properties.Settings.Default.overlayskillbuttons = this.cb_skillbuttons.Checked;
            Properties.Settings.Default.Save();
        }
        
        private void bt_delete_hotkey_skillslot1_Click(object sender, EventArgs e)
        {

            if (this.tb_assignedSkill1.Text.Length > 1)
            {
                this.tb_assignedSkill1.Text = "";
                Properties.Settings.Default.HotkeySlot1 = "";
                Properties.Settings.Default.Save();
            }
        }

        private void bt_delete_hotkey_skillslot2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill2.Text.Length > 1)
            {
                this.tb_assignedSkill2.Text = "";
                Properties.Settings.Default.HotkeySlot2 = "";
                Properties.Settings.Default.Save();
            }
        }

        private void bt_delete_hotkey_skillslot3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill3.Text.Length > 1)
            {
                this.tb_assignedSkill3.Text = "";
                Properties.Settings.Default.HotkeySlot3 = "";
                Properties.Settings.Default.Save();
            }
        }

        private void bt_delete_hotkey_skillslot4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkill4.Text.Length > 1)
            {
                this.tb_assignedSkill4.Text = "";
                Properties.Settings.Default.HotkeySlot4 = "";
                Properties.Settings.Default.Save();
            }
        }

        private void bt_delete_hotkey_skillslotrmb_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillRMB.Text.Length > 1)
            {
                this.tb_assignedSkillRMB.Text = "";
                Properties.Settings.Default.HotkeySlotRmb = "";
                Properties.Settings.Default.Save();
            }
        }

        private void tb_assignedSkill1_TextChanged(object sender, EventArgs e)
        {

        }

        private void cb_autogamble_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoGambleBool = this.cb_autogamble.Checked;
            Properties.Settings.Default.Save();
        }

        private void bt_delete_hotkey_paragonpoints1_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints1.Text.Length > 1)
            {
                this.tb_assignedParagonPoints1.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints1 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints2_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints2.Text.Length > 1)
            {
                this.tb_assignedParagonPoints2.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints2 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints3_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints3.Text.Length > 1)
            {
                this.tb_assignedParagonPoints3.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints3 = "";
                Properties.Settings.Default.Save();

            }
        }

        private void bt_delete_hotkey_paragonpoints4_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedParagonPoints4.Text.Length > 1)
            {
                this.tb_assignedParagonPoints4.Text = "";
                Properties.Settings.Default.HotkeyParagonPoints4 = "";
                Properties.Settings.Default.Save();

            }
        }



        private void bt_delete_hotkey_skillslotlmb_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedSkillLMB.Text.Length > 1)
            {
                this.tb_assignedSkillLMB.Text = "";
                Properties.Settings.Default.HotkeySlotLmb = "";
                Properties.Settings.Default.Save();

            }
        }

        private void tb_updaterate_TextChanged(object sender, EventArgs e)
        {
            int value;
            if (int.TryParse(this.tb_updaterate.Text, out value))
            {
                if (value >= 1 && value <= 60)
                {
                    Properties.Settings.Default.D3Helper_UpdateRate = value;
                    Properties.Settings.Default.Save();
                }
                else
                {
                    MessageBox.Show("Wrong Value! Min: 1 Max: 60");
                }
            }
        }

        private void cb_riftprogressinrange_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayriftprogress = this.cb_riftprogressinrange.Checked;
            Properties.Settings.Default.Save();
        }

        private void tb_riftprogressradius_TextChanged(object sender, EventArgs e)
        {
            double radius;
            if (double.TryParse(tb_riftprogressradius.Text, out radius))
            {
                if (radius > 0 && radius <= 100)
                {
                    Properties.Settings.Default.riftprogress_radius = radius;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void tb_assignedParagonPoints4_TextChanged(object sender, EventArgs e)
        {

        }

        private void bt_delete_hotkey_autogamble_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoGambleHotkey.Text.Length > 1)
            {
                this.tb_assignedAutoGambleHotkey.Text = "";
                Properties.Settings.Default.HotkeyAutoGamble = "";
                Properties.Settings.Default.Save();

            }
        }

        private void CB_ExtendedLogging_CheckedChanged(object sender, EventArgs e)
        {

            Properties.Settings.Default.Logger_extendedLog = this.CB_ExtendedLogging.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_skillbuttonsastext_CheckedChanged(object sender, EventArgs e)
        {
            if (cb_skillbuttons.Checked)
                cb_skillbuttons.Checked = false;

            Properties.Settings.Default.overlayskillbuttonsastext = this.cb_skillbuttonsastext.Checked;
            Properties.Settings.Default.Save();
        }

        private void cb_conventionelements_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.overlayconventiondraws = this.cb_conventionelements.Checked;
            Properties.Settings.Default.Save();
        }

        private void BTN_ParagonPointsNew_Click(object sender, EventArgs e)
        {
            if (this.TB_ParagonPointsSetupName.Text.Length < 1)
            {
                MessageBox.Show("Please enter a name for the new Setup");
                return;
            }

            if (this.TabControl_ParagonPoints.TabPages.Count == 4)
            {
                MessageBox.Show("You can not add more then 4 Setups per Hero");
                return;
            }

            this.TabControl_ParagonPoints.TabPages.Add(this.TB_ParagonPointsSetupName.Text, this.TB_ParagonPointsSetupName.Text);

            Populate_ParagonPoints_DefaultValues(TabControl_ParagonPoints.TabPages.OfType<TabPage>().Last());
        }

        private static List<string> ComboboxContent = new List<string>() { "core0", "core1", "core2", "core3", "offense0", "offense1", "offense2", "offense3", "defense0", "defense1", "defense2", "defense3", "utility0", "utility1", "utility2", "utility3" };

        private void Populate_ParagonPoints_DefaultValues(TabPage _this)
        {
            _this.BackColor = Color.Transparent;

            DataGridView DG = new DataGridView();
            DG.Width = _this.Width;
            DG.Height = _this.Height;
            DG.CellEndEdit += DG_CellEndEdit; ;

            DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
            comboBox.Name = "Property";
            comboBox.DataSource = ComboboxContent;

            DataGridViewTextBoxColumn value = new DataGridViewTextBoxColumn();
            value.Name = "Value";

            DataGridViewTextBoxColumn maxvalue = new DataGridViewTextBoxColumn();
            maxvalue.Name = "MaxValue";
            maxvalue.ReadOnly = true;

            DG.Columns.Add(comboBox);
            DG.Columns.Add(value);
            DG.Columns.Add(maxvalue);

            _this.Controls.Add(DG);
        }

        private void DG_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            DataGridView _this = sender as DataGridView;

            if (_this.CurrentCellAddress.X == 0)
            {
                DataGridViewComboBoxCell ComboBox = _this.Rows[e.RowIndex].Cells["Property"] as DataGridViewComboBoxCell;
                _this.Rows[e.RowIndex].Cells["MaxValue"].Value = Load_MaxValue(ComboBox.Value as string);

            }

            if (_this.CurrentCellAddress.X == 1)
            {
                DataGridViewTextBoxCell TextBox = _this.Rows[e.RowIndex].Cells["Value"] as DataGridViewTextBoxCell;

                var type = _this.Rows[e.RowIndex].Cells["Property"] as DataGridViewComboBoxCell;

                int maxvalue = Load_MaxValue(type.Value as string);

                int value = int.Parse(TextBox.Value as string);

                if (value < -1)
                    TextBox.Value = "-1";
                else if (value > maxvalue && maxvalue != -1)
                    TextBox.Value = maxvalue.ToString();
            }
        }

        private int Load_MaxValue(string PropertyType)
        {
            switch (PropertyType)
            {
                case "core0":
                case "core1":
                    return -1;
                case "core2":
                case "core3":
                case "offense0":
                case "offense1":
                case "offense2":
                case "offense3":
                case "defense0":
                case "defense1":
                case "defense2":
                case "defense3":
                case "utility0":
                case "utility1":
                case "utility2":
                case "utility3":
                    return 50;

                default:
                    return 0;
            }
        }

        private void Load_Setups()
        {
            try
            {
                TabControl_ParagonPoints.TabPages.Clear();

                if (A_Collection.Me.ParagonPointSpender.Setups.ContainsKey(A_Collection.Me.HeroGlobals.HeroID))
                {
                    var Setups = A_Collection.Me.ParagonPointSpender.Setups[A_Collection.Me.HeroGlobals.HeroID];

                    foreach (var setup in Setups)
                    {
                        TabPage page = new TabPage();
                        page.Name = setup.Name;
                        page.Text = setup.Name;
                        page.BackColor = Color.Transparent;

                        DataGridView DG = new DataGridView();
                        DG.Width = _this.Width;
                        DG.Height = _this.Height;
                        DG.CellEndEdit += DG_CellEndEdit;

                        DataGridViewComboBoxColumn comboBox = new DataGridViewComboBoxColumn();
                        comboBox.Name = "Property";
                        comboBox.DataSource = ComboboxContent;

                        DataGridViewTextBoxColumn value = new DataGridViewTextBoxColumn();
                        value.Name = "Value";

                        DataGridViewTextBoxColumn maxvalue = new DataGridViewTextBoxColumn();
                        maxvalue.Name = "MaxValue";
                        maxvalue.ReadOnly = true;

                        DG.Columns.Add(comboBox);
                        DG.Columns.Add(value);
                        DG.Columns.Add(maxvalue);

                        page.Controls.Add(DG);

                        TabControl_ParagonPoints.TabPages.Add(page);

                        foreach (var bonuspoint in setup.BonusPoints)
                        {
                            DG.Rows.Add(bonuspoint.Type.ToString(), bonuspoint.Value.ToString(),
                                bonuspoint.MaxValue.ToString());
                        }
                    }
                }
            }
            catch (Exception)
            {
            }
        }

        private void Save_Setups()
        {
            try
            {
                List<ParagonPointSetup> Buffer = new List<ParagonPointSetup>();

                foreach (var tabpage in TabControl_ParagonPoints.TabPages.OfType<TabPage>())
                {
                    DataGridView DG = tabpage.Controls.OfType<DataGridView>().FirstOrDefault();

                    List<BonusPoint> BonusPoints = new List<BonusPoint>();

                    foreach (var row in DG.Rows.OfType<DataGridViewRow>())
                    {
                        if (row.Cells["Property"].Value != null)
                        {
                            BonusPoints Type =
                                (BonusPoints)Enum.Parse(typeof(BonusPoints), row.Cells["Property"].Value as string);

                            int Value = int.Parse(row.Cells["Value"].Value as string);

                            BonusPoints.Add(new BonusPoint(Type, Value, Load_MaxValue(Type.ToString())));
                        }
                    }

                    Buffer.Add(new ParagonPointSetup(tabpage.TabIndex, tabpage.Name, BonusPoints));
                }

                if (!A_Collection.Me.ParagonPointSpender.Setups.ContainsKey(A_Collection.Me.HeroGlobals.HeroID))
                    A_Collection.Me.ParagonPointSpender.Setups.Add(A_Collection.Me.HeroGlobals.HeroID, Buffer);
                else
                {
                    A_Collection.Me.ParagonPointSpender.Setups[A_Collection.Me.HeroGlobals.HeroID] = Buffer;
                }

                A_Tools.T_ExternalFile.ParagonPointSpenderSettings.Save();
            }
            catch (Exception)
            {
            }
        }
        private void BTN_ParagonPointsReload_Click(object sender, EventArgs e)
        {
            Load_Setups();
        }

        private void BTN_ParagonPointsSave_Click(object sender, EventArgs e)
        {
            Save_Setups();
        }

        private void BTN_ParagonPointsDeleteTab_Click(object sender, EventArgs e)
        {
            if (TabControl_ParagonPoints.SelectedIndex >= 0 && TabControl_ParagonPoints.SelectedIndex < TabControl_ParagonPoints.TabCount)
                this.TabControl_ParagonPoints.TabPages.RemoveAt(TabControl_ParagonPoints.SelectedIndex);
        }



  

        public static Image resizeImage(Image imgToResize, Size size)
        {
            return (Image)(new Bitmap(imgToResize, size));
        }


       

        private void CB_ApsAndSnapShotAPs_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Overlay_APS = this.CB_ApsAndSnapShotAPs.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_EliteCircles_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.Overlay_EliteCircles = this.CB_EliteCircles.Checked;
            Properties.Settings.Default.Save();
        }



        private void bt_delete_hotkey_autopick_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoPick.Text.Length > 1)
            {
                this.tb_assignedAutoPick.Text = "";
                Properties.Settings.Default.HotkeyAutoPick = "";
                Properties.Settings.Default.Save();
            }
        }

        private void CB_AutoPick_Gem_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Gem = this.CB_AutoPick_Gem.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Material_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Material = this.CB_AutoPick_Material.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Legendary_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Legendary = this.CB_AutoPick_Legendary.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_LegendaryAncient_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_LegendaryAncient = this.CB_AutoPick_LegendaryAncient.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_GreaterRiftKeystone_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_GreaterRiftKeystone = this.CB_AutoPick_GreaterRiftKeystone.Checked;
            Properties.Settings.Default.Save();
        }

        private void TB_AutoPick_PickupRadius_TextChanged(object sender, EventArgs e)
        {
            int radius;
            if (int.TryParse(TB_AutoPick_PickupRadius.Text, out radius))
            {
                if (radius > 0 && radius <= 100)
                {
                    Properties.Settings.Default.AutoPickSettings_PickupRadius = radius;
                    Properties.Settings.Default.Save();
                }
            }
        }

        private void CB_AutoPick_Whites_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Whites = this.CB_AutoPick_Whites.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Magics_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Magics = this.CB_AutoPick_Magics.Checked;
            Properties.Settings.Default.Save();
        }

        private void CB_AutoPick_Rares_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.AutoPickSettings_Rares = this.CB_AutoPick_Rares.Checked;
            Properties.Settings.Default.Save();
        }

        private void bt_delete_hotkey_autocube_upgradeRare_Click(object sender, EventArgs e)
        {
            if (tb_assignedAutoCube_UpgradeRare.Text.Length > 1)
            {
                tb_assignedAutoCube_UpgradeRare.Text = "";
                Properties.Settings.Default.HotkeyAutoCube_UpgradeRare = "";
                Properties.Settings.Default.Save();
            }
        }

        private void delete_hotkey_autocube_ConvertMaterial_Click(object sender, EventArgs e)
        {
            if (this.tb_assignedAutoCube_ConvertMaterial.Text.Length > 1)
            {
                this.tb_assignedAutoCube_ConvertMaterial.Text = "";
                Properties.Settings.Default.HotkeyAutoCube_ConvertMaterial = "";
                Properties.Settings.Default.Save();
            }
        }

        private void cb_DisableAutocastOnNoOverride_CheckedChanged(object sender, EventArgs e)
        {
            Properties.Settings.Default.DisableAutocastOnNoOverride = this.cb_DisableAutocastOnNoOverride.Checked;
            Properties.Settings.Default.Save();
        }

        private void button_simplecast_save_Click(object sender, EventArgs e)
        {
            T_SimpleCast.save_from_datagrid(dataGridView_simpleCast);
            Window_Main.d3helperform.loadSimeCastListToCombobox();
        }

        private void dataGridView_simpleCast_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void button_simplecast_add_Click(object sender, EventArgs e)
        {
            try
            {
               

                DataTable table = (DataTable)dataGridView_simpleCast.DataSource;
                DataRow newRow = table.NewRow();
                newRow["name"] = "new Definition";
                table.Rows.Add(newRow);

                //dataGridView_simpleCast.Rows.Add("new", 0, 0, 0, 0, 0, 0);
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            };
        }

        private void button_simplecast_remove_Click(object sender, EventArgs e)
        {
            try
            {
                foreach (DataGridViewRow item in this.dataGridView_simpleCast.SelectedRows)
                {
                    dataGridView_simpleCast.Rows.RemoveAt(item.Index);
                }
            }
            catch (Exception ex) {
                MessageBox.Show(ex.Message);
            };
        }

        private void button_get_ui_elements_Click(object sender, EventArgs e)
        {
            visible_uxcontrols = new List<UXControl>();
            foreach (UXControl uxcontrol in UXHelper.Enumerate())
            {
                try
                {
                    if (uxcontrol.IsVisible())
                    {
                        visible_uxcontrols.Add(uxcontrol);
                        listBox_ui_elements.Items.Add(uxcontrol.ToString());
                    }
                }
                catch (Exception) { }
            }
        }



        private void listbox_ui_elemts_copySelectedValuesToClipboard()
        {
            var builder = new StringBuilder();
            foreach (string item in listBox_ui_elements.SelectedItems)
            {
                builder.AppendLine(item);
                //builder.AppendLine(item.SubItems[1].Text);
            }

            Clipboard.SetText(builder.ToString());
        }

        private void textBox_filter_ui_elemets_listbox_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != listBox_ui_elements) return;

            //strg+c
            if (e.Control && e.KeyCode == Keys.C)
                listbox_ui_elemts_copySelectedValuesToClipboard();
        }

        private void textBox_filter_ui_elemets_listbox_TextChanged_1(object sender, EventArgs e)
        {
            if(visible_uxcontrols != null && visible_uxcontrols.Any())
            {
                listBox_ui_elements.BeginUpdate();
                listBox_ui_elements.Items.Clear();

                string text = textBox_filter_ui_elemets_listbox.Text;

                if (!string.IsNullOrEmpty(text))
                {


                    foreach (UXControl ux in visible_uxcontrols)
                    {
                        string str = ux.ToString();

                        if (str.ToLower().Contains(text.ToLower()))
                        {
                            listBox_ui_elements.Items.Add(str);
                        }
                    }
                }

                listBox_ui_elements.EndUpdate();
            }else
            {

                listBox_ui_elements.BeginUpdate();
                listBox_ui_elements.Items.Clear();

                foreach(UXControl ux in visible_uxcontrols)
                {
                    string str = ux.ToString();
                    listBox_ui_elements.Items.Add(str);
                }

                listBox_ui_elements.EndUpdate();

            }
        }

        private void listBox_ui_elements_SelectedIndexChanged(object sender, EventArgs e)
        {
            string text = (string)listBox_ui_elements.SelectedItem;
            if(text != null)
            {
                A_WPFOverlay.Overlay.selected_uxcontrol = text;
                Clipboard.SetText(text);
            }
        }

    }
}
