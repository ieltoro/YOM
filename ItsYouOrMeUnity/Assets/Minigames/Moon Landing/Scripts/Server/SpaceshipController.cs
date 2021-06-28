using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    [SerializeField] Rigidbody rb;
    public float speed;
    public float torque;
    [SerializeField] ParticleSystem fireFX;
    [SerializeField] float time = 30;
    Vector2 inputJoystick;
    

    public void InputRecieved(Vector2 input)
    {
        inputJoystick = input;
    }
    private void Update()
    {
        inputJoystick = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        inputJoystick.y = Mathf.Clamp(inputJoystick.y, -0.4f, 1);

        time -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        //if (inputJoystick.y > 0.05)
        //{
        //    if (!fireFX.isPlaying)
        //        fireFX.Play();
        //}
        //else
        //{
        //    fireFX.Stop();
        //}
        
        rb.AddForce(transform.up * inputJoystick.y * speed, ForceMode.Impulse);
        rb.AddTorque(-transform.forward * torque * inputJoystick.x);
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
        float x = collision.relativeVelocity.x;
        float y = collision.relativeVelocity.y;
        if(x < 0)
        {
            x *= -1;
        }
        if (y < 0)
        {
            y *= -1;
        }
        float score = 70 - ((x + y) * 10) + time;
        print(collision.relativeVelocity + " score is " + score);
    }
}
