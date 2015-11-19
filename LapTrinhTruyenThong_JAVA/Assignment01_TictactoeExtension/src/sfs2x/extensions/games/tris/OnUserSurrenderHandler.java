package sfs2x.extensions.games.tris;


//import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class OnUserSurrenderHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User sender, ISFSObject params) {
		// TODO Auto-generated method stub
		trace("Receive Surrender cmd");
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		// Room gameRoom = gameExt.getGameRoom();

		ISFSObject respObj = new SFSObject();
		respObj.putInt("loserid", sender.getPlayerId());
		respObj.putUtfString("losername", sender.getName());
		
		if(sender.getPlayerId()==1)
		{
			respObj.putInt("wid", 2);
			respObj.putUtfString("wn",gameExt.getGameRoom().getUserByPlayerId(2).getName());
		}	
		else if(sender.getPlayerId()==2)
		{
			respObj.putInt("wid", 1);
			respObj.putUtfString("wn",gameExt.getGameRoom().getUserByPlayerId(1).getName());
		}

		gameExt.stopGame();

		gameExt.send("surrender", respObj, gameExt.getGameRoom().getUserList());

		gameExt.setLastGameEndResponse(new LastGameEndResponse("win", respObj));
		
		// Next turn will be given to the winning user.
		if(sender.getPlayerId()==1)
		{
			gameExt.setTurn(gameExt.getGameRoom().getUserByPlayerId(2));
		}	
		else if(sender.getPlayerId()==2)
		{
			gameExt.setTurn(gameExt.getGameRoom().getUserByPlayerId(1));
		}	
	}

}
