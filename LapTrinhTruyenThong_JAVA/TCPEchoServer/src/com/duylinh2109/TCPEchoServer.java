package com.duylinh2109;

import java.io.IOException;
import java.io.InputStream;
import java.io.OutputStream;
import java.net.ServerSocket;
import java.net.Socket;


public class TCPEchoServer {
	public final static int defaultPort = 7;

	public static void main(String[] args) 
	{ 
		try 
		{ 
			ServerSocket ss = new ServerSocket(defaultPort); 
			while (true) 
			{ 
				try 
				{
					System.out.println("Waiting for client!!!!");
					Socket s = ss.accept(); 
					System.out.println("Conected!!!!!");
					OutputStream os = s.getOutputStream(); 
					InputStream is = s.getInputStream(); 
					int ch=0; 
					while(true) 
					{ 	
						ch = is.read(); 
						if(ch == -1) break; 
						System.out.println("receive character "+ (char)ch+ " From Client!");
						os.write(ch); 
					} 
					s.close(); 
				}
				catch (IOException e) 
				{
					System.err.println(" Connection Error: "+e); 
	 			}
			}
		}
		catch (IOException ie)
		{
			System.out.println("Server creation erro!! "+ ie);
		}
	}
}
