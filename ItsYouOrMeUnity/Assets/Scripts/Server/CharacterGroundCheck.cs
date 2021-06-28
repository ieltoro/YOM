using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGroundCheck : MonoBehaviour
{
    [SerializeField] PlayerMovement move;

    private void OnTriggerStay(Collider other)
    {
        move.grounded = true;
    }
    private void OnTriggerExit(Collider other)
    {
        move.grounded = false;
    }
}
