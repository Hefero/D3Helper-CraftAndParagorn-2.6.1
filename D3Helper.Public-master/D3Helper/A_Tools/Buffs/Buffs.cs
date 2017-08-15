using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

namespace D3Helper.A_Tools.Buffs
{
    class Buffs
    {
        public class B_Global
        {
            public static int get_PartyMembers_WithBuff(int SnoPowerId, int AttribId, double PowerMaxDistance, out int PartyMembersInRange)
            {
                PartyMembersInRange = 0;

                try
                {
                    var local = A_Collection.Me.HeroGlobals.LocalACD;

                    List<A_Collector.ACD> partyMemberContainer;
                    lock (A_Collection.Environment.Actors.AllActors)
                        partyMemberContainer =
                            A_Collection.Environment.Actors.AllActors.ToList()
                                .Where(x => x._ACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player)
                                .ToList();

                    int counter = 0;

                    if (partyMemberContainer.Count > 1)
                        PartyMembersInRange = partyMemberContainer.Count - 1;

                    foreach (var member in partyMemberContainer)
                    {

                        if (member.Distance <= PowerMaxDistance && member.Distance >= 3)
                        {
                            if (A_Tools.T_ACD.isBuff(SnoPowerId, AttribId, member._ACD))
                            {
                                counter++;
                            }

                        }

                    }

                    return counter;


                }
                catch
                {
                    return 0;
                }
            }
            public static int get_PartyMembers_WithoutBuff(int SnoPowerId, int AttribId, double PowerMaxDistance, out int PartyMembersInRange)
            {
                PartyMembersInRange = 0;

                try
                {
                    var local = A_Collection.Me.HeroGlobals.LocalACD;

                    List<A_Collector.ACD> partyMemberContainer;
                    lock (A_Collection.Environment.Actors.AllActors)
                        partyMemberContainer =
                            A_Collection.Environment.Actors.AllActors.ToList()
                                .Where(x => x._ACD.x17C_ActorType == Enigma.D3.Enums.ActorType.Player)
                                .ToList();

                    int counter = 0;

                    if (partyMemberContainer.Count > 1)
                        PartyMembersInRange = partyMemberContainer.Count - 1;
                   
                    foreach (var member in partyMemberContainer)
                    {

                        if (member.Distance <= PowerMaxDistance && member.Distance >= 3)
                        {
                            if (!A_Tools.T_ACD.isBuff(SnoPowerId, AttribId, member._ACD))
                            {
                                counter++;
                            }

                        }

                    }

                    return counter;


                }
                catch
                {
                    return 0;
                }
            }
        }
        public class B_WitchDoctor
        {
            public static int get_ZombieDogCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;
                    var alldogs = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("zombiedog"));

                    return alldogs.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 8);
                }
                catch { return 0; }
            }
            public static int get_FetishCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                    var allfetishes = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("fetish"));

                    return allfetishes.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 11);
                }
                catch { return 0; }
            }
            public static int get_GargantuanCount()
            {
                try
                {
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;

                    var allgargantuans = acdcontainer.Where(x => x._ACD.x000_Id != -1 && x._ACD.GetAttributeValue(AttributeId.PetOwner) == local.x0000_Index && x._ACD.x004_Name.ToLower().Contains("gargantuan"));

                    return allgargantuans.Count(x => x._ACD.GetAttributeValue(AttributeId.PetType) == 10);
                }
                catch { return 0; }
            }
        }
        public class B_Necromancer
        {
            public static int get_SkeletalMageCount()
            {

                try
                {
                    HashSet<int> skeletonMageActorSNOs = new HashSet<int>
                    {
                        472275, // Skeleton Mage - No Rune
                        472588, // Skeleton Mage - Gift of Death
                        472769, // Skeleton Mage - Contamination
                        472801, // Skeleton Mage - Archer
                        472606, // Skeleton Mage - Singularity
                        472715  // Skeleton Mage - Life Support
                    };
                    List<A_Collector.ACD> acdcontainer;
                    lock (A_Collection.Environment.Actors.AllActors) acdcontainer = A_Collection.Environment.Actors.AllActors.ToList();

                    PlayerData local;
                    lock (A_Collection.Me.HeroGlobals.LocalPlayerData)
                        local = A_Collection.Me.HeroGlobals.LocalPlayerData;
                    var allmages = acdcontainer.Where(EachActor => EachActor._ACD.x000_Id != -1 && skeletonMageActorSNOs.Contains(EachActor._ACD.x098_NonPlayerNonItemActorSnoId));

                    return allmages.Count(x => x._ACD.x004_Name.ToLower().Contains("skeletonmage"));
                }
                catch { return 0; }
            }
        }
    }
}
