using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MinigamesManager : MonoBehaviour
{
    private List<Minigames> mg;
    public Transform contentView;
    [SerializeField] RectTransform contentPos;
    [SerializeField] Canvas canvas;
    public GameObject listPrefab;
    int currentMgDisplayed;
    int outside;
    float bottom;
    float size;

    void Start()
    {
        mg = GameSaveHolder.gsh.minigames;
        size = canvas.renderingDisplaySize.y / 10;
        bottom = canvas.renderingDisplaySize.y;
        contentView.GetComponent<FlexibleGridLayout>().cellSize = new Vector2(0, size);
        contentView.GetComponent<RectTransform>().sizeDelta = new Vector2(contentView.GetComponent<RectTransform>().sizeDelta.x, mg.Count * size);
        
        foreach (Minigames g in mg)
        {
            GameObject temp = Instantiate(listPrefab, contentView);
            temp.transform.GetChild(0).GetComponent<Text>().text = g.nameMinigame;
            if(contentPos.transform.childCount == 1)
            {
                temp.transform.GetChild(1).gameObject.SetActive(true);
            }
        }
        ServerCalls.sc.ChangeScene(SceneManager.GetActiveScene().name);

    }

    public void PressedDirection(int direction)
    {
        if (direction == 0)
        {
            if(currentMgDisplayed > 0)
            {
                contentPos.transform.GetChild(currentMgDisplayed).transform.GetChild(1).gameObject.SetActive(false);
                currentMgDisplayed--;
                contentPos.transform.GetChild(currentMgDisplayed).transform.GetChild(1).gameObject.SetActive(true);

                if ((currentMgDisplayed * size + size) <= (bottom - canvas.renderingDisplaySize.y))
                {
                    outside--;
                    contentPos.anchoredPosition = new Vector2(contentPos.anchoredPosition.x, currentMgDisplayed * size);
                    bottom = (currentMgDisplayed * size) + canvas.renderingDisplaySize.y;
                }
            }
        }
        if (direction == 1)
        {
            if (currentMgDisplayed < mg.Count-1)
            {
                contentPos.transform.GetChild(currentMgDisplayed).transform.GetChild(1).gameObject.SetActive(false);
                currentMgDisplayed++;
                contentPos.transform.GetChild(currentMgDisplayed).transform.GetChild(1).gameObject.SetActive(true);
               
                if (bottom < currentMgDisplayed * size + size)
                {
                    outside++;
                    contentPos.anchoredPosition = new Vector2(contentPos.anchoredPosition.x, outside * size);
                    bottom = currentMgDisplayed * size + size;
                }
            }
        }
        if (direction == 2)
        {
            print("Load minigame");
            StartCoroutine(StartMinigame());
        }
    }
    IEnumerator StartMinigame()
    {
        yield return new WaitForSeconds(2);
        SceneManager.LoadScene(mg[currentMgDisplayed].nameMinigame);
    }
}
