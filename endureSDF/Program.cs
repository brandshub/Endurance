using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using endureSDF;

namespace linqtsql
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Preloader preloader = new Preloader();
            Application.Run(preloader);
           /* if (preloader.DialogResult == DialogResult.OK)
                Application.Run(new Form1(preloader.selectedUser));*/
        }
    }
}
