using UnityEngine;
using System.Collections;
using UnityEngine.AI;


public class enemy1AI : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    
    [SerializeField] Renderer model;
    [Header("Stats")]
    [Range(1, 50)][SerializeField] int HP;
    [SerializeField] int faceTargetSpeed;
    [Header("Weapons")]
    [SerializeField] GameObject bullet;
    [SerializeField] Transform gunPivot;
    [SerializeField] Transform shootPos;
    [SerializeField] float shootRate;
    [SerializeField] int gunRotateSpeed;
   
    Color colorOrig;

    Vector3 playerDir;
    float shootTimer;
    bool playerInTrigger;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
        
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInTrigger)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);
            shootTimer = shootTimer+ Time.deltaTime;
            playerDir = gameManager.instance.player.transform.position - transform.position;
            faceTarget();
            rotateGun();
            if (shootTimer >= shootRate)
            {
                shoot();
            }
        }
        
        

    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerInTrigger = true;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        playerInTrigger = false;
    }
    void faceTarget()
    {
        Quaternion rot =Quaternion.LookRotation(new Vector3 (playerDir.x,0,playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, faceTargetSpeed*Time.deltaTime);
    }
    void rotateGun()
    {
        Quaternion rot = Quaternion.LookRotation(playerDir);
        gunPivot.rotation = Quaternion.Lerp(gunPivot.rotation, rot, gunRotateSpeed * Time.deltaTime);
    }
    public void takeDamage(int amount)
    {
        HP -= amount;
        if(HP<=0)
        {
            Destroy(gameObject);
        }
        else
        {
            StartCoroutine(flashRed());
        }
    }
    IEnumerator flashRed()
    {
        model.material.color = Color.red;
        yield return new WaitForSeconds(0.01f);
        model.material.color = colorOrig;
    }
    void shoot()
    {
        shootTimer = 0;
       
        Instantiate(bullet, shootPos.position, transform.rotation);
        
       

    }
}
