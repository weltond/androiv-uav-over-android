using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Data
{
    public class RoutePoint
    {
        #region "Constants"
        public const double const_MAXSPEED = 9999;
        public const double const_MINSPEED = 0;
        #endregion

        #region "Properties"

        public string Id
        { set; get; }

        public double Longitude
        { set; get; }

        public double Latitide
        { set; get; }
        /// <summary>
        /// in Meters from sea level
        /// </summary>
        public double Altitude
        { set; get; }
        /// <summary>
        /// in meter per second
        /// </summary>
        public double MaxSpeed
        { set; get; }
        /// <summary>
        /// in meter per second.
        /// </summary>
        public double MinSpeed
        { set; get; }
        /// <summary>
        /// in meter per second
        /// </summary>
        public double Speed
        { set; get; }

        #endregion

        #region "Constructors"
        public RoutePoint()
        {
            MaxSpeed = const_MAXSPEED;
            MinSpeed = const_MINSPEED;
        }

        public RoutePoint(string Id,double Lng, double Lat)
        {
            this.Id = Id;
            Longitude = Lng;
            Latitide = Lat;
            MaxSpeed = const_MAXSPEED;
            MinSpeed = const_MINSPEED;
        }

        public RoutePoint(double Lng, double Lat, double Alt, double Spd)
        {
            Longitude = Lng;
            Latitide = Lat;
            Altitude = Alt;
            Speed = Spd;
            MaxSpeed = const_MAXSPEED;
            MinSpeed = const_MINSPEED;
        }
        #endregion


        #region "Methods"
        #endregion
    }
}
