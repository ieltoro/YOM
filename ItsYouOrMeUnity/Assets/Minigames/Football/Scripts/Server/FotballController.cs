using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FotballController : MonoBehaviour
{
    public Vector2 input, pos;
    [SerializeField] float speed, lerpSpeed;
    [SerializeField] Rigidbody rb;
    Vector3 vPos;


    public void InputRecieved(Vector2 inp)
    {
        input = inp;
    }

    private void Update()
    {       
        //input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        vPos = new Vector3(input.y, 0, -input.x).normalized;
        rb.AddTorque(vPos * speed, ForceMode.Force);
    }


}
