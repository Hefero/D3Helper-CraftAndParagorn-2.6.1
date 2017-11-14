using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Enigma.D3;

using D3Helper.A_Collector;
using D3Helper.A_Tools;
using D3Helper.A_Handler.EventHandler;

namespace D3Helper.A_Collection
{
    public class SNOid
    {
        public SNOid(int sno)
        {
            this.SNO = sno;
        }
        public int SNO { get; set; }
    }
    public class LevelArea
    {
        public LevelArea(int act, string internalName, string name, int hostAreaSNO)
        {
            this.Act = act;
            this.InternalName = internalName;
            this.Name = name;
            this.HostAreaSNO = hostAreaSNO;
        }
        public int Act { get; set; }
        public string InternalName { get; set; }
        public string Name { get; set; }
        public int HostAreaSNO { get; set; }


        public static A_Collection.LevelArea get_LevelAreaNameBySnoId(int levelAreaSnoId)
        {
            try
            {
                A_Collection.LevelArea area = A_Collection.Environment.Areas.AreaList.Where(x => x.Key.SNO == levelAreaSnoId).First().Value;
                if (area != null)
                {
                    return area;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);
            }
            return null;
        }

    }
    class Environment
    {
        public class Scene
        {
            public static int Counter_CurrentFrame;
            public static int GameTick;
        }
        public class Actors
        {
            public static List<ACD> AllActors = new List<ACD>();
            public static List<ActorCommonData> MonstersInRange = new List<ActorCommonData>();
            public static List<ActorCommonData> EliteInRange = new List<ActorCommonData>();

            public static List<ActorCommonData> SelectedMonster_MonstersInRange = new List<ActorCommonData>();
            public static double RiftProgressInRange_Points;
            public static double RiftProgressInRange_Percentage;
            public static double RiftHitpointsInRange;
        }
        public class Areas
        {
            public static int currentArea_SnoId;                                // default: -1
            public static Dictionary<SNOid, LevelArea> AreaList = new Dictionary<SNOid, LevelArea>();
            public static int GreaterRift_Tier;
        }
        
    }
}
