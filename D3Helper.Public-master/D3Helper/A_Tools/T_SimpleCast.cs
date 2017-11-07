using D3Helper.A_Tools.InputSimulator;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3Helper.A_Tools
{
    class T_SimpleCast
    {
        private static string simpleCast_File = path.AppDir + @"\simplecast.xml";

        private static DataTable globalDataTable = null;


        public static void bindListToCombobox(ComboBox combobox,List<string> list)
        {
            BindingSource bs = new BindingSource();
            bs.DataSource = list;
            combobox.DataSource = bs;
        }



        public static DataTable load_to_DataTable(String xmlfile)
        {
            try
            {
                DataTable dt = new DataTable("SimpleCastTimings");

                dt.Columns.Add(new DataColumn("name", typeof(string)));
                dt.Columns.Add(new DataColumn("skill_1", typeof(int)));
                dt.Columns.Add(new DataColumn("skill_2", typeof(int)));
                dt.Columns.Add(new DataColumn("skill_3", typeof(int)));
                dt.Columns.Add(new DataColumn("skill_4", typeof(int)));
                dt.Columns.Add(new DataColumn("lmb", typeof(int)));
                dt.Columns.Add(new DataColumn("rmb", typeof(int)));

                if (File.Exists(xmlfile))
                {
                    dt.ReadXml(xmlfile);
                }

                return dt;
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }

            return null;
        }




        public static DataTable load_to_datagrid(DataGridView grid)
        {
            try
            {
                DataTable dt = load_to_DataTable(simpleCast_File);

                if(grid != null)
                {
                    grid.AllowUserToAddRows = true;
                    grid.AutoGenerateColumns = true;

                    grid.DataSource = dt;

                    //column width
                    grid.Columns[0].Width = 100;
                    grid.Columns[1].Width = 50;
                    grid.Columns[2].Width = 50;
                    grid.Columns[3].Width = 50;
                    grid.Columns[4].Width = 50;
                    grid.Columns[5].Width = 50;
                }

                return dt;
            }
            catch (Exception e) {
                Console.WriteLine("An error occurred: '{0}'", e);
            }

            return null;
        }

        public static void save_from_datagrid(DataGridView grid)
        {
            try
            {
                grid.EndEdit();
                DataTable dt = (DataTable)grid.DataSource;
                save_from_datatable(dt);
                globalDataTable = dt;
            }
            catch (Exception e) {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }

        public static void save_from_datatable(DataTable dt)
        {
            try
            {
                dt.WriteXml(simpleCast_File);
            }
            catch (Exception e)
            {
                Console.WriteLine("An error occurred: '{0}'", e);
            }
        }


        public static List<String> getSimpleCastNames()
        {
            if (globalDataTable == null)
            {
                globalDataTable = load_to_DataTable(simpleCast_File);
            }

            List<String> names = new List<String>();

            foreach(DataRow row in globalDataTable.Rows)
            {
                names.Add(row["name"].ToString());
            }

            return names;
        }


        public static void StartSimpleCastThread()
        {
            Thread Handler = new Thread(new ThreadStart(Execute));

            Handler.SetApartmentState(ApartmentState.STA);
            Handler.Start();
        }

        private static void Execute()
        {
            //if (!Window_Main.d3helperform.SupportedProcessVersion())
            //{

            //}

            if(globalDataTable == null)
            {
                globalDataTable = load_to_DataTable(simpleCast_File);
            }


            int skill_1_second_since_last_cast = -1;
            int skill_2_second_since_last_cast = -1;
            int skill_3_second_since_last_cast = -1;
            int skill_4_second_since_last_cast = -1;

            int lmb_second_since_last_cast = -1;
            int rmb_second_since_last_cast = -1;



            while (true)
            {
                Thread.Sleep(50); //reduce cpu usage

                //only cast if Diablo III Window is in Foreground!
                if (!A_Tools.T_D3Client.IsForeground())
                {
                    continue;
                }

                if (!Window_Main.d3helperform.isSimpleCastEnabled)
                {
                    continue;
                }

                string selectedName = Window_Main.d3helperform.selectedSimpleCastName;

                foreach(DataRow row in globalDataTable.Rows)
                {
                    try
                    {
                        if (row["name"].Equals(selectedName))
                        {

                            try
                            {
                                int s1 = (int) row["skill_1"];
                                if(s1 > 0)
                                {
                                    if (skill_1_second_since_last_cast == -1 || skill_1_second_since_last_cast == s1)
                                    {
                                        //cast
                                        WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.VK_1);
                                        skill_1_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        skill_1_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };

                            try
                            {
                                int s2 = (int)row["skill_2"];
                                if (s2 > 0)
                                {
                                    if (skill_2_second_since_last_cast == -1 || skill_2_second_since_last_cast == s2)
                                    {
                                        //cast
                                        WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.VK_2);
                                        skill_2_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        skill_2_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };

                            try
                            {
                                int s3 = (int)row["skill_3"];
                                if (s3 > 0)
                                {
                                    if (skill_3_second_since_last_cast == -1 || skill_3_second_since_last_cast == s3)
                                    {
                                        //cast
                                        WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.VK_3);
                                        skill_3_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        skill_3_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };


                            try
                            {
                                int s4 = (int)row["skill_4"];
                                if (s4 > 0)
                                {
                                    if (skill_4_second_since_last_cast == -1 || skill_4_second_since_last_cast == s4)
                                    {
                                        //cast
                                        WindowsInput.InputSimulator.SimulateKeyPress(WindowsInput.VirtualKeyCode.VK_4);
                                        skill_4_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        skill_4_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };

                            try
                            {
                                int lmb = (int)row["lmb"];
                                if (lmb > 0)
                                {
                                    if (lmb_second_since_last_cast == -1 || lmb_second_since_last_cast == lmb)
                                    {
                                        //cast
                                        IS_Mouse.LeftClick();
                                        lmb_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        lmb_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };


                            try
                            {
                                int rmb = (int)row["rmb"];
                                if (rmb > 0)
                                {
                                    if (rmb_second_since_last_cast == -1 || rmb_second_since_last_cast == rmb)
                                    {
                                        //cast
                                        IS_Mouse.RightCLick();
                                        rmb_second_since_last_cast = 0;
                                    }
                                    else
                                    {
                                        rmb_second_since_last_cast++;
                                    }
                                }
                            }
                            catch (Exception) { };


                        }

                    }catch(Exception) {}
                }

                System.Threading.Thread.Sleep(1000);
            }

        }


    }
}
