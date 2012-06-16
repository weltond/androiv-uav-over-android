package com.uav;

import java.util.ArrayList;
import java.util.List;

public final class UAVState {
	
	public static int RoutePoint_ActiveIndex;
	public static ArrayList<RoutePoint> RoutePoint;
	
	public static boolean Simulator_Active;
	public static boolean Arming;
	public static boolean Calibrated;
	
	public static boolean Accelerometer_Active;
	public static double Accelerometer_X;
	public static double Accelerometer_Y;
	public static double Accelerometer_Z;
	public static double Accelerometer_X_Calibrated;
	public static double Accelerometer_Y_Calibrated;
	public static double Accelerometer_Z_Calibrated;
	
	public static boolean Gyro_Active;
	public static double Gyro_X;
	public static double Gyro_Y;
	public static double Gyro_Z;
	public static double Gyro_X_Calibrated;
	public static double Gyro_Y_Calibrated;
	public static double Gyro_Z_Calibrated;
	
	public static boolean Magnetic_Active;
	public static double Magnetic_X;
	public static double Magnetic_Y;
	public static double Magnetic_Z;
	
	public static boolean Orientation_SIM_Active;
	public static boolean Orientation_Active;
	public static double Orientation_X;			// Pitch
	public static double Orientation_Y;			// Heading
	public static double Orientation_Z;         // Roll
	
	public static double Old_Orientation_X;			// Pitch
	public static double Old_Orientation_Y;			// Heading
	public static double Old_Orientation_Z;         // Roll
	
	public static boolean GPS_SIM_Active;
	public static boolean GPS_Active;
	public static double GPS_LNG;
	public static double GPS_LAT;
	public static double GPS_ALT;
	public static double GPS_SPD;

	public static double Old_GPS_LNG;
	public static double Old_GPS_LAT;
	public static double Old_GPS_ALT;
	public static double Old_GPS_SPD;
	
	public static double Throttle=0;
	public static double Elevator=0;
	public static double Rudder=0;
	public static double Aileron=0;
	
	public static boolean WIFI_Active;
	
}
