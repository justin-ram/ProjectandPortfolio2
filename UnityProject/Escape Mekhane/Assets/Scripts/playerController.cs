using UnityEngine;

public class playerController : MonoBehaviour
{
    [SerializeField] CharacterController controller;

    [Range(1, 10)][SerializeField] int HP;
    [Range(5, 10)][SerializeField] int speed;
    [Range(5, 10)][SerializeField] int jumpSpeed;
    [SerializeField] int maxJumps;
    [SerializeField] int sprintMult;
    [SerializeField] float fireRate;
    [SerializeField] int gravity;
    //how long before you can press dash again
    [SerializeField] float dashCoolDownTime;
    [SerializeField] int dashSpeed;
    int HPOriginal;
    int jumpCount;
    //timer for dash cool down
    float dashTimer;

    //time before velocity is set to 0.
    float timeDashLasts;
    [SerializeField] float timeDashLastsTimer;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    Vector3 dashDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        dash();
    }

    void movement()
    {
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity.y = 0;
        }

        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);
        
       
        jump();
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

       
        
    }

    void sprint()
    {
        if(Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMult;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMult;
        }
    }
    void jump()
    {
        if(Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }
    }

    void dash()
    {
        if (dashTimer > 0)
        {
            dashTimer -= Time.deltaTime;
        }
        if (timeDashLasts > 0)
        {
            timeDashLasts -= Time.deltaTime;
            controller.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
        }
        if (timeDashLasts <= 0)
        {
            dashTimer = 0;
            playerVelocity.x = 0;
        }
        if(Input.GetButtonDown("Dash") && dashTimer <= 0)
        {
            dashDirection = transform.forward;
            dashTimer = dashCoolDownTime;
            timeDashLasts = timeDashLastsTimer;
        }
    }
}
