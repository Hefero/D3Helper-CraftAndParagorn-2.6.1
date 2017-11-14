using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D3Helper.A_Tools.Version
{
    class AppVersion
    {
        public const string version = "17.07.03.0";

        public static DateTime LatestOnlineVersion = new DateTime();

        //public static bool isOutdated()
        //{
        //    try
        //    {
        //        DateTime latest = get_LatestOnlineVersionDate();

        //        LatestOnlineVersion = latest;

        //        DateTime current = get_CurrentVersionDate();

        //        if (latest.AddDays(-2) > current)
        //            return true;

        //        return false;
        //    }
        //    catch (Exception)
        //    {
                
        //        return false;
        //    }
        //}


        public static DateTime get_LatestOnlineVersionDate()
        {
            //try
            //{
            //    return Convert.ToDateTime(A_TCPClient.TCPClient.send_Instruction(A_Enums.TCPInstructions.GetLatestVersion, ""));
            //}
            //catch { return DateTime.Now; }

            return DateTime.Now;
        }
        public static DateTime get_CurrentVersionDate()
        {
            try
            {
                DateTime currentVersionDate = DateTime.ParseExact(version, "yy.MM.d.H", System.Globalization.CultureInfo.InvariantCulture);

                return currentVersionDate;
            }
            catch { return DateTime.Now; }
        }
    }
}
