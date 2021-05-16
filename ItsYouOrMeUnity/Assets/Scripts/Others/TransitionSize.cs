using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TransitionSize : MonoBehaviour
{
    bool animate;
    float screenW, screenH;
    [SerializeField] RectTransform circle, black;
    [SerializeField] float size;
    [SerializeField] float target, start, moveTime;
    [SerializeField] float speed = 0.7f;
    bool xBiggest;

    private void Start()
    {
        screenW = Screen.width;
        screenH = Screen.height;
        if (screenH > screenW)
            xBiggest = false;
        else
            xBiggest = true;
        black.sizeDelta = new Vector2(screenW + 20, screenH + 20);
    }
    public void TransitionIn()
    {
        print("Transition In");
        size = 0;
        start = 0;
        moveTime = 0;
        circle.sizeDelta = new Vector2(size, size);
        if (xBiggest)
            target = screenW * 2;
        else
            target = screenH * 2;
        circle.transform.gameObject.SetActive(true);
        animate = true;
        this.enabled = true;
    }
    public void TransitionOut()
    {
        print("Transition Out");
        if (xBiggest)
        {
            start = screenW * 2;
            size = screenW * 2;
        }
        else
        {
            start = screenH * 2;
            size = screenH * 2;
        }
        moveTime = 0;
        circle.sizeDelta = new Vector2(size, size);
        target = 0;
        circle.transform.gameObject.SetActive(true);
        animate = true;
        this.enabled = true;
    }
    public void StopTransition()
    {
        animate = false;
        this.enabled = false;
    }
    private void Update()
    {
        if(animate)
        {
            moveTime += Time.deltaTime * speed;
            size = Mathf.Lerp(start, target, moveTime);
            circle.sizeDelta = new Vector2(size, size);
        }
    }
}

