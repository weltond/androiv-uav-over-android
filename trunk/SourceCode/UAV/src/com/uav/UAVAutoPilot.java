/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
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
		
		mIHWControl = iControl;
	}
	
	 /**
	   * Override Function : Call Thread
	   */
	  public void run() {
	    try {
	    	
	    		// enable simulation sensors so that they can be broadcasted to Ground Station as true GPS & Orientation data.
		    	UAVState.Orientation_SIM_Active=true;
	    		UAVState.GPS_SIM_Active=true;
	    		TargetRollAngle = 0.0;
	    		UAVState.RoutePoint_ActiveIndex=0;
	    		
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
	  
	  


	  /**
	   * 
	   * @param Value value to apply cutoff on.
	   * @param ZeroRange range in which the return = ZeroValue .... e.g. +/- 0.2 around zero. Send the POSITIVE value only
	   * @param ZeroValue value returned in the ZeroRange ...
	   * @return
	   */
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

      
      /**
       * 
       * @param Value value to apply upper limit on.
       * @param MaxRange range [MaxRange,-MaxRane] after which MaxRange value is returned as a result.  Send the POSITIVE value only.
       * @return
       */
      public double ApplyUpperLimit(double Value, double MaxRange)
      {
          if (Value > MaxRange)
          {
              Value = MaxRange;
          }
          if (Value < -MaxRange)
          {
              Value = -MaxRange;
          }
          return Value;
      }
      
      
      
      public double RollToBearingDifference()
      {
          
          double MinimumBearing = 0.01;
          double DivideFactor = 0.01;
          double BearingDiff ;
         
          BearingDiff = (TargetHeading - CurrentHeading) ;
          if (BearingDiff  > 180) BearingDiff = (360 - BearingDiff) * -1 ;
         
          /*if ((BearingDiff > Math.PI) || (BearingDiff < -Math.PI))
          {
              BearingDiff = CurrentHeading + TargetHeading ; // eqv to - ( TargetHeading * -1)
          }
          */
          //BearingDiff = BearingDiff / Math.PI; // scale from 0 to 1  [u need to verify]

          HeadingDifference=BearingDiff;
          //HeadingDifference = ApplyCutoFFs(BearingDiff, MinimumBearing, 0);

          //final double RollFactor = (Math.PI *2)  / (MaximumRoll * 2);
          double Roll =  HeadingDifference ; /// RollFactor ; //   / DivideFactor;
          
          //Roll = ApplyCutoFFs(Roll, 5, 0); // zero if small roll.
          if (Roll != 0)
          {
              Roll = ApplyCutoFFs(Roll, MinimumRoll, MinimumRoll);
          }

          Roll = ApplyUpperLimit(Roll, MaximumRoll); // no need for it.
          TargetRollAngle = (Roll/MaximumRoll) * 20;
          UAVState.Debug=String.valueOf(TargetHeading) + "**"+ String.valueOf(CurrentHeading) + "**"+ String.valueOf(BearingDiff) + "**" + String.valueOf(Roll);
          return Roll;
      }
      
      
      final double MinimumRoll = 3;
      final double MaximumRoll = 20;
      final double ZeroRangeRoll =2.0; // difference between current roll and target roll.
      final double MAX_Aileron =0.4;
      final double Aileron_Step =0.01;
      
      final double Elevator_Step=0.01;
      final double MAX_Elevator=0.4;
      
      public void ExecuteStep()
      {
          // Calculate Bearing 
    	  if ((UAVState.Old_Orientation_X == UAVState.Orientation_X) && (UAVState.Old_Orientation_Z == UAVState.Orientation_Z)) return ;
    	  
          double ZeroRoll = TargetRollAngle;
          double ZeroPitch = 0;
          if (UAVState.RoutePoint.isEmpty()==false)
          {

        	  // enter to route planning if there is an update in GPS location
        	  if  ((UAVState.Old_GPS_LAT!= UAVState.GPS_LAT) || (UAVState.Old_GPS_LNG!= UAVState.GPS_LNG))
        	  {
	              this.CurrentRoutePoint = UAVState.RoutePoint.get(UAVState.RoutePoint_ActiveIndex);
	              
	              //////////////////////////////////////////////////////////////// Selecting Mile Stone
	              double DistanceFromPlane = GPSHelper.CalculateDistance(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
	             
	              if ((DistanceFromPlane < 50) && (CurrentRoutePoint.DistanceFromPlane != Double.MAX_VALUE))
	              {
	            	  // mark current point as reached.
	                  this.CurrentRoutePoint.IsReached=true;
	                  
	                  UAVState.RoutePoint_ActiveIndex += 1;
	                  // if end of points then go to start point again.
	                  if (UAVState.RoutePoint_ActiveIndex == UAVState.RoutePoint.size())
	                  {
	                	  UAVState.RoutePoint_ActiveIndex = 0;
	                  }
	                  // get next route point
	                  this.CurrentRoutePoint = UAVState.RoutePoint.get(UAVState.RoutePoint_ActiveIndex);
	                  // update distance by measuring distance from the new current route point.
	                  DistanceFromPlane = GPSHelper.CalculateDistance(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
	              }
	
	              this.CurrentRoutePoint.DistanceFromPlane = DistanceFromPlane;
	              
	              ///////////////////////////////////////////////////////////////////// High Level Flying
	              CurrentHeading = GPSHelper.CalculateBearing(UAVState.Old_GPS_LNG, UAVState.Old_GPS_LAT, UAVState.GPS_LNG, UAVState.GPS_LAT);
	              TargetHeading = GPSHelper.CalculateBearing(UAVState.GPS_LNG, UAVState.GPS_LAT, CurrentRoutePoint.Longitude, CurrentRoutePoint.Latitude);
	              //mGMapMarkerPlane.Heading = CurrentHeading;
	
	              ZeroRoll = RollToBearingDifference();
        	  }
        	  
              UAVState.Old_GPS_LAT= UAVState.GPS_LAT;
              UAVState.Old_GPS_LNG= UAVState.GPS_LNG;
          
          }


          //////////////////////////////////////////////////////////////// Low Level Flying
          // ORI_X: Pitch
          // ORI_Y: Yaw
          // ORI_Z: Roll
          
          
          // if the plane is on Roll is positive while we need negative Roll or vice versa.
          // the action is to immediately switch aileron to the correct direction.
          if ((UAVState.Orientation_Z < ZeroRoll) && (Aileron < 0)) Aileron =Aileron_Step;
          if ((UAVState.Orientation_Z > ZeroRoll) && (Aileron > 0)) Aileron = -Aileron_Step;

          // 1- if the difference should be ignored then ignore
          if (ApplyCutoFFs((UAVState.Orientation_Z - ZeroRoll), ZeroRangeRoll, 0) == 0.0)
          {   // Stable
              Aileron = 0;
          }
          else if (UAVState.Orientation_Z < ZeroRoll)
          {  
        	  // 2- LEFT: if we should roll left then roll

              if (UAVState.Orientation_Z <= UAVState.Old_Orientation_Z)
              {   // still moving far from zero
                  // then turn sharper
                  Aileron += Aileron_Step;
                  // but don't exceed the max.
                  if (Aileron > MAX_Aileron) Aileron = MAX_Aileron;
 
              }
          } 
          else /*if (UAVState.Orientation_Z < ZeroRoll)*/
          {   // 3- RIGHT: if we should roll right then roll
              if (UAVState.Orientation_Z >= UAVState.Old_Orientation_Z)
              {   // still moving far from 0
                  Aileron -= Aileron_Step;
                  if (Aileron < -MAX_Aileron) Aileron = -MAX_Aileron;
              }
          }
          UAVState.Old_Orientation_Z= UAVState.Orientation_Z;


          if ((UAVState.Orientation_X > ZeroPitch) && (Elevator < 0)) Elevator = Elevator_Step;
          if ((UAVState.Orientation_X < ZeroPitch) && (Elevator > 0)) Elevator = -Elevator_Step;

          if (UAVState.Orientation_X == ZeroPitch)
          {   // Stable
              Elevator = 0;
          }
          else if (UAVState.Orientation_X > UAVState.Old_Orientation_X)
          {  // Up

              if (UAVState.Orientation_X >= ZeroPitch)
              {   // still moving far from zero
                  Elevator += Elevator_Step;
                  if (Elevator > MAX_Elevator) Elevator = MAX_Elevator;

              }

          }
          else 
          {   // Down
              if (UAVState.Orientation_X <= UAVState.Old_Orientation_X)
              {   // still moving far from 0
                  Elevator -= Elevator_Step;
                  if (Elevator < -MAX_Elevator) Elevator = -MAX_Elevator;
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
