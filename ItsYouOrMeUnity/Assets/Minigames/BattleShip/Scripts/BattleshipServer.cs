using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class BattleshipServer : MonoBehaviour
{
    YOMNetworkManager manager;
    ServerCalls sc;
    public List<BattleshipPlayer> players;
    int pConnected;
    [SerializeField] Text connectedText;

    void Start()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ConnectedToMiniGame(SceneManager.GetActiveScene().name);
    }
    public void ConnectedToMiniGame(GameObject p)
    {
        manager.SpawnNewPlayer(p, 3);
        pConnected++;
        connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.playersAliveAndConnected;
    }
    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame();
        SceneManager.LoadScene("Game");
    }

}
