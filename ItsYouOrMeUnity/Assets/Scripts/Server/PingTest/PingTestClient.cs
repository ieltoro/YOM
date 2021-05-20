using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingTestClient : MonoBehaviour
{
    void Start()
    {
        FindObjectOfType<PlayerScript>().MinigameConnectedTo(1);
    }
}
