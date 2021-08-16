using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lotbuild : MonoBehaviour
{
    [SerializeField] int zoneID;

    public void PressedThisLot()
    {
        FindObjectOfType<MyTownManager>().PressedBuyZone(zoneID, true);
    }

}
