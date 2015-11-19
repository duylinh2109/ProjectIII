package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class RegisterHandle extends BaseClientRequestHandler  {

	@Override
	public void handleClientRequest(User theUser, ISFSObject params) {
		
		String userName= params.getUtfString("userName");
		String password= params.getUtfString("password");
		
		trace("Received Register command!");
		
		IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
		Connection connection = null;
		
		ISFSObject respObj = new SFSObject();
		
		try
		{
			trace(dbManager.getName());
			connection= dbManager.getConnection();
			
			 PreparedStatement stmt = connection.prepareStatement("INSERT INTO account(user_name,password) VALUES (?,?)");
		     stmt.setString(1, userName);
		     stmt.setString(2, password);
		        
		     // Execute query
		     boolean a= stmt.execute();
		     if(a==false)
		     {
		    	 trace("Succeed!");
		    	 respObj.putUtfString("message", "Succeed!");
		     }
		     else
		     {
		    	 trace("Failed!");
		     }
		}
		catch(SQLException e)
		{
			trace("Loi: " + e.toString());
			respObj.putUtfString("message","Failed! "+ e.toString());
		}
		finally
		{
			// Return connection to the DBManager connection pool
			try {
				connection.close();
			} catch (SQLException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		
		}
		trace("Send Message!");
		send("registerok", respObj, theUser);
	}

}
