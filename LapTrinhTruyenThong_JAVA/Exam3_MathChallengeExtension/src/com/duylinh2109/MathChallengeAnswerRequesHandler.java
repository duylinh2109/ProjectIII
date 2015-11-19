package com.duylinh2109;


import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class MathChallengeAnswerRequesHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User sender, ISFSObject param) {
		// TODO Auto-generated method stub
		
		//Create a response object
		ISFSObject resObj= SFSObject.newInstance();
		
		int id= param.getInt("id");
		int result= param.getInt("result_client");
		
		resObj.putBool("result_server", Question.GetInstance().GetQuestionList().get(id).answer== result);
		
		send(Consts.CMD_ANSWER, resObj, sender);
	}
}
