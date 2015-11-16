using UnityEngine;
using System.Collections;

public class NewBehaviourScript : MonoBehaviour {

    private float clickDelay;
    private int timeClicked;
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
        clickDelay += Time.deltaTime;
        if (Input.GetMouseButtonDown(0) && timeClicked.Equals(0))
        {
            timeClicked = 1;
            clickDelay = 0;
        }
        else if (Input.GetMouseButtonDown(0) && timeClicked.Equals(1))
        {
            if (clickDelay < 0.2f)
            {
                timeClicked = 2;
                clickDelay = 0;
            }
        }
        

        if (clickDelay > 0.2f)
        {
            if (timeClicked.Equals(2))
                //speed += 20;
            timeClicked = 0;
        }
    }
}
