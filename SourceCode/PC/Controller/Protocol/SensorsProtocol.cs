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
    public class SensorsProtocol
    {

        #region "Delegates"

        public delegate void delegate_OnSensorDataRecieved4(string w, string x, string y, string z);
        public delegate void delegate_OnSensorDataRecieved(string x, string y, string z);
        public delegate void delegate_OnControlSurfaceDataRecieved(string x);
        #endregion 

        #region "Attributes"

        /// <summary>
        /// List of vehicles.
        /// </summary>
        protected Dictionary<String, VehicleInfo> mVisibleVehicles = new Dictionary<String, VehicleInfo>();
        public delegate_OnSensorDataRecieved mOnGyroDataRecieved;
        public delegate_OnSensorDataRecieved mOnAccelerometerDataRecieved;
        public delegate_OnSensorDataRecieved mOnMagneticDataRecieved;
        public delegate_OnSensorDataRecieved mOnOrientationDataRecieved;
        public delegate_OnSensorDataRecieved4 mOnGPSDataRecieved;
        public delegate_OnSensorDataRecieved mOnReplyDataRecieved;
        
        public delegate_OnControlSurfaceDataRecieved mOnThrottleReceived;
        public delegate_OnControlSurfaceDataRecieved mOnElevatorReceived;
        public delegate_OnControlSurfaceDataRecieved mOnRudderReceived;
        public delegate_OnControlSurfaceDataRecieved mOnAileronReceived;


        UdpClient mUdpClient = null;
        IPEndPoint mIPEndPoint;

        #endregion 


        #region "Methods"

        /// <summary>
        /// Listen to identification boradcast.
        /// </summary>
        public void Listen(String IP)
        {
            IPEndPoint ServerIPEndPoint = new IPEndPoint(IPAddress.Parse(IP), (int)Enum_ProtocolPort.SensorsProtocol);
            UdpClient Socket = new UdpClient(ServerIPEndPoint);

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
            // "ACC#0.009357303#-0.0495964#0.08727932%%GYR#0.0#0.0#0.0%"voisio
            string[] Commands = receiveString.Split('%');
            foreach (string Command in Commands)
            {
                string[] Tokens = Command.Split('#');
                switch (Tokens[0])
                {
                    case "THR":
                        mOnThrottleReceived(Tokens[1]);
                        break;
                    case "ELV":
                        mOnElevatorReceived(Tokens[1]);
                        break;
                    case "RUD":
                        mOnRudderReceived(Tokens[1]);
                        break;
                    case "AIL":
                        mOnAileronReceived(Tokens[1]);
                        break;
                    case "GYR":
                        mOnGyroDataRecieved(Tokens[1], Tokens[2], Tokens[3]);
                        break;
                    case "ACC":
                        mOnAccelerometerDataRecieved(Tokens[1], Tokens[2], Tokens[3]);
                        break;
                    case "MAG":
                        mOnMagneticDataRecieved(Tokens[1], Tokens[2], Tokens[3]);
                        break;
                    case "ORI":
                        mOnOrientationDataRecieved(Tokens[1], Tokens[2], Tokens[3]);
                        break;
                    case "GPS":
                        mOnGPSDataRecieved(Tokens[1], Tokens[2], Tokens[3], Tokens[4]);
                        break;
                    case "SWT": // reply confirmation
                        /*
                         * Switch Sensor Control
                         * "SWT#ACC#OFF"   confirmation that ACC is turned off.
                         * "SWT#GPS#ON"    confirmation that GPS is turned on.
                         * */
                        mOnReplyDataRecieved(Tokens[1], Tokens[2], Tokens[3]);
                        break;
                }
            }
            // Listen Again
            oUdpClient.BeginReceive(new AsyncCallback(ReceiveData), ((UdpState)(iar.AsyncState)));

        }


        #endregion


    }
}
