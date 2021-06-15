using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class BattleshipPlayer : NetworkBehaviour
{
    void Start()
    {

        if (isLocalPlayer)
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
    }

    #region Client
    BattleshipClient bc;
    public string playerName;

    void SetOwnerName()
    {
        playerName = ClientSaveGame.csg.playerName;
        CMD_SetOwnerName(playerName);
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
    public int attackTile;
    
    [Command]
    void CMD_SetOwnerName(string name)
    {
        playerName = name;
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

    }
    #endregion
}
