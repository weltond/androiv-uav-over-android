/**
 * 
 */
package com.uav;

import ioio.lib.api.DigitalOutput;
import ioio.lib.api.IOIO;
import ioio.lib.api.IOIOFactory;
import ioio.lib.api.PwmOutput;
import ioio.lib.api.exception.ConnectionLostException;
import ioio.lib.api.exception.IncompatibilityException;

/**
 * @author M.Hefny
 *
 */
public final class IOIOHardware {

	
	public static IOIO mIOIO;
	public static DigitalOutput mLed;
	private static PwmOutput mPWMOutput[]= new PwmOutput[5];

	
	public static  void Connect()
	{
		mIOIO = IOIOFactory.create();
		
		try {
				mIOIO.waitForConnect();
			
			} 
		catch (ConnectionLostException e) {
			// TODO Auto-generated catch block
				e.printStackTrace();
			} 
		catch (IncompatibilityException e) {
			// TODO Auto-generated catch block
				e.printStackTrace();
			}
		
		try {
				mLed = mIOIO.openDigitalOutput(IOIO.LED_PIN, false);
			} 
		catch (ConnectionLostException e) {
			// TODO Auto-generated catch block
				e.printStackTrace();
			}
	}
	
	public static  void Disconnect ()
	{
		if (UAVState.IOIO_Active==true)
		{
			mIOIO.disconnect();
			
		}
		
		mIOIO = null;
	}

	/***
	 * 
	 * @param Port IOIO port from 4 .. 11
	 * @param Frequency 
	 * @param DutyCycle from 0.0 .. 1.0
	 */
	public static void GeneratePWM (int Port, int Frequency, float DutyCycle)
	{
		
		  try {
			  	if (Frequency==0) return; // data is  not set
			  	
				if (mPWMOutput[Port]!=null)
				{
				mPWMOutput[Port].close();
				}
				mPWMOutput[Port]= mIOIO.openPwmOutput(Port, Frequency);
				/*
				 * http://trandi.wordpress.com/2011/07/03/android-ioio-rc-servo-or-esc/
				 * _servo.setDutyCycle(0.05f + _varValue * 0.05f); 
				 * which basically varies the duty cyle between 5 and 10% (depending on the position of the seekBar) 
				 * so that the HIGH period of the signal will vary between 1 and 2 milliSeconds (again corresponding 
				 * to that a RC servo/ESC or anything else, expects)
				 * 
				 */
				(mPWMOutput[Port]).setDutyCycle(0.03f + DutyCycle * 0.09f );
			} 
		  catch (Exception e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
	}
}
