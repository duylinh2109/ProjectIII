using Sfs2X;
using UnityEngine;
using MySql.Data;
using System.Data;
using MySql.Data.MySqlClient;

public class SmartFoxConnection : MonoBehaviour
{
    private static SmartFoxConnection mInstance;
    private static SmartFox sfs;
    public static MySqlConnection sCon;


    public static SmartFox Connection
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
                sCon = new MySqlConnection("Server=127.0.0.1;Database=dotabound;Uid=root;Pooling=true;Allow User Variables=True");
            }
            return sfs;
        }
        set
        {
            if (mInstance == null)
            {
                mInstance = new GameObject("SmartFoxConnection").AddComponent(typeof(SmartFoxConnection)) as SmartFoxConnection;
                sCon = new MySqlConnection("Server=127.0.0.1;Database=dotabound;Uid=root;Pooling=true;Allow User Variables=True");
            }
            sfs = value;
        }
    }

    public static bool IsInitialized
    {
        get
        {
            return (sfs != null);
        }
    }

    //Handle disconnection, important
    private void OnApplicationQuit()
    {
        if (sfs.IsConnected)
        {
            sfs.Disconnect();
        }
    }
}