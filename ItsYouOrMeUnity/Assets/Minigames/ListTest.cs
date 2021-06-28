using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ListTest : MonoBehaviour
{
    public List<int> test;

    private void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            int r = Random.Range(0, 100);
            int p = Random.Range(0, test.Count);
            print("add " + r + " at pos = " + p);
            test.Insert(p, r);
        }
    }

}
