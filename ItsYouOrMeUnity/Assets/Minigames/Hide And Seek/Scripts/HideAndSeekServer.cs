using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekServer : MonoBehaviour
{
    YOMNetworkManager manager;
    ServerCalls sc;
    public List<GameObject> players;

    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        ServerCalls.sc.ConnectedToMiniGame("InputPing");
    }

    public void ConnectedToRed(GameObject p)
    {
        print("Spawn player");
        manager.SpawnNewPlayer(p, 4);
    }
}
