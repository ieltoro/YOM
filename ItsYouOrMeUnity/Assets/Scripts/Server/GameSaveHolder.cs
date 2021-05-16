using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameSaveHolder : MonoBehaviour
{
    public static GameSaveHolder gsh;
    private void Awake()
    {
        if (GameSaveHolder.gsh == null)
        {
            GameSaveHolder.gsh = this;
        }
        else
        {
            if (GameSaveHolder.gsh != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public int round;
    public List<GameObject> players;
    public int playersAlive;
    public GameObject leader;
    public List<PlayerSaveHolder> playerSaves; 
    public List<GameObject> resultsLastGame;

    public void ResetPlayerList(GameObject remove)
    {
        players.Remove(remove);
    }
}
