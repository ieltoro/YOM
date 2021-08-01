using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballController : MonoBehaviour
{
    public FootballPlayer owner;
    [SerializeField] float speed;
    [SerializeField] Rigidbody rb;
    Vector2 inputController;
    Vector3 vPos;
    
    void Start()
    {
        
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
    }
}
