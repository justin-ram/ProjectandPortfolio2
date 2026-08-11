using UnityEngine;

public class ShipItems : MonoBehaviour , IInteract
{
    public void Interact()
    {
        updateShipItemsCount();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //Interact();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void updateShipItemsCount()
    {
        //get the shipitemscount from ui +1 it then after that destroy it
        Destroy(gameObject);
    }
}
