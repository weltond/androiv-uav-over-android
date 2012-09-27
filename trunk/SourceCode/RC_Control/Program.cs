using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Threading;

namespace RC_Control
{
    static class RC_ControlMain
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            bool flag;
            new Mutex(false, @"Local\AudioPPMInstance", out flag);
            if (!flag)
                return;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            
            Application.Run(new RemoteControl());
        }
    }
}
