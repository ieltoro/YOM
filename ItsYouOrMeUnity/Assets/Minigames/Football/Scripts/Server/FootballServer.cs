using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FootballServer : MonoBehaviour
{
    #region Setup

    [Header("Setup")]
    YOMNetworkManager manager;
    ServerCalls sc;
    [SerializeField] BattleshipTime timer;
    [SerializeField] GameObject ballPrefab;
    [SerializeField] GameObject[] fields;
    [SerializeField] GameObject startCanvas;
    int field;

    [Header("Players & Teams")]
    public List<FootballPlayer> players;
    [SerializeField] Text connectedText;
    int pConnected;

    [Header("Game")]
    public List<int> teamscore;
    public GameObject goalDisplay;


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
    }
    public void SpawnedPlayer(FootballPlayer p)
    {
        pConnected++;
        players.Add(p);
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
        print("Starting Football");
        SetupField();
    }
    void SetupField()
    {
        print(players.Count + " :Amount of players");
        if (players.Count == 1)
        {
            field = 0;
            for (int x = 2; x > 0; x--)
            {
                teamscore.Add(0);
                print("Add teamscore");
            }
            fields[field].SetActive(true);
            print("Activate field");
        }
        if (players.Count == 2)
        {
            field = 0;
            for (int x = 2; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 3)
        {
            field = 1;
            for (int x = 3; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 4)
        {
            int r = Random.Range(0, 2);
            if(r == 0)
            {
                field = 0;
                for (int x = 2; x > 0; x--)
                {
                    teamscore.Add(0);
                }
                fields[field].SetActive(true);
            }
            else
            {
                field = 2;
                for (int x = 4; x > 0; x--)
                {
                    teamscore.Add(0);
                }
                fields[field].SetActive(true);
            }
        }
        if (players.Count == 5)
        {
            field = 3;
            for(int x = 5; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 6)
        {
            field = 1;
            for (int x = 3; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 7)
        {
            field = 4;
            for (int x = 7; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 8)
        {
            field = 2;
            for (int x = 4; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 9)
        {
            field = 1;
            for (int x = 3; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        if (players.Count == 10)
        {
            field = 3;
            for (int x = 4; x > 0; x--)
            {
                teamscore.Add(0);
            }
            fields[field].SetActive(true);
        }
        SetupPlayers();
    }
    void SetupPlayers()
    {
        int pi = new int();

        foreach (FootballPlayer p in players)
        {
            p.SpawnPlayer(pi, fields[field].GetComponent<FootballFieldSetup>().GetSpawnPos());
            pi++;
            if(pi >= teamscore.Count)
            {
                pi = 0;
            }
        }
        StartCoroutine(StartGameCD());
    }
    IEnumerator StartGameCD()
    {
        startCanvas.SetActive(false);
        foreach (FootballPlayer p in players)
        {
            p.StartMovement(true);
        }
        yield return new WaitForSeconds(1);
        GameObject b = Instantiate(ballPrefab, new Vector3(0, 2, 0), new Quaternion(0, 0, 0, 0));
        b.GetComponent<Rigidbody>().useGravity = true;
        for (int i = 3; i >= 0; i--)
        {
            yield return new WaitForSeconds(1);
            print(i);
            if(i == 0)
            {
                print("0 So start the game");
                StartGame();
            }
        }
    }
    #endregion

    #region Play
    void StartGame()
    {
        print("Starting");
        foreach (FootballPlayer p in players)
        {
            p.myPlayer.canMove = true;
            print(p.playerID + " Can now move");
        }
    }
    public void TeamScoredOn(int i, GameObject ball)
    {
        foreach (FootballController g in ball.GetComponent<FootballTouchHolder>().playerTouch)
        {
            if (g.teamId != i)
            {
                AddScore(g.teamId, g.owner.GetComponent<FootballPlayer>().playerID);
                return;
            }
        }
        AddScore(i, "Own goal");
    }
    public void AddScore(int team, string scoredBy)
    {
        print("Scored by " + scoredBy);
        goalDisplay.SetActive(true);
        teamscore[team]++;
        StartCoroutine(HideGoalUI());
        StartCoroutine(SpawnBall());
    }
    IEnumerator HideGoalUI()
    {
        yield return new WaitForSeconds(2);
        goalDisplay.SetActive(false);
    }
    IEnumerator SpawnBall()
    {
        yield return new WaitForSeconds(Random.Range(1f, 3f));
        GameObject b = Instantiate(ballPrefab, new Vector3(0, 2, 0), new Quaternion(0, 0, 0, 0));
        b.GetComponent<Rigidbody>().useGravity = true;
    }

    #endregion

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame(manager.gamemode);
        SceneManager.LoadScene("Game");
    }
}