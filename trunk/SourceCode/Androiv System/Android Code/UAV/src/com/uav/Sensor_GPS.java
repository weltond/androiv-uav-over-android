package com.uav;


import android.location.GpsStatus;
import android.location.Location;
import android.location.LocationListener;
import android.location.LocationProvider;
import android.os.Bundle;

public class Sensor_GPS implements GpsStatus.Listener , LocationListener{


	protected static final double d2r = Math.PI / 180.0;
   
	protected static final int TWO_MINUTES =  2 * 60 * 1000; 		// used to define old readings
	
	protected Location mCurrentBestLocation = null; 
	protected MainActivity  mMainActivity;
	
	public Sensor_GPS (MainActivity oMainActivity)
	{
		mMainActivity = oMainActivity;
	}
	
	
	public void onGpsStatusChanged(int event) {
        switch (event) {
            case GpsStatus.GPS_EVENT_SATELLITE_STATUS:
           	 //if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, " status Changed: GPS_EVENT_SATELLITE_STATUS");
     		
                break;

            case GpsStatus.GPS_EVENT_FIRST_FIX:
           	 //if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, " status Changed: GPS_EVENT_FIRST_FIX");
     		
                break;
            case GpsStatus.GPS_EVENT_STARTED:
           	 //if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, " status Changed: GPS_EVENT_STARTED");
            	mMainActivity.mtxtBroadCastGPS.setText ("GPS Started");
                break;
            case GpsStatus.GPS_EVENT_STOPPED:
           	 //if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, " status Changed: GPS_EVENT_STOPPED");
            	mMainActivity.mtxtBroadCastGPS.setText ("GPS Stopped");
                break;
        }
    }
	
	 @Override
		public void onLocationChanged(Location loc) {
  	// Called when a new location is found by the network location provider.
  	// Update location using JSON
		
		 if (loc == null)return ;
		 
		 
		 // if no location yet known start with this.
		 if (mCurrentBestLocation == null)
		 {
			 mCurrentBestLocation= loc; 
			 return ;
		 }
		 
		 // skip it if it is not better
		 if (isBetterLocation(loc,mCurrentBestLocation) == false) return ;
		 
		 UAVState.GPS_LAT = loc.getLatitude();
		 UAVState.GPS_LNG = loc.getLongitude();
		 UAVState.GPS_ALT= loc.getAltitude();
		 UAVState.GPS_SPD= loc.getSpeed();
		 mCurrentBestLocation= loc;
		 
		 // Log Data 
		 GPSLogger.LogSensors();
		 MainActivity.NetSendInfo();
		 
		 mMainActivity.mtxtBroadCastGPS.setText ("lat:" + Double.toString(loc.getLatitude()) + ", lng:" + Double.toString(loc.getLongitude()));
		
  }

  @Override
		public void onStatusChanged(String provider, int status, Bundle extras) 
  	{
 	 
 	 switch (status)
 	 {
 	 	 case LocationProvider.AVAILABLE:
 			// if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, provider + " status Changed: AVAILABLE");
 			 break;
 		 case LocationProvider.OUT_OF_SERVICE:
 			// if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, provider + " status Changed: OUT_OF_SERVICE");
 			 break;
 		 case LocationProvider.TEMPORARILY_UNAVAILABLE:
 			// if ((mLocationService !=null) &&(mLocationService.mEnableDebug)) Logger.Log(mLocationService, provider + " status Changed: TEMPORARILY_UNAVAILABLE");
 			 break;
 		
 	 };
 	}

  @Override
		public void onProviderEnabled(String provider) {
 	/* if (mLocationService != null)
 	 {
 		if (mLocationService.mEnableDebug) Logger.Log(mLocationService, provider + " onProviderEnabled");
  		Toast.makeText( mLocationService.getApplicationContext(),"Gps Enabled",Toast.LENGTH_SHORT).show();
 	 }
 	 */
  }

  		@Override
		public void onProviderDisabled(String provider) {
 	 /*if (mLocationService != null)
 	 {
 		 if (mLocationService.mEnableDebug) Logger.Log(mLocationService, provider + " onProviderDisabled");
 		 Toast.makeText( mLocationService.getApplicationContext(), "Gps disabled .. please switch it on.", Toast.LENGTH_SHORT ).show();
 	 }
 	 */
  }

