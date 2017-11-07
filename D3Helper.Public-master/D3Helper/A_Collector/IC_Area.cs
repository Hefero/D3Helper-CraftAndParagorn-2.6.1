using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using Enigma.D3;
using Enigma.D3.Enums;
using Enigma.D3.Helpers;

namespace D3Helper.A_Collector
{
    class IC_Area
    {
        public static void Collect()
        {
            try
            {
                if (A_Collection.Me.HeroStates.isInGame && A_Collection.Environment.Scene.GameTick > 1 && A_Collection.Environment.Scene.Counter_CurrentFrame != 0)
                {
                    get_currentLevelArea();
                    if (!A_Collection.Me.HeroStates.isInTown && A_Collection.Me.HeroGlobals.LocalACD != null)
                    {
                        get_currentGRiftTier();

                        if (Properties.Settings.Default.overlayriftprogress)
                            get_RiftProgressInRange();
                    }
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_currentLevelArea()
        {
            try
            {
                var levelArea = Enigma.D3.LevelArea.Instance.x044_SnoId;

                if (A_Collection.Environment.Areas.currentArea_SnoId != levelArea)
                    A_Collection.Me.HeroDetails.SnapShotted_APS = A_Collection.Me.HeroDetails.AttacksPerSecondTotal;

                A_Collection.Environment.Areas.currentArea_SnoId = levelArea;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_currentGRiftTier()
        {
            try
            {
                ActorCommonData Local;
                lock (A_Collection.Me.HeroGlobals.LocalACD) Local = A_Collection.Me.HeroGlobals.LocalACD;

                if(Local != null)
                    A_Collection.Environment.Areas.GreaterRift_Tier = (int)Local.GetAttributeValue(AttributeId.InTieredLootRunLevel) + 1;
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }


        private static void get_RiftProgressInRange()
        {
            try
            {
                List<ACD> acds;
                lock (A_Collection.Environment.Actors.AllActors)
                    acds = A_Collection.Environment.Actors.AllActors.ToList();

                double radius = Properties.Settings.Default.riftprogress_radius;
                var inrange = acds.Where(x => x.Distance <= radius && x.IsMonster);

                var progress = inrange.Sum(x => x.Progress);
                //var hitpoints = inrange.Sum(x => x._ACD.x188_Hitpoints);

                //A_Collection.Environment.Actors.RiftHitpointsInRange = hitpoints;

                A_Collection.Environment.Actors.RiftProgressInRange_Points = progress;

                A_Collection.Environment.Actors.RiftProgressInRange_Percentage =
                    A_Tools.T_ACD.convert_ProgressPointsToPercentage(progress);

            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
        }
        
    }
}
