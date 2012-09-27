/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/93-iobservable/
 * ============================================================================ */

#region usings
using System;
using System.Collections.Generic;
using System.Text;
#endregion

namespace FgfsSharp
{
    public interface IObservable
    {
        void RegisterObserver(IObserver observer);
        void RemoveObserver(IObserver observer);
        void NotifyObservers();
    }
}
