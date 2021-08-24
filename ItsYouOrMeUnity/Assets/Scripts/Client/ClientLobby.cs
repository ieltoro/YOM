using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientLobby : MonoBehaviour
{
    [SerializeField] YOMNetworkDiscovery networkDiscovery;
    [Tooltip("0 = First Laumch \n 1 = Sign up \n 2 = Sign in \n 3 = Menu \n 4 = Connecting \n 5 = Lobby \n 6 = Waiting")]
    public GameObject[] canvas;
    [SerializeField] YOMNetworkManager manager;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
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
        PlayerPrefs.SetString("PlayerID", GetProjectName());
        ClientSaveGame.csg.playerID = PlayerPrefs.GetString("PlayerID");
        ChangeUi(4);
        networkDiscovery.StartDiscovery();
        
    }
    public void PressedMyTown()
    {
        ChangeScene("MyTown Phone");
    }
    public void FoundServer(string ip4)
    {
        print("Got server");
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
        SceneManager.LoadScene(name);
    }

}
