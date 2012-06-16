package com.uav;

import android.location.Location;

public class GPSHelper {


	protected static final double d2r = Math.PI / 180.0;
	   
	
	/**
	 * Returns distance in meters between two points.
	 * @param NewLocation
	 * @param CurrentLocation
	 * @return
	 */
	public static double CalculateDistance (Location NewLocation, Location CurrentLocation) {
	
		return CalculateDistance (NewLocation.getLongitude(),NewLocation.getLatitude(), CurrentLocation.getLongitude(), CurrentLocation.getLatitude());
	}
	
	public static double CalculateDistance (double NewLongitude, double NewLatitude, double CurrentLongitude, double CurrentLatitude) 
	{
		try
		{
		double dlong = (NewLongitude - CurrentLongitude) * d2r;
	    double dlat = (NewLatitude - CurrentLatitude) * d2r;
	    double a = Math.pow(Math.sin(dlat/2.0), 2) + Math.cos(CurrentLatitude*d2r) * Math.cos(NewLatitude*d2r) * Math.pow(Math.sin(dlong/2.0), 2);
	    double c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
	    double distance = 6367000.0 * c;			// 6367000 = Earth Diameter
	    return distance ;
	    }
		catch (Exception e)
		{
			return 1000; // large difference
		}
	}
	
	
	
	/**
	 * Returns speed in meters between two points.
	 * @param NewLocation
	 * @param CurrentLocation
	 * @return
	 */
	public static float CalculateSpeed (Location NewLocation, Location CurrentLocation) {
		try
		{
		double dlong = (NewLocation.getLongitude() - CurrentLocation.getLongitude()) * d2r;
	    double dlat = (NewLocation.getLatitude() - CurrentLocation.getLatitude()) * d2r;
	    double a = Math.pow(Math.sin(dlat/2.0), 2) + Math.cos(CurrentLocation.getLatitude()*d2r) * Math.cos(NewLocation.getLatitude()*d2r) * Math.pow(Math.sin(dlong/2.0), 2);
	    double c = 2 * Math.atan2(Math.sqrt(a), Math.sqrt(1-a));
	    double distance = 6367000 * c;
	    double TimeDelta = NewLocation.getTime() - CurrentLocation.getTime();	// in milliseconds.
	    return (float) (distance/TimeDelta) * 3600;		// [ m / s ] * [ 3600 / 1000 ] = [km / hr]
		}
		catch (Exception e)
		{
			//if (mEnableBeeps) mTG.startTone(android.media.ToneGenerator.TONE_DTMF_2,2500);
            // Log.e("TrackGPS",e.getMessage());
           //  updateNotification ("DEBUG2: " + e.getMessage());
			return 0;
		}
	}
	
	/// <summary>
    /// In general, your current heading will vary as you follow a great circle path (orthodrome); 
    /// the final heading will differ from the initial heading by varying degrees according to distance and latitude 
    /// (if you were to go from say 35°N,45°E (Baghdad) to 35°N,135°E (Osaka), you would start on a heading of 60° and end up on a heading of 120°!).
    /// This formula is for the initial bearing (sometimes referred to as forward azimuth) 
    /// which if followed in a straight line along a great-circle arc will take you from the start point to the end point:1
    /// <see cref="http://www.movable-type.co.uk/scripts/latlong.html"/>
    /// </summary>
    /// <param name="Longitude1"></param>
    /// <param name="Latitude1"></param>
    /// <param name="Longitude2"></param>
    /// <param name="Latitude2"></param>
    /// <returns>return angle in radius</returns>
    public static double CalculateBearing (double NewLongitude, double NewLatitude, double CurrentLongitude, double CurrentLatitude) 
    {
        double y = Math.sin(NewLongitude - CurrentLongitude) * Math.cos(NewLatitude);
        double x = Math.cos(CurrentLatitude) * Math.sin(NewLatitude) -
        Math.sin(CurrentLatitude) * Math.cos(NewLatitude) * Math.cos(NewLatitude - CurrentLongitude);

        return Math.atan2(y, x);
    }

}
