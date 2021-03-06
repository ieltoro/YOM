using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneholderController : MonoBehaviour
{
    public ZoneholderPlayer owner;
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    Vector2 inputController;
    Vector3 vPos;
    public float size;
    [SerializeField] float scoreSecond;
    bool inside;

    public void StartMoving(bool b)
    {
        enabled = b;
        print(b);
    }

    public void InputRecieved(Vector2 input)
    {
        inputController = input;
    }
    void Update()
    {
        inputController *= speed;
        vPos = new Vector3(inputController.y, 0, -inputController.x);
        rb.AddTorque(vPos, ForceMode.VelocityChange);
        if(inside)
        {
            size += scoreSecond;
        }
    }
    public void InsideZone(bool zone)
    {
        inside = zone;
    }
}
