using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Documents;

namespace D3Helper.A_Collector
{
    class H_ExternalFiles
    {
        private static bool OverridesLoaded = false;
        private static bool CustomDefinitionsLoaded = false;
        private static bool ParagonPointsLoaded = false;

        private static bool external_files_loaded = false;

        public static void Collect()
        {
            try
            {
                if (!external_files_loaded) //dont load external files every milliseconds, once should be enough
                {
                    load_CustomDefinitions();
                    load_AreaList();
                    load_MonsterRiftProgress();
                    load_SkillPowers();
                    load_SNOPowers();
                    load_ParagonPointSetups();
                    load_Tooltips_ConditionTypes();
                    load_AutoCastOverrides();
                    
                    external_files_loaded = true;
                }

                update_AutoCastOverrides();
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_AreaList()
        {
            try
            {
                if(A_Collection.Environment.Areas.AreaList.Count < 1)
                {
                    A_Tools.T_ExternalFile.AreaList.Load();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_MonsterRiftProgress()
        {
            try
            {
                if (A_Collection.Presets.Monsters.Monsters_RiftProgress.Count < 1)
                {
                    A_Tools.T_ExternalFile.Monsters.RiftProgress.Load();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_SkillPowers()
        {
            try
            {
                if (A_Collection.Presets.SkillPowers.AllSkillPowers.Count < 1)
                {
                    A_Tools.T_ExternalFile.SkillPowers.Load();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_CustomDefinitions()
        {
            try
            {
                if (!CustomDefinitionsLoaded)
                {
                    A_Collection.SkillCastConditions.Custom.CustomDefinitions = A_Tools.T_ExternalFile.CustomSkillDefinitions.Load();
                    A_Tools.T_ExternalFile.CustomSkillDefinitions.Fix();

                    CustomDefinitionsLoaded = true;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_Tooltips_ConditionTypes()
        {
            try
            {
                if (A_Collection.Presets.Manual.Tooltips.ConditionTypes.Count < 1)
                {
                    A_Tools.T_ExternalFile.Manual.ConditionType_Tooltips.Load();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_SNOPowers()
        {
            try
            {
                if (A_Collection.Presets.SNOPowers.AllPowers.Count < 1)
                {
                    A_Tools.T_ExternalFile.SNO_Powers.Load();
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }
        
        private static void load_ParagonPointSetups()
        {
            try
            {
                if(!ParagonPointsLoaded)
                {
                    A_Tools.T_ExternalFile.ParagonPointSpenderSettings.Load();

                    ParagonPointsLoaded = true;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void load_AutoCastOverrides()
        {
            try
            {
                if (!OverridesLoaded)
                {
                    A_Tools.T_ExternalFile.AutoCastOverrides.Load();

                    OverridesLoaded = true;
                }
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }


        private static void update_AutoCastOverrides()
        {
            try
            {
                A_Tools.T_ExternalFile.AutoCastOverrides.UpdateOverrides();              
            }
            catch (Exception e)
            {
                A_Handler.Log.Exception.addExceptionLogEntry(e, A_Enums.ExceptionThread.ECollector);
            }
        }
    }
}
