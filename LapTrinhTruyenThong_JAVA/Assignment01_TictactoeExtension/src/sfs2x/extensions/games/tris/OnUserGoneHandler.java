package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;
import java.util.Map;

import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.core.SFSEventType;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class OnUserGoneHandler extends BaseServerEventHandler
{
	@SuppressWarnings("unchecked")
    @Override
	public void handleServerEvent(ISFSEvent event) throws SFSException
	{
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		Room gameRoom = gameExt.getGameRoom();
		
		// Get event params
		User user = (User) event.getParameter(SFSEventParam.USER);
		Integer oldPlayerId;
		
		// User disconnected
		if (event.getType() == SFSEventType.USER_DISCONNECT)
		{
			Map<Room, Integer> playerIdsByRoom = (Map<Room, Integer>) event.getParameter(SFSEventParam.PLAYER_IDS_BY_ROOM);
			oldPlayerId = playerIdsByRoom.get(gameRoom);
		}
		
		// User just left the room
		else
		{
			oldPlayerId = (Integer) event.getParameter(SFSEventParam.PLAYER_ID);
		}
		UpdateStatusForLeaveChallenge(gameExt, user);
		//trace("User out :"+ user.getName());
		// Old user was in this Room
		if (oldPlayerId != null)
		{
			// And it was a player
			if (oldPlayerId > 0)
			{
				gameExt.stopGame(true);
				
				// If 1 player is inside let's notify him that the game is now stopped
				if (gameRoom.getSize().getUserCount() > 0)
				{
					ISFSObject resObj = new SFSObject();
					resObj.putUtfString("n", user.getName());
					gameExt.send("stop", resObj, gameRoom.getUserList());
					
					for(int i=0; i<gameRoom.getUserList().size(); i++)
					{
						UpdateStatusForLeaveChallenge(gameExt, gameRoom.getUserList().get(i));
					}
					
				}
			}
		}
	}
	
	//Update status tren database, Loai bo trang thai thach dau cho 2 Player khi game Stop
	private void UpdateStatusForLeaveChallenge(TrisExtension gameExt, User theUser)
	{
		IDBManager dbManager = getParentExtension().getParentZone()
				.getDBManager();
		Connection connection = null;

		try {
			connection = dbManager.getConnection();
			
			PreparedStatement stmt = connection.prepareStatement("UPDATE account SET status= 0 WHERE id=?");
			stmt.setInt(1,(int)theUser.getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			boolean res1= stmt.execute();
		     
		     if (!res1){
		    	 trace("success update status of user: "+ theUser.getName());
				}
		     else 
		     {
		    	 trace("failed update status of user: "+theUser.getName());
		     }

//			// Build a prepared statement
//			PreparedStatement stmt1 = connection.prepareStatement("UPDATE account SET status= 0 WHERE id=?");
//			stmt1.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
//			// Execute query
//			boolean res2= stmt1.execute();
//		     
//		     if (!res2){
//		    	 trace("success update status of user: "+ gameExt.getGameRoom().getUserByPlayerId(2).getName());
//				}
//		     else 
//		     {
//		    	 trace("failed update status of user: "+gameExt.getGameRoom().getUserByPlayerId(2).getName());
//		     }

			
		} catch (SQLException e) {
			trace(e.toString());
		}
		finally
		{
			try
			{
			connection.close();
			}
			catch(SQLException e)
			{
				trace(e.toString());
			}
		}
	}
}
