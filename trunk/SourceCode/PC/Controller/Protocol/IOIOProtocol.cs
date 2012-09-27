using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Protocol.Enumerations;

namespace Protocol
{
    public interface IOIOProtocol 
    {


    

        #region "Methods"

        void GeneratePWM(int PortNum, int Frequency, double DutyCycle);

        void StorePWMCalibration(int PortNum, int Frequency, double MinDutyCycle, double MaxDutyCycle);
        #endregion
    }
}
