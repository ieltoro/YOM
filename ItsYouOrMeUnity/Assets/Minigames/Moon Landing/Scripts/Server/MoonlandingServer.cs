using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MoonlandingServer : MonoBehaviour
{
    #region Setup

    public List<MoonlandingPlayer> players;
    [SerializeField] Text connectedText;
    YOMNetworkManager manager;
    ServerCalls sc;
    int pConnected;

    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ConnectedToMiniGame(SceneManager.GetActiveScene().name);
    }
    public void ConnectedToMiniGame(GameObject p)
    {
        manager.SpawnNewPlayer(p, 4);
        pConnected++;
        connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.playersAliveAndConnected;
        if (pConnected == GameSaveHolder.gsh.playersAliveAndConnected)
        {
            AllConnectedAndPressedStart();
        }
    }
    public void AllConnectedAndPressedStart()
    {
        SetupPlayarea();
    }
    #endregion
    #region Game

    [SerializeField] Vector2 windDirection;

    public void SetupPlayarea()
    {

    }

    void Update()
    {

    }
    #endregion
}
