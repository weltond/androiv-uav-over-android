/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;

import java.io.IOException;

public interface IHWControl {

	 void sentControls (double Throttle, double Rudder, double Aileron, double Elevator) throws IOException;
		
}
