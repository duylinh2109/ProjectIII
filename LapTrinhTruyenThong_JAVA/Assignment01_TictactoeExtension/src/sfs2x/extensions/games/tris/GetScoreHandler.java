package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;

public class GetScoreHandler extends BaseClientRequestHandler {

	@Override
	public void handleClientRequest(User theUser, ISFSObject params) {
		// TODO Auto-generated method stub
		TrisExtension gameExt = (TrisExtension) getParentExtension();

		IDBManager dbManager = getParentExtension().getParentZone()
				.getDBManager();
		Connection connection = null;

		try {
			connection = dbManager.getConnection();

			// Build a prepared statement
			PreparedStatement stmt = connection
					.prepareStatement("SELECT score,count_win,count_lose FROM account WHERE id=?");
			 stmt.setInt(1,
			 (int)theUser.getSession().getProperty(TrisExtension.DATABASE_ID));
			stmt.setInt(1,(int)theUser.getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			ResultSet res = stmt.executeQuery();

			// Verify that one record was found
			if (!res.first()) {
				trace("Loi");
			}

			int count_win = res.getInt("count_win");
			int count_lose = res.getInt("count_lose");
			int score = res.getInt("score");

			ISFSObject respObj = new SFSObject();
			respObj.putInt("count_lose", count_lose);
			respObj.putInt("count_win", count_win);
			respObj.putInt("score", score);
			gameExt.send("getscore", respObj, theUser);
			
			trace("Done! Send Score For User Needed!"+ theUser.getName());
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
