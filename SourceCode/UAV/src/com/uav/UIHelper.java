package com.uav;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;

public final class UIHelper {

	public static void ShowOKDialog (Context context, String message)
	{
		// prepare the alert box
        AlertDialog.Builder alertbox = new AlertDialog.Builder(context);

        // set the message to display
        alertbox.setMessage(message);
        alertbox.setNeutralButton("Ok", new DialogInterface.OnClickListener() {
        	 
            // click listener on the alert box
            public void onClick(DialogInterface arg0, int arg1) {
                                    
            }
        });
        alertbox.show();
	}
	
	
	public static void ShowYesNoDialog (Context context, String message,DialogInterface.OnClickListener OnYesClickListener, DialogInterface.OnClickListener OnNoClickListener)
	{
		 
		 	AlertDialog.Builder builder = new AlertDialog.Builder(context);
         	builder.setMessage(message);
         	builder.setCancelable(false);
            builder.setPositiveButton("Yes", OnYesClickListener);
         	builder.setNegativeButton("No", OnNoClickListener);
         	
         	AlertDialog alert = builder.create();
         	alert.show();
	}
	
	
}
