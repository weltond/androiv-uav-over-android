/* ===========================================================
 * FgfsSharp project - 2008/11/17
 * Built with -
 * Microsoft Visual Studio 2005 - Version 8.0.50727.762  (SP.050727-7600)
 * Microsoft .NET Framework - Version 2.0.50727 SP1
 * Installed Edition: C# Express - Microsoft Visual C# 2005
 * 
 * Original from: http://linkslink.wordpress.com/a-gentle-start/
 * Many thanks for that start ...
 * 
 * The original used SpeechLib (http://code.google.com/p/speechlib/source/checkout)
 * BUT, this ALSO required Boost (http://www.boost.org/), which is a massive checkout
 * 
 * SO, I opted for Microsoft Speech API (SAPI) instead, but this MAY require
 * a download of the SDK 5.1 (http://msdn.microsoft.com/en-us/library/ms723627.aspx)
 * It seems quite OLD, circa 2001-2005, but seems to work fine. This was in XP.
 * 
 * NOTE: When compiled in Vista, using MSVC9 (2008), it appears this Speech library
 * is aleady available, but the 'compile' showed some 57 warnings, mainly about
 * 'cannot be marshalled ... may require unsafe code to manipulate', but NO errors
 * and it seemed to run FINE ;=()
 * 
 * If you do NOT want to include this, then you can undefine the USING_MS_SAPI
 * and maybe remove the SpeechLib in Solution Explorer -> References
 * 
 * TO RUN this application WITH speech, you must also have the
 * Interop.SpeechLib.dll, or the DLL registered ... and it is kind of cute
 * to have the ATC message SPOKEN ;=)) as it should be ...
 * 
 * Also, while the enumeration of the machine IPs often yields the current
 * IP perhaps assigned by your router, I used the localhost IP 127.0.0.1 mostly.
 * 
 * Open Source
 * 2008/11/17 - geoff mclane - http://geoffair.net/fg/
 * =========================================================== */

//#define USING_MS_SAPI

#region Usings
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Collections;
using System.IO;
using System.IO.Ports;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using GMap.NET.WindowsForms.ToolTips;
using BlackArrow.GMap.Map_Controls;

#if USING_MS_SAPI
using SpeechLib;    // Requires MS Speech SDK - and COM ref. added
#endif


#endregion

namespace FgfsSharp
{
    public partial class FgfsMainForm : Form, IObserver
    {
        public delegate void UpdateObserverDelegate(FgfsDataObject dataObject);

        #region Privates

        private FgfsServer _server;
        private FgfsDataObject _dataObject;
        private FgfsDataHelper _dataHelper;
        private ArrayList _registredDisplays;
        private SerialPort _sPort;
        private FgfsLog _swLog = null;

        private byte[] _byteArray = new byte[3];
        private int _pitchLevel;
        private int _leftRoll;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem exitToolStripMenuItem;
        private GroupBox groupBox1;
        private CheckBox _soundCheckBox;
        private CheckBox _rs232CheckBox;
        private GroupBox groupBox2;
        private ComboBox IpComboBox;
        private TextBox PortBox;
        private Label label1;
        private Button _startBtn;
        private GroupBox groupBox3;
        private TrackBar PitchBar;
        private TrackBar RollBar;
        private Label label3;
        private Label label2;
        private TextBox AltitudeBox;
        private TextBox SpeedBox;
        private Label ChoosePortLabel;
        private TextBox LogTextBox;
        private CheckBox LogFileBox;
        private TextBox fgfsMessageBox;
        private Timer timer1;
        private IContainer components;
        private int _rightRoll;
        private TextBox TimerBox;
        private Label label4;
        private Button button1;
        private Label label5;
        private TextBox MsgCntBox;
        private long _timerticks = 0;
        private long _messagecount = 0;
        private int _max_pitch, _min_pitch;
        private int _max_roll, _min_roll;
        private int pitch_scale, roll_scale;
        private int pitch_scale2, roll_scale2;
        private int pitch_max, pitch_min, roll_max, roll_min;
        private TextBox TimeBox;



        #region "GMap"
        const double const_StartLatitude=30.1065610015682;
        const double const_StartLongitude = 31.3763952255249;
        AutoPilot mAutoPilot ;
        GMapMarker currentMarker;
        // layers
        internal GMapOverlay top;
        internal  GMapOverlay mGMapObjects ;
        internal GMapOverlay mGMapRoutes;
        internal  GMapOverlay mGMapPolygons ;

        #endregion
        // Speech SDK
#if USING_MS_SAPI
        private SpeechVoiceSpeakFlags SpFlags;
        private SpVoice ms_Voice;
        private string _last_spoken;
        //private SpeechLib.SpVoice _speechObj;
#endif
        private TextBox RollValBox;
        private Label label7;
        private Label label6;
        private CheckBox chkAutoPilot;
        private CheckBox chkInverserElevator;
        private CheckBox chkInverseAileron;
        private TextBox txtAileron;
        private Label label8;
        private TextBox txtRoll;
        private Label label9;
        private Button btnReset;
        private TrackBar trkAlironControl;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private ToolStripMenuItem miStartServer;
        private ToolStripSeparator toolStripSeparator1;
        private ListView lstLog;
        private TabPage tabPage3;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private TrackBar trkMapZoom;
        private GMap.NET.WindowsForms.GMapControl MainMap;
        private Label label10;
        private TextBox txtHeading;
        private Label label11;
        private TextBox txtTargetHeading;
        private TextBox txtAileronControlValue;
        private GroupBox groupBox4;
        private Label label12;
        private TextBox textDistance;
        private Label label14;
        private TextBox txtAltitude;
        private Label label13;
        private TextBox txtPitch;
        private Button btnDebug;
        private TextBox PitchValBox;
        #endregion

        #region Properties

        private FgfsServer Server
        {
            get { return _server; }
            set { _server = value; }
        }

        private FgfsDataObject DataObject
        {
            get { return _dataObject; }
            set { _dataObject = value; }
        }

        private FgfsDataHelper DataHelper
        {
            get { return _dataHelper; }
        }

        #endregion

        #region Constructor

        public FgfsMainForm()
        {
            Console.WriteLine("Constructor: FgfsMainForm");

            InitializeComponent();
            InitializePrivates();

            //register the window as observer
            DataObject.RegisterObserver(this);
        }

        #endregion

