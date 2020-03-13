using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Game : MonoBehaviour
{

    public int gamePhase = 0;
    public string currentQuest;
    public int currentQuestPhase;
    
    public Text messages;
    public GameObject messagesParent;
    public GameObject player;
    public string state = "None";


    // Start is called before the first frame update
    void Start()
    {
        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");

        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
