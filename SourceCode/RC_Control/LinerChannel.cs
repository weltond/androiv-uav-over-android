using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RC_Control
{
    public partial class LinerChannel : UserControl
    {

        #region "Delegates"
        public delegate void ValueChanged(object sender, LinerChannelEventArgs e);
        #endregion

        #region "Events"
        public event ValueChanged OnValueChanged;
        #endregion

        #region "Attributes"
        bool btnStatus = false;
        int nChannelNumber;
        #endregion


        #region "Properties"
        [Browsable(true), Description("Channel Number in TX"), Category("Misc")]
        public string ChannelName
        {
            set
            {
                grBox.Text = value;
            }
            get
            {
                return grBox.Text;
            }
        }

        [Browsable(true), Description("Channel Number in TX"), Category("Misc")]
        public int ChannelNumber
        {
            get
            {
                return nChannelNumber;
            }
            set
            {
                nChannelNumber = value;
                if (nChannelNumber > 8) nChannelNumber = 8;
                if (nChannelNumber < 1) nChannelNumber = 1;
                numChannel.Value = nChannelNumber;
            }
        }

        public int Value
        {
            get
            {
                return trkChannel.Value;
            }
            set
            {
                trkChannel.Value = value;
            }
        }

        #endregion


        public LinerChannel()
        {
            InitializeComponent();
        }

        private void trkChannel_Scroll(object sender, EventArgs e)
        {
            lblValue.Text = trkChannel.Value.ToString().PadLeft(4,'0');
            int Value;
            if (chkInv.Checked == true)
            {
                Value = trkChannel.Maximum - trkChannel.Value;
            }
            else
            {
                Value = trkChannel.Value;
            }

            OnValueChanged(this, new LinerChannelEventArgs(ChannelNumber ,trkChannel.Value));
        }

        private void btnOnOff_Click(object sender, EventArgs e)
        {
            if (btnStatus == false)
            {
                btnStatus = true;
                btnOnOff.Image = global::RC_Control.Properties.Resources.sort_ascending;
                trkChannel.Value = trkChannel.Maximum;
            }
            else
            {
                btnStatus = false;
                btnOnOff.Image = global::RC_Control.Properties.Resources.sort_descending;
                trkChannel.Value = trkChannel.Minimum;
            }

            int Value;
            if (chkInv.Checked == true)
            {
                Value = trkChannel.Maximum -trkChannel.Value;
            }
            else
            {
                Value = trkChannel.Value;
            }
            OnValueChanged(this, new LinerChannelEventArgs(ChannelNumber ,trkChannel.Value));
        }

        private void numChannel_ValueChanged(object sender, EventArgs e)
        {
            nChannelNumber = (int)numChannel.Value;
        }
    }
}
