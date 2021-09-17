using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballGoal : MonoBehaviour
{
    FootballServer setup;
    [SerializeField] int goalId;

    private void Start()
    {
        setup = FindObjectOfType<FootballServer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Ball")
        {
            other.transform.GetComponent<FootballTouchHolder>().scored = true;
            setup.TeamScoredOn(goalId, other.gameObject);
            FindObjectOfType<MultiCamera>().targets.Remove(other.transform);
            Destroy(other.gameObject, 1);
        }
    }
}
