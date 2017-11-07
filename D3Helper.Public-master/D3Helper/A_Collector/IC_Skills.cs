using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Collection;
using Enigma.D3;
using Enigma.D3.UI;
using Enigma.D3.Helpers;
using Enigma.D3.UI.Controls;

namespace D3Helper.A_Collector
{
    class IC_Skills
    {
        private static DateTime _lastPassiveUpdate = DateTime.Now;

        public static void Collect()
        {
            try
            {
                if (A_Collection.Me.HeroStates.isInGame && A_Collection.Environment.Scene.GameTick > 1 && A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                {
                    get_ActiveSkills();

                    if (_lastPassiveUpdate.AddMilliseconds(500) <= DateTime.Now)
                    {
                        get_PassiveSkills();
                        _lastPassiveUpdate = DateTime.Now;
                    }

                    get_SkillControls();
                    get_HotBarSkills();

                    update_AutoCastOverrides();

                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }

        }


        private static void get_ActiveSkills()
        {
            try
            {
                lock (A_Collection.Me.HeroDetails.ActiveSkills)
                {

                    if (A_Collection.Me.HeroGlobals.LocalPlayerData != null)
                    {
                        A_Collection.Me.HeroDetails.ActiveSkills.Clear();

                        List<ActivePower> ActivePowers;
                        lock (A_Collection.Me.HeroDetails.ActivePowers)
                            ActivePowers = A_Collection.Me.HeroDetails.ActivePowers.ToList();

                        var SkillOverrides =
                            ActivePowers.Where(
                                x => A_Tools.T_LocalPlayer.PowerCollection.isSkillOverride(x.AttribId) && x.Value != -1);

                        if (SkillOverrides.Count() > 0)
                        {
                            foreach (var _override in SkillOverrides)
                            {
                                int PowerSNO = _override.Value;
                                //int Rune = _override.PowerSnoId;

                                switch (PowerSNO)
                                {
                                    case 392883:
                                    case 392884:
                                    case 392885:
                                        PowerSNO = 167355;
                                        break;

                                    case 392886:
                                    case 392887:
                                    case 392888:
                                        PowerSNO = 135166;
                                        break;

                                    case 392889:
                                    case 392890:
                                    case 392891:
                                        PowerSNO = 135238;
                                        break;
                                }

                                if(!A_Collection.Me.HeroDetails.ActiveSkills.ContainsKey(PowerSNO))
                                    A_Collection.Me.HeroDetails.ActiveSkills.Add(PowerSNO, -1);
                            }
                        }
                        else
                        {

                            lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                            {
								var activeskillslist = A_Collection.Me.HeroGlobals.LocalPlayerData.GetActiveSkills();

                                for (int i = 0; i < activeskillslist.Count(); i++)
                                {
                                    if (
                                        !A_Collection.Me.HeroDetails.ActiveSkills.ContainsKey(
                                            activeskillslist[i].x00_PowerSnoId))
                                    {
                                        A_Collection.Me.HeroDetails.ActiveSkills.Add(
                                            activeskillslist[i].x00_PowerSnoId, activeskillslist[i].x04_RuneId);
                                    }
                                }
                            }
                        }

                        //-- adds HealthPotion Power to List
                        A_Collection.Me.HeroDetails.ActiveSkills.Add(0, -1);
                        //

                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_PassiveSkills()
        {
            try
            {
                if (A_Collection.Me.HeroGlobals.LocalPlayerData != null)
                {
                    PlayerData Local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        Local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                    List<int> Buffer = new List<int>();
					Buffer = Local.GetPassivePowerSnoIds().ToList();

                    lock (A_Collection.Me.HeroDetails.PassiveSkills) A_Collection.Me.HeroDetails.PassiveSkills = Buffer;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_SkillControls()
        {
            try
            {
                var _Controls = new List<UXIcon>();

                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar1));
                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar2));
                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar3));
                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar4));
                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBarRightclick));
                _Controls.Add(UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBarLeftclick));

                lock(A_Collection.Skills.UI_Controls.SkillControls) A_Collection.Skills.UI_Controls.SkillControls = _Controls;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_HotBarSkills()
        {
            try
            {
                while (!A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.portrait_0))
                    System.Threading.Thread.Sleep(5);

                var HotBar1 = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar1);
                var HotBar2 = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar2);
                var HotBar3 = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar3);
                var HotBar4 = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBar4);
                var HotBarRightClick = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBarRightclick);
                var HotBarLeftClick = UXHelper.GetControl<UXIcon>(A_Enums.UIElements.SkillHotBarLeftclick);

                List<SkillPower> AllSkillPowers;
                lock (A_Collection.Presets.SkillPowers.AllSkillPowers)
                    AllSkillPowers = A_Collection.Presets.SkillPowers.AllSkillPowers.ToList();

                A_Collection.Skills.SkillInfos._HotBar1Skill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBar1.GetPowerSnoId()));
                A_Collection.Skills.SkillInfos._HotBar2Skill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBar2.GetPowerSnoId()));
                A_Collection.Skills.SkillInfos._HotBar3Skill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBar3.GetPowerSnoId()));
                A_Collection.Skills.SkillInfos._HotBar4Skill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBar4.GetPowerSnoId()));
                A_Collection.Skills.SkillInfos._HotBarRightClickSkill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBarRightClick.GetPowerSnoId()));
                A_Collection.Skills.SkillInfos._HotBarLeftClickSkill = AllSkillPowers.FirstOrDefault(x => x.PowerSNO == convert_ArchonPowerSNO(HotBarLeftClick.GetPowerSnoId()));

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static int convert_ArchonPowerSNO(int PowerSNO)
        {
            try
            {
                switch (PowerSNO)
                {
                    case 392883:
                    case 392884:
                    case 392885:
                        return 167355;

                    case 392886:
                    case 392887:
                    case 392888:
                        return 135166;

                    case 392889:
                    case 392890:
                    case 392891:
                        return 135238;

                    default:
                        return PowerSNO;
                }
            }
            catch (Exception)
            {
                
                return PowerSNO;
            }
        }


        private static void update_AutoCastOverrides()
        {
            try
            {
                if(A_Tools.T_ExternalFile.AutoCastOverrides_changed)
                {
                    A_Tools.T_ExternalFile.AutoCastOverrides.UpdateOverrides();

                    A_Tools.T_ExternalFile.AutoCastOverrides_changed = false;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }



    }
}
