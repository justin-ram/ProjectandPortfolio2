using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour
{
    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] int movementSpeed;
    [SerializeField] float delayTimer;
    [SerializeField] GameObject platform;

    Vector3 targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        platform.transform.position = pointA.transform.position;
        targetPos = pointB.transform.position;
        StartCoroutine(movePlatform());
    }

    // Update is called once per frame
    void Update()
    {
        
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
            
            if (targetPos == pointA.transform.position)
            {
                targetPos = pointB.transform.position;
            }
            else if (targetPos == pointB.transform.position)
            {
                targetPos = pointA.transform.position;
            }

            yield return new WaitForSeconds(delayTimer);
        }
    }
}
