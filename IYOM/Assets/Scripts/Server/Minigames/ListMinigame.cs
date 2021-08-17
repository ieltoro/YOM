using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListMinigame : MonoBehaviour
{
    public Minigames game;
    [SerializeField] Text text;
    public void SetInfo(Minigames g)
    {
        game = g;
        text.text = game.nameMinigame;
    }
}
