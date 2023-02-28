using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class ButtonPlayer : MonoBehaviourPunCallbacks
{
 
    PhotonView view;
    public string intendedrecipient; //decided by playerai
    // Start is called before the first frame update
    void Start()
    {
        view = GetComponent<PhotonView>();


        
    }
   
    
    // Update is called once per frame
    void Update()
    {
        
    }
 
    public void ButtonAdmit()
    {
        view.RPC("Admitting", RpcTarget.AllBuffered, intendedrecipient);
    }
    [PunRPC]
    public void Admitting(string loladmission)
    {
        foreach(var player in PhotonNetwork.PlayerList)
        {
            if (player.NickName == loladmission)
            {
                PlayerManagement.Instance.ModifyStatsd(player, true);
                gameObject.GetComponent<Playerai>().admittedPlayers.Add(player);
            }
        }
        
    }
}
