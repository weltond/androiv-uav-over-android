using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Data
{

    /// <summary>
    /// Stores Servo calibration status for each servo.
    /// </summary>
    public class ServoCalibration
    {
        #region "Properties"

        public int Channel
        { set; get; }

        public int Frequency
        { set; get; }

        public double MinDutyCycle
        { set; get; }

        public double MaxDutyCycle
        { set; get; }

        #endregion 

        #region "Constructor"

        public ServoCalibration()
        {
        }

        public ServoCalibration(int Channel, int Frequency, double MinDutyCycle, double MaxDutyCycle)
        {
            this.Channel = Channel;
            this.Frequency = Frequency;
            this.MinDutyCycle = MinDutyCycle;
            this.MaxDutyCycle = MaxDutyCycle;
        }

        #endregion
    }
}
