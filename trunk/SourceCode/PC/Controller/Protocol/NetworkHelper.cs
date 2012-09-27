using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Net;

namespace Protocol
{
    public class NetworkHelper
    {

        public static ArrayList DeterminePossibleIPs()
        {
            ArrayList aL = new ArrayList();
            IPHostEntry hostEntry = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress address in hostEntry.AddressList)
            {
                aL.Add(address.ToString());
            }
            aL.Add(IPAddress.Loopback.ToString());
            return aL;
        }

    }
}
