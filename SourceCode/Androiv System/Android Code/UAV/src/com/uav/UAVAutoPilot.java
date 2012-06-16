package com.uav;

import java.io.IOException;
import java.net.DatagramSocket;

import android.util.Log;

public class UAVAutoPilot extends Thread {

	
	public double CurrentHeading;
	public double TargetHeading;
	public double HeadingDifference;
	public double Throttle;
	public double Aileron;
	public double Elevator;
	public double Rudder;
	public double TargetRollAngle;
	public RoutePoint CurrentRoutePoint;
	protected MainActivity oMainActivity;
	protected IHWControl mIHWControl;
	
	public UAVAutoPilot (IHWControl iControl)
	{
		UAVState.RoutePoint_ActiveIndex=0;
		TargetRollAngle = 0.0;
		mIHWControl = iControl;
	}
	
	 /**
	   * Override Function : Call Thread
	   */
	  public void run() {
	    try {
	    	
		    	UAVState.Orientation_SIM_Active=true;
	    		UAVState.GPS_SIM_Active=true;
	    		
	    		while (UAVState.Simulator_Active==true)
		    	{
		    		UAVAutoPilot.sleep(100,0);
		    		this.ExecuteStep();
		    	}
		    	
		    	UAVState.Orientation_SIM_Active=false;
		    	UAVState.GPS_SIM_Active=false;
	    	} catch (Exception e)  {
	    		Log.e("UAV", "Could not send discovery request", e);
	    	}
	  }
	  
	  
	  /// <summary>
      /// 
      /// </summary>
      /// <param name="Value"></param>
      /// <param name="ZeroRange"> e.g. +/- 0.2 around zero. Send the positive value only</param>
      /// <returns></returns>
      public double ApplyCutoFFs(double Value, double ZeroRange, double ZeroValue)
      {
          if ((Value <= 0) && (Value >= -ZeroRange))
          {
              Value = -ZeroValue;
          }
          else if ((Value >= 0) && (Value <= ZeroRange))
          {
              Value = ZeroValue;
          }

          return Value;
      }

      public double ApplyUpperLimit(double Value, double MaxRange)
      {
          if ((Value > 0) && (Value > MaxRange))
          {
              Value = MaxRange;
          }
          if ((Value < 0) && (Value < -MaxRange))
          {
              Value = -MaxRange;
          }
          return Value;
      }
      
      public double RollToBearingDifference()
      {
          double MinimumRoll = 15;
          double MaximumRoll = 35;
          
          double MinimumBearing = 0.0;
          double DivideFactor = 0.01;
          double BearingDiff ;
         
          BearingDiff = (TargetHeading - CurrentHeading) ;
          if ((BearingDiff > Math.PI) || (BearingDiff < -Math.PI))
          {
              BearingDiff = CurrentHeading - (TargetHeading * -1);
          }

          BearingDiff = BearingDiff / Math.PI;

          HeadingDifference = ApplyCutoFFs(BearingDiff, MinimumBearing, 0);

          double Roll = HeadingDifference / DivideFactor;
          
          //Roll = ApplyCutoFFs(Roll, 5, 0); // zero if small roll.
          if (Roll != 0)
          {
              Roll = ApplyCutoFFs(Roll, MinimumRoll, MinimumRoll);
          }

          Roll = ApplyUpperLimit(Roll, MaximumRoll);
          TargetRollAngle = Roll;
          return Roll;
      }
      
