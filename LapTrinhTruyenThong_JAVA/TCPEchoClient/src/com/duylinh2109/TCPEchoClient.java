package com.duylinh2109;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.Socket;

public class TCPEchoClient {
	public static void main(String[] args) {
		String ipServer="127.0.0.1";
		try
		{
			Socket s= new Socket(ipServer,7);  //connect to server
			InputStream is= s.getInputStream(); //get input stream of socket
			OutputStream os= s.getOutputStream(); //get output stream of socket
			
			for(int i='0'; i<='9'; i++)
			{
				os.write(i); //sent one character to server
				int ch= is.read(); //wait for read one character from server
				System.out.println("Receive "+(char)ch+ " From Server"); //print character on screen
				try {
				    Thread.sleep(2000);                 //1000 milliseconds is one second.
				} catch(InterruptedException ex) {
				    Thread.currentThread().interrupt();
				}
			}
			
			s.close();
		}
		catch(IOException ie)
		{
			System.out.println("Loi khong tao duoc socket: "+ie);
		}
	}
}
