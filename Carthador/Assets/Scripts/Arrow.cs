using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{

    private float speed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroySelf());
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.position += this.transform.up * speed * Time.deltaTime;

    }


    public IEnumerator destroySelf ()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);

    }
}
