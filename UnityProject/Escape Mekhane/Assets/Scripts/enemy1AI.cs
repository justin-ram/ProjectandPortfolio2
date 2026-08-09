using UnityEngine;
using System.Collections;

public class enemy1AI : MonoBehaviour, IDamage
{
    [Range(1,10)][SerializeField] int HP;
    [SerializeField] Renderer model;
    Color colorOrig;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        colorOrig = model.material.color;
    }

    // Update is called once per frame
    void Update()
    {
        
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
}
