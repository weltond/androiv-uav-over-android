/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import android.os.Environment;

public class GPSLogger {

	
	
	private final static String SENSORS_FILENAME = "sensorsGPS.log";
	
	
	protected static File mfpSensors;
	protected static FileWriter mSensorsWriter;
	
	public static void Init()
	{
		try {
			
				File root = new File(Environment.getExternalStorageDirectory(), "UAV");
		        if (!root.exists()) {
		            root.mkdirs();
		        } 
		        
		        mfpSensors = new File(root, SENSORS_FILENAME);
		        mfpSensors.createNewFile();
		        
		        mSensorsWriter = new FileWriter(mfpSensors,true);
		        mSensorsWriter.append("\r\nGPS_LNG,GPS_LAT,GPS_ALT,GPS_SPD");
				
	        
			} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
	}
	
	public static void LogSensors ()
	{
		  try
		    {
			    mSensorsWriter.append("\r\n" 
			    				+ Double.toString(UAVState.GPS_LNG) 		+ ", " 
			    				+ Double.toString(UAVState.GPS_LAT) 		+ ", " 
			    				+ Double.toString(UAVState.GPS_ALT)			+ ", "
			    				+ Double.toString(UAVState.GPS_SPD)
			    				);
		    }
		    catch(IOException e)
		    {
		         e.printStackTrace();
		    }
		    catch (Exception e)
		    {
		    	return ;
		    }
   }    
	
	
	public static void Close()
	{
		try {
			if (mSensorsWriter== null) return;
			mSensorsWriter.flush();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	
}
