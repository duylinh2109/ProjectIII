package sfs2x.extensions.games.projectfinal;


import java.sql.SQLException;
import java.sql.Connection;
import java.sql.ResultSet;
import java.sql.PreparedStatement;
import com.smartfoxserver.bitswarm.sessions.ISession;
import com.smartfoxserver.v2.entities.data.SFSArray;
import com.smartfoxserver.v2.core.SFSEventParam;
import com.smartfoxserver.v2.core.ISFSEvent;
import com.smartfoxserver.v2.exceptions.SFSException;
import com.smartfoxserver.v2.exceptions.SFSLoginException;
import com.smartfoxserver.v2.exceptions.SFSErrorData;
import com.smartfoxserver.v2.exceptions.SFSErrorCode;
import com.smartfoxserver.v2.extensions.BaseServerEventHandler;
import com.smartfoxserver.v2.extensions.ExtensionLogLevel;

/*
 * To change this license header, choose License Headers in Project Properties.
 * To change this template file, choose Tools | Templates
 * and open the template in the editor.
 */
/**
 *
 * @author fmo08
 */
public class LoginHandler extends BaseServerEventHandler {

    @Override
    public void handleServerEvent(ISFSEvent event) throws SFSException {
        
        String username = (String) event.getParameter(SFSEventParam.LOGIN_NAME);
        String password = (String) event.getParameter(SFSEventParam.LOGIN_PASSWORD);
        ISession session = (ISession) event.getParameter(SFSEventParam.SESSION);
        
        try {
            trace(getParentExtension().getParentZone().getName());
            Connection conn = getParentExtension().getParentZone().getDBManager().getConnection();

            PreparedStatement sql = conn.prepareStatement("SELECT * from playerinfo where _username=?" );
            sql.setString(1, username);
            ResultSet result = sql.executeQuery();
            SFSArray row = SFSArray.newFromResultSet(result);
            if(0==row.size())
            {
                SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);
                data.addParameter(username);
                throw new SFSLoginException("No such username : " + username, data);
            }
            else if (!getApi().checkSecurePassword(session, row.getSFSObject(0).getUtfString("_password"), password)) {
                SFSErrorData data = new SFSErrorData(SFSErrorCode.LOGIN_BAD_PASSWORD);

                data.addParameter(username);

                throw new SFSLoginException("Wrong password for user " + username, data);
            }
            conn.close();
            trace("Login successful, joining room!");
            
            
        } catch (SQLException e) {
            trace(ExtensionLogLevel.WARN, " SQL Failed: " + e.toString());
        }
    }

}
