package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSArray;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetLeaderBoardHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User theUser, ISFSObject params) {
		
		TrisExtension gameExt = (TrisExtension) getParentExtension();

		IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
		Connection connection = null;
		
		trace(dbManager.getName());
		
		try {
			connection = dbManager.getConnection();

			// Build a prepared statement
			PreparedStatement stmt = connection.prepareStatement("SELECT user_name FROM account ORDER BY score DESC");
			// Execute query
			ResultSet res = stmt.executeQuery();

			// Verify that one record was found
			if (!res.first()) {
				trace("Loi");
			}

			ISFSObject respObj = new SFSObject();
			
			ISFSArray sfsa = new SFSArray();
			do
			{
				trace("Done! Send Leader Board For User is: "+ theUser.getName()+" : "+ res.getString("user_name"));
				sfsa.addUtfString(res.getString("user_name"));
			}while(res.next());
			respObj.putSFSArray("ldboard", sfsa);
			gameExt.send("getlb", respObj, theUser);
			
			
		} 
		catch (SQLException e) 
		{
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
