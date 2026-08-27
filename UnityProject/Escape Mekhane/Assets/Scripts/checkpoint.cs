using System.Collections;
using UnityEngine;

public class checkpoint : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player") && gameManager.instance.playerSpawnPos.transform.position != transform.position)
        {
            gameManager.instance.playerSpawnPos.transform.position = other.transform.position;
            StartCoroutine(displayPopup());
        }
    }

    IEnumerator displayPopup()
    {
        gameManager.instance.checkPointPopup.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.checkPointPopup.SetActive(false);
    }

}
