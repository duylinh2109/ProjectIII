package com.duylinh2109;

import java.util.ArrayList;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class MathChallengeGetQuesRequestHandler extends BaseClientRequestHandler {
	
	@Override
	public void handleClientRequest(User sender, ISFSObject param) {
		// TODO Auto-generated method stub
		
		//Create a response object
		ISFSObject resObj= SFSObject.newInstance();

		ArrayList<String> listQuestion= new ArrayList<String>();
		
		for(int i=0; i< Question.GetInstance().mQuestionList.size(); i++)
		{
			String qs= Question.GetInstance().GetQuestionList().get(i).content;
			listQuestion.add(qs);
		}
		
		resObj.putUtfStringArray(Consts.Key_ListQS, listQuestion);
		
		send(Consts.CMD_GETQUESTION, resObj, sender);
	}
}
