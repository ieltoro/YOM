using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalanceMazePlayer : MonoBehaviour
{
    [SerializeField] GameObject prefab;
    public Vector2 input, pos;
    [SerializeField] float speed, lerpSpeed;
    [SerializeField] Rigidbody rb;
    Vector3 vPos;
    private void Start()
    {
        //Instantiate(prefab, this.transform);
    }

    private void Update()
    {
        // Move object
        //input = new Vector2(Input.acceleration.x, Input.acceleration.y);
        //input *= speed;
        //pos = Vector2.Lerp(pos, input, lerpSpeed);
        //transform.eulerAngles = new Vector3(pos.y , 0, -pos.x);
        //transform.eulerAngles += new Vector3(input.y, 0, -input.x) * speed * Time.deltaTime; // Dosnt go back to 0,0,0 when phone is 0,0,0. Stays where it is 

        //Move ball
        input = new Vector2(Input.acceleration.x, Input.acceleration.y);
        input *= speed;
        vPos = new Vector3(input.y, 0, -input.x);
        rb.AddTorque(vPos, ForceMode.VelocityChange);

        //if (!isServer)
        //{

        //}
        //if (isServer)
        //{

        //}
    }

    //[Command]
    //void CMD_SendInput(Vector2 v)
    //{
    //    input = v;
    //}
}
