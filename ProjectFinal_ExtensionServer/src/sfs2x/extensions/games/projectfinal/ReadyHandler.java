/*    */ package sfs2x.extensions.games.projectfinal;
/*    */ 
/*    */ import com.smartfoxserver.v2.entities.Room;
/*    */ import com.smartfoxserver.v2.entities.RoomSize;
/*    */ import com.smartfoxserver.v2.entities.User;
/*    */ import com.smartfoxserver.v2.entities.data.ISFSObject;
/*    */ import com.smartfoxserver.v2.entities.data.SFSObject;
/*    */ import com.smartfoxserver.v2.extensions.BaseClientRequestHandler;
/*    */ 
/*    */ public class ReadyHandler extends BaseClientRequestHandler
/*    */ {
/*    */   public void handleClientRequest(User user, ISFSObject params)
/*    */   {
/* 14 */     ProjectFinalExtension projExt = (ProjectFinalExtension)getParentExtension();
/*    */     
/* 16 */     if (user.isPlayer())
/*    */     {
/* 18 */       UpdateStatusForUserReadyToGame(projExt, user);
/* 19 */       projExt.numUserInRoomIsReady += 1;
/* 20 */       trace(new Object[] { Integer.valueOf(projExt.numUserInRoomIsReady) });
/* 21 */       if ((projExt.getParentRoom().getSize().getUserCount() == 2) && (projExt.numUserInRoomIsReady == 2))
/*    */       {
/* 23 */         trace(new Object[] { "start game!" });
/* 24 */         projExt.startGame();
/*    */       }
/*    */     }
/*    */   }
/*    */   
/*    */ 
/*    */   private void UpdateStatusForUserReadyToGame(ProjectFinalExtension projExt, User user)
/*    */   {
/* 32 */     trace(new Object[] { user.getName() + " Is Ready!" + " PlayerId: " + user.getPlayerId() });
/*    */     
/* 34 */     ISFSObject resObj = new SFSObject();
/* 35 */     resObj.putUtfString("playernameisready", user.getName());
/* 36 */     resObj.putInt("playeridisready", user.getId());
/*    */     
/* 38 */     send("playerready", resObj, projExt.getGameRoom().getUserList());
/*    */   }
/*    */ }


/* Location:              C:\Program Files\SmartFoxServer_2X\SFS2X\extensions\SFSExtensions\projectfinalExtension.jar!\sfs2x\extensions\games\projectfinal\ReadyHandler.class
 * Java compiler version: 7 (51.0)
 * JD-Core Version:       0.7.1
 */