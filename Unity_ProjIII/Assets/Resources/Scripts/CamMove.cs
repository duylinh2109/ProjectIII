using UnityEngine;

public class CamMove : MonoBehaviour
{
    float hTD = 20.0f;
    float dTD = 20.0f;
    

    public float offset;

    public Transform target;
    public CameraState cs;

    public enum CameraState
    {
        TopDown,
        SideScrolling
    } 

    private void Start()
    {
        cs = CameraState.TopDown;
        offset = 0;
    }
    
    private void Update()
    {
        if (cs.Equals(CameraState.TopDown))
            CameraTopDown();
        else
            CameraSideScrolling();
    }

    void CameraSideScrolling()
    {
        if (Input.GetKey(KeyCode.UpArrow) && offset >= 0.0f)
        {
            offset -= 1f * Time.deltaTime;
            this.transform.Translate(Vector3.forward * Time.deltaTime * 10);
        }
        else if (Input.GetKey(KeyCode.DownArrow) && offset < 2.0f)
        {
            offset += 1f * Time.deltaTime;
            this.transform.Translate(-Vector3.forward * Time.deltaTime * 10);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Translate(Vector3.left * Time.deltaTime * 10);
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Translate(Vector3.right * Time.deltaTime * 10);
        }
        else
        {
        }
    }

    void CameraTopDown()
    {
        transform.position = Vector3.Lerp(transform.position, new Vector3(target.position.x, target.position.y + hTD, target.position.z - dTD), 10);
        transform.LookAt(target.position);
    }

    void FixedUpdate()
    {
        
    }

    void LateUpdate()
    {
        
    }
}