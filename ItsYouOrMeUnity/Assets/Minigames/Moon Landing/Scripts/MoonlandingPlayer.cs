using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MoonlandingPlayer : NetworkBehaviour
{
    [SerializeField] GameObject spaceshipPrefab;
    [SerializeField] SpaceshipController mySpaceShip;
    public string playerID;

    private void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            FindObjectOfType<MoonlandingClient>().player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            GameObject temp = Instantiate(spaceshipPrefab);
            mySpaceShip = temp.GetComponent<SpaceshipController>();
            mySpaceShip.owner = this;
            FindObjectOfType<MoonlandingServer>().players.Add(this);
            return;
        }
        Destroy(gameObject);
    }

    #region Client
    void SetOwnerID()
    {
        playerID = ClientSaveGame.csg.playerID;
        CMD_SetOwnerName(playerID);
    }
    public void RecievedInput(Vector2 input)
    {
        CMD_RecievedInput(input);
    }

    #endregion
    #region Server
    public GameObject owner;
    
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
    [Command]
    void CMD_RecievedInput(Vector2 input)
    {
        mySpaceShip.InputRecieved(input);
    }

    #endregion
}
