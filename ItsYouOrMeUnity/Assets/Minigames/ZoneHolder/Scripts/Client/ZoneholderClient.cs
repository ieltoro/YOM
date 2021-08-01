using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneholderClient : MonoBehaviour
{
    public ZoneholderPlayer player;
    Vector2 input;

    void Start()
    {

    }
    public void StartGame()
    {

    }
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
