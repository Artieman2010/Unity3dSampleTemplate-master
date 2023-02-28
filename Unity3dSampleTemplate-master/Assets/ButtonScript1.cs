using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript1 : MonoBehaviourPunCallbacks
{
    PhotonView view;

    // Start is called before the first frame update
    void Start()
    {
        view = gameObject.GetComponent<PhotonView>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonStartGame()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            view.RPC("StartTheGame", RpcTarget.All);
        }

    }
    [PunRPC]
    public void StartTheGame()
    {
       gameObject.GetComponent<Playerai>().gameStarted = true;
    }
    public void ButtonLeaveGame()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.LoadLevel("Contest");
    }
}
