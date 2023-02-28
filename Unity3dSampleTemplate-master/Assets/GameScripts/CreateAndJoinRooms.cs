using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;
//using UnityEngine.Rendering.PostProcessing;

public class CreateAndJoinRooms : MonoBehaviourPunCallbacks
{
    public TMP_InputField inputFoe;
    public RoomItem roomItemprefab;
    List<RoomItem> roomItemsList = new List<RoomItem>();
    public Transform contentObject;
    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.JoinLobby();
        PhotonNetwork.NickName = "Player" + Random.Range(0, 100000);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    public void CreateRoom()
    { 
        if(inputFoe.text.Length > 5)
        {
           
            var options = new RoomOptions() { IsVisible = true, IsOpen = true, MaxPlayers = 4 };
            PhotonNetwork.CreateRoom(inputFoe.text, roomOptions: options);
           
           // loadGameScene();
        }
        

    }
    void loadGameScene()
    {
       
        PhotonNetwork.LoadLevel("Game");
    }
    public override void OnCreatedRoom()
    {
        //PhotonNetwork.LoadLevel("Game");
        loadGameScene();
    }
   
    public void JoinRoom()
    {
        
        PhotonNetwork.JoinRoom(inputFoe.text);

        //loadGameScene();

    }
   
    public override void OnJoinedRoom()
    {
       
        loadGameScene();
        //PhotonNetwork.LoadLevel("Game");
    }
  
    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        UpdateRoomList(roomList);
    }
    void UpdateRoomList(List<RoomInfo> list) //check video to make sure done correctly
    {
        foreach(RoomItem item in roomItemsList)
        {
            Destroy(item.gameObject);
        }
        roomItemsList.Clear();
        foreach (RoomInfo room in list)
        {
            RoomItem x = Instantiate(roomItemprefab, contentObject);
            x.SetRoomName(room.Name);
            roomItemsList.Add(x);
        }
    }
}
