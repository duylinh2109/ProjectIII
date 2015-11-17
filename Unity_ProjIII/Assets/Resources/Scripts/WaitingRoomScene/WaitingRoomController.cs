using UnityEngine;
using UnityEngine.UI;

public class WaitingRoomController : MonoBehaviour
{
    public GameObject exitLobby;
    public GameObject createRoom;

    public GameObject smartFoxLobby;

    // Hàm chọn exit lobby
    public void clickExitLobby()
    {
        Application.Quit();
        Debug.Log("Out Lobby");
    }

    //Hàm chọn Create Room
    public void clickCreateRoom()
    {
        smartFoxLobby.GetComponent<SmartFoxWaitingRoom>().createRoom();
        //Application.LoadLevel("MainRoom");
    }

    public void clickSendMessage()
    {
        smartFoxLobby.GetComponent<SmartFoxWaitingRoom>().SendPublicMessage();
    }

    public void clickJoinRoom()
    {
        GameObject.Find("SmartFoxWaitingRoomController").GetComponent<SmartFoxWaitingRoom>().JoinRoom(this.transform.Find("Text").GetComponent<Text>().text);
    }
}