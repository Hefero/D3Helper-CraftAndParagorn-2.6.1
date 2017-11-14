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
    class Th_Handler
    {


        private static int _tick;
        public static List<double> ProcessingTimes = new List<double>();
        public static int ExceptionCount = 0;

        public static void New_Handler()
        {
            Thread Handler = new Thread(new ThreadStart(Execute));

            Handler.SetApartmentState(ApartmentState.STA);
            Handler.Start();
        }
        private static void Execute()
        {
            EngineOptions engineOptions = new EngineOptions();
            engineOptions.VersionMatching = VersionMatching.MajorMinorBuild;


            //if (Program.SingleThreaded)
            //    while (Engine.Create(engineOptions) == null) { System.Threading.Thread.Sleep(50); }

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
                        Stopwatch s_handler = new Stopwatch();
                        s_handler.Start();
                        //

                        ////--SingleThreaded
                        //if (Program.SingleThreaded)
                        //{
                        //    A_Collector.IC_Player.Collect();
                        //    A_Collector.IC_Skills.Collect();
                        //    A_Collector.IC_Actors.Collect();
                        //    A_Collector.IC_Area.Collect();
                        //    A_Collector.IC_Preferences.Collect();
                        //    A_Collector.IC_D3UI.Collect();
                        //    A_Collector.IC_Party.Collect();
                        //}
                        ////

                        A_Collector.H_D3Client.Collect();

                        if (A_Collection.Environment.Scene.GameTick > 1 && A_Collection.Environment.Scene.Counter_CurrentFrame != 0 && A_Collection.Me.HeroStates.isInGame)
                        {
                            //A_Handler.HealthPotion.HealthPotion.handlePotion();
                            A_Handler.SkillHandler.SkillHandler.handleSkills();
                            A_Tools.InputSimulator.IS_Keyboard.ChannelKey_Maintain();
                            A_Handler.EventHandler.EventHandler.handleEvents();
                            A_Collector.H_Keyboard.Collect();
                        }

                        A_Handler.StatHandler.StatHandler.handleStats();
                        A_Collector.H_ExternalFiles.Collect();


                        if (A_Handler.Log.Exception.ExceptionLog.Count > 0)
                        {
                            A_Handler.Log.Exception.log_Exceptions();
                        }

                        if (A_Handler.Log.Exception.HandlerLog.Count > 0 && Properties.Settings.Default.Logger_extendedLog)
                        {
                            A_Handler.Log.Exception.log_Handler();
                        }

                        //--
                        s_handler.Stop();
                        TimeSpan t_handler = s_handler.Elapsed;

                        lock (ProcessingTimes)
                        {
                            ProcessingTimes.Add(t_handler.TotalMilliseconds);
                        }
                        if (ProcessingTimes.Count > 180)
                        {
                            lock (ProcessingTimes)
                            {
                                ProcessingTimes.RemoveAt(0);
                            }
                        }

                        double TimeLeftToNextTick = ((1000 / Properties.Settings.Default.D3Helper_UpdateRate) - t_handler.TotalMilliseconds);
                        if (TimeLeftToNextTick > 0)
                            System.Threading.Thread.Sleep((int)TimeLeftToNextTick);
                    }
                }
                catch (Exception e)
                {
                    A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.Handler);

                    System.Environment.Exit(1);
                }

            }

        }
    }
}
