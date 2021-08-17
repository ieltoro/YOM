using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BattleshipPlayer : NetworkBehaviour
{
    void Start()
    {
        if (hasAuthority)
        {
            bc = FindObjectOfType<BattleshipClient>();
            bc.player = this;
            SetOwnerName();

            return;
        }
        if(isServer)
        {
            bs = FindObjectOfType<BattleshipServer>();
            bs.players.Add(this);
            return;
        }
        Destroy(this.gameObject);
    }

    #region Client
    BattleshipClient bc;
    public string playerID;

    void SetOwnerName()
    {
        playerID = ClientSaveGame.csg.playerID;
        CMD_SetOwnerName(playerID);
    }

    public void StartGame()
    {
        CMD_StartGame();
    }
    public void AssignShipPosition(int ship1, float rot1, int ship2, float rot2)
    {
        CMD_AssignShipPosition(ship1, rot1, ship2, rot2);
    }
    public void BombedThisTile(int tile)
    {
        CMD_BombedThisTile(tile);
    }

    #endregion

    #region Server

    BattleshipServer bs;
    public List<GameObject> ship;
    GameObject owner;
    public int attackTile;
    
    [Command]
    void CMD_SetOwnerName(string name)
    {
        playerID = name;
        foreach(GameObject g in GameSaveHolder.gsh.players)
        {
            if(g.GetComponent<PlayerScript>().id == playerID)
            {
                owner = g;
                return;
            }
        }
    }
    [Command]
    void CMD_StartGame()
    {
        bs.AllConnectedAndPressedStart();
    }
    [Command]
    void CMD_AssignShipPosition(int ship1Pos, float rot1, int ship2Pos, float rot2)
    {
        bs.AssignBattleshipPositions(ship1Pos, rot1, ship2Pos, rot2, this);
    }
    [Command]
    void CMD_BombedThisTile(int tile)
    {
        bs.BombedATile(tile);
    }
    public void ShipSunken(GameObject s)
    {
        ship.Remove(s);
        int i = new int();
        foreach(GameObject g in ship)
        {
            if(g == null)
            {
                i++;
                if( i == 2)
                {
                    print("Player is out");
                    bs.PlayerIsOut(owner);
                }
            }
        }
    }
    #endregion
}
