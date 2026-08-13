using UnityEngine;
using System.Collections;

public class playerController : MonoBehaviour, IDamage
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [Range(1, 100)][SerializeField] int HP;
    [Range(5, 10)][SerializeField] int speed;
    [Range(5, 10)][SerializeField] int jumpSpeed;
    [SerializeField] int maxJumps;
    [SerializeField] int sprintMult;
    [SerializeField] float shootFireRate;
    [SerializeField] int shootDamage;
    [SerializeField] int shootDistance;
    [SerializeField] int gravity;
    //how long before you can press dash again
    [SerializeField] float dashCoolDownTime;
    [SerializeField] int dashSpeed;
    int HPOriginal;
    int jumpCount;
    //timer for dash cool down
    float dashTimer;
    float shootTimer;
    //time before velocity is set to 0.
    float timeDashLasts;
    [SerializeField] float timeDashLastsTimer;
    [SerializeField] int interactDist;
    float healCoolDown;
    [SerializeField] int healAmount;
    [SerializeField] float healCoolDownTimer;
    float healDuration;
    [SerializeField] float healDurationTimer;

    Vector3 moveDirection;
    Vector3 playerVelocity;
    Vector3 dashDirection;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        HPOriginal = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        movement();
        sprint();
        dash();
        HealHp();
    }

    void movement()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDist, Color.blue);
        shootTimer += Time.deltaTime;
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

        if (Input.GetButton("Fire1") && shootTimer > shootFireRate)
        {
            shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            interactWith();
        }
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMult;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMult;
        }
    }
    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
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
            updatePlayerUI();
        }
        if (timeDashLasts > 0)
        {
            timeDashLasts -= Time.deltaTime;
            controller.Move(dashDirection.normalized * dashSpeed * Time.deltaTime);
        }
        if (timeDashLasts <= 0)
        {
            playerVelocity.x = 0;
        }
        if (Input.GetButtonDown("Dash") && dashTimer <= 0)
        {
            dashDirection = transform.forward;
            dashTimer = dashCoolDownTime;
            timeDashLasts = timeDashLastsTimer;
            updatePlayerUI();
        }
    }

    void shoot()
    {
        shootTimer = 0;

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDistance, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        healCoolDown = healCoolDownTimer;
        updatePlayerUI();
        StartCoroutine(flashDamage());
        if (HP <= 0)
        {
            //You lose. Pauses game and put loss screen.
            gameManager.instance.youLose();
        }
    }

    void HealHp()
    {
        if (healCoolDown > 0)
        {
            healCoolDown -= Time.deltaTime;
        }
        if (healDuration > 0)
        {
            healDuration -= Time.deltaTime;
        }
        if (healCoolDown <= 0 && HP < HPOriginal)
        {
            if (healDuration <= 0)
            {
                HP += healAmount;
                healDuration = healCoolDownTimer;
                updatePlayerUI();
            }
        }
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOriginal;
        gameManager.instance.playerDashBar.fillAmount = (float)dashTimer / dashCoolDownTime;
    }

    IEnumerator flashDamage()
    {
        gameManager.instance.damageFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.damageFlash.SetActive(false);
    }
    void interactWith()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDist, ~ignoreLayer))
        {
            Debug.Log(hit.collider.name);
            IInteract interact = hit.collider.GetComponent<IInteract>();
            if (interact != null)
            {
                interact.Interact();
            }
        }
    }

}
