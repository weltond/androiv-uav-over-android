using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Protocol.Data
{
    public class VehicleInfo
    {


        #region "Attributes"

        protected string mGUID;
        protected string mName;
        protected string mType;
        protected IPAddress mIPAddress;
        protected System.DateTime mLastUpdate;
        
        #endregion 



        #region "Properties"

        public string GUID
        {
            get
            {
                return mGUID;
            }

            set
            {
                mGUID = value;
            }

        }

        public string Name
        {
            get
            {
                return mName;
            }

            set
            {
                mName = value;
            }
        }

        public string Type
        {
            get
            {
                return mType;
            }

            set
            {
                mType = value;
            }
        }

        public IPAddress IPAddress
        {
          get
            {
                return mIPAddress;
            }

            set
            {
                mIPAddress = value;
            }
      }


        public DateTime LastUpdate
        {
            get
            {
                return mLastUpdate;
            }

            set
            {
                mLastUpdate = value;
            }
        }
        #endregion 


        #region "Constructor"

        public VehicleInfo()
        {
        }

        public VehicleInfo(string GUID, string Name, string Type, IPAddress IPAddress)
        {
            this.GUID = GUID;
            this.Name = Name;
            this.Type = Type;
            this.IPAddress = IPAddress;
            this.LastUpdate = System.DateTime.Now;
        }

        #endregion 

    }
}
