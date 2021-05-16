using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MinigameDisplay : MonoBehaviour
{
    [SerializeField] Image img;
    [SerializeField] Text text;
    public Minigames mini;
    public void SetMinigame(Minigames miniG)
    {
        mini = miniG;
        img.sprite = mini.sprite;
        text.text = mini.nameMinigame;
    }
}
