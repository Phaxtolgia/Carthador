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

    [HideInInspector] public GameObject inventory;
    [HideInInspector] public Inventory inventoryScript;
    private List <GameObject> inventoryItems;


    [HideInInspector] public bool isGamePaused = false;



    public GameObject player;
    private MainCharacter playerController;
    
        public string state = "None";

    [HideInInspector] public Image healthBar;
    [HideInInspector] public Image aetherBar;


    // Start is called before the first frame update
    void Start()
    {

        Physics2D.IgnoreLayerCollision(8, 9);
        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<MainCharacter>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        aetherBar = GameObject.Find("AetherBar").GetComponent<Image>();

        this.inventory = GameObject.Find ("Inventory");
        this.inventory.SetActive(false);
        this.inventoryScript = this.GetComponent<Inventory>();
        this.inventoryItems = new List <GameObject> ();

        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        healthBar.fillAmount = (float) playerController.currentHealth / playerController.maxHealth;
        aetherBar.fillAmount = (float) playerController.currentAether / playerController.maxAether;

        if (Input.GetButtonDown ("Inventory")){
            
            if (!inventory.activeSelf) {
                
                inventory.SetActive (true);
                this.isGamePaused = true;
                Time.timeScale = 0;
                this.fillInventory ();
            }
            else if (inventory.activeSelf) {
                
                inventory.SetActive (false);
                this.isGamePaused = false;
                Time.timeScale = 1;
            }

        }

    }


    private void fillInventory () {

        float panelWidth = Screen.width;
        float panelHeight = Screen.height;

        float squareRoot = Mathf.Sqrt (inventoryScript.maxItems);

        float itemSize = panelHeight / Mathf.Ceil (squareRoot) / 2;

        int columnsCounter = 1;
        int rowsCounter = 0;


        for (int i = 0;i < inventoryScript.maxItems; i++) {

            float posX = 0;
            float posY = 0;

            
            if (i == 0) {

                posX = panelWidth - itemSize / 2;
                posY = itemSize / 2;
            }
            else if (rowsCounter == 0) {
                posX = inventoryItems[i - 1].transform.position.x - itemSize;
                posY = inventoryItems [i - 1].transform.position.y;
            }
            else {
                posX = inventoryItems[i - (int) squareRoot].transform.position.x;
                posY = inventoryItems[i - (int) squareRoot].transform.position.y + itemSize;
            }
            


            string path = "Inventory/Empty";

            if ( i < inventoryScript.items.Count)
                path = "Inventory/" + inventoryScript.items[i];

            inventoryItems.Add ( GameObject.Instantiate (Resources.Load <GameObject> (path), inventory.transform) );
            inventoryItems[i].transform.position = new Vector3 (posX, posY, 0);
            
            
            inventoryItems[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemSize, itemSize);


            columnsCounter++;
            if (columnsCounter > squareRoot) {
                columnsCounter = 1;
                rowsCounter++;
            }
        }
    }
}
