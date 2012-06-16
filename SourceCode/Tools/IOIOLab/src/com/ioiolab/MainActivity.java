package com.ioiolab;



import ioio.lib.api.DigitalOutput;
import ioio.lib.api.IOIO;
import ioio.lib.api.IOIOFactory;
import ioio.lib.api.PwmOutput;
import ioio.lib.api.exception.ConnectionLostException;
import ioio.lib.api.exception.IncompatibilityException;
import android.app.Activity;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.SeekBar;
import android.widget.TextView;
import android.widget.Toast;
import android.widget.ToggleButton;

public class MainActivity extends Activity implements SeekBar.OnSeekBarChangeListener {
	
	protected final  int DC_MAX =200;
	
	protected MainActivity mMainActivity;
	private TextView mTextValue;
	private SeekBar mSeekBar;
	private ToggleButton mtbtnSignalOn; 
	private Button mDCPlus; 
	private Button mDCMinus;
	private EditText mDCText; 
	private Button mHzPlus;
	private Button mHzMinus;
	private EditText mHzText;
	private EditText mPortNum;
	private int mDC;
	private int mHz;
	private int mPinNum;
	private DigitalOutput mLed;
	private PwmOutput mPWMOutput;
	private IOIO ioio;
	
    /** Called when the activity is first created. */
    @Override
    public void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.main);
        mMainActivity=this;
        mDC=0;
        mHz=50;
        mPinNum=5;
        mSeekBar = (SeekBar) findViewById(R.id.seekBar);
        mTextValue = (TextView) findViewById(R.id.txtValue);
        mtbtnSignalOn = (ToggleButton) findViewById(R.id.tbtnSignalOn);
        mDCText = (EditText) findViewById(R.id.edtDC);
        mHzText = (EditText) findViewById(R.id.edtHz);
        mDCPlus = (Button) findViewById(R.id.btnDCPlus);
        mHzPlus = (Button) findViewById(R.id.btnHzPlus);
        mDCMinus = (Button) findViewById(R.id.btnDCMinus);
        mHzMinus = (Button) findViewById(R.id.btnHzMinus);
        mPortNum = (EditText) findViewById (R.id.edtPortNum);
        
        mSeekBar.setMax(DC_MAX);
        
        
        mtbtnSignalOn.setOnClickListener(new View.OnClickListener()
        {
        	@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
        		
        		 
        		if (mtbtnSignalOn.isChecked()==true)
        		{
        			ioio = IOIOFactory.create();
        			try {
						ioio.waitForConnect();
						Toast.makeText(mMainActivity, "IOIO Connected", 1000);
					} catch (ConnectionLostException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (IncompatibilityException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
        			try {
        				mLed = ioio.openDigitalOutput(IOIO.LED_PIN, false);
        				mPinNum = Integer.parseInt(mPortNum.getText().toString());
        				mPWMOutput= ioio.openPwmOutput(mPinNum, mHz);
						
					} catch (ConnectionLostException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
        			mtbtnSignalOn.setText("ON");
        		}
        		else
        		{
        			try {
        				mLed.write(true);
        				mLed.close();
						ioio.waitForDisconnect();
					} catch (InterruptedException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					} catch (ConnectionLostException e) {
						// TODO Auto-generated catch block
						e.printStackTrace();
					}
        			mtbtnSignalOn.setText("OFF");
        		}
        		
        	}
        });
        
        mDCPlus.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if (mDC<99) mDC+=1;
				mDCText.setText(String.valueOf(mDC));
				UpdateServoPosition();
			}
		});
        
        mDCMinus.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if (mDC >0) mDC-=1;
				mDCText.setText(String.valueOf(mDC));
				UpdateServoPosition();
			}
		});
	
        mHzPlus.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if (mHz< 220000) mHz+=1;
				mHzText.setText(String.valueOf(mHz));
				UpdateServoPosition();
			}
		});
        
	 
 
	 	mHzMinus.setOnClickListener(new View.OnClickListener() {
			
			@Override
			public void onClick(View arg0) {
				// TODO Auto-generated method stub
				if (mHz>0) mHz-=1;
				mHzText.setText(String.valueOf(mHz));
				UpdateServoPosition();
			}
		});
	 	
	 	mHzText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
			
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				if (hasFocus==false)
				{
					String s =mHzText.getText().toString();
					try
					{
						int v1 = Integer.parseInt(s);
						mHz = v1;
							
						
						
					}
					catch (Exception e)
					{
						mHzText.setText(String.valueOf(mHz));
					}
				}
			}
		});
 
	 	mDCText.setOnFocusChangeListener(new View.OnFocusChangeListener() {
			
			@Override
			public void onFocusChange(View v, boolean hasFocus) {
				// TODO Auto-generated method stub
				if (hasFocus==false)
				{
					String s =mDCText.getText().toString();
					try
					{
						int v1 = Integer.parseInt(s);
						mDC = v1;
						UpdateServoPosition();
					}
					catch (Exception e)
					{
						mDCText.setText(String.valueOf(mDC));
					}
				}
			}
		});
	 	
	 	mSeekBar.setOnSeekBarChangeListener(this);
        
 
    }

    
    @Override
    public void onStart () 
    {
    	mPortNum.setText(String.valueOf(mPinNum));
    	mHzText.setText(String.valueOf(mHz));
    	mDCText.setText(String.valueOf(mDC));
    	
    	super.onStart();
    }
    
    
	@Override
	public void onProgressChanged(SeekBar seekBar, int progress, boolean fromTouch) {
		UpdateSeekDimention();
    }

	@Override
	public void onStartTrackingTouch(SeekBar arg0) {
		// TODO Auto-generated method stub
		
	}

	@Override
	public void onStopTrackingTouch(SeekBar arg0) {
		// TODO Auto-generated method stub
		
	}
	
	protected void UpdateSeekDimention () 
	{
		mDC = mSeekBar.getProgress();
		mDCText.setText(String.valueOf(mDC));
		this.mTextValue.setText(String.valueOf(mDC));
		
		UpdateServoPosition();
	}


	protected void UpdateServoPosition()
	{
		if (mPWMOutput!=null)
		{
			try {
				mPWMOutput.close();
				mPWMOutput= ioio.openPwmOutput(mPinNum, mHz);
				mPWMOutput.setDutyCycle(0.03f + mDC/200.0f * 0.09f );
			} catch (ConnectionLostException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
	}
		
}