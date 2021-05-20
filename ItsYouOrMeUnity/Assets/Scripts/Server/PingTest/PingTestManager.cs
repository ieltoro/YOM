using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingTestManager : MonoBehaviour
{
    YOMNetworkManager manager;
    ServerCalls sc;
   
    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ConnectedToMiniGame("PingTest");
    }

    public void ConnectedToRed(GameObject p)
    {
        manager.SpawnNewPlayer(p, 3);
    }
}
