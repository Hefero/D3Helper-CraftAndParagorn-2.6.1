using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace D3Helper
{
    
    static class Program
    {
        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>

        //public static bool SingleThreaded = false;
        
        [STAThread]
        static void Main(string[] args)
        {
            //if(args.Length < 1)
            //    Environment.Exit(0);

            //if (args != null)
            //{
            //    for (int i = args.Length - 1; i >= 0; i--)
            //    {
            //        string arg = args[i];
            //        if (arg.StartsWith("appdir="))
            //        {
            //            path.AppDirOverride = arg.Split('=')[1];
            //            continue;
            //        }
            //    }
            //}

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new Window_Main());
        }
    }
    public static class path
    {

        private static readonly Uri AppUri = new Uri(Path.GetDirectoryName(Path.GetFullPath(Assembly.GetExecutingAssembly().GetName().CodeBase.Replace(@"file:///", ""))));

        public static string AppDirOverride = null;

        public static string AppDir { get { return AppDirOverride ?? AppUri.LocalPath; } }
    }
}
