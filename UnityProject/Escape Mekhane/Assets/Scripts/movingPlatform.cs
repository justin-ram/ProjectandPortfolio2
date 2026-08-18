using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointB;
    [SerializeField] int movementSpeed;
    [SerializeField] float delayTimer;
    [SerializeField] GameObject platform;

    Vector3 startingPos;
    Vector3 targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = platform.transform.position;
        targetPos = pointB.transform.position;
        StartCoroutine(movePlatform());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = platform.transform;
        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.transform.parent = null;
        }
    }

    IEnumerator movePlatform()
    {
        while(true)
        {
            while ((targetPos - platform.transform.position).sqrMagnitude > 0.1f)
            {
                platform.transform.position = Vector3.MoveTowards(platform.transform.position, targetPos, movementSpeed * Time.deltaTime);
                yield return null;
            }
            
            if (targetPos == startingPos)
            {
                targetPos = pointB.transform.position;
            }
            else if (targetPos == pointB.transform.position)
            {
                targetPos = startingPos;
            }

            yield return new WaitForSeconds(delayTimer);
        }
    }
}
