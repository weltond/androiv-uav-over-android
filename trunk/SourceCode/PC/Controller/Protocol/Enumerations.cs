using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Protocol.Enumerations
{
    public enum Enum_PortDirection
    {

        Digital_Output,
        Digital_Input,
        Digital_InputOutput,
        Analog_Input,
        Analog_Output,
        Analog_InputOutput
    }


    public enum Enum_DigitalPortMode 
    {
        PullUp,
        PullDown
    }


    public enum Enum_PWMPortMode
    {
        Normal,
        OpenDrain
    }


    public enum Enum_ProtocolPort : int
    {
        IdentificationProtocol =45000,
        SensorsProtocol = 45001,
        ControlProtocol = 12345
    }
}
