using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSlider : MonoBehaviour
{
    float screenW, screenH;

    [SerializeField] RectTransform content, bar;
    [SerializeField] Transform infoScreen, startScreen, actionsScreen;
    [SerializeField] float speed;
    public float[] posC;
    public float pos, target;
    void Start()
    {
        
        screenW = Screen.width;
        screenH = Screen.height;
        print(screenH - bar.sizeDelta.y);

        content.sizeDelta = new Vector2(screenW * 3, screenH);
        infoScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(screenW, screenH - bar.sizeDelta.y);
        startScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(screenW, screenH - bar.sizeDelta.y);
        actionsScreen.GetComponent<RectTransform>().sizeDelta = new Vector2(screenW, screenH - bar.sizeDelta.y);
        infoScreen.position =  new Vector2(screenW/2, (screenH / 2) + (bar.sizeDelta.y / 2));
        startScreen.position = new Vector2(screenW * 1.5f, (screenH / 2) + (bar.sizeDelta.y / 2));
        actionsScreen.position = new Vector2(screenW * 2.5F , (screenH / 2) + (bar.sizeDelta.y / 2));
        pos = -screenW;
        target = -screenW;
        content.position = new Vector2(-screenW, screenH);
        posC[0] = 0;
        posC[1] = -screenW;
        posC[2] = -screenW *2;
    }

    public void ChangePoss(int poss)
    {
        target = posC[poss];
    }

    private void Update()
    {
        pos = Mathf.Lerp(pos, target, speed);
        content.position = new Vector2(pos, screenH);
    }
}
