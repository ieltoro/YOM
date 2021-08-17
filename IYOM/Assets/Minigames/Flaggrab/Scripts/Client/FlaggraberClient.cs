using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FlaggraberClient : MonoBehaviour
{
    public FlagPlayer player;
    [SerializeField] GameObject[] ui;
    Vector2 input;
    Joystick joystick;

    private void Update()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
    }

}
