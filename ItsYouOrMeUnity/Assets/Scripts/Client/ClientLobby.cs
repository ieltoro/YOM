using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientLobby : MonoBehaviour
{
    [SerializeField] YOMNetworkDiscovery networkDiscovery;
    public GameObject[] canvas;
    YOMNetworkManager manager;

    private void Awake()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        manager = FindObjectOfType<YOMNetworkManager>();
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
        ChangeUi(1);
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
