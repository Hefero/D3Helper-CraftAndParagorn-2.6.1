using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Collection
{
    class Stats
    {
        public class Player
        {
            public static long TotalXP;                                                           // default: -1 min: 0
            public static double Progression;                                                     // default: -1 min: 0 max: 100
            public static double NextRoundedHundred;                                              // default: -1 min: 100 max: 10000
            public static double NextRoundedHundredTotalXP;                                       // default: -1 min: 0
        }
    }
}
