using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class TestSceneChange : NetworkBehaviour
{
    YOMNetworkManager manager;
    GameSaveHolder gsh;
    [SerializeField] GameObject playerPref;
    [SerializeField] TransitionSize trans;

    private void Start()
    {
        gsh = GameSaveHolder.gsh;
        manager = FindObjectOfType<YOMNetworkManager>();
        foreach(GameObject g in gsh.players)
        {
            manager.SpawnNewPlayer(g, 3);
        }
        trans.TransitionIn();
    }
    public void ChangeScene()
    {
        trans.TransitionOut();
        SceneManager.LoadSceneAsync(1);
    }
}
