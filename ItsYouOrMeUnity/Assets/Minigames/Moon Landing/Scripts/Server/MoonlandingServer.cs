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
        sc.ChangeScene(SceneManager.GetActiveScene().name);

        foreach (GameObject g in GameSaveHolder.gsh.players)
        {
            if (g.GetComponent<PlayerScript>().connected && g.GetComponent<PlayerScript>().hp > 0)
                YOMNetworkManager.manager.SpawnNewPlayer(g, 3);
        }
    }
    public void ConnectedToMiniGame(MoonlandingPlayer p)
    {
        pConnected++;
        players.Add(p);
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
        windDirection = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
    }
    public void StartFlying()
    {
        foreach(MoonlandingPlayer g in players)
        {
            g.StartFlying();
        }
        StartCoroutine(StartGameCD());
    }
    IEnumerator StartGameCD()
    {
        print("3");
        yield return new WaitForSeconds(1);
        print("2");
        yield return new WaitForSeconds(1);
        print("1");
        yield return new WaitForSeconds(1);
        print("0");
    }
    #endregion
}