        #region IObserver - Methods
        public void UpdateObserver(FgfsDataObject dataObject)
        {
            if (this.InvokeRequired == false)
            {
                DataObject = dataObject;
                DisplayData();
            }
            else
            {
                UpdateObserverDelegate updateDelegate = new UpdateObserverDelegate(UpdateObserver);
                this.BeginInvoke(updateDelegate, new object[] { dataObject });
            }
        }

        public void DisplayData()
        {
            int npitch, nroll;
            try
            {
                if (chkAutoPilot.Checked == true)
                {
                    AutoPilot();
                }
                _messagecount++;
                MsgCntBox.Text = _messagecount.ToString();

                //update the visual displays
                SpeedBox.Text = DataObject.Speed.ToString();
                AltitudeBox.Text = DataObject.Altitude.ToString();

                int pitch = DataObject.Pitch;
                int roll = DataObject.Roll;
                // get MAX/MIN values
                // Pitch has MAX  90, MIN  -90
                // Roll  has MAX 180, MIN -180
                if (pitch > _max_pitch)
                    _max_pitch = pitch;
                if (pitch < _min_pitch)
                    _min_pitch = pitch;
                if (roll > _max_roll)
                    _max_roll = roll;
                if (roll < _min_roll)
                    _min_roll = roll;
                // The sliders are set to a scale of 0 to 10 
                // Modify pitch and roll to values 0 to 10
                // (maybe should be larger, and would love a log scale)
                npitch = pitch_min + pitch_scale2 +
                    (int)(((double)pitch / 90.0) * (double)pitch_scale2);
                nroll = roll_min + roll_scale2 +
                    (int)(((double)roll / 180.0) * (double)roll_scale2);

                // show actual value received
                this.RollValBox.Text = roll.ToString();
                this.PitchValBox.Text = pitch.ToString();
                // set sliders per modified values
                SetBar(nroll, ref RollBar);
                SetBar(npitch, ref PitchBar);

                if (this._sPort != null)
                {
                    this.setModelOrientation(RollBar.Value, PitchBar.Value, ref this._byteArray);
                    this._sPort.Write(this._byteArray, 0, 3);
                }
                if (this._swLog != null)
                {
                    this._swLog.LogWrite(DataObject.Message);
                }

                fgfsMessageBox.Text = DataObject.ATC;

#if USING_MS_SAPI
                if (Wants_Speech() && (_last_spoken != DataObject.ATC))
                {
                    _last_spoken = DataObject.ATC;
                    this.ms_Voice.Speak(_last_spoken, SpFlags);
                }
#endif
            }
            catch (Exception portException)
            {
                Console.WriteLine("DisplayData:Exception: " + portException.Message);
            }
        }

        #endregion

        #region Private Methods

        private void InitializePrivates()
        {
            Console.WriteLine("Init the privates ...");
            //init the privates
            _dataObject = new FgfsDataObject();
            _dataHelper = new FgfsDataHelper(_dataObject);
            _registredDisplays = new ArrayList();
            //_display = new FgfsDisplay();

#if USING_MS_SAPI
            // MS Speech SDK
            //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
            ms_Voice = null;
            //this._speechObj = null;
#else
            this._soundCheckBox.Enabled = false;
#endif

            this._sPort = null;
            this._swLog = null;

            //prepare the gui
            IpComboBox.DataSource = FgfsServer.DeterminePossibleIPs();

            // initialize pitch/roll max/min
            _max_pitch = -1;
            _min_pitch = 999999;
            _max_roll = -1;
            _min_roll = 999999;

            pitch_max = this.PitchBar.Maximum;
            pitch_min = this.PitchBar.Minimum;
            roll_max = this.RollBar.Maximum;
            roll_min = this.RollBar.Minimum;
            pitch_scale = pitch_max - pitch_min;
            roll_scale = roll_max - roll_min;
            pitch_scale2 = pitch_scale / 2;
            roll_scale2 = roll_scale / 2;
            Console.WriteLine("pitch {0} {1} {2} {3} roll {4} {5} {6} {7}",
                pitch_min, pitch_max, pitch_scale, pitch_scale2,
                roll_min, roll_max, roll_scale, roll_scale2);

        }

        #endregion

#if USING_MS_SAPI
        private bool Wants_Speech()
        {
            if ((this._soundCheckBox.Checked == true) && (ms_Voice != null))
            {
                return true;
            }
            return false;
        }
#endif

        private bool Is_Server_Running()
        {
            if (this._startBtn.Text == "Stop Server")
            {
                return true;
            }
            return false;
        }

        private void Stop_Server()
        {
            string msg = "Stopping server.";
#if USING_MS_SAPI
            if (Wants_Speech())
                ms_Voice.Speak(msg, SpFlags);
#endif
            Console.WriteLine(msg);
            Server = FgfsServer.GetInstance();
            Server.SuspendServer();
            this._startBtn.Text = "Start Server";
            ChoosePortLabel.Visible = true;
            _timerticks = 0;    // restart the seconds counter
            _messagecount = 0;
        }

        private void Start_Server()
        {
            ChoosePortLabel.Visible = false;
            string ip = IpComboBox.SelectedItem.ToString();
            int port = Convert.ToInt32(PortBox.Text);

            Server = FgfsServer.GetInstance();
            Server.StartServer(ip, port, DataHelper);

#if USING_MS_SAPI
            if (Wants_Speech())
            {
                string speechOut = string.Format("Server I.P. is {0}, on Port {1}", ip, port);
                this.ms_Voice.Speak(speechOut, SpFlags);
                //this._speechObj.Speak(speechOut, SpeechLib.SpeechVoiceSpeakFlags.SVSFDefault);
                //this._speechObj.WaitUntilDone(5000000);
            }
#endif
            //this._startBtn.Enabled = false;
            this._startBtn.Text = "Stop Server";
            _timerticks = 0;    // restart the seconds counter
            _messagecount = 0;
        }

