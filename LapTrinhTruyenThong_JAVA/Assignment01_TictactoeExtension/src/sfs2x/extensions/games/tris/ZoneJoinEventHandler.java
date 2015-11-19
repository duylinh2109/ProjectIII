package sfs2x.extensions.games.tris;

import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.util.Arrays;
import java.util.List;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.Room;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.entities.variables.SFSUserVariable;
import com.smartfoxserver.v2.entities.variables.UserVariable;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;

public class ZoneJoinEventHandler extends BaseServerEventHandler {
	@Override
	public void handleServerEvent(ISFSEvent event) throws SFSException {
		trace("ZoneJoinEventHandler handle........");
		if (this.getParentExtension().getParentZone().isCustomLogin() == false)
		{
			trace("ZoneJoinEventHandlerCustom: login flag = false");
			return;
		}
	
		User theUser = (User) event.getParameter(SFSEventParam.USER);

		// dbid is a hidden UserVariable, available only server side
		UserVariable uv_dbId = new SFSUserVariable("dbid", theUser.getSession()
				.getProperty(TrisExtension.DATABASE_ID));
		uv_dbId.setHidden(true);

		// The avatar UserVariable is a regular UserVariable
		UserVariable uv_avatar = new SFSUserVariable("avatar", "avatar_"
				+ theUser.getName() + ".jpg");

		// Set the variables
		List<UserVariable> vars = Arrays.asList(uv_dbId, uv_avatar);
		getApi().setUserVariables(theUser, vars);

		// Join the user
		Room lobby = getParentExtension().getParentZone().getRoomByName(
				"The Lobby");

		if (lobby == null)
			throw new SFSException(
					"The Lobby Room was not found! Make sure a Room called 'The Lobby' exists in the Zone to make this example work correctly.");

		// Cau lenh dua User join vao room lobby tu server
		//getApi().joinRoom(theUser, lobby);

		IDBManager dbManager = getParentExtension().getParentZone()
				.getDBManager();
		Connection connection = null;

		try {
			connection = dbManager.getConnection();

			// Build a prepared statement
			PreparedStatement stmt = connection
					.prepareStatement("SELECT score,count_win,count_lose FROM account WHERE id=?");
			stmt.setInt(
					1,
					(int) theUser.getSession().getProperty(
							TrisExtension.DATABASE_ID));
			stmt.setInt(
					1,
					(int) theUser.getSession().getProperty(
							TrisExtension.DATABASE_ID));
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
			getParentExtension().send("score", respObj, theUser);

			/*
			 * PreparedStatement stmt = connection .prepareStatement(
			 * "SELECT score,count_win,count_lose FROM account WHERE id=?");
			 * stmt.setInt( 1, (int) theUser.getSession().getProperty(
			 * TrisExtension.DATABASE_ID)); // Execute query ResultSet res =
			 * stmt.executeQuery(); trace(res.getInt("score") + " + " +
			 * res.getInt("count_win") + " + " + res.getInt("count_lose"));
			 */
		} catch (SQLException e) {
			trace(e.toString());
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
	}
}
