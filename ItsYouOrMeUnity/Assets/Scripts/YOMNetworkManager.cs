using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;
using Mirror.Discovery;

public class YOMNetworkManager : NetworkManager
{
    public static YOMNetworkManager manager;
    public YOMNetworkDiscovery networkDiscovery;

    public override void Awake()
    {
        if (YOMNetworkManager.manager == null)
            manager = this;
        else
        {
            Destroy(YOMNetworkManager.manager.gameObject);
            manager = this;
        }
        base.Awake();
    }

    #region Server
    public bool playing;
    public LobbySetup ls;
    ServerCalls sc;
    public override void OnStartServer()
    {
        print("Success start server");
        base.OnStartServer();
        GameObject networkpref = (GameObject)Instantiate(spawnPrefabs[0], transform.position, transform.rotation);
        NetworkServer.Spawn(networkpref);
        sc = networkpref.GetComponent<ServerCalls>();
        networkDiscovery.StartServer();
        FindObjectOfType<LobbySetup>().HostSucceded();
    }
    public override void OnStopServer()
    {
        base.OnStopServer();
    }
    public override void OnServerConnect(NetworkConnection conn)
    {
        if (!playing)
        {
            base.OnServerConnect(conn);
            base.OnServerAddPlayer(conn);
        }
        if(playing)
        {
            base.OnServerConnect(conn);
            base.OnServerAddPlayer(conn);
            //NetworkServer.SpawnObjects();
        }
    }
    public override void OnServerDisconnect(NetworkConnection conn)
    {
        if(!playing)
        {
            print("Player left at lobby, remove");
            GameObject remove = new GameObject();
            HashSet<NetworkIdentity> tmp = new HashSet<NetworkIdentity>(conn.clientOwnedObjects);
            print(tmp.Count);
            foreach (NetworkIdentity netIdentity in tmp)
            {
                foreach(GameObject g in GameSaveHolder.gsh.players)
                {
                    if (netIdentity.gameObject == g)
                    {
                        remove = g;
                    }
                }
            }
            GameSaveHolder.gsh.ResetPlayerList(remove);
            ls.PlayerLeftLobby();
            base.OnServerDisconnect(conn);
        }

        if (playing)
        { 
            print("Player left during game");
            HashSet<NetworkIdentity> tmp = new HashSet<NetworkIdentity>(conn.clientOwnedObjects);
            print(tmp.Count);
            foreach (NetworkIdentity netIdentity in tmp)
            {
                foreach (GameObject g in GameSaveHolder.gsh.players)
                {
                    if (netIdentity.gameObject == g)
                    {
                        g.GetComponent<PlayerScript>().connected = false;
                    }
                }
            }
        }
        
    }
    public void SpawnNewPlayer(GameObject owner, int prefabNr)
    {
        GameObject temp = (GameObject)Instantiate(spawnPrefabs[prefabNr], transform.position, transform.rotation);
        NetworkServer.Spawn(temp, owner);
    }
    public override void OnServerChangeScene(string newSceneName)
    {
        base.OnServerChangeScene(newSceneName);
    }
    public override void OnServerSceneChanged(string sceneName)
    {
        base.OnServerSceneChanged(sceneName);
    }
    #endregion
    #region Client

    public override void OnClientConnect(NetworkConnection conn)
    {
        print("Success join server");
        base.OnClientConnect(conn);
    }
    public override void OnClientDisconnect(NetworkConnection conn)
    {
        base.OnClientDisconnect(conn);
        if (SceneManager.GetActiveScene().name != "Lobby Phone")
        { 
            SceneManager.LoadScene("Lobby Phone");
            Destroy(gameObject);
            return; 
        }
        FindObjectOfType<ClientLobby>().LostConnection();
    }
    public override void OnClientSceneChanged(NetworkConnection conn)
    {
        base.OnClientSceneChanged(conn);
        
    }
    #endregion
}
