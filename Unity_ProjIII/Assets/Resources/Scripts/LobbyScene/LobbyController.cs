using UnityEngine;
using UnityEngine.UI;

public class LobbyController : MonoBehaviour
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
        smartFoxLobby.GetComponent<SmartFoxLobby>().createRoom();
        //Application.LoadLevel("MainRoom");
    }

    public void clickSendMessage()
    {
        smartFoxLobby.GetComponent<SmartFoxLobby>().SendPublicMessage();
    }

    public void clickJoinRoom()
    {
        GameObject.Find("SmartFoxLobbyController").GetComponent<SmartFoxLobby>().JoinRoom(this.transform.Find("Text").GetComponent<Text>().text);
    }
}