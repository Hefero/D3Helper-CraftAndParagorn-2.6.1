using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using D3Helper.A_Collection;
using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

using D3Helper.A_Collector;
using DamageType = D3Helper.A_Enums.DamageType;

namespace D3Helper.A_Tools
{
    class T_LocalPlayer
    {
        public class PowerCollection
        {
            public static bool isSkillOverride(int AttributeID)
            {
                try
                {
                    return AttributeID == (int)AttributeId.SkillOverride;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public static bool isBuffCount(int AttributeID)
            {
                try
                {
                    return AttributeID >= (int)AttributeId.BuffIconCount0 && AttributeID <= (int)AttributeId.BuffIconCount31;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public static bool isBuffStartTick(int AttributeID)
            {
                try
                {
                    return AttributeID >= (int)AttributeId.BuffIconStartTick0 && AttributeID <= (int)AttributeId.BuffIconStartTick31;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public static bool isBuffEndTick(int AttributeID)
            {
                try
                {
                    return AttributeID >= (int)AttributeId.BuffIconEndTick0 && AttributeID <= (int)AttributeId.BuffIconEndTick31;
                }
                catch (Exception)
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// get number of alive party members
        /// </summary>
        /// <returns></returns>
        public static int get_PartyMemberAlive()
        {
            try
            {
                ActorCommonData local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) local = A_Collection.Me.HeroGlobals.LocalACD;

                List<A_Collector.ACD> partyMemberContainer;
                lock (A_Collection.Environment.Actors.AllActors) partyMemberContainer = A_Collection.Environment.Actors.AllActors.ToList().Where(x => x._ACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player && x._ACD.x000_Id != local.x000_Id).ToList();

                int counter = 0;

                foreach (var member in partyMemberContainer)
                {
                    if (member._ACD.GetAttributeValue(AttributeId.HitpointsCur) > 0) // is partymember alive/not dead?
                    {
                        counter++;
                    }
                }

                return counter;
            }
            catch
            {
                return 0;
            }
        }

        public static int get_PartyMemberInRange(double Distance)
        {
            try
            {
                ActorCommonData local;
                lock(A_Collection.Me.HeroGlobals.LocalACD) local = A_Collection.Me.HeroGlobals.LocalACD;

                List<A_Collector.ACD> partyMemberContainer;
                lock (A_Collection.Environment.Actors.AllActors) partyMemberContainer = A_Collection.Environment.Actors.AllActors.ToList().Where(x => x._ACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player && x._ACD.x000_Id != local.x000_Id).ToList();

                int counter = 0;

                foreach (var member in partyMemberContainer)
                {
                    if (member.Distance <= Distance && member.Distance >= 3)
                    {
                        counter++;
                    }
                }

                return counter;
            }
            catch { return 0; }
        }


        public static bool IsPartyMemberInRange_MinHitpoints(double Distance, int MinHitpoints)
        {
            try
            {
                ActorCommonData local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) local = A_Collection.Me.HeroGlobals.LocalACD;


                List<A_Collector.ACD> partyMemberContainer;
                lock (A_Collection.Environment.Actors.AllActors) partyMemberContainer = A_Collection.Environment.Actors.AllActors.ToList().Where(x => x._ACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player && x._ACD.x000_Id != local.x000_Id).ToList();


                foreach (var member in partyMemberContainer)
                {


                    if (member.Distance <= Distance && member.Distance >= 3)
                    {
                        var CurHp = member._ACD.GetAttributeValue(AttributeId.HitpointsCur);
                        var TotalHp = member._ACD.GetAttributeValue(AttributeId.HitpointsMaxTotal);
                        var CurPercentage = (CurHp / TotalHp) * 100;

                        if (CurPercentage <= MinHitpoints)
                            return true;

                    }

                }

                return false;


            }
            catch { return false; }
        }
        public static ActorCommonData get_SelectedAtackableAcd(out double distance, out ActorType type)
        {
            distance = 0;
            type = ActorType.Invalid;

            try
            {


                
                var ListB = Engine.Current.ObjectManager.xA30_PlayerInput.Dereference().x00_ListB_Of_ActorId.ToList();
                

                if (ListB.Count() > 0)
                {
                    var selectedActorId = ListB.ToList()[0];

                    
                    var selectedtype = Engine.Current.ObjectManager.xA30_PlayerInput.Dereference().x14_StructStart_Min56Bytes;

                    if (selectedtype == 44580)
                    {

                        List<A_Collector.ACD> acdcontainer;
                        lock (A_Collection.Environment.Actors.AllActors)
                            acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                        var acd = acdcontainer.FirstOrDefault(x => x._ACD.x08C_ActorId == selectedActorId);

                        if (acd != null)
                        {
                            
                            distance = acd.Distance;
                            type = acd._ACD.x17C_ActorType;
                            
                            return acd._ACD;
                        }


                    }
                }

                return null;

            }
            catch { return null; }
        }
        
        public static int get_BuffTicksLeft(int SkillSno, int AttribId)
        {
            try
            {
                List<ActivePower> activePowers;
                lock (A_Collection.Me.HeroDetails.ActivePowers) activePowers = A_Collection.Me.HeroDetails.ActivePowers.ToList();

                    // int currentFrame = Engine.Current.ObjectManager.x798_Storage.x0F0_GameTick;
                int currentFrame = A_Collection.Environment.Scene.GameTick;

                var endtick =
                    activePowers.FirstOrDefault(
                        x => x.PowerSnoId == SkillSno && x.AttribId == AttribId && A_Tools.T_LocalPlayer.PowerCollection.isBuffEndTick(x.AttribId) && x.Value != 0);

                if (endtick != null)
                {
                    int ticksleft = endtick.Value - currentFrame;

                    return ticksleft;
                }
                return 0;

            }
            catch { return 0; }
        }
        public static bool isBuff(int SnoId)
        {
            try
            {
                lock (A_Collection.Me.HeroDetails.ActivePowers)
                {

                    var activepowers = A_Collection.Me.HeroDetails.ActivePowers.ToList();

                    var tryGetBuff = activepowers.FirstOrDefault(x => x.PowerSnoId == SnoId && PowerCollection.isBuffCount(x.AttribId) && x.Value > 0);
                    if (tryGetBuff != null)
                    {

                        return true;

                    }

                    return false;
                }
            }
            catch { return false; }
        }
        public static bool isBuff(int SnoId, int AttribId)
        {
            try
            {
                lock (A_Collection.Me.HeroDetails.ActivePowers)
                {
                    
                    var tryGetBuff = A_Collection.Me.HeroDetails.ActivePowers.ToList().FirstOrDefault(x => x.PowerSnoId == SnoId && x.AttribId == AttribId && PowerCollection.isBuffCount(x.AttribId) && x.Value > 0);
                    if (tryGetBuff != null)
                    {

                        return true;

                    }
                    return false;
                }
            }
            catch { return false; }
        }
        public static int getBuffCount(int SnoId, int AttribId)
        {
            try
            {
                lock (A_Collection.Me.HeroDetails.ActivePowers)
                {
                    var tryGetBuff = A_Collection.Me.HeroDetails.ActivePowers.ToList().FirstOrDefault(x => x.PowerSnoId == SnoId && x.AttribId == AttribId && PowerCollection.isBuffCount(x.AttribId));
                    if (tryGetBuff != null)
                    {

                        return tryGetBuff.Value;

                    }
                    return 0;
                }
            }
            catch { return 0; }
        }

        public class ConventionOfElements
        {
            public static A_Enums.DamageType getConventionElement()
            {
                ActorCommonData local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) local = A_Collection.Me.HeroGlobals.LocalACD;

                int index = Array.FindIndex(Presets.ConventionElements.Convention_BuffcountAttribIdRotation,
                    a => a == Presets.ConventionElements.Convention_BuffcountAttribIdRotation.FirstOrDefault(
                        x => isBuff(A_Enums.Powers.Convention_PowerSno, x)));

                if (index < 0)
                    return A_Enums.DamageType.none;

                return Presets.ConventionElements.Convention_ElementRotation[index];
            }

            public static A_Enums.DamageType getNextConventionElement(A_Enums.DamageType CurrentElement, A_Enums.HeroClass Class)
            {
                var class_elements = Presets.ConventionElements.Convention_ClassElements.First(x => x.Key == Class).Value;

                int currentIndex = Array.FindIndex(Presets.ConventionElements.Convention_ElementRotation, x => x == CurrentElement);

                int class_currentindex = Array.FindIndex(class_elements, x => x == CurrentElement);

                if (currentIndex < 0)
                    return DamageType.none;

                if (class_currentindex == class_elements.Count() - 1)
                    return class_elements[0];

                return class_elements[class_currentindex + 1];
            }

            public static double getConventionTicksLeft(A_Enums.DamageType CurrentElement)
            {
                int currentIndex = Array.FindIndex(Presets.ConventionElements.Convention_ElementRotation, x => x == CurrentElement);

                if (currentIndex < 0)
                    return 0;

                int AttribId_BuffEndTick = Presets.ConventionElements.Convention_BuffEndtickAttribIdRotation[currentIndex];

                double ticksleft = get_BuffTicksLeft(A_Enums.Powers.Convention_PowerSno, AttribId_BuffEndTick);

                return ticksleft;
            }
        }
    }
}
