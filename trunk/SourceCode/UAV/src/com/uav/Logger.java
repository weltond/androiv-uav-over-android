/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import java.io.File;
import java.io.FileWriter;
import java.io.IOException;

import android.os.Environment;

public class Logger {

	
	private final static String SENSORS_FILENAME = "sensors.log";
	
	
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
		        mSensorsWriter.append("\r\n ACC_X,ACC_Y,ACC_Z,Gyro_X,Gyro_Y,Gyro_Z,Mag_X,Mag_Y,Mag_Z");
				
	        
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
			    				+ Double.toString(UAVState.Accelerometer_X) 	+ ", " 
			    				+ Double.toString(UAVState.Accelerometer_Y) 	+ ", "
			    				+ Double.toString(UAVState.Accelerometer_Z) 	+ ", " 
			    				+ Double.toString(UAVState.Gyro_X) 			+ ", " 
			    				+ Double.toString(UAVState.Gyro_Y) 			+ ", " 
			    				+ Double.toString(UAVState.Gyro_Z) 			+ ", " 
			    				+ Double.toString(UAVState.Magnetic_X) 		+ ", " 
			    				+ Double.toString(UAVState.Magnetic_Y) 		+ ", " 
			    				+ Double.toString(UAVState.Magnetic_Z)
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
			if (mSensorsWriter==null) return ;
			mSensorsWriter.flush();
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}
		
	}
	

}
