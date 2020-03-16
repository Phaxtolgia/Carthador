﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SpawnImportantObjects : MonoBehaviour
{

    private GameObject player;
    private  GameObject camera;

    private GameObject canvas;


    public void Awake () {

        camera = GameObject.Instantiate (Resources.Load <GameObject> ("Main Camera"),new Vector3 (0, 0, -10), Quaternion.identity);
        player = GameObject.Instantiate (Resources.Load <GameObject> ("Player"), new Vector3 (0, 0, 0), Quaternion.identity);
        canvas = GameObject.Instantiate (Resources.Load <GameObject> ("Canvas"));

        player.SetActive (false);
        camera.SetActive (false);
        canvas.SetActive (false);
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnStartMenu_NewGame () {

        player.SetActive (true);
        camera.SetActive(true);
        canvas.SetActive (true);

        SceneManager.LoadScene ("China");


    }

}
