using UnityEngine;
using System.Collections;

public class SlidingDoor : MonoBehaviour, IInteract
{
    [SerializeField] GameObject pointB;
    [SerializeField] int movementSpeed;

    Vector3 startingPos;
    Vector3 targetPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startingPos = transform.position;
        targetPos = pointB.transform.position;
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator moveDoor()
    {
        while ((targetPos - transform.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movementSpeed * Time.deltaTime);
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
    }

    public void Interact()
    {
        StartCoroutine(moveDoor());
    }
}
