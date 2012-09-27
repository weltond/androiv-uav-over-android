/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;


import java.util.Date;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;
import android.location.Location;
import android.util.Log;


public class Sensor_Gyro implements SensorEventListener{

	
	/////// Attributes
	protected Date mDate;
	protected long mLastTick;
	protected double gravity[] = new double[3];
	///////////EOF Attributes
	
	public void Sensor_Gyro ()
	{
		mDate = new Date();
		mLastTick = mDate.getTime();
	}
	
	@Override
	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		// TODO Auto-generated method stub
		
	}

	
	@Override
	public void onSensorChanged(SensorEvent event) {
		// TODO Auto-generated method stub
		/*
		long lNow = mDate.getTime();
		long dt = (lNow - mLastTick) / 1000;
		 final double alpha = (double) 0.6;
	     final double beta = (double)  0.4; // alpha + beta = 1.0
	        
	        gravity[0] = alpha * (gravity[0] + event.values[0]) * dt + beta * UAVState.Accelerometer_Y;
	        gravity[1] = alpha * (gravity[1] + event.values[1]) * dt + beta * UAVState.Accelerometer_X;
	        gravity[2] = alpha * (gravity[2] + event.values[2]) * dt + beta * UAVState.Accelerometer_Z;
	        
	        //UAVState.Gyro_X = event.values[0] - gravity[0];
	        //UAVState.Gyro_Y = event.values[1] - gravity[1];
	        //UAVState.Gyro_Z = event.values[2] - gravity[2];
	        
	        mLastTick = lNow; // update last reading
	        */
	        UAVState.Gyro_X = gravity[0];
	        UAVState.Gyro_Y = gravity[1];
	        UAVState.Gyro_Z = gravity[2];
	        
	       
	        Logger.LogSensors();
	        MainActivity.NetSendInfo();
	}
	
	

}
