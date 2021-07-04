using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    GameSaveHolder save;
    ServerCalls sc;
    public List<GameObject> players;
    [SerializeField] Timerhandler time;
    [SerializeField] TransitionSize trans;
    [SerializeField] GameObject background;
    [Tooltip(" 0 = roundstart \n 1 = ChooseMinigame")]
    [SerializeField] GameObject[] UI;
    public static int startPositionIndex;
    public List<Transform> startPositions;

    #region Start
    private void Start()
    {
        GetMinigames();
        //sc = FindObjectOfType<ServerCalls>();
        //save = GameSaveHolder.gsh;
        //foreach (GameObject g in save.players)
        //{
        //    YOMNetworkManager.manager.SpawnNewPlayer(g, 2);
        //}
        //StartCoroutine(StartGameCD());
    }
    IEnumerator StartGameCD()
    {
        yield return new WaitForSeconds(1);
        trans.TransitionIn();
    }
    public Transform GetSpawnPos()
    {
        Transform pos = startPositions[startPositionIndex];
        startPositionIndex++;
        return pos;
    }

    #endregion

    int voted;
    public void PlayerHaveVoted()
    {
        voted++;
        if(voted == players.Count)
        {
            print("All have voted");
            CalculateStanding();
        }
    }
    public List<GameObject> order, drawplayers;
    int upCheck;
    bool added, draw;
    void CalculateStanding()
    {
        for (int i = 0; i < players.Count; i++)
        {
            if (order.Count != 0)
            {
                if (order[i - 1].GetComponent<CharacterGame>().votes <= players[i].GetComponent<CharacterGame>().votes) // Kolla sista positionen i order 
                {
                    order.Add(players[i]);
                }
                else
                {
                    upCheck = 0;
                    foreach (GameObject g in order)
                    {
                        if (g.GetComponent<CharacterGame>().votes < players[i].GetComponent<CharacterGame>().votes && !added)
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
        print("All done in order");
        #region Check if draw
        int last = order[order.Count - 1].GetComponent<CharacterGame>().votes;
        if (order.Count >= 2)
        {
            foreach(GameObject g in order)
            {
                if(g.GetComponent<CharacterGame>().votes == last)
                {
                    drawplayers.Add(g);
                }
            }
            if (drawplayers.Count > 1)
            {
                draw = true;
            }
        }

        StartCoroutine(DisplayResult());
        #endregion
    }

    IEnumerator DisplayResult()
    {
        yield return new WaitForSeconds(2);
        GetMinigames();
    }

    #region Minigames

    [Header("Minigames")]
    [SerializeField] GameObject minigamesCanvas;
    [SerializeField] List<Minigames> mg;
    [SerializeField] MinigameDisplay miniGDisplay1, miniGDisplay2;
    Minigames mg1, mg2;

    void GetMinigames()
    {
        List<Minigames> list = new List<Minigames>();
        foreach(Minigames g in mg)
        {
            list.Add(g);
        }
        mg1 = list[Random.Range(0, list.Count)];
        list.Remove(mg1);
        mg2 = list[Random.Range(0, list.Count)];
        print(mg1.nameMinigame + "  ,  " + mg2.nameMinigame);


        miniGDisplay1.SetMinigame(mg1);
        miniGDisplay2.SetMinigame(mg2);
        minigamesCanvas.SetActive(true);
    }
    #endregion
}
