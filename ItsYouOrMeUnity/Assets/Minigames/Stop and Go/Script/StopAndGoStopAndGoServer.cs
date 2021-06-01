using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StopAndGoServer : MonoBehaviour
{

    YOMNetworkManager manager;
    ServerCalls sc;
    [SerializeField] GameObject redLight;
    [SerializeField] GameObject[] canvas;
    [SerializeField] Text connectedText;
    [SerializeField] GameObject startPanel;
    public List<StopAndGoPlayer> players;
    [SerializeField] MultiCamera cam;
    [SerializeField] TransitionSize trans;
    [SerializeField] Transform[] spawnPos;
    [HideInInspector] public GameObject leader;
    
    private void Awake()
    {
        manager = FindObjectOfType<YOMNetworkManager>();
        sc = FindObjectOfType<ServerCalls>();
        sc.ConnectedToMiniGame(SceneManager.GetActiveScene().name);
        connectedText.text = "0/" + GameSaveHolder.gsh.players.Count;
        StartCoroutine(TransitionStart());
    }
    IEnumerator TransitionStart()
    {
        yield return new WaitForSeconds(1);
        trans.TransitionIn();
    }
    int pConnected;
    public void ConnectedToRed(GameObject p)
    {
        manager.SpawnNewPlayer(p, 3);
        pConnected++;
        connectedText.text = pConnected.ToString() + "/" + GameSaveHolder.gsh.players.Count; ;
    }

    int s;
    public void StartRedLight()
    {
        foreach (StopAndGoPlayer g in players)
        {
            g.SetSpawn(spawnPos[s]);
            cam.targets.Add(g.transform);
            s++;
        }
        StartCoroutine(StartGameCD());
    }

    IEnumerator StartGameCD()
    {
        yield return new WaitForSeconds(1);
        trans.TransitionOut();
        yield return new WaitForSeconds(2);
        canvas[0].SetActive(false);
        canvas[1].SetActive(true);
        yield return new WaitForSeconds(2);
        trans.TransitionIn();
        yield return new WaitForSeconds(2);
        startPanel.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        startPanel.SetActive(false);
        NewRound();
    }
    public void NewRound()
    {
        StartCoroutine(RandomWaitTime());
    }

    IEnumerator RandomWaitTime()
    {
        float r = Random.Range(0.5f, 4.0f);
        yield return new WaitForSeconds(r);
        redLight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        StopRound();
    }
    public void StopRound()
    {
        foreach(StopAndGoPlayer g in players)
        {
            g.redLight = true;
        }
        StartCoroutine(RandomStartWaitTime());
    }
    IEnumerator RandomStartWaitTime()
    {
        float r = Random.Range(2.0f, 4.0f);
        yield return new WaitForSeconds(r);
        NewRound();
        redLight.SetActive(false);
    }

    public void ReturnFromGame()
    {
        GameSaveHolder.gsh.resultsLastGame.Add(GameSaveHolder.gsh.players[0]);
        ServerCalls.sc.ReturnFromMiniGame();
        SceneManager.LoadScene("Game");
    }
}