	/** Determines whether one Location reading is better than the current Location fix
	 * Logic:
	 *  if too old return FALSE.
	 * 	if too new return TRUE anyway as the current is too old.
	 *  if more accurate then return TRUE 
	 *  if newer and same or more accurate then return TRUE.
	 *  if newer and less accurate but same provider return TRUE.
	 *  ------------------------------------------------------
	 *  Time		Accuracy	Same Provider		Return
	 *  ------------------------------------------------------
	 *	Too Old			x			x				FALSE
	 *	Too New			x			x				TRUE
	 *	Older	  	  Plus			x				TRUE
	 *	Newer		  Plus			x				TRUE
	 *	Newer		  Same			x				TRUE
	 *	Newer		  Less		  TRUE				TRUE
	 *  ======================================================
	 * @param location  The new Location that you want to evaluate
	 * @param currentBestLocation  The current Location fix, to which you want to compare the new one
	*/
	protected boolean isBetterLocation(Location location, Location currentBestLocation) {
		try
		{
		location.setSpeed(0); // preset
		
	    if (currentBestLocation == null) {
	        // A new location is always better than no location
	    	//if (mEnableDebug)Logger.Log(getBaseContext(), "Accepted: first location");
	        return true;
	    }
	    
	    // Check whether the new location fix is newer or older
	    long timeDelta = location.getTime() - currentBestLocation.getTime();
	    boolean isSignificantlyNewer = timeDelta > TWO_MINUTES;
	    boolean isSignificantlyOlder = timeDelta < -TWO_MINUTES;
	    boolean isNewer = timeDelta > 0;

	    // If it's been more than two minutes since the current location, use the new location
	    // because the user has likely moved
	    if (isSignificantlyNewer) {
	    //	if (mEnableDebug)Logger.Log(getBaseContext(), "Accepted: isSignificantlyNewer");
	        return true;
	    // If the new location is more than two minutes older, it must be worse
	    } else if (isSignificantlyOlder) {
	    //	if (mEnableDebug)Logger.Log(getBaseContext(), "Rejected: isSignificantlyOlder");
	        return false;
	    }

	    // Check whether the new location fix is more or less accurate
	    int accuracyDelta = (int) (location.getAccuracy() - currentBestLocation.getAccuracy());
	    boolean isLessAccurate = accuracyDelta > 0;
	    boolean isMoreAccurate = accuracyDelta < 0;
	    boolean isSignificantlyLessAccurate = accuracyDelta > 200;
	    int distance = (int)GPSHelper.CalculateDistance (location,currentBestLocation);
	    boolean isAccuracyLargerThanDistance = accuracyDelta > distance;
	    
	    // Check if the old and new location are from the same provider
	    boolean isFromSameProvider = isSameProvider(location.getProvider(),
	            currentBestLocation.getProvider());

	    // Determine location quality using a combination of timeliness and accuracy
	    if (isMoreAccurate) {
	    	location.setSpeed(GPSHelper.CalculateSpeed (location,currentBestLocation));
	   // 	if (mEnableDebug)Logger.Log(getBaseContext(), "Accepted: isMoreAccurate");
	        return true;
	    } else if (isNewer && !isLessAccurate) {
	    	location.setSpeed(GPSHelper.CalculateSpeed (location,currentBestLocation));
	    //	if (mEnableDebug)Logger.Log(getBaseContext(), "Accepted: isNewer and not isLessAccurate");
	        return true;
	    } else if (isNewer && isAccuracyLargerThanDistance)
	    {
	    //	if (mEnableDebug)Logger.Log(getBaseContext(), "Rejected: isNewer && isAccuracyLargerThanDistance");
	    	return false;
	    }
	    else if (isNewer && !isSignificantlyLessAccurate && isFromSameProvider) {
	    	location.setSpeed(GPSHelper.CalculateSpeed (location,currentBestLocation));
	    //	if (mEnableDebug)Logger.Log(getBaseContext(), "Accepted: isNewer and LessAccurate but from same provider");
		       
		    return true;
	    }
	    
	   // if (mEnableDebug)Logger.Log(getBaseContext(), "Rejected: ???");
	    
	    return false;
		}
		catch (Exception e)
		{
			//updateNotification ("Debug3: " + e.getMessage());
			return false;
		}
	}

	/** Checks whether two providers are the same */
	protected boolean isSameProvider(String provider1, String provider2) {
	    if (provider1 == null) {
	      return provider2 == null;
	    }
	    return provider1.equals(provider2);
	}
	
	
	
}
