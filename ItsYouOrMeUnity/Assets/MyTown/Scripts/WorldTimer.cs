using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldTimer : MonoBehaviour
{
    //public static WorldTimer sharedInstance = null;
    private string url = "http://appsbyapes.com/Time/WorldTEU.php";
    private string timeData;
    private string startTime;
    private string startDate;

    //void Awake()
    //{
    //    if (sharedInstance == null)
    //    {
    //        sharedInstance = this;
    //    }
    //    else if (sharedInstance != this)
    //    {
    //        Destroy(gameObject);
    //    }
    //    DontDestroyOnLoad(gameObject);
    //}
    void Start()
    {
        StartCoroutine(getTime());
    }

    public IEnumerator getTime()
    {
        WWW www = new WWW(url);
        yield return www;
        timeData = www.text;
        string[] words = timeData.Split('/');
        //timerTestLabel.text = www.text;
        Debug.Log("The date is : " + words[0] + ", The time is : " + words[1]);

        startDate = words[0];
        startTime = words[1];
        print("Time :" + startTime + "  Date : " + startDate);
    }


    public string getCurrentDateNow()
    {
        return startDate;
    }

    public string getCurrentTimeNow()
    {
        return startTime;
    }
}
