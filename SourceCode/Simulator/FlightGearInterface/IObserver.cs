/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/91-iobserver/
 * ============================================================================ */
#region usings
using System;
using System.Collections.Generic;
using System.Text;
using FgfsSharp;
#endregion

namespace FgfsSharp
{
    public interface IObserver
    {
        void UpdateObserver(FgfsDataObject dataObject);
        void DisplayData();
    }
}

