/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;



import java.io.File;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;
import java.util.Locale;


import android.app.Activity;
import android.content.ComponentName;
import android.content.Context;
import android.content.Intent;
import android.content.ServiceConnection;
import android.graphics.Color;
import android.hardware.Sensor;
import android.hardware.SensorManager;
import android.location.LocationManager;
import android.net.wifi.WifiManager;
import android.os.Bundle;
import android.os.Environment;
import android.os.IBinder;
import android.os.PowerManager;
import android.speech.tts.TextToSpeech;
import android.telephony.TelephonyManager;
import android.util.Log;
import android.view.Menu;
import android.view.MenuInflater;
import android.view.MenuItem;
import android.view.View;

import android.widget.TextView;
import android.widget.Toast;
import android.widget.ToggleButton;

public class MainActivity extends Activity  implements TextToSpeech.OnInitListener 
{
    
	private final String WavFolder = Environment.getExternalStorageDirectory()+"/UAV/Wav/";
	private TextToSpeech mTts;
	
	/////////////////UI Attributes
	private ToggleButton mtbtnBroadCast; 
	private ToggleButton mtbtnBroadCastGPS; 
	private ToggleButton mtbtnBroadCastGyro; 
	private ToggleButton mtbtnBroadCastAcc; 
	private ToggleButton mtbtnBroadCastMagnetic; 
	private ToggleButton mtbtnBroadCastOrientation;
	private ToggleButton mtbtnSimulator;
	private ToggleButton mtbtnIOIO; 
	private TextView mtxtBroadCastID; 
	public TextView mtxtBroadCastGPS; 
	public TextView mtxtStation;
	public TextView mtxtSimulation;
	public TextView mtxtARMConnected;
	private TextView mtxtBroadCastGyro; 
	private TextView mtxtBroadCastAcc;
	private TextView mtxtBroadCastMagnetic; 
	private TextView mtxtBroadCastOrientation;
	private TextView mtxtIOIO;
	private Menu mMainMenu;
	
	////////////////EOF UI Attributes
	
	///////////////// Attributes
	private MainActivity mMe;
	private Comm_TCPServer  mBoundService=null;
	private boolean mIsBound = false;
	private boolean mbExit = false;
	
	private IDProtocol mIDProtocol;
	private Comm_FlightGear mComm_FlightGear; 
	private static UAVProtocol mUAVProtocol;
	private WifiManager mWifi;
	private UAVAutoPilot mUAVAutoPilot;
	
	
	private SensorManager mSensors;
	private Sensor mAccelerometer;
	private Sensor mMagnetic;
	private Sensor mGyro;
	private Sensor mOrientation;
	protected LocationManager mlocManager ;
	
	private Sensor_Accelerometer mSensor_Accelerometer;
	private Sensor_Gyro mSensor_Gyro;
	private Sensor_Magnetic mSensor_Magnetic;
	private Sensor_GPS mSensor_GPS;
	private Sensor_Orientation mSensor_Orientation;
	
	private PowerManager.WakeLock mWakeLock ;
	private PowerManager mPowerManagement; 
	
	//////////////EOF Attributes
	
	
	protected static final int mLocationTimeMinUpdate = 5000; 		// minimum millisec  time for wait between signals.
	protected static final int mLocationDistanceMinUpdate =  5;  	// minimum meters before call back
	
	
	
	/* ------------------------------------- Service Communication */
		
