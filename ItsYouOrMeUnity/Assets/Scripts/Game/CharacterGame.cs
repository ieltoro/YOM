using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterGame : NetworkBehaviour
{
    #region Setup

    [SerializeField] GameObject charPrefab;
    [SerializeField] CharacterMovement character;
    [SerializeField] ClientGameSetup cgs;
    GameSetup gs;
    public string playerID;
    public int votes;
    public GameObject owner;

    private void Start()
    {
        if (hasAuthority)
        {
            FindObjectOfType<CharacterGameClient>().player = this;
            SetOwnerID();
            cgs = FindObjectOfType<ClientGameSetup>();
            return;
        }
        if (isServer)
        {
            gs = FindObjectOfType<GameSetup>();
            Transform pos = gs.GetSpawnPos();
            transform.position = pos.position;
            transform.rotation = pos.rotation;
            GameObject temp = Instantiate(charPrefab, transform);
            character = temp.GetComponent<CharacterMovement>();
            character.owner = gameObject;
            FindObjectOfType<GameSetup>().players.Add(gameObject);
            StartCharacterMovement();
            foreach(GameObject g in GameSaveHolder.gsh.players)
            {
                if(g.GetComponent<NetworkIdentity>().connectionToClient == connectionToClient)
                {
                    g.GetComponent<PlayerScript>().currentChild = gameObject;
                }
            }
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

    #endregion
    #region Client
    [TargetRpc]
    void RPC_StartCharacterMovement()
    {
        FindObjectOfType<CharacterGameClient>().StartController(true);
    }
    public void SendInput(Vector2 inp)
    {
        CMD_SendInput(inp);
    }
    public void Jump()
    {
        CMD_Jump();
    }

    [TargetRpc]
    void RPC_EnteredVote(bool b)
    {
        cgs.EnableVote(b);
    }
    [TargetRpc]
    void RPC_EnteredStore(bool b)
    {
        cgs.EnableShop(b);
    }
    #endregion
    #region Server

    [Command]
    void CMD_SendInput(Vector2 pos)
    {
        character.UpdateInput(pos);
    }
    [Command]
    void CMD_Jump()
    {
        character.Jumping();
    }

    #region game
    void StartCharacterMovement()
    {
        RPC_StartCharacterMovement();
    }
    public void EnteredVote(bool b)
    {
        RPC_EnteredVote(b);
    }
    public void EnteredStore(bool b)
    {
        RPC_EnteredStore(b);
    }
    #endregion
    #endregion
}
