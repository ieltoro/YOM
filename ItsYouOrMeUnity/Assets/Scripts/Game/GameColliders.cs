using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameColliders : MonoBehaviour
{
    [SerializeField] bool store;

    private void OnTriggerEnter(Collider other)
    {
        if(other.transform.tag == "Character")
        {
            if (store)
            {
                other.GetComponent<CharacterMovement>().owner.GetComponent<CharacterGame>().EnteredStore(true);
            }
            else
            {
                other.GetComponent<CharacterMovement>().owner.GetComponent<CharacterGame>().EnteredVote(true);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.transform.tag == "Character")
        {
            if (store)
            {
                other.GetComponent<CharacterMovement>().owner.GetComponent<CharacterGame>().EnteredStore(false);
            }
            else
            {
                other.GetComponent<CharacterMovement>().owner.GetComponent<CharacterGame>().EnteredVote(false);
            }
        }
    }

}
