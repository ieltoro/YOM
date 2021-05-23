using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HideAndSeekClient : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<PlayerScript>().MinigameConnectedTo(1);
    }
}
