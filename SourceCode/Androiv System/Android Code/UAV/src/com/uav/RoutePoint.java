/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

public class RoutePoint {

	/////// Attributes
	public String Id;
	public double Longitude;
	public double Latitude;
	public double Altitude;
	public double Speed;
	public double MaxSpeed;
	public double MinSpeed;
	public double DistanceFromPlane=Double.MAX_VALUE; /// initial value
	public boolean IsReached = false;
	///////////EOF Attributes
	
}
