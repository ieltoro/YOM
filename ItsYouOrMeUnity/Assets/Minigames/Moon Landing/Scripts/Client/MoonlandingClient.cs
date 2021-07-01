using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonlandingClient : MonoBehaviour
{
    public MoonlandingPlayer player;
    Vector2 input;
    [SerializeField] float yValue, target;
    [SerializeField] float timespeed;
    
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
    }

    public void PressedDown()
    {
        target = -0.4f;
    }
    public void PressedUp()
    {
        target = 1;
    }
    public void Released()
    {
        target = 0;
    }
    void Update()
    {
        yValue = Mathf.Lerp(yValue, target, timespeed);
        input = new Vector2(Input.acceleration.x, yValue);
    }
}
