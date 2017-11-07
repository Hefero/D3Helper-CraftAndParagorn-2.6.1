using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Enigma.D3;
using Enigma.D3.Helpers;

using D3Helper.A_Tools;
using D3Helper.A_Collector;
using Enigma.D3.Enums;
using DamageType = D3Helper.A_Enums.DamageType;
using Enigma.D3.AttributeModel;

namespace D3Helper.A_Collector
{
    public class Monster_ActivePower
    {
        public Monster_ActivePower(int attribID, int modifier, int value)
        {
            this.AttribID = attribID;
            this.Modifier = modifier;
            this.Value = value;
        }
        public int AttribID { get; set; }
        public int Modifier { get; set; }
        public int Value { get; set; }
    }
    public class ActivePower
    {
        public ActivePower(int attribId, int powerSnoId, int value)
        {
            this.AttribId = attribId;
            this.PowerSnoId = powerSnoId;
            this.Value = value;

        }
        public int AttribId { get; set; }
        public int PowerSnoId { get; set; }
        public int Value { get; set; }
        
    }
    
    public class Vector3D
    {
        public Vector3D(float x, float y, float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }
    }

    class IC_Player
    {
        
        private static Vector3D _lastWorldPost = new Vector3D(-1,-1,-1);
        private static DateTime _lastAPS = DateTime.Now;
        private static DateTime _lastTotalXP = DateTime.Now;
        private static DateTime _lastPlayersInGame = DateTime.Now;
        private static DateTime _lastLVL = DateTime.Now;

