using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

using D3Helper.A_Collector;
using D3Helper.A_Collection;

using DamageType = D3Helper.A_Enums.DamageType;

namespace D3Helper.A_Tools
{
    class T_ACD
    {
        public static double convert_ProgressPointsToPercentage(double ProgressPoints)
        {
            try
            {
                return (ProgressPoints/530)*100;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static double get_RiftProgress(ActorCommonData monster)
        {
            try
            {
                if (A_Collection.Presets.Monsters.Monsters_RiftProgress.Count > 0)
                {
                   
                    return A_Collection.Presets.Monsters.Monsters_RiftProgress[monster.x090_ActorSnoId];
                }
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static double get_RiftProgress(List<ActorCommonData> monsters)
        {
            try
            {
                if (A_Collection.Presets.Monsters.Monsters_RiftProgress.Count > 0)
                {
                    double progress = 0;

                    foreach (var monster in monsters)
                    {
                        progress += A_Collection.Presets.Monsters.Monsters_RiftProgress[monster.x090_ActorSnoId];
                    }

                    return progress;
                }
                return 0;
            }
            catch (Exception)
            {

                return 0;
            }
        }
        public static double get_Distance(float actorX, float actorY)
        {
            try
            {
                if (A_Collection.Me.HeroGlobals.LocalACD != null)
                {
                    ActorCommonData LocalAcd;
                    lock (A_Collection.Me.HeroGlobals.LocalACD) LocalAcd = A_Collection.Me.HeroGlobals.LocalACD;

                    if (LocalAcd != null)
                    {
                        float localX = LocalAcd.x0D0_WorldPosX;
                        float localY = LocalAcd.x0D4_WorldPosY;

                        float diffX = actorX - localX;
                        float diffY = actorY - localY;


                        float distance = (diffX*diffX) + (diffY*diffY);

                        return Math.Sqrt((double) distance);
                    }
                }
                return 0;
            }
            catch { return 0; }
        }
        public static double get_Distance(float actorX, float actorY, float ToActorX, float ToActorY)
        {
            try
            {
                float localX = actorX;
                float localY = actorY;

                float diffX = ToActorX - localX;
                float diffY = ToActorY - localY;


                float distance = (diffX * diffX) + (diffY * diffY);

                return Math.Sqrt((double)distance);
            }
            catch { return 0; }
        }
        public static bool IsValidMonster(ActorCommonData acd)
        {
            return acd.x180_Hitpoints > 0.00001 &&
             (acd.x190_Flags_Is_Trail_Proxy_Etc & 1) == 0 &&
             acd.x17C_ActorType == Enigma.D3.Enums.ActorType.Monster &&
             acd.x188_TeamId == 10;
        }

        //from memorymodel
        //private static bool IsValidMonster(ACD acd)
        //{
        //    return acd.Hitpoints > 0.00001 &&
        //        (acd.ObjectFlags & 1) == 0 &&
        //        acd.TeamID == 10;
        //}


        public static bool IsTreasureGoblin(ActorCommonData Acd)
        {
            switch (Acd.x090_ActorSnoId)
            {
                case 5984: // treasureGoblin_A
                case 5985: // treasureGoblin_B
                case 5987: // treasureGoblin_C
                case 5988: // treasureGoblin_D
                case 408655: // (PH) Backup Goblin
                case 408989: // Blood Thief
                case 391593: // Treasure Fiend
                case 413289: // (PH) Teleporty Goblin
                case 410576:
                case 410586:
                case 326803:
                case 408354:
                case 410572:
                case 410574:

                    return true;

                default:
                    return false;
            }
        }
        public static bool isBuff(int SnoPowerID, ActorCommonData acd)
        {
            try
            {
                acd.TakeSnapshot();
                if (acd.x000_Id != -1)
                {
                    var attributes = acd.EnumerateAttributes();

                    foreach (var attrib in attributes)
                    {
                        uint attribId = (uint)(attrib.x04_Key & 0xFFF);
                        uint modifier = (uint)(attrib.x04_Key >> 12);
                        int value = attrib.x08_Value.Int32;

                        if ((int)modifier == SnoPowerID && value > 0)
                        {
                            acd.FreeSnapshot();
                            return true;
                        }
                    }
                }
                acd.FreeSnapshot();
                return false;
            }
            catch { return false; }
        }
        public static bool isBuff(int SnoPowerID, int AttribId, ActorCommonData acd)
        {
            try
            {
                acd.TakeSnapshot();
                if (acd.x000_Id != -1)
                {
                    return (int)acd.GetAttributeValue((AttributeId)AttribId, SnoPowerID) > 0;
                }
                acd.FreeSnapshot();
                return false;
            }
            catch { return false; }
        }
        public static bool isBuff(List<int> SnoPowerIDs, ActorCommonData acd)
        {
            try
            {
                acd.TakeSnapshot();
                if (acd.x000_Id != -1)
                {
                    var attributes = acd.EnumerateAttributes();

                    foreach (var attrib in attributes)
                    {
                        uint attribId = (uint)(attrib.x04_Key & 0xFFF);
                        uint modifier = (uint)(attrib.x04_Key >> 12);
                        int value = attrib.x08_Value.Int32;

                        if (SnoPowerIDs.Contains((int)modifier) && value > 0)
                        {
                            acd.FreeSnapshot();
                            return true;
                        }
                    }
                }
                acd.FreeSnapshot();
                return false;
            }
            catch { return false; }
        }
        public static int getBuffCount(int SnoPowerID, int AttribId, ActorCommonData acd)
        {
            try
            {
                acd.TakeSnapshot();
                if (acd.x000_Id != -1)
                {
                    return (int) acd.GetAttributeValue((AttributeId) AttribId, SnoPowerID);
                    
                }
                acd.FreeSnapshot();
                return 0;
            }
            catch { return 0; }
        }
        public static List<Monster_ActivePower> get_MonsterActivePowers(ActorCommonData acd)
        {
            try
            {
                List<Monster_ActivePower> Buffer = new List<Monster_ActivePower>();

                var attributes = acd.EnumerateAttributes().ToList();


                foreach (var attrib in attributes)
                {
                    uint attribId = (uint)(attrib.x04_Key & 0xFFF);
                    uint modifier = (uint)(attrib.x04_Key >> 12);
                    int value = attrib.x08_Value.Int32;

                    Buffer.Add(new Monster_ActivePower((int)attribId, (int)modifier, value));
                }

                return Buffer;
            }
            catch { return new List<Monster_ActivePower>(); }
        }
        public static int get_MonstersInRangeOfSelectedMonster(ActorCommonData selectedMonster, double Range, out List<ActorCommonData> MonstersInRange)
        {
            MonstersInRange = new List<ActorCommonData>();

            try
            {
               
                List<A_Collector.ACD> acdcontainer;
                lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                int counter = 0;

                foreach (var acd in acdcontainer)
                {
                    if (acd.IsMonster && acd._ACD.x000_Id != selectedMonster.x000_Id)
                    {
                        double distance = A_Tools.T_ACD.get_Distance(selectedMonster.x0D0_WorldPosX, selectedMonster.x0D4_WorldPosY, acd._ACD.x0D0_WorldPosX, acd._ACD.x0D4_WorldPosY);
                        if (distance <= Range)
                        {
                            counter++;
                            MonstersInRange.Add(acd._ACD);
                        }
                    }
                }

                return counter;

            }
            catch { return 0; }
        }

        public static List<ActorCommonData> getEliteInRange(int radius)
        {
            //List<ActorCommonData> acdsinrange;
            //List<ActorCommonData> eliteInRange = new List<ActorCommonData>();


            //get_MonstersInRange(radius, true, true, out acdsinrange);
            //foreach (ActorCommonData monsterInRange in acdsinrange)
            //{
            //    if (isElite(monsterInRange) && !isEliteIllusionist(monsterInRange)) //dont list summoned illusionists
            //    {
            //        eliteInRange.Add(monsterInRange);
            //    }
            //}



            List <ActorCommonData> eliteInRange = new List<ActorCommonData>();

            List<A_Collector.ACD> acdcontainer;
            lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

            foreach (var monster in acdcontainer.Where(x => x.IsMonster))
            {
                //if (monster.Distance <= radius)
                //{
                    if (isElite(monster._ACD) && !isEliteIllusionist(monster._ACD)) //dont list summoned illusionists
                    {
                        eliteInRange.Add(monster._ACD);
                    }
                //}
            }

            return eliteInRange;
        }


        public static List<ActorCommonData> getNonEliteMonsterInRange(int radius)
        {


            List<ActorCommonData> NoneliteInRange = new List<ActorCommonData>();

            List<A_Collector.ACD> acdcontainer;
            lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

            foreach (var monster in acdcontainer.Where(x => x.IsMonster))
            {
                //if (monster.Distance <= radius)
                //{
                if (monster.IsMonster && !isElite(monster._ACD) && !isEliteIllusionist(monster._ACD)) //dont list summoned illusionists and elite
                {
                    NoneliteInRange.Add(monster._ACD);
                }
                //}
            }

            return NoneliteInRange;
        }



        public static int get_MonstersInRange(int radius, bool elite, bool boss, out List<ActorCommonData> acdsinrange)
        {
            acdsinrange = new List<ActorCommonData>();

            try
            {


                var localindex = A_Collection.Me.HeroGlobals.LocalDataIndex;


                List<A_Collector.ACD> acdcontainer;
                lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();


                int counter = 0;

                if (acdcontainer.Count() == 0) { return 0; }

                foreach (var monster in acdcontainer.Where(x => x.IsMonster && IsTargetable(x._ACD)))
                {

                    if (monster.Distance <= radius)
                    {
                        if (!elite && !boss)
                        {


                            counter++;

                        }
                        if (elite)
                        {
                            if (isElite(monster._ACD))
                            {

                                counter++;

                            }
                        }
                        if (boss)
                        {
                            if (isBoss(monster._ACD))
                            {
                                counter++;
                            }
                        }
                        acdsinrange.Add(monster._ACD);
                    }
                }

                return counter;

            }
            catch { return 0; }
        }

        public static bool IsTargetable(ActorCommonData acd)
        {
            if (acd.x000_Id == -1)
                return false;

            return acd.GetAttributeValue(AttributeId.Invulnerable) < 1 &&
                acd.GetAttributeValue(AttributeId.Stealthed) < 1 &&
                acd.GetAttributeValue(AttributeId.Burrowed) < 1 &&
                acd.GetAttributeValue(AttributeId.Untargetable) == 0;
        }

        public static bool isElite(ActorCommonData monster)
        {
            try
            {
                switch (monster.x0B8_MonsterQuality)
                {
                    case Enigma.D3.Enums.MonsterQuality.Unique:
                    case Enigma.D3.Enums.MonsterQuality.Rare:
                    case Enigma.D3.Enums.MonsterQuality.Champion:
                    case Enigma.D3.Enums.MonsterQuality.Boss:
                    //case Enigma.D3.Enums.MonsterQuality.Minion:
                        return true;

                    default:
                        if(IsTreasureGoblin(monster))
                            return true;
                        return false;
                }
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool isBoss(ActorCommonData monster)
        {
            try
            {
                return monster.x0B8_MonsterQuality == MonsterQuality.Boss;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool isEliteBlue(ActorCommonData monster)
        {
            try
            {
                return monster.x0B8_MonsterQuality == MonsterQuality.Champion;
            }
            catch (Exception)
            {

                return false;
            }
        }

        public static bool isEliteYellow(ActorCommonData monster)
        {
            try
            {
                return monster.x0B8_MonsterQuality == MonsterQuality.Rare;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static bool isEliteIllusionist(ActorCommonData monster)
        {
            try
            {
                //if (hasAffix_Illusionist(monster))
                //{
                //    A_Handler.Log.LogEntry.addLogEntry("------------------------------------------------------");
                //    A_Handler.Log.LogEntry.addLogEntry("Illusionist Affix  : " + hasAffix_Illusionist(monster));
                //    A_Handler.Log.LogEntry.addLogEntry("SummonedByACDID    : " + monster.GetAttributeValue(AttributeId.SummonedByACDID));
                //    A_Handler.Log.LogEntry.addLogEntry("SummonedByAutocast : " + monster.GetAttributeValue(AttributeId.SummonedByAutocast));
                //    A_Handler.Log.LogEntry.addLogEntry("SummonedBySNO      : " + monster.GetAttributeValue(AttributeId.SummonedBySNO));
                //    A_Handler.Log.LogEntry.addLogEntry("SummonerID         : " + monster.GetAttributeValue(AttributeId.SummonerID));
                //    A_Handler.Log.LogEntry.addLogEntry("Illusion           : " + monster.GetAttributeValue(AttributeId.Illusion));

                //}
                //return false;

                return monster.GetAttributeValue(AttributeId.SummonedByACDID) != -1; //if summoned illu this is != -1
            }
            catch (Exception)
            {
                return false;
            }
        }



        public static double get_HitpointsPercentage(ActorCommonData monster)
        {
            try
            {
                double currentHitpoints = monster.x180_Hitpoints;
                double totalHitpoints = monster.GetAttributeValue(AttributeId.HitpointsMaxTotal);
                double currentPercentage = currentHitpoints / totalHitpoints * 100;

                return currentPercentage;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public static bool hasAffix_ArcaneEnchanted(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    214594,
                    221130,
                    221219,
                    219671,
                    214791,
                    384426,
                    392128,
                    384436,

                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false;}
        }
        public static bool hasAffix_Avenger(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    226292,
                    226289,
                    

                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Desecrator(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    156106,
                    221131,
                    156105,

                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Electrified(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    81420,
                    365083,
                    109899,
                    169461
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_ExtraHealth(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    70650
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Fast(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    70849
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Frozen(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    90144,
                    231149,
                    231157
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Healthlink(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                   71239
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Illusionist(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                   71108,
                   264185

                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Jailer(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                   222743,
                   222745,
                   222744,
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Knockback(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                  70655
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Firechains(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                  226497
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Molten(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                  90314,
                  109898
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Mortar(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                  215756,
                  215757
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Nightmarish(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                 247258
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Plagued(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                 90566,
                 231115
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_ReflectsDamage(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    230877,
                    285770
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Shielding(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    226437,
                    226438
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Teleporter(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    155958,
                    155959

                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Thunderstorm(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    336177,
                    336178,
                    336179
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Vortex(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    81615,
                    120306,
                    221132,
                    120305
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }
        public static bool hasAffix_Waller(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                    226293,
                    226294,
                    231117,
                    231118
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }


        public static bool hasAffix_Juggernaut(ActorCommonData monster)
        {
            try
            {
                List<int> AffixPowers = new List<int>()
                {
                   455436
                };

                return isBuff(AffixPowers, monster);
            }
            catch { return false; }
        }


        public class ConventionOfElements
        {
            


            public static A_Enums.DamageType getConventionElement(ActorCommonData acd)
            {
               acd.TakeSnapshot();

                int index = Array.FindIndex(A_Collection.Presets.ConventionElements.Convention_BuffcountAttribIdRotation,
                    a => a == A_Collection.Presets.ConventionElements.Convention_BuffcountAttribIdRotation.FirstOrDefault(
                        x => isBuff(A_Enums.Powers.Convention_PowerSno, x, acd)));

                if(index < 0)
                    return DamageType.none;

                A_Enums.DamageType element = A_Collection.Presets.ConventionElements.Convention_ElementRotation[index];
                acd.FreeSnapshot();

                return element;
            }

            public static A_Enums.DamageType getNextConventionElement(A_Enums.DamageType CurrentElement, A_Enums.HeroClass Class)
            {
                var class_elements = A_Collection.Presets.ConventionElements.Convention_ClassElements.First(x => x.Key == Class).Value;

                int currentIndex = Array.FindIndex(A_Collection.Presets.ConventionElements.Convention_ElementRotation, x => x == CurrentElement);

                int class_currentindex = Array.FindIndex(class_elements, x => x == CurrentElement);

                if (currentIndex < 0)
                    return DamageType.none;

                if (class_currentindex == class_elements.Count() - 1)
                    return class_elements[0];

                return class_elements[class_currentindex + 1];
            }

            public static double getConventionTicksLeft(A_Enums.DamageType CurrentElement, ActorCommonData acd)
            {
                acd.TakeSnapshot();

                int currentIndex = Array.FindIndex(A_Collection.Presets.ConventionElements.Convention_ElementRotation, x => x == CurrentElement);

                if (currentIndex < 0)
                    return 0;

                int AttribId_BuffEndTick = A_Collection.Presets.ConventionElements.Convention_BuffEndtickAttribIdRotation[currentIndex];

                double ticksleft = getBuffCount(A_Enums.Powers.Convention_PowerSno, AttribId_BuffEndTick, acd) - A_Collection.Environment.Scene.GameTick;

                acd.FreeSnapshot();

                return ticksleft;
            }
        }
    }
}
