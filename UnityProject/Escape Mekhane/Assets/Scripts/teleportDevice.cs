using UnityEngine;

public class teleportDevice : MonoBehaviour, IInteract
{
    [SerializeField] GameObject pointB;
    public void Interact()
    {
        gameManager.instance.playerScript.teleportPlayer(pointB.transform.position);
    }
}
