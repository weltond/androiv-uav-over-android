
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;


namespace FgfsSharp
{

    

    public class FgfsClient
    {

        #region "Attributes"
        
        /// <summary>
        /// Recieves Data from Simulator
        /// </summary>
        protected TcpClient mTCPClientOutput;

        /// <summary>
        /// Sends Data to Simulator
        /// </summary>
        protected TcpClient mTCPClientInput;
        protected FgfsDataObject _dataObject;
        protected FgfsDataHelper _dataHelper;

        #endregion 


        #region "Properties"

        private FgfsDataHelper Helper
        {
            get { return _dataHelper; }
            set { _dataHelper = value; }
        }

        private FgfsDataObject DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }

        public FgfsDataHelper DataHelper
        {
            get { return _dataHelper; }
        }

        #endregion

        #region "Constructor"


        protected FgfsClient(string IPAddress,int OutputPort)
        {
            mTCPClientInput = new TcpClient ();
            mTCPClientInput.Connect(IPAddress, OutputPort);
        }

        public FgfsClient(TcpClient client,int OutputPort):this("127.0.0.1",OutputPort)
        {

            _dataObject = new FgfsDataObject();
            _dataHelper = new FgfsDataHelper(_dataObject);

            mTCPClientOutput = client;
        }


        public FgfsClient(FgfsDataHelper Helper, TcpClient client, int OutputPort): this("127.0.0.1", OutputPort)
        {
            _dataObject = Helper.DataObject;
            _dataHelper = Helper;

            mTCPClientOutput = client;
        }


        #endregion

        #region "Public"


        public void SendToClient(byte[] Data)
        {
            if (mTCPClientInput.Connected == false) return;

            NetworkStream clientStream = mTCPClientInput.GetStream();
            clientStream.Write(Data, 0, Data.Length);

            return;
        }

        public void HandleClientComm()
        {
            NetworkStream clientStream = mTCPClientOutput.GetStream();
            StreamReader incomingStream = new StreamReader(clientStream);
            byte[] message = new byte[4096];
            int bytesRead;

            while (mTCPClientOutput.Connected == true)
            {
                bytesRead = 0;

                try
                {
                    //blocks until a client sends a message
                    bytesRead = Helper.UpdateDataObject(incomingStream);
                }
                catch (Exception e)
                {
                    //a socket error has occured
                    break;
                }

                if (bytesRead == 0)
                {
                    //the client has disconnected from the server
                    break;
                }

                //message has successfully been received

            }

            mTCPClientOutput.Close();
        }

        #endregion

    }
}
