using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintballClient : MonoBehaviour
{
    public PaintballNetwork player;
    Joystick joystick;
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
    // Update is called once per frame
    void Update()
    {
        if(playing)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
            UpdateInput();
        }
    }

    void UpdateInput()
    {
        player.RecievedInput(input);
    }
    public void PressedShot()
    {
        player.PressedShot();
    }
}
