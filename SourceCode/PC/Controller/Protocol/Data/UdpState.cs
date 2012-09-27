using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Data
{


    /// <summary>
    /// Class uses as Object State for UDP Socket
    /// </summary>
    public class UdpState
    {


        #region "Properties"

        public System.Net.Sockets.UdpClient UdpClient
        {
            set;
            get;
        }


        public System.Net.IPEndPoint IPEndPoint
        {
            set;
            get;
        }

        #endregion 
    }
}
