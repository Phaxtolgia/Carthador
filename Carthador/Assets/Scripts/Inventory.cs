using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{

    [HideInInspector] public List <string> items;
    public int maxItems;

    // Start is called before the first frame update
    void Start()
    {

        maxItems = 10;

        items.Add("Health Potion");
        items.Add("Aether Potion");
        items.Add("Health Potion");
        items.Add("Aether Potion");
        items.Add("Health Potion");
        items.Add("Aether Potion");
        items.Add("Health Potion");
        items.Add("Aether Potion");
        items.Add("Health Potion");
        items.Add("Aether Potion");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
