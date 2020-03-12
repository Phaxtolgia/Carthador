using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCharacter : MonoBehaviour
{

    private Animator anim;
    private Vector3 scale;

    private float h;
    private float v;
    private float previousH;
    private float previousV;

    private GameObject arrow;


    // Start is called before the first frame update
    void Start()
    {
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



            if (Input.GetButtonDown("Fire1"))
            {

                anim.SetBool("Attack", true);
                if (v > 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Back") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Back"))
                {
                    anim.Play("Attack_Back");

                    arrow = GameObject.Instantiate(Resources.Load<GameObject>("Sprites/Arrow"), this.transform.position + (this.transform.up * 0.1f), Quaternion.identity);
                }
                else if (v < 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Front") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Front"))
                {
                    anim.Play("Attack_Front");

                    arrow = GameObject.Instantiate(Resources.Load<GameObject>("Sprites/Arrow"), this.transform.position -(this.transform.up * 0.1f), Quaternion.Euler (0, 0 , 180));
                }
                else if (h > 0 || anim.GetCurrentAnimatorStateInfo(0).IsName("Main1_Idle_Side") || anim.GetCurrentAnimatorStateInfo(0).IsName("Attack_Side"))
                {
                    anim.Play("Attack_Side");
                    arrow = GameObject.Instantiate(Resources.Load<GameObject>("Sprites/Arrow"), this.transform.position + (new Vector3 (this.scale.x,0,0) * 0.1f), Quaternion.Euler(0, 0, 90)); ;
                }

            }

        }
        

        if (anim.GetBool ("Attack") && (anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1))
        {
                
            anim.SetBool("Attack", false);
        }



        this.transform.Translate(new Vector3(h, v, 0) * Time.deltaTime);
    }


    private void FlipX ()
    {
        scale = this.transform.localScale;
        scale.x *= -1;
        this.transform.localScale = scale;


    }
}
