using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using System.Threading;

namespace FYPSERVER
{
    static class program
    {
        /* public static void client_thread()
         {
             Application.Run(new Register());
         }
         */
        public static void server_thread()
        {
            Application.Run(new Registration_Server());
        }
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            //   Thread tid1 = new Thread(new ThreadStart(Program.client_thread));
            Thread tid2 = new Thread(new ThreadStart(program.server_thread));
            //   tid1.Start();
            tid2.Start();
        }

    }
}

