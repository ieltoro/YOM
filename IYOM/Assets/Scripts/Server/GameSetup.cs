using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    GameSaveHolder save;
    ServerCalls sc;
    public List<CharacterGame> players;
    [SerializeField] Timerhandler time;
    [SerializeField] TransitionSize trans;
    [SerializeField] GameObject background;
    [Tooltip(" 0 = roundstart \n 1 = ChooseMinigame")]
    [SerializeField] GameObject[] UI;
    public static int startPositionIndex;
    public List<Transform> startPositions;
    [SerializeField] Text voteAmountText;

    #region Start
    private void Start()
    {
        sc = FindObjectOfType<ServerCalls>();
        save = GameSaveHolder.gsh;
        foreach (GameObject g in save.players)
        {
            if(g.GetComponent<PlayerScript>().connected && g.GetComponent<PlayerScript>().hp > 0)
                YOMNetworkManager.manager.SpawnNewPlayer(g, 2);
        }
    }
    public void Addplayer(CharacterGame obj)
    {
        players.Add(obj);
        voteAmountText.text = voted.ToString() + " / " + players.Count;
        if (players.Count == GameSaveHolder.gsh.playersAliveAndConnected)
        {
            StartCoroutine(StartGameCD());
        }
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
        voteAmountText.text = voted.ToString() + " / " + players.Count;
        if (voted == players.Count)
        {
            voted = 0;
            print("All have voted");
            CalculateStanding();
        }
    }
    public List<CharacterGame> order, drawplayers;
    int upCheck;
    bool added, draw;
    void CalculateStanding()
    {
        foreach(CharacterGame g in players)
        {
            g.character.StopMoving();
        }
        for (int i = 0; i < players.Count; i++)
        {
            if (order.Count != 0)
            {
                if (order[i - 1].votes <= players[i].votes) // Kolla sista positionen i order 
                {
                    order.Add(players[i]);
                }
                else
                {
                    upCheck = 0;
                    foreach (CharacterGame g in order)
                    {
                        if (g.votes < players[i].votes && !added)
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
        if (order.Count >= 2)
        {
            int last = order[order.Count - 1].votes;
            foreach (CharacterGame g in order)
            {
                if(g.votes == last)
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
    [SerializeField] MinigameDisplay[] miniGDisplay;
    private int[] voteCount = new int[2];
    [SerializeField] Text[] countText;
    Minigames mg1, mg2, winner;

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


        miniGDisplay[0].SetMinigame(mg1);
        miniGDisplay[1].SetMinigame(mg2);
        minigamesCanvas.SetActive(true);
        sc.SendMiniGames(mg1.nameMinigame, mg2.nameMinigame);
    }
    public void VotedMinigame(int nr)
    {
        voted++;
        voteCount[nr]++;
        countText[nr].text = voteCount[nr].ToString();
        
        if(voteCount[0] > players.Count/2)
        {
            if(players.Count == 3|| players.Count == 5 || players.Count == 7 || players.Count == 9)
            {
                if (voteCount[0] > (players.Count / 2) +1)
                {
                    VotingMiniDone(0);
                }
            }
            else
            {
                VotingMiniDone(0);
            }

        }
        if (voteCount[1] > players.Count / 2)
        {
            if (players.Count == 3 || players.Count == 5 || players.Count == 7 || players.Count == 9)
            {
                if (voteCount[0] > (players.Count / 2) + 1)
                {
                    VotingMiniDone(1);
                }
            }
            else
            {
                VotingMiniDone(1);
            }
        }
        if (voted == players.Count)
        {
            if(voteCount[0] > voteCount[1])
            {
                VotingMiniDone(0);
            }
            if (voteCount[0] < voteCount[1])
            {
                VotingMiniDone(1);
            }
            if(voteCount[0] == voteCount[1])
            {
                VotingMiniDone(Random.Range(0, 2));
            }
        }
    }

    void VotingMiniDone(int w)
    {
        if(w == 0)
        {
            winner = mg1;
        }
        if (w == 1)
        {
            winner = mg2;
        }
        StartCoroutine(StartMinigame());
    }

    IEnumerator StartMinigame()
    {
        trans.TransitionOut();
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(winner.name);
    }
    #endregion
}
