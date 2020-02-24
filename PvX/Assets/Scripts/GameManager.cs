using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    public GameObject playerPrefab;
    public GameObject canvas;
    public GameObject sceneCam;

    public Text spawnText;
    public GameObject respawnUI;

    private float TimeAmount = 5;
    private bool startRespawn;
    public static GameManager instance = null;
    [HideInInspector]
    public GameObject LocalPlayer;

    public Transform[] spawner;

    public GameObject enemyToSpawn;
    public PlayerController playerScript;
    //public Text pingrate;

    void Awake()
    {
        instance = this;
        canvas.SetActive(true);
    }

    private void Update()
    {
        if (startRespawn)
        {
            StartRespawn();
        }

        if (GameObject.FindGameObjectsWithTag("Player").Length > 1)
        {
            if (GameObject.FindGameObjectsWithTag("Enemy").Length > 1) return;
            Vector2 pos = new Vector2(Random.Range(-1.5f, 2), Random.Range(-1, 2));
            GameObject enemies = Instantiate(enemyToSpawn, pos, Quaternion.identity);
        }
    }

    public void StartRespawn()
    {
        TimeAmount -= Time.deltaTime;
        spawnText.text = "Resurrecting in: " + TimeAmount.ToString("F0");

        if(TimeAmount <= 0)
        {
            respawnUI.SetActive(false);
            startRespawn = false;
            relocationPlayer();
            LocalPlayer.GetComponent<Health>().enableInput();
            LocalPlayer.GetComponent<PhotonView>().RPC("Revive", RpcTarget.AllBuffered); ;
        }
    }
    
    public void relocationPlayer()
    {
        LocalPlayer.transform.localPosition = new Vector2(Random.Range(0, spawner.Length), Random.Range(0 , spawner.Length));
    }

    public void enableRespawn()
    {
        TimeAmount = 5;
        startRespawn = true;
        respawnUI.SetActive(true);
    }

    public void SpawnPlayer()
    {
        float randomValue = Random.Range(-0.5f, 0.5f);
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector2(playerPrefab.transform.position.x * randomValue, playerPrefab.transform.position.y), Quaternion.identity,0);
        canvas.SetActive(false);
        sceneCam.SetActive(false);
    }
}
