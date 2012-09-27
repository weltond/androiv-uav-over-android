/* ============================================================================
 * circa June 28, 2007 by slink - http://linkslink.wordpress.com/94-fgfsdatahelper/
 * 2008/11/17 - geoff mclane - http://geoffair.net/fg/
 * Added check of split, and ONLY do the 'update' when there is
 * a change in one of the parameters. 
 * ============================================================================ */

#region Usings

using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Net;
using System.Threading;

#endregion

namespace FgfsSharp
{
    //TODO: Documentation
    public class FgfsDataHelper
    {
        #region Privates

        private FgfsDataObject _dataObject;
        private string m_atc;

        private int m_speed;

        private double m_latitude;
        private double m_longitude;
        private int m_altitude;
        private double m_groudLevel;

        private int m_pitch;
        private int m_roll;
        private int m_heading;

        #endregion

        #region Properties

        public FgfsDataObject DataObject
        {
            get { return _dataObject; }
            private set { _dataObject = value; }
        }

        #endregion

        #region Constructor

        public FgfsDataHelper(FgfsDataObject dataObject)
        {
            Console.WriteLine("Constructor: FgfsDataHelper");
            this.DataObject = dataObject;
        }

        #endregion

        #region Interface Methods

        public int UpdateDataObject(StreamReader streamReader)
        {

            string incoming = streamReader.ReadLine();
            // think about using a propery!
            string[] plainData = incoming.Split(new char[] { '|' });
            int leng = plainData.Length;
            int changed = 0;
            if (leng > 0)
            {

                int nv = Convert.ToInt32(plainData[0]);
                if (nv != m_speed)
                {
                    changed++;
                    m_speed = nv;
                }
                if (leng > 1)
                {
                    m_latitude = Double.Parse(plainData[1]);

                    if (leng > 2)
                    {
                        m_longitude = Double.Parse(plainData[2]);

                        if (leng > 3)
                        {
                            m_altitude = int.Parse(plainData[3]);

                            if (leng > 4)
                            {
                                m_groudLevel = Double.Parse(plainData[4]);

                                if (leng > 5)
                                {
                                    m_pitch = int.Parse(plainData[5]);

                                    if (leng > 6)
                                    {
                                        m_roll = int.Parse(plainData[6]);

                                        if (leng > 7)
                                        {
                                            m_heading = int.Parse(plainData[7]);

                                        }
                                    }

                                }
                            }
                        }
                    }
                }
            }
            changed = 1;
            if (changed > 0)
            {
                // ONLY IF SOME CHANGE
                // ===================
                Console.WriteLine("Update: {0}, split {1}",
                    incoming.ToString(),
                    leng.ToString());
                lock (DataObject)
                {
                    DataObject.Message = incoming;  // set the message line
                    DataObject.Speed = m_speed;
                    DataObject.Latitude = m_latitude;
                    DataObject.Longitude = m_longitude;
                    DataObject.Altitude = m_altitude;
                    DataObject.Pitch = m_pitch;
                    DataObject.Roll = m_roll;
                    DataObject.Heading = m_heading;
                    DataObject.ATC = m_atc;
                }
                DataObject.NotifyObservers();
            }
            Thread.Sleep(1);
            return leng;
        }

        #endregion

        #region Public Method (obsolete)

        public void PrintObject()
        {
            Console.WriteLine("Speed: {0}, Alt: {1}", DataObject.Speed, DataObject.Altitude);
        }

        #endregion
    }
}
