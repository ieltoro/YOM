using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleshipClient : MonoBehaviour
{
    public BattleshipPlayer bp;
    public List<string> otherPlayers;
    void Start()
    {
        FindObjectOfType<ClientSaveGame>().localPlayer.GetComponent<PlayerScript>().MinigameConnectedTo(0);
    }

   
    void Update()
    {
        
    }
}
