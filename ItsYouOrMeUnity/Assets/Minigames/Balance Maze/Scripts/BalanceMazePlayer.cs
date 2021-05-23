using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMazePlayer : MonoBehaviour
{
    Vector2 input;

    private void Update()
    {
        input = new Vector2(Input.acceleration.x, Input.acceleration.y);
        transform.rotation = new Quaternion(input.x, 0, input.y,0);
    }
}
