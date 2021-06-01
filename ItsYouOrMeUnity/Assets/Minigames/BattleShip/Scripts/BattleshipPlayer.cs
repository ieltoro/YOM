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
            bc.bp = this;
            return;
        }
        if(!isServer && isLocalPlayer)
        {
            bc.otherPlayers.Add(gameObject.GetComponent<NetworkIdentity>().);
        }
        if(isServer)
        {
            bs = FindObjectOfType<BattleshipServer>();
            bs.players.Add(this);
            return;
        }

        Destroy(gameObject);
    }



    #region Client
    BattleshipClient bc;

    public void AssignShipPosition()
    {

    }
    #endregion


    #region Server
    BattleshipServer bs;


    #endregion
}
