using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeaveShop : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public void Leave()
	{
		GameObject innMenu = GameObject.Find("InnMenu");

        GameObject.FindGameObjectWithTag ("MainCamera").GetComponent <Game> ().state = "None";
		
		innMenu.SetActive(false);
		
		Time.timeScale = 1;
				
	}
	
}
