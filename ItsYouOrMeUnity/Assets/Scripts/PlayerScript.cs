using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerScript : NetworkBehaviour
{
    //public static PlayerScript ps;
    YOMNetworkManager manager;
    [SyncVar]
    public int playerNR;
    public string playerName;
    public string id;
    public bool pReady;
    public int votesBalance = 100;
    public int votesAmount;
    public int hp = 3;
    public bool leader;
    public int gamemode;
    public int characterCosmetics;
    public bool connected;

    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        if (hasAuthority)
        {
            print("You are the owner");
            gameObject.name = "MyPrefab";
            gameObject.tag = "Player";
            ClientSaveGame.csg.localPlayer = this.gameObject;
            playerName = ClientSaveGame.csg.playerName;
            characterCosmetics = ClientSaveGame.csg.cosmetic;
            id = ClientSaveGame.csg.playerID;
            ConnectUpdate(playerName, characterCosmetics, id);
            if (!leader)
                FindObjectOfType<ClientLobby>().ChangeUi(-1);
        }
        if (isServer)
        {
            manager = FindObjectOfType<YOMNetworkManager>();
            manager.SpawnNewPlayer(gameObject, 1);
            playerNR = GameSaveHolder.gsh.GetPNR();
            GameSaveHolder.gsh.players.Add(this.gameObject);
            if (GameSaveHolder.gsh.leader == null)
            {
                RPC_AssignLeader();
                GameSaveHolder.gsh.leader = this.gameObject;
            }
        }
        if (!isServer && !hasAuthority)
        {
            ClientSaveGame.csg.players.Add(this.gameObject);
        }

        gameObject.transform.position = new Vector3(0, 0, 0);
    }

    #region Client
    
    [ClientRpc]
    void RPC_RecievedplayerInfo(string name)
    {
        playerName = name;
    }
    [TargetRpc]
    public void RPC_AssignLeader()
    {
        print("Leader");
        leader = true;
        FindObjectOfType<ClientLobby>().ChangeUi(-1);
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
    [TargetRpc]
    void RPC_UpdateAmountsBalanceOnClients(int amount)
    {
        votesBalance = amount;
        
    }
    public void VotesCasted(int amount)
    {
        votesBalance -= amount;
        CMD_VotesCasted(amount);
    }
    #endregion


    ///////////////////////////////////////////////////


    #region Server
    public GameObject currentChild;
    public void AssignAsLeader()
    {
        RPC_AssignLeader();
    }
    public void SendInfoToLobby()
    {
        RPC_RecievedplayerInfo(playerName);
    }
    public void UpdateOwner()
    {
        if(connected == false)
        {
            GameSaveHolder.gsh.playersAliveAndConnected++;
        }
        connected = true;
        string sceneName = SceneManager.GetActiveScene().name;
        RPC_UpdateOwner(leader, sceneName);
    }
    public void SetHP(int i)
    {
        hp = i;
    }
    [Command]
    void ConnectUpdate(string pname, int cosmeticNr, string pid)
    {
        connected = true;
        playerName = pname;
        id = pid;
        characterCosmetics = cosmeticNr;
        this.gameObject.name = pname + "Prefab";
        FindObjectOfType<LobbySetup>().PlayerJoinedLobby(pname);
    }

    public void UpdateAmountsBalanceOnClients()
    {
        RPC_UpdateAmountsBalanceOnClients(votesBalance);
    }
    [Command]
    void CMD_VotesCasted(int amount)
    {
        votesAmount = amount;
        votesBalance -= votesAmount;
        currentChild.GetComponent<CharacterGame>().votes = amount;
        FindObjectOfType<GameSetup>().PlayerHaveVoted();
    }
    #endregion

}
