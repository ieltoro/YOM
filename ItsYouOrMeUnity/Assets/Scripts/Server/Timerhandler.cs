using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Timerhandler : MonoBehaviour
{
    [SerializeField] GameSetup gs;
    [SerializeField] Text timerDisplay;
    int modeR;
    public void StartTimer(int time, int mode)
    {
        modeR = mode;
        StartCoroutine(StartTimerCD(time));
    }
    public void TimesOut()
    {

    }
    public void CancelTimer()
    {
        StopAllCoroutines();
    }
    IEnumerator StartTimerCD(int timer)
    {
        timerDisplay.gameObject.SetActive(true);
        for (int t = timer; t > 0; t--)
        {
            timerDisplay.text = t.ToString();
            yield return new WaitForSeconds(1);
            if (t == 1)
            {
                yield return new WaitForSeconds(1);
                timerDisplay.text = "0";
                yield return new WaitForSeconds(1);
                TimesOut();
            }
        }
    }
}
