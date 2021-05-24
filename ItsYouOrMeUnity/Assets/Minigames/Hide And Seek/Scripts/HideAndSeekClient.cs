using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekClient : MonoBehaviour
{
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft;
        FindObjectOfType<PlayerScript>().MinigameConnectedTo(1);
    }
}
