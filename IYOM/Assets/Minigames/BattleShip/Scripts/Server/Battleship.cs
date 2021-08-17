using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Battleship : MonoBehaviour
{
    public BattleshipPlayer owner;
    [SerializeField] MeshRenderer ship;
    int hp = 2;

    public void SetInfo(BattleshipPlayer name)
    {
        owner = name;
    }
    public void HitOnShip()
    {
        hp--;
        if(hp == 0)
        {
            owner.ShipSunken(gameObject);
            ship.enabled = true;
        }
    }
}
