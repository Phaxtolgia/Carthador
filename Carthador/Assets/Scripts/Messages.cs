﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Messages : MonoBehaviour
{

    [HideInInspector] public List<string> messages;
    [HideInInspector] public string message;
    private int page = 0;

    private Game game;

    private bool firstMessage;

    // Start is called before the first frame update
    void Start()
    {
        game = Camera.main.GetComponent<Game>();
        
    }

    // Update is called once per frame
    void Update()
    {

        if (message != "")
        {
            messages = new List<string>(message.Split("/".ToCharArray()));
            firstMessage = true;
            message = "";
        }


        if (this.GetComponent<Text>().text == "" && messages.Count != 0)
        {
            Time.timeScale = 0;
            this.GetComponent<Text>().text = messages[0];

        }
        else if (page < messages.Count)
            this.GetComponent<Text>().text = messages[page];
        else if (page >= messages.Count && game.state != "NearNPC")
        {
            this.GetComponent<Text>().text = "";
            message = "";
            messages.Clear();
            page = 0;

            Time.timeScale = 1;


            this.transform.parent.gameObject.SetActive(false);
        }

        if (Input.GetButtonDown("Fire1") && game.state == "Talking" && !firstMessage)
        {

            this.GetComponent<AudioSource>().Play();
            page++;
        }

        if (firstMessage)
            firstMessage = false;

    }
}
