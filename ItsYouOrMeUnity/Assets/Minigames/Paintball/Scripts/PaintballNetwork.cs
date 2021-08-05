using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PaintballNetwork : NetworkBehaviour
{
    [SerializeField] GameObject prefab;
    [SerializeField] PaintballPlayer myPlayer;
    [SerializeField] PaintballClient client;
    public string playerID;
    int hp = 3;
    void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            client = FindObjectOfType<PaintballClient>();
            client.player = this;
            SetOwnerID();
            return;
        }
        if (isServer)
        {
            print("IS Server");
            GameObject temp = Instantiate(prefab);
            myPlayer = temp.GetComponent<PaintballPlayer>();
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
        myPlayer.UpdateInput(input);
    }
    [Command]
    void CMD_PressedShot()
    {
        myPlayer.Shot();
    }
    public void Gothit()
    {
        print("Got hit");
        hp--;
        if(hp == 0)
        {
            print("Player is out");
        }
    }
    #endregion
    #region Client


    public void RecievedInput(Vector2 input)
    {
        CMD_RecievedInput(input);
    }
    public void PressedShot()
    {
        CMD_PressedShot();
    }
    #endregion
}