        private void AutoPilot()
        {
            mAutoPilot.ExecuteStep(DataObject);

            double Aileron, Elevator, Rudder;

            if (chkInverseAileron.Checked == true)
            {
                Aileron = mAutoPilot.Aileron * -1;
            }
            else
            {
                Aileron = mAutoPilot.Aileron;
            }

            if (chkInverserElevator.Checked == true)
            {
                Elevator = mAutoPilot.Elevator * -1;
            }
            else
            {
                Elevator = mAutoPilot.Elevator;
            }

            Rudder = mAutoPilot.Rudder;

            txtAileron.Text = Aileron.ToString();
            txtAltitude.Text = DataObject.Altitude.ToString();
            txtPitch.Text = DataObject.Pitch.ToString();
            txtRoll.Text = DataObject.Roll.ToString();
            txtHeading.Text = (mAutoPilot.CurrentHeading).ToString();
            txtTargetHeading.Text = (mAutoPilot.TargetHeading).ToString();
            if (mAutoPilot.GMapMarkerMileStones.Count > 0)
            {
                textDistance.Text = mAutoPilot.GMapMarkerMileStones[mAutoPilot.CurrentMileStoneIndex].DistanceFromPlane.ToString();
            }

            string strData = Aileron.ToString() + "|" + Elevator.ToString() + "|" + Rudder.ToString() + "|" + mAutoPilot.Throttle.ToString() + "|\r\n";

            System.Text.UTF8Encoding enc = new System.Text.UTF8Encoding();
            SetPlanePosition(DataObject.Latitude, DataObject.Longitude);
            Server.TCPClients[0].SendToClient(enc.GetBytes(strData));

        }

        #region UI - HandlerMethods


        private void miStartServer_Click(object sender, EventArgs e)
        {
            StartServer();
        }


        private void _startBtn_Click(object sender, EventArgs e)
        {
            StartServer();
        }

        #endregion

        private void StartServer()
        {
            if (Is_Server_Running())
            {
                Stop_Server();
            }
            else
            {
                // start server
                if (PortBox.Text != string.Empty)
                {
                    Start_Server();
                }
                else
                {
                    ChoosePortLabel.Visible = true;
                }
            }
        }

        private void Application_Exit()
        {
            string tmsg = "Bye! for now...";
            if (Is_Server_Running())
            {
                Stop_Server();
            }
            if (Is_Log_Running())
            {
                if (_swLog != null)
                {
                    Close_Log_File();
                }
            }

            Console.Write(tmsg);
#if USING_MS_SAPI
            if (Wants_Speech())
            {
                this.ms_Voice.Speak(tmsg, SpFlags);
                this.ms_Voice.WaitUntilDone(5000);
            }
#endif
            // this.Dispose(); // maybe NOT required
            Application.Exit();
        }

        #region Menu
        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application_Exit();
        }

        /* ==========================
        private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FgfsOptionView OptionView = new FgfsOptionView();
            OptionView.ShowDialog();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Hide();
            this.TrayIcon.Visible = true;
        }

        private void TrayIcon_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.TrayIcon.Visible = false;
        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutDialog aboutDialog = new AboutDialog();
            aboutDialog.Show();
        }
        =================== */

        #endregion

