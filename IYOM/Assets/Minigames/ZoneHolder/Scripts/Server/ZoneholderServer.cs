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
    public List<Transform> spawnZonesPos;
    int currentZone, newZone;
    int zoneAmounts = 10;
    IEnumerator StartgameCD()
    {
        yield return new WaitForSeconds(1);
        foreach (ZoneholderController p in controllers)
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
        NextZone();
    }
    public void NextZone()
    {
        foreach (ZoneholderController p in controllers)
        {
            p.GetComponent<ZoneholderController>().InsideZone(false);
        }
        if (zoneAmounts == 10)
        {
            newZone = Random.Range(0, spawnZonesPos.Count);
        }
        else
        {
            if (zoneAmounts == -1)
            {
                EndGame();
                return;
            }
            else
            {
                newZone = Random.Range(0, spawnZonesPos.Count);
                if (Vector3.Distance(spawnZonesPos[currentZone].position, spawnZonesPos[newZone].position) > 10)
                {
                    print("Continue, new zone is far enough");
                }
                else
                {
                    print("Zone aint far enough, redo");
                    NextZone();
                    return;
                }
            }
        }

        currentZone = newZone;
        Instantiate(zonesPrefab, spawnZonesPos[currentZone].position, spawnZonesPos[currentZone].rotation);
        zoneAmounts--;
        StartCoroutine(ActivateNextZone());
    }
    void GrabSpawn()
    {

        
    }
    IEnumerator ActivateNextZone()
    {
        float f = Random.Range(1f, 3f);
        yield return new WaitForSeconds(f);
    }
    void EndGame()
    {

    }
    #endregion
    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame(manager.gamemode);
        SceneManager.LoadScene("Game");
    }
}

