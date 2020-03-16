using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainCharacter : MonoBehaviour
{

    public int maxHealth = 100;
    [HideInInspector] public int currentHealth;

    public float maxAether = 100;
    [HideInInspector]public float currentAether;

    public int airAttackCost = 1;
    public int defenseCost = 5;


    private Inventory inventory;

    private Animator anim;
    private Vector3 scale;

    private float h;
    private float v;
    private float previousH;
    private float previousV;

    private GameObject arrow;

    private Game game;

    [HideInInspector] public bool attacked = false;
    [HideInInspector] public bool nearEnemy = false;

    [HideInInspector] public bool isDefending = false;

    // Start is called before the first frame update


    void Start()
    {

         DontDestroyOnLoad (this.gameObject);

         SceneManager.sceneLoaded += OnLevelChanged;
    }

    // Update is called once per frame
    void Update()
    {

        if (game == null) {
            
            currentHealth = maxHealth;
            currentAether = maxAether;

            game = Camera.main.GetComponent<Game>();
            inventory = game.GetComponent <Inventory> ();

            anim = this.GetComponent<Animator>();
            anim.Play("Main1_Idle_Back");


        }


        if (game.isGamePaused)
            return;

        previousH = h;
        previousV = v;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");


        if (Mathf.Sign(scale.x) != Mathf.Sign(h) && h != 0)
            FlipX();


        if (!anim.GetBool("Attack"))
        {

            if (previousH < 0 && (h == 0 && v == 0))
            {
                anim.Play("Main1_Idle_Side");

            }
            else if (previousH > 0 && (h == 0 && v == 0))
            {
                anim.Play("Main1_Idle_Side");
            }
            else if (previousV < 0 && (h == 0 && v == 0))
            {
                anim.Play("Main1_Idle_Front");
            }
            else if (previousV > 0 && (h == 0 && v == 0))
            {
                anim.Play("Main1_Idle_Back");
            }

            if (h != 0 || v != 0)
            {
                anim.Play("WalkBlendTree");
                anim.SetFloat("Horizontal", h);
                anim.SetFloat("Vertical", v);
            }
        }


        if (Input.GetButtonDown("Fire1") && (game.state == "Fighting" || game.state == "ReadyToFight") && this.currentAether >= airAttackCost)
        {
            this.currentAether -= airAttackCost;

            string path = "AetherBullet";

            Vector3 attackDir = Camera.main.ScreenToWorldPoint(Input.mousePosition) - this.transform.position;

            float aimH = Input.GetAxis("Horizontal2");
            float aimV = Input.GetAxis("Vertical2");

            if (aimH != 0 || aimV != 0)
                attackDir = new Vector3(aimH, aimV, 0);


            anim.SetBool("Attack", true);
            arrow = GameObject.Instantiate(Resources.Load<GameObject>(path), this.transform.position + attackDir.normalized, Quaternion.identity);

            if (v > 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Back") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Back"))
                anim.Play("Attack_Back");
            else if (v < 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Front") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front"))
                anim.Play("Attack_Front");
            else if (h != 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Side") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Side"))
                anim.Play("Attack_Side");
        }





            if (anim.GetBool ("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1))
        {
                
            anim.SetBool("Attack", false);
        }


        // AETHER DEFENSE //
        if (Input.GetButtonDown("Fire2") && this.currentAether >= defenseCost)
        {
            this.isDefending = true;
            this.currentAether -= defenseCost;
            GameObject.Instantiate(Resources.Load("AetherDefense"), this.transform);

        }
        
        if (this.currentHealth <= 0)
            SceneManager.LoadScene("China");

        if (this.currentAether < 0)
            this.currentAether = 0;

    }




    public void FixedUpdate()
    {

        if (game == null || game.isGamePaused)
            return;

        previousH = h;
        previousV = v;

        h = Input.GetAxisRaw("Horizontal");
        v = Input.GetAxisRaw("Vertical");


        if (!attacked)
            this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(h, v) * 3f  * Time.deltaTime);
        else
            StartCoroutine(revertAttacked());
    }







    private void FlipX ()
    {
        scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;


    }


    public IEnumerator revertAttacked()
    {
        yield return new WaitForSeconds(0.25f);


        this.GetComponent<Rigidbody2D>().velocity = Vector3.zero;



        attacked = false;
    }



    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Aether")
        {
            int i = inventory.items.IndexOf ("Empty");

            if ( i >= 0){ 
                inventory.items [i] = "Aether Potion";

                Destroy(collision.collider.gameObject);
            }

        }
        if (collision.collider.tag == "Health")
        {
            int i = inventory.items.IndexOf ("Empty");

            if ( i >= 0){ 
                inventory.items [i] = "Health Potion";

                Destroy(collision.collider.gameObject);
            }
            else
                print (inventory.items[3]);
        }
    }

    public void OnLevelChanged (Scene scene, LoadSceneMode mode) {


        Vector3 spawnPosition;
        if (game != null)
            spawnPosition = GameObject.Find ("Player Spawn " + game.lastLevel).transform.position;
        else
            spawnPosition = GameObject.Find ("Player Spawn").transform.position;
        
        this.transform.position = spawnPosition;

    }

}
