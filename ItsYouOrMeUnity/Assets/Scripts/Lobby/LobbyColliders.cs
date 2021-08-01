using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LobbyColliders : MonoBehaviour
{

    [SerializeField] TextMeshPro countdown;
    [SerializeField] int timeToExecute;
    [SerializeField] bool minigame;
    [SerializeField] LobbyColliders theOther;
    [SerializeField] LobbySetup ls;
    bool started;
   
    private void OnTriggerStay(Collider other)
    {
        if(!started)
        {
            StartCoroutine(Startcountdown());
            theOther.DeactivateThis(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        countdown.enabled = false;
        StopAllCoroutines();
        started = false;
        
        theOther.DeactivateThis(false);
    }
    IEnumerator Startcountdown()
    {
        started = true;
        countdown.enabled = true;
        for (int i = timeToExecute; i >= 0; i--)
        {
            countdown.text = i.ToString();
            yield return new WaitForSeconds(1);
        }
        if(minigame)
        {
            print("Launch minigame");
            ls.PlayersReady(1);
        }
        else
        {
            ls.PlayersReady(0);
            print("Start Game");
        }
    }
    public void DeactivateThis(bool answer)
    {
        StopAllCoroutines();
        GetComponent<BoxCollider>().enabled = !answer;
        countdown.enabled = false;
    }
}
