using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;
using Sfs2X.Entities.Variables;

public class SmartFoxLobby : MonoBehaviour
{
    private SmartFox sfs;
    public Sprite rdySprite;
    public Sprite cancelSprite;
    public GameObject startBTN;
    public GameObject readyBTN;
    public GameObject cancelBTN;
    public GameObject unitList;
    public GameObject mapList;
    public GameObject unitCard;
    public GameObject mapCard;
    public GameObject chatFrame;
    public GameObject chatContent;
    public InputField chatBox;
    public Room currentRoom;
    public bool isLoaded;

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

        sfs.AddEventListener(SFSEvent.CONNECTION_LOST, OnConnectionLost);
        sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);
        sfs.AddEventListener(SFSEvent.USER_ENTER_ROOM, OnUserEnterRoom);
        sfs.AddEventListener(SFSEvent.USER_EXIT_ROOM, OnUserExitRoom);
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionResponse);


        UserVariable uv = new SFSUserVariable("Ready", "NO");
        UserVariable uv2 = new SFSUserVariable("PlayerID", sfs.MySelf.PlayerId);
        List<UserVariable> lUV = new List<UserVariable>();
        lUV.Add(uv);
        lUV.Add(uv2);
        sfs.Send(new SetUserVariablesRequest(lUV));
        currentRoom = sfs.LastJoinedRoom;
    }

    private void Start()
    {
        StartCoroutine(WaitForLoad());
    }

    IEnumerator WaitForLoad()
    {
        yield return new WaitForSeconds(1);

        
    } 

    private void Update()
    {
        if (sfs != null)
            sfs.ProcessEvents();

        if (!Application.isLoadingLevel && !isLoaded)
        {
            foreach (User u in sfs.LastJoinedRoom.PlayerList)
            {
                    GameObject.Find("Slot" + u.PlayerId).transform.Find("PlayerName").GetComponent<Text>().text = u.Name;
                    if ((u.GetVariable("Ready").Value).ToString().Equals("YES"))
                        GameObject.Find("Slot" + u.PlayerId).transform.Find("Status").GetComponent<Image>().sprite = rdySprite;
                    else
                        GameObject.Find("Slot" + u.PlayerId).transform.Find("Status").GetComponent<Image>().sprite = cancelSprite;
            }

            if (sfs.MySelf.PlayerId.Equals(1))
            {
                readyBTN.SetActive(false);
                cancelBTN.SetActive(false);
                startBTN.SetActive(true);
                startBTN.GetComponent<Button>().interactable = false;

                RoomVariable map = new SFSRoomVariable("Map", "Map1");
                List<RoomVariable> lRV = new List<RoomVariable>();
                lRV.Add(map);
                sfs.Send(new SetRoomVariablesRequest(lRV, sfs.LastJoinedRoom));
            }
            else
            {
                readyBTN.SetActive(true);
                cancelBTN.SetActive(false);
                startBTN.SetActive(false);
            }

            LoadCard();
            LoadMap();
            isLoaded = true;
        }
    }

    public void reset()
    {
        sfs.RemoveAllEventListeners();
    }

    private void OnConnectionLost(BaseEvent evt)
    {
        // Remove SFS2X listeners
        reset();

        //if (shuttingDown == true)
        //    return;

        // Return to login scene
        Application.LoadLevel("Login");
    }

    private void OnPublicMessage(BaseEvent evt)
    {
        User sender = (User)evt.Params["sender"];
        string message = (string)evt.Params["message"];
        ISFSObject objIn = (ISFSObject)evt.Params["data"];
        switch (message)
        {
            case "PlayerReady":
                GameObject.Find("Slot" + objIn.GetInt("Sender")).transform.Find("Status").GetComponent<Image>().sprite = rdySprite;
                if (sfs.MySelf.PlayerId.Equals(1))
                {
                    if (int.Parse(currentRoom.GetVariable("ReadyCount").Value.ToString()) % 2 == 0)
                    {
                        if (GameObject.Find("Slot2").transform.Find("Status").GetComponent<Image>().sprite.name.Equals("pReady") || GameObject.Find("Slot4").transform.Find("Status").GetComponent<Image>().sprite.name.Equals("pReady"))
                        {
                            Debug.Log("YES");
                            //startBTN.SetActive(true);
                            startBTN.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            Debug.Log("NO");
                            //startBTN.SetActive(false);
                            startBTN.GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        Debug.Log("NO");
                        //startBTN.SetActive(false);
                        startBTN.GetComponent<Button>().interactable = false;
                    }
                }
                break;

            case "PlayerCancel":
                GameObject.Find("Slot" + objIn.GetInt("Sender")).transform.Find("Status").GetComponent<Image>().sprite = cancelSprite;
                if (sfs.MySelf.PlayerId.Equals(1))
                {
                    if (int.Parse(currentRoom.GetVariable("ReadyCount").Value.ToString()) % 2 == 0)
                    {
                        if (GameObject.Find("Slot2").transform.Find("Status").GetComponent<Image>().sprite.name.Equals("pReady") || GameObject.Find("Slot4").transform.Find("Status").GetComponent<Image>().sprite.name.Equals("pReady"))
                        {
                            Debug.Log("YES");
                            //startBTN.SetActive(true);
                            startBTN.GetComponent<Button>().interactable = true;
                        }
                        else
                        {
                            Debug.Log("NO");
                            //startBTN.SetActive(false);
                            startBTN.GetComponent<Button>().interactable = false;
                        }
                    }
                    else
                    {
                        Debug.Log("NO");
                        //startBTN.SetActive(false);
                        startBTN.GetComponent<Button>().interactable = false;
                    }
                }
                break;

            case "PublicMessage":
                GameObject go = Instantiate(chatContent, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(chatFrame.transform);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
                go.transform.Find("Sender").GetComponent<Text>().text = sender.Name + " : ";
                go.transform.Find("Content").GetComponent<Text>().text = objIn.GetUtfString("Content");
                break;
        }
    }

    private void OnUserEnterRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];
        GameObject.Find("Slot" + user.PlayerId).transform.Find("PlayerName").GetComponent<Text>().text = user.Name;
        //readyCount++;
        // Show system message
        //printSystemMessage("User " + user.Name + " entered the room");
    }

    private void OnUserExitRoom(BaseEvent evt)
    {
        User user = (User)evt.Params["user"];

        GameObject.Find("Slot" + user.GetVariable("PlayerID").Value).transform.Find("PlayerName").GetComponent<Text>().text = "Waiting for player...";
        GameObject.Find("Slot" + user.GetVariable("PlayerID").Value).transform.Find("Status").GetComponent<Image>().sprite = cancelSprite;

        if (user != sfs.MySelf)
        {
            // Show system message
            //printSystemMessage("User " + user.Name + " left the room");
        }
        //readyCount--;
    }

    private void OnExtensionResponse(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        SFSObject dataObject = (SFSObject)evt.Params["params"];

        switch (cmd)
        {
            case ConfigResponseCmd.cmd_playerready:
                break;

            case ConfigResponseCmd.cmd_startgame:
                sfs.RemoveAllEventListeners();

                Application.LoadLevel("Game");
                break;
        }
    }

    public void Ready()
    {
        UserVariable uv = new SFSUserVariable("Ready", "YES");
        List<UserVariable> lUV = new List<UserVariable>();
        lUV.Add(uv);
        sfs.Send(new SetUserVariablesRequest(lUV));

        Debug.Log("AAAAAAAA : "+sfs.LastJoinedRoom.GetVariable("ReadyCount").Value);

        RoomVariable rv = new SFSRoomVariable("ReadyCount", int.Parse(sfs.LastJoinedRoom.GetVariable("ReadyCount").Value.ToString()) + 1);
        List<RoomVariable> lRV = new List<RoomVariable>();
        lRV.Add(rv);
        sfs.Send(new SetRoomVariablesRequest(lRV));

        ISFSObject isfsO = new SFSObject();
        isfsO.PutInt("Sender",sfs.MySelf.PlayerId);
        sfs.Send(new PublicMessageRequest("PlayerReady",isfsO,sfs.LastJoinedRoom));
    }

    public void StartGame()
    {
        
    }

    public void Cancel()
    {
        UserVariable uv = new SFSUserVariable("Ready", "NO");
        List<UserVariable> lUV = new List<UserVariable>();
        lUV.Add(uv);
        sfs.Send(new SetUserVariablesRequest(lUV));

        RoomVariable rv = new SFSRoomVariable("ReadyCount", int.Parse(sfs.LastJoinedRoom.GetVariable("ReadyCount").Value.ToString()) - 1);
        List<RoomVariable> lRV = new List<RoomVariable>();
        lRV.Add(rv);
        sfs.Send(new SetRoomVariablesRequest(lRV));

        ISFSObject isfsO = new SFSObject();
        isfsO.PutInt("Sender", sfs.MySelf.PlayerId);
        sfs.Send(new PublicMessageRequest("PlayerCancel", isfsO, sfs.LastJoinedRoom));
    }

    public void LoadCard()
    {
        //Sprite[] ol = Resources.LoadAll<Sprite>("Textures/Cards");
        //foreach(Sprite i in ol)
        //{
        //    GameObject go = Instantiate(unitCard,Vector3.zero,Quaternion.identity) as GameObject;
        //    go.transform.SetParent(unitList.transform);
        //    go.transform.localScale = new Vector3(1, 1, 1);
        //    go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
        //    go.GetComponent<Image>().sprite = i;
        //}
    }

    public void LoadMap()
    {
        if (sfs.MySelf.PlayerId.Equals(1))
        {
            Sprite[] ol = Resources.LoadAll<Sprite>("Textures/Maps");
            foreach (Sprite i in ol)
            {
                GameObject go = Instantiate(mapCard, Vector3.zero, Quaternion.identity) as GameObject;
                go.transform.SetParent(mapList.transform);
                go.transform.localScale = new Vector3(1, 1, 1);
                go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
                go.GetComponent<Image>().sprite = i;
            }
        }
        else
        {
            Sprite s = Resources.Load<Sprite>("Textures/Maps/" + sfs.LastJoinedRoom.GetVariable("Map").Value);
            GameObject go = Instantiate(mapCard, Vector3.zero, Quaternion.identity) as GameObject;
            go.transform.SetParent(mapList.transform);
            go.transform.localScale = new Vector3(1, 1, 1);
            go.transform.localPosition = new Vector3(go.transform.position.x, go.transform.position.y, 0);
            go.GetComponent<Image>().sprite = s;
        }
    }

    public void SendPublicMessage()
    {
        ISFSObject objOut = new SFSObject();
        objOut.PutUtfString("Sender", sfs.MySelf.Name);
        objOut.PutUtfString("Content", chatBox.text);
        chatBox.text = "";
        chatBox.ActivateInputField();
        chatBox.Select();

        sfs.Send(new PublicMessageRequest("PublicMessage", objOut, sfs.LastJoinedRoom));
    }

    public void OnApplicationQuit()
    {
        sfs.Disconnect();
    }
}