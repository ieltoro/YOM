using Mirror.Discovery;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientLobby : MonoBehaviour
{
    YOMNetworkDiscovery networkDiscovery;
    [Tooltip("0 = JoinPanel \n 1 = Connecting \n 2 = LobbySceen \n 3 = Waiting")]
    [SerializeField] GameObject[] canvas;
    [SerializeField] YOMNetworkManager manager;
    [SerializeField] InputField ifNameplayer,idInput;

    private void Start()
    {
        if (PlayerPrefs.GetString("PlayerID") == null || PlayerPrefs.GetString("PlayerID") == "")
        {
            int r = (int)Random.Range(1, 9999999999);
            PlayerPrefs.SetString("PlayerID", "ID" + r.ToString());
            print(r);
        }
        ClientSaveGame.csg.playerID = PlayerPrefs.GetString("PlayerID");
        ClientSaveGame.csg.cosmetic = Random.Range(0, 5);
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
        if (ifNameplayer.text == null || ifNameplayer.text == "")
        {
            print("Need a name");
            return;
        }
        PlayerPrefs.SetString("PlayerID", "ID" + idInput.text);
        ClientSaveGame.csg.playerID = PlayerPrefs.GetString("PlayerID");
        ChangeUi(1);
        ClientSaveGame.csg.playerName = ifNameplayer.text;
        networkDiscovery.StartDiscovery();
        
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
        canvas[nr].SetActive(true);
    }
    public void PlayerReady()
    {
        ClientSaveGame.csg.localPlayer.GetComponent<PlayerScript>().PlayerReady();
        ChangeUi(3);
    }
    public void ChangeToGameSceen()
    {
        print("Change Scene");
        SceneManager.LoadScene("Game Phone");
    }
}
