using System;
using System.IO;
using System.Text;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using D3Helper.A_Enums;

namespace D3Helper.A_TCPClient
{

    class TCPClient
    {
        //private enum _ObjectType
        //{
        //    None = -1,
        //    _String = 0,
        //    _Int = 1,
        //    _Bool = 2,
        //    _DateTime = 3,
        //    List_string = 4
        //}

        //private const string ServerAdrdress = "81.169.155.242";
        //private const int ServerPort = 6688;

        //public static object send_Instruction(A_Enums.TCPInstructions Instruction, string Data)
        //{
        //    try
        //    {
        //        using (var tcpclnt = new TcpClient())
        //        {
        //            tcpclnt.SendTimeout = 5000;
        //            tcpclnt.ReceiveTimeout = 5000;

        //            tcpclnt.Connect(ServerAdrdress, ServerPort);
                    
        //            while (!tcpclnt.Connected)
        //                System.Threading.Thread.Sleep(10);

        //            String instruction = "";

        //            switch (Instruction)
        //            {
        //                case A_Enums.TCPInstructions.ValidateBTag:
        //                    instruction = "1-$" + Data + "$";
        //                    break;

        //                case A_Enums.TCPInstructions.ValidateClanTag:
        //                    instruction = "2-$" + Data + "$";
        //                    break;

        //                case A_Enums.TCPInstructions.WriteLogEntry:
        //                    instruction = "3-$" + Data + "$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetLatestVersion:
        //                    instruction = "4-$$";
        //                    break;

        //                case A_Enums.TCPInstructions.WriteBugReport:
        //                    instruction = "5-$" + Data + "$";
        //                    break;

        //                case A_Enums.TCPInstructions.WriteSuggestionsReport:
        //                    instruction = "6-$" + Data + "$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetTotalUniqueUsers:
        //                    instruction = "7-$$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetActiveUsers:
        //                    instruction = "8-$$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetChangelog:
        //                    instruction = "9-$$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetBugReports:
        //                    instruction = "10-$$";
        //                    break;

        //                case A_Enums.TCPInstructions.GetSuggestions:
        //                    instruction = "11-$$";
        //                    break;

        //                case TCPInstructions.GetPowers:
        //                    instruction = "12-$$";
        //                    break;

        //                case TCPInstructions.StorePower:
        //                    instruction = "13-$" + Data + "$";
        //                    break;

        //            }

        //            using (var stm = tcpclnt.GetStream())
        //            {
        //                ASCIIEncoding asen = new ASCIIEncoding();
        //                byte[] ba = asen.GetBytes(instruction);
                        

        //                stm.Write(ba, 0, ba.Length);

        //                byte[] bb = new byte[204800];
        //                int k = stm.Read(bb, 0, 204800);

        //                int _DataSize = BitConverter.ToInt32(bb, 0);
        //                int _Header = BitConverter.ToInt32(bb, 4);

        //                //while (k < 4 + 4 + _DataSize)
        //                //    System.Threading.Thread.Sleep(5);
        //                //while (k < 4 + 4 + _DataSize)
        //                //    k = stm.Read(bb, k, 102400);

        //                byte[] _Data = new byte[_DataSize];
        //                Buffer.BlockCopy(bb, 8, _Data, 0, _DataSize);

        //                _ObjectType Type = (_ObjectType)_Header;

                        

        //                switch (Type)
        //                {
        //                    case _ObjectType._String:
        //                        string _String = Encoding.ASCII.GetString(_Data, 0, _Data.Length);
        //                        return _String as string;
                                

        //                    case _ObjectType._Int:
        //                        string _Int = Encoding.ASCII.GetString(_Data, 0, _Data.Length);
								//if (_Int == "false")
								//	return 0;
								//if (_Int == "true")
								//	return 1;
        //                        return int.Parse(_Int);
                               

        //                    case _ObjectType._DateTime:
        //                        string _DateTime = Encoding.ASCII.GetString(_Data, 0, _Data.Length);
        //                        return DateTime.ParseExact(_DateTime, "yy.MM.d.H", System.Globalization.CultureInfo.InvariantCulture);
                                
        //                    case _ObjectType._Bool:
        //                        string _Bool = Encoding.ASCII.GetString(_Data, 0, _Data.Length);
        //                        return Convert.ToBoolean(_Bool);
                                
        //                    case _ObjectType.List_string:
        //                        List<string> list = new List<string>();
        //                        list = ByteArrayToList(_Data);
        //                        return list as List<string>;

        //                    case _ObjectType.None:
        //                        return null;
        //                }
        //            }
        //        }
        //        return null;
        //    }
        //    catch { return null; }
        //}
        //private static List<string> ByteArrayToList(byte[] ListAsBytes)
        //{
        //    List<string> List;

        //    using (var ms = new MemoryStream(ListAsBytes))
        //    {
        //        BinaryFormatter bs = new BinaryFormatter();
        //        List = bs.Deserialize(ms) as List<string>;
        //    }
        //    return List;
        //}

        //public static Dictionary<int, string> ConvertToCustomPowerNames(List<string> RawRecievedPowersList)
        //{
        //    try
        //    {
        //        Dictionary<int,string> Buffer = new Dictionary<int, string>();

        //        foreach (var rawdata in RawRecievedPowersList)
        //        {
        //            if(rawdata.Length < 3 || !rawdata.Contains("\t"))
        //                continue;
                    
        //            int PowerSNO = int.Parse(rawdata.Split('\t')[0]);
        //            string Name = rawdata.Split('\t')[1];

        //            if(!Buffer.ContainsKey(PowerSNO))
        //                Buffer.Add(PowerSNO, Name);
        //        }

        //        return Buffer;
        //    }
        //    catch (Exception)
        //    {
                
        //        return new Dictionary<int, string>();
        //    }
        //} 
    }
}
