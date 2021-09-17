using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballClient : MonoBehaviour
{
    public FootballPlayer player;
    [SerializeField] Joystick joystick;
    [SerializeField] GameObject startCanvas, gameCanvas;
    Vector2 input;
    bool active;
   
    void Start()
    {
        print(1);
        FindObjectOfType<PlayerScript>().ConnectedToMinigame(1);
        print(2);
    }
    public void PressedStart()
    {
        player.PressedStart();
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
    }
    void Update()
    {
        if (active)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
            SendData();
        }
    }
    public void StartMoving(bool answer)
    {
        active = answer;
    }
    void SendData()
    {
        player.RecievedInput(input);
    }
}
