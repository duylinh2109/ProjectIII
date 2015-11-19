package sfs2x.extensions.games.tris;
import java.sql.Connection;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;

import com.smartfoxserver.v2.annotations.Instantiation;
import com.smartfoxserver.v2.annotations.Instantiation.InstantiationMode;
import com.smartfoxserver.v2.db.IDBManager;
import com.smartfoxserver.v2.entities.User;
import com.smartfoxserver.v2.entities.data.ISFSObject;
import com.smartfoxserver.v2.entities.data.SFSObject;
import com.smartfoxserver.v2.exceptions.SFSRuntimeException;
import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
import com.smartfoxserver.v2.extensions.ExtensionLogLevel;

@Instantiation(InstantiationMode.SINGLE_INSTANCE)
public class MoveHandler extends BaseClientRequestHandler
{
	private static final String CMD_WIN = "win";
	private static final String CMD_TIE = "tie";
	private static final String CMD_MOVE = "move";
	
	@Override
	public void handleClientRequest(User user, ISFSObject params)
	{
		// Check params
		if (!params.containsKey("x") || !params.containsKey("y"))
			throw new SFSRuntimeException("Invalid request, one mandatory param is missing. Required 'x' and 'y'");
		
		TrisExtension gameExt = (TrisExtension) getParentExtension();
		TrisGameBoard board = gameExt.getGameBoard();
		
		int moveX = params.getInt("x");
		int	moveY = params.getInt("y");
		
		gameExt.trace(String.format("Handling move from player %s. (%s, %s) = %s ", user.getPlayerId(), moveX, moveY, board.getTileAt(moveX, moveY)));
		
		if (gameExt.isGameStarted())
		{
			if (gameExt.getWhoseTurn() == user)
			{
				if (board.getTileAt(moveX, moveY) == Tile.EMPTY)
				{
					// Set game board tile
					board.setTileAt(moveX, moveY, user.getPlayerId() == 1 ? Tile.GREEN : Tile.RED);
					
					// Send response
					ISFSObject respObj = new SFSObject();
					respObj.putInt("x", moveX);
					respObj.putInt("y", moveY);
					respObj.putInt("t", user.getPlayerId());
					respObj.putInt("t1", user.getId());
										
					send(CMD_MOVE, respObj, gameExt.getGameRoom().getUserList());
					
					// Increse move count and check game status					
					gameExt.increaseMoveCount();
					
					// Switch turn
					gameExt.updateTurn();
					
					// Check if game is over
					checkBoardState(gameExt, user);
				}
			}
			
			// Wrong turn
			else
				gameExt.trace(ExtensionLogLevel.WARN, "Wrong turn error. It was expcted: " + gameExt.getWhoseTurn() + ", received from: " + user);
		}
		else
			gameExt.trace(ExtensionLogLevel.WARN, "Wrong turn error. It was expcted: " + gameExt.getWhoseTurn() + ", received from: " + user);
		
	}
	
	private void checkBoardState(TrisExtension gameExt, User user)
	{
		GameState state = gameExt.getGameBoard().getGameStatus(gameExt.getMoveCount());
		
		if (state == GameState.END_WITH_WINNER)
		{
			int winnerId = gameExt.getGameBoard().getWinner();
			
			gameExt.trace("Winner found: ", winnerId);
			
			// Stop game
			gameExt.stopGame();
			
			//Update Score on DataBase When WinGame
			UpdateDataBase(gameExt, user);
			
			//Update Status on DataBase
			UpdateStatusForLeaveChallenge(gameExt);
			
			// Send update
			ISFSObject respObj = new SFSObject(); 
			respObj.putInt("w", winnerId);
			respObj.putUtfString("wn", user.getName());
			gameExt.send(CMD_WIN, respObj, gameExt.getGameRoom().getUserList());
			
			// Set the last game ending for spectators joining after the end and before a new game starts
			gameExt.setLastGameEndResponse(new LastGameEndResponse(CMD_WIN, respObj));
			
			// Next turn will be given to the winning user.
			gameExt.setTurn(gameExt.getGameRoom().getUserByPlayerId(winnerId));
		}
		
		else if (state == GameState.END_WITH_TIE)
		{
			gameExt.trace("TIE!");
			
			// Stop game
			gameExt.stopGame();
			
			//Update Status on DataBase
			UpdateStatusForLeaveChallenge(gameExt);
			
			// Send update
			ISFSObject respObj = new SFSObject();
			gameExt.send(CMD_TIE, respObj, gameExt.getGameRoom().getUserList());
			
			// Set the last game ending for spectators joining after the end and before a new game starts
			gameExt.setLastGameEndResponse(new LastGameEndResponse(CMD_TIE, respObj));
		}
	}
	
