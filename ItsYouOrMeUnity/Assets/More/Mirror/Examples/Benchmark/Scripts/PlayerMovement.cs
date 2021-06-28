using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    
    [SerializeField] Rigidbody rb;
    Vector2 input;
    Vector3 pos;
    Quaternion toR;
    public float speed = 2, rotspeed = 8, jump = 2;
    
    
    [Header("GroundChecker")]
    public  bool grounded;
    [SerializeField] GameObject groundCheck;
    [SerializeField] float checkDistance = 0.02f;
    RaycastHit hit;

    [Header("SpineMove")]
    [SerializeField] GameObject targetSpine;
    [SerializeField] GameObject targetSpineTarget;
    Vector3 startPos;

    [Header("Animation")]
    [SerializeField] Animator anim;
    bool walking;
    float xposA , yposA , speedW;

    void Update()
    {
        #region Raycast Ground check
        //grounded = false;
        //if (Physics.Raycast(groundCheck.transform.position, groundCheck.transform.TransformDirection(Vector3.forward), out hit, checkDistance))
        //{
        //    grounded = true;
        //}
        #endregion
        #region Movement Input
        input = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        if (input.x < -0.03 || input.x > 0.03 || input.y < -0.03 || input.y > 0.03)
        {
            float angle = Mathf.Atan2(input.y, input.x) * Mathf.Rad2Deg - 90;
            toR = Quaternion.Euler(0, -angle, 0);
        } // Rotation
        if (grounded)
        {
           
            pos = new Vector3(input.x, 0, input.y);
            if (Input.GetButtonDown("Jump"))
            {
                rb.AddForce(new Vector3(input.x * 20, jump * 100, input.y * 20), ForceMode.Impulse);
            }
        }
        #endregion
        #region Spine

        targetSpine.transform.position = targetSpineTarget.transform.position;

        #endregion
        #region Animations

        xposA = Input.GetAxis("Horizontal");
        if (xposA < 0)
            xposA *= -1;
        yposA = Input.GetAxis("Vertical");
        if (yposA < 0)
            yposA *= -1;
        speedW = xposA + yposA;
        speedW = Mathf.Clamp(speedW, 0, 1);
        
        print(speedW);
        anim.SetFloat("Speed", speedW);
        if (grounded)
        {
            anim.SetBool("Grounded", grounded);
        }
        #endregion
    }
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + pos * speed * Time.fixedDeltaTime);
        rb.MoveRotation(Quaternion.Lerp(rb.rotation, toR, Time.deltaTime * rotspeed));
    }
}
