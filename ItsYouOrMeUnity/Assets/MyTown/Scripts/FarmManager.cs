using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarmManager : MonoBehaviour
{
    private int currentLevel;
    [SerializeField] GameObject[] levels;
    [SerializeField] GameObject houseUi;

    public void SetUpBuilding(int level)
    {
        currentLevel = level;
        levels[currentLevel - 1].SetActive(true);
    }
    public void PressedBuilding()
    {
        print("WELCOME TO MY HOUSE");
        houseUi.SetActive(true);
    }
    public void PressedUpgrade()
    {
        if (ClientSaveGame.csg.pBalance.coins > 100)
        {
            levels[currentLevel - 1].SetActive(false);
            levels[currentLevel].SetActive(true);
            currentLevel++;
        }
        else
        {
            Debug.Log("Need more coins");
        }
    }
    public void ExitBuilding()
    {
        houseUi.SetActive(false);
    }
}
