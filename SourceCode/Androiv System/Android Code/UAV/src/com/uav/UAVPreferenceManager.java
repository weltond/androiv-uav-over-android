package com.uav;

import android.content.SharedPreferences;



public class UAVPreferenceManager  {


	public static final String PREFS_COUNT = "MyPrefsFile";
	
/*-------------------------------------------------Read/Write Preference*/
	
	/**
	/* Read a boolean setting 
	**/
	protected static Boolean ReadSavedPreference (android.content.ContextWrapper contextWrapper, String propertyName, Boolean defaultValue )
	 {
		 SharedPreferences settings =  contextWrapper.getSharedPreferences(PREFS_COUNT, 0);
		 return settings.getBoolean(propertyName, defaultValue);
	 }
	
	/**
	/* Read a string setting 
	**/
	protected static String ReadSavedPreference (android.content.ContextWrapper contextWrapper, String propertyName, String defaultValue)
	 {
		 SharedPreferences settings =  contextWrapper.getSharedPreferences(PREFS_COUNT, 0);
		 return settings.getString(propertyName, defaultValue);
	 }
	
	/**
	/* Write a boolean setting 
	**/
	protected static void WriteSavedPreference (android.content.ContextWrapper contextWrapper,String propertyName, Boolean boolValue)
	 {
		 SharedPreferences settings = contextWrapper.getSharedPreferences(PREFS_COUNT, 0);
		 SharedPreferences.Editor editor = settings.edit();
		 editor.putBoolean(propertyName, boolValue);
		 editor.commit();
	 }
	
	/**
	/* Write a string setting 
	**/
	protected static void WriteSavedPreference (android.content.ContextWrapper contextWrapper,String propertyName,String stringValue)
	 {
		 SharedPreferences settings = contextWrapper.getSharedPreferences(PREFS_COUNT, 0);
		 SharedPreferences.Editor editor = settings.edit();
		 editor.putString(propertyName, stringValue);
		 editor.commit();
	 }
	 
	/* Read Preference: EOF*/
	 
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	/////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
	
	public static String GetFlightGearPort_In(android.content.ContextWrapper contextWrapper)
	{
		return ReadSavedPreference(contextWrapper,"FlightGear_In","5888");
	}
	

	public static void SetFlightGearPort_In(android.content.ContextWrapper contextWrapper, String PortIn)
	{
		WriteSavedPreference(contextWrapper,"FlightGear_In",PortIn);
	}
	
	public static String GetFlightGearPort_Out(android.content.ContextWrapper contextWrapper)
	{
		return ReadSavedPreference(contextWrapper,"FlightGear_Out","5889");
	}
	
	public static void SetFlightGearPort_Out(android.content.ContextWrapper contextWrapper, String PortOut)
	{
		WriteSavedPreference(contextWrapper,"FlightGear_Out",PortOut);
	}
	
	public static Boolean GetAutoStart(android.content.ContextWrapper contextWrapper)
	{
		return UAVPreferenceManager.ReadSavedPreference(contextWrapper,"AutoStart",false);
	}
	
	
	public static void SetAutoStart(android.content.ContextWrapper contextWrapper, Boolean bEnabled)
	{
		WriteSavedPreference(contextWrapper,"AutoStart",bEnabled);
	}
	

	public static String GetVehicleName (android.content.ContextWrapper contextWrapper)
	{
		return ReadSavedPreference(contextWrapper,"VehicleName","BlackArrow");
	}
	
	public static void SetVehicleName (android.content.ContextWrapper contextWrapper,String VehicleName )
	{
		WriteSavedPreference(contextWrapper,"VehicleName",VehicleName );
	}
	 

	public static String GetIDProtocolPort (android.content.ContextWrapper contextWrapper)
	{
		return ReadSavedPreference(contextWrapper,"IDProtocolPort","45000");
	}
	
	public static void SetIDProtocolPort (android.content.ContextWrapper contextWrapper,String ProtocolPort )
	{
		WriteSavedPreference(contextWrapper,"IDProtocolPort",ProtocolPort );
	}
	 
	public static String GetUAVProtocolPort (android.content.ContextWrapper contextWrapper)
	{
		return ReadSavedPreference(contextWrapper,"UAVProtocolPort","45001");
	}
	
	public static void SetUAVProtocolPort (android.content.ContextWrapper contextWrapper,String ProtocolPort )
	{
		WriteSavedPreference(contextWrapper,"UAVProtocolPort",ProtocolPort );
	}
}
