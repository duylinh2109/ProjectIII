package com.duylinh2109;

import org.matheclipse.parser.client.eval.ComplexEvaluator;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class CalculateServerRequestHandler extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User sender, ISFSObject param) {
		//Create a response object
		ISFSObject resObj= SFSObject.newInstance();
		try{
		ComplexEvaluator engineCalculate= new ComplexEvaluator();
		
		org.matheclipse.parser.client.math.Complex c= 
				engineCalculate.evaluate(param.getUtfString("expression"));
		
		//Create a response object
		resObj.putUtfString("result", ComplexEvaluator.toString(c));
		}
		catch(Exception e)
		{
			e.printStackTrace();
			resObj.putBool("erro", true);
			resObj.putUtfString("result", e.getMessage());
		}
		
		send("result", resObj, sender);
		
	}

}
