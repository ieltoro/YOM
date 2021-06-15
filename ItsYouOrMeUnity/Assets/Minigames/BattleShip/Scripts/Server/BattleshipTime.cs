using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipTime : MonoBehaviour
{
    [SerializeField] BattleshipServer bs;
    
    public void StartPlaceShipTimer()
    {
        StartCoroutine(TimerPlace());
    }
    public void StopPlaceShipTimer()
    {
        
    }
    public void TimesOutPlaceShipTimer()
    {
        bs.TimeisOutSetup();
    }

    IEnumerator TimerPlace()
    {
        yield return new WaitForSeconds(30);
        StopPlaceShipTimer();
    }
}
