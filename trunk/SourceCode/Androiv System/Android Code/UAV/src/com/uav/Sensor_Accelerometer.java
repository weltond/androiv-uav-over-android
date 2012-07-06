/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;

public class Sensor_Accelerometer  implements SensorEventListener{

	/////// Attributes
	protected double gravity[] = new double[3];
	///////////EOF Attributes
	
	
	
	public Sensor_Accelerometer()
	{
		
	}
	
	@Override
	public void onAccuracyChanged(Sensor arg0, int arg1) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		// TODO Auto-generated method stub
		
		final float alpha = (float) 0.2;
	    final float beta = (float)  0.8; // alpha + beta = 1.0

        /*
        gravity[0] = alpha * gravity[0] + beta * event.values[0];
        gravity[1] = alpha * gravity[1] + beta * event.values[1];
        gravity[2] = alpha * gravity[2] + beta * event.values[2];

        double tsqr = Math.sqrt(Math.pow(gravity[0], 2) + Math.pow(gravity[1], 2) + Math.pow(gravity[2], 2))/3.0;
        
        UAVState.Accelerometer_X = Math.atan2(gravity[0], tsqr)/tsqr;
        UAVState.Accelerometer_Y = Math.atan2(gravity[1], tsqr)/tsqr;
        UAVState.Accelerometer_Z = Math.atan2(gravity[2], tsqr)/tsqr;
        */
        UAVState.Accelerometer_X = gravity[0];
        UAVState.Accelerometer_Y = gravity[1];
        UAVState.Accelerometer_Z = gravity[2];
        
        Logger.LogSensors();
        MainActivity.NetSendInfo();     
	}

}
