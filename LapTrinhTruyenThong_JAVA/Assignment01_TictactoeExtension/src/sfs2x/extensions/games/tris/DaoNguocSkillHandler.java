package sfs2x.extensions.games.tris;

import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class DaoNguocSkillHandler extends BaseClientRequestHandler{

	@Override
	public void handleClientRequest(User user, ISFSObject params) {
		
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		
		gameExt.getGameBoard().DaoNguocBanCo();
		
		ISFSObject respObj = new SFSObject();
		respObj.putInt("t", user.getPlayerId());
		respObj.putInt("t1", user.getId());
		
		gameExt.send("daonguoc", respObj, gameExt.getGameRoom().getPlayersList());
		
		gameExt.updateTurn();
		
	}
}
