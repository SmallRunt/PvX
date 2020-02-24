using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class Health : MonoBehaviourPun
{
    public Image fillImage;
    public float health = 1;

    public Rigidbody2D rb;
    public SpriteRenderer sr;
    public BoxCollider2D BoxCollider;
    public GameObject playerCanvas;
    public PlayerController playerScript;

    public void CheckHealth()
    {
        if(photonView.IsMine && health <= 0)
        {
            GameManager.instance.enableRespawn();
            playerScript.canMove = true;
            this.GetComponent<PhotonView>().RPC("death", RpcTarget.AllBuffered);
        }
    }

    [PunRPC]
    public void death()
    {
        rb.gravityScale = 0;
        playerScript.KeyFrags = 0;
        BoxCollider.enabled = false;
        sr.enabled = false;
        playerCanvas.SetActive(false);
    }
    [PunRPC]
    public void Revive()
    {
        fillImage.fillAmount = 1;
        health = 1;
        rb.gravityScale = 0;
        BoxCollider.enabled = true;
        sr.enabled = true;
        playerCanvas.SetActive(true);
    }

    [PunRPC]
    public void HealthUpdate(float damage)
    {
        fillImage.fillAmount -= damage;
        health = fillImage.fillAmount;
        CheckHealth();
    }

    public void enableInput()
    {
        playerScript.canMove = false;
    }
}