        public static void Collect()
        {
            try
            {
                get_CounterCurrentFrame();
                get_GameTick();
                get_isInGame();

                get_BTag();

                if (A_Collection.Me.HeroStates.isInGame && A_Collection.Environment.Scene.GameTick > 1 && A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                {
                    get_LocalACD();

                    if (A_Collection.Me.HeroGlobals.LocalACD != null)
                    {
                        get_LocalPlayerData();
                        get_SelectedAttackableACD();
                        

                        get_Hero();

                        if (_lastLVL.AddMilliseconds(1000) <= DateTime.Now)
                        {
                            get_Lvl();
                            _lastLVL = DateTime.Now;
                        }

                        get_Hitpoints();

                        if (_lastAPS.AddMilliseconds(500) <= DateTime.Now)
                        {
                            get_AttackPerSecondTotal();
                            _lastAPS = DateTime.Now;
                        }

                        get_ResourcePrimary();
                        get_ResourceSecondary();
                        get_ActivePowers();

                        if (_lastTotalXP.AddMilliseconds(1000) <= DateTime.Now)
                        {
                            get_TotalXP();
                            _lastTotalXP = DateTime.Now;
                        }

                        get_LocalDataIndex();

                        if (_lastPlayersInGame.AddMilliseconds(1000) <= DateTime.Now)
                        {
                            get_PlayersInGame();
                            _lastPlayersInGame = DateTime.Now;
                        }

                        if(Properties.Settings.Default.overlayconventiondraws)
                            get_ConventionElement();

                        get_isUsingPower();
                        get_isInTown();
                        get_isMoving();
                        get_isTeleporting();
                        get_isResuracting();
                        get_isInParty();
                        get_isAlive();

                        observe_HealthPotion();
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_LocalACD()
        {
            try
            {
                if (A_Collection.Environment.Scene.Counter_CurrentFrame > 0)
                {
                    var localACD = ActorCommonData.Local;

                    if (localACD != null)
                    {
                        if (localACD.x000_Id != -1 && localACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player)
                        {                           
                            if(A_Collection.Me.HeroGlobals.LocalACD == null)
                                A_Collection.Me.HeroGlobals.LocalACD = localACD;
                            else
                            lock (A_Collection.Me.HeroGlobals.LocalACD) A_Collection.Me.HeroGlobals.LocalACD = localACD;
                        }
                        else
                        {
                            lock (A_Collection.Me.HeroGlobals.LocalACD) A_Collection.Me.HeroGlobals.LocalACD = null;
                        }
                    }
                    else
                    {
                        A_Collection.Me.HeroGlobals.LocalACD = null;
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_CounterCurrentFrame()
        {
            try
            {
                A_Collection.Environment.Scene.Counter_CurrentFrame = ObjectManager.Instance.x038_Counter_CurrentFrame;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_GameTick()
        {
            try
            {
                //A_Collection.Environment.Scene.GameTick = Engine.Current.ObjectManager.x7B0_Storage.x120_GameTick;

                A_Collection.Environment.Scene.GameTick = Storage.Instance.GetGameTick();

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_LocalPlayerData()
        {
            try
            {
                if(A_Collection.Me.HeroGlobals.LocalPlayerData == null)
                    A_Collection.Me.HeroGlobals.LocalPlayerData = PlayerData.Local;
                else
                lock (A_Collection.Me.HeroGlobals.LocalPlayerData) A_Collection.Me.HeroGlobals.LocalPlayerData = PlayerData.Local;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_SelectedAttackableACD()
        {
            try
            {
                ActorType type = ActorType.Invalid;
                double distance = 0;

                var acd = A_Tools.T_LocalPlayer.get_SelectedAtackableAcd(out distance, out type);

                if (type == ActorType.Monster)
                {
                    A_Collection.Me.HeroDetails.SelectedMonsterACD = acd;
                    A_Collection.Me.HeroDetails.Distance_SelectedMonsterACD = distance;
                    A_Collection.Me.HeroDetails.SelectedDestructibleACD = null;
                    A_Collection.Me.HeroDetails.Distance_SelectedDestructibleACD = 0;
                }
                else if(type == ActorType.Gizmo)
                {
                    A_Collection.Me.HeroDetails.SelectedMonsterACD = null;
                    A_Collection.Me.HeroDetails.Distance_SelectedMonsterACD = 0;
                    A_Collection.Me.HeroDetails.SelectedDestructibleACD = acd;
                    A_Collection.Me.HeroDetails.Distance_SelectedDestructibleACD = distance;
                }
                else
                {
                    A_Collection.Me.HeroDetails.SelectedMonsterACD = null;
                    A_Collection.Me.HeroDetails.Distance_SelectedMonsterACD = 0;
                    A_Collection.Me.HeroDetails.SelectedDestructibleACD = null;
                    A_Collection.Me.HeroDetails.Distance_SelectedDestructibleACD = 0;
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }
        

        private static void get_BTag()
        {
            try
            {

                if (A_Collection.Me.Account.BTag == null)
                {
                    A_Collection.Me.Account.BTag = "noBTag"; 
                    // disabled BTag check cause Ptr chain in ScreenMgr is obviously changed and i dont have time to figure it out
                    //Engine.Current.Memory.Reader.ReadChain<RefString>(0x01E247D8, 0x10, 0x9C + 4, 0x30).x04_PtrText; // ScreenManager Address + chain
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_Hero()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
                    A_Collection.Me.HeroGlobals.HeroID = A_Collection.Me.HeroGlobals.LocalPlayerData.GetHeroId();
                    lock (A_Collection.Me.HeroGlobals.HeroName) A_Collection.Me.HeroGlobals.HeroName = A_Collection.Me.HeroGlobals.LocalPlayerData.GetHeroName();
                    A_Collection.Me.HeroGlobals.HeroClass = (A_Enums.HeroClass)A_Collection.Me.HeroGlobals.LocalPlayerData.GetHeroClass();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }        


        private static void get_Lvl()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
                    A_Collection.Me.HeroGlobals.Lvl = A_Collection.Me.HeroGlobals.LocalPlayerData.GetLevel();
                    A_Collection.Me.HeroGlobals.Alt_Lvl = A_Collection.Me.HeroGlobals.LocalPlayerData.GetAltLevel();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }        


        private static void get_Hitpoints()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalACD) A_Collection.Me.HeroDetails.Hitpoints = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.HitpointsCur);
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData) A_Collection.Me.HeroDetails.Hitpoints_Percentage = A_Collection.Me.HeroGlobals.LocalPlayerData.GetLifePercentage() * 100;

                if (A_Collection.Me.HeroDetails.Hitpoints < 0.001)
                    A_Collection.Me.HeroDetails.SnapShotted_APS = A_Collection.Me.HeroDetails.AttacksPerSecondTotal;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_AttackPerSecondTotal()
        {
            try
            {
                ActorCommonData Local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) Local = A_Collection.Me.HeroGlobals.LocalACD;

                //double APS_CurrentHand = Local.GetAttributeValue(AttributeId.AttacksPerSecondItemCurrentHand);
                //double APS_ItemBonus = Local.GetAttributeValue(AttributeId.AttacksPerSecondItemBonus);

                //double APS_Total = APS_CurrentHand + APS_ItemBonus;

                //double IAS_BonusPercent = Local.GetAttributeValue(AttributeId.AttacksPerSecondPercent);
                //double IAS_HiddenBonus = IC_Player.IAS_HiddenBonus();

                //double IAS_BonusTotal = IAS_BonusPercent + IAS_HiddenBonus;

                //double APS_MaxTotal = APS_Total*(1 + IAS_BonusTotal);

                A_Collection.Me.HeroDetails.AttacksPerSecondTotal = Local.GetAttributeValue(AttributeId.AttacksPerSecondTotal);
                

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_ResourcePrimary()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalACD)
                {
                    int ResourceType_Primary = (int)A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceTypePrimary);

                    double resourceNow = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceCur, ResourceType_Primary);
                    double resourceTotal = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceMaxTotal, ResourceType_Primary);
                    double resourcecostreduce = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceCostReductionPercentAll);
                    //double resourcemultiplier = 1 + resourcecostreduce;


                    //A_Collection.Me.HeroDetails.ResourcePrimary = resourceNow * resourcemultiplier;
                    //A_Collection.Me.HeroDetails.ResourcePrimary_Percentage = (resourceNow / resourceTotal * 100) * resourcemultiplier;

                    A_Collection.Me.HeroDetails.ResourcePrimary = resourceNow;
                    A_Collection.Me.HeroDetails.ResourcePrimary_Percentage = (resourceNow / resourceTotal * 100);

                    A_Collection.Me.HeroDetails.ResourceCostReduction = resourcecostreduce;

                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_ResourceSecondary()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalACD)
                {
                    int ResourceType_Secondary = (int)A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceTypeSecondary);

                    double resourceNow = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceCur, ResourceType_Secondary);
                    double resourceTotal = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceMaxTotal, ResourceType_Secondary);
                    //double resourcecostreduce = A_Collection.Me.HeroGlobals.LocalACD.GetAttributeValue(Enigma.D3.Enums.AttributeId.ResourceCostReductionPercentAll);
                    //double resourcemultiplier = 1 + resourcecostreduce;

                    //A_Collection.Me.HeroDetails.ResourceSecondary = resourceNow * resourcemultiplier;
                    //A_Collection.Me.HeroDetails.ResourceSecondary_Percentage = (resourceNow / resourceTotal * 100) * resourcemultiplier;

                    A_Collection.Me.HeroDetails.ResourceSecondary = resourceNow ;
                    A_Collection.Me.HeroDetails.ResourceSecondary_Percentage = (resourceNow / resourceTotal * 100);
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_ActivePowers()
        {
            try
            {
                //Stopwatch s1 = new Stopwatch();
                //s1.Start();
                
                List<Enigma.D3.Collections.Map<int, AttributeValue>.Entry> attributes;
                lock(A_Collection.Me.HeroGlobals.LocalACD) attributes = A_Collection.Me.HeroGlobals.LocalACD.EnumerateAttributes().ToList();

                List<ActivePower> Buffer = new List<ActivePower>();

                for (int i = 0; i < attributes.Count; i++)
                {
                    uint attribId = (uint)(attributes[i].x04_Key & 0xFFF);
                    uint modifier = (uint)(attributes[i].x04_Key >> 12);
                    int value = attributes[i].x08_Value.Int32;
                    
                    Buffer.Add(new ActivePower((int)attribId, (int)modifier, value));
                }

                lock (A_Collection.Me.HeroDetails.ActivePowers) A_Collection.Me.HeroDetails.ActivePowers = Buffer;

                //s1.Stop();
                //TimeSpan t1 = s1.Elapsed;
                //Console.WriteLine(t1.TotalMilliseconds);

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_TotalXP()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalACD)
                {
                    var currentLVLTotal = A_Enums.ParagonXPTable.TotalXp[A_Collection.Me.HeroGlobals.Alt_Lvl];
                    var nextLVLTotal = A_Enums.ParagonXPTable.TotalXp[A_Collection.Me.HeroGlobals.Alt_Lvl + 1];
                    var xplefttonext = Attributes.AltExperienceNextLo.GetValue(A_Collection.Me.HeroGlobals.LocalACD);

                    long TotalXP = currentLVLTotal + ((nextLVLTotal - currentLVLTotal) - (long)xplefttonext);

                    if (TotalXP != A_Collection.Stats.Player.TotalXP)
                    {
                        A_Handler.StatHandler.StatHandler.TotalXP_changed = true;
                    }

                    A_Collection.Stats.Player.TotalXP = TotalXP;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_LocalDataIndex()
        {
            try
            {

                lock(A_Collection.Me.HeroGlobals.LocalPlayerData) A_Collection.Me.HeroGlobals.LocalDataIndex = A_Collection.Me.HeroGlobals.LocalPlayerData.x0000_Index;
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }


        private static void get_PlayersInGame()
        {
            try
            {
                int counter = 0;

                for (int i = 0; i < 4; i++)
                {
                    if (A_Tools.T_D3UI.UIElement.isVisible("Root.NormalLayer.portraits.stack.party_stack.portrait_" + i))
                    {
                        counter++;
                    }
                }

                A_Collection.Me.Party.PlayersInGame = counter;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_ConventionElement()
        {
            try
            {
                if (A_Tools.T_LocalPlayer.isBuff(A_Enums.Powers.Convention_PowerSno, 746))
                {
                    A_Collection.Me.HeroDetails.CurrentConventionElement =
                        A_Tools.T_LocalPlayer.ConventionOfElements.getConventionElement();

                    A_Collection.Me.HeroDetails.NextConventionElement =
                        A_Tools.T_LocalPlayer.ConventionOfElements.getNextConventionElement(
                            A_Collection.Me.HeroDetails.CurrentConventionElement, A_Collection.Me.HeroGlobals.HeroClass);

                    A_Collection.Me.HeroDetails.Convention_TicksLeft =
                        A_Tools.T_LocalPlayer.ConventionOfElements.getConventionTicksLeft(
                            A_Collection.Me.HeroDetails.CurrentConventionElement);
                }
                else
                {
                    A_Collection.Me.HeroDetails.CurrentConventionElement = DamageType.none;
                    A_Collection.Me.HeroDetails.NextConventionElement = DamageType.none;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isInGame()
        {
            try
            {
                var isNotInGame = LocalData.Instance.x04_IsNotInGame;

                if(isNotInGame == 1)
                {
                    A_Collection.Me.HeroStates.isInGame = false;
                }
                else
                {
                    A_Collection.Me.HeroStates.isInGame = true;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isInTown()
        {
            try
            {
                A_Collection.Me.HeroStates.isInTown = A_Tools.T_LocalPlayer.isBuff(A_Enums.Powers.InTownBuff);
                
            }
            catch (Exception e)
            {
                A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, A_Enums.ExceptionThread.ICollector);

                lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
            }
        }
        
        private static DateTime _firstStop = new DateTime();
        private static TimeSpan _stopDuration;

        private static void get_isMoving()
        {
            try
            {
                if (A_Collection.Hotkeys.IngameKeys.IsForceStandStill)
                {
                    A_Collection.Me.HeroStates.isMoving = false;

                    if(A_Collection.Me.HeroDetails.StandStill_Start == new DateTime())
                        A_Collection.Me.HeroDetails.StandStill_Start = DateTime.Now;
                    return;
                }

                ActorCommonData Local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) Local = A_Collection.Me.HeroGlobals.LocalACD;

                if (Local != null)
                {
                    if (Local.x000_Id != -1)
                    {
                        Vector3D curWorldPos = new Vector3D(Local.x0D0_WorldPosX, Local.x0D4_WorldPosY, Local.x0D8_WorldPosZ);

                        if (curWorldPos.X != _lastWorldPost.X ||
                            curWorldPos.Y != _lastWorldPost.Y ||
                            curWorldPos.Z != _lastWorldPost.Z)
                        {
                            A_Collection.Me.HeroStates.isMoving = true;
                            A_Collection.Me.HeroDetails.StandStill_Start = new DateTime();
                            _firstStop = new DateTime();
                        }
                        else
                        {
                            if(A_Collection.Me.HeroDetails.StandStill_Start == new DateTime())
                                A_Collection.Me.HeroDetails.StandStill_Start = DateTime.Now;

                            if (_firstStop == new DateTime())
                                _firstStop = DateTime.Now;

                            _stopDuration = new TimeSpan(DateTime.Now.Ticks - _firstStop.Ticks);

                            if (_stopDuration.TotalMilliseconds >= 500)
                            {
                                A_Collection.Me.HeroStates.isMoving = false;
                                _firstStop = new DateTime();
                            }
                        }

                        _lastWorldPost = curWorldPos;
                        
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isTeleporting()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
					var tag = A_Collection.Me.HeroGlobals.LocalACD.GetAnimTag();

                    //if(tag != 69632)
                    //{
                    //    A_Handler.Log.LogEntry.addLogEntry("Tag -> " + tag);
                    //}

                    //69632 = standing
                    //410129 = bountie animation
                    //0x68500 = teleport

                    // Casting/channeling teleport sets anim tag 0x68500 during cast. Instant teleport does not use an anim tag.
                    // Reaching teleport destination animates tag 0x68000 which may or may not be seen depending on load time.
                    // Those tags are valid at least in 2.4.2
                    A_Collection.Me.HeroStates.isTeleporting = (tag == 0x68500) || (tag == 410129);
				}
			}
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isResuracting()
        {
            try
            {
                lock(A_Collection.Me.HeroGlobals.LocalPlayerData)
                {
                    switch (A_Collection.Me.HeroGlobals.LocalPlayerData.GetPowerUse())
                    {
                        case A_Enums.Powers.RessurectPlayer:

                            A_Collection.Me.HeroStates.isResuracting = true;
                            break;

                        default:
                            A_Collection.Me.HeroStates.isResuracting = false;
                            break;
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_isInParty()
        {
            try
            {
                A_Collection.Me.HeroStates.isInParty = A_Tools.T_D3UI.UIElement.isVisible(A_Enums.UIElements.portrait_1);
            }
            catch { A_Collection.Me.HeroStates.isInParty = false; A_Initialize.Th_ICollector.ExceptionCount++; }
        }


        private static void get_isAlive()
        {
            try
            {
                if (A_Collection.Me.HeroDetails.Hitpoints >= 0.000001)
                {
                    A_Collection.Me.HeroStates.isAlive = true;
                }
                else
                {
                    A_Collection.Me.HeroStates.isAlive = false;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }

        private static bool usedHealthPotion = false;
        private static bool setSnapShot = false;

        private static void observe_HealthPotion()
        {
            try
            {
                ActorCommonData Local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) Local = A_Collection.Me.HeroGlobals.LocalACD;

                double cooldown = Local.GetAttributeValue(AttributeId.PowerCooldown, A_Enums.Powers.DrinkHealthPotion);
                
                if (cooldown > -1)
                {
                    usedHealthPotion = true;
                }
                else
                {
                    usedHealthPotion = false;
                    setSnapShot = false;
                }

                if (usedHealthPotion)
                {
                    if (!setSnapShot)
                    {
                        A_Collection.Me.HeroDetails.SnapShotted_APS = A_Collection.Me.HeroDetails.AttacksPerSecondTotal;
                        setSnapShot = true;
                    }
                }

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }

        private static int LastUsedPower = -1;

        private static void get_isUsingPower()
        {
            try
            {
                PlayerData local;
                lock (A_Collection.Me.HeroGlobals.LocalPlayerData) local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                if (local != null)
                {
                    A_Collection.Me.HeroDetails.CurrentlyUsingPower = local.GetPowerUse();

                    if (local.GetPowerUse() == LastUsedPower)
                        A_Collection.Me.HeroStates.usedNewPower = false;
                    else
                        A_Collection.Me.HeroStates.usedNewPower = true;

                    LastUsedPower = local.GetPowerUse();

                    if (local.xA0E0_PowerUse != -1)
                    {
                        A_Collection.Me.HeroStates.isUsingPower = true;
                    }
                    else
                    {
                        A_Collection.Me.HeroStates.isUsingPower = false;
                    }
                }
                else
                {
                    A_Collection.Me.HeroDetails.CurrentlyUsingPower = -1;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }
    }
}
