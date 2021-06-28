using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FlaggraberManager : MonoBehaviour
{
    #region Setup

    public List<FlagPlayer> players;
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
    #region Play

    [Header("Play")]
    [SerializeField] GameObject platformPrefab;
    public List<FlagPlatform> platforms;
    public List<FlagPlayer> order;

    void SetupPlayarea()
    {
        for(int i = 0; i < pConnected; i++)
        {
            //GameObject temp = Instantiate(flagPrefab, flagPositions[i]);
            //temp.GetComponent<FlagManager>().manager = this;
            //temp.GetComponent<FlagManager>().points = pConnected - i;
            //flags.Add(temp.GetComponent<FlagManager>());
        }
    }

    
    int upCheck;
    bool added;
    void GetOrder()
    {
        for(int i = 0; i < players.Count; i++)
        {
            if (order.Count != 0)
            {
                if(order[i - 1].score < players[i].score) // Kolla sista positionen i order 
                {
                    order.Add(players[i]);
                }
                else
                {
                    upCheck = 0;
                    foreach (FlagPlayer p in order)
                    {
                        if(p.score < players[i].score && !added)
                        {
                            order.Insert(upCheck, players[i]);
                            added = true;
                        }
                        upCheck++;
                    }
                }
            }
            else
            {
                order.Add(players[i]);
            }
        }
    }
    #endregion
}