        private void _soundCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this._soundCheckBox.Checked == true)
            {
#if USING_MS_SAPI
                string msg = "Voice setup done...";
                try
                {
                    this.ms_Voice = new SpVoice();
                    this.ms_Voice.Speak(msg, SpFlags);
                    //this._speechObj = new SpeechLib.SpVoice();
                    //SpeechVoiceSpeakFlags SpFlags = SpeechVoiceSpeakFlags.SVSFlagsAsync;
                }
                catch (Exception SpeechException)
                {
                    string failureMessage = string.Format("Something with the Speech-Output ran out of control: {0}", SpeechException.Message);
                    this._soundCheckBox.Checked = false;
                    MessageBox.Show(failureMessage);
                }
#endif
            }
            else
            {
                // leave this until EXIT
                //this._speechObj = null;
            }

        }

        private void _rs232CheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (this._rs232CheckBox.Checked == true)
            {
                try
                {
                    this._sPort = new SerialPort();
                    this._sPort.BaudRate = 9600;
                    this._sPort.Open();
                }
                catch (Exception portException)
                {
                    string failureMessage = string.Format("Your COM-Port FAILED: {0}", portException.Message);
                    this._rs232CheckBox.Checked = false;
                    MessageBox.Show(failureMessage);
                }
            }
            else
            {
                if (this._sPort != null)
                {
                    this._sPort.Close();
                    this._sPort.Dispose();
                    this._sPort = null;
                }
            }
        }

        private bool Is_Log_Running()
        {
            if (this.LogFileBox.Checked == true)
            {
                return true;
            }
            return false;
        }

        private void Close_Log_File()
        {
            string maxmin = string.Format("END: Pitch max={0} min={1}, Roll max={2} min={3}",
                _max_pitch.ToString(), _min_pitch.ToString(),
                _max_roll.ToString(), _min_roll.ToString());
            _swLog.LogWrite(maxmin);
            _swLog.Close();
            _swLog = null;
        }

        private void LogFileBox_CheckedChanged(object sender, EventArgs e)
        {
            if (Is_Log_Running())
            {
                // create output stream writer, if OUTPUT file given
                string file = LogTextBox.Text;
                if ((file.Length > 0) && (file != "<add file name>"))
                {
                    _swLog = new FgfsLog(file);
                    _swLog.AddTime = true;  // add TIME to messages
                }
                else
                {
                    LogTextBox.Text = "<add file name>";
                }
            }
            else
            {
                // close output stream writer
                if (_swLog != null)
                {
                    Close_Log_File();
                }
            }
        }

        public void WriteLog(string incoming)
        {
            if (_swLog != null)
            {
                _swLog.LogWrite(incoming);
            }
        }

        private void SetBar(int value, ref System.Windows.Forms.TrackBar bar)
        {
            if (value >= 0)
            {
                if (value > bar.Minimum)
                {
                    if (value < bar.Maximum)
                    {
                        bar.Value = value;
                    }
                    else
                    {
                        bar.Value = bar.Maximum;
                    }
                }
                else
                {
                    bar.Value = bar.Minimum;
                }
            }
        }

        private void setModelOrientation(int roll, int pitch, ref byte[] byteArray)
        {

            //right turn, so rigth side must be pulled down

            if (roll > 0)
            {
                _rightRoll = (int)160 - roll / 2;
                _leftRoll = (int)225 + roll / 2;

            }
            else if (roll < 0)
            {
                _pitchLevel = (int)95 + pitch / 2;
                _leftRoll -= (int)pitch / 2;
                _rightRoll -= (int)pitch / 2;
            }   //and if the value equals null, set the motor to mid-position
            else
            {
                _pitchLevel = 95;
            }

            byteArray[0] = byte.Parse(_leftRoll.ToString());
            byteArray[1] = byte.Parse(_rightRoll.ToString());
            byteArray[2] = byte.Parse(_pitchLevel.ToString());
        }

        #region Initialize
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FgfsMainForm));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.miStartServer = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.exitToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.LogTextBox = new System.Windows.Forms.TextBox();
            this.chkInverseAileron = new System.Windows.Forms.CheckBox();
            this.LogFileBox = new System.Windows.Forms.CheckBox();
            this.chkInverserElevator = new System.Windows.Forms.CheckBox();
            this._rs232CheckBox = new System.Windows.Forms.CheckBox();
            this.chkAutoPilot = new System.Windows.Forms.CheckBox();
            this._soundCheckBox = new System.Windows.Forms.CheckBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.ChoosePortLabel = new System.Windows.Forms.Label();
            this._startBtn = new System.Windows.Forms.Button();
            this.PortBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.IpComboBox = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.txtAileron = new System.Windows.Forms.TextBox();
            this.PitchValBox = new System.Windows.Forms.TextBox();
            this.RollValBox = new System.Windows.Forms.TextBox();
            this.TimeBox = new System.Windows.Forms.TextBox();
            this.fgfsMessageBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.AltitudeBox = new System.Windows.Forms.TextBox();
            this.SpeedBox = new System.Windows.Forms.TextBox();
            this.PitchBar = new System.Windows.Forms.TrackBar();
            this.RollBar = new System.Windows.Forms.TrackBar();
            this.btnReset = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.txtRoll = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.txtHeading = new System.Windows.Forms.TextBox();
            this.trkAlironControl = new System.Windows.Forms.TrackBar();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.TimerBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.MsgCntBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.btnDebug = new System.Windows.Forms.Button();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.lstLog = new System.Windows.Forms.ListView();
            this.columnHeader1 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.columnHeader2 = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label14 = new System.Windows.Forms.Label();
            this.txtAltitude = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtPitch = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.textDistance = new System.Windows.Forms.TextBox();
            this.txtAileronControlValue = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtTargetHeading = new System.Windows.Forms.TextBox();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.MainMap = new GMap.NET.WindowsForms.GMapControl();
            this.trkMapZoom = new System.Windows.Forms.TrackBar();
            this.menuStrip1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PitchBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.RollBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkAlironControl)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkMapZoom)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(747, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.miStartServer,
            this.toolStripSeparator1,
            this.exitToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // miStartServer
            // 
            this.miStartServer.Name = "miStartServer";
            this.miStartServer.Size = new System.Drawing.Size(133, 22);
            this.miStartServer.Text = "Start Server";
            this.miStartServer.Click += new System.EventHandler(this.miStartServer_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(130, 6);
            // 
            // exitToolStripMenuItem
            // 
            this.exitToolStripMenuItem.Name = "exitToolStripMenuItem";
            this.exitToolStripMenuItem.Size = new System.Drawing.Size(133, 22);
            this.exitToolStripMenuItem.Text = "Exit";
            this.exitToolStripMenuItem.Click += new System.EventHandler(this.exitToolStripMenuItem_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.LogTextBox);
            this.groupBox1.Controls.Add(this.chkInverseAileron);
            this.groupBox1.Controls.Add(this.LogFileBox);
            this.groupBox1.Controls.Add(this.chkInverserElevator);
            this.groupBox1.Controls.Add(this._rs232CheckBox);
            this.groupBox1.Controls.Add(this.chkAutoPilot);
            this.groupBox1.Controls.Add(this._soundCheckBox);
            this.groupBox1.Location = new System.Drawing.Point(15, 20);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(297, 135);
            this.groupBox1.TabIndex = 1;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Options";
            // 
            // LogTextBox
            // 
            this.LogTextBox.Location = new System.Drawing.Point(83, 16);
            this.LogTextBox.Name = "LogTextBox";
            this.LogTextBox.Size = new System.Drawing.Size(208, 20);
            this.LogTextBox.TabIndex = 3;
            this.LogTextBox.Text = "tempFgfsLog.txt";
            // 
            // chkInverseAileron
            // 
            this.chkInverseAileron.AutoSize = true;
            this.chkInverseAileron.Location = new System.Drawing.Point(88, 111);
            this.chkInverseAileron.Name = "chkInverseAileron";
            this.chkInverseAileron.Size = new System.Drawing.Size(56, 17);
            this.chkInverseAileron.TabIndex = 10;
            this.chkInverseAileron.Text = "inv-ailr";
            this.chkInverseAileron.UseVisualStyleBackColor = true;
            // 
            // LogFileBox
            // 
            this.LogFileBox.AutoSize = true;
            this.LogFileBox.Location = new System.Drawing.Point(8, 19);
            this.LogFileBox.Name = "LogFileBox";
            this.LogFileBox.Size = new System.Drawing.Size(69, 17);
            this.LogFileBox.TabIndex = 2;
            this.LogFileBox.Text = "Log File: ";
            this.LogFileBox.UseVisualStyleBackColor = true;
            this.LogFileBox.CheckedChanged += new System.EventHandler(this.LogFileBox_CheckedChanged);
            // 
            // chkInverserElevator
            // 
            this.chkInverserElevator.AutoSize = true;
            this.chkInverserElevator.Location = new System.Drawing.Point(88, 88);
            this.chkInverserElevator.Name = "chkInverserElevator";
            this.chkInverserElevator.Size = new System.Drawing.Size(63, 17);
            this.chkInverserElevator.TabIndex = 10;
            this.chkInverserElevator.Text = "inv-elev";
            this.chkInverserElevator.UseVisualStyleBackColor = true;
            // 
            // _rs232CheckBox
            // 
            this._rs232CheckBox.AutoSize = true;
            this._rs232CheckBox.Location = new System.Drawing.Point(8, 42);
            this._rs232CheckBox.Name = "_rs232CheckBox";
            this._rs232CheckBox.Size = new System.Drawing.Size(127, 17);
            this._rs232CheckBox.TabIndex = 1;
            this._rs232CheckBox.Text = "Enable COM1 Output";
            this._rs232CheckBox.UseVisualStyleBackColor = true;
            this._rs232CheckBox.CheckedChanged += new System.EventHandler(this._rs232CheckBox_CheckedChanged);
            // 
            // chkAutoPilot
            // 
            this.chkAutoPilot.AutoSize = true;
            this.chkAutoPilot.Location = new System.Drawing.Point(8, 88);
            this.chkAutoPilot.Name = "chkAutoPilot";
            this.chkAutoPilot.Size = new System.Drawing.Size(71, 17);
            this.chkAutoPilot.TabIndex = 9;
            this.chkAutoPilot.Text = "Auto Pilot";
            this.chkAutoPilot.UseVisualStyleBackColor = true;
            // 
            // _soundCheckBox
            // 
            this._soundCheckBox.AutoSize = true;
            this._soundCheckBox.Location = new System.Drawing.Point(8, 65);
            this._soundCheckBox.Name = "_soundCheckBox";
            this._soundCheckBox.Size = new System.Drawing.Size(208, 17);
            this._soundCheckBox.TabIndex = 0;
            this._soundCheckBox.Text = "Enable FlightGear Message to Speech";
            this._soundCheckBox.UseVisualStyleBackColor = true;
            this._soundCheckBox.CheckedChanged += new System.EventHandler(this._soundCheckBox_CheckedChanged);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.ChoosePortLabel);
            this.groupBox2.Controls.Add(this._startBtn);
            this.groupBox2.Controls.Add(this.PortBox);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.IpComboBox);
            this.groupBox2.Location = new System.Drawing.Point(14, 165);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(298, 108);
            this.groupBox2.TabIndex = 2;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Server Settings";
            // 
            // ChoosePortLabel
            // 
            this.ChoosePortLabel.AutoSize = true;
            this.ChoosePortLabel.Location = new System.Drawing.Point(6, 22);
            this.ChoosePortLabel.Name = "ChoosePortLabel";
            this.ChoosePortLabel.Size = new System.Drawing.Size(120, 13);
            this.ChoosePortLabel.TabIndex = 4;
            this.ChoosePortLabel.Text = "Choose Server and Port";
            // 
            // _startBtn
            // 
            this._startBtn.Location = new System.Drawing.Point(6, 65);
            this._startBtn.Name = "_startBtn";
            this._startBtn.Size = new System.Drawing.Size(286, 37);
            this._startBtn.TabIndex = 3;
            this._startBtn.Text = "Start Server";
            this._startBtn.UseVisualStyleBackColor = true;
            this._startBtn.Click += new System.EventHandler(this._startBtn_Click);
            // 
            // PortBox
            // 
            this.PortBox.Location = new System.Drawing.Point(202, 39);
            this.PortBox.Name = "PortBox";
            this.PortBox.Size = new System.Drawing.Size(91, 20);
            this.PortBox.TabIndex = 2;
            this.PortBox.Text = "5555";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(178, 41);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(18, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "@";
            // 
            // IpComboBox
            // 
            this.IpComboBox.FormattingEnabled = true;
            this.IpComboBox.Location = new System.Drawing.Point(7, 38);
            this.IpComboBox.Name = "IpComboBox";
            this.IpComboBox.Size = new System.Drawing.Size(166, 21);
            this.IpComboBox.TabIndex = 0;
            this.IpComboBox.Text = "127.0.0.1";
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.txtAileron);
            this.groupBox3.Controls.Add(this.PitchValBox);
            this.groupBox3.Controls.Add(this.RollValBox);
            this.groupBox3.Controls.Add(this.TimeBox);
            this.groupBox3.Controls.Add(this.fgfsMessageBox);
            this.groupBox3.Controls.Add(this.label3);
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.AltitudeBox);
            this.groupBox3.Controls.Add(this.SpeedBox);
            this.groupBox3.Controls.Add(this.PitchBar);
            this.groupBox3.Controls.Add(this.RollBar);
            this.groupBox3.Location = new System.Drawing.Point(3, 158);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(342, 175);
            this.groupBox3.TabIndex = 3;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Controls";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(8, 119);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(14, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "P";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(274, 20);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(14, 13);
            this.label8.TabIndex = 13;
            this.label8.Text = "A";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(8, 77);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(15, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "R";
            // 
            // txtAileron
            // 
            this.txtAileron.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAileron.Location = new System.Drawing.Point(295, 18);
            this.txtAileron.Name = "txtAileron";
            this.txtAileron.ReadOnly = true;
            this.txtAileron.Size = new System.Drawing.Size(32, 20);
            this.txtAileron.TabIndex = 13;
            // 
            // PitchValBox
            // 
            this.PitchValBox.Location = new System.Drawing.Point(28, 116);
            this.PitchValBox.Name = "PitchValBox";
            this.PitchValBox.ReadOnly = true;
            this.PitchValBox.Size = new System.Drawing.Size(32, 20);
            this.PitchValBox.TabIndex = 10;
            // 
            // RollValBox
            // 
            this.RollValBox.Location = new System.Drawing.Point(28, 74);
            this.RollValBox.Name = "RollValBox";
            this.RollValBox.ReadOnly = true;
            this.RollValBox.Size = new System.Drawing.Size(32, 20);
            this.RollValBox.TabIndex = 9;
            // 
            // TimeBox
            // 
            this.TimeBox.Location = new System.Drawing.Point(9, 146);
            this.TimeBox.Name = "TimeBox";
            this.TimeBox.ReadOnly = true;
            this.TimeBox.Size = new System.Drawing.Size(59, 20);
            this.TimeBox.TabIndex = 8;
            // 
            // fgfsMessageBox
            // 
            this.fgfsMessageBox.Location = new System.Drawing.Point(70, 147);
            this.fgfsMessageBox.Name = "fgfsMessageBox";
            this.fgfsMessageBox.ReadOnly = true;
            this.fgfsMessageBox.Size = new System.Drawing.Size(222, 20);
            this.fgfsMessageBox.TabIndex = 7;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(67, 119);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Altitude (ft)";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(67, 81);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Speed (kt)";
            // 
            // AltitudeBox
            // 
            this.AltitudeBox.Location = new System.Drawing.Point(126, 116);
            this.AltitudeBox.Name = "AltitudeBox";
            this.AltitudeBox.ReadOnly = true;
            this.AltitudeBox.Size = new System.Drawing.Size(72, 20);
            this.AltitudeBox.TabIndex = 4;
            // 
            // SpeedBox
            // 
            this.SpeedBox.Location = new System.Drawing.Point(126, 81);
            this.SpeedBox.Name = "SpeedBox";
            this.SpeedBox.ReadOnly = true;
            this.SpeedBox.Size = new System.Drawing.Size(72, 20);
            this.SpeedBox.TabIndex = 4;
            // 
            // PitchBar
            // 
            this.PitchBar.Location = new System.Drawing.Point(223, 19);
            this.PitchBar.Name = "PitchBar";
            this.PitchBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.PitchBar.Size = new System.Drawing.Size(45, 122);
            this.PitchBar.TabIndex = 1;
            this.PitchBar.Value = 5;
            // 
            // RollBar
            // 
            this.RollBar.Location = new System.Drawing.Point(11, 19);
            this.RollBar.Name = "RollBar";
            this.RollBar.Size = new System.Drawing.Size(206, 45);
            this.RollBar.TabIndex = 0;
            this.RollBar.Value = 5;
            // 
            // btnReset
            // 
            this.btnReset.Location = new System.Drawing.Point(407, 481);
            this.btnReset.Name = "btnReset";
            this.btnReset.Size = new System.Drawing.Size(50, 20);
            this.btnReset.TabIndex = 11;
            this.btnReset.Text = "Reset";
            this.btnReset.UseVisualStyleBackColor = true;
            this.btnReset.Click += new System.EventHandler(this.btnReset_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(36, 465);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(15, 13);
            this.label9.TabIndex = 15;
            this.label9.Text = "R";
            // 
            // txtRoll
            // 
            this.txtRoll.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtRoll.Location = new System.Drawing.Point(39, 481);
            this.txtRoll.Name = "txtRoll";
            this.txtRoll.ReadOnly = true;
            this.txtRoll.Size = new System.Drawing.Size(32, 20);
            this.txtRoll.TabIndex = 14;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(372, 435);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(84, 13);
            this.label10.TabIndex = 17;
            this.label10.Text = "Current Heading";
            // 
            // txtHeading
            // 
            this.txtHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.txtHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtHeading.Location = new System.Drawing.Point(393, 451);
            this.txtHeading.Name = "txtHeading";
            this.txtHeading.ReadOnly = true;
            this.txtHeading.Size = new System.Drawing.Size(32, 20);
            this.txtHeading.TabIndex = 16;
            // 
            // trkAlironControl
            // 
            this.trkAlironControl.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.trkAlironControl.Location = new System.Drawing.Point(8, 532);
            this.trkAlironControl.Maximum = 180;
            this.trkAlironControl.Name = "trkAlironControl";
            this.trkAlironControl.Size = new System.Drawing.Size(280, 45);
            this.trkAlironControl.TabIndex = 16;
            this.trkAlironControl.Value = 90;
            this.trkAlironControl.ValueChanged += new System.EventHandler(this.trkAlironControl_ValueChanged);
            // 
            // timer1
            // 
            this.timer1.Enabled = true;
            this.timer1.Interval = 1000;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // TimerBox
            // 
            this.TimerBox.Location = new System.Drawing.Point(71, 301);
            this.TimerBox.Name = "TimerBox";
            this.TimerBox.ReadOnly = true;
            this.TimerBox.Size = new System.Drawing.Size(44, 20);
            this.TimerBox.TabIndex = 4;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(20, 304);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(52, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Seconds:";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(249, 301);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(61, 20);
            this.button1.TabIndex = 6;
            this.button1.Text = "Exit";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(121, 304);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(50, 13);
            this.label5.TabIndex = 7;
            this.label5.Text = "Updates:";
            // 
            // MsgCntBox
            // 
            this.MsgCntBox.Location = new System.Drawing.Point(175, 302);
            this.MsgCntBox.Name = "MsgCntBox";
            this.MsgCntBox.ReadOnly = true;
            this.MsgCntBox.Size = new System.Drawing.Size(52, 20);
            this.MsgCntBox.TabIndex = 8;
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(0, 27);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(747, 632);
            this.tabControl1.TabIndex = 11;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.btnDebug);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.MsgCntBox);
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.label4);
            this.tabPage1.Controls.Add(this.button1);
            this.tabPage1.Controls.Add(this.TimerBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(491, 593);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Server";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // btnDebug
            // 
            this.btnDebug.Location = new System.Drawing.Point(44, 359);
            this.btnDebug.Name = "btnDebug";
            this.btnDebug.Size = new System.Drawing.Size(75, 23);
            this.btnDebug.TabIndex = 9;
            this.btnDebug.Text = "Debug";
            this.btnDebug.UseVisualStyleBackColor = true;
            this.btnDebug.Click += new System.EventHandler(this.btnDebug_Click);
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.lstLog);
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(491, 593);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Autopilot";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // lstLog
            // 
            this.lstLog.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lstLog.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.columnHeader1,
            this.columnHeader2});
            this.lstLog.GridLines = true;
            this.lstLog.Location = new System.Drawing.Point(7, 9);
            this.lstLog.Name = "lstLog";
            this.lstLog.Size = new System.Drawing.Size(337, 149);
            this.lstLog.TabIndex = 17;
            this.lstLog.UseCompatibleStateImageBehavior = false;
            this.lstLog.View = System.Windows.Forms.View.Details;
            // 
            // columnHeader1
            // 
            this.columnHeader1.Text = "Time";
            // 
            // columnHeader2
            // 
            this.columnHeader2.Text = "Action";
            this.columnHeader2.Width = 269;
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label14);
            this.tabPage3.Controls.Add(this.txtAltitude);
            this.tabPage3.Controls.Add(this.label13);
            this.tabPage3.Controls.Add(this.txtPitch);
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.btnReset);
            this.tabPage3.Controls.Add(this.label12);
            this.tabPage3.Controls.Add(this.textDistance);
            this.tabPage3.Controls.Add(this.txtRoll);
            this.tabPage3.Controls.Add(this.label10);
            this.tabPage3.Controls.Add(this.txtAileronControlValue);
            this.tabPage3.Controls.Add(this.label11);
            this.tabPage3.Controls.Add(this.txtHeading);
            this.tabPage3.Controls.Add(this.txtTargetHeading);
            this.tabPage3.Controls.Add(this.groupBox4);
            this.tabPage3.Controls.Add(this.trkAlironControl);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(739, 606);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Map";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(259, 453);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(47, 13);
            this.label14.TabIndex = 25;
            this.label14.Text = "altitude :";
            // 
            // txtAltitude
            // 
            this.txtAltitude.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAltitude.Location = new System.Drawing.Point(312, 451);
            this.txtAltitude.Name = "txtAltitude";
            this.txtAltitude.ReadOnly = true;
            this.txtAltitude.Size = new System.Drawing.Size(32, 20);
            this.txtAltitude.TabIndex = 24;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(90, 465);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(14, 13);
            this.label13.TabIndex = 23;
            this.label13.Text = "P";
            // 
            // txtPitch
            // 
            this.txtPitch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtPitch.Location = new System.Drawing.Point(93, 481);
            this.txtPitch.Name = "txtPitch";
            this.txtPitch.ReadOnly = true;
            this.txtPitch.Size = new System.Drawing.Size(32, 20);
            this.txtPitch.TabIndex = 22;
            // 
            // label12
            // 
            this.label12.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(501, 435);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(53, 13);
            this.label12.TabIndex = 21;
            this.label12.Text = "distance :";
            // 
            // textDistance
            // 
            this.textDistance.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.textDistance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.textDistance.Location = new System.Drawing.Point(560, 438);
            this.textDistance.Name = "textDistance";
            this.textDistance.ReadOnly = true;
            this.textDistance.Size = new System.Drawing.Size(97, 20);
            this.textDistance.TabIndex = 20;
            // 
            // txtAileronControlValue
            // 
            this.txtAileronControlValue.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtAileronControlValue.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtAileronControlValue.Location = new System.Drawing.Point(256, 583);
            this.txtAileronControlValue.Name = "txtAileronControlValue";
            this.txtAileronControlValue.ReadOnly = true;
            this.txtAileronControlValue.Size = new System.Drawing.Size(32, 20);
            this.txtAileronControlValue.TabIndex = 18;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(17, 435);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(81, 13);
            this.label11.TabIndex = 19;
            this.label11.Text = "Target Heading";
            // 
            // txtTargetHeading
            // 
            this.txtTargetHeading.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.txtTargetHeading.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtTargetHeading.Location = new System.Drawing.Point(39, 451);
            this.txtTargetHeading.Name = "txtTargetHeading";
            this.txtTargetHeading.ReadOnly = true;
            this.txtTargetHeading.Size = new System.Drawing.Size(32, 20);
            this.txtTargetHeading.TabIndex = 18;
            this.txtTargetHeading.Text = "0.0";
            // 
            // groupBox4
            // 
            this.groupBox4.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox4.Controls.Add(this.MainMap);
            this.groupBox4.Controls.Add(this.trkMapZoom);
            this.groupBox4.Location = new System.Drawing.Point(8, 3);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(713, 429);
            this.groupBox4.TabIndex = 2;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "groupBox4";
            // 
            // MainMap
            // 
            this.MainMap.Bearing = 0F;
            this.MainMap.CanDragMap = true;
            this.MainMap.Dock = System.Windows.Forms.DockStyle.Fill;
            this.MainMap.GrayScaleMode = false;
            this.MainMap.LevelsKeepInMemmory = 5;
            this.MainMap.Location = new System.Drawing.Point(3, 16);
            this.MainMap.MarkersEnabled = true;
            this.MainMap.MaxZoom = 2;
            this.MainMap.MinZoom = 2;
            this.MainMap.MouseWheelZoomType = GMap.NET.MouseWheelZoomType.MousePositionAndCenter;
            this.MainMap.Name = "MainMap";
            this.MainMap.NegativeMode = false;
            this.MainMap.PolygonsEnabled = true;
            this.MainMap.RetryLoadTile = 0;
            this.MainMap.RoutesEnabled = true;
            this.MainMap.ShowTileGridLines = false;
            this.MainMap.Size = new System.Drawing.Size(662, 410);
            this.MainMap.TabIndex = 0;
            this.MainMap.Zoom = 0D;
            // 
            // trkMapZoom
            // 
            this.trkMapZoom.Dock = System.Windows.Forms.DockStyle.Right;
            this.trkMapZoom.Location = new System.Drawing.Point(665, 16);
            this.trkMapZoom.Maximum = 1700;
            this.trkMapZoom.Name = "trkMapZoom";
            this.trkMapZoom.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.trkMapZoom.Size = new System.Drawing.Size(45, 410);
            this.trkMapZoom.TabIndex = 1;
            this.trkMapZoom.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trkMapZoom.Value = 900;
            this.trkMapZoom.ValueChanged += new System.EventHandler(this.trkMapZoom_ValueChanged);
            // 
            // FgfsMainForm
            // 
            this.ClientSize = new System.Drawing.Size(747, 655);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.menuStrip1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "FgfsMainForm";
            this.Text = "FlightGear Interface";
            this.Load += new System.EventHandler(this.FgfsMainForm_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.PitchBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.RollBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trkAlironControl)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trkMapZoom)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion

        private void timer1_Tick(object sender, EventArgs e)
        {
            _timerticks++;
            TimerBox.Text = _timerticks.ToString();
            Server = FgfsServer.GetInstance();
            if (Server.Status == 2)
            {
                // thread exited
                if (Is_Server_Running())
                {
                    Console.WriteLine("Stopping server, on timer tick...");
                    Stop_Server();
                }
            }
            // TimeBox.Text = DateTime.Now.ToShortTimeString();
            TimeBox.Text = DateTime.Now.ToLongTimeString();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Application_Exit();
        }


        private void FgfsMainForm_Load(object sender, EventArgs e)
        {

            #region "Map Control init"
            // config map 
            MainMap.MapProvider = GMapProviders.OpenStreetMap;
            MainMap.Position = new PointLatLng(const_StartLatitude, const_StartLongitude);
            MainMap.MinZoom = 1;
            MainMap.MaxZoom = 24;
            MainMap.Zoom = trkMapZoom.Value / 100;
            MainMap.CanDragMap = true;

            mAutoPilot= new AutoPilot(const_StartLatitude, const_StartLongitude, 2000);

            top = new GMapOverlay(MainMap,"top");
            mGMapRoutes = new GMapOverlay(MainMap, "routes");
            mGMapPolygons = new GMapOverlay(MainMap, "polygons");
            mGMapObjects = new GMapOverlay(MainMap, "objects");
            mGMapObjects.Markers.Add(mAutoPilot.GMapMarkerPlane);
            

            #region "GMap Custom Layer"
            {
                MainMap.Overlays.Add(mGMapRoutes);
                MainMap.Overlays.Add(mGMapPolygons);
                MainMap.Overlays.Add(mGMapObjects);
                MainMap.Overlays.Add(top);

                mGMapRoutes.Routes.CollectionChanged += new GMap.NET.ObjectModel.NotifyCollectionChangedEventHandler(Routes_CollectionChanged);
                mGMapObjects.Markers.CollectionChanged += new GMap.NET.ObjectModel.NotifyCollectionChangedEventHandler(Markers_CollectionChanged);
            }
            #endregion

            #region "MapEvents"
            
            MainMap.OnTileLoadStart += new TileLoadStart(MainMap_OnTileLoadStart);
            MainMap.OnTileLoadComplete += new TileLoadComplete(MainMap_OnTileLoadComplete);

            MainMap.OnMapTypeChanged += new MapTypeChanged(MainMap_OnMapTypeChanged);
            MainMap.OnPositionChanged += new PositionChanged(MainMap_OnPositionChanged);
            MainMap.OnMapZoomChanged += new MapZoomChanged(MainMap_OnMapZoomChanged);

            MainMap.OnMarkerClick += new MarkerClick(MainMap_OnMarkerClick);
            MainMap.OnMarkerEnter += new MarkerEnter(MainMap_OnMarkerEnter);
            MainMap.OnMarkerLeave += new MarkerLeave(MainMap_OnMarkerLeave);

            MainMap.MouseMove += new MouseEventHandler(MainMap_MouseMove);
            MainMap.MouseDown += new MouseEventHandler(MainMap_MouseDown);
            MainMap.MouseUp += new MouseEventHandler(MainMap_MouseUp);
            MainMap.MouseDoubleClick += new MouseEventHandler(MainMap_MouseDoubleClick);

            #endregion

            #endregion
        }


        #region "Map Control"

        protected void trkMapZoom_ValueChanged(object sender, EventArgs e)
        {
            MainMap.Zoom = trkMapZoom.Value / 100.0;
        }


        protected void MainMap_OnTileLoadStart()
        {
        }

        protected void MainMap_OnTileLoadComplete(long ElapsedMilliseconds)
        {
        }

        protected void MainMap_OnMapTypeChanged(GMapProvider type)
        {
        }

        protected void MainMap_OnMapZoomChanged()
        {
             int Zoom = (int) (MainMap.Zoom * 100);
             if (Zoom > trkMapZoom.Maximum)
             {
                 trkMapZoom.Value = trkMapZoom.Maximum;
             }else
                 if (Zoom < trkMapZoom.Minimum)
                 {
                     trkMapZoom.Value = trkMapZoom.Minimum;
                 }
                 else
                 {
                     trkMapZoom.Value = Zoom;
                 }

        }



        protected void MainMap_MouseMove(object sender, MouseEventArgs e)
        {
        }

        protected void MainMap_MouseDown(object sender, MouseEventArgs e)
        {
        }

        protected void MainMap_MouseUp(object sender, MouseEventArgs e)
        {
        }

        protected void MainMap_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            //mAutoPilot.GMapMarkerMileStones.Clear();
            GMapMarkerMileStone cc = new GMapMarkerMileStone(MainMap.FromLocalToLatLng(e.X, e.Y), 1000);
            mAutoPilot.GMapMarkerMileStones.Add(cc);
            mGMapObjects.Markers.Add(cc);
        }


        private void MainMap_OnPositionChanged(PointLatLng point)
        {
        }


        protected void MainMap_OnMarkerClick(GMapMarker item, MouseEventArgs e)
        {
            
        }

        protected void MainMap_OnMarkerEnter(GMapMarker item)
        {
        }


        protected void MainMap_OnMarkerLeave(GMapMarker item)
        {
        }

        protected void MainMap_OnPolygonEnter(GMapPolygon item)
        {
        }


        protected void MainMap_OnPolygonLeave(GMapPolygon item)
        {
        }

        protected void Markers_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
        {
        }

        protected void Routes_CollectionChanged(object sender, GMap.NET.ObjectModel.NotifyCollectionChangedEventArgs e)
        {
        }

        protected void SetPlanePosition(double lat, double lng)
        {

            mAutoPilot.GMapMarkerPlane.Position = new PointLatLng(lat, lng);

        }

        #endregion

        private void trkAlironControl_ValueChanged(object sender, EventArgs e)
        {
            txtAileronControlValue.Text = trkAlironControl.Value.ToString();
            mAutoPilot.TargetRollAngle = trkAlironControl.Value - 90.0;
        }

        private void btnReset_Click(object sender, EventArgs e)
        {
            try
            {
                System.Net.WebClient WebClient = new System.Net.WebClient();
                // Set Position
                WebClient.DownloadString(@"http://127.0.0.1:5500/position/latitude-deg?value=30.1062297&submit=update");
                WebClient.DownloadString(@"http://127.0.0.1:5500/position/longitude-deg?value=31.3777894&submit=update");

                WebClient.DownloadString(@"http://127.0.0.1:5500/orientation/pitch-deg?value=0&submit=update");
                WebClient.DownloadString(@"http://127.0.0.1:5500/orientation/roll-deg?value=0.0&submit=update");
                WebClient.DownloadString(@"http://127.0.0.1:5500/orientation/yaw-rate-degps?value=0.0&submit=update");

                WebClient.DownloadString(@"http://127.0.0.1:5500/controls/flight/elevator-trim?value=-0.00&submit=update");
                WebClient.DownloadString(@"http://127.0.0.1:5500/controls/flight/aileron-trim?value=-0.00&submit=update");
            }
            catch
            {
                return;
            }


            mAutoPilot.GMapMarkerMileStones.Clear();
            mGMapObjects.Markers.Clear();
            mGMapObjects.Markers.Add(mAutoPilot.GMapMarkerPlane);
        }

        private void btnDebug_Click(object sender, EventArgs e)
        {
            AutoPilot mA = new AutoPilot(30.0, 30.0,1000);

            StringBuilder sb = new StringBuilder();
            sb.Append("Target,Current,Diff,Roll\r\n");
            for (double j,i = -3.1; i <= 3.14; i += 0.3)
            {
                for (j = -3.1; j <= 3.14; j += 0.3)
                {
                    mA.CurrentHeading = i;
                    mA.TargetHeading = j;
                    mA.RollToBearingDifference();
                    sb.Append(mA.TargetHeading.ToString());
                    sb.Append(",");
                    sb.Append(mA.CurrentHeading.ToString());
                    sb.Append(",");
                    sb.Append(mA.HeadingDifference.ToString());
                    sb.Append(",");
                    sb.Append(mA.TargetRollAngle.ToString());
                    sb.Append("\r\n");
                }
            }
            


        }



    }
}

