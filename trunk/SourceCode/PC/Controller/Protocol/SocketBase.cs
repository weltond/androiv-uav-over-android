using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;


namespace Protocol
{
    public class SocketBase
    {

        /// <summary>
        /// A Listener Socket
        /// </summary>
        /// <param name="ClientIP"></param>
        /// <param name="ClientPortNumber"></param>
        public void ListenToClient (String ClientIP, UInt32 ClientPortNumber)
        {
            throw new System.NotImplementedException();
        }


        /// <summary>
        /// Send raw data through socket.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void SendData(byte[] Data)
        {
            throw new System.NotImplementedException();
        }



        /// <summary>
        /// Called when data is recieved.
        /// </summary>
        /// <param name="Data"></param>
        public virtual void  OnReceive (byte[] Data)
        {
            throw new System.NotImplementedException();
        }

    }
}
