using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSaveHolder : MonoBehaviour
{
    public int playerBalance;
    public string playerName;
    public string playerId;
    public int wins;

    public void Setinfo(string id, string name, int balance)
    {
        playerId = id;
        playerName = name;
        playerBalance = balance;
    }
}
