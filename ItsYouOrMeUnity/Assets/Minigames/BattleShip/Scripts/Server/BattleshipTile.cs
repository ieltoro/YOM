using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipTile : MonoBehaviour
{
    public bool free;
    public bool bombed;
    public MeshRenderer tileRend;

    public void HitThisTile()
    {

    }
    public void ResetTile()
    {
        bombed = false;
    }
}
