using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class PlayerBehaviour : MonoBehaviour
{
    public GameObject bullet;
    public CamMove cam;

    public Transform targetLock;
    public Transform mainChar;
    public Transform mainWeap;
    public Transform shootAnchor;
    public Transform hpAnchor;
    public Transform shootHelper;
    public Transform shootHelperStart;
    public Transform shootHelperEnd;

    public GUIStyle lblNameStyle;
    public GUIStyle lblNameBadgeStyle;
    public GUIStyle lblDegreeStyle;
    public GUIStyle lblClockStyle;

    public Texture2D RTHPBar;
    public Texture2D BTHPBar;
    public Texture2D HPBar;
    public Texture2D energyBar;

    public Texture2D forceBar;
    public Texture2D forceBarActive;
    public Texture2D forceBarBack;
    public Texture2D forceBarMark;
    public Texture2D arrowAngle;
    public Texture2D angleBG;

    public Texture2D blueTeamBadge;
    public Texture2D redTeamBadge;

    public Texture2D currentBadge;
    public Texture2D nextBadge;

    public Texture2D clockBG;

    public float HP , EN , maxHP, maxEN;

    public bool isMyTurn;

    public float shootForce , shootAngle , shootForceMark , shootAngleHelper;

    public List<string> blueTeam, redTeam;

    public string playerName;

    public float countDown;

    public int currentTurn, nextTurn;

    void Start()
    {
        HP = maxHP = 100;
        EN = maxEN = 100;
        isMyTurn = true;
        mainChar = transform.Find("Char");
        mainWeap = transform.Find("Weap");
        cam = Camera.main.GetComponent<CamMove>();
        lblNameStyle = new GUIStyle();
        lblNameStyle.fontSize = 12;
        lblNameStyle.fontStyle = FontStyle.Bold;
        lblNameStyle.normal.textColor = Color.white;

        lblNameBadgeStyle = new GUIStyle();
        lblNameBadgeStyle.fontSize = 14;
        lblNameBadgeStyle.fontStyle = FontStyle.Bold;
        lblNameBadgeStyle.normal.textColor = Color.white;

        lblDegreeStyle = new GUIStyle();
        lblDegreeStyle.fontSize = 18;
        lblDegreeStyle.fontStyle = FontStyle.Bold;
        lblDegreeStyle.normal.textColor = Color.white;

        lblClockStyle = new GUIStyle();
        lblClockStyle.fontSize = 60;
        lblClockStyle.fontStyle = FontStyle.Bold;
        lblClockStyle.normal.textColor = Color.white;

        shootAngle = shootAngleHelper = 20;

        blueTeam = new List<string>();
        redTeam = new List<string>();

        blueTeam.Add("Player 1");
        redTeam.Add("Player 2");

        countDown = 60;

        currentTurn = 20;
        nextTurn = 10;
    }

    void Update()
    {
        if (cam.cs.Equals(CamMove.CameraState.TopDown))
            Control();

        if (Input.GetKey("a"))
        {
            shootAngle -= 20 * Time.deltaTime;
        }
        else if (Input.GetKey("d"))
        {
            shootAngle += 20 * Time.deltaTime;
        }
        Fire();

        shootHelper.rotation = Quaternion.LookRotation(new Vector3(targetLock.position.x, this.transform.position.y, targetLock.position.z) - transform.position) * Quaternion.FromToRotation(Vector3.forward, new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * shootAngle), Mathf.Cos(Mathf.Deg2Rad * shootAngle)));

        countDown -= Time.deltaTime;
        if (countDown < 0)
            countDown = 60;
    }

    void Control()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            mainChar.GetComponent<Animation>().CrossFade("Troop_Run");
            mainWeap.GetComponent<Animation>().CrossFade("Weapon_Rifle_Idle");
            transform.rotation = Quaternion.LookRotation(Vector3.forward);
            transform.Translate(Vector3.forward * Time.deltaTime * 10);

        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            mainChar.GetComponent<Animation>().CrossFade("Troop_Run");
            mainWeap.GetComponent<Animation>().CrossFade("Weapon_Rifle_Idle");
            transform.rotation = Quaternion.LookRotation(-Vector3.forward);
            transform.Translate(Vector3.forward * Time.deltaTime * 10);

        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            mainChar.GetComponent<Animation>().CrossFade("Troop_Run");
            mainWeap.GetComponent<Animation>().CrossFade("Weapon_Rifle_Idle");
            transform.rotation = Quaternion.LookRotation(Vector3.left);
            transform.Translate(Vector3.forward * Time.deltaTime * 10);

        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            mainChar.GetComponent<Animation>().CrossFade("Troop_Run");
            mainWeap.GetComponent<Animation>().CrossFade("Weapon_Rifle_Idle");
            transform.rotation = Quaternion.LookRotation(Vector3.right);
            transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        else if (!mainChar.GetComponent<Animation>().isPlaying)
        {
            mainChar.GetComponent<Animation>().CrossFade("Troop_Idle");
            mainWeap.GetComponent<Animation>().CrossFade("Weapon_Rifle_Idle");
        }

        if (Input.GetKeyDown("l") && !cam.cs.Equals(CamMove.CameraState.SideScrolling))
        {
            transform.LookAt( new Vector3(targetLock.position.x, transform.position.y,targetLock.position.z) );
            cam.cs = CamMove.CameraState.SideScrolling;
            Vector3 oldPos = Camera.main.transform.position;
            Camera.main.transform.position = transform.position;
            Camera.main.transform.rotation = Quaternion.LookRotation(Quaternion.AngleAxis(90, Vector3.up) * (transform.position - targetLock.position));
            Camera.main.transform.position = Camera.main.transform.position - Camera.main.transform.forward * 15;
            Camera.main.transform.position = Camera.main.transform.position + Camera.main.transform.up * 15;
            Camera.main.transform.LookAt(transform);
        }
    }

    void OnGUI()
    {
        DrawHPBar();

        DrawTeamBadge();

        DrawClock();

        DrawShootHelper();

       
       
    }

    void Fire()
    {
        if (isMyTurn)
        {
            if (Input.GetKey(KeyCode.Space) && shootForce < 4000)
            {
                shootForce += 50;
            }
            if (Input.GetKeyUp(KeyCode.Space))
            {
                GetComponent<Rigidbody>().isKinematic = true;
                shootForceMark = shootForce;
                mainChar.GetComponent<Animation>().CrossFade("Troop_Grenade");
                StartCoroutine(Throw());
            }
        }
    }

    IEnumerator Throw()
    {
        //shootHelper.gameObject.SetActive(false);
        yield return new WaitForSeconds(1.6f);
        GameObject go = Instantiate(bullet, shootAnchor.position, Quaternion.identity) as GameObject;
        go.layer = 15;
        go.transform.rotation = Quaternion.LookRotation(new Vector3(targetLock.position.x, this.transform.position.y, targetLock.position.z) - transform.position) * Quaternion.FromToRotation(Vector3.forward, new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * shootAngle), Mathf.Cos(Mathf.Deg2Rad * shootAngle)));
        go.GetComponent<Rigidbody>().AddForce(go.transform.forward * shootForce);
        //componentSmartfoxInGame.SendPublicMessageTurnBase(sfs.MySelf.PlayerId);
        //componentSmartfoxInGame.SendPublicMessageRequestFire(sfs.MySelf.PlayerId, (int)speedForce, direction.x, direction.y);
        shootForce = 0;
        GetComponent<Rigidbody>().isKinematic = false;
        //isMyTurn = false;
    }

    void Fire(float directX, float directY, int power)
    {
        //Vector3 direction = new Vector3(0, Mathf.Sin(Mathf.Deg2Rad * angle), Mathf.Cos(Mathf.Deg2Rad * angle));

        Vector3 direction = new Vector3(directX, directY, 0);

        GameObject go = Instantiate(bullet, transform.position + new Vector3(0, 1, 0), Quaternion.identity) as GameObject;
        Debug.Log(power);
        go.GetComponent<Rigidbody>().AddForce(direction * power);
    }

    void DrawMiniMap()
    {

    }

    void DrawHPBar()
    {
        Vector2 y = Camera.main.WorldToScreenPoint(hpAnchor.position);
        {
            //HP bar
            GUI.DrawTexture(new Rect(y.x - BTHPBar.width / 2, Screen.height - y.y - BTHPBar.height / 2 - (cam.offset * 10), BTHPBar.width, BTHPBar.height), BTHPBar);

            GUI.DrawTextureWithTexCoords(new Rect(y.x - BTHPBar.width / 2 + 38, Screen.height - y.y - BTHPBar.height / 2 + 4 - (cam.offset * 10), (HP / maxHP) * HPBar.width, HPBar.height), HPBar, new Rect(0.0f, 0.0f, HP / maxHP, 1.0f), true);

            GUI.DrawTextureWithTexCoords(new Rect(y.x - BTHPBar.width / 2 + 38, Screen.height - y.y - BTHPBar.height / 2 + 26 - (cam.offset * 10), (EN / maxEN) * energyBar.width, energyBar.height), energyBar, new Rect(0.0f, 0.0f, EN / maxEN, 1.0f), true);

            GUI.Label(new Rect(y.x - (lblNameStyle.CalcSize(new GUIContent("Player 1")).x / 2), Screen.height - y.y - (lblNameStyle.CalcSize(new GUIContent("Player 1")).y / 2) - 7 - (cam.offset * 10), Screen.width, Screen.height), new GUIContent("Player 1"), lblNameStyle);

            //Force Bar Back
            GUI.DrawTexture(new Rect(Screen.width / 2 - forceBarBack.width / 2, Screen.height - forceBarBack.height / 2 - 30, forceBarBack.width, forceBarBack.height), forceBarBack);

            //Force Bar Mark
            GUI.DrawTexture(new Rect(Screen.width / 2 - forceBarActive.width / 2 + (shootForceMark / 4000) * forceBarActive.width, Screen.height - forceBarMark.height / 2 - 30, forceBarMark.width, forceBarMark.height), forceBarMark);

            //Force Bar Active
            GUI.DrawTextureWithTexCoords(new Rect(Screen.width / 2 - forceBarActive.width / 2, Screen.height - forceBarActive.height / 2 - 30, (shootForce / 4000) * forceBarActive.width, forceBarActive.height), forceBarActive, new Rect(0.0f, 0.0f, shootForce / 4000, 1.0f), true);

            //Force Bar
            GUI.DrawTexture(new Rect(Screen.width / 2 - forceBar.width / 2, Screen.height - forceBar.height / 2 - 30, forceBar.width, forceBar.height), forceBar);
        }

        if (cam.cs.Equals(CamMove.CameraState.SideScrolling))
        {
            lblDegreeStyle.fontSize = 18;
            Vector2 lblDegreeOffest = new Vector2(lblDegreeStyle.CalcSize(new GUIContent(((int)shootAngle).ToString())).x, lblDegreeStyle.CalcSize(new GUIContent(((int)shootAngle).ToString())).y);
            GUI.Label(new Rect(y.x - BTHPBar.width / 2 + 19 - lblDegreeOffest.x / 2, Screen.height - y.y - BTHPBar.height / 2 - (cam.offset * 10) + 19 - lblDegreeOffest.y / 2, BTHPBar.width, BTHPBar.height), new GUIContent(((int)shootAngle).ToString()), lblDegreeStyle);
        }
        else
        {
            lblDegreeStyle.fontSize = 33;
            Vector2 lblDegreeOffest = new Vector2(lblDegreeStyle.CalcSize(new GUIContent("B")).x, lblDegreeStyle.CalcSize(new GUIContent("B")).y);
            GUI.Label(new Rect(y.x - BTHPBar.width / 2 + 19 - lblDegreeOffest.x / 2, Screen.height - y.y - BTHPBar.height / 2 - (cam.offset * 10) + 20 - lblDegreeOffest.y / 2, BTHPBar.width, BTHPBar.height), new GUIContent("B"), lblDegreeStyle);

        }
    }

    void DrawShootHelper()
    {
        if (cam.cs.Equals(CamMove.CameraState.SideScrolling))
        {
            Vector2 x = Camera.main.WorldToScreenPoint(shootAnchor.position);

            Vector3 helperStart = Camera.main.WorldToScreenPoint(shootHelperStart.position);
            Vector3 helperEnd = Camera.main.WorldToScreenPoint(shootHelperEnd.position);
            shootAngleHelper = Vector3.Angle(new Vector3(5, 0, 0), helperEnd - helperStart);


            GUIUtility.RotateAroundPivot(-shootAngleHelper % 360, new Vector2(x.x, Screen.height - x.y - arrowAngle.height / 2));
            GUI.DrawTexture(new Rect(x.x, Screen.height - x.y - arrowAngle.height / 2, arrowAngle.width, arrowAngle.height), arrowAngle);
        }
    }

    void DrawTurnUI()
    {

    }

    void DrawTeamBadge()
    {
        for(int i = 0;i<blueTeam.Count;i++)
        {
            GUI.DrawTexture(new Rect(Screen.width/2 - blueTeamBadge.width - (blueTeamBadge.width *i) -  clockBG.width / 2, blueTeamBadge.height / 2, blueTeamBadge.width, blueTeamBadge.height), blueTeamBadge);
            
            Vector2 lblNameOffset = new Vector2(lblNameBadgeStyle.CalcSize(new GUIContent(blueTeam[i])).x , lblNameBadgeStyle.CalcSize(new GUIContent(blueTeam[i])).y);
            GUI.Label(new Rect(Screen.width / 2 - blueTeamBadge.width/2 - (blueTeamBadge.width * i) - lblNameOffset.x/ 2  - clockBG.width / 2, blueTeamBadge.height / 2 + +lblNameOffset.y / 2, blueTeamBadge.width, blueTeamBadge.height), new GUIContent(blueTeam[i]), lblNameBadgeStyle);

            if(currentTurn >= 20 && currentTurn % 20 == i)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - currentBadge.width - (currentBadge.width * i) - clockBG.width / 2, currentBadge.height / 2 + 33, currentBadge.width, currentBadge.height), currentBadge);

            }

            if (nextTurn >= 20 && nextTurn % 20 == i)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 - nextBadge.width - (nextBadge.width * i) - clockBG.width / 2, nextBadge.height / 2 + 33, nextBadge.width, nextBadge.height), nextBadge);

            }
        }

        for (int i = 0; i < redTeam.Count; i++)
        {
            GUI.DrawTexture(new Rect(Screen.width / 2 +  (redTeamBadge.width * i)  + clockBG.width / 2, redTeamBadge.height / 2, redTeamBadge.width, redTeamBadge.height), redTeamBadge);

            Vector2 lblNameOffset = new Vector2(lblNameBadgeStyle.CalcSize(new GUIContent(redTeam[i])).x, lblNameBadgeStyle.CalcSize(new GUIContent(redTeam[i])).y);
            GUI.Label(new Rect(Screen.width / 2  + (redTeamBadge.width * i) + redTeamBadge.width/2 - lblNameOffset.x / 2 + clockBG.width / 2, redTeamBadge.height / 2 + +lblNameOffset.y / 2, redTeamBadge.width, redTeamBadge.height), new GUIContent(redTeam[i]), lblNameBadgeStyle);


            if (currentTurn < 20 && currentTurn%10 == i)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 + (currentBadge.width * i) + clockBG.width / 2, currentBadge.height / 2 + 33, currentBadge.width, currentBadge.height), currentBadge);

            }

            if (nextTurn < 20 && nextTurn % 10 == i)
            {
                GUI.DrawTexture(new Rect(Screen.width / 2 + (nextBadge.width * i) + clockBG.width / 2, nextBadge.height / 2 + 33, nextBadge.width, nextBadge.height), nextBadge);

            }
        }
    }

    void DrawClock()
    {
        GUI.DrawTexture(new Rect(Screen.width / 2 - clockBG.width/2, 0, clockBG.width, clockBG.height), clockBG);

        Vector2 lblClockOffset = new Vector2(lblClockStyle.CalcSize(new GUIContent((int)countDown+"")).x, lblClockStyle.CalcSize(new GUIContent((int)countDown + "")).y);
        GUI.Label(new Rect(Screen.width / 2 -lblClockOffset.x/2, 8, lblClockOffset.x, lblClockOffset.y), new GUIContent((int)countDown + ""), lblClockStyle);

    }
}
