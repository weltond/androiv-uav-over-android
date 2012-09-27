/* ==================================================================
 * FgfsLog.cs
 * 
 * AIM: To write a LOG of the FG messages received
 * There is an option to add the TIME HH:MM:SS: to the message
 * Undefine the ADD_TIME_STRING below ... if you intend to try
 * to feed this file back to FG, then you may NOT want the time added.
 * You may also need to kill the BEGIN and END messages.
 * 
 * 2008/11/17 - geoff mclane - http://geoffair.net/fg/
 * ================================================================== */

// LOG FEATURES
#define ADD_TIME_STRING

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace FgfsSharp
{
    class FgfsLog
    {
        public bool bAddTime = true;
        private StreamWriter sw = null;
        private string logfile;

        #region Constructor
        public FgfsLog(string file)
        {
            logfile = file;
            Console.WriteLine("FgfsLog:Constructor: file {0} ...", logfile);
            sw = new StreamWriter(logfile);
            string msg = string.Format("# BEGIN Log {0}", DateTime.Now.ToLongTimeString());
            LogWrite(msg);
        }
        #endregion

        #region Public
        public bool AddTime
        {
            get { return bAddTime; }
            set { bAddTime = value; }
        }
        public void Close()
        {
            if (sw != null)
            {
                string msg = string.Format("# END Log {0}", DateTime.Now.ToLongTimeString());
                LogWrite(msg);
                sw.Close();
                Console.WriteLine("FgfsLog::Close: file {0} ...", logfile);
            }
            sw = null;
        }

        public void LogWrite(string text)
        {
            if (sw != null)
            {
#if ADD_TIME_STRING
                if (bAddTime)
                {
                    string msg = string.Format("{0}|{1}",
                        DateTime.Now.ToLongTimeString(),
                        text);
                    sw.WriteLine(msg);
                }
                else
                {
                    sw.WriteLine(text);
                }
#else
                sw.WriteLine(text);
         
#endif
                sw.Flush(); // ensure added to file NOW !!!
            }
        }
        #endregion
    }
}
