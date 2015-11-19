package com.duylinh2109;

import java.io.IOException;
import java.io.PipedInputStream;
import java.io.PipedOutputStream;

public class PipedEchoClient extends Thread {

	PipedInputStream readPiped;
	PipedOutputStream writePiped;

	PipedEchoClient(PipedInputStream readPiped, PipedOutputStream writePiped) {
		this.readPiped = readPiped;
		this.writePiped = writePiped;
		System.out.println("Client Creation!");
		this.start();
	}

	public void run() {
		while (true) {
			try {
				int ch = System.in.read();
				writePiped.write(ch);
				ch = readPiped.read();
				System.out.print((char) ch);
			} catch (IOException e) {
				System.out.println("Client Echo Erro! " + e);
			}
		}
	}

}
