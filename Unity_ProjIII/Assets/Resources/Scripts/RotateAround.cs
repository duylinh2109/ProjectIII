using UnityEngine;

public class RotateAround : MonoBehaviour
{
    private void Start()
    {
    }

    private void Update()
    {
        this.transform.rotation *= Quaternion.Euler(0, 25 * Time.deltaTime, 0);
    }
}