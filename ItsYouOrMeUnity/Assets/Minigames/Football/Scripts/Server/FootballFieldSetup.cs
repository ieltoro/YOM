using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FootballFieldSetup : MonoBehaviour
{
    public List<Transform> spawnPos;
    int i;

    public Transform GetSpawnPos()
    {
        i++;
        return spawnPos[i-1];
    }

}
