using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Entities.Variables;
using Sfs2X.Requests;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.UI;

public class SmartFoxWaitingRoom : MonoBehaviour
{
    private const string EXTENSION_ID = "projectfinal";
    private const string EXTENSION_CLASS = "sfs2x.extensions.games.projectfinal.ProjectFinalExtension";

    private SmartFox sfs;
    public GameObject RoomListContent;
    public GameObject RoomButton;
    public InputField ChatBox;
    public GameObject ChatContent;
    public GameObject ChatBoxContent;

    private void Awake()
    {
        Application.runInBackground = true;

        if (SmartFoxConnection.IsInitialized)
            sfs = SmartFoxConnection.Connection;
        else
        {
            Application.LoadLevel("Login");
            return;
        }

        //sfs.RemoveAllEventListeners();

        Debug.Log("Login As: " + sfs.MySelf.Name);
        // Register event listeners
        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN, OnRoomJoin);
        sfs.AddEventListener(SFSEvent.ROOM_JOIN_ERROR, OnRoomJoinError);
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
        sfs.AddEventListener(SFSEvent.ROOM_ADD, OnRoomAdded);
        sfs.AddEventListener(SFSEvent.ROOM_REMOVE, OnRoomRemoved);
        sfs.AddEventListener(SFSEvent.PLAYER_TO_SPECTATOR, OnSpectatorEnterRoom);
        sfs.AddEventListener(SFSEvent.PLAYER_TO_SPECTATOR_ERROR, OnSpectatorEnterRoomErro);

