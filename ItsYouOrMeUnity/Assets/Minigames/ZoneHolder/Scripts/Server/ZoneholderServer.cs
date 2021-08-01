using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ZoneholderServer : MonoBehaviour
{
    #region Setup

    YOMNetworkManager manager;
    ServerCalls sc;
    public List<ZoneholderPlayer> players;
    public List<ZoneholderController> controllers;
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
        manager.SpawnNewPlayer(p, 5);
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
    #region GAME

    [Header("Zones")]
    [SerializeField] GameObject zonesPrefab;
    public Transform[] spawnZonesPos;
    int currentZone, newZone;
    IEnumerator StartgameCD()
    {
        yield return new WaitForSeconds(1);

        foreach (ZoneholderPlayer p in players)
        {
            p.StartGame();
        }
        yield return new WaitForSeconds(2);
        print("3");
        yield return new WaitForSeconds(1);
        print("2");
        yield return new WaitForSeconds(1);
        print("1");
        yield return new WaitForSeconds(1);
        print("0");

        NextZone();
    }
    public void NextZone()
    {

        StartCoroutine(ActivateNextZone());
    }
    IEnumerator ActivateNextZone()
    {
        float f = Random.Range(1f, 3f);
        yield return new WaitForSeconds(f);
    }
    #endregion
    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame(manager.gamemode);
        SceneManager.LoadScene("Game");
    }
}

