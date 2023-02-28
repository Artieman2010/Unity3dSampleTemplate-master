using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Photon.Pun;

public class RoomItem : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetRoomName(string _roomName)
    {
        gameObject.transform.GetChild(0).GetComponent<TMP_Text>().text = _roomName;
    }
    public void joinaroomlolclick()
    {
        PhotonNetwork.JoinRoom(transform.GetChild(0).GetComponent<TMP_Text>().text);
    }
}
