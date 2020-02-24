using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class ChatManager : MonoBehaviourPun, IPunObservable
{
    public PhotonView photonView;
    public GameObject BubbleSpeech;
    public Text ChatText;

    public PlayerController player;

    InputField ChatInput;
    private bool disableSend;

    private void Awake()
    {
        ChatInput = GameObject.Find("Chat Input Field").GetComponent<InputField>();
    }

    // Update is called once per frame
    void Update()
    {
        if(photonView.IsMine)
        {
            if(ChatInput.isFocused)
            {
                player.canMove = true;
            }
            else
            {
                player.canMove = false;
            }

            if(!disableSend && ChatInput.isFocused)
            {
                if(ChatInput.text != " " && ChatInput.text.Length > 1 && Input.GetButton("Submit"))
                {
                    photonView.RPC("SendMsg", RpcTarget.AllBuffered, ChatInput.text);
                    BubbleSpeech.SetActive(true);
                    ChatInput.text = "";
                    disableSend = true;
                }
            }
        }
    }

    [PunRPC]
    void SendMsg(string msg)
    {
        ChatText.text = msg;
        StartCoroutine(hideBubbleSpeech());
    }

    IEnumerator hideBubbleSpeech()
    {
        yield return new WaitForSeconds(3);
        BubbleSpeech.SetActive(false);
        disableSend = false;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if(stream.IsWriting)
        {
            stream.SendNext(BubbleSpeech.activeSelf);
        }
        else if(stream.IsReading)
        {
            BubbleSpeech.SetActive((bool)stream.ReceiveNext());
        }
    }
}
