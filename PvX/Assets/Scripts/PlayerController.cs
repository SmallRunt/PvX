using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviourPun
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
    public PhotonView photonview;
    public SpriteRenderer sprite;
    public Text playerName;
    public GameObject playerCam;
    public bool canMove = false;
    bool canShoot;
    public float timer = 0.5f;

    public int bankedKeyFrags;
    int currentKeyFrags;

    public GameObject winnerText;
    public string myNickname;

    public Text gemsCollectedText;
    public Text gemsBankedText;
    public AudioClip shootingSFX;
    public AudioClip gemsCollectingSFX;
    public AudioClip gemsDepositSFX;

    void Awake()
    {
        if (photonView.IsMine)
        {
            GameManager.instance.LocalPlayer = this.gameObject;
            playerCam.SetActive(true);
            playerName.text = "You: " + PhotonNetwork.NickName;
            playerName.color = Color.green;
            myNickname = PhotonNetwork.NickName;
        }
        else
        {
            playerName.text = photonview.Owner.NickName;
            playerName.color = Color.red;
        }
    }

    // Update is called once per frame
    void Update()
    {
        gemsCollectedText.text = "Carrying:" + KeyFrags.ToString();
        gemsBankedText.text = "Banked:" + bankedKeyFrags.ToString();

        if (photonview.IsMine)
        {
            if (bankedKeyFrags > 9)
            {
                StartCoroutine(youWin());
            }
        }

        if (!canShoot)
        {
            timer -= 1f * Time.deltaTime;

            if(timer <= 0)
            {
                timer = 0.5f;
                canShoot = true;
            }
        }

        if (photonView.IsMine && !canMove)
        {
            //checkInput();

            movement.x = Input.GetAxisRaw("Horizontal");
            movement.y = Input.GetAxisRaw("Vertical");
            
            anim.SetFloat("Horizontal", movement.x);
            anim.SetFloat("Vertical", movement.y);
            anim.SetFloat("Speed", movement.sqrMagnitude);

            if (Input.GetButtonDown("Fire1") && timer >= 0.5f)
            {
                AudioSource.PlayClipAtPoint(shootingSFX, transform.position, 0.9f);
                Shoot();
            }
        }
    }

    private void FixedUpdate()
    {
        rb.transform.Translate(movement * moveSpeed * Time.fixedDeltaTime, Space.Self);
        //rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
    }

    void Shoot()
    {
        GameObject bullet = PhotonNetwork.Instantiate(bulletPrefab.name, new Vector2(shootPos.position.x, shootPos.position.y), Quaternion.identity, 0);
        //GameObject bullet = Instantiate(bulletPrefab, shootPos.position, shootPos.rotation);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        rb.AddForce(shootPos.up * bulletSpeed, ForceMode2D.Impulse);
        canShoot = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Key"))
        {
            AudioSource.PlayClipAtPoint(gemsCollectingSFX, transform.position, 0.8f);
            KeyFrags += 1;
        }

        if(collision.gameObject.CompareTag("Bank"))
        {
            AudioSource.PlayClipAtPoint(gemsDepositSFX, transform.position, 0.9f);
            bankedKeyFrags += KeyFrags;
            KeyFrags = 0;
        }
    }

    void checkInput()
    {
        var movement = new Vector3(Input.GetAxisRaw("Horizontal"), 0);
        transform.position += movement * moveSpeed * Time.deltaTime;

        if(Input.GetKeyDown(KeyCode.D))
        {
            sprite.flipX = false;
        }

        if(Input.GetKeyDown(KeyCode.A))
        {
            sprite.flipX = true;
        }
    }

    IEnumerator youWin()
    {
        winnerText.SetActive(true);
        yield return new WaitForSeconds(3f);
        SceneManager.LoadScene(0);
    }
}
