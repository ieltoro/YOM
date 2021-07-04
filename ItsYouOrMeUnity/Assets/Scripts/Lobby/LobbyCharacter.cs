using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class LobbyCharacter : NetworkBehaviour
{
    [SerializeField] GameObject charPrefab;
    public CharacterMovement character;
    public string playerID;

    private void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            FindObjectOfType<LobbyClient>().player = this;
            FindObjectOfType<LobbyClient>().StartController();
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            GameObject temp = Instantiate(charPrefab, this.transform);
            character = temp.GetComponent<CharacterMovement>();
            character.owner = gameObject;
            FindObjectOfType<LobbySetup>().players.Add(this);
            print("1  - " + GetComponent<NetworkIdentity>().connectionToClient.connectionId);
            print("2  - " + GetComponent<NetworkIdentity>().connectionToClient.identity);
            print("3  - " + GetComponent<NetworkIdentity>().connectionToClient.authenticationData);
            print("4  - " + GetComponent<NetworkIdentity>().connectionToClient.isReady);
            return;
        }

        Destroy(gameObject);
    }
    public GameObject owner;
  
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
  
    public void SendInput(Vector2 inp)
    {
        CMD_SendInput(inp);
    }
    [Command]
    void CMD_SendInput(Vector2 pos)
    {
         character.UpdateInput(pos);
    }

    public void Jump()
    {
        CMD_Jump();
    }
    [Command]
    void CMD_Jump()
    {
        character.Jumping();
    }

}
