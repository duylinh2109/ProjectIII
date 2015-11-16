using Sfs2X;
using Sfs2X.Core;
using Sfs2X.Entities.Data;
using Sfs2X.Requests;
using UnityEngine;

public class SmartFoxInGame : MonoBehaviour
{
    private SmartFox sfs;

    private string userName = "Player";

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
        sfs.RemoveAllEventListeners();

        // Register event listeners
        sfs.AddEventListener(SFSEvent.EXTENSION_RESPONSE, OnExtensionReponse);
        sfs.AddEventListener(SFSEvent.PUBLIC_MESSAGE, OnPublicMessage);

        OnStartGame();
    }

    private void OnExtensionReponse(BaseEvent evt)
    {
        string cmd = (string)evt.Params["cmd"];
        SFSObject dataObject = (SFSObject)evt.Params["params"];

        switch (cmd)
        {
            case "say_hello":
                Debug.Log(dataObject.GetUtfString("message"));
                break;
        }
    }

    public void Update()
    {
        // As Unity is not thread safe, we process the queued up callbacks on every frame
        if (sfs != null)
            sfs.ProcessEvents();
    }

    #region --Public Messages  ---

    private GameObject player;

    private void OnStartGame()
    {
        if (sfs.MySelf.PlayerId == 1)
        {
            player = (GameObject)Instantiate(Resources.Load("Prefabs/player", typeof(GameObject)), new Vector3(-5, 0, 0), Quaternion.identity);
            player.gameObject.name = userName + sfs.MySelf.PlayerId.ToString();
        }
        else if (sfs.MySelf.PlayerId == 2)
        {
            player = (GameObject)Instantiate(Resources.Load("Prefabs/player", typeof(GameObject)), new Vector3(5, 0, 0), Quaternion.identity);
            player.name = userName + sfs.MySelf.PlayerId;
        }

        SFSObject obj = new SFSObject();
        obj.PutUtfString("username", userName + sfs.MySelf.PlayerId);
        obj.PutFloat("x", player.transform.position.x);
        obj.PutFloat("y", player.transform.position.y);
        obj.PutFloat("z", player.transform.position.z);

        sfs.Send(new PublicMessageRequest("newplayerjoingame", obj));
    }

    public void SendPublicMessageRequestFire(int myPlayerID, int shootPower, float directX, float directY)
    {
        SFSObject obj = new SFSObject();
        obj.PutUtfString("username", userName + myPlayerID);
        obj.PutInt("shootpower", shootPower);
        obj.PutFloat("directx", directX);
        obj.PutFloat("directy", directY);
        sfs.Send(new PublicMessageRequest("playerfire", obj));
    }

    public void SendPublicMessageTurnBase(int myPlayerId)
    {
        SFSObject obj = new SFSObject();
        obj.PutUtfString("username", userName + myPlayerId);
        sfs.Send(new PublicMessageRequest("turnbase", obj));
    }

    public void SendPublicMessageForSyncMove(int myPlayerId, Vector3 destination)
    {
        SFSObject obj = new SFSObject();
        obj.PutUtfString("username", userName + myPlayerId);
        obj.PutFloat("destinationx", destination.x);
        obj.PutFloat("destinationy", destination.y);
        obj.PutFloat("destinationz", destination.z);
        sfs.Send(new PublicMessageRequest("move", obj));
    }

    private void OnPublicMessage(BaseEvent evt)
    {
        string cmd = (string)evt.Params["message"];
        ISFSObject objIn = (ISFSObject)evt.Params["data"];

        switch (cmd)
        {
            case "newplayerjoingame":
                Debug.Log("AAA");
                if (objIn.GetUtfString("username") != this.userName + sfs.MySelf.PlayerId)
                {
                    GameObject player2 = (GameObject)Instantiate(Resources.Load("Prefabs/player", typeof(GameObject)), new Vector3(objIn.GetFloat("x"), objIn.GetFloat("y"), objIn.GetFloat("z")), Quaternion.identity);
                    player2.name = objIn.GetUtfString("username");
                    player2.GetComponent<PlayerBehaviour>().isMyTurn = false;
                    GameObject.Find(this.userName + sfs.MySelf.PlayerId).GetComponent<PlayerBehaviour>().isMyTurn = true;
                }
                break;

            case "turnbase":

                if (objIn.GetUtfString("username") != this.userName + sfs.MySelf.PlayerId)
                {
                    GameObject.Find(userName + sfs.MySelf.PlayerId).GetComponent<PlayerBehaviour>().isMyTurn = true;
                }
                break;

            case "playerfire":

                if (objIn.GetUtfString("username") != this.userName + sfs.MySelf.PlayerId)
                {
                   // GameObject.Find(objIn.GetUtfString("username")).GetComponent<PlayerBehaviour>().Fire(
                    //    objIn.GetFloat("directx"), objIn.GetFloat("directy"), objIn.GetInt("shootpower"));
                }
                break;

            case "move":
                if (objIn.GetUtfString("username") != this.userName + sfs.MySelf.PlayerId)
                {
                   // GameObject.Find(objIn.GetUtfString("username")).GetComponent<PlayerBehaviour>().isMoving = true;
                  //  GameObject.Find(objIn.GetUtfString("username")).GetComponent<PlayerBehaviour>().destination =
                        new Vector3(objIn.GetFloat("destinationx"), objIn.GetFloat("destinationy"), objIn.GetFloat("destinationz"));
                }
                break;
        }
    }

    #endregion --Public Messages  ---
}