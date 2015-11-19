package com.duylinh2109;

import java.io.IOException;
import java.io.PipedInputStream;
import java.io.PipedOutputStream;

public class PipedEcho {
	public static void main(String[] args) {
		try {
			
			PipedOutputStream cwPipe = new PipedOutputStream();
			PipedInputStream crPipe = new PipedInputStream();

			
			// Noi dau ghi cua server voi dau doc cua client(Client -> Server)
			PipedOutputStream swPipe = new PipedOutputStream(crPipe);
			
		
			// Noi dau doc cua server vs dau ghi cua client(Server->Client)
			PipedInputStream srPipe = new PipedInputStream(cwPipe);

			// Piped Echo Server
			PipedEchoServer server = new PipedEchoServer(srPipe, swPipe);
			PipedEchoClient client = new PipedEchoClient(crPipe, cwPipe);
		} catch (IOException ie) {
			System.out.println("Pipe Echo Erro!" + ie);
		}
	}
}
