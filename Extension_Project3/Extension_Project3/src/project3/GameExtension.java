package project3;

import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.extensions.SFSExtension;

import eventHandler.*;

public class GameExtension extends SFSExtension {

	private User whoseTurn;
	private volatile boolean gameStarted;
	
	private final String version = "1.0.0";
	
	@Override
	public void init() {
		// TODO Auto-generated method stub
		trace("Tris game Extension for project 3 " + version);	
		
		
		 addEventHandler(SFSEventType.USER_LOGIN, LoginEventHandler.class);
		 addEventHandler(SFSEventType.USER_JOIN_ZONE, ZoneJoinEventHandler.class);
		 addEventHandler(SFSEventType.USER_DISCONNECT, OnUserGoneHandler.class);
		 addEventHandler(SFSEventType.USER_LEAVE_ROOM, OnUserGoneHandler.class);
		 addEventHandler(SFSEventType.SPECTATOR_TO_PLAYER, OnSpectatorToPlayerHandler.class);
	}
	
	@Override
	public void destroy() 
	{
		super.destroy();
	}
	
	User getWhoseTurn()
    {
	    return whoseTurn;
    }
	
	void setTurn(User user)
	{
		whoseTurn = user;
	}
	
	boolean isGameStarted()
	{
		return gameStarted;
	}
	
	
	public void startGame() {
	     gameStarted= true;
	}
	
	
	void stopGame()
	{
		stopGame(false);
	}
	
	
	void stopGame(boolean resetTurn)
	{
		gameStarted = false;
		whoseTurn = null;
	}
	
	Room getGameRoom()
	{
		return this.getParentRoom();
	}
	
}
