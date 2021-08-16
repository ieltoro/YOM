﻿using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientLobby : MonoBehaviour
{
    YOMNetworkDiscovery networkDiscovery;
    [Tooltip("0 = First Laumch \n 1 = Sign up \n 2 = Sign in \n 3 = Menu \n 4 = Connecting \n 5 = Lobby \n 6 = Waiting")]
    public GameObject[] canvas;
    [SerializeField] YOMNetworkManager manager;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
    }

    public void ConnectedToFirebase()
    {
        print(1.1);

        if (PlayerPrefs.GetString("FirstLaunch") == "")
        {
            print(1.2);
            print("First launch");
            ChangeUi(0);
            print(1.3);
        }


        //if (PlayerPrefs.GetString("PlayerID") == null || PlayerPrefs.GetString("PlayerID") == "")
        //{
        //    int r = (int)Random.Range(1, 9999999999);
        //    //PlayerPrefs.SetString("PlayerID", "ID" + r.ToString());
        //    PlayerPrefs.SetString("PlayerID", GetProjectName());
        //}
        //PlayerPrefs.SetString("PlayerID", GetProjectName());
        //ClientSaveGame.csg.playerID = PlayerPrefs.GetString("PlayerID");
        //ClientSaveGame.csg.cosmetic = Random.Range(0, 5);
    }
    public string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
    public void LostConnection()
    {
        ChangeUi(0);
        print("Lost Connection");
    }
    public void PressedJoinLocal()
    {
        if (networkDiscovery == null)
            networkDiscovery = gameObject.AddComponent<YOMNetworkDiscovery>();

        PlayerPrefs.SetString("PlayerID", GetProjectName());
        ClientSaveGame.csg.playerID = PlayerPrefs.GetString("PlayerID");
        ChangeUi(1);
        networkDiscovery.StartDiscovery();
        
    }
    public void PressedMyTown()
    {
        ChangeScene("MyTown Phone");
    }
    public void FoundServer(string ip4)
    {
        Destroy(networkDiscovery);
        manager.StartClient(ip4);
    }
    public void ChangeUi(int nr)
    {
        foreach (GameObject g in canvas)
        {
            g.SetActive(false);
        }
        if(nr > -1)
            canvas[nr].SetActive(true);
    }
    public void ChangeScene(string name)
    {
        print("Connected anc change to " + name);
        SceneManager.LoadScene(name);
    }

}
