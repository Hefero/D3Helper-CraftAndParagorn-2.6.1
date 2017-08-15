using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;

using D3Helper.A_Enums;
using D3Helper.A_Collector;
using D3Helper.A_Tools;

namespace D3Helper.A_Collection
{
    class Me
    {
        public class Account
        {
            //-- Account Globals
            public static string BTag = null;                                                              // default: null
            //
        }
        public class HeroGlobals
        {
            //-- Hero Globals
            public static ActorCommonData LocalACD;                                                 // default: null
            public static PlayerData LocalPlayerData;                                               // default: null    
            public static long HeroID;                                                              // default: -1
            public static int LocalDataIndex;                                                       // default: -1
            public static string HeroName = String.Empty;                                           // default: null
            public static HeroClass HeroClass;                                                      // default: None
            public static int Lvl;                                                                  // default: -1 min: 1 max: 70
            public static int Alt_Lvl;                                                              // default: -1 min: 0 max: 10000
            
            //
        }
        public class HeroStates
        {
            //-- Hero States
            public static bool isInGame;                                                            // default: false
            public static bool isInTown;                                                            // default: false
            public static bool isWalking;                                                           // default: false
            public static bool isMoving;
            public static bool isTeleporting;                                                       // default: false
            public static bool isResuracting;                                                       // default: false
            public static bool isInParty;                                                           // default: false
            public static bool isAlive;                                                             // default: false
            public static bool isUsingPower;                                                        // default: false
            public static bool usedNewPower;
            //
        }
        public class HeroDetails
        {
            //-- Hero Details
            public static double Hitpoints;                                                         // default: -1 min: 0
            public static double Hitpoints_Percentage;                                              // default: -1 min: 0 max: 100
            public static double ResourcePrimary;                                                   // default: -1 min: 0
            public static double ResourcePrimary_Percentage;                                        // default: -1 min: 0 max: 100
            public static double ResourceSecondary;                                                 // default: -1 min: 0
            public static double ResourceSecondary_Percentage;                                      // default: -1 min: 0 max: 100
            public static List<ActivePower> ActivePowers = new List<ActivePower>();                 // default: null
            public static List<int> PassiveSkills = new List<int>(); 
            public static List<ActorCommonData> EquippedItems = new List<ActorCommonData>();        // default: null
            public static Dictionary<int, int> ActiveSkills = new Dictionary<int, int>();
            public static ActorCommonData SelectedMonsterACD;
            public static double Distance_SelectedMonsterACD;
            public static ActorCommonData SelectedDestructibleACD;
            public static double Distance_SelectedDestructibleACD;
            public static DamageType CurrentConventionElement;
            public static DamageType NextConventionElement;
            public static double Convention_TicksLeft;
            public static double AttacksPerSecondTotal;
            public static int CurrentlyUsingPower;
            public static DateTime StandStill_Start;
            public static double SnapShotted_APS;
            public static double ResourceCostReduction;                                              //0.1 = 10%, cindercoat not included
            
            //
        }
        public class Party
        {
            //--
            public static int PlayersInGame;                                                        // default: -1 min: 0 max: 3
            public static List<ACD> PartyMemberInRange = new List<ACD>();
            public static PlayerData PartyMember1;
            public static PlayerData PartyMember2;
            public static PlayerData PartyMember3;
            public static List<ActivePower> PartyMember1_ActivePowers = new List<ActivePower>();
            public static List<ActivePower> PartyMember2_ActivePowers = new List<ActivePower>();
            public static List<ActivePower> PartyMember3_ActivePowers = new List<ActivePower>();
            public static DamageType PartyMember1_CurrentConventionElement;
            public static DamageType PartyMember1_NextConventionElement;
            public static double PartyMember1_Convention_TicksLeft;
            public static DamageType PartyMember2_CurrentConventionElement;
            public static DamageType PartyMember2_NextConventionElement;
            public static double PartyMember2_Convention_TicksLeft;
            public static DamageType PartyMember3_CurrentConventionElement;
            public static DamageType PartyMember3_NextConventionElement;
            public static double PartyMember3_Convention_TicksLeft;
            //
        }
        public class ParagonPointSpender
        {
            //--
            public static string ParagonPoints_FilePath = path.AppDir + @"\paragonpoints.txt";
            public static bool ReloadSetups = true;                                                 // default: true
            public static bool Is_SpendingPoints = false;                                           // default: false
            public static int SelectedParagonPoints_Setup = 0;                                      // default: 0 min: 0 max: 4
            public static Dictionary<long,List<ParagonPointSetup>> Setups = new Dictionary<long, List<ParagonPointSetup>>();
            
            //
        }

        public class AutoCastOverrides
        {
            //--
            public static string FilePath = path.AppDir + @"\overrides.txt";
            public static List<Override> _Overrides = new List<Override>();
            public static bool AutoCast1Override = false;
            public static bool AutoCast2Override = false;
            public static bool AutoCast3Override = false;
            public static bool AutoCast4Override = false;
            public static bool AutoCastRMBOverride = false;
            public static bool AutoCastLMBOverride = false;
            //
        }

    }
}
