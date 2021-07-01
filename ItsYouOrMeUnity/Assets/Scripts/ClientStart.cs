using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ClientStart : NetworkBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        if (isLocalPlayer)
        {
            gameObject.name = "My ClientStart";
            RPC_Connected(PlayerPrefs.GetString("PlayerID"));
            manager = FindObjectOfType<YOMNetworkManager>();
        }
        if (!isLocalPlayer && !isServer)
            Destroy(gameObject);
    }

    GameSaveHolder save;
    YOMNetworkManager manager;

    [Command]
    void RPC_Connected(string playerID)
    {
        NetworkConnection conn = connectionToClient;
        if (save == null)
            save = GameSaveHolder.gsh;
        if (manager == null)
            manager = FindObjectOfType<YOMNetworkManager>();
        if (manager.playing)
        {
            if (save.players.Count > 0)
            {
                foreach (GameObject g in save.players)
                {
                    if (g.GetComponent<PlayerScript>().id == playerID)
                    {
                        g.GetComponent<NetworkIdentity>().RemoveClientAuthority();
                        g.GetComponent<NetworkIdentity>().AssignClientAuthority(conn);
                        g.GetComponent<PlayerScript>().UpdateOwner();
                    }
                }
            }
        }
        else
        {
            manager.SpawnNewPlayer(gameObject, 2);
            manager.SpawnNewPlayer(gameObject, 3);
        }
    }
}
