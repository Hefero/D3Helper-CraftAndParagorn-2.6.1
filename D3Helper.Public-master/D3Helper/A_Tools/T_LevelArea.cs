using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Tools
{
    class T_LevelArea
    {
        public static bool IsRift()
        {
            try
            {
                int CurrentArea = A_Collection.Environment.Areas.currentArea_SnoId;

                switch(CurrentArea)
                {
                    case 288482:
                    case 288684:
                    case 288686:
                    case 288797:
                    case 288799:
                    case 288801:
                    case 288803:
                    case 288809:
                    case 288812:
                    case 288813:
                        return true;

                    default:
                        return false;
                }
                                
            }
            catch { return false; }
        }
        public static bool IsGRift()
        {
            try
            {
                if(A_Collection.Environment.Areas.GreaterRift_Tier == 0)
                    return false;
                
                int CurrentArea = A_Collection.Environment.Areas.currentArea_SnoId;

                switch (CurrentArea)
                {
                    case 288482:
                    case 288684:
                    case 288686:
                    case 288797:
                    case 288799:
                    case 288801:
                    case 288803:
                    case 288809:
                    case 288812:
                    case 288813:
                        return true;

                    default:
                        return false;
                }

            }
            catch { return false; }
        }
    }
}
