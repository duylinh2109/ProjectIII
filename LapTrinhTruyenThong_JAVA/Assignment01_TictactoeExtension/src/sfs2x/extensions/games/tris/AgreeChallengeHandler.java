package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;


public class AgreeChallengeHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User theUser, ISFSObject params) {
		// TODO Auto-generated method stub
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		
		//Cap nhat status cho 2 Player thanh trang thai thach dau vao database(0 la binh thuong, 1 la thach dau)
		UpdateStatusForChallenge(gameExt, theUser);
		
		ISFSObject respObj = new SFSObject();
		
		
//		if(theUser.getPlayerId()==1)
//			gameExt.send("agree", respObj, gameExt.getGameRoom().getUserByPlayerId(2));
//		else
//			gameExt.send("agree", respObj, gameExt.getGameRoom().getUserByPlayerId(1));
		
		gameExt.send("agree", respObj, gameExt.getGameRoom().getPlayersList());
	}
	
	//Update Status tren DataBase cho 2 Player. Khi ca 2 dong y vao choi voi thach thuc
	private void UpdateStatusForChallenge(TrisExtension gameExt, User theUser)
	{
		IDBManager dbManager = getParentExtension().getParentZone()
				.getDBManager();
		Connection connection = null;

		try {
			connection = dbManager.getConnection();
			
			PreparedStatement stmt = connection.prepareStatement("UPDATE account SET status= 1 WHERE id=?");
			stmt.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(1).getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			boolean res1= stmt.execute();
		     
		     if (!res1){
		    	 trace("success update status of user: "+ gameExt.getGameRoom().getUserByPlayerId(1).getName());
				}
		     else 
		     {
		    	 trace("failed update status of user: "+gameExt.getGameRoom().getUserByPlayerId(1).getName());
		     }

			// Build a prepared statement
			PreparedStatement stmt1 = connection.prepareStatement("UPDATE account SET status= 1 WHERE id=?");
			stmt1.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			boolean res2= stmt1.execute();
		     
		     if (!res2){
		    	 trace("success update status of user: "+ gameExt.getGameRoom().getUserByPlayerId(2).getName());
				}
		     else 
		     {
		    	 trace("failed update status of user: "+gameExt.getGameRoom().getUserByPlayerId(2).getName());
		     }

			
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
