using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderManager : MonoBehaviour
{
    public Slider slider;
    [SerializeField] float lastTarget, target, curr, moveTime;
    [SerializeField] float speed;


    void Update()
    {
        moveTime += Time.deltaTime * speed;
        curr = Mathf.Lerp(lastTarget, target, moveTime);
        slider.value = curr;
        if( slider.value == 100)
        {
            moveTime = 0;
            lastTarget = 100;
            target = -100;
        }
        if (slider.value == -100)
        {
            moveTime = 0;
            lastTarget = -100;
            target = 100;
        }
        if(Input.GetButtonDown("Jump"))
        {
            float r = Random.Range(0.1f, 2f);
            print(GetValue(r) + "  ,  " + r);
        }
    }
    float v;
    public float GetValue(float ping)
    {
        if(target == 100)
        {
            v = slider.value - ping;
            if(v < -100)
            {
                float xtra = v + 100;
                v += xtra;
            }
        }
        if (target == -100)
        {
            v = slider.value + ping;
            if (v > 100)
            {
                float xtra = v - 100;
                v -= xtra;
            }
        }
        return v;
    }
}
