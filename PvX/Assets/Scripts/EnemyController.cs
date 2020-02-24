using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public float chaseSpeed;
    public GameObject projectilePrefab;
    private Transform playerPos;
    public float startCD;
    private float shootCD;
    
    public float eHealth;
    public GameObject keyFrag;
    public AudioClip explosionSFX;

    // Start is called before the first frame update
    void Start()
    {
        eHealth = 1f;
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > 0.1)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, chaseSpeed * Time.deltaTime);
        }

        if (eHealth <= 0)
        {
            AudioSource.PlayClipAtPoint(explosionSFX, transform.position, 1f);
            Destroy(this.gameObject);
        }
    }

    private void OnDestroy()
    {
        Instantiate(keyFrag, transform.position, transform.rotation);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Projectile") || collision.gameObject.CompareTag("Player"))
        {
            eHealth -= 0.5f;
        }   
        
        if(collision.gameObject.CompareTag("Player"))
        {
            collision.gameObject.GetComponent<Health>().HealthUpdate(0.5f);
        }
    }

    void shoot()
    {
        GameObject eProjectile = Instantiate(projectilePrefab,transform.position, transform.rotation);
    }
}
