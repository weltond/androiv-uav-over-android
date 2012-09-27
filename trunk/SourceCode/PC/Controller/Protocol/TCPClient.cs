using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;

namespace Protocol
{
    public class TCPClient
    {

        #region "Delegates"

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Data"></param>
        /// <returns>number of bytes read...if zero the socket closes.</returns>
        public delegate int delegate_OnDataReceived(byte[] Data);

        #endregion

        #region "Attributes"
        protected delegate_OnDataReceived mOnDataReceived;
        protected Thread mConnectionThread;
        protected TcpClient mTcpClient;
        protected IPAddress mIPAddress;
        protected int mPort;
        protected NetworkStream mClientStream;

        #endregion


        #region "Properties"

        public delegate_OnDataReceived OnDataReceived
        {
            set
            {
                mOnDataReceived = value;
            }
        }

        public bool IsConnected
        {
            get
            {
                return mTcpClient.Connected;
            }
        }

        public IPAddress Ip
        {
            get { return mIPAddress; }
            private set { mIPAddress = value; }
        }

        public int Port
        {
            get { return mPort; }
            private set { mPort = value; }
        }

        #endregion

        #region "Constructor"

        protected TCPClient()
        {
        }

        public TCPClient(IPAddress IPAddress, int Port, delegate_OnDataReceived OnDataReceived)
        {
            mIPAddress = IPAddress;
            mPort = Port;
            mOnDataReceived = OnDataReceived;
            Connect();
        }

        public TCPClient(string IPAddress, int Port, delegate_OnDataReceived OnDataReceived)
        {
            mIPAddress = System.Net.IPAddress.Parse(IPAddress);
            mPort = Port;
            mOnDataReceived = OnDataReceived;
            Connect();
        }


        protected virtual void Connect()
        {
           

                mTcpClient = new TcpClient();
                mTcpClient.Connect(mIPAddress, mPort);
                mConnectionThread = new Thread(HandleClientComm);
                mConnectionThread.Start();
           
        }

        #endregion

        #region "Methods"

        
        public virtual void SendToClient(String Data)
        {
            SendToClient(Encoding.ASCII.GetBytes(Data));
        }

        public virtual void SendToClient(byte[] Data)
        {
            if (mTcpClient.Connected == false) return;

            
            mClientStream.Write(Data, 0, Data.Length);

            return;
        }

        public virtual void HandleClientComm()
        {
            try
            {
                mClientStream = mTcpClient.GetStream();
                SendToClient("GET_SWT\r\n");

                int bytesRead;
                while (mTcpClient.Connected == true)
                {
                    bytesRead = 0;
                    byte[] buffer = new byte[4096];
                       
                    try
                    {
                        //blocks until a client sends a message
                         bytesRead = mClientStream.Read(buffer, 0, 4096);
                         ProcessDataReceived(buffer);
                        
                    }
                    catch (Exception e)
                    {

                        break;
                    }

                    if (bytesRead == 0)
                    {
                        //the client has disconnected from the server
                        
                        break;
                    }

                    //message has successfully been received

                }

                mTcpClient.Close();
            }
            catch
            {
                int a = 0;
            }
        }

        protected virtual void ProcessDataReceived(byte[] Data)
        {
            mOnDataReceived(Data);
        }

        #endregion
    }
}
