using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

namespace D3Helper.A_Collector
{
    public class ACD
    {
        public ACD(ActorCommonData _acd, bool isMonster, bool isPlayer, double distance, double progress)
        {
            this._ACD = _acd;
            this.IsMonster = isMonster;
            this.IsPlayer = isPlayer;
            this.Distance = distance;
            this.Progress = progress;
        }

        public ActorCommonData _ACD { get; set; }
        public bool IsMonster { get; set; }
        public bool IsPlayer { get; set; }
        public double Distance { get; set; }
        public double Progress { get; set; }

    }


    class IC_Actors
    {
        public static void Collect()
        {
            try
            {
                get_AllActors();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_AllActors()
        {
            try
            {
                lock(A_Collection.Environment.Actors.AllActors)
                {
                    lock(A_Collection.Me.HeroDetails.EquippedItems)
                    {
                        A_Collection.Environment.Actors.AllActors.Clear();
                        A_Collection.Me.HeroDetails.EquippedItems.Clear();

                        Predicate<Enigma.D3.Enums.ItemLocation> isEquipped = (location) =>
                        location >= Enigma.D3.Enums.ItemLocation.PlayerHead &&
                        location <= Enigma.D3.Enums.ItemLocation.PlayerNeck;

                        var acdcontainer = ActorCommonData.Container.ToList();

                        
                        for(int i = 0; i < acdcontainer.Count(); i++)
                        {
                            if(acdcontainer[i].x000_Id == -1)
                                continue;

                            ActorCommonData acd = acdcontainer[i];
                            
                           
                            bool isMonster = false;
                            isMonster = A_Tools.T_ACD.IsValidMonster(acd); // Experimental

                            bool isPlayer = false;
                            if (!isMonster && acd.x17C_ActorType == ActorType.Player)
                                isPlayer = true;
                            
                            double progress = 0;
                            if (isMonster && Properties.Settings.Default.overlayriftprogress)
                                progress = A_Tools.T_ACD.get_RiftProgress(acd);

                            double distance = A_Tools.T_ACD.get_Distance(acd.x0D0_WorldPosX, acd.x0D4_WorldPosY);
                            
                            A_Collection.Environment.Actors.AllActors.Add(new ACD(acd, isMonster, isPlayer, distance, progress));

                            if (isEquipped(acd.x114_ItemLocation))
                            {
                                A_Collection.Me.HeroDetails.EquippedItems.Add(acd);
                            }
                        }

                        
                    }
                }       
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }
    }
}
