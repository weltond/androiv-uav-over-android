using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using GMap.NET;
using BlackArrow.GMap.Map_Controls;


namespace FgfsSharp
{
    public class AutoPilot
    {
        /*
         * *Gyro sensors*
            /orientation/roll-rate-degps
            /orientation/pitch-rate-degps
            /orientation/yaw-rate-degps

            *Accelerometers*
            /accelerations/pilot/x-accel-fps_sec
            /accelerations/pilot/y-accel-fps_sec
            /accelerations/pilot/z-accel-fps_sec

            *ground truth data to be logged and compared with my own AHRS output*
            /orientation/roll-deg
            /orientation/pitch-deg
            /orientation/heading-deg
         * */
        protected bool bFirst = true;
        protected double PitchLatest = 0;
        protected double RollLatest = 0;

        protected double oldLng;
        protected double oldLat;

        protected List<GMapMarkerMileStone> mlstGMapMarkerMileStone;
        protected GMapMarkerPlane mGMapMarkerPlane;



        public int CurrentMileStoneIndex
        { get; set; }

        public List<GMapMarkerMileStone> GMapMarkerMileStones
        {
            get
            {
                return mlstGMapMarkerMileStone;
            }
        }

        public GMapMarkerPlane GMapMarkerPlane
        {
            get
            {
                return mGMapMarkerPlane;
            }
        }


        public double CurrentHeading
        { get; set; }

        public double TargetHeading
        { get; set; }

        public double HeadingDifference
        { get; set; }

        public double Throttle
        { get; set; }

        public double Aileron
        { get; set; }
        public double Elevator
        { get; set; }
        public double Rudder
        { get; set; }


        public double TargetRollAngle
        { get; set; }


        public AutoPilot(double Latitude, double Longitude, int Altitude)
        {
            CurrentMileStoneIndex = 0;
            mlstGMapMarkerMileStone = new List<GMapMarkerMileStone>();
            mGMapMarkerPlane = new GMapMarkerPlane(new PointLatLng(Latitude, Longitude), Altitude);
            TargetRollAngle = 0.0;
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
            double MaximumRoll = 45;
            
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

        public void ExecuteStep(FgfsDataObject DataObject)
        {
            // Calculate Bearing 
            double ZeroRoll = TargetRollAngle;
            double ZeroPitch = 0;
            if (mlstGMapMarkerMileStone.Count != 0)
            {

                GMapMarkerMileStone CurrentMileStone = mlstGMapMarkerMileStone[CurrentMileStoneIndex];
                CurrentMileStone.MileStoneStatus = GMapMarkerMileStone.ENUM_MileStoneStatus.Target;

                //////////////////////////////////////////////////////////////// Selecting Mile Stone
                double DistanceFromPlane = GPSMath.CalculateDistance(DataObject.Longitude, DataObject.Latitude, CurrentMileStone.Position.Lng, CurrentMileStone.Position.Lat);
                if ((DistanceFromPlane < 100) && (CurrentMileStone.DistanceFromPlane != -1.0))
                {
                    CurrentMileStoneIndex += 1;
                    if (CurrentMileStoneIndex == mlstGMapMarkerMileStone.Count)
                    {
                        CurrentMileStoneIndex = 0;
                    }
                    CurrentMileStone.MileStoneStatus = GMapMarkerMileStone.ENUM_MileStoneStatus.Reached;
                    CurrentMileStone = mlstGMapMarkerMileStone[CurrentMileStoneIndex];
                    DistanceFromPlane = GPSMath.CalculateDistance(DataObject.Longitude, DataObject.Latitude, CurrentMileStone.Position.Lng, CurrentMileStone.Position.Lat);

                }

                CurrentMileStone.DistanceFromPlane = DistanceFromPlane;



                ///////////////////////////////////////////////////////////////////// High Level Flying

                CurrentHeading = GPSMath.CalculateBearing(oldLng, oldLat, DataObject.Longitude, DataObject.Latitude);
                TargetHeading = GPSMath.CalculateBearing(DataObject.Longitude, DataObject.Latitude, CurrentMileStone.Position.Lng, CurrentMileStone.Position.Lat);//double.Parse(txtTargetHeading.Text) * (2.0 * 3.14 / 360);
                mGMapMarkerPlane.Heading = CurrentHeading;

                ZeroRoll = RollToBearingDifference();

                oldLat = DataObject.Latitude;
                oldLng = DataObject.Longitude;
            }



            //////////////////////////////////////////////////////////////// Low Level Flying

            if ((DataObject.Roll < ZeroRoll) && (Aileron < 0)) Aileron = 0.1;
            if ((DataObject.Roll > ZeroRoll) && (Aileron > 0)) Aileron = -0.1;

            if (DataObject.Roll < ZeroRoll)
            {  // LEFT

                if (DataObject.Roll < RollLatest)
                {   // still moving far from zero
                    // then turn sharper
                    Aileron += 0.1;
                    // but dont exceed the max.
                    if (Aileron > 0.8) Aileron = 0.9;

                }
            }
            else if (ApplyCutoFFs((DataObject.Roll - ZeroRoll), 2.0, 0) == 0.0)
            {   // Stable
                Aileron = 0;
            }
            else
            {   // RIGHT
                if (DataObject.Roll > RollLatest)
                {   // still moving far from 0
                    Aileron -= 0.1;
                    if (Aileron < -0.8) Aileron = -0.9;
                }
            }
            RollLatest = DataObject.Roll;


            if ((DataObject.Pitch > ZeroPitch) && (Elevator < 0)) Elevator = 0.1;
            if ((DataObject.Pitch < ZeroPitch) && (Elevator > 0)) Elevator = -0.1;

            if (DataObject.Pitch > PitchLatest)
            {  // LEFT

                if (DataObject.Pitch > ZeroPitch)
                {   // still moving far from zero
                    Elevator += 0.1;
                    if (Elevator > 0.8) Elevator = 0.9;

                }

            }
            else if (DataObject.Pitch == ZeroPitch)
            {   // Stable
                Elevator = 0;
            }
            else
            {   // RIGHT
                if (DataObject.Pitch < PitchLatest)
                {   // still moving far from 0
                    Elevator -= 0.1;
                    if (Elevator < -0.8) Elevator = -0.9;
                }
            }


            //elevator = elevator + ((DataObject.Pitch - PitchLatest) / 90.0) * 0.30;
            PitchLatest = DataObject.Pitch;

            Rudder = 0; //(double.Parse(DataObject.Heading.ToString()) ) / 180.0;
            Throttle = 0.8;

        }



    }
}
