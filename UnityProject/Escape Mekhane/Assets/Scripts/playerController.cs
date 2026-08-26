using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class playerController : MonoBehaviour, IDamage, IPickup
{
    [SerializeField] CharacterController controller;
    [SerializeField] LayerMask ignoreLayer;

    [Header("Stats")]
    [Range(1, 100)][SerializeField] int HP;
    [Range(5, 10)][SerializeField] int speed;
    [Range(5, 10)][SerializeField] int jumpSpeed;
    [SerializeField] int maxJumps;
    [SerializeField] int sprintMult;
    [SerializeField] int gravity;
    //how long before you can press dash again
    [SerializeField] float dashCoolDownTime;
    [SerializeField] int dashSpeed;
    int HPOriginal;
    int jumpCount;
    //timer for dash cool down
    float dashTimer;
    float shootTimer;
    [SerializeField] float timeDashLastsTimer;
    //time before velocity is set to 0.
    float timeDashLasts;
    [SerializeField] int interactDist;
    float healCoolDown;
    [SerializeField] int healAmount;
    [SerializeField] float healCoolDownTimer;
    float healDuration;
    [SerializeField] float healDurationTimer;

    [Header("Gun Stuff")]
    [SerializeField] List<gunStats> gunInv = new List<gunStats>();
    [SerializeField] GameObject gunModel;

    [Header("Audio")]
    public AudioClip[] audJumpSound;
    [Range(0, 1)][SerializeField] float audJumpVol;
    public AudioClip[] audStepsSound;
    [Range(0, 1)][SerializeField] float audStepsVol;
    public AudioClip[] audDashSound;
    [Range(0, 1)][SerializeField] float audDashVol;
    public AudioClip[] audHurtSound;
    [Range(0, 1)][SerializeField] float audHurtVol;
    int gunInvPos;
    bool isSprinting;
    bool isPlayingSteps;

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
        interactUpdateUi();
    }

    void movement()
    {
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * gunInv[gunInvPos].shootDistance, Color.red);
        //Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * interactDist, Color.blue);
        shootTimer += Time.deltaTime;
        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity.y = 0;
            
            if(moveDirection.magnitude > 0.3 && !isPlayingSteps)
            {
                StartCoroutine(playSteps());
            }
        }

        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDirection.normalized * speed * Time.deltaTime);


        jump();
        controller.Move(playerVelocity * Time.deltaTime);
        playerVelocity.y -= gravity * Time.deltaTime;

        if (Input.GetButton("Fire1") && gunInv.Count > 0 &&shootTimer > gunInv[gunInvPos].shootFireRate)
        {
            shoot();
        }

        if (Input.GetButtonDown("Fire2"))
        {
            interactWith();
        }

        selectGun();
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMult;
            isSprinting = true;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMult;
            isSprinting = false;
        }
    }

    IEnumerator playSteps()
    {
        isPlayingSteps = true;
        audioManager.instance.audPlayer.PlayOneShot(audStepsSound[Random.Range(0, audStepsSound.Length)], audStepsVol);
        if(isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.5f);
        }
        isPlayingSteps = false;
    }
    void jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumps)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
            audioManager.instance.audPlayer.PlayOneShot(audJumpSound[Random.Range(0, audJumpSound.Length)], audJumpVol);
        }
    }

    public void jumpPowerUp(int amount)
    {
        maxJumps += amount;
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
            StartCoroutine(invincibilityWindow());
            audioManager.instance.audPlayer.PlayOneShot(audDashSound[Random.Range(0, audDashSound.Length)], audDashVol);
        }
    }

    IEnumerator invincibilityWindow()
    {
        gameObject.layer = LayerMask.NameToLayer("Invincible");
        yield return new WaitForSeconds(timeDashLastsTimer);
        gameObject.layer = LayerMask.NameToLayer("Player");
    }
    void shoot()
    {
        shootTimer = 0;
        audioManager.instance.audPlayer.PlayOneShot(gunInv[gunInvPos].shootSound[Random.Range(0, gunInv[gunInvPos].shootSound.Length)], gunInv[gunInvPos].shootSoundVol);

        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, gunInv[gunInvPos].shootDistance, ~ignoreLayer))
        {
            // Debug.Log(hit.collider.name);
          
            Instantiate(gunInv[gunInvPos].hitEffect, hit.point, Quaternion.identity);
            IDamage dmg = hit.collider.GetComponent<IDamage>();
            if (dmg != null)
            {
                dmg.takeDamage(gunInv[gunInvPos].shootDamage);
            }
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        audioManager.instance.audPlayer.PlayOneShot(audHurtSound[Random.Range(0, audHurtSound.Length)], audHurtVol);
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

    public void getGunStats(gunStats gun)
    {
        gunInv.Add(gun);
        gunInvPos = gunInv.Count - 1;
        changeGunModel();
    }

    void changeGunModel()
    {
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunInv[gunInvPos].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunInv[gunInvPos].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }
    void selectGun()
    {
        if(Input.GetAxis("Mouse ScrollWheel") > 0 && gunInvPos < gunInv.Count - 1)
        {
            gunInvPos++;
            changeGunModel();
        }
        else if(Input.GetAxis("Mouse ScrollWheel") < 0 && gunInvPos > 0)
        {
            gunInvPos--;
            changeGunModel(); 
        }
    }
    IEnumerator flashDamage()
    {
        gameManager.instance.damageFlash.SetActive(true);
        yield return new WaitForSeconds(0.3f);
        gameManager.instance.damageFlash.SetActive(false);
    }

    void interactUpdateUi()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDist, ~ignoreLayer))
        {
           // Debug.Log(hit.collider.name);
            IInteract interact = hit.collider.GetComponent<IInteract>();
            gameManager.instance.showInteract(interact);
        }
        else
        {
            gameManager.instance.disableInteract();
        }
    }
    void interactWith()
    {
        RaycastHit hit;

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, interactDist, ~ignoreLayer))
        {
          //  Debug.Log(hit.collider.name);
            IInteract interact = hit.collider.GetComponent<IInteract>();
            if (interact != null)
            {
                interact.Interact();
            }
        }
    }


}
