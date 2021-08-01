using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballClient : MonoBehaviour
{
    public FootballPlayer player;
    Vector2 input;
   
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        input = new Vector2(Input.acceleration.x, Input.acceleration.y);
        SendData();
    }
    void SendData()
    {
        player.RecievedInput(input);
    }
}
