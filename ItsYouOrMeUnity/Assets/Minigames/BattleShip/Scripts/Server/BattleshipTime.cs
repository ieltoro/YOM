using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleshipTime : MonoBehaviour
{
    [SerializeField] BattleshipServer bs;
    [SerializeField] Text time;
    
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
