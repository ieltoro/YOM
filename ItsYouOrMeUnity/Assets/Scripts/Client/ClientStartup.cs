using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ClientStartup : MonoBehaviour
{
    public List<GameObject> panels;
    public Text connText;

    private void Start()
    {
        Screen.orientation = ScreenOrientation.Portrait;
        StartCoroutine(Test());
    }

    public void ConnectedToFB()
    {
        panels[0].SetActive(true);
        panels[3].SetActive(!panels[3].activeSelf);
    }

    public void ChangText(string inf)
    {
        string i = inf;
        print("Change text " + i);
        connText.text = i;
        print("Changed text");
    }
    public void ChangePanel(int nr)
    {
        foreach(GameObject g in panels)
        {
            g.SetActive(false);
        }
        panels[nr].SetActive(true);
    }
    public void SignedIn()
    {
        print("We are in");
        StartCoroutine(SignedInCD());
    }
    IEnumerator SignedInCD()
    {
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Lobby Phone");
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            print("Changing");
            panels[3].SetActive(!panels[3].active);
        }
    }
    IEnumerator Test()
    {
        yield return new WaitForSeconds(0.3f);
        if(AutManager.aut.db != null)
        {
            ConnectedToFB();
        }
        else
        {
            StartCoroutine(Test());
        }
    }
}
