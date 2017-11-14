using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Diagnostics;

using Enigma.D3;
using Enigma.D3.Memory;

namespace D3Helper.A_Initialize
{
    class Th_ICollector
    {
        private static int _tick = -1;
        public static List<double> ProcessingTimes = new List<double>();
        public static int ExceptionCount = 0;

        public static void New_ICollector()
        {
            Thread ICollector = new Thread(new ThreadStart(Execute));

            ICollector.SetApartmentState(ApartmentState.STA);
            ICollector.Start();
        }
        private static void Execute()
        {
            EngineOptions engineOptions = new EngineOptions();
            engineOptions.VersionMatching = VersionMatching.MajorMinorBuild;

            while(Engine.Create(engineOptions) == null) { System.Threading.Thread.Sleep(50); }

            
            while (Engine.Current == null) { System.Threading.Thread.Sleep(1); }

            
            while (true)
            {
                try
                {
                    Thread.Sleep(16); //reduce cpu usage
                   
                    var tick = Engine.Current.ApplicationLoopCount;
                    if (tick != _tick)
                    {
                        _tick = tick;


                        //--
                        Stopwatch s_collector = new Stopwatch();
                        s_collector.Start();
                        //
                        A_Collector.IC_Player.Collect();
                        A_Collector.IC_Skills.Collect();
                        A_Collector.IC_Actors.Collect();
                        A_Collector.IC_Area.Collect();
                        A_Collector.IC_Preferences.Collect();
                        A_Collector.IC_D3UI.Collect();
                        A_Collector.IC_Party.Collect();
                        
                        //--
                        s_collector.Stop();
                        TimeSpan t_collector = s_collector.Elapsed;

                        lock (ProcessingTimes)
                        {
                            ProcessingTimes.Add(t_collector.TotalMilliseconds);
                        }
                        if (ProcessingTimes.Count > 180)
                        {
                            lock (ProcessingTimes)
                            {
                                ProcessingTimes.RemoveAt(0);
                            }
                        }

                        double TimeLeftToNextTick = ((1000 / Properties.Settings.Default.D3Helper_UpdateRate) - t_collector.TotalMilliseconds);
                        if (TimeLeftToNextTick > 0)
                            System.Threading.Thread.Sleep((int)TimeLeftToNextTick);
                    }
                }
                catch (Exception e)
                {
                    A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ICollector);

                    System.Environment.Exit(1);
                }

            }
            
        }
    }
}
