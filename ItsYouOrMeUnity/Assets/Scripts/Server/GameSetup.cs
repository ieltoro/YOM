using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameSetup : MonoBehaviour
{
    GameSaveHolder save;
    ServerCalls sc;
    public List<GameObject> players;
    [SerializeField] Timerhandler time;
    [SerializeField] TransitionSize trans;
    [SerializeField] GameObject background;
    [Tooltip(" 0 = roundstart \n 1 = ChooseMinigame")]
    [SerializeField] GameObject[] UI;
    public static int startPositionIndex;
    public List<Transform> startPositions;

    #region Start
    private void Start()
    {
        sc = FindObjectOfType<ServerCalls>();
        save = GameSaveHolder.gsh;
        foreach (GameObject g in save.players)
        {
            YOMNetworkManager.manager.SpawnNewPlayer(g, 4);
        }
        StartCoroutine(StartGameCD());
    }
    IEnumerator StartGameCD()
    {
        yield return new WaitForSeconds(1);
        trans.TransitionIn();
    }
    public Transform GetSpawnPos()
    {
        Transform pos = startPositions[startPositionIndex];
        startPositionIndex++;
        return pos;
    }


    #endregion

}
