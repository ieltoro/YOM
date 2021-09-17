using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootballController : MonoBehaviour
{
    public FootballPlayer owner;
    public Vector2 input, pos;
    Vector3 air;
    [SerializeField] float speed, lerpSpeed, jump;
    [SerializeField] Rigidbody rb;
    Vector3 vPos;
    [SerializeField] bool grounded;
    public bool canMove;
    public int teamId;

    private void Start()
    {
        FindObjectOfType<MultiCamera>().targets.Add(this.transform);
    }
    public void InputRecieved(Vector2 inp)
    {
        if(canMove)
        {
            input = inp;
        }
    }
    public void PressedJump()
    {
        rb.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
    }
    private void Update()
    {
        vPos = new Vector3(input.y, 0, -input.x).normalized;
    }
    private void FixedUpdate()
    {
        if (Input.GetButtonDown("Jump") && grounded)
        {
            print("Jump");
            rb.AddForce(new Vector3(0, jump, 0), ForceMode.Impulse);
        }
        rb.AddTorque(vPos * speed, ForceMode.Force);
    }
    private void OnCollisionStay(Collision collision)
    {
        grounded = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        StartCoroutine(JumpCD());
    }
    IEnumerator JumpCD()
    {
        yield return new WaitForSeconds(0.1f);
        grounded = false;
    }
}
