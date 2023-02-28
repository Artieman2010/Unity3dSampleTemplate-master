using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using OPS.AntiCheat.Field;
using System.Linq;
using System;
using OPS.AntiCheat.Prefs;

public class ConnectToServer : MonoBehaviourPunCallbacks
{
   
    // Start is called before the first frame update
    void Start()
    {
        //ProtectedPlayerPrefs.SetFloat("isHacker", 0);
        if (ProtectedPlayerPrefs.GetFloat("isHacker") > 0)
        {
            Console.WriteLine("Haha you thought coming back would work. Gotta wait till your punishment is over");
            if(ProtectedPlayerPrefs.GetFloat("isHacker") + ProtectedPlayerPrefs.GetFloat("HackingStartingDate") > 365)
            {
                ProtectedPlayerPrefs.SetFloat("HackingStartingDate", 0);
                ProtectedPlayerPrefs.SetFloat("isHacking", 0);
            }
            else
            {
                ProtectedPlayerPrefs.SetFloat("isHacker", DateTime.Now.DayOfYear - ProtectedPlayerPrefs.GetFloat("HackingStartingDate"));
                Application.Quit();
            }
            
        }
        else
        {
            ProtectedPlayerPrefs.SetFloat("HackingStartingDate", 0);
            ProtectedPlayerPrefs.SetFloat("isHacking", 0);
        }
        
        PhotonNetwork.ConnectUsingSettings();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    public override void OnConnectedToMaster()
    {
        PhotonNetwork.EnableCloseConnection = true;
        SceneManager.LoadScene("Contest");
    }
    
}
