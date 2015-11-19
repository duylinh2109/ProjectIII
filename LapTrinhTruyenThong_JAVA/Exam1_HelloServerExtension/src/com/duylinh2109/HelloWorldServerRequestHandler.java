package com.duylinh2109;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class HelloWorldServerRequestHandler extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User sender, ISFSObject param) {
		
		//Get the client parameter
		String msg= param.getUtfString("message");
		trace(msg);
		
		//Create a response object
		ISFSObject resObj= SFSObject.newInstance();
		resObj.putUtfString("message", "Hello Client 2015-Aug-03");
		
		//Send it back to client
		send("say_hello", resObj, sender);
		
	}

}
