using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagCollision : MonoBehaviour
{
    public int points;
    public FlaggraberManager manager;
    bool grabed;
   
    private void OnTriggerEnter(Collider other)
    {
        if (grabed)
            return;

        grabed = true;
    }

}
