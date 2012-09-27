using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using Protocol.Data;

namespace Protocol
{

    /// <summary>
    /// Remote - Control-Protocol [RCP]
    /// <para>This protocol emulates a remote control of n-channels.</para>
    /// <para>This is a write only protocol</para>
    /// </summary>
    public class RCP : TCPClient, IOIOProtocol
    {


        #region "Attributes"

        private IOIOProtocol mIOIOProtocol;

        #endregion



        #region "Properties"

        #endregion


        #region "Constructor"

        protected  RCP()
        {
        }

        public RCP(IPAddress IPAddress, int Port, delegate_OnDataReceived OnDataReceived)
        {
            mIPAddress = IPAddress;
            mPort = Port;
            mOnDataReceived = OnDataReceived;
            Connect();
        }

        public RCP(string IPAddress, int Port, delegate_OnDataReceived OnDataReceived)
        {
            mIPAddress = System.Net.IPAddress.Parse(IPAddress);
            mPort = Port;
            mOnDataReceived = OnDataReceived;
            Connect();
        }
        #endregion


        #region "Methods"


       

        /// <summary>
        /// Emulate a stick/button position.
        /// </summary>
        /// <param name="ChannelNumber">use PMW IOIO ports.</param>
        /// <param name="value"></param>
        public void SetChannel(byte ChannelNumber, int value)
        {

        }


        protected override void ProcessDataReceived(byte[] Data)
        {
            base.ProcessDataReceived(Data); // allow calling delegate
        }


        #region "Misc Control"
        public void WriteOnLCD(string Text)
        {
            throw new NotImplementedException();
        }

        public void SetSimulatorON()
        {
             SendToClient("SET_SWT#SIM#ON\r\n");
        }

        public void SetSimulatorOFF()
        {
             SendToClient("SET_SWT#SIM#OFF\r\n");
        }

        public void Arming()
        {
            SendToClient("SET_ARM#ON\r\n");
        }

        public void DisArming()
        {
            SendToClient("SET_ARM#OFF\r\n");
        }

        public void Calibrate()
        {
            SendToClient("CMD_CLB\r\n");
        }


        #endregion 

        #region "Sensors Control"

        /// <summary>
        /// Set accelerometer seonsor On
        /// </summary>
        public void SetAccSensorON()
        {
            SendToClient("SET_SWT#ACC#ON\r\n");
        }

        /// <summary>
        /// Set accelerometer seonsor Off
        /// </summary>
        public void SetAccSensorOFF()
        {
            SendToClient("SET_SWT#ACC#OFF\r\n");
        }

        /// <summary>
        /// Set magnetic seonsor On
        /// </summary>
        public void SetMagneticSensorON()
        {
            SendToClient("SET_SWT#MAG#ON\r\n");
        }

        /// <summary>
        /// Set magnetic seonsor Off
        /// </summary>
        public void SetMagneticSensorOFF()
        {
            SendToClient("SET_SWT#MAG#OFF\r\n");
        }

        /// <summary>
        /// Set gyro seonsor On
        /// </summary>
        public void SetGyroSensorON()
        {
            SendToClient("SET_SWT#GYR#ON\r\n");
        }

        /// <summary>
        /// Set gyro seonsor Off
        /// </summary>
        public void SetGyroSensorOFF()
        {
            SendToClient("SET_SWT#GYR#OFF\r\n");
        }

        /// <summary>
        /// Set gyro seonsor On
        /// </summary>
        public void SetGPSSensorON()
        {
            SendToClient("SET_SWT#GPS#ON\r\n");
        }

        /// <summary>
        /// Set accelerometer seonsor Off
        /// </summary>
        public void SetGPSSensorOFF()
        {
            SendToClient("SET_SWT#GPS#OFF\r\n");
        }

        /// <summary>
        /// Set ORI seonsor On
        /// </summary>
        public void SetORISensorON()
        {
            SendToClient("SET_SWT#ORI#ON\r\n");
        }

        /// <summary>
        /// Set orientation seonsor Off
        /// </summary>
        public void SetORISensorOFF()
        {
            SendToClient("SET_SWT#ORI#OFF\r\n");
        }


        #endregion

        #region "Route Control"
        
        /// <summary>
        /// earase route in androiv.
        /// </summary>
        public void ClearRouteNodes()
        {
            SendToClient("SET_ROT#CLR\r\n");
        }

        /// <summary>
        /// Send a list of route points to androiv.
        /// </summary>
        /// <param name="lstRoutePoint"></param>
        public void SendRouteNodes(List<RoutePoint> lstRoutePoint)
        {
            /*
             * "SET_ROT#SAV#0$31.365966796875$30.1341416247186$0$0$9999$0%1$31.3978958129883$30.1118698492352$0$0$9999$0%2$31.3845062255859$30.0872165636535$0$0$9999$0\r\n"
             */
            StringBuilder oSB = new StringBuilder();
            
            oSB.Append("SET_ROT#SAV#");
            int count = 0;
            foreach (var oItem in lstRoutePoint)
            {
                oSB.Append(oItem.Id);
                oSB.Append("&");
                oSB.Append(oItem.Longitude);
                oSB.Append("&");
                oSB.Append(oItem.Latitide);
                oSB.Append("&");
                oSB.Append(oItem.Altitude);
                oSB.Append("&");
                oSB.Append(oItem.Speed);
                oSB.Append("&");
                oSB.Append(oItem.MaxSpeed);
                oSB.Append("&");
                oSB.Append(oItem.MinSpeed);
                ++count;
                if (count < lstRoutePoint.Count) oSB.Append("%");
            }
            oSB.Append("\r\n");

            this.SendToClient(oSB.ToString());          
        }
        #endregion
        #endregion

        #region "IOIO Interface"

        /// <summary>
        /// Set IOIO On
        /// </summary>
        public void SetIOIOON()
        {
            SendToClient("SET_SWT#IOIO#ON\r\n");
        }

        /// <summary>
        /// Set IOIO Off
        /// </summary>
        public void SetIOIOOFF()
        {
            SendToClient("SET_SWT#IOIO#OFF\r\n");
        }

        void IOIOProtocol.GeneratePWM(int PortNum, int Frequency, double DutyCycle)
        {
             StringBuilder oSB = new StringBuilder();

             oSB.Append("SET_IOIO#PWM#APPLY#");
             oSB.Append(PortNum);
             oSB.Append("#");
             oSB.Append(Frequency);
             oSB.Append("#");
             oSB.Append(DutyCycle);
             oSB.Append("\r\n");
             this.SendToClient(oSB.ToString());        
        }

        void IOIOProtocol.StorePWMCalibration(int PortNum, int Frequency, double MinDutyCycle, double MaxDutyCycle)
        {
            StringBuilder oSB = new StringBuilder();

            oSB.Append("SET_IOIO#PWM#STORE#");
            oSB.Append(PortNum);
            oSB.Append("#");
            oSB.Append(Frequency);
            oSB.Append("#");
            oSB.Append(MinDutyCycle);
            oSB.Append("#");
            oSB.Append(MaxDutyCycle);
            oSB.Append("\r\n");
            this.SendToClient(oSB.ToString()); 
        }
        #endregion
    }
}
