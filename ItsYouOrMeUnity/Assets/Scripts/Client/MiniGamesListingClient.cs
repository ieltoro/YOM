using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniGamesListingClient : MonoBehaviour
{
    PlayerScript script;

    private void Start()
    {
        script = FindObjectOfType<PlayerScript>();
    }
    public void PressedDirection(int i)
    {
        script.MiniGameDirectionList(i);
    }
}
