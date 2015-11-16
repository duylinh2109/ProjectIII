using UnityEngine;

public class Unit : MonoBehaviour
{
    // Use this for initialization
    private void Start()
    {
        Object[] o = Resources.LoadAll("Animations/CrystalMaiden/", typeof(GameObject));
        foreach (GameObject ac in o)
        {
            this.GetComponent<Animation>().AddClip(ac.GetComponent<Animation>().GetClip("Take 001"), ac.name);
        }
        this.GetComponent<Animation>().Play("Crystal Maiden_Attack1");
    }

    // Update is called once per frame
    private void Update()
    {
    }
}