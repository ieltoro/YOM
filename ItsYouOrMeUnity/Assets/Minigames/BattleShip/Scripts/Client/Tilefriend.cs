using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilefriend : MonoBehaviour
{
    [SerializeField] int pos;
    [SerializeField] BattleshipTileSelected parent;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        print("Collision");
        if(collision.tag == "Tile")
        {
            parent.CheckAround(pos, collision.GetComponent<BattleshipTileSelected>());
        }
    }
}
