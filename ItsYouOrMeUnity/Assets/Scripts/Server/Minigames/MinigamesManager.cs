using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigamesManager : MonoBehaviour
{
    private List<Minigames> mg;
    public Transform contentView;
    public GameObject listPrefab;
    int currentMgDisplayed;

    void Start()
    {
        mg = GameSaveHolder.gsh.minigames;

        contentView.GetComponent<FlexibleGridLayout>().cellSize = new Vector2(0, 200);
        contentView.GetComponent<RectTransform>().sizeDelta = new Vector2(contentView.GetComponent<RectTransform>().sizeDelta.x, mg.Count * 200);

        print(mg.Count);
        foreach (Minigames g in mg)
        {
            GameObject temp = Instantiate(listPrefab, contentView);
            temp.transform.GetChild(0).GetComponent<Text>().text = g.nameMinigame;
        }
    }

    public void PressedDirection(int direction)
    {
        if(direction == 0)
        {
            currentMgDisplayed--;
            if(currentMgDisplayed < 0)
            {
                currentMgDisplayed = mg.Count - 1;
            }
        }
        if (direction == 1)
        {
            currentMgDisplayed++;
            if (currentMgDisplayed >= mg.Count)
            {
                currentMgDisplayed = 0;
            }
        }
        if(direction == 2)
        {
            print("Load minigame");
            StartCoroutine(StartMinigame());
        }
        print("current minigame = " + mg[currentMgDisplayed].nameMinigame);
    }
    IEnumerator StartMinigame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(mg[currentMgDisplayed].nameMinigame);
    }
}
