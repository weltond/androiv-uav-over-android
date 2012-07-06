/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.SocketTimeoutException;

import android.graphics.Color;
import android.net.wifi.WifiManager;
import android.util.Log;

public class Comm_FlightGear extends Thread implements IHWControl {
	
	
	 protected WifiManager mWifi;
	 protected static android.content.ContextWrapper mContext;
	 protected MainActivity  mMainActivity;
	 public static int mFlightGearPortIn;
	 public static int mFlightGearPortOut;
	 protected DatagramSocket mSocketOut;  // out from mobile In to Flight Gear
	 protected static boolean mStarted=false;
	 protected static boolean mBusySending = false; 
	 protected static boolean mBusySending2 = false;
	 private static final int TIMEOUT_MS = 500;
	  public boolean mExit;

	 public Comm_FlightGear() {
		    
	  }
 
	 public  Comm_FlightGear(WifiManager wifi,android.content.ContextWrapper context,MainActivity oMainActivity) {
		
	   mExit = false;
	   mWifi = wifi;
	   mContext = context;
	   mMainActivity = oMainActivity;
	   
	   ReadPreference();	
	  
	   try {
		      mSocketOut = new DatagramSocket(mFlightGearPortIn);
		      mSocketOut.setBroadcast(true);
		      mSocketOut.setSoTimeout(TIMEOUT_MS);
			  mStarted = true;
	   		} catch (IOException e) {
	   		Log.e("UAV", "Could not send discovery request", e);
	   		}
	 }

	 public static boolean IsStarted()
	 {
		 return mStarted;
	 }
	 
	 public void CloseSocketOut()
	 {
		 mSocketOut.close();
	 }
	
	 /**
	  * Read preference values for Port, GUID & Name
	  */
	  public void ReadPreference()
	  {
		  mFlightGearPortIn = Integer.parseInt(UAVPreferenceManager.GetFlightGearPort_In(mContext));
		  mFlightGearPortOut = Integer.parseInt(UAVPreferenceManager.GetFlightGearPort_Out(mContext));
	  }
	  
	  
	  
	  /**
	   * Override Function : Call Thread
	   */
	  public void run() {
	    try {
	    	
		      	 DatagramSocket socket = new DatagramSocket(mFlightGearPortOut);
		      	 while (mExit==false)
			     {
			    	 listenForInPort(socket);
			     }
		       
		         mSocketOut.close();
	    	} 
	    catch (IOException e) 
	    	{
	    		Log.e("UAV", "Could not send discovery request", e);
	    	}
	    	
	    	mMainActivity.runOnUiThread(SIM_OFF);
	    
	    	
	  }
	  
	  
	  private void listenForInPort(DatagramSocket socket) throws IOException {
		    byte[] buf = new byte[1024];
		    try {
		        DatagramPacket packet = new DatagramPacket(buf, buf.length, 
		  			  NetHelper.getBroadcastAddressHost(), mFlightGearPortOut);
		        socket.receive(packet);
		        String FlightGearOutput = new String(packet.getData(), 0, packet.getLength());
		        mMainActivity.runOnUiThread(SIM_ON);	
			       
		        String[] fgOutputs = FlightGearOutput.split("#");
		        UAVState.GPS_SPD= Double.parseDouble(fgOutputs[0]);
		        UAVState.GPS_LAT= Double.parseDouble(fgOutputs[1]);
		        UAVState.GPS_LNG= Double.parseDouble(fgOutputs[2]);
		        UAVState.GPS_ALT= Double.parseDouble(fgOutputs[3]);
		        //UAVState.G= Double.parseDouble(fgOutputs[4]); // Ground Level
		        
		        // pitch: X
		        // roll: Z
		        // heading: Y
		        UAVState.Orientation_X= Double.parseDouble(fgOutputs[5]);
		        UAVState.Orientation_Z= Double.parseDouble(fgOutputs[6]);
		        UAVState.Orientation_Y= Double.parseDouble(fgOutputs[7]);
		        
		        mMainActivity.NetSendInfo();
		    } catch (Exception e) {
		      Log.d("UAV", "Error in Socket or Parsing Data");
		    }
	}
	  
	  public void sentControls (double Throttle, double Rudder, double Aileron, double Elevator) throws IOException 
		 {
			 if ( mBusySending == true) return ;
			 mBusySending = true;
			
			 try
			 {
				   String data =Double.toString(Aileron) + "|" + Double.toString(Elevator) + "|" + Double.toString(Rudder) + "|" + Double.toString(Throttle) + "|\r\n";

				   DatagramPacket packet = new DatagramPacket(data.getBytes(), data.length(),
							 NetHelper.getBroadcastAddress(), mFlightGearPortIn);
					    
				   mSocketOut.send(packet);
			 }
			 finally 
			 {
				 mBusySending = false;
			 }
		 }
	
	  protected Runnable SIM_ON = new Runnable() {
			public void run() {
				mMainActivity.mtxtSimulation.setTextColor(Color.rgb(220,0,0));	    
			   }
			};
			
		protected Runnable SIM_OFF = new Runnable() {
				public void run() {
					mMainActivity.mtxtSimulation.setTextColor(Color.rgb(0,0,220));	    
				}
			};	
			
}
