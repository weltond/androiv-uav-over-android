package com.uav;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStream;
import java.io.PrintWriter;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.net.ServerSocket;
import java.net.Socket;
import java.net.UnknownHostException;
import java.util.Enumeration;

import android.app.Service;
import android.content.Intent;
import android.graphics.Color;
import android.os.Binder;
import android.os.IBinder;

public class Comm_TCPServer extends Service {

	/* ------------------------------------- Attributes */
	 
	public MainActivity mMainActivity = null;
	protected Thread mThread;
	protected ServerSocket mServerSocket; 
	protected Socket mClientSocket;
	protected BufferedReader mBufferedReader;
	// This is the object that receives interactions from clients.  See
    // RemoteService for a more complete example.
    private final IBinder mBinder = new LocalBinder();

    
    
    
	 /**
     * Class for clients to access.  Because we know this service always
     * runs in the same process as its clients, we don't need to deal with
     * IPC.
     */
    public class LocalBinder extends Binder {
    	Comm_TCPServer getService() {
    		return Comm_TCPServer.this;
        }
    }
	
    public void Send(byte[] buffer)
    {
    	if (mClientSocket!=null)
    	{
    		if (mClientSocket.isConnected()==true)
    		{
    			try {
					OutputStream oOutputStream =mClientSocket.getOutputStream();
					oOutputStream.write(buffer);
				} catch (IOException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
    		}
    	}
    }
    
	/**
	 * @see android.app.Service#onBind(Intent)
	 */
	@Override
	public IBinder onBind(Intent intent) {
		// TODO Put your code here
		return mBinder;
	}
	
	@Override
	public boolean onUnbind(Intent intent) {
		// TODO Put your code here
		return  true;
	}

	/**
	 * @see android.app.Service#onCreate()
	 */
	@Override
	public void onCreate() {
		// TODO Put your code here

	}
	
	public void onStop()
	{
		int a=3;
	}
	
	/**
	 * @see android.app.Service#onStart(Intent,int)
	 */
	@Override
	public void onStart(Intent intent, int startId) {
		// TODO Put your code here
		mServerSocket =null;
		mThread = new Thread (new Runnable()
		{
			public void run ()
			{
				Listen();
			}
		});
		mThread.start();
	}
	
	@Override
	public void onDestroy()
	{
		if (mServerSocket!=null)
		{
			try {
				mServerSocket.close();
				mClientSocket.close();
			} catch (IOException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
	
	protected void Listen ()
	{
		try {
			
			mMainActivity.mtxtStation.setTextColor(Color.rgb(0,0,220));	
			
	        Boolean end = false;
	        mServerSocket = new ServerSocket(12345);
	        
	        for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements();) {
	            NetworkInterface intf = en.nextElement();
	            for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements();) {
	                InetAddress inetAddress = enumIpAddr.nextElement();
	                
	            }
	        }
	        
	        while(!end)
	        {
	                //Server is waiting for client here, if needed
	        		mClientSocket = mServerSocket.accept();
	        		mMainActivity.runOnUiThread(SIT_ON);
	        		 mBufferedReader = new BufferedReader(
	        		new InputStreamReader(mClientSocket.getInputStream()));
	        		String line = "";
	        		while ((line = mBufferedReader.readLine()) != null) 
	        		{
	        			
	        			Parse(line);	                 		
	        		}
	        		 mMainActivity.runOnUiThread(SIT_OFF);
	        }
	        
	      
	       
			} catch (Exception e) {
			        // TODO Auto-generated catch block
			        e.printStackTrace();
			        mMainActivity.runOnUiThread(SIT_OFF);
			}
		 
	}
	
	
	protected Runnable SIT_ON = new Runnable() {
		public void run() {
			mMainActivity.mtxtStation.setTextColor(Color.rgb(220,0,0));	    
		   }
		};
		
	protected Runnable SIT_OFF = new Runnable() {
			public void run() {
				mMainActivity.mtxtStation.setTextColor(Color.rgb(0,0,220));	    
			}
		};	
		
	protected Runnable NetCMD_ActiveGPS = new Runnable() {
		public void run() {
			mMainActivity.NetCMD_ActiveGPS();
			}
		};
	
	protected Runnable NetCMD_ActiveGyro = new Runnable() {
		   public void run() {
				mMainActivity.NetCMD_ActiveGyro();		     
		   			}
				};
		
    protected Runnable NetCMD_ActiveAcc = new Runnable() {
				   public void run() {
						mMainActivity.NetCMD_ActiveAcc();		     
				   }
				};
				
	protected Runnable NetCMD_ActiveMag = new Runnable() {
				   public void run() {
						mMainActivity.NetCMD_ActiveMag();		     
				   }
				};
		
	protected Runnable NetCMD_ActiveOrientation = new Runnable() {
				   public void run() {
						mMainActivity.NetCMD_ActiveOrientation();		     
				   }
				};
					
	protected Runnable NetCMD_ReleaseGPS = new Runnable() {
					public void run() {
						mMainActivity.NetCMD_ReleaseGPS();		     
				   }
				};	
					
	protected Runnable NetCMD_ReleaseGyro = new Runnable() {
				public void run() {
					mMainActivity.NetCMD_ReleaseGyro();		     
				   }
				};
	
	protected Runnable NetCMD_ReleaseAcc= new Runnable() {
				public void run() {
						mMainActivity.NetCMD_ReleaseAcc();		     
					   }
					};	
	
	protected Runnable NetCMD_ReleaseMag= new Runnable() {
				public void run() {
					mMainActivity.NetCMD_ReleaseMag();		     
				   }
				};				
					
	protected Runnable NetCMD_ReleaseOrientation = new Runnable() {
					public void run() {
						mMainActivity.NetCMD_ReleaseOrientation();		     
					   }
					};
				
	protected Runnable NetCMD_SetSimulatonOn = new Runnable() {
					public void run() {
						mMainActivity.NetCMD_ActiveSimulator();		     
						  }
					};
					
	protected Runnable NetCMD_SetSimulatonOff = new Runnable() {
					public void run() {
						mMainActivity.NetCMD_ReleaseSimulator();		     
						}
					};			
					
	protected  void Parse (String Line)
	{
		 
		String[] oCmd = Line.split("#");
		
		if (oCmd[0].compareTo("SA")==0)
		{
			Send("AHLAN".getBytes());
			
		}
		else
		if (oCmd[0].compareTo("SET_ARM")==0)
		{
			if (oCmd[1].compareTo("ON")==0)
			{
			}
			else
			if (oCmd[1].compareTo("OFF")==0)
			{
			}	
		}
		else
		if (oCmd[0].compareTo("CMD_CLB")==0)
		{ // Calibration
				
				
		}
		else
		if (oCmd[0].compareTo("SET_ROT")==0)
		{
			// Route Commands
			if (oCmd[1].compareTo("CLE")==0)
			{
				UAVState.RoutePoint.clear();
			}
			else
			if (oCmd[1].compareTo("SAV")==0)
			{
				/*
	            * "SET_ROT#SAV#0$31.365966796875$30.1341416247186$0$0$9999$0%1$31.3978958129883$30.1118698492352$0$0$9999$0%2$31.3845062255859$30.0872165636535$0$0$9999$0\r\n"
	            */
				UAVState.RoutePoint.clear();

				String[] oRoute=oCmd[2].split("%");
				String[] oPoint;
				for (int i=0;i<oRoute.length;++i)
				{
					oPoint = oRoute[i].split("&");
					RoutePoint oRoutePoint = new RoutePoint();
					oRoutePoint.Id=oPoint[0];
					oRoutePoint.Longitude=Double.parseDouble(oPoint[1]);
					oRoutePoint.Latitude=Double.parseDouble(oPoint[2]);
					oRoutePoint.Altitude=Double.parseDouble(oPoint[3]);
					oRoutePoint.Speed=Double.parseDouble(oPoint[4]);
					oRoutePoint.MaxSpeed=Double.parseDouble(oPoint[5]);
					oRoutePoint.MinSpeed=Double.parseDouble(oPoint[6]);
					UAVState.RoutePoint.add(oRoutePoint);
				}
			}
					
		}
		else
		if (oCmd[0].compareTo("SET_SWT")==0)
		{
			// switch command
			if (oCmd[1].compareTo("GPS")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{	mMainActivity.runOnUiThread(NetCMD_ActiveGPS);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_ReleaseGPS);
				}
			} else
			if (oCmd[1].compareTo("GYR")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{
					mMainActivity.runOnUiThread(NetCMD_ActiveGyro);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_ReleaseGyro);
				}
			}
			else
			if (oCmd[1].compareTo("ACC")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{
					mMainActivity.runOnUiThread(NetCMD_ActiveAcc);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_ReleaseAcc);
				}
			} else
			if (oCmd[1].compareTo("MAG")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{	mMainActivity.runOnUiThread(NetCMD_ActiveMag);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_ReleaseMag);
			 	}
			} else
			if (oCmd[1].compareTo("ORI")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{	mMainActivity.runOnUiThread(NetCMD_ActiveOrientation);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_ReleaseOrientation);
				}
			} else
			if (oCmd[1].compareTo("SIM")==0)
			{
				if (oCmd[2].compareTo("ON")==0)
				{	mMainActivity.runOnUiThread(NetCMD_SetSimulatonOn);
				}
				else
				{ // OFF
					mMainActivity.runOnUiThread(NetCMD_SetSimulatonOff);
				}
			}
		}	
	}
	
	
}
