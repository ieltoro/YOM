using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneholderClient : MonoBehaviour
{
    public ZoneholderPlayer player;
    [SerializeField] GameObject playUI;
    Vector2 input;
    bool playing;

    void Start()
    {

    }
    public void StartGame(bool answer)
    {

        playUI.SetActive(answer);
        playing = answer;
    }
    void Update()
    {
        if(playing)
        {
            input = new Vector2(Input.acceleration.x, Input.acceleration.y);
            SendData();
        }

    }
    void SendData()
    {
        player.RecievedInput(input);
    }
}
