using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class CharacterGame : NetworkBehaviour
{
    #region Setup

    [SerializeField] GameObject charPrefab;
    [SerializeField] CharacterMovement character;
    GameSetup gs;
    public string playerID;
    public GameObject owner;

    private void Start()
    {
        if (hasAuthority)
        {
            FindObjectOfType<CharacterGameClient>().player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            gs = FindObjectOfType<GameSetup>();
            Transform pos = gs.GetSpawnPos();
            transform.position = pos.position;
            transform.rotation = pos.rotation;
            GameObject temp = Instantiate(charPrefab, this.transform);
            character = temp.GetComponent<CharacterMovement>();
            character.owner = gameObject;
            FindObjectOfType<GameSetup>().players.Add(gameObject);
            StartCharacterMovement();
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

    #endregion
    #endregion
}
