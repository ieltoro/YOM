using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballClient : MonoBehaviour
{
    public FootballPlayer player;
    [SerializeField] Joystick joystick;
    Vector2 input;
   
    void Start()
    {
        
    }
    void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
        SendData();
    }
    void SendData()
    {
        player.RecievedInput(input);
    }
}
