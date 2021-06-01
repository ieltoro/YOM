using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerScript : NetworkBehaviour
{
    //public static PlayerScript ps;
    [SyncVar]
    public int playerNR;
    public string playerName;
    public bool pReady;
    public string id;
    public int votesBalance = 100;
    public int votesAmount;
    public int hp = 3;
    public bool leader;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
        if (isServer)
            save = GameSaveHolder.gsh;

    }
    private void Start()
    {
        if (hasAuthority)
        {
            print("You are the owner");
            ClientSaveGame.csg.localPlayer = this.gameObject;

            gameObject.name = "MyPrefab";
            playerName = ClientSaveGame.csg.playerName;
            characterCosmetics = ClientSaveGame.csg.cosmetic;
            id = ClientSaveGame.csg.playerID;
            gameObject.tag = "Player";
            ConnectUpdate(playerName, characterCosmetics, id);
            if (!leader)
                FindObjectOfType<ClientLobby>().ChangeUi(3);
        }
        if (isServer)
        {
            if (save == null)
                save = GameSaveHolder.gsh;
            ls = FindObjectOfType<LobbySetup>();

            manager = FindObjectOfType<YOMNetworkManager>();
            save.players.Add(this.gameObject);
            if (save.leader == null)
            {
                RPC_AssignLeader();
                save.leader = this.gameObject;
                playerNR = 0;
            }
        }
        if (!isServer && !hasAuthority)
        {
            ClientSaveGame.csg.players.Add(this.gameObject);
        }

        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    #region Client

    ClientGameSetup cgs;

    #region Start

    public void PlayerReady()
    {
        CMD_Ready();
    }
    [TargetRpc]
    public void RPC_AssignLeader()
    {
        print("Leader");
        leader = true;
        FindObjectOfType<ClientLobby>().ChangeUi(2);
    }
    [ClientRpc]
    void RPC_RecievedplayerInfo(string name)
    {
        playerName = name;
    }
    [TargetRpc]
    void RPC_UpdateOwner(bool l, string scene)
    {
        print("You are the owner");
        ClientSaveGame.csg.localPlayer = this.gameObject;
        gameObject.name = "MyPrefab";
        gameObject.tag = "Player";
        leader = l;
        string load = scene + " Phone";
        SceneManager.LoadScene(load);
    }
    #endregion
    #region Vote

    public void SendVotes(int amount)
    {
        CMD_Votes(amount);
    }
    [TargetRpc]
    void VotingTimeClient(int balance)
    {
        if (cgs == null)
            cgs = FindObjectOfType<ClientGameSetup>();
        votesBalance = balance;
        cgs.VoteStart(votesBalance);
    }
    [ClientRpc]
    void RPC_leastvotes(int h)
    {
        print("Darn, you used the least votes");
        hp = h;
        if (isLocalPlayer && hp == 0)
        {
            ClientSaveGame.csg.dead = true;
            cgs.dead = true;
        }
    }

    
    #endregion
    #region minigame

    public void MinigameIVotedfor(int i)
    {
        MiniGameVote(i);
    }
    public void MinigameConnectedTo(int i)
    {
        ConnectedToMinigame(i);
    }
    #endregion
    #region minigame Duel

    public void RPC_VoteMiniGameDuel(int amount, string g1, string g2)
    {
       cgs.StartMinigameVote(g1, g2);
    }
    public void VotedMiniGameDuel(int nr)
    {
        CMD_VotedMiniGameDuel(nr);
    }
    #endregion
    #region Chat



    public void SendMsg(string text, string sender)
    {

    }

    [TargetRpc]
    public void RecievedMsg(string mxg, string from)
    {

    }
    #endregion
    #endregion


    ///////////////////////////////////////////////////


    #region Server

    LobbySetup ls;
    GameSetup gs;
    GameSaveHolder save;
    YOMNetworkManager manager;
    [SerializeField] GameObject characterPrefab;
    public GameObject myPlayer;
    public bool connected;
    public int characterCosmetics;

    #region Setup

    [Command]
    void ConnectUpdate(string pname, int cosmeticNr, string pid)
    {
        connected = true;
        playerName = pname;
        id = pid;
        characterCosmetics = cosmeticNr;
        this.gameObject.name = pname + "Prefab";
        ls.PlayerJoinedLobby(pname);
        myPlayer = Instantiate(characterPrefab, this.gameObject.transform);
        myPlayer.GetComponent<CharacterManager>().ChangeCosmetics(characterCosmetics);
    }
    public void AssignAsLeader()
    {
        RPC_AssignLeader();
    }
    [Command]
    public void CMD_Ready()
    {
        pReady = true;
        ls.PlayerReady();
    }
    public void SendInfoToLobby()
    {
        RPC_RecievedplayerInfo(playerName);
    }
    public void SetHP(int i)
    {
        hp = i;
    }
    public void UpdateOwner()
    {
        connected = true;
        string sceneName = SceneManager.GetActiveScene().name;
        RPC_UpdateOwner(leader, sceneName);
    }

    #endregion
    #region game
    public void SetSpawn(int nr)
    {
        if (gs == null)
            gs = FindObjectOfType<GameSetup>();
        playerNR = nr;
        Transform temp = gs.startPositions[nr];
        transform.position = temp.position;
        transform.rotation = temp.rotation;
    }
    public void SetupGameStart(GameSetup setup)
    {
        if(gs == null)
            gs = setup;
    }
    
    #region Vote
    public void TimeToVote()
    {
        VotingTimeClient(votesBalance);
    }
    [Command]
    void CMD_Votes(int nr)
    {
        votesAmount = nr;
        votesBalance -= nr;
        if (votesBalance < 0)
            votesBalance = 0;
        gs.PlayerHaveVoted();
    } // recieved vote from client
    public void VotedLeast()
    {
        hp--;
        if(hp == 0)
        {

        }
        RPC_leastvotes(hp);
    }
    public void VotedDraw()
    {

    }
    #endregion
    #region MiniGame

    void MiniGameVote(int i)
    {
        gs.MinigameVotedFor(i);
    }
    [Command]
    void ConnectedToMinigame(int i)
    {
        if(i == 0)// BattleShip
        {
            FindObjectOfType<BattleshipServer>().ConnectedToMiniGame(this.gameObject);
        }
        if(i == 1)// PingTest
        {
            FindObjectOfType<HideAndSeekServer>().ConnectedToMiniGame(this.gameObject);
        }
    }
    public void AddVoteBalance(int add)
    {
        votesBalance += add;
        if (votesBalance > 100)
            votesBalance = 100;
    } // Add score from minigame

    public void VoteMiniGameDuel(int x, string gName1, string gName2)
    {
        RPC_VoteMiniGameDuel(x, gName1, gName2);
    }
    [Command]
    void CMD_VotedMiniGameDuel(int x)
    {
        gs.RecievedDuelMiniGameVote(x);
    }
    #endregion

    #endregion
    #endregion
}
