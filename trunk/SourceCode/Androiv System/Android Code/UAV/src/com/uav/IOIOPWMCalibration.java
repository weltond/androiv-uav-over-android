package com.uav;

public class IOIOPWMCalibration {

	public int Channel;
	public int Frequency;
	public float MinDutyCycle;
	public float MaxDutyCycle;
	
	
	public static IOIOPWMCalibration[] Create ()
	{
		IOIOPWMCalibration[] oIOIOPWMCalibration = new IOIOPWMCalibration[8];
		
		for (int i=0; i < 8; ++i)
		{
			oIOIOPWMCalibration[i] = new IOIOPWMCalibration();
		}
		return  oIOIOPWMCalibration;
	}

	public static void ReadCalibrationPreference(android.content.ContextWrapper contextWrapper, IOIOPWMCalibration[] mIOIOPWMCalibration)
	{
		for (int i=0; i < 8; ++i)
		{
			mIOIOPWMCalibration[i].Channel=i+4;
			mIOIOPWMCalibration[i].Frequency=UAVPreferenceManager.GetUAVProtocolPWMSignalForIOIOPort_Frequency(contextWrapper, i);
			mIOIOPWMCalibration[i].MinDutyCycle=UAVPreferenceManager.GetUAVProtocolPWMSignalForIOIOPort_MinDutyCycle(contextWrapper, i);
			mIOIOPWMCalibration[i].MaxDutyCycle=UAVPreferenceManager.GetUAVProtocolPWMSignalForIOIOPort_MaxDutyCycle(contextWrapper, i);
					
		}
	}
}
