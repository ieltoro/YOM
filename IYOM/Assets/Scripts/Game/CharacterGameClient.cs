using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterGameClient : MonoBehaviour
{
    public CharacterGame player;
    [SerializeField] GameObject canvas;
    [SerializeField] Joystick joystick;
    Vector2 input;
    bool sentZero;

    public void StartController(bool answer)
    {
        print("Activate");
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        canvas.SetActive(answer);
        this.enabled = answer;
    }

    void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
        UpdateInput();
    }
    void UpdateInput()
    {
        if(player != null)
            player.SendInput(input);
    }
    public void PressedJump()
    {
        player.Jump();
    }
    public void EnableUI(bool a)
    {
        canvas.SetActive(a);
    }
}
