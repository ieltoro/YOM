using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ClientStartScene : MonoBehaviour
{
    public GameObject con;
    public List<GameObject> panels;
    public Text connText;




    public void ConnectedToFirebase()
    {
        StartItup();
        if (PlayerPrefs.GetString("FirstLaunch") == "" || PlayerPrefs.GetString("FirstLaunch") == null)
        {
            print("First launch");
        }
    }

    void StartItup()
    {
        ChangePanel(0);
    }
    public void ChangePanel(int nr)
    {
        print(panels.Count);
        foreach(GameObject p in panels)
        {
            print("+1");
        }
        print(2);
        con.SetActive(false);
        print(3);
        panels[3].SetActive(false);
        print(4);
    }

    public void ChangeInfoText(string text)
    {
        connText.text = text;
    }
    public void SignedIn()
    {
        StartCoroutine(ConnecedCD());
    }
    IEnumerator ConnecedCD()
    {
        connText.text = "Starting";
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(name);
    }
}
