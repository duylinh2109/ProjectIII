using UnityEngine;

public class LobbyController : MonoBehaviour
{
    public GameObject ready;
    public GameObject leave;
    public GameObject cancel;
    public GameObject start;
    public GameObject smartFoxRoomCtrl;

    //Hàm chọn ready
    public void clickReady()
    {
        ready.SetActive(false);
        cancel.SetActive(true);
        smartFoxRoomCtrl.GetComponent<SmartFoxLobby>().Ready();
        //Application.LoadLevel("Game");
    }

    public void clickStart()
    {
        smartFoxRoomCtrl.GetComponent<SmartFoxLobby>().StartGame();
        //Application.LoadLevel("Game");
    }

    public void clickCancel()
    {
        ready.SetActive(true);
        cancel.SetActive(false);
        smartFoxRoomCtrl.GetComponent<SmartFoxLobby>().Cancel();
        //Application.LoadLevel("Game");
    }

    //Hàm chọn leave
    public void clickLeave()
    {
        //Application.LoadLevel("MainLobby");
    }

    public void clickSend()
    {
        smartFoxRoomCtrl.GetComponent<SmartFoxLobby>().SendPublicMessage();
    }
}