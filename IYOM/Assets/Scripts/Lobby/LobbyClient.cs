using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyClient : MonoBehaviour
{
    public LobbyCharacter player;
    [SerializeField] GameObject canvas;
    [SerializeField] Joystick joystick;
    Vector2 input;
    bool sentZero;
    

    public void StartController()
    {
        print("Activeate");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        canvas.SetActive(true);
        this.enabled = true;
    }

    void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
        UpdateInput();
    }

    void UpdateInput()
    {
        player.SendInput(input);
    }
    public void PressedJump()
    {
        player.Jump();
    }
}
