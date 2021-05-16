﻿using System.Collections;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Net.Sockets;
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

    public List<string> presidentCandidatesNames;
    private string namesPlaying;
    [SerializeField] Text namesListTxt;

    [Header("Lobby / start")]
    int playersReady;

    private void Start()
    {       
        StartCoroutine(StartCD());
    }

    IEnumerator StartCD()
    {
        yield return new WaitForSeconds(1);
        
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
        save.playersAlive = save.players.Count;
        sc = FindObjectOfType<ServerCalls>();
        sc.StartingGame();
        StartCoroutine(StartRoundTimer());
    }
    IEnumerator StartRoundTimer()
    {
        trans.TransitionOut();

        yield return new WaitForSeconds(3);
        SceneManager.LoadScene("Game");
    }
}
