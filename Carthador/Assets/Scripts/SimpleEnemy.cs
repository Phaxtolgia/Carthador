using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : MonoBehaviour
{

    public int maxHealth = 30;
    [HideInInspector] public int currentHealth;

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.currentHealth <= 0)
            Destroy(this.gameObject);
        
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        print(collision.collider.tag);
        if (collision.collider.tag == "Arrow")
        {
            Destroy(collision.collider.gameObject);
            this.currentHealth -= 10;
        }
    }
}
