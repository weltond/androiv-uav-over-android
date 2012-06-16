package com.uav;

import android.os.Bundle;
import android.preference.CheckBoxPreference;
import android.preference.EditTextPreference;
import android.preference.Preference;
import android.preference.PreferenceActivity;
import android.preference.Preference.OnPreferenceChangeListener;
import android.preference.Preference.OnPreferenceClickListener;

public class SettingsActivity extends PreferenceActivity {
	
	
	
	CheckBoxPreference mchk_autostart;
	EditTextPreference mtxt_VehicleName;
	EditTextPreference mtxt_ProtocolPort;
	EditTextPreference mtxt_UAVProtocolPort;
	EditTextPreference mtxtFlightGearPort_In;
	EditTextPreference mtxtFlightGearPort_Out;
	
	private void getPreference() {
		try
		{
			mchk_autostart.setChecked(UAVPreferenceManager.GetAutoStart(getApplication()));
		}
		catch (Exception e)
		{
			return ;
		}
			mchk_autostart.setOnPreferenceClickListener(new OnPreferenceClickListener()
			{
				@Override
				public boolean onPreferenceClick(Preference arg0) {
					// TODO Auto-generated method stub
					UAVPreferenceManager.SetAutoStart(getApplication(),((CheckBoxPreference)arg0).isChecked());
					return false;
				
				}
			}
		);
		
		mtxt_VehicleName.setText(UAVPreferenceManager.GetVehicleName(getApplication()));
		mtxt_VehicleName.setOnPreferenceChangeListener(new OnPreferenceChangeListener()
			{
	        	
				@Override
				public boolean onPreferenceChange(Preference arg0, Object newValue) {
					// TODO Auto-generated method stub
							
					if (((String)newValue).length()>0)
					{
						UAVPreferenceManager.SetVehicleName(getApplication(),(String) newValue);
					
						return true;
					}
					else
					{
					
						return false;
					}
				}
			}
			);
	        
		mtxt_UAVProtocolPort.setText(UAVPreferenceManager.GetUAVProtocolPort(getApplication()));
		mtxt_UAVProtocolPort.setOnPreferenceChangeListener(new OnPreferenceChangeListener()
			{
	        	
				@Override
				public boolean onPreferenceChange(Preference arg0, Object newValue) {
					// TODO Auto-generated method stub
					try
					{
						Integer.parseInt((String)newValue);		
						UAVPreferenceManager.SetUAVProtocolPort(getApplication(),(String) newValue);
					
						return true;
					}
					catch (NumberFormatException nfe) 
					{
						return false;
					}
				}
			}
			);
	    
	    
	    mtxt_ProtocolPort.setText(UAVPreferenceManager.GetIDProtocolPort(getApplication()));
	    mtxt_ProtocolPort.setOnPreferenceChangeListener(new OnPreferenceChangeListener()
			{
	        	
				@Override
				public boolean onPreferenceChange(Preference arg0, Object newValue) {
					// TODO Auto-generated method stub
					try
					{
						Integer.parseInt((String)newValue);		
						UAVPreferenceManager.SetIDProtocolPort(getApplication(),(String) newValue);
					
						return true;
					}
					catch (NumberFormatException nfe) 
					{
						return false;
					}
				}
			}
			);
	    
	    
	    mtxtFlightGearPort_In.setText(UAVPreferenceManager.GetFlightGearPort_In(getApplication()));
		mtxtFlightGearPort_In.setOnPreferenceChangeListener(new OnPreferenceChangeListener()
				{
		        	
					@Override
					public boolean onPreferenceChange(Preference arg0, Object newValue) {
						// TODO Auto-generated method stub
						try
						{
							Integer.parseInt((String)newValue);		
							UAVPreferenceManager.SetFlightGearPort_In(getApplication(),(String) newValue);
						
							return true;
						}
						catch (NumberFormatException nfe) 
						{
							return false;
						}
					}
				}
				);
		    
		mtxtFlightGearPort_Out.setText(UAVPreferenceManager.GetFlightGearPort_Out(getApplication()));
		mtxtFlightGearPort_Out.setOnPreferenceChangeListener(new OnPreferenceChangeListener()
				{
		        	
					@Override
					public boolean onPreferenceChange(Preference arg0, Object newValue) {
						// TODO Auto-generated method stub
						try
						{
							Integer.parseInt((String)newValue);		
							UAVPreferenceManager.SetFlightGearPort_Out(getApplication(),(String) newValue);
						
							return true;
						}
						catch (NumberFormatException nfe) 
						{
							return false;
						}
					}
				}
				);
	}
	
	
	/**
	 * @see android.app.Activity#onCreate(Bundle)
	 */
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		// TODO Put your code here
		addPreferencesFromResource(R.xml.settings);
		
			}
	
	
	@Override
	protected void onStart ()
	{
		
		mchk_autostart = (CheckBoxPreference) findPreference("chk_autostart");
		mtxt_VehicleName = (EditTextPreference) findPreference("txt_VehicleName");
		mtxt_ProtocolPort = (EditTextPreference) findPreference("txt_ProtocolPort");
		mtxt_UAVProtocolPort = (EditTextPreference) findPreference("txt_UAVProtocolPort");
		mtxtFlightGearPort_In=(EditTextPreference) findPreference("txt_FlightGearPort_In");
		mtxtFlightGearPort_Out=(EditTextPreference) findPreference("txt_FlightGearPort_Out");
		super.onStart();
		getPreference();
	}
}
