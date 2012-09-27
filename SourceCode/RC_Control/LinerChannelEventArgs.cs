using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RC_Control
{
    public class LinerChannelEventArgs : EventArgs
    {

        #region "PRoperties"

        public int ChannelNumber
        {
            get;
            set;
        }

        public int ChannelValue
        {
            get;
            set;
        }

        #endregion


        #region "Constructor"

        public LinerChannelEventArgs(int Channel, int Value)
        {
            ChannelNumber = Channel;
            ChannelValue = Value;
        }

        #endregion 

    }
}
