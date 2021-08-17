using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipCollision : MonoBehaviour
{
    [SerializeField] Battleship ship;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Bomb")
        {
            GetComponent<BoxCollider>().enabled = false;
            ship.HitOnShip();
        }
    }
}
