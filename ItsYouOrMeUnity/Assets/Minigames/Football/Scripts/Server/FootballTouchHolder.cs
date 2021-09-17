using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballTouchHolder : MonoBehaviour
{
    public bool scored;
    public List<FootballController> playerTouch;

    private void Start()
    {
        FindObjectOfType<MultiCamera>().targets.Add(this.transform);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.transform.tag == "Goal")
        {
            scored = true;
        }
        if(!scored && collision.transform.tag == "Player")
        {
            playerTouch.Insert(0, collision.transform.GetComponent<FootballController>());
        }
    }

}
