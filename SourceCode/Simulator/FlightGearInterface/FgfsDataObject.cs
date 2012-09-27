/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/93-fgfsdataobject/
 * 2008/11/17 - geoff mclane - http://geoffair.net/fg/
 * Add ATC message, and small suggested fix for the List
 * ============================================================================ */

#region Usings
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using FgfsSharp;
#endregion

namespace FgfsSharp
{
    //TODO: Documentation
    public class FgfsDataObject : IObservable
    {
        #region Privates

        //the obeserver list
        // private List _observer;
        //private List<IObserver> _observer = new List<IObserver>();
        private List<IObserver> _observer;

        private int _speed;
        private int _altitude;
        private double _longitude;
        private double _latitude;
        private double _groudLevel;
        private int _roll;
        private int _pitch;
        private int _heading;
        private int _headingMagneticNorth;
        private string _message;
        private string _atc;
        
        

        
        #endregion

        #region Properties

        public string ATC
        {
            get { return _atc; }
            set { _atc = value; }
        }
        public string Message
        {
            get {return _message;}
            set { _message = value; }
        }
        public int Speed
        {
            get {return _speed;}
            set { _speed = value; }
        }

        public int Altitude
        {
            get { return _altitude; }
            set { _altitude = value; }
        }

        public double Latitude
        {
            get { return _latitude; }
            set { _latitude = value; }
        }

        public double Longitude
        {
            get { return _longitude; }
            set { _longitude = value; }
        }

        public double GroudLevel
        {
            get { return _groudLevel; }
            set { _groudLevel = value; }
        }

        public int Roll
        {
            get { return _roll; }
            set { _roll = value; }
        }

        public int Pitch
        {
            get { return _pitch; }
            set { _pitch = value; }
        }

        public int HeadingMagN
        {
            get { return _headingMagneticNorth; }
            set { _headingMagneticNorth = value; }
        }


        public int Heading
        {
            get { return _heading; }
            set { _heading = value; }
        }


     

        #endregion

        #region Constructor

        public FgfsDataObject()
        {
            Console.WriteLine("Constructor: DataObject");
            //_observer = new List();
            _observer = new List<IObserver>();
        }

        #endregion

        #region Interface Methods

        public void RemoveObserver(IObserver observer)
        {
            int i = _observer.IndexOf(observer);
            if (i >= 0)
            _observer.Remove(observer);
        }

        public void RegisterObserver(IObserver observer)
        {
            this._observer.Add(observer);
            Console.WriteLine("Observer {0} added", observer.ToString());
        }

        public void NotifyObservers()
        {
            int count = 0;
            // update every observer here!
            foreach (IObserver observer in this._observer)
            {
                count++;
                // Console.WriteLine("{0}: NotifyingObservers ...", count.ToString());
                observer.UpdateObserver(this);
            }
            if (count == 0 )
                Console.WriteLine("WARNING: NO Observers to NOTIFY!");

        }

        #endregion
    }
}

