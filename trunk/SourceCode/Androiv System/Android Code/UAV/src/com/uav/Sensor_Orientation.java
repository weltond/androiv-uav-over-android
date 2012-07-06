/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;


/*
 * pls read http://www.workingfromhere.com/blog/2009/03/30/orientation-sensor-tips-in-android/
 */
public class Sensor_Orientation implements SensorEventListener{

	
	/////// Attributes
	protected float gravity[] = new float[3];
	///////////EOF Attributes
	
	
	@Override
	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		// TODO Auto-generated method stub
		
		
		 final float alpha = (float) 0.8;
	       
	        
	        gravity[0] = alpha * gravity[0] + (1 - alpha) * event.values[0];
	        gravity[1] = alpha * gravity[1] + (1 - alpha) * event.values[1];
	        gravity[2] = alpha * gravity[2] + (1 - alpha) * event.values[2];

	        UAVState.Orientation_X = event.values[0]; // Pitch
	        UAVState.Orientation_Y = event.values[1]; // Yaw
	        UAVState.Orientation_Z = event.values[2]; // Roll
	        
	        Logger.LogSensors();
	        MainActivity.NetSendInfo();
	}
	
	

}
