using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Realtime;
using Photon.Pun;
using System;
using OPS.AntiCheat;
using OPS.AntiCheat.Field;
using OPS.AntiCheat.Prefs;

public class PlayerManagement : MonoBehaviourPunCallbacks
{
    public static PlayerManagement Instance { get; private set; }
    protected PhotonView photonView;
    private List<PlayerStats> playerStats = new List<PlayerStats>();
    private List<PlayerStatsd> playerStatsd = new List<PlayerStatsd>();
    bool caughtHacking;
    string hackername;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        photonView = GetComponent<PhotonView>();
    }
    List<float> list = new List<float>();
    // List<float> list2 = new List<float>();

    [PunRPC]
    void saytoEverybodyhackerishere(Player player)
    {
        caughtHacking = true;
        
        if(PhotonNetwork.NickName == hackername)
        {
            Debug.Log("hacker:" + hackername);
            Console.WriteLine("Fuck you hacker. Go to hell where you belong to be for a month (ban time). You know you are ruining people's fun, so you better shut the fuck up and never play again.");
            ProtectedPlayerPrefs.SetFloat("isHacker", 30);
            ProtectedPlayerPrefs.SetFloat("HackerStartingDate", DateTime.Now.DayOfYear + PlayerPrefs.GetFloat("isHacker"));
            //make sure if is greater than 365 it does not count cuz that can never be reached
            PhotonNetwork.LeaveRoom();
            Application.Quit();
          
        }
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.CloseConnection(player);
        }
      
     
    }
    // Update is called once per frame
    void Update()
    {
        
    
    }
   public void AddPlayerStats(Player player)
    {
        int index = playerStats.FindIndex(x => x.Player == player);
        if(index == -1)
        {
            playerStats.Add(new PlayerStats(player, GameObject.Find(player.NickName).transform.position));
            
        }
    }
    public void ModifyStats(Player player, Vector3 hi)
    {
        int index = playerStats.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            PlayerStats playerStatsd = playerStats[index];
            if(hi.x - playerStatsd.PositionOfPlayer.x > 4)
            {
                playerStatsd.PositionOfPlayer.x = hi.x;
                caughtHacking = true;
                hackername = playerStatsd.Player.NickName;
            }
            if (PhotonNetwork.IsMasterClient)
            {
                photonView.RPC("saytoEverybodyhackerishere", RpcTarget.All, player);

            }



        }
    }
    public void AddPlayerStatsd(Player player)
    {
        int index = playerStatsd.FindIndex(x => x.Player == player);
        if (index == -1)
        {
            playerStatsd.Add(new PlayerStatsd(player, false));

        }
    }
    public void ModifyStatsd(Player player, bool isAdmitted)
    {
        int index = playerStatsd.FindIndex(x => x.Player == player);
        if (index != -1)
        {
            PlayerStatsd playerStatsdd = playerStatsd[index];
            playerStatsdd.isAdmitted = isAdmitted;
            



        }
    }
} //https://youtu.be/17h3y1A_tAg?t=537 continue cuz need it
public class PlayerStats
{
    public PlayerStats(Player player, Vector3 positionOfPlayer)
    {
        Player = player;
        PositionOfPlayer = positionOfPlayer;
    }
    public readonly Player Player;
    public Vector3 PositionOfPlayer;
}
public class PlayerStatsd
{
    public PlayerStatsd(Player player, bool isAdmittedd)
    {
        Player = player;
        isAdmitted = isAdmittedd;
    }
    public readonly Player Player;
    public bool isAdmitted;
}
