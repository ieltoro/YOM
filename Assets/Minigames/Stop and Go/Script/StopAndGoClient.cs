using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopAndGoClient : MonoBehaviour
{
    [SerializeField] GameObject[] canvas;
    public StopAndGoPlayer myPlayer;
    void Start()
    {
        FindObjectOfType<PlayerScript>().MinigameConnectedTo(0);
    }
    public void ChangeUI(int i)
    {
        if (ClientSaveGame.csg.dead)
            return;
        foreach (GameObject g in canvas)
            g.SetActive(false);
        canvas[i].SetActive(true);
    }
    public void Leader()
    {
        ChangeUI(1);

    }
    public void PressedStartMiniGame()
    {
        ChangeUI(0);
        myPlayer.PressedStartRed();
    }
    public void StartGame()
    {
        ChangeUI(2);
    }


    public void PressingButton()
    {
        myPlayer.PlayerPressing(true);
    }
    public void ReleaseButton()
    {
        myPlayer.PlayerPressing(false);
    }
}
