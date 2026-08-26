using UnityEngine;

public class jumpPowerup : MonoBehaviour 
{
    [SerializeField] int increaseAmount;
    private void OnTriggerEnter(Collider other)
    {
        IPickup pickup = other.GetComponent<IPickup>();
        if(pickup != null)
        {
            pickup.jumpPowerUp(increaseAmount);
            Destroy(gameObject);
        }
    }
 
}
