using UnityEngine;
using System.Collections;

public class Bullet : MonoBehaviour {

    float shootAngle;
    float shootForce;

    public GameObject hitEffect;

	void Start () {
	
	}
	
	void Update () {
	
	}

    void OnCollisionEnter(Collision col)
    {
        if(col.transform.tag.Equals("BlueTeam")|| col.transform.tag.Equals("RedTeam"))
        {
            GameObject go = Instantiate(hitEffect, col.transform.GetComponent<PlayerBehaviour>().hpAnchor.position, Quaternion.identity) as GameObject; 
        }
    }
}
