package com.duylinh2109;

import com.smartfoxserver.v2.extensions.SFSExtension;

public class MathChallengeExtension extends SFSExtension {

	@Override
	public void init() {
		// TODO Auto-generated method stub
		addRequestHandler(Consts.CMD_ANSWER,MathChallengeAnswerRequesHandler.class);
		
		addRequestHandler(Consts.CMD_GETQUESTION, MathChallengeGetQuesRequestHandler.class);
		
	}
}
