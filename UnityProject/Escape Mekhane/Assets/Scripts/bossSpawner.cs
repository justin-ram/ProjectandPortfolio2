using UnityEngine;
using UnityEngine.AI;

public class bossSpawner : MonoBehaviour
{
    [SerializeField] GameObject objectToSpawn;
    [SerializeField] GameObject enemy2;
    
    
    [SerializeField] int spawnDist;

   
    
    public bool spawn2;
    

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    

   

    public void spawn(int amount)
    {
        
       

        for (int i = 0; i < amount; i++)
        {

            Vector3 ranPos = Random.insideUnitSphere * spawnDist;

            ranPos += transform.position;

            NavMeshHit hit;

            NavMesh.SamplePosition(ranPos, out hit, spawnDist, 1);
            if (spawn2 == false)
            {
                Instantiate(objectToSpawn, hit.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
            else
            {
                Instantiate(enemy2, hit.position, Quaternion.Euler(0, Random.Range(0, 360), 0));
            }
        }
        
    }
}

