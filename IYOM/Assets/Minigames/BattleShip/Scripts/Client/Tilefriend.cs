using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tilefriend : MonoBehaviour
{
    [SerializeField] int[] pos;
    [SerializeField] BattleshipTileSelected parent;
    int current;
    public void Check()
    {
        current++;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.tag == "Tile")
        {
            parent.CheckAround(pos[current-1], collision.GetComponent<BattleshipTileSelected>());
            
        }
    }
}
