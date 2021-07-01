using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpaceshipController : MonoBehaviour
{
    public MoonlandingPlayer owner;
    [SerializeField] Rigidbody rb;
    [SerializeField] float speed;
    [SerializeField] float torque;
    [SerializeField] ParticleSystem fireFX;
    [SerializeField] float time = 30;
    Vector2 inputJoystick;
    [SerializeField] float yValue, target;
    bool up, down;
    public Vector2 wind;
    [SerializeField] RectTransform windArrow, arrowTarget;
    public int score;

    public void WindDirection(Vector2 dir)
    {
        wind = dir;
        arrowTarget.position = new Vector2(arrowTarget.position.x + dir.x * 100, arrowTarget.position.y + dir.y * 100);
        windArrow.LookAt(arrowTarget, transform.up);
    }

    public void InputRecieved(Vector2 input)
    {
        inputJoystick = input;
    }
    private void Update()
    {
        time -= Time.deltaTime;
    }
    private void FixedUpdate()
    {
        rb.AddForce(transform.up * inputJoystick.y * speed, ForceMode.Impulse);
        rb.AddTorque(-transform.forward * torque * inputJoystick.x);
        rb.AddForce(wind, ForceMode.Impulse);
    }
    private void OnCollisionEnter(Collision collision)
    {
        rb.useGravity = true;
        float x = collision.relativeVelocity.x;
        float y = collision.relativeVelocity.y;
        float angle = transform.localRotation.z * 100;

        if (angle < 0)
        {
            angle *= -1;
        }
        if (x < 0)
        {
            x *= -1;
        }
        if (y < 0)
        {
            y *= -1;
        }


        float scoref = 70 - ((x + y) * 10) - angle + time;
        score = Mathf.RoundToInt(scoref);
    }
}
