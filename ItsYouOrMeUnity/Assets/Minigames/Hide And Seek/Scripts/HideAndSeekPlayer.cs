using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class HideAndSeekPlayer : MonoBehaviour
{
    [SerializeField] GameObject prefab, canvas, myPlayer;
    [SerializeField] Rigidbody rb;
    public float speedStart, rotspeed;
    [SerializeField] float runningTime = 10;
    float speed;
    Vector2 input;
    Vector3 pos;
    Joystick joystick;
    bool running;


    [SerializeField] Image fillIMG;
    void Start()
    {
        speed = speedStart;
        joystick = FindObjectOfType<Joystick>();
        myPlayer = Instantiate(prefab, this.transform);
        rb = myPlayer.GetComponent<Rigidbody>();

        //if (hasAuthority)
        //{
        //    print("Is mine");
        //    joystick = FindObjectOfType<Joystick>();
        //}
        //if (isServer)
        //{
        //    canvas.SetActive(false);
        //    print("IS Server");
        //    myPlayer = Instantiate(prefab);
        //    rb = myPlayer.GetComponent<Rigidbody>();
        //    FindObjectOfType<PingTestManager>().players.Add(this.gameObject);
        //}

    }
    private void Update()
    {
        if (running)
        {
            runningTime -= Time.deltaTime;
            fillIMG.fillAmount = runningTime / 10;
            if(runningTime <= 0)
            {
                Walking();
            }
        }

    }
    private void FixedUpdate()
    {
        input = new Vector2(joystick.Horizontal, joystick.Vertical);
        pos = new Vector3(input.x, 0, input.y);
        rb.MovePosition(rb.position + pos * speed * Time.fixedDeltaTime);
        //rb.AddForce(new Vector3(speed * input.x, 0, speed * input.y), ForceMode.VelocityChange);

        if (input.x < -0.1 || input.x > 0.1 || input.y < -0.1 || input.y > 0.1)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90;
            Quaternion toR = Quaternion.Euler(0, -angle, 0);
            rb.MoveRotation(Quaternion.Lerp(rb.rotation, toR, Time.deltaTime * rotspeed));
            //rb.rotation = Quaternion.Euler(0, -angle, 0);
        }





        //if (!isServer)
        //{
        //    input = new Vector2(joystick.Horizontal, joystick.Vertical);
        //    print("INput " + input);
        //    CMD_SendInput(input);
        //}
        //if (isServer)
        //{
        //    rb.AddForce(new Vector3(speed * input.x, 0, speed * input.y), ForceMode.VelocityChange);
        //}
    }
    //[Command]
    //void CMD_SendInput(Vector2 pos)
    //{
    //    input = pos;
    //}

    public void Sprinting()
    {
        if (runningTime <= 0)
            return;
        running = true;
        speed *= 1.5f;
    }
    public void Walking()
    {
        running = false;
        speed = speedStart;
    }
    public void RefilEnergy(int amount)
    {
        runningTime += amount;
    }
}