      public void ExecuteStep()
      {
          // Calculate Bearing 
          double ZeroRoll = TargetRollAngle;
          double ZeroPitch = 0;
          if (UAVState.RoutePoint.isEmpty()==false)
          {

        	  
              this.CurrentRoutePoint = UAVState.RoutePoint.get(UAVState.RoutePoint_ActiveIndex);
              
              //////////////////////////////////////////////////////////////// Selecting Mile Stone
              double DistanceFromPlane = GPSHelper.CalculateDistance(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
             
              if ((DistanceFromPlane < 50) && (CurrentRoutePoint.DistanceFromPlane != Double.MAX_VALUE))
              {
                  UAVState.RoutePoint_ActiveIndex += 1;
                  if (UAVState.RoutePoint_ActiveIndex == UAVState.RoutePoint.size())
                  {
                	  UAVState.RoutePoint_ActiveIndex = 0;
                  }
                  this.CurrentRoutePoint.IsReached=true;
                  this.CurrentRoutePoint = UAVState.RoutePoint.get(UAVState.RoutePoint_ActiveIndex);
                  DistanceFromPlane = GPSHelper.CalculateDistance(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
              }

              this.CurrentRoutePoint.DistanceFromPlane = DistanceFromPlane;
              
              ///////////////////////////////////////////////////////////////////// High Level Flying
              CurrentHeading = GPSHelper.CalculateBearing(UAVState.Old_GPS_LNG, UAVState.Old_GPS_LAT, UAVState.GPS_LNG, UAVState.GPS_LAT);
              TargetHeading = GPSHelper.CalculateBearing(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
              //mGMapMarkerPlane.Heading = CurrentHeading;

              ZeroRoll = RollToBearingDifference();

              UAVState.Old_GPS_LAT= UAVState.GPS_LAT;
              UAVState.Old_GPS_LNG= UAVState.GPS_LNG;
          
          }


          //////////////////////////////////////////////////////////////// Low Level Flying
          // ORI_X: Pitch
          // ORI_Y: Yaw
          // ORI_Z: Roll
          
          if ((UAVState.Orientation_Z < ZeroRoll) && (Aileron < 0)) Aileron = 0.1;
          if ((UAVState.Orientation_Z > ZeroRoll) && (Aileron > 0)) Aileron = -0.1;

          if (UAVState.Orientation_Z < ZeroRoll)
          {  // LEFT

              if (UAVState.Orientation_Z < UAVState.Old_Orientation_Z)
              {   // still moving far from zero
                  // then turn sharper
                  Aileron += 0.1;
                  // but dont exceed the max.
                  if (Aileron > 0.8) Aileron = 0.9;

              }
          }
          else if (ApplyCutoFFs((UAVState.Orientation_Z - ZeroRoll), 2.0, 0) == 0.0)
          {   // Stable
              Aileron = 0;
          }
          else
          {   // RIGHT
              if (UAVState.Orientation_Z > UAVState.Old_Orientation_Z)
              {   // still moving far from 0
                  Aileron -= 0.1;
                  if (Aileron < -0.8) Aileron = -0.9;
              }
          }
          UAVState.Old_Orientation_Z= UAVState.Orientation_Z;


          if ((UAVState.Orientation_X > ZeroPitch) && (Elevator < 0)) Elevator = 0.1;
          if ((UAVState.Orientation_X < ZeroPitch) && (Elevator > 0)) Elevator = -0.1;

          if (UAVState.Orientation_X > UAVState.Old_Orientation_X)
          {  // Up

              if (UAVState.Orientation_X > ZeroPitch)
              {   // still moving far from zero
                  Elevator += 0.1;
                  if (Elevator > 0.8) Elevator = 0.9;

              }

          }
          else if (UAVState.Orientation_X == ZeroPitch)
          {   // Stable
              Elevator = 0;
          }
          else
          {   // Down
              if (UAVState.Orientation_X < UAVState.Old_Orientation_X)
              {   // still moving far from 0
                  Elevator -= 0.1;
                  if (Elevator < -0.8) Elevator = -0.9;
              }
          }


          //elevator = elevator + ((DataObject.Pitch - PitchLatest) / 90.0) * 0.30;
          UAVState.Old_Orientation_X = UAVState.Orientation_X;

          Rudder = 0; //(double.Parse(DataObject.Heading.ToString()) ) / 180.0;
          Throttle = 0.8;
          try {
        	  UAVState.Throttle = Throttle;
        	  UAVState.Rudder= Rudder;
        	  UAVState.Aileron = Aileron;
        	  UAVState.Elevator = Elevator;
			mIHWControl.sentControls(Throttle, Rudder, Aileron, Elevator);
		} catch (IOException e) {
			// TODO Auto-generated catch block
			e.printStackTrace();
		}

      }
}
