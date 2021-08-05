using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PaintballServer : MonoBehaviour
{

    #region Setup

    YOMNetworkManager manager;
    ServerCalls sc;
    public List<PaintballNetwork> network;
    public List<PaintballPlayer> players;
    [SerializeField] Text connectedText;
    int pConnected;

    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ChangeScene(SceneManager.GetActiveScene().name);

    }

    public void ConnectedToMiniGame(GameObject p)
    {
        print("0");
        manager.SpawnNewPlayer(p, 6);
        pConnected++;
        if (manager.gamemode == 0)
        {
            connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.playersAliveAndConnected;
        }
        else
        {
            connectedText.text = pConnected.ToString();
        }
    }
    public void AllConnectedAndPressedStart()
    {

        StartCoroutine(StartgameCD());
    }
    #endregion
    #region Game

    [Header("Paintball")]
    [SerializeField] GameObject zonesPrefab;
    public List<Transform> spawnZonesPos;
    int currentZone, newZone;
    int zoneAmounts = 10;
    IEnumerator StartgameCD()
    {
        yield return new WaitForSeconds(1);
        foreach (PaintballPlayer p in players)
        {
            p.StartMoving(true);
        }
        sc.StartZone(true);

        yield return new WaitForSeconds(2);

        for (int i = 3; i >= 0; i--)
        {
            print(i);
            yield return new WaitForSeconds(1);
        }
    }

    #endregion

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame(manager.gamemode);
        SceneManager.LoadScene("Game");
    }
}
