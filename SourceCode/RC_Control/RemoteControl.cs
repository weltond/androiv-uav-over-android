using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RC_Control
{
    public partial class RemoteControl : Form
    {

        #region "Attributes"

        protected bool mEnablePPM=false;
        protected SoundPlay sndplay;
        #endregion

        public RemoteControl()
        {
            InitializeComponent();


            cmbRate.Text = (string) cmbRate.Items[0];
            cmbChannels.Text = (string)cmbChannels.Items[0];

            sndplay = new SoundPlay();

            for (int i = 0; i < 8; ++i)
            {
                sndplay.PPMchannels[i] = 100;
            }
        }


        /// <summary>
        /// Enable and Disable PMM
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnableP_Click(object sender, EventArgs e)
        {
            if (mEnablePPM == false)
            {
                cmbChannels.Enabled = false;
                cmbRate.Enabled = false;
                mEnablePPM = true;
                btnEnableP.Image = global::RC_Control.Properties.Resources.keyboard2_cordless;
                btnEnableP.Text = "On";
                sndplay.PlayPPM(this.Handle, Int32.Parse(cmbRate.Text), short.Parse(cmbChannels.Text));

            }
            else
            {
                cmbChannels.Enabled = true;
                cmbRate.Enabled = true;
                mEnablePPM = false;
                btnEnableP.Image = global::RC_Control.Properties.Resources.keyboard2;
                btnEnableP.Text = "Off";
                sndplay.StopPPM();
            }
            
        }


        /// <summary>
        /// Called when any track control changes.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void linerChannel1_OnValueChanged(object sender, LinerChannelEventArgs e)
        {
            if (mEnablePPM)
            {
                lock (sndplay.channels_lock)
                {
                    sndplay.PPMchannels[e.ChannelNumber] = e.ChannelValue;
                }
            }
        }
    }
}
