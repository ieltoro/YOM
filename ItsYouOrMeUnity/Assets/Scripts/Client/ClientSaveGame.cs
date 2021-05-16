using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientSaveGame : MonoBehaviour
{
    public static ClientSaveGame csg;
    private void Awake()
    {
        if (ClientSaveGame.csg == null)
        {
            ClientSaveGame.csg = this;
        }
        else
        {
            if (ClientSaveGame.csg != this)
            {
                Destroy(this.gameObject);
            }
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public string playerName;
    public int cosmetic = 2;
    public string playerID;
    public bool dead;
    public GameObject localPlayer;
    public List<GameObject> players;
}
