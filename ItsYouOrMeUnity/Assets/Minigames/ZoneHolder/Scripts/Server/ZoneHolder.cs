using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneHolder : MonoBehaviour
{
    ZoneholderServer manager;
    GameObject player;
    private void Start()
    {
        manager = FindObjectOfType<ZoneholderServer>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if(player = null)
        {
             player = other.gameObject;
        }
        other.transform.GetComponent<ZoneholderController>().score++;
        manager.NextZone();
    }
}
