package sfs2x.extensions.games.tris;

//import java.sql.Connection;
//import java.sql.PreparedStatement;
//import java.sql.SQLException;
//
//import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ChallengeHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User theUser, ISFSObject params) {
		// TODO Auto-generated method stub
				TrisExtension gameExt = (TrisExtension) getParentExtension();
				
				ISFSObject respObj = new SFSObject();
				if(theUser.getPlayerId()==1)
					gameExt.send("challenge", respObj, gameExt.getGameRoom().getUserByPlayerId(2));
				else
					gameExt.send("challenge", respObj, gameExt.getGameRoom().getUserByPlayerId(1));
		
	}

}
