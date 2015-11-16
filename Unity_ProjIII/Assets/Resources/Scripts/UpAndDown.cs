using UnityEngine;

public class UpAndDown : MonoBehaviour
{
    public bool isUp;
    public bool isLeft;

    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (this.isUp)
        {
            if (this.transform.position.y < 13)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1 * Time.deltaTime, this.transform.position.z);
            }
            else
                this.isUp = false;
        }
        else
        {
            if (this.transform.position.y > 3)
            {
                this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y - 1 * Time.deltaTime, this.transform.position.z);
            }
            else
                this.isUp = true;
        }

        if (this.isLeft)
        {
            if (this.transform.position.x < 260)
            {
                this.transform.position = new Vector3(this.transform.position.x + 1 * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
            else
                this.isLeft = false;
        }
        else
        {
            if (this.transform.position.x > 248)
            {
                this.transform.position = new Vector3(this.transform.position.x - 1 * Time.deltaTime, this.transform.position.y, this.transform.position.z);
            }
            else
                this.isLeft = true;
        }
    }
}