using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FootballServer : MonoBehaviour
{
    #region Setup

    [SerializeField] BattleshipTime timer;
    YOMNetworkManager manager;
    ServerCalls sc;
    public List<BattleshipPlayer> players;
    [SerializeField] Text connectedText;
    [SerializeField] BattleshipWaterGrid grid;
    [SerializeField] GameObject shipPrefab;
    public List<GameObject> ships;
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
        manager.SpawnNewPlayer(p, 4);
        pConnected++;
        if(manager.gamemode == 0)
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

    }

    #endregion

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame(manager.gamemode);
        SceneManager.LoadScene("Game");
    }
}
