package com.uav;

import java.io.IOException;
import java.net.InetAddress;
import java.net.NetworkInterface;
import java.util.Enumeration;

public class NetHelper {

	
	 /**
	   * Calculate the broadcast IP we need to send the packet along. If we send it
	   * to 255.255.255.255, it never gets sent. I guess this has something to do
	   * with the mobile network not wanting to do broadcast.
	   */
	  public static  InetAddress getBroadcastAddress() throws IOException {
		/*
		   DhcpInfo dhcp = mWifi.getDhcpInfo();
	    if (dhcp == null) {
	      Log.d("UAV", "Could not get dhcp info");
	      return null;
	    }

	    int broadcast = (dhcp.ipAddress & dhcp.netmask) | ~dhcp.netmask;
	    byte[] quads = new byte[4];
	    for (int k = 0; k < 4; k++)
	      quads[k] = (byte) ((broadcast >> k * 8) & 0xFF);
	    return InetAddress.getByAddress(quads);
	  */
	    for (Enumeration<NetworkInterface> en = NetworkInterface.getNetworkInterfaces(); en.hasMoreElements();) {
	        NetworkInterface intf = en.nextElement();
	        for (Enumeration<InetAddress> enumIpAddr = intf.getInetAddresses(); enumIpAddr.hasMoreElements();) {
	              InetAddress inetAddress = enumIpAddr.nextElement();
	             if (!inetAddress.isMulticastAddress()) {
	            	 byte[] quads = new byte[4];
	            	 quads=inetAddress.getAddress();
	            	 quads[3]=(byte) 0xff;	
	            	 return InetAddress.getByAddress(quads);

	              }
	         }
	      }
		return InetAddress.getByName("127.0.0.1");
		
	  }
}
