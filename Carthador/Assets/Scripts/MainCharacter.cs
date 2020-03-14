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
        currentHealth = maxHealth;
        currentAether = maxAether;

        game = Camera.main.GetComponent<Game>();
        anim = this.GetComponent<Animator>();
        anim.Play("Main1_Idle_Back");
    }

    // Update is called once per frame
    void Update()
    {

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


            // AETHER ATTACK //
            if (Input.GetButtonDown("Fire1") && (game.state == "Fighting" || game.state == "ReadyToFight") && this.currentAether >= airAttackCost)
            {

                this.currentAether -= airAttackCost;

                string path = "AetherBullet";

                anim.SetBool("Attack", true);
                if (v > 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Back") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Back"))
                {
                    anim.Play("Attack_Back");

                    arrow = GameObject.Instantiate(Resources.Load<GameObject>(path), this.transform.position + (this.transform.up * 0.3f), Quaternion.identity);
                }
                else if (v < 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Front") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front"))
                {
                    anim.Play("Attack_Front");

                    arrow = GameObject.Instantiate(Resources.Load<GameObject>(path), this.transform.position - (this.transform.up * 0.3f), Quaternion.Euler(0, 0, 180));
                }
                else if (h != 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Side") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Side"))
                {
                    anim.Play("Attack_Side");

                    if (scale.x >= 0)
                        arrow = GameObject.Instantiate(Resources.Load<GameObject>(path), this.transform.position + (this.transform.right * 0.3f), Quaternion.Euler(0, 0, -90));
                    else
                        arrow = GameObject.Instantiate(Resources.Load<GameObject>(path), this.transform.position + (-this.transform.right * 0.3f), Quaternion.Euler(0, 0, 90));
                }

            }

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
            GameObject.Instantiate(Resources.Load("AetherDefense"),this.transform);

        }









        if (!attacked)
            this.GetComponent<Rigidbody2D>().MovePosition(new Vector2(this.transform.position.x, this.transform.position.y) + new Vector2(h, v) * 7f * Time.deltaTime);
        else
            StartCoroutine(revertAttacked());
        
        if (this.currentHealth <= 0)
            SceneManager.LoadScene("China");

        if (this.currentAether < 0)
            this.currentAether = 0;

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
            if (this.currentAether + 10 >= this.maxAether)
                this.currentAether = this.maxAether;
            else
                this.currentAether += 10;
            Destroy(collision.collider.gameObject);

        }
    }
}
