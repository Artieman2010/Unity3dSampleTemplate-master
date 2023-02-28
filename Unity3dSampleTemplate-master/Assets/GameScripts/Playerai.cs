using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;
using Photon.Pun;
using OPS.AntiCheat.Field;
using Photon.Realtime;
using TMPro;
using OPS.AntiCheat.Prefs;
using System;

[System.Serializable]
public class Playerai : MonoBehaviourPunCallbacks
{

    public GameObject entityPrefabd;
    public GameObject targetd;
    public int[] layersd = new int[] { 35, 70, 70, 8 };
    ProtectedVector3 yd = new Vector3(0, 0, 0);
    protected ProtectedString theWinner;
    List<Player> players = new List<Player>();
    List<WalkerScript> walkerScripts = new List<WalkerScript>();
    PhotonView view;
    public bool gameStarted = false;
    public GameObject playerItem;
    public Transform contentPlayerlist;
    public GameObject theCamera;
    public GameObject startButton;

    List<GameObject> playerlister = new List<GameObject>();

    public GameObject etherSendtab;

    public List<Player> admittedPlayers = new List<Player> ();
    public void onEtherSend()
    {
        view.RPC("ethersendrpc", RpcTarget.MasterClient);
    }
    [PunRPC]
    void ethersendrpc()
    {
        //tell MASTERCLIENT THAT IT WAS SENT and uncomment the not spawning of master client
    }
    
    // Start is called before the first frame update
    void Start()
    {
        
        view = gameObject.GetComponent<PhotonView>();
        if (view.IsMine)
        {
           theCamera.SetActive(true);
        }

       

    }

    [PunRPC]
    void CoolAddPlayer(Player player)
    {
       // player = PhotonNetwork.LocalPlayer;
        PlayerManagement.Instance.AddPlayerStats(player);
    }
    [PunRPC]
    void CoolModifyPlayer(Player player)
    {
        PlayerManagement.Instance.ModifyStats(player, GameObject.Find(player.NickName).transform.position);
    }
    // Update is called once per frame
    protected ProtectedFloat timerplswork;
    bool gameAlready = false;

    void sayImmaCheater()
    {
     
        Console.WriteLine("Fuck you hacker. Go to hell where you belong to be for a month (ban time). You know you are ruining people's fun, so you better shut the fuck up and never play again.");
        ProtectedPlayerPrefs.SetFloat("isHacker", 30);
        ProtectedPlayerPrefs.SetFloat("HackerStartingDate", DateTime.Now.DayOfYear + PlayerPrefs.GetFloat("isHacker"));
        //make sure if is greater than 365 it does not count cuz that can never be reached
        PhotonNetwork.LeaveRoom();
        Application.Quit();
    }
    private void FixedUpdate()
    {
        foreach(WalkerScript walkerScript in walkerScripts)
        {
            if(walkerScript.failed == true)
            {
                PhotonNetwork.Destroy(walkerScript.gameObject);
            }
        }
    }
    
    void Update()
    {
        if (gameStarted == true)
        {
            view.RPC("CreatingtheBodies", RpcTarget.AllBuffered);
            contentPlayerlist.parent.parent.parent.gameObject.SetActive(false);
            gameStarted = false;
            gameAlready = true;

            view.RPC("CoolAddPlayer", RpcTarget.AllBuffered, PhotonNetwork.LocalPlayer);
        }
       
        timerplswork += Time.deltaTime; //gotta fix speed hax
        if (timerplswork > 0.1f)
        {
            if(gameAlready == true)
            {
                if (CheatEngineDetect.IsCheatEngineRunning() == true)
                {
                    sayImmaCheater();   
                }
                
            }
            if (view.IsMine)
            {
                if (gameAlready != true)
                {
                   
                    foreach (var item in PhotonNetwork.PlayerList)
                    {
                        foreach (var c in playerlister)
                        {
                            Destroy(c);
                            playerlister.Remove(c);
                        }

                        var x = Instantiate(playerItem, contentPlayerlist);
                        x.transform.GetChild(0).GetComponent<TMP_Text>().text = item.NickName;
                        playerlister.Add(x);
                    }

                    if (!PhotonNetwork.IsMasterClient)
                    {
                        startButton.SetActive(false);
                    }
                }

                
            }
           
           
            foreach(Player player in players)
            {
                if(GameObject.Find(player.NickName) != null)
                {
                    PlayerManagement.Instance.ModifyStats(player, GameObject.Find(player.NickName).transform.position);
                }
               
            }
            timerplswork = 0;
            /* foreach(WalkerScript walkerScript in walkerScripts)
             {
                 if (walkerScript.failed = true)
                 {
                     PhotonNetwork.Destroy(walkerScript.gameObject);
                     walkerScripts.Remove(walkerScript);
                 }
             }

         }*/
        }
        


    }
    [PunRPC]
    void CreatingtheBodies()
    {

       // if (!PhotonNetwork.IsMasterClient) //UNCOMMENT THIS LATER
        {
            Debug.Log("hi");
            NeuralNetwork netd = new NeuralNetwork(layersd);

            BinaryFormatter bf = new BinaryFormatter();
            using (FileStream fs = new FileStream(Application.dataPath + "/PlayerDataddFile.json", FileMode.Open))
                netd.weights = (float[][][])bf.Deserialize(fs); //the weights work fine 

            //netd.Mutate();
            WalkerScript walkerScript = ((GameObject)PhotonNetwork.Instantiate(entityPrefabd.name, new Vector3(0, 0.27f, 0), entityPrefabd.transform.rotation)).GetComponent<WalkerScript>();
            walkerScript.Init(netd, targetd.transform); //this is the source of problem i think
            theCamera.GetComponent<CameraFollow>().target = walkerScript.transform;

            walkerScript.gameObject.name = PhotonNetwork.NickName;
            walkerScripts.Add(walkerScript); //some random list that does not mean anything
        }
       




    }
     public override void OnPlayerEnteredRoom(Player player)
     {
        players.Add(player);

     }
     public override void OnPlayerLeftRoom(Player otherPlayer)
     {
         players.Remove(otherPlayer);
     }

    IEnumerator WhenItEnds()
    {

        foreach (var item in PhotonNetwork.PlayerList)
        {
            try
            {
                var xd = GameObject.Find(item.NickName);
                if (xd.gameObject.transform.position.x > yd.Value.x)
                {
                    yd = new Vector3(xd.transform.position.x, 0, 0);
                }
            }
            catch
            {

            }


        }
        yield return new WaitForSeconds(0.2f);
        foreach (var item in PhotonNetwork.PlayerList)
        {
            try
            {
                var xd = GameObject.Find(item.NickName);
                if (xd.gameObject.transform.position.x == yd.Value.x)
                {
                    theWinner = xd.gameObject.name;
                }

            }
            catch
            {

            }



        }

    }
}
