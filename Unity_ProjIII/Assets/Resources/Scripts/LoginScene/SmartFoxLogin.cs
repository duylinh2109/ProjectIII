using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Requests;
using Sfs2X.Util;
using UnityEngine;

public class SmartFoxLogin : MonoBehaviour
{
    //----------------------------------------------------------
    // Editor public properties
    //----------------------------------------------------------

    [Tooltip("IP address or domain name of the SmartFoxServer 2X instance")]
    private string Host = "127.0.0.1";

    [Tooltip("TCP port listened by the SmartFoxServer 2X instance; used for regular socket connection in all builds except WebGL")]
    private int TcpPort = 9933;

    [Tooltip("WebSocket port listened by the SmartFoxServer 2X instance; used for in WebGL build only")]
    private int WSPort = 8888;

    [Tooltip("Name of the SmartFoxServer 2X Zone to join")]
    private string Zone = "BasicExamples";

    public GameObject UILogin;

    //----------------------------------------------------------
    // Private properties
    //----------------------------------------------------------

    private SmartFox sfs;

    //----------------------------------------------------------
    // Unity calback methods
    //----------------------------------------------------------

    private void Awake()
    {
        Application.runInBackground = true;

#if UNITY_WEBPLAYER
        if (!Security.PrefetchSocketPolicy(Host, TcpPort, 500))
        {
            Debug.LogError("Security Exception. Policy file loading failed!");
        }
#endif
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();
    }

    //----------------------------------------------------------
    // Public interface methods for UI
    //----------------------------------------------------------

    public void JoinZone()
    {
        // Set connection parameters
        ConfigData cfg = new ConfigData();
        cfg.Host = Host;
#if !UNITY_WEBGL
        cfg.Port = TcpPort;
#else
        cfg.Port = WSPort;
#endif
        cfg.Zone = Zone;

        // Initialize SFS2X client and add listeners
#if !UNITY_WEBGL
        sfs = new SmartFox();
#else
		sfs = new SmartFox(UseWebSocket.WS);
#endif
        cfg.Debug = true;

        // Set ThreadSafeMode explicitly, or Windows Store builds will get a wrong default value (false)
        sfs.ThreadSafeMode = true;

        sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
        sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);

        // Connect to SFS2X
        sfs.Connect(cfg);
    }

    //----------------------------------------------------------
    // Private helper methods
    //----------------------------------------------------------

    public void Login(string username, string password)
    {
        sfs.Send(new LoginRequest(username, password, Zone));
    }

    private void reset()
    {
        if (sfs != null)
            // Remove SFS2X listeners
            // This should be called when switching scenes, so events from the server do not trigger code in this scene
            sfs.RemoveAllEventListeners();
    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    private void OnConnection(BaseEvent evt)
    {
        if ((bool)evt.Params["success"])
        {
            // Save reference to SmartFox instance; it will be used in the other scenes
            SmartFoxConnection.Connection = sfs;
            reset();
            sfs.AddEventListener(SFSEvent.CONNECTION, OnConnection);
            sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
            sfs.AddEventListener(SFSEvent.LOGIN, OnLogin);
            sfs.AddEventListener(SFSEvent.LOGIN_ERROR, OnLoginError);
            Debug.Log("Logged into Zone");
        }
        else
        {
            Debug.Log("Failed to login into Zone");
        }
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS2X listeners and re-enable interface
        string reason = (string)evt.Params["reason"];

        if (reason != ClientDisconnectionReason.MANUAL)
        {
        }
    }

    private void OnLogin(BaseEvent evt)
    {
        Debug.Log("Logged into MainLobby");
        Application.LoadLevel("WaitingRoom");
        //sfs.Send(new JoinRoomRequest("Lobby"));
    }

    private void OnLoginError(BaseEvent evt)
    {
        Debug.Log("Failed to login into MainLobby");
    }
}