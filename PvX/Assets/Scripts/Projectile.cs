using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rb;
    public float bulletSpeed;
    public float destroyTime = 1 ;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        destroyTime -= 5 * Time.deltaTime;
        if (destroyTime < 0)
        {
            Destroy(gameObject);
        }

        rb.velocity = transform.forward * bulletSpeed;
        
        
    }

}
