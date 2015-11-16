using Sfs2X;
using UnityEngine;
using MySql.Data;
using System.Data;
using MySql.Data.MySqlClient;
using UnityEngine.UI;

public class LoginController : MonoBehaviour
{
    public GameObject setting;
    public GameObject menuSetting;
    public GameObject soundOn;
    public GameObject soundOff;
    public GameObject about;
    public GameObject aboutBR;
    public GameObject back;
    public GameObject returnMenu;
    public GameObject exitBR;
    public GameObject groupLogin;
    public GameObject groupRegister;
    public InputField username;
    public InputField password;
    public InputField usernameReg;
    public InputField passwordReg;
    public InputField passwordCFMReg;
    public Text error;

    public bool isSoundOn;
    public bool isSoundOff;

    public GameObject loginSmartFoxController;

    private void Awake()
    {
        isSoundOn = true;
        isSoundOff = false;
    }

    private void Start()
    {
        soundOff.SetActive(false);
        soundOn.SetActive(false);
        loginSmartFoxController.GetComponent<SmartFoxLogin>().JoinZone();
    }

    // Hàm chọn đăng ký
    public void clickRegister()
    {
        groupLogin.SetActive(false);
        groupRegister.SetActive(true);
    }

    //Hàm chọn Accept
    public void clickAccept()
    {
        //groupLogin.SetActive(true);
        //groupRegister.SetActive(false);
        Register();
    }

    //Hàm chọn cancel
    public void clickCancel()
    {
        groupLogin.SetActive(true);
        groupRegister.SetActive(false);
    }

    //Hàm chọn Login
    public void clickLogin()
    {
        Debug.Log("ClickLogin");
        loginSmartFoxController.GetComponent<SmartFoxLogin>().Login(username.text, password.text);
    }

    //Hàm chọn setting
    public void clickSetting()
    {
        setting.SetActive(false);
        returnMenu.SetActive(true);
        menuSetting.SetActive(true);
        about.SetActive(true);
        exitBR.SetActive(true);

        //isSoundOn = true;
        //isSoundOff = false;

        //Xét icon sound on
        if (isSoundOn)
        {
            soundOn.SetActive(true);
        }
        else
            soundOn.SetActive(false);

        //Xét icon sound off
        if (isSoundOff)
        {
            soundOff.SetActive(true);
        }
        else
            soundOff.SetActive(false);
    }

    //Hàm chọn SoundOn
    public void SoundOn()
    {
        isSoundOn = false;
        isSoundOff = true;
        //Xét icon sound on
        if (isSoundOn)
        {
            soundOn.SetActive(true);
        }
        else
            soundOn.SetActive(false);

        //Xét icon sound off
        if (isSoundOff)
        {
            soundOff.SetActive(true);
        }
        else
            soundOff.SetActive(false);
    }

    //Hàm chọn SoundOff
    public void SoundOff()
    {
        isSoundOn = true;
        isSoundOff = false;

        Debug.Log("tắt loa");

        //Xét icon sound on
        if (isSoundOn)
        {
            soundOn.SetActive(true);
        }
        else
            soundOn.SetActive(false);

        //Xét icon sound off
        if (isSoundOff)
        {
            soundOff.SetActive(true);
        }
        else
            soundOff.SetActive(false);
    }

    //Hàm chọn about
    public void clickAbout()
    {
        aboutBR.SetActive(true);
        back.SetActive(true);
        //aboutBR02.SetActive(false);
    }

    //Hàm chọn back
    public void clickBack()
    {
        groupLogin.SetActive(true);
        groupRegister.SetActive(false);
    }

    //Hàm chọn return menu setting
    public void clickReturn()
    {
        about.SetActive(false);
        menuSetting.SetActive(false);
        returnMenu.SetActive(false);
        setting.SetActive(true);
        exitBR.SetActive(false);
        Start();
    }

    // Hàm chọn exitBR
    public void clickExitBR()
    {
        Application.Quit();
        Debug.Log("Out Game");
    }

    public void Register()
    {
        if (usernameReg.text.Equals(""))
        {
            this.error.text = "Username can not be blank";
            return;
        }

        if (usernameReg.text.Length < 4)
        {
            this.error.text = "Username must have at least 4 characters";
            return;
        }

        if (passwordReg.text.Equals(""))
        {
            this.error.text = "Password can not be blank";
            return;
        }

        if (passwordReg.text.Length < 6)
        {
            this.error.text = "Password must have at least 4 characters";
            return;
        }

        if (!passwordReg.text.Equals(passwordCFMReg.text))
        {
            this.error.text = "Passwords mismatch!";
            return;
        }
        

        try
        {
            SmartFoxConnection.sCon.Open();
            //Debug.Log(sCon.State);
            MySqlCommand sCom = null;
            sCom = new MySqlCommand("INSERT INTO playerinfo(_username,_password) VALUES (@_Username,@_Password)", SmartFoxConnection.sCon);
            sCom.Parameters.AddWithValue("@_Username", "");
            sCom.Parameters.AddWithValue("@_Password", "");
            sCom.Parameters["@_Username"].Value = this.usernameReg.text.ToString();
            sCom.Parameters["@_Password"].Value = this.passwordReg.text.ToString();
            sCom.ExecuteNonQuery();
            Debug.Log("Done");
            SmartFoxConnection.sCon.Dispose();
            this.error.text = "You have successfully created an account\nYou will be redirecting to login section now.";
        }
        catch (MySqlException mse)
        {
            this.error.text = "This Username is already existed";
            Debug.Log(mse.InnerException);
        }
    }
}