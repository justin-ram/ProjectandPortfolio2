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

    int HPOriginal;
    int jumpCount;

    Vector3 moveDirection;
    Vector3 playerVelocity;

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
}
