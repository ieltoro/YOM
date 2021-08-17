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
        ServerCalls.sc.ChangeScene("InputPing");
    }

    public void ConnectedToMiniGame(GameObject p)
    {
        print("Spawn player");
        manager.SpawnNewPlayer(p, 4);
    }
}
