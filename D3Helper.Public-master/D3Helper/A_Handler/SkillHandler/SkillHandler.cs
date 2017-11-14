using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Linq;
using D3Helper.A_Collection;
using D3Helper.A_Enums;
using D3Helper.A_Handler.Log;
using Exception = System.Exception;

namespace D3Helper.A_Handler.SkillHandler
{
    class SkillHandler
    {
        public static Dictionary<A_Collection.SkillPower, A_Enums.ActionBarSlot> SkillsToCast = new Dictionary<A_Collection.SkillPower, A_Enums.ActionBarSlot>();
        
        private const int Sleep = 10;

        public static Dictionary<int, DateTime> _CastTimes = new Dictionary<int, DateTime>(); 

        public static void handleSkills()
        {
            try
            {
                get_SkillsToCast();

                if (can_Cast())
                {
                    HealthPotion.HealthPotion.handlePotion();
                    try_SkillCasts();
                }
                else
                {
                    A_Tools.InputSimulator.IS_Keyboard.ChannelKey_StopAll();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }

        private static void get_SkillsToCast()
        {
            try
            {
                Dictionary<int, int> ActiveSkills;
                lock(A_Collection.Me.HeroDetails.ActiveSkills) ActiveSkills = A_Collection.Me.HeroDetails.ActiveSkills;

                if (ActiveSkills.Count > 0)
                {
                    if (SkillsToCast.Count > 0)
                    {
                        SkillsToCast.Clear();
                    }

                    if (A_Collection.Skills.SkillInfos._HotBar1Skill != null && !A_Collection.Me.AutoCastOverrides.AutoCast1Override)
                    {
                        if(!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBar1Skill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBar1Skill, A_Enums.ActionBarSlot.Slot1);
                    }
                    if (A_Collection.Skills.SkillInfos._HotBar2Skill != null && !A_Collection.Me.AutoCastOverrides.AutoCast2Override)
                    {
                        if (!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBar2Skill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBar2Skill, A_Enums.ActionBarSlot.Slot2);
                    }
                    if (A_Collection.Skills.SkillInfos._HotBar3Skill != null && !A_Collection.Me.AutoCastOverrides.AutoCast3Override)
                    {
                        if (!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBar3Skill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBar3Skill, A_Enums.ActionBarSlot.Slot3);
                    }
                    if (A_Collection.Skills.SkillInfos._HotBar4Skill != null && !A_Collection.Me.AutoCastOverrides.AutoCast4Override)
                    {
                        if (!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBar4Skill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBar4Skill, A_Enums.ActionBarSlot.Slot4);
                    }
                    if (A_Collection.Skills.SkillInfos._HotBarRightClickSkill != null && !A_Collection.Me.AutoCastOverrides.AutoCastRMBOverride)
                    {
                        if (!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBarRightClickSkill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBarRightClickSkill, A_Enums.ActionBarSlot.RMB);
                    }
                    if (A_Collection.Skills.SkillInfos._HotBarLeftClickSkill != null && !A_Collection.Me.AutoCastOverrides.AutoCastLMBOverride)
                    {
                        if (!SkillsToCast.ContainsKey(A_Collection.Skills.SkillInfos._HotBarLeftClickSkill))
                            SkillsToCast.Add(A_Collection.Skills.SkillInfos._HotBarLeftClickSkill, A_Enums.ActionBarSlot.LMB);
                    }

                    //- Adds Potion to List
                    SkillsToCast.Add(A_Collection.Presets.SkillPowers.AllSkillPowers.FirstOrDefault(x => x.PowerSNO == 0), ActionBarSlot.Potion);
                    //
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static void try_SkillCasts()
        {
            try
            {
                foreach (var Skill in SkillsToCast)
                {

                    bool IsTimedCast = false;
                    bool ShouldChannel = false;
                    int ChannelTicks = 0;

                    int EquippedRune = A_Tools.Skills.Skills.S_Global.get_EquippedSkillRune(Skill.Key.PowerSNO);


                    if (Aggregator_CastConditions.CanCast(Skill.Key, EquippedRune, out IsTimedCast, out ShouldChannel,
                        out ChannelTicks))
                    {
                        if (!ShouldChannel)
                        {
                            if (A_Collection.Me.HeroDetails.CurrentlyUsingPower != Skill.Key.PowerSNO)
                            {
                                A_Tools.InputSimulator.IS_Keyboard.PressKey(Skill.Value);

                                if (IsTimedCast)
                                {
                                    if (_CastTimes.ContainsKey(Skill.Key.PowerSNO))
                                        _CastTimes[Skill.Key.PowerSNO] = DateTime.Now;
                                    else
                                    {
                                        _CastTimes.Add(Skill.Key.PowerSNO, DateTime.Now);
                                    }
                                }
                            }
                        }
                        else if (ShouldChannel)
                        {
                            if (!A_Collection.Hotkeys.IngameKeys.IsTownPortal)
                                A_Tools.InputSimulator.IS_Keyboard.ChannelKey_Start(Skill.Value, ChannelTicks);
                            else
                            {
                                A_Tools.InputSimulator.IS_Keyboard.ChannelKey_StopAll();
                            }
                        }
                    }
                    

                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }


        private static bool can_Cast()
        {
            try
            {
                return
                    A_Collection.D3Client.Window.isForeground &&
                    A_Collection.Me.HeroStates.isAlive &&
                    A_Collection.Me.HeroStates.isInGame &&
                    !A_Collection.Me.HeroStates.isInTown &&
                    !A_Collection.Me.HeroStates.isResuracting &&
                    !A_Collection.Me.HeroStates.isTeleporting &&
                    !A_Collection.D3UI.isChatting &&
                    !A_Collection.D3UI.isOpenFriendlist &&
                    !A_Collection.D3UI.isOpenInventory &&
                    !A_Collection.D3UI.isOpenBountyMap &&
                    !A_Collection.D3UI.isOpenPlayerContextMenu &&
                    !A_Collection.D3UI.isOpenMap &&
                    !A_Collection.D3UI.isOpenSkillPanel &&
                    !A_Collection.Me.ParagonPointSpender.Is_SpendingPoints &&
                    !A_Collection.D3UI.isLeavingGame;
            }
            catch (Exception e)
            { 
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);

                return false;
            }
        }
    }
}
