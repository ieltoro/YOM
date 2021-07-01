using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LobbySetup : MonoBehaviour
{
    ServerCalls sc;
    [SerializeField] YOMNetworkManager manager;
    [SerializeField] TransitionSize trans;
    [Tooltip(" 0 = Start \n 1 = lobby \n 2 = roundstart")]
    [SerializeField] GameObject startCanvas, canvasHost, lobbyLocalCanvas, lobbyOnlineCanvas, staringGameCanvas;
    [SerializeField] GameSaveHolder save;
    public GameObject hostLeader;
    public int starthp = 3;

    public List<LobbyCharacter> players;
    public List<string> presidentCandidatesNames;
    private string namesPlaying;
    [SerializeField] Text namesListTxt;
    [SerializeField] GameObject[] colliders;

    int playersReady;

    private void Start()
    {
        print(SceneManager.GetActiveScene().name);
        print(GetProjectName());
        StartCoroutine(StartCD());
    }
    public string GetProjectName()
    {
        string[] s = Application.dataPath.Split('/');
        string projectName = s[s.Length - 2];
        return projectName;
    }
    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(1);
        trans.TransitionIn();
    }
    public void PressedSettings()
    {

    }
    public void PressedQuit()
    {
        Application.Quit();
    }
    public void PressedHost()
    {
        startCanvas.SetActive(false);
        canvasHost.SetActive(true);
    }
    public void HostSucceded()
    {
        colliders[0].SetActive(true);
        colliders[1].SetActive(true);
    }
    public void PressedHostLocal()
    {
        manager.StartServer();
        canvasHost.SetActive(false);
        lobbyLocalCanvas.SetActive(true);
    }
    public void PressedHostOnline()
    {
        manager.StartServer();
        canvasHost.SetActive(false);
        lobbyLocalCanvas.SetActive(true);
    }
    public void PlayerJoinedLobby(string player)
    {
        print("Add name " + player);
        presidentCandidatesNames.Add(player);
        namesPlaying = "";
        foreach (string s in presidentCandidatesNames)
        {
            namesPlaying += "\n" + s;
        }
        namesListTxt.text = namesPlaying;
    }
    public void PlayerLeftLobby()
    {
        presidentCandidatesNames.Clear();
        namesListTxt.text = "";
        namesPlaying = "";
        if (save.players.Count > 0)
        {
            foreach (GameObject g in save.players)
            {
                print("Adding name");
                presidentCandidatesNames.Add(g.GetComponent<PlayerScript>().playerName);
            }
            foreach (string s in presidentCandidatesNames)
            {
                namesPlaying += "\n" + s;
            }
            namesListTxt.text = namesPlaying;
            if (hostLeader == null)
            {
                hostLeader = save.players[0];
                save.players[0].GetComponent<PlayerScript>().AssignAsLeader();
            }
        }
    }
    public void ChangeHP(int i)
    {
        starthp = i;
        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().SetHP(starthp);
        }
    }
    public void PlayerReady()
    {
        foreach(LobbyCharacter g in players)
        {
            g.character.StopMoving();
        }
        manager.playing = true;
        StartGame();
    }
    private void StartGame()
    {
        lobbyLocalCanvas.SetActive(false);
        staringGameCanvas.SetActive(true);
        foreach (GameObject g in save.players)
        {
            g.GetComponent<PlayerScript>().SendInfoToLobby();
        }
        save.playersAliveAndConnected = save.players.Count;
        ServerCalls.sc.StartingGame();
        StartCoroutine(StartRoundTimer());
    }
    IEnumerator StartRoundTimer()
    {
        trans.TransitionOut();

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Game");
    }
}
