/*
 * Author: Mohammad Said Hefny: mohammad.hefny@gmail.com
 * 
 */
package com.uav;


import java.io.IOException;
import java.net.DatagramPacket;
import java.net.DatagramSocket;
import java.net.InetAddress;
import java.net.InetSocketAddress;
import java.net.MulticastSocket;
import java.net.NetworkInterface;
import java.net.SocketException;
import java.net.SocketTimeoutException;
import java.util.Enumeration;
import java.util.List;

import android.net.DhcpInfo;
import android.net.wifi.WifiManager;
import android.util.Log;

//import android.util.Log;

/*
 * This class tries to send a broadcast UDP packet over your wifi network
 *  Code Taken From: http://code.google.com/p/boxeeremote/source/browse/trunk/Boxee+Remote/src/com/andrewchatham/Discoverer.java?spec=svn28&r=28
 */

public class IDProtocol extends Thread {
  
  protected static String mGUID;
  protected static String mVehicleName;
  protected static android.content.ContextWrapper mContext;
  public static int mDiscoveryPort;
  private static final int TIMEOUT_MS = 500;
    
  public static boolean mExit;
  
  // TODO: Vary the challenge, or it's not much of a challenge :)
  protected WifiManager mWifi;

  interface DiscoveryReceiver {
    void addAnnouncedServers(InetAddress[] host, int port[]);
  }

  
  IDProtocol() {
	
	  mExit = false;
	  }
  
  IDProtocol(WifiManager wifi,android.content.ContextWrapper context, String GUID) {
	mExit = false;
    mWifi = wifi;
    mContext = context;
    mGUID = GUID;
  }

  
  /**
   * Read preference values for Port, GUID & Name
   */
  public void ReadPreference()
  {
	  mDiscoveryPort = Integer.parseInt(UAVPreferenceManager.GetIDProtocolPort(mContext));
	  mVehicleName = UAVPreferenceManager.GetVehicleName(mContext);
  }
  
  /**
   * Override Function : Call Thread
   */
  public void run() {
    try {
    	
		      ReadPreference();	
		     
		      DatagramSocket socket = new DatagramSocket(mDiscoveryPort);
		      socket.setBroadcast(true);
		      socket.setSoTimeout(TIMEOUT_MS);
		
		      while (mExit==false)
		      {
		    	 
		    	 sendDiscoveryRequest(socket);
		    	
		    	 try {
					this.sleep(250, 0);
				} catch (InterruptedException e) {
					// TODO Auto-generated catch block
					e.printStackTrace();
				}
		      }
		      
		      socket.close();
        } catch (IOException e) {
      Log.e("UAV", "Could not send discovery request", e);
    }
  }

  /**
   * Send a broadcast UDP packet containing Identification Packet
   * 
   * @throws IOException
   */
  private void sendDiscoveryRequest(DatagramSocket  socket) throws IOException {
   
	  String data = String.format(mGUID + "#" + mVehicleName + "#Plane");
      
	  DatagramPacket packet = new DatagramPacket(data.getBytes(), data.length(), 
			  NetHelper.getBroadcastAddress(), mDiscoveryPort);
    
	  socket.send(packet);
  
  }

  
  	    
  /**
   * Listen on socket for responses, timing out after TIMEOUT_MS
   * 
   * @param socket
   *          socket on which the announcement request was sent
   * @throws IOException
   */
  /*
  private void listenForResponses(DatagramSocket socket) throws IOException {
    byte[] buf = new byte[1024];
    try {
        DatagramPacket packet = new DatagramPacket(buf, buf.length);
        socket.receive(packet);
        String s = new String(packet.getData(), 0, packet.getLength());
        Log.d("UAV", "Received response " + s);
    } catch (SocketTimeoutException e) {
      Log.d("UAV", "Receive timed out");
    }
  }*/

  /**
   * Calculate the signature we need to send with the request. It is a string
   * containing the hex md5sum of the challenge and REMOTE_KEY.
   * 
   * @return signature string
   */
  /*
  private String getSignature(String challenge) {
    MessageDigest digest;
    byte[] md5sum = null;
    try {
      digest = java.security.MessageDigest.getInstance("MD5");
      digest.update(challenge.getBytes());
      digest.update(REMOTE_KEY.getBytes());
      md5sum = digest.digest();
    } catch (NoSuchAlgorithmException e) {
      e.printStackTrace();
    }

    StringBuffer hexString = new StringBuffer();
    for (int k = 0; k < md5sum.length; ++k) {
      String s = Integer.toHexString((int) md5sum[k] & 0xFF);
      if (s.length() == 1)
        hexString.append('0');
      hexString.append(s);
    }
    return hexString.toString();
  }
*/

}