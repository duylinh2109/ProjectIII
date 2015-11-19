package sfs2x.extensions.games.tris;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class ReadyHandler extends BaseClientRequestHandler
{
	@Override
	public void handleClientRequest(User user, ISFSObject params)
	{
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		
		if (user.isPlayer())
		{
			UpdateStatusForUserReadyForGame(gameExt, user);
			// Checks if two players are available and start game
			if (gameExt.getGameRoom().getSize().getUserCount() == 2)
				gameExt.startGame();
		}	
		else
		{
			gameExt.updateSpectator(user);
			
			LastGameEndResponse endResponse = gameExt.getLastGameEndResponse();
			
			// If game has ended send the outcome
			if (endResponse != null)
				send(endResponse.getCmd(), endResponse.getParams(), user);
		}
	}
	
	private void UpdateStatusForUserReadyForGame(TrisExtension gameExt, User theUser)
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