	private void UpdateDataBase(TrisExtension gameExt, User user)
	{
		//Update Score for Winner and Loser
		IDBManager dbManager = getParentExtension().getParentZone().getDBManager();
		Connection connection = null;
		try
		{
			connection= dbManager.getConnection();
			
			PreparedStatement stmtgetstatus1 = connection
					.prepareStatement("SELECT status FROM account WHERE id=?");
			stmtgetstatus1.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(1).getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			ResultSet resStatus1 = stmtgetstatus1.executeQuery();

			// Verify that one record was found
			if (!resStatus1.first()) {
				trace("Loi");
			}
			
			PreparedStatement stmtgetstatus2 = connection
					.prepareStatement("SELECT status FROM account WHERE id=?");
			stmtgetstatus2.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
			// Execute query
			ResultSet resStatus2 = stmtgetstatus2.executeQuery();

			// Verify that one record was found
			if (!resStatus2.first()) {
				trace("Loi");
			}
			
			int status1= resStatus1.getInt("status");
			int status2= resStatus2.getInt("status");
			
			boolean isChanllenge= ((status1==1) && (status2==1))?true:false;
			if(isChanllenge)
			{
				trace("Is Challenge");
			}
			if (user.getPlayerId()==1) {
				
				trace(gameExt.getGameRoom().getUserByPlayerId(2).getName());
				
				PreparedStatement stmt = connection
						.prepareStatement("UPDATE account SET score= score+? WHERE id=?");
				if(isChanllenge)
					stmt.setInt(1, 20);
				else
					stmt.setInt(1, 10);
				stmt.setInt(2,(int) user.getSession().getProperty(TrisExtension.DATABASE_ID));
				
				boolean res= stmt.execute();
				 if (!res){
			    	 trace("success update score winer!");
					}
			     else 
			     {
			    	 trace("failed update score winer!");
			     }

				PreparedStatement stmt1 = connection.prepareStatement("UPDATE account SET score= score-? WHERE id=?");
				if(isChanllenge)
					stmt1.setInt(1, 20);
				else
					stmt1.setInt(1, 10);
				stmt1.setInt(2,(int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
				
				 boolean res1= stmt1.execute();
			     
			     if (!res1){
			    	 trace("success update score loser!");
					}
			     else 
			     {
			    	 trace("failed update score loser!");
			     }
			     
			     PreparedStatement stmt2= connection.prepareStatement("UPDATE account SET count_win= count_win+? WHERE id=?");
			     stmt2.setInt(1, 1);
			     stmt2.setInt(2, (int)user.getSession().getProperty(TrisExtension.DATABASE_ID));
			     
			     boolean res2= stmt2.execute();
			     
			     if(!res2)
			     {
			    	 trace("success update count_win winner");
			     }
			     
			     PreparedStatement stmt3= connection.prepareStatement("UPDATE account SET count_lose= count_lose+? WHERE id=?");
			     stmt3.setInt(1, 1);
			     stmt3.setInt(2, (int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
			     
			     boolean res3= stmt3.execute();
			     
			     if(!res3)
			     {
			    	 trace("success update count_lose loser!");
			     }
			}
			
			else if(user.getPlayerId()==2)
			{
				trace(gameExt.getGameRoom().getUserByPlayerId(1).getName());
				
				
				PreparedStatement stmt = connection
						.prepareStatement("UPDATE account SET score= score+? WHERE id=?");
				if(isChanllenge)
					stmt.setInt(1, 20);
				else
					stmt.setInt(1, 10);
				stmt.setInt(2,(int) user.getSession().getProperty(TrisExtension.DATABASE_ID));
				
				boolean res= stmt.execute();
				
				 if (!res){
			    	 trace("success update score winer!");
					}
			     else 
			     {
			    	 trace("failed update score winer!");
			     }

				PreparedStatement stmt1 = connection
						.prepareStatement("UPDATE account SET score= score-? WHERE id=?");
				if(isChanllenge)
					stmt1.setInt(1, 20);
				else
					stmt1.setInt(1, 10);
				stmt1.setInt(2,(int) gameExt.getGameRoom().getUserByPlayerId(1).getSession().getProperty(TrisExtension.DATABASE_ID));
				
				 
				 boolean res1= stmt1.execute();
			     
				 if (!res1){
			    	 trace("success update score loser!");
					}
			     else 
			     {
			    	 trace("failed update score loser!");
			     }
				 
				 PreparedStatement stmt2= connection.prepareStatement("UPDATE account SET count_win= count_win+? WHERE id=?");
			     stmt2.setInt(1, 1);
			     stmt2.setInt(2, (int)user.getSession().getProperty(TrisExtension.DATABASE_ID));
			     
			     boolean res2= stmt2.execute();
			     
			     if(!res2)
			     {
			    	 trace("success update count_win winner");
			     }
			     
			     PreparedStatement stmt3= connection.prepareStatement("UPDATE account SET count_lose= count_lose+? WHERE id=?");
			     stmt3.setInt(1, 1);
			     stmt3.setInt(2, (int)gameExt.getGameRoom().getUserByPlayerId(1).getSession().getProperty(TrisExtension.DATABASE_ID));
			     
			     boolean res3= stmt3.execute();
			     
			     if(!res3)
			     {
			    	 trace("success update count_lose loser!");
			     }
			}
		}
		catch(SQLException e)
		{
			trace("Loi"+ e.toString());
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
	
	private void UpdateStatusForLeaveChallenge(TrisExtension gameExt)
	{
		IDBManager dbManager = getParentExtension().getParentZone()
				.getDBManager();
		Connection connection = null;

		try {
			connection = dbManager.getConnection();
			
			PreparedStatement stmt = connection.prepareStatement("UPDATE account SET status= 0 WHERE id=?");
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
		     
		     
		     PreparedStatement stmt1 = connection.prepareStatement("UPDATE account SET status= 0 WHERE id=?");
				stmt.setInt(1,(int)gameExt.getGameRoom().getUserByPlayerId(2).getSession().getProperty(TrisExtension.DATABASE_ID));
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
