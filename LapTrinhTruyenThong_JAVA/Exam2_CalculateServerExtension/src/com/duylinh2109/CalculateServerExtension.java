package com.duylinh2109;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class CalculateServerExtension extends SFSExtension{

	@Override
	public void init() {
		// TODO Auto-generated method stub
		
		trace("Hello! This is Calculate Server! ");
		
		addRequestHandler("result", CalculateServerRequestHandler.class);
		
	}
	
}
