using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class FlagPlayer : NetworkBehaviour
{
    [SerializeField] GameObject prefab, myPlayer;
    public string playerID;
    [SerializeField] float runningTime = 10;
    float speed;
    bool running;
    [SerializeField] Image fillIMG;
    
    void Start()
    {
        if (hasAuthority)
        {
            print("Is mine");
            FindObjectOfType<FlaggraberClient>().player = this;
            SetOwnerID();
        }
        if (isServer)
        {
            print("IS Server");
            myPlayer = Instantiate(prefab);
            FindObjectOfType<FlaggraberManager>().players.Add(this);
        }
        Destroy(gameObject);
    }

    #region Client

    

    void SetOwnerID()
    {
        playerID = ClientSaveGame.csg.playerID;
        CMD_SetOwnerName(playerID);
    }
    public void SendInput(Vector2 inp)
    {
        CMD_SendInput(inp);
    }
    public void Sprinting(bool answer)
    {
        CMD_Sprinting(answer);
    }
    public void Jump()
    {
        CMD_Jump();
    }
    #endregion
    #region Server

    FlagPlayerMove playerMove;
    public GameObject owner;
    public int score;

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
    void CMD_SendInput(Vector2 pos)
    {
        playerMove.UpdateInput(pos);
    }
    void CMD_Sprinting(bool Answer)
    {

    }
    void CMD_Jump()
    {

    }
    public void AddScore(int amount)
    {
        score += amount;
    }
    #endregion

}
