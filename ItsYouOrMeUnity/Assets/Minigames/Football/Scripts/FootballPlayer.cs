using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class FootballPlayer : NetworkBehaviour
{
    [SerializeField] GameObject prefab;
    public FootballController myPlayer;
    [SerializeField] FootballClient client;
    public string playerID;
   
    void Start()
    {
        print(11);
        if (hasAuthority)
        {
            print("Is mine");
            client = FindObjectOfType<FootballClient>();
            client.player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            FindObjectOfType<FootballServer>().SpawnedPlayer(this);
            return;
        }
        Destroy(gameObject);
    }

    void SetOwnerID()
    {
        playerID = ClientSaveGame.csg.playerID;
        CMD_SetOwnerName(playerID);
    }
    [Command]
    void CMD_SetOwnerName(string pID)
    {
        playerID = pID;
        foreach (GameObject g in GameSaveHolder.gsh.players)
        {
            if (g.GetComponent<PlayerScript>().id == playerID)
            {
                owner = g;
                return;
            }
        }
    }

    #region Server
    public GameObject owner;
    public void SpawnPlayer(int _teamId, Transform _pos)
    {
        GameObject temp = Instantiate(prefab, _pos.position, _pos.rotation);
        temp.GetComponent<FootballController>().teamId = _teamId;
        myPlayer = temp.GetComponent<FootballController>();
        myPlayer.owner = this;
    }
    [Command]
    void CMD_PressedStart()
    {
        FindObjectOfType<FootballServer>().AllConnectedAndPressedStart();
    }
    [Command]
    void CMD_RecievedInput(Vector2 input)
    {
        if(myPlayer != null)
        {
            myPlayer.InputRecieved(input);
        }
    }
    public void StartMovement(bool answer)
    {
        RPC_StartMovement(answer);
    }
    #endregion
    #region Client
    public void PressedStart()
    {
        CMD_PressedStart();
    }
    [TargetRpc]
    void RPC_StartMovement(bool answer)
    {
        client.StartMoving(answer);
    }
    public void RecievedInput(Vector2 input)
    {
        CMD_RecievedInput(input);
    }

    #endregion
}
