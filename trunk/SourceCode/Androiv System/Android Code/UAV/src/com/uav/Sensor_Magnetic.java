package com.uav;

import android.hardware.Sensor;
import android.hardware.SensorEvent;
import android.hardware.SensorEventListener;

public class Sensor_Magnetic implements SensorEventListener{


	/////// Attributes
	protected double magnetic[] = new double[3];
	///////////EOF Attributes
	
	
	@Override
	public void onAccuracyChanged(Sensor sensor, int accuracy) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onSensorChanged(SensorEvent event) {
		// TODO Auto-generated method stub
		
		
		final double alpha = (double) 0.2;
	    final double beta = (double)  0.8; // alpha + beta = 1.0
  
	        /*
		    magnetic[0] = alpha * magnetic[0] + beta * event.values[0];
	        magnetic[1] = alpha * magnetic[1] + beta * event.values[1];
	        magnetic[2] = alpha * magnetic[2] + beta * event.values[2];

	        double avg = (magnetic[0] + magnetic[1] + magnetic[2]) / 3.0;
	        if (avg == 0) return;
	        
	        double tsqr = Math.sqrt(magnetic[0] * magnetic[0] + magnetic[1] * magnetic[1] + magnetic[2] * magnetic[2]);
            
	        UAVState.Magnetic_X = Math.atan2( magnetic[0], tsqr);
	        UAVState.Magnetic_Y = Math.atan2( magnetic[1], tsqr);
	        UAVState.Magnetic_Z = Math.atan2( magnetic[2], tsqr);
	        */
	        
	        UAVState.Magnetic_X = magnetic[0];
	        UAVState.Magnetic_Y = magnetic[1];
	        UAVState.Magnetic_Z = magnetic[2];
	      
	        
	        Logger.LogSensors();
	        MainActivity.NetSendInfo();
	}
}
