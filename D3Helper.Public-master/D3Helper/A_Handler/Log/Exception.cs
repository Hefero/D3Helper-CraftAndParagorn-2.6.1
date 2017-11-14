using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace D3Helper.A_Handler.Log
{
    public class ExceptionLogEntry
    {
        public ExceptionLogEntry(System.Exception e, DateTime timestamp, A_Enums.ExceptionThread exceptionthread)
        {
            this.E = e;
            this.Timestamp = timestamp;
            this.ExceptionThread = exceptionthread;
        }

        public System.Exception E { get; set; }
        public DateTime Timestamp { get; set; }
        public A_Enums.ExceptionThread ExceptionThread { get; set; }


        public static void addExceptionLogEntry(System.Exception e, A_Enums.ExceptionThread exceptionthread)
        {
            A_Handler.Log.ExceptionLogEntry newEntry = new A_Handler.Log.ExceptionLogEntry(e, DateTime.Now, exceptionthread);

            lock (A_Handler.Log.Exception.ExceptionLog) A_Handler.Log.Exception.ExceptionLog.Add(newEntry);
        }

    }


    public class LogEntry
    {
        public LogEntry(DateTime timestamp, string text)
        {
            
            this.Timestamp = timestamp;
            this.Text = text;
        }
        
        public DateTime Timestamp { get; set; }
        public string Text { get; set; }


        public static void addLogEntry(string text)
        {
            if (Properties.Settings.Default.Logger_extendedLog)
            {
                lock (A_Handler.Log.Exception.HandlerLog) A_Handler.Log.Exception.HandlerLog.Add(new A_Handler.Log.LogEntry(DateTime.Now, text));
            }
        }

    }


    class Exception
    {
        private static string ExceptionsFilePath = path.AppDir + @"\logs\exceptions.txt";
        private static string HandlerFilePath = path.AppDir + @"\logs\log_handler.txt";
        private static string LogFolderPath = path.AppDir + @"\logs";

        public static List<ExceptionLogEntry> ExceptionLog = new List<ExceptionLogEntry>();
        public static List<LogEntry> HandlerLog = new List<LogEntry>();

        public static int ExceptionCount = 0;

        public static void log_Exceptions()
        {
            bool enableExceptionLog = true;

            if(enableExceptionLog == false)
            {
                return;
            }


            try
            {
                List<ExceptionLogEntry> ExceptionsToWrite = new List<ExceptionLogEntry>();

                lock(ExceptionLog)
                {
                    foreach(var entry in ExceptionLog.ToList())
                    {
                        ExceptionsToWrite.Add(entry);

                        ExceptionLog.Remove(entry);
                    }
                }

                string WriteToFile = String.Empty;

                foreach(var entry in ExceptionsToWrite)
                {
                    WriteToFile = WriteToFile + entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,ffff") + "\t" + entry.ExceptionThread.ToString() + "\t" + entry.E.Message + "\t" + entry.E.StackTrace + Environment.NewLine;

                    ExceptionCount++;
                }

                if (!Directory.Exists(LogFolderPath)) Directory.CreateDirectory(LogFolderPath);

                File.AppendAllText(ExceptionsFilePath, WriteToFile);
            }
            catch (System.Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }
        public static void log_Handler()
        {
            try
            {
                List<LogEntry> HandlerLogsToWrite = new List<LogEntry>();

                lock (HandlerLog)
                {
                    foreach (var entry in HandlerLog.ToList())
                    {
                        HandlerLogsToWrite.Add(entry);

                        HandlerLog.Remove(entry);
                    }
                }

                string WriteToFile = String.Empty;

                foreach (var entry in HandlerLogsToWrite)
                {
                    WriteToFile = WriteToFile + entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss,ffff") + "\t" + entry.Text + Environment.NewLine;

                    //ExceptionCount++;
                }

                if (!Directory.Exists(LogFolderPath)) Directory.CreateDirectory(LogFolderPath);

                File.AppendAllText(HandlerFilePath, WriteToFile);
            }
            catch (System.Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);
            }
        }

        public static void addExceptionLogEntry(System.Exception e, A_Enums.ExceptionThread exceptionthread)
        {
            ExceptionLogEntry.addExceptionLogEntry(e, exceptionthread);
        }
    }
}
