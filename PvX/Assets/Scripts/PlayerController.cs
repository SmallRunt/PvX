using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float moveSpeed = 5f;
    public Animator anim;
    public Rigidbody2D rb;
    public Transform shootPos;
    public GameObject bulletPrefab;
    public int KeyFrags;
    public float bulletSpeed;
    public int Health;
    Vector2 movement;
  
    // Update is called once per frame
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        anim.SetFloat("Horizontal", movement.x);
        anim.SetFloat("Vertical", movement.y);
        anim.SetFloat("Speed", movement.sqrMagnitude);

        if(Input.GetKeyDown(KeyCode.LeftControl))
        {
            Shoot();
        }

        if (Health <= 0)
        {
            Destroy(gameObject);
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = Instantiate(bulletPrefab,shootPos.position,shootPos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootPos.up * bulletSpeed, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("P2Projectile"))
        {
            Health -= 1;
        }

        if (collision.gameObject.CompareTag("Key"))
        {
            KeyFrags += 1;
        }
    }
}
