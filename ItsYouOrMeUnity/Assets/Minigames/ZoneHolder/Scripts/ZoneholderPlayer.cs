using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ZoneholderPlayer : NetworkBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] ZoneholderController myPlayer;
    [SerializeField] ZoneholderClient client;
    public string playerID;

    void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            client = FindObjectOfType<ZoneholderClient>();
            client.player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            GameObject temp = Instantiate(prefab);
            myPlayer = temp.GetComponent<ZoneholderController>();
            myPlayer.owner = this;
            FindObjectOfType<ZoneholderServer>().ConnectedToMiniGame(gameObject);
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

    [Command]
    void CMD_RecievedInput(Vector2 input)
    {
        myPlayer.InputRecieved(input);
    }
    #endregion
    #region Client


    public void RecievedInput(Vector2 input)
    {
        CMD_RecievedInput(input);
    }

    #endregion
}