        // Join the Lobby Room (must exist in the Zone!)
        sfs.Send(new JoinRoomRequest("The Lobby"));
    }

    // Use this for initialization
    private void Start()
    {
        ReloadRoomList();
    }

    // Update is called once per frame
    private void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (!ChatBox.text.Trim().Equals(""))
                SendPublicMessage();
        }   
    }

    public void createRoom()
    {
        // Configure Game Room
        RoomSettings settings = new RoomSettings(sfs.MySelf.Name + "'s Room");
        settings.GroupId = "games";
        settings.IsGame = true;
        
        //Settings for maximun player in game is 2
        settings.MaxUsers = 4;

        //Settings for maximun of spectators is no limit
        settings.MaxSpectators = 0;
        //settings.MaxSpectators = 0;

        settings.Extension = new RoomExtension(EXTENSION_ID, EXTENSION_CLASS);

        // Request Game Room creation to server
        sfs.Send(new CreateRoomRequest(settings, true, sfs.LastJoinedRoom));
        
        Debug.Log("Created Room");
    }

    private void reset()
    {
        // Remove SFS2X listeners
        sfs.RemoveAllEventListeners();
    }

    //----------------------------------------------------------
    // SmartFoxServer event listeners
    //----------------------------------------------------------

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS2X listeners
        reset();

        // Return to login scene
        Application.LoadLevel("Login");
    }

    private void OnRoomJoin(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];
        Debug.Log(room.Name);

        if (room.IsGame)
        {
            reset();
            if (room.Name.Contains(sfs.MySelf.Name))
            {
                Debug.Log("Create Var");
                RoomVariable map = new SFSRoomVariable("Map", "");
                RoomVariable pass = new SFSRoomVariable("Pass", "");
                RoomVariable readyCount = new SFSRoomVariable("ReadyCount", 1);
                List<RoomVariable> lRV = new List<RoomVariable>();
                lRV.Add(map);
                lRV.Add(pass);
                lRV.Add(readyCount);
                sfs.Send(new SetRoomVariablesRequest(lRV, room));
            }
            Debug.Log("Chuyen vao Lobby cua Room Game");
            PlayerPrefs.SetString("RoomName",room.Name);

            Application.LoadLevel("Lobby");
        }
        else
        {
            Debug.Log("\nYou joined a Room: " + room.Name);
        }
    }

    private void OnRoomJoinError(BaseEvent evt)
    {
        // Show error message
        Debug.Log("Room join failed: " + (string)evt.Params["errorMessage"]);
    }

    private void ReloadRoomList()
    {
        foreach (Transform child in RoomListContent.transform)
        {
            GameObject.Destroy(child.gameObject);
        }
        List<Room> listRoom = sfs.RoomList;
        int count = 0;
        foreach (Room room in listRoom)
        {
            if (room.IsGame)
            {
                Debug.Log(room.Name);
                GameObject go = Instantiate(RoomButton, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(RoomListContent.transform);
                go.transform.localScale = new Vector3(1,1,1);
                go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y,0);
                go.transform.Find("Text").GetComponent<Text>().text = room.Name;
                //go.transform.Find("Text").GetComponent<Text>().text = room.Name.Split('-')[0];
                //go.transform.Find("Pass").GetComponent<Text>().text = room.Name.Split('-')[1];
                //go.name = "BtnJoinRoom";
                //if (room.IsPasswordProtected)
                //    go.transform.Find("Image").GetComponent<Image>().sprite = this.lockIcon;
                //go.transform.Find("Player").GetComponent<Text>().text = room.UserCount.ToString();
                Debug.Log(room.PlayerList.Count);
                Debug.Log(room.UserList.Count);
            }
            count++;
        }
    }

    private void OnPublicMessage(BaseEvent evt)
    {
        User sender = (User)evt.Params["sender"];
        string message = (string)evt.Params["message"];
        ISFSObject objIn = (ISFSObject)evt.Params["data"];
        switch (message)
        {
            case "RoomCreated":
                ReloadRoomList();
                break;

            case "PublicMessage":
                GameObject go = Instantiate(ChatContent, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(ChatBoxContent.transform);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
                go.transform.Find("Sender").GetComponent<Text>().text = sender.Name + " : ";
                go.transform.Find("Content").GetComponent<Text>().text = objIn.GetUtfString("Content");
                break;
        }

        

        Debug.Log(sender.ToString() + message);
    }

    private void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        // Show system message
        Debug.Log("User " + user.Name + " entered the room: ");
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];

        if (user != sfs.MySelf)
        {
            // Show system message
            Debug.Log("User " + user.Name + " left the room");
        }
    }

    private void OnRoomAdded(BaseEvent evt)
    {
        Room room = (Room)evt.Params["room"];

        // Update view (only if room is game)
        if (room.IsGame)
        {
            ISFSObject objOut = new SFSObject();
            sfs.Send(new PublicMessageRequest("RoomCreated", objOut, sfs.LastJoinedRoom));
        }
    }

    public void OnRoomRemoved(BaseEvent evt)
    {
        // Update view
        // populateGamesList();
    }

    private void OnSpectatorEnterRoom(BaseEvent evt)
    {
        Debug.Log("Vao voi tu cach la khach");
    }

    private void OnSpectatorEnterRoomErro(BaseEvent evt)
    {
        //printSystemMessage("Room join failed: " + (string)evt.Params["errorMessage"]);
    }

    private void UpdateUserInLobby()
    {
        //currentUsersCountWaiting = sfs.GetRoomByName("The Lobby").UserCount;
        //currentUsersWaitingText.text = "Current Users: " + currentUsersCountWaiting.ToString();
    }

    public void SendPublicMessage()
    {
        ISFSObject objOut = new SFSObject();
        objOut.PutUtfString("Sender", sfs.MySelf.Name);
        objOut.PutUtfString("Content", ChatBox.text);
        ChatBox.text = "";
        ChatBox.ActivateInputField();
        ChatBox.Select();

        sfs.Send(new PublicMessageRequest("PublicMessage", objOut, sfs.LastJoinedRoom));
    }

    public void JoinRoom(string roomName)
    {
        if(sfs.GetRoomByName(roomName).PlayerList.Count <=4)
        {
            sfs.Send(new JoinRoomRequest(roomName));
        }
    }
}