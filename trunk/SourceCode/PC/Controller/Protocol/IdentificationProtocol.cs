using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;

using Protocol.Data;
using Protocol.Enumerations;

namespace Protocol
{

    /// <summary>
    /// This is Identification protocol used to tell Station about surrounded vehicles.
    /// </summary>
    public class IdentificationProtocol
    {

        #region "Delegate"

        /// <summary>
        /// Called when a new vehicle data received.
        /// </summary>
        /// <param name="oVehicleInfo"></param>
        public delegate void delegate_OnVehicleDetected(VehicleInfo oVehicleInfo);


        /// <summary>
        /// Called when an ID received from an existed Vehicle.
        /// </summary>
        /// <param name="oVehicleInfo"></param>
        public delegate void delegate_OnVehicleUpdated(VehicleInfo oVehicleInfo);
        #endregion 

        #region "Attributes"

        /// <summary>
        /// List of vehicles.
        /// </summary>
        protected Dictionary<String, VehicleInfo> mVisibleVehicles = new Dictionary<String, VehicleInfo>();
        protected delegate_OnVehicleDetected mOnVehicleDetected;
        protected delegate_OnVehicleUpdated mOnVehicleUpdated; 


        UdpClient mUdpClient = null;
        IPEndPoint mIPEndPoint;

        #endregion 



        #region "Properties"


        public delegate_OnVehicleDetected OnVehicleDetected
        {
              get
            {
                return mOnVehicleDetected;
            }

            set
            {
                mOnVehicleDetected = value;
            }
        }


        public delegate_OnVehicleUpdated OnVehicleUpdated
        {
            get
            {
                return mOnVehicleUpdated;
            }

            set
            {
                mOnVehicleUpdated = value;
            }
        }

        public string GUID
        { get; set; }

        public string Name
        { get; set; }

        public string Type
        { get; set; }

       

        #endregion

        #region "Constructors"

        public IdentificationProtocol()
        {
            mUdpClient = new  UdpClient();
            mIPEndPoint = new IPEndPoint(IPAddress.Parse("255.255.255.255"), (int) Enum_ProtocolPort.IdentificationProtocol);
            
            GUID = new Guid().ToString();
            Name = "NoName";
            Type = "unknown";

        }

        #endregion


        #region "Methods"

        /// <summary>
        /// Broadcast Identification on network.
        /// </summary>
        public void Boradcast()
        {
            byte[] data = Encoding.ASCII.GetBytes(GUID + '#' + Name + '#' + Type);
            mUdpClient.Send(data, data.Length, mIPEndPoint);
        }


        /// <summary>
        /// Listen to identification boradcast.
        /// </summary>
        public void Listen(String IP)
        {
            IPEndPoint ServerIPEndPoint = new IPEndPoint(IPAddress.Parse(IP), (int)Enum_ProtocolPort.IdentificationProtocol);
            UdpClient  Socket = new UdpClient(ServerIPEndPoint);

            UdpState IC = new UdpState();
            IC.IPEndPoint = ServerIPEndPoint;
            IC.UdpClient = Socket;

            Socket.BeginReceive(new AsyncCallback(ReceiveData), IC);
            

        }


        /// <summary>
        /// http://msdn.microsoft.com/en-us/library/system.net.sockets.udpclient.beginreceive.aspx
        /// </summary>
        /// <param name="iar"></param>
        private void ReceiveData(IAsyncResult iar)
        {
            UdpClient oUdpClient = (UdpClient)((UdpState)(iar.AsyncState)).UdpClient;
            IPEndPoint EndPoint = (IPEndPoint)((UdpState)(iar.AsyncState)).IPEndPoint;
            
            Byte[] receiveBytes = oUdpClient.EndReceive(iar, ref EndPoint);
            string receiveString = Encoding.ASCII.GetString(receiveBytes);
            string [] ParsedPacket = receiveString.Split('#');
            VehicleInfo oVehicleInfo = new VehicleInfo(ParsedPacket[0], ParsedPacket[1], ParsedPacket[2], EndPoint.Address);
              
            // Update dictinary if vehicle not listed.
            if (mVisibleVehicles.ContainsKey(oVehicleInfo.GUID) == false)
            {
                // New Vehicle

                mVisibleVehicles.Add(oVehicleInfo.GUID, oVehicleInfo);

                if (mOnVehicleDetected != null) mOnVehicleDetected(oVehicleInfo);
            }
            else
            {
                // Update Access Time
                mVisibleVehicles[oVehicleInfo.GUID].LastUpdate = System.DateTime.Now;

                if (mOnVehicleUpdated != null) mOnVehicleUpdated(oVehicleInfo);
            }

           
            // Listen Again
            oUdpClient.BeginReceive(new AsyncCallback(ReceiveData), ((UdpState)(iar.AsyncState)));
            
        }


        #endregion 
    }
}
