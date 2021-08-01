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
        StartCoroutine(TimerZone());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(player = null)
        {
             player = other.gameObject;
        }
        other.GetComponent<ZoneholderController>().InsideZone(true);
        manager.NextZone();
    }

    IEnumerator TimerZone()
    {
        yield return new WaitForSeconds(10);
        manager.
    }
}
