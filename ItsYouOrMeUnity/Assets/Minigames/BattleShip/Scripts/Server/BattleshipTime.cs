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
        StopAllCoroutines();
    }
    public void TimesOutPlaceShipTimer()
    {
        bs.TimeisOutSetup();
    }
    IEnumerator TimerPlace()
    {
        for(int i = 90; i > 0; i--)
        {
            yield return new WaitForSeconds(90);
            time.text = i.ToString();
        }
        StopPlaceShipTimer();
    }
}
