/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/21-fgfsserver/
 * 2008/11/17 - geoff mclane - http://geoffair.net/fg/
 * Added a 'status' so when the thread exits, timer to reset the 'server' to OFF
 * ready for the next time ...
 * ============================================================================ */

#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FgfsSharp;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Threading;
#endregion

namespace FgfsSharp
{
    // The FG message server should run in it’s own thread and
    // fetch continously Flightgear-data, as it arrives ...
    public class FgfsServer
    {
        #region Public
        public int running = 0;
        #endregion

        #region Privates

        private IPAddress _ip;
        private int _port;
        private FgfsDataHelper _helper;
        private ArrayList _ipList;
        private static FgfsServer _uniqueServer = null;
        private Thread _serverThread = null;

        private TcpListener tcpListener = null;
        private Socket fgfsSocket = null;
        private NetworkStream netStream = null;
        private StreamReader incomingStream = null;
        private List<FgfsClient> mlstTCPClients = new List<FgfsClient>();
        protected FgfsDataHelper _dataHelper;
      
        #endregion

        #region Properties

        public IPAddress Ip
        {
            get { return _ip; }
            private set { _ip = value; }
        }

        public int Port
        {
            get { return _port; }
            private set { _port = value; }
        }
        public int Status
        {
            get { return running; }
        }

        
        public ArrayList IpList
        {
            get { return _ipList; }
            private set { _ipList = value; ;}
        }

        private FgfsDataHelper Helper
        {
            get { return _dataHelper; }
            set { _dataHelper = value; }
        }


        public List<FgfsClient> TCPClients
        {
            get
            {
                return mlstTCPClients;
            }
        }

        #endregion

        #region Constructor

        private FgfsServer()
        {
            Console.WriteLine("Constructor: FgfsServer");
            IpList = DeterminePossibleIPs();
        }

        #endregion

        #region Public Methods

        public static FgfsServer GetInstance()
        {
            if (_uniqueServer == null)
            {
               _uniqueServer = new FgfsServer();
            }

            return _uniqueServer;
        }

        private void Close_Streams()
        {
            if (incomingStream != null)
                incomingStream.Close();
            if (netStream != null)
                netStream.Close();
            if (fgfsSocket != null)
                fgfsSocket.Close();
            if (tcpListener != null)
                tcpListener.Stop();

            tcpListener = null;
            fgfsSocket = null;
            netStream = null;
            incomingStream = null;

        }

        public void SuspendServer()
        {
            if (_serverThread == null)
            {
                Console.WriteLine("SuspendServer: IS NULL!");
            }
            else
            {
                // _serverThread.Suspend();
                _serverThread.Abort();
                Console.WriteLine("SuspendServer: Thread is aborted...");
                _serverThread = null;
                Close_Streams();
                running = 0;
            }
        }

        //transforms string in ipaddress –
        public void StartServer(string ip, int port, FgfsDataHelper helper)
        {
            IPAddress ipAddress = IPAddress.Parse(ip);
            Console.WriteLine("StartServer: On {0}, at {1} ...", ip, port.ToString());
            this.StartServer(ipAddress, port, helper);
        }

        //starts the server @ ip:port
        public void StartServer(IPAddress ip, int port, FgfsDataHelper helper)
        {
            this.Ip = ip;
            this.Port = port;
            this.Helper = helper;


            mlstTCPClients.Clear();
            
            //start a new thread on the run-method
            _serverThread = new Thread(new ThreadStart(ListenForClients));
            _serverThread.Name = "ServerThread";
            _serverThread.Priority = ThreadPriority.Normal;
            _serverThread.Start();

            Console.WriteLine("Started Server…");
            running = 1;
        }



        #endregion

        #region Private Methods

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

        protected void Run()
        {
            Console.WriteLine("FgfsServer:Run(): Create socket on {0}, port {1} ...",
                this.Ip.ToString(), this.Port.ToString() );
            //create a socket, waiting for Flightgear to connect
            tcpListener = new TcpListener(this.Ip, this.Port);
            Console.WriteLine("FgfsServer:Run(): Starting listener ...");
            tcpListener.Start();

            Console.WriteLine("FgfsServer:Run(): Waiting for connection ...");

            fgfsSocket = tcpListener.AcceptSocket();
            Console.WriteLine("FgfsServer:Run(): Client socket connected");
            netStream = new NetworkStream(fgfsSocket);
            incomingStream = new StreamReader(netStream);
            try
            {
                do
                {
                    Helper.UpdateDataObject(incomingStream);
                    //Helper.PrintObject();
                }
                while (fgfsSocket.Connected);
                Console.WriteLine("FgfsServer:Run(): Exit while = fgfsSocket NOT CONNECTED!");
            }
            catch (Exception e)
            {
                string err = e.Message;
                Console.WriteLine("Note:Exception: [" + err + "]");
            }
            finally
            {
                Console.WriteLine("FgfsServer:Run():finally: Close and null variables!");
                Close_Streams();
            }
            running = 2;    // aborted thread, due to close at other end probably
        }

        protected void ListenForClients()
        {
            Console.WriteLine("FgfsServer:Run(): Create socket on {0}, port {1} ...",
                this.Ip.ToString(), this.Port.ToString());
            //create a socket, waiting for Flightgear to connect
            tcpListener = new TcpListener(this.Ip, this.Port);
            Console.WriteLine("FgfsServer:Run(): Starting listener ...");
            tcpListener.Start();

            Console.WriteLine("FgfsServer:Run(): Waiting for connection ...");
            
            while (true)
            {
                //blocks until a client has connected to the server
                TcpClient client = this.tcpListener.AcceptTcpClient();
                FgfsClient fgfsClient = new FgfsClient(Helper, client, this.Port+1);
                //create a thread to handle communication 
                //with connected client
                Thread clientThread = new Thread(new ParameterizedThreadStart(HandleClientComm));

                mlstTCPClients.Add(fgfsClient);
                clientThread.Start(fgfsClient);
            }
        }

        private void HandleClientComm(object client)
        {
            ((FgfsClient)client).HandleClientComm();
            
        }

        #endregion



    }
}

