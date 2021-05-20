using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class PlayerInputTest : NetworkBehaviour
{
    [SerializeField] GameObject prefab, canvas, myPlayer;
    [SerializeField] Rigidbody rb;
    Vector2 input;
    Joystick joystick;
   
    void Start()
    {

        if (hasAuthority)
        {
            joystick = FindObjectOfType<Joystick>();
        }
        if (isServer)
        {
            myPlayer = Instantiate(myPlayer);
            rb = myPlayer.GetComponent<Rigidbody>();
        }
    }

    private void FixedUpdate()
    {
        if (!isServer)
        {
            input = new Vector2(joystick.Horizontal, joystick.Vertical);
            print("INput " + input);
            rb.AddForce(new Vector3(input.x, 0, input.y), ForceMode.VelocityChange);
            //CMD_SendInput(input);
        }
        if (isServer)
        {
            rb.AddForce(new Vector3(input.x, 0, input.y), ForceMode.VelocityChange);
        }
    }
    [Command]
    void CMD_SendInput(Vector2 pos)
    {
        input = pos;
    }
}
