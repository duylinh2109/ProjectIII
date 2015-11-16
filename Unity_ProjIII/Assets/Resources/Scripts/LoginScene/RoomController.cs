using UnityEngine;

public class RoomController : MonoBehaviour
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
        smartFoxRoomCtrl.GetComponent<SmartFoxMainRoom>().Ready();
        //Application.LoadLevel("Game");
    }

    public void clickStart()
    {
        smartFoxRoomCtrl.GetComponent<SmartFoxMainRoom>().StartGame();
        //Application.LoadLevel("Game");
    }

    public void clickCancel()
    {
        ready.SetActive(true);
        cancel.SetActive(false);
        smartFoxRoomCtrl.GetComponent<SmartFoxMainRoom>().Cancel();
        //Application.LoadLevel("Game");
    }

    //Hàm chọn leave
    public void clickLeave()
    {
        //Application.LoadLevel("MainLobby");
    }

    public void clickSend()
    {
        smartFoxRoomCtrl.GetComponent<SmartFoxMainRoom>().SendPublicMessage();
    }
}