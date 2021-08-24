using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestaActive : MonoBehaviour
{
    [SerializeField] List<GameObject> panels;
    void Start()
    {
        
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            print("Changing");
            panels[1].SetActive(!panels[1].active);
        }
    }
}
