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
    private MainCharacter playerController;
    
        public string state = "None";

    [HideInInspector] public Image healthBar;
    [HideInInspector] public Image aetherBar;


    // Start is called before the first frame update
    void Start()
    {
        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<MainCharacter>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        aetherBar = GameObject.Find("AetherBar").GetComponent<Image>();

        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        healthBar.fillAmount = (float) playerController.currentHealth / playerController.maxHealth;
        aetherBar.fillAmount = (float) playerController.currentAether / playerController.maxAether;


    }
}