		private ServiceConnection mConnection = new ServiceConnection() {
		    public void onServiceConnected(ComponentName className, IBinder service) {
		        // This is called when the connection with the service has been
		        // established, giving us the service object we can use to
		        // interact with the service.  Because we have bound to a explicit
		        // service that we know is running in our own process, we can
		        // cast its IBinder to a concrete class and directly access it.
		        mBoundService = ((Comm_TCPServer.LocalBinder)service).getService();
		        //mbtnStart.setEnabled(false);
		        
				startService(new Intent(MainActivity.this,Comm_TCPServer.class)); // start the service
				mBoundService.mMainActivity=MainActivity.this;
				

		        // Tell the user about this for our demo.
		        Toast.makeText(MainActivity.this, "service connected",Toast.LENGTH_SHORT).show();
		    }

		    public void onServiceDisconnected(ComponentName className) {
		        // This is called when the connection with the service has been
		        // unexpectedly disconnected -- that is, its process crashed.
		        // Because it is running in our same process, we should never
		        // see this happen.

		    	Toast.makeText(MainActivity.this, "ServiceDisconnected",Toast.LENGTH_SHORT).show();	    	
		    	
				if (mbExit == true)
				{
		    	/*
		    	 * http://stackoverflow.com/questions/2176375/android-service-wont-stop
		    	 * 
		    	 *Since both unbindService() and stopService() are asynchronous, 
		    	 *something might be going haywire with the timing, in which case you 
		    	 *may get better luck if you call stopService() from your ServiceConnection's 
		    	 *onServiceDisconnected() method
		    	 */
				ExitApp();
				}

		    	mBoundService.mMainActivity=null;
		        mBoundService = null;
		        Toast.makeText(MainActivity.this, "Oops App Crashed !! pls restart :(",
		                Toast.LENGTH_SHORT).show();
		    }
		};

		
		void doBindService() {
		    // Establish a connection with the service.  We use an explicit
		    // class name because we want a specific service implementation that
		    // we know will be running in our own process (and thus won't be
		    // supporting component replacement by other applications).
			if (mIsBound==false)
			{
				bindService(new Intent(MainActivity.this,Comm_TCPServer.class), mConnection, Context.BIND_AUTO_CREATE);
				mIsBound = true;
			}
		}

