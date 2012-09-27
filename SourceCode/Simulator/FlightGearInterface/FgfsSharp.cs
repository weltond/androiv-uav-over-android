/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/90-fgfsmain/
 * Main entry to windows application
 * ============================================================================ */

#region Usings
using System;
using System.Collections.Generic;
using System.Text;
using FgfsSharp;
using System.Windows.Forms;
using System.Collections;
using System.Collections.Specialized;
#endregion

namespace FgfsSharp
{
    static class FgfsSharp
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new FgfsMainForm());
        }
    }
}
