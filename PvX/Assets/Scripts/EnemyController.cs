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
    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector2.Distance(transform.position, playerPos.position) > 1.2)
        {
            transform.position = Vector2.MoveTowards(transform.position, playerPos.position, chaseSpeed * Time.deltaTime);
        }

        if(Vector2.Distance(transform.position, playerPos.position) > 0.3)
        {
            if(shootCD <= 0)
            {
                Instantiate(projectilePrefab, transform.position, Quaternion.identity);
                shootCD = startCD;
            }
            else
            {
                shootCD -= Time.deltaTime;
            }


        }
    }

    void shoot()
    {
        GameObject eProjectile = Instantiate(projectilePrefab,transform.position, transform.rotation);
    }
}