		void doUnbindService() {
		    if (mIsBound) {
		        // Detach our existing connection.
		        unbindService(mConnection);
		        mIsBound = false;
		    }
		}

		
	/* EOF Service Communication */
		
			
		
		
	@SuppressWarnings("static-access")
	public static void NetSendInfo()
	{
		
		if (mUAVProtocol!= null)
			try {
				if (mUAVProtocol.IsStarted()==false)
				{
					return ;
				}
				mUAVProtocol.NetSendInfo();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	}
	
	
	@SuppressWarnings("static-access")
	public static void NetSendSwitchInfo()
	{
		if (UAVState.WIFI_Active==false) return;
		
		if (mUAVProtocol!= null)
				if (mUAVProtocol.IsStarted()==false)
				{
					return ;
				}
				//mUAVProtocol.NetSendSwitchInfo();
			
	}
	
	
	private void Init()
	{
		// Create Folder.
		File Folder = new File(Environment.getExternalStorageDirectory(), "UAV");
        if (!Folder.exists()) {
        	Folder.mkdirs();
        } 
        Folder = new File(Environment.getExternalStorageDirectory()+"/UAV", "WAV");
        if (!Folder.exists()) {
        	Folder.mkdirs();
        } 
        
		mMe = this;
		mTts = new TextToSpeech(this, this);
		
		 
		UAVState.IOIOPWMCalibration= IOIOPWMCalibration.Create();
		IOIOPWMCalibration.ReadCalibrationPreference(getApplication(),UAVState.IOIOPWMCalibration);
		Logger.Init();
		GPSLogger.Init();
		
		UAVState.Arming=false;
		UAVState.Calibrated = false;
		
		UAVState.Accelerometer_Active=false;
		UAVState.Gyro_Active=false;
		UAVState.Magnetic_Active = false;
		UAVState.GPS_Active=false;
		UAVState.Orientation_Active=false;
		
		
		UAVState.Debug=" ";
		
		setContentView(R.layout.main);
		
		mtxtStation= (TextView) findViewById(R.id.txtStationConnected);
		mtxtSimulation= (TextView) findViewById(R.id.txtSimulatorConnected);
		mtxtARMConnected = (TextView) findViewById (R.id.txtARMConnected);
		mtxtStation.setTextColor(Color.rgb(0, 0, 220));
		mtxtSimulation.setTextColor(Color.rgb(0, 0, 220));
		mtxtARMConnected.setTextColor(Color.rgb(0, 0, 220));
	    
		mtxtIOIO = (TextView) findViewById(R.id.txtIOIO);
		mtxtBroadCastID = (TextView) findViewById(R.id.txtBroadcastID);
		mtxtBroadCastGPS = (TextView) findViewById(R.id.txtBroadcastGPS);
	    mtxtBroadCastGyro = (TextView) findViewById(R.id.txtBroadcastGyro);
	    mtxtBroadCastAcc = (TextView) findViewById(R.id.txtBroadcastAcc);
	    mtxtBroadCastMagnetic = (TextView) findViewById(R.id.txtBroadcastMagnetic);
	    mtxtBroadCastOrientation = (TextView) findViewById(R.id.txtBroadcastOrientation);   
	    
	    mtbtnIOIO = (ToggleButton) findViewById (R.id.tbtnIOIO);
	    mtbtnBroadCast = (ToggleButton) findViewById(R.id.tbtnBroadcastID);
	    mtbtnBroadCastGPS = (ToggleButton) findViewById(R.id.tbtnBroadcastGPS);
	    mtbtnBroadCastGyro = (ToggleButton) findViewById(R.id.tbtnBroadcastGyro);
	    mtbtnBroadCastAcc = (ToggleButton) findViewById(R.id.tbtnBroadcastAcc);
	    mtbtnBroadCastMagnetic = (ToggleButton) findViewById(R.id.tbtnBroadcastMagnetic);
	    mtbtnBroadCastOrientation = (ToggleButton) findViewById(R.id.tbtnBroadcastOrientation);
	    mtbtnSimulator = (ToggleButton) findViewById (R.id.tbtnSimulation);
	    mMainMenu = (Menu) findViewById(R.menu.mainmenu);
	    
	    try
		{
		// Sensors
		mSensors = (SensorManager) this.getSystemService(Context.SENSOR_SERVICE);
		mSensor_Gyro = new Sensor_Gyro();
		mSensor_Accelerometer = new Sensor_Accelerometer();
		mSensor_Magnetic = new Sensor_Magnetic();
		mSensor_GPS= new Sensor_GPS(this);
		mSensor_Orientation = new Sensor_Orientation (); 
		//////////////////////// EOF Sensors
		
		UAVState.Simulator_Active= false;
		UAVState.RoutePoint_ActiveIndex=-1;
		UAVState.RoutePoint = new ArrayList<RoutePoint>();
		
		doBindService();
		}
		catch (Exception e)
		{
			Log.e("UAV", e.getMessage());
		}
	    mPowerManagement= (PowerManager) getSystemService(POWER_SERVICE);
	    mWakeLock = mPowerManagement.newWakeLock(PowerManager.SCREEN_DIM_WAKE_LOCK, "UAV");
        mWakeLock.acquire();
       
	
	}
	
	   @Override 
	    protected void onStart()
	    {
	    	/**
	    	 * using data from saved instance we can determine if we need to re-bind to a started service or not.
	    	 */
	    	
	    	super.onStart();
	    	
	    	
		
			
	    }
	     
	   
	@Override
	public void onPause()
	{
	  	super.onPause();
	    	
	}

	    
	public void NetCMD_ActiveGPS()
	{
	   	if (UAVState.Gyro_Active == true) return ;
	   	mtbtnBroadCastGPS.setChecked(true);
	   	ActivateGPS();
	    	
	   	return ;
	}
	   
	public void NetCMD_ActiveGyro()
	{
	   	if (UAVState.Gyro_Active == true) return ;
	   	mtbtnBroadCastGyro.setChecked(true);
	   	ActivateGyro();
	    	
	   	return ;
	}
	    
	public void NetCMD_ActiveAcc()
	{
	   	if (UAVState.Accelerometer_Active== true) return ;
	   	mtbtnBroadCastAcc.setChecked(true);
	   	ActivateAcc();
	    	
	   	return ;
	}
	    
    public void NetCMD_ActiveMag()
	{
	   	if (UAVState.Magnetic_Active== true) return ;
	   	mtbtnBroadCastMagnetic.setChecked(true);
	  	ActivateMag();
	    	
	  	return ;
	 }
	    
	 public void NetCMD_ActiveOrientation()
	 {
	    if (UAVState.Orientation_Active== true) return ;
	    mtbtnBroadCastOrientation.setChecked(true);
	    ActivateOrientation();
	    	
	    return ;
	 }
	    
	 public void NetCMD_ActiveSimulator()
	 {
	    if (UAVState.Simulator_Active== true) return ;
	    mtbtnSimulator.setChecked(true);
	    ActivateSimulator();
	    	
	    return ;
	}
	    
	public void NetCMD_ActivateIOIO()
	{
		if (UAVState.IOIO_Active== true) return ;
			mtbtnIOIO.setChecked(true);
		    ActivateIOIO();
		    	
		return ;
	}
		
	public void NetCMD_ReleaseIOIO()
	{
	   	if (UAVState.IOIO_Active == false) return ;
	   	mtbtnIOIO.setChecked(false);
	   	ReleaseIOIO();
	    	
	   	return ;
	}
	    
	public void NetCMD_ReleaseGPS()
	    {
	    	if (UAVState.GPS_Active == false) return ;
	    	mtbtnBroadCastGPS.setChecked(false);
	    	ReleaseGPS();
	    	
	    	return ;
	    }

    public void NetCMD_ReleaseGyro()
    {
    	if (UAVState.Gyro_Active == false) return ;
    	mtbtnBroadCastGyro.setChecked(false);
    	ReleaseGyro();
    	
    	return ;
    }
   
    public void NetCMD_ReleaseAcc()
    {
    	if (UAVState.Accelerometer_Active== false) return ;
    	mtbtnBroadCastAcc.setChecked(false);
    	ReleaseAcc();
    	
    	return ;
    }
    
    public void NetCMD_ReleaseOrientation()
    {
    	if (UAVState.Orientation_Active== false) return ;
    	mtbtnBroadCastOrientation.setChecked(false);
    	ReleaseOrientation();
    	
    	return ;
    }
    
    public void NetCMD_ReleaseMag()
    {
    	if (UAVState.Magnetic_Active== false) return ;
    	mtbtnBroadCastMagnetic.setChecked(false);
    	ReleaseMag();
    	
    	return ;
    }
    
    public void NetCMD_ReleaseSimulator()
    {
    	if (UAVState.Simulator_Active== false) return ;
    	mtbtnSimulator.setChecked(false);
    	ReleaseSimulator();
    	
    	return ;
    }
    
    protected void ActivateIOIO()
    {
    	if (UAVState.IOIO_Active==true) return;
    	IOIOHardware.Connect();
    	
    	mtxtIOIO.setText("IOIO Connected...");
		
		UAVState.IOIO_Active=true;
		NetSendSwitchInfo();
		
		speakOut("Yoyo Activated");
    }
    
    protected void ReleaseIOIO()
	{
		if (IOIOHardware.mIOIO == null) return ;
		
		IOIOHardware.Disconnect();
		mtxtIOIO.setText("IOIO Disconnected...");
		
		UAVState.IOIO_Active=false;
		NetSendSwitchInfo();
		
		speakOut("Yoyo Deactivated");
	}
	
    protected void ReleaseGPS ()
	{
		if (UAVState.GPS_Active)
		{
			mlocManager.removeUpdates(mSensor_GPS);
			mlocManager.removeGpsStatusListener(mSensor_GPS);
			mtxtBroadCastGPS.setText("GPS Disconnected...");
		}
		
		UAVState.GPS_Active = false;
		NetSendSwitchInfo();
		
		speakOut("GPS Deactivated");
	}
	
	protected  void ReleaseGyro ()
	{
	
		if (UAVState.Gyro_Active)
		{
			mSensors.unregisterListener(mSensor_Gyro);
			mtxtBroadCastGyro.setText("Gyro Disconnected");
		}
		
		UAVState.Gyro_Active = false;
		NetSendSwitchInfo();
		
		speakOut("Gyro Deactivated");
	}
	
	
	protected void ReleaseAcc ()
	{
	
		if (UAVState.Accelerometer_Active)
		{
			mSensors.unregisterListener(mSensor_Accelerometer);
			mtxtBroadCastAcc.setText("Acc Disconnected");
		}
		
		UAVState.Accelerometer_Active = false;
		NetSendSwitchInfo();
		
		speakOut("Accelerometer Deactivated");
	}
	
	protected void ReleaseOrientation()
	{
		if (UAVState.Orientation_Active)
		{
			mSensors.unregisterListener(mSensor_Orientation);
			mtxtBroadCastOrientation.setText("Orientation Disconnected.");
		}
		
		UAVState.Orientation_Active = false;
		NetSendSwitchInfo();
		
		speakOut("Orientation Deactivated");
	}
	
	protected void ReleaseMag ()
	{
		if (UAVState.Magnetic_Active)
		{
			mSensors.unregisterListener(mSensor_Magnetic);
			mtxtBroadCastMagnetic.setText("Magnetic Disconnected.");
		}
		
		UAVState.Magnetic_Active = false;
		NetSendSwitchInfo();
		
		speakOut("Magnetic Deactivated");
	}
	
	protected void ActivateSimulator()
	 {
	    	this.NetCMD_ReleaseGPS();
	    	this.NetCMD_ReleaseOrientation();
	    	this.NetCMD_ReleaseMag();
	    	// TODO Auto-generated method stub
	    	mComm_FlightGear = new Comm_FlightGear(mWifi,getApplication(),mMe);
			mComm_FlightGear.start();
			
			mUAVAutoPilot = new UAVAutoPilot(mComm_FlightGear);
			mUAVAutoPilot.start();
			
			UAVState.Simulator_Active = true;
	    	NetSendSwitchInfo();
	    	
	    	speakOut("Simulator Activated");
	 }
	    
	protected void ReleaseSimulator()
	{
		// TODO Auto-generated method stub
		try
		{
			UAVState.Simulator_Active=false;
			mUAVAutoPilot.interrupt();
			mUAVAutoPilot=null;
			
			mComm_FlightGear.CloseSocketOut();
			mComm_FlightGear.mExit=true;
			mComm_FlightGear.interrupt();
			mComm_FlightGear=null;
			
			NetSendSwitchInfo();
			
			speakOut("Simulator Deactivated");
		}
		catch (Exception e)
		{
		
		}
	}
	
	protected void ReleaseBroadCast ()
	{
		if (mUAVProtocol!=null)
		{
		mUAVProtocol.mExit=true;
		mUAVProtocol.interrupt();
		mUAVProtocol=null;
		mtxtBroadCastID.setText("Try Stop Broadcast...");
		}
		
		
		if (mIDProtocol!=null)
		{
		IDProtocol.mExit=true;
		mIDProtocol.interrupt();
		mIDProtocol=null;
		mtxtBroadCastID.setText("No Broadcast.");
		}
		UAVState.WIFI_Active=false;
		
		speakOut("Broadcast Deactivated");
	}
	
	private void ExitApp()
	{
		//stopService (new Intent(MainActivity.this,Comm_TCPServer.class));
		UAVState.WIFI_Active=false;
    	ReleaseOrientation();
		ReleaseGPS();
		ReleaseAcc();
		ReleaseGyro();
		ReleaseSimulator();
		ReleaseBroadCast();
		GPSLogger.Close();
		Logger.Close();
		//mIDProtocol.stop();
		mWakeLock.release();
		
		//this.closeContextMenu();
		 // Unbind from the service
		
		if (mTts != null) {
   		 mTts.stop();
   		 mTts.shutdown();
        }
		
		doUnbindService();
    	stopService (new Intent(MainActivity.this,Comm_TCPServer.class));
    	
		this.finish();
	}
	
	@Override
	public boolean onCreateOptionsMenu(Menu menu) {
		MenuInflater inflater = getMenuInflater();
	 	inflater.inflate(R.menu.mainmenu, menu);
	 	return true;
	}
	
	@Override
	public boolean onOptionsItemSelected(MenuItem item) {
	    // Handle item selection
	    switch (item.getItemId()) {
	    
	    case R.id.miExit:
	    	ExitApp();
	    	/*
	    	if (mIsBound==true)
	    	{
	    	   	doUnbindService(); // call stop service in onDisconnect event
	    	}
	    	else
	    	{
	    		// stop is called with disconnect if bounded.
	    		stopService (new Intent(MainActivity.this,LocationService.class));
	    		this.finish();
	    	}
	    	*/
	    	
	    	return true;
	        
	    case R.id.miPreference:
	    	startActivity (new Intent (this, SettingsActivity.class));
	    	return true;
	    	
	   
	    default:
	        return super.onOptionsItemSelected(item);
	    }
	}

	
	/** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        
    	
        Init();
        
      	
    /*	if (savedInstanceState != null)
    	{
    			mShouldConnectOnReStart =savedInstanceState.getBoolean("mIsBound");
    			if (mBoundService !=null)
    	    		mBoundService.ApplyPreferenceSttings();
    	    	
    	}
    	else
    	{
    		mShouldConnectOnReStart = false;
    	}
        */
        
        // Broadcast Button
        mtbtnBroadCast.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				
				if (UAVState.WIFI_Active==false)
				{
					ActivateBroadCast();
				}
				else
				{
					ReleaseBroadCast();
				}
					
			}
        	
        });
        
        mtbtnBroadCastGPS.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				mtxtBroadCastGPS.setText("GPS Connecting...");
				
				if (UAVState.GPS_Active==false)
				{
				
					ActivateGPS();
				}
				else
				{
					ReleaseGPS();
				}
			}
        	
        });
        
        // Gyro Button
        mtbtnBroadCastGyro.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				
				if (UAVState.Gyro_Active==false)
				{
					ActivateGyro();
				}
				else
				{
					ReleaseGyro();
				}
			}
        	
        });
        
        // Accelerometer Button
        mtbtnBroadCastAcc.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				
				if (UAVState.Accelerometer_Active==false)
				{
					ActivateAcc();	
				   	
				}
				else
				{
					ReleaseAcc();
				}
			}
        	
        });

        // Magnetic Button
        mtbtnBroadCastMagnetic.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				
				if (UAVState.Magnetic_Active==false)
				{
					ActivateMag();
				}
				else
				{
					ReleaseMag();
				}
				
			}
        	
        });
        
        
     // Orientation Button
        mtbtnBroadCastOrientation.setOnClickListener(new View.OnClickListener() 
        {

			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				
				UAVState.Orientation_Active = mtbtnBroadCastOrientation.isChecked();
				
				if (UAVState.Orientation_Active)
				{
					// TODO Auto-generated method stub
					ActivateOrientation();
				}
				else
				{
					ReleaseOrientation();
				}
				NetSendSwitchInfo();
			}
        	
        });
        
        
        // Orientation Button
        mtbtnSimulator.setOnClickListener(new View.OnClickListener() 
           {

   			@Override
   			public void onClick(View arg0) {
   				// TODO Auto-generated method stub
   				
   				UAVState.Simulator_Active = mtbtnSimulator.isChecked();
   				
   				if (UAVState.Simulator_Active==true)
   				{
   					
   				    ActivateSimulator();
   				
   				}
   				else
   				{
   					ReleaseSimulator();
   				}
   				  				
   				NetSendSwitchInfo();
   			}
           	
           });
    }
    
    
  
    
    protected void ActivateGPS ()
    {
    	mtxtBroadCastGPS.setText("Connecting to GPS");
		mlocManager = (LocationManager)getSystemService(Context.LOCATION_SERVICE);
		mtxtBroadCastGPS.setText ("getting location");
		mlocManager.addGpsStatusListener(mSensor_GPS);
	    mlocManager.requestLocationUpdates(LocationManager.GPS_PROVIDER, mLocationTimeMinUpdate, mLocationDistanceMinUpdate, mSensor_GPS);
	    
	    UAVState.GPS_Active = true;
	    NetSendSwitchInfo();
	    
	    speakOut ("GPS Activated");
	    
    }
    
    protected void ActivateGyro()
    {
		// TODO Auto-generated method stub
    	
		mtxtBroadCastGyro.setText("Gyro Connecting...");
		mGyro = mSensors.getDefaultSensor(Sensor.TYPE_GYROSCOPE);
		mSensors.registerListener(mSensor_Gyro, mGyro, SensorManager.SENSOR_DELAY_NORMAL);
		mtxtBroadCastGyro.setText("Gyro Connected.");
		
		UAVState.Gyro_Active = true;
	    NetSendSwitchInfo();
    
	    speakOut ("Gyro Activated");
    }
    
    protected void ActivateAcc()
    {
		// TODO Auto-generated method stub
		mtxtBroadCastAcc.setText("Acc Connecting...");
		mAccelerometer = mSensors.getDefaultSensor(Sensor.TYPE_ACCELEROMETER);
	    mSensors.registerListener(mSensor_Accelerometer, mAccelerometer, SensorManager.SENSOR_DELAY_NORMAL);
	    mtxtBroadCastAcc.setText("Acc Connected");
		
	    UAVState.Accelerometer_Active = true;
		NetSendSwitchInfo();
		
		speakOut ("Accelerometer Activated");
    }
    
    protected void ActivateMag()
    {
    	// TODO Auto-generated method stub
		mtxtBroadCastMagnetic.setText("Magnetic Connecting...");
		mMagnetic = mSensors.getDefaultSensor(Sensor.TYPE_MAGNETIC_FIELD);
		mSensors.registerListener(mSensor_Magnetic, mMagnetic, SensorManager.SENSOR_DELAY_NORMAL);
		mtxtBroadCastMagnetic.setText("Magnetic Connected.");
		
		UAVState.Magnetic_Active = true;
    	NetSendSwitchInfo();
    	
    	speakOut ("Magnetic Activated");
    }
    
    protected void ActivateOrientation()
    {
    	mtxtBroadCastOrientation.setText("Orientation Connecting...");
		mOrientation = mSensors.getDefaultSensor(Sensor.TYPE_ORIENTATION);
		mSensors.registerListener(mSensor_Orientation, mOrientation, SensorManager.SENSOR_DELAY_NORMAL);
		mtxtBroadCastOrientation.setText("Orientation Connected.");
		
		UAVState.Orientation_Active = true;
		
		speakOut ("Orientation Activated");
    }
    
   
    protected void ActivateBroadCast()
    {
    	mtxtBroadCastID.setText("Try Broadcast...");
		
		TelephonyManager telephonyManager = (TelephonyManager) getSystemService(Context.TELEPHONY_SERVICE);
		mWifi = (WifiManager) getSystemService(Context.WIFI_SERVICE);
		mIDProtocol = new IDProtocol(mWifi,getApplication(),telephonyManager.getDeviceId());
		mIDProtocol.start();
		mUAVProtocol = new UAVProtocol(mWifi,getApplication(),telephonyManager.getDeviceId());
		mUAVProtocol.start();
		String IP ="unknown";
		try {
			IP = NetHelper.getBroadcastAddress().toString();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			mtxtBroadCastID.setText("Broadcasting on" + IP);
			e.printStackTrace();
		}
		mtxtBroadCastID.setText("Broadcasting on" + IP);
		
		UAVState.WIFI_Active=true;
		
		speakOut ("BroadCast Activated");
    }
    
    /**
     * Process Life Cycle Diagram:
     * http://developer.android.com/reference/android/app/Activity.html
     */
    @Override
    public void onRestart () 
    {
    	super.onRestart();
    	
    }
    
    protected void onResume() {
        super.onResume();
        doBindService();
    }
    
    
 
      
    @Override
    public void onStop()
    {
    	
        super.onStop();
        
    }
    
    @Override
    public void onDestroy ()
    {
    	
    	 if (mTts != null) {
    		 mTts.stop();
    		 mTts.shutdown();
         }
    	
    	super.onDestroy();
    	
    }

	@Override
	public void onInit(int status) {
		// TODO Auto-generated method stub
		 if (status == TextToSpeech.SUCCESS) {
			 
	            int result = mTts.setLanguage(Locale.US);
	 
	            if (result == TextToSpeech.LANG_MISSING_DATA
	                    || result == TextToSpeech.LANG_NOT_SUPPORTED) {
	                Log.e("TTS", "This Language is not supported");
	            } else {
	                
	            	//mTts.addSpeech("Androiv Started", WavFolder + "AndroivStarted.wav");
	            	//mTts.addSpeech("Simulator Activated", WavFolder + "SimulatorActivated.wav");
	            	//mTts.addSpeech("Simulator Deactivated", WavFolder + "SimulatorDeactivated.wav");
	            	//mTts.addSpeech("Flight Gear ON", WavFolder + "FlightGearON.wav");
	            	//mTts.addSpeech("ConnectedToMonitor", WavFolder + "MonitorConnected.wav");
	            }
	 
	        } else {
	            Log.e("TTS", "Initilization Failed!");
	        }
	 
	}
	
	/*@Override
	public void onUtteranceCompleted(String uttId) {
	    if (uttId == "end of wakeup message ID") {
	       // playAnnoyingMusic();
	    } 
	}*/
	
    public void speakOut (String Text)
    {
    	if (mTts == null) return ;
    	mTts.speak(Text, TextToSpeech.QUEUE_FLUSH, null);
    }
    

    

}