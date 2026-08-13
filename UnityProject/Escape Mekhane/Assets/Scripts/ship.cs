using UnityEngine;

public class ship : MonoBehaviour, IInteract
{
    public void Interact()
    {
        endGame();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void endGame()
    {
        //if all ship parts are collected play win menu 
        
        gameManager.instance.shipWin();
    }
    
}
