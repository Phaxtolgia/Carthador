using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{

    public int gamePhase = 0;
    public string mainQuest;
    public string currentQuest;

    public List <string> completedQuests;
    public int currentQuestPhase;

    public Text messages;
    public GameObject messagesParent;

    [HideInInspector] public GameObject inventory;
    [HideInInspector] public Inventory inventoryScript;
    [HideInInspector]public List <GameObject> inventoryItems;


    [HideInInspector]public GameObject mainMenu;
    [HideInInspector]public GameObject innMenu;


    [HideInInspector] public bool isGamePaused = false;

    

    [HideInInspector]public GameObject player;
    private MainCharacter playerController;

    [HideInInspector] public string lastLevel = "China";
    
        public string state = "None";

    [HideInInspector] public Image healthBar;
    [HideInInspector] public Image aetherBar;
    [HideInInspector] public Text timeText;


    public int timeOfDay = 12;


    // Start is called before the first frame update


    void Start()
    {

        Physics2D.IgnoreLayerCollision(8, 9);
        state = "None";
        player = GameObject.FindGameObjectWithTag("Player");
        playerController = player.GetComponent<MainCharacter>();

        healthBar = GameObject.Find("HealthBar").GetComponent<Image>();
        aetherBar = GameObject.Find("AetherBar").GetComponent<Image>();
        timeText = GameObject.Find("TimeText").GetComponent<Text>();


        this.inventory = GameObject.Find ("Inventory");
        this.inventory.SetActive(false);
        this.inventoryScript = this.GetComponent<Inventory>();
        this.inventoryItems = new List <GameObject> ();

        messages = GameObject.Find("MessagesText").GetComponent<Text>();
        messagesParent = messages.transform.parent.gameObject;
        messagesParent.SetActive(false);
        

        mainMenu = GameObject.Find ("MainMenu");
        mainMenu.SetActive(false);

        innMenu = GameObject.Find ("InnMenu");
        innMenu.SetActive(false);



        completedQuests = new List <string> ();


        StartCoroutine (dayNightCycle());


        DontDestroyOnLoad (this.gameObject);

        SceneManager.sceneLoaded += OnLevelChanged;

    }

    // Update is called once per frame
    void Update()
    {
        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player");


        healthBar.fillAmount = (float) playerController.currentHealth / playerController.maxHealth;
        aetherBar.fillAmount = (float) playerController.currentAether / playerController.maxAether;

        if (this.timeOfDay != 0 && this.timeOfDay != 12)
            timeText.text = (this.timeOfDay % 12).ToString() + ":00";
        else if (this.timeOfDay == 0)
            timeText.text = "OO:00";
        else
            timeText.text = "12:00";

        if (Input.GetButtonDown ("Inventory") && this.state != "Talking" && this.state != "InMenu"){
            
            if (!inventory.activeSelf) {
                
                inventory.SetActive (true);
                this.isGamePaused = true;
                this.fillInventory ();

                this.state = "InInventory";

                Time.timeScale = 0;
                
            }
            else if (inventory.activeSelf) {
                
                inventory.SetActive (false);
                this.isGamePaused = false;
                this.clearInventory();
                this.state = "None";

                Time.timeScale = 1;
            }
        }



         if (Input.GetButtonDown ("Cancel") && this.state != "Talking" && this.state != "InInventory"){
            
            if (!mainMenu.activeSelf) {
                
                mainMenu.SetActive (true);
                this.isGamePaused = true;
                this.state = "InMenu";

                Time.timeScale = 0;
                
            }
            else if (mainMenu.activeSelf) {
                
                mainMenu.SetActive (false);
                this.isGamePaused = false;
                this.state = "None";

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

                posX = (panelWidth / 2) - (panelWidth / 8);
                posY = (panelHeight / 2) - (panelHeight / 8);
            }
            else if (rowsCounter == 0) {
                posX = inventoryItems[i - 1].transform.position.x + itemSize;
                posY = inventoryItems [i - 1].transform.position.y;
            }
            else {
                posX = inventoryItems[i - (int) squareRoot].transform.position.x;
                posY = inventoryItems[i - (int) squareRoot].transform.position.y + itemSize;
            }
            


            string path = "Inventory/Empty";

            if ( i < inventoryScript.items.Count) {
                if (inventoryScript.items[i].Contains ("(Clone)"))
                    path = "Inventory/" + inventoryScript.items[i].Substring(0, inventoryScript.items[i].IndexOf ("(Clone)"));
                else
                 path = "Inventory/" + inventoryScript.items[i];
            }
            
            inventoryItems.Add ( GameObject.Instantiate (Resources.Load <GameObject> (path), inventory.transform) );
            inventoryItems[i].name = inventoryItems [i].name + " " + i;

            inventoryScript.items [i] = inventoryItems[i].name;
            
            if (inventoryScript.items [i].Contains ("Empty"))
                inventoryScript.items [i] = "Empty";

            inventoryItems[i].transform.position = new Vector3 (posX, posY, 0);
            
            
            inventoryItems[i].GetComponent<RectTransform> ().sizeDelta = new Vector2 (itemSize, itemSize);


            columnsCounter++;
            if (columnsCounter > squareRoot) {
                columnsCounter = 1;
                rowsCounter++;
            }
        }
    }


    private void clearInventory () {

        foreach (GameObject item in inventoryItems) {

            Destroy (item.gameObject);
        }

        inventoryItems.Clear();

    }


    public void OnLevelChanged (Scene scene, LoadSceneMode mode) {

        this.transform.position = player.transform.position;

        shadeObjects ();

        StartCoroutine (changeLastLevelnName(scene));

    }

    private IEnumerator changeLastLevelnName (Scene scene) {

        yield return new WaitForSeconds (0.1f);

        this.lastLevel = scene.name;
    }


    private IEnumerator dayNightCycle () {

        yield return new WaitForSeconds ((0.5f*60)/24);

        this.timeOfDay += 1;

        if (this.timeOfDay >= 24)
            this.timeOfDay = 0;

        shadeObjects ();

        this.StartCoroutine (dayNightCycle());

    }

    private void shadeObjects (){

        if (SceneManager.GetActiveScene ().name == "China") {
           
            GameObject globalLight = GameObject.Find ("GlobalLight");

            if (this.timeOfDay >= 0 && this.timeOfDay <= 5)
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (50, 50 ,0 ));
            else if (this.timeOfDay >= 6 && this.timeOfDay <= 8)
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (50, 50 - this.timeOfDay * 2.5f ,0 ));
            else if (this.timeOfDay >= 9 && this.timeOfDay <= 17)
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (50, 0,0 ));
            else if (this.timeOfDay >= 18 && this.timeOfDay <= 23)
                globalLight.transform.rotation = Quaternion.Euler (new Vector3 (50, this.timeOfDay * 1.5f,0 ));


            

               
        }
    }





}
