package com.uav;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.Enumeration;

import android.net.DhcpInfo;
import android.net.wifi.WifiManager;

import android.util.Log;


public class UAVProtocol extends Thread{

	 protected static String mGUID;
	 protected static String mVehicleName;
	 protected static android.content.ContextWrapper mContext;
	 protected DatagramSocket mSocket;
	 public static int mUAVProtocolPort;
	 public static boolean mExit;
	 private static final int TIMEOUT_MS = 500;
	 protected static boolean mStarted = false;
	 protected static boolean mBusySending = false; 
	 protected static boolean mBusySending2 = false;
	 protected WifiManager mWifi;


	 
	 public static boolean IsStarted()
	 {
		 return mStarted;
	 }
	 
	 
	 public UAVProtocol(WifiManager wifi,android.content.ContextWrapper context, String GUID) {
		
		mExit = false;
	    mWifi = wifi;
	    mContext = context;
	    mGUID = GUID;
	    
	    ReadPreference();
		try {
				mSocket = new DatagramSocket(mUAVProtocolPort);
				mSocket.setBroadcast(true);
				mSocket.setSoTimeout(TIMEOUT_MS);
				mStarted = true;
		} catch (IOException e) {
	      Log.e("UAV", "Could not send discovery request", e);
	    }
	  }
	 
	 
	 
	 /**
	   * Read preference values for Port, GUID & Name
	   */
	  public void ReadPreference()
	  {
		  mUAVProtocolPort = Integer.parseInt(UAVPreferenceManager.GetUAVProtocolPort(mContext));
		  mVehicleName = UAVPreferenceManager.GetVehicleName(mContext);
	  }
	 
	
	 /**
	   * Override Function : Call Thread
	   */
	  public void run() {
	    	
	     int n=0;
	     while (mExit == false)
	     {
	    	 if (n==900)
	    	 {
	    		 //Logger.LogSensors();
	    		 n=0;
	    	 }
	    	 n=n+1;
	     }
	  }
	  
	/*public void NetSendSwitchInfo () throws IOException
	{
	 if ( mBusySending2 == true) return ;
		 mBusySending2 = true;
		
		 String data="";
		 
		 data +="%SWT#ACC#" + UAVState.Accelerometer_Active +"#OK#%";
		 data +="%SWT#GPS#" + UAVState.GPS_Active +"#OK#%";
		 data +="%SWT#GYR#" + UAVState.Gyro_Active +"#OK#%";
		 data +="%SWT#MAG#" + UAVState.Magnetic_Active +"#OK#%";
		 data +="%SWT#ORI#" + UAVState.Orientation_Active +"#OK#%";
		 data +="%SWT#WIFI#" + UAVState.WIFI_Active +"#OK#%";
		 
			 
	     
	     
	     try
		 {
			 
			 if (data.length()!=0)
			 {
				 DatagramPacket packet = new DatagramPacket(data.getBytes(), data.length(),
				        getBroadcastAddress(), mUAVProtocolPort);
				    
				 mSocket.send(packet);
			 }
		 
		 
		 }
		 finally 
		 {
			 mBusySending2 = false;
		 }
		
	} */
	  
	 public void NetSendInfo () throws IOException 
	 {
		 if ( mBusySending == true) return ;
		 mBusySending = true;
		
		 String data="";
		 
		 if (UAVState.Debug_Active==true)
			 data +="%DBG#" + UAVState.Debug ;  
		
		 
		 data +=  "%THR#" + Double.toString(UAVState.Throttle)
				+ "%ELV#" + Double.toString(UAVState.Elevator)
				+ "%RUD#" + Double.toString(UAVState.Rudder)
				+ "%AIL#" + Double.toString(UAVState.Aileron);
		 
			 
		 if (UAVState.Accelerometer_Active==true)
			 data +="%ACC#" + Double.toString(UAVState.Accelerometer_X) + "#" + Double.toString(UAVState.Accelerometer_Y) + "#" + Double.toString(UAVState.Accelerometer_Z);
		 else
			 data +="%";
		 
		 if (UAVState.Gyro_Active==true)
				data +="%GYR#" + Double.toString(UAVState.Gyro_X) + "#" + Double.toString(UAVState.Gyro_Y) + "#" + Double.toString(UAVState.Gyro_Z);
		 else
			 data +="%";
		 
		 if (UAVState.Magnetic_Active==true)
				data +="%MAG#" + Double.toString(UAVState.Magnetic_X) + "#" + Double.toString(UAVState.Magnetic_Y) + "#" + Double.toString(UAVState.Magnetic_Z);
		 else
			 data +="%";
		 
		 if ((UAVState.GPS_Active==true) || (UAVState.GPS_SIM_Active==true))
				data +="%GPS#" + Double.toString(UAVState.GPS_LNG) + "#" + Double.toString(UAVState.GPS_LAT) + "#" + Double.toString(UAVState.GPS_ALT) + "#" + Double.toString(UAVState.GPS_SPD);
		 else
			 data +="%";
		 
		 if ((UAVState.Orientation_Active==true) || (UAVState.Orientation_SIM_Active==true))
				data +="%ORI#" + Double.toString(UAVState.Orientation_X) + "#" + Double.toString(UAVState.Orientation_Y) + "#" + Double.toString(UAVState.Orientation_Z);
		 else
			 data +="%";
		 
		 data +="%";
		
		 try
		 {
			 
			 if (data.length()!=0)
			 {
				 DatagramPacket packet = new DatagramPacket(data.getBytes(), data.length(),
						 NetHelper.getBroadcastAddress(), mUAVProtocolPort);
				    
				 mSocket.send(packet);
			 }
		 
		 
		 }
		 finally 
		 {
			 mBusySending = false;
		 }
	 }
	 
	
	 protected void  NetSend (String data) throws IOException
	 {
		 
			 
		 try
		 {
			 
			 if (data.length()!=0)
			 {
				 DatagramPacket packet = new DatagramPacket(data.getBytes(), data.length(),
						NetHelper.getBroadcastAddress(), mUAVProtocolPort);
				    
				 mSocket.send(packet);
			 }
		 
		 
		 }
		 finally 
		 {
			 mBusySending = false;
		 }
	 }
	 
	 
		 
}
