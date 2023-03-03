using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using JetBrains.Annotations;
using Nethereum.Web3.Accounts;

public class CompetitionScriptRedo : NetworkBehaviour
{

    public GameObject walker;
    
    List<RegisteredWeights> RegisteredWeightslist = new List<RegisteredWeights>();
   
    int chosen;
    string secretCode;
    public TMP_Text playerCount;
    double[][][] weightgive;
    public GameObject shopCanvas;
    public GameObject gameCanvas;
    public TMP_Text timetillnextroundtext;
    // Start is called before the first frame update
    void Start()
    {
        var y = Camera.main.gameObject.GetComponent<loadCompetition>().loaderlist;
        playerCount = y[0].GetComponent<TMP_Text>();
        shopCanvas = y[1];
        gameCanvas = y[2];
        timetillnextroundtext = y[3].GetComponent<TMP_Text>();
        submitWeightbutton = y[4];
        publicKeyInput = y[5].GetComponent<TMP_InputField>();
        LastWinnerText = y[6].GetComponent<TMP_Text>();
        winnerObject = y[7];
        convertpublictoprivate = y[8].GetComponent<TMP_InputField>();
        prizeMoney_text = y[9].GetComponent<TMP_Text>();
        spawnpoint = y[10].transform;

        if (PlayerPrefs.GetInt("Compete") == 1)
        {
            shopCanvas.SetActive(false);
            gameCanvas.SetActive(true);
        }
        if(NetworkClient.connection.connectionId == 0)
        {
            CmdGetSecretCode();
        }
        CmdaskForList(NetworkClient.connection.connectionId);
    }
    public GameObject submitWeightbutton;
    public TMP_InputField publicKeyInput;
    public void registerToWeightsButton()
    {
        double[] giveMuts;
        if(PlayerPrefs.GetString("username") != "")
        {
            BinaryFormatter binaryFormatter = new BinaryFormatter();
            using (FileStream fs = new FileStream(Application.persistentDataPath + $"/dat/WeightSave{chosen}.bin", FileMode.Open))
                weightgive = (double[][][])binaryFormatter.Deserialize(fs);
            BinaryFormatter bf2 = new BinaryFormatter();
            using (FileStream fs2 = new FileStream(Application.persistentDataPath + $"/dat/MutVars{chosen}.bin", FileMode.Open))
                giveMuts = (double[])bf2.Deserialize(fs2);
            var d = gameObject.GetComponent<HandleTheDropdown>().ConvertDouble1ToBytes(giveMuts);
            var x = gameObject.GetComponent<HandleTheDropdown>().ConvertDataToBytes(weightgive);
            CmdregisterWeights(NetworkClient.connection.connectionId, x, d, PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), publicKeyInput.text);
            submitWeightbutton.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("Compete", 2);
            gameCanvas.SetActive(false);
                shopCanvas.SetActive(true);
            gameObject.GetComponent<ButtonScript1>().resetCreateaccountObject();

               
        }
       
    }
    public void resubmitweightsbutton()
    {
        submitWeightbutton.SetActive(true);
    }
    //DO SOMETHING TO TEXT ON WALKER AND ADD SIGN IN
    public void chososeWights(int weight)
    {
        chosen = weight;
    }
    bool getsecretCode = true;
    void CmdGetSecretCode()
    {
        if(getsecretCode == true)
        {
            secretCode = Hashing.ToSHA256(Random.Range(0, 100000000000000000).ToString());
            TargetHostto(NetworkServer.connections[0], secretCode);
            getsecretCode = false;
        }
      
        
    }
    [TargetRpc]
    void TargetHostto(NetworkConnection conn, string code)
    {
        secretCode = code;
    }
    // Update is called once per frame
    float timerkik;
    float hostimer;

    void Update()
    {
        timetillnextroundtext.text = "Time till next round: " + (50 - Mathf.RoundToInt(hostimer)).ToString();
        timerkik += Time.deltaTime;
        if(timerkik > 10)
        {
            if(PlayerPrefs.GetInt("Compete") == 1)
            {
                CmdaskForList(NetworkClient.connection.connectionId);
            }
           
        }
        hostimer += Time.deltaTime;

        if (hostimer > 40)
        {
            if (NetworkClient.connection.connectionId == 0)
            {
                CmdonWin(secretCode);
                hostimer = 0;
            }

        }
        else if (hostimer > 10 && hostimer < 10.1f)
        {
           
              
                if (NetworkClient.connection.connectionId == 0)
                {
                   
                        restart();
                hostimer += 0.1f;
                    

                }
            
            
            
        }
    }
    void restart()
    {
        
        Cmdstopwinnerpicture(secretCode);
        CmdStartGame(secretCode);
       
    }
    [Command (requiresAuthority =false)]
    void Cmdstopwinnerpicture(string code)
    {
        if(code == secretCode)
        {
            RpcStopWinnerPicture();
        }
       
    }
    [Command(requiresAuthority = false)]
    void CmdaskForList(int connid)
    {
        TargetreceiveRpcList(NetworkServer.connections[connid], NetworkServer.connections.Count, winnerUsername, amountToGive);  
    }
    public TMP_Text LastWinnerText;
    [TargetRpc]
    void TargetreceiveRpcList(NetworkConnection conn, int playerCount, string user, decimal prizeMoney)
    { 
        this.playerCount.text = "Players: " + playerCount.ToString();
        LastWinnerText.text = "Last Winner Was: " + user;
        prizeMoney_text.text = "Prize Money: " + prizeMoney;
    }

    int connIDWinner = 0;
    [Command(requiresAuthority = false)]
    void CmdonWin(string code)
    {
        if(secretCode == code)
        {
            
          if(runners.Count > 0)
            {
                float bestTransformposition = 0;
             
                var objects = GameObject.FindGameObjectsWithTag("WalkerObject");
                var winners = runners.Select(runner => (
                    runner: runner,
                    obj: objects.FirstOrDefault(obj => obj.name == runner.racer.name)
                )).Where(pair => pair.obj != null && pair.obj.transform.position.x > bestTransformposition);

                if (winners.Any())
                {
                    var winnerPair = winners.First();
                    connIDWinner = winnerPair.runner.connID;

                   
                    bestTransformposition = winnerPair.obj.transform.position.x;






                    Debug.Log("gothere");
                    var y = gameObject.GetComponent<LeaderboardList>().ethersendobject.transform.GetChild(4).gameObject.GetComponent<EtherTransferCoroutinesUnityWebRequest>();


                    y.AddressTo = RegisteredWeightslist.Find(x => x.connID == connIDWinner).publicKey;
                    y.Amount = amountToGive;
                    y.TransferRequest();
                    amountToGive = 0;
                }
                winnerUsername = RegisteredWeightslist.Find(x => x.connID == connIDWinner).usernamed;

                for (int i = 0; i < runners.Count; i++)
                {
                    NetworkServer.Destroy(runners[i].racer);
                }
                runners.Clear();

                gameObject.GetComponent<LeaderboardList>().scores.Find(x => x.name == winnerUsername).score += 1f;

                RpcWinnerPicture(winnerUsername);
            }
           

            
        }
       
    }public decimal amountToGive;
   
    public GameObject winnerObject;
    [ClientRpc]
    void RpcWinnerPicture(string whoWon)
    {
        winnerObject.SetActive(true);
        winnerObject.GetComponent<TMP_Text>().text = whoWon +" Won This Round!";

    }
    [ClientRpc]
    void RpcStopWinnerPicture()
    {
        winnerObject.SetActive(false);
       
    }
    

    [Command(requiresAuthority = false)]
    void CmdregisterWeights(int connId, byte[] weightsd, byte[] mutvarsd, string username, string pass, string publicKey)
    {
        var d = gameObject.GetComponent<HandleTheDropdown>();
        var weights = d.ConvertBytesToData(weightsd);
        var mutvars = d.ConvertBytesTodouble1(mutvarsd);
        var bruh = gameObject.GetComponent<LeaderboardList>();
       if (NetworkServer.connections.ContainsKey(connId))
        {
            
            if (bruh.scores.Exists(x => x.name == username))
            {
                var y = bruh.scores.Find(x => x.name == username);
                if (y.pass == Hashing.ToSHA256(pass))
                {
                    if (!RegisteredWeightslist.Exists(x => x.connID == connId))
                    {
                        RegisteredWeightslist.Add(new RegisteredWeights(weights, mutvars, connId, username, y.ogStatus, publicKey));
                    }
                    else
                    {
                        RegisteredWeightslist.Find(x => x.connID == connId).weights = weights;
                    }
                }
                
            }
            else
            {
                TargetNAmenotexist(NetworkServer.connections[connId]);
            }
          
        }


    }
    [TargetRpc]
    void TargetNAmenotexist(NetworkConnection conn)
    {
       
            var bruh = gameObject.GetComponent<HandleTheDropdown>();
            var trans = bruh.cotnetErrors;
            var objecttospawn = bruh.errorObject;
            var x = Instantiate(objecttospawn, trans);
        shopCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        gameObject.GetComponent<LeaderboardList>().captchaObject.SetActive(true);
        gameObject.GetComponent<LeaderboardList>().createAccountObject.SetActive(true);
        gameObject.GetComponent<CaptchaScript>().CmdgetTheCaptcha(NetworkClient.connection.connectionId);
        PlayerPrefs.SetInt("Compete", 2);
        x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Invalid input. Create an account please.";
        
    }
    int[] layers = new int[] { 9, 50, 20, 50, 5 };
    public Transform spawnpoint;
    [Command(requiresAuthority = false)]
    void CmdStartGame(string secretCoded)
    {

        if(secretCode == secretCoded)
        {
            Debug.Log("close");
            for (int i = 0; i < RegisteredWeightslist.Count; i++)
            {
                Debug.Log("near here" + RegisteredWeightslist.Count);
                if (NetworkServer.connections.ContainsKey(RegisteredWeightslist[i].connID))
                {
                    Debug.Log("here");
                    GameObject awker = Instantiate(walker, spawnpoint );
                    var y = awker.GetComponent<NetEntity1>();
                    NeuralNetwork net = new NeuralNetwork(layers, RegisteredWeightslist[i].weights);
                    net.weights = RegisteredWeightslist[i].weights;
                    net.mutatableVariables = RegisteredWeightslist[i].mutvars;
                    y.Init(net, 0, layers[0], 20000, 0);
                   
                   
                    awker.name = RegisteredWeightslist[i].usernamed;
                    y.ogrank.text = "OG Rank: " + RegisteredWeightslist[i].ogStatus.ToString();
                    y.nameofus.text = "Username: " + RegisteredWeightslist[i].usernamed;
                  //  y.usernameText.text = RegisteredWeightslist[i].usernamed;
                   // y.ogStatus.text = "OG Rank: " + RegisteredWeightslist[i].ogStatus.ToString();
                    //y.Init(net);
                    runners.Add(new SpawndRacerss(awker, RegisteredWeightslist[i].connID));


                    NetworkServer.Spawn(awker);
                    StartCoroutine(iteratingtheobjecT(awker));
                    //make it run on same server to create leaderboard
                }
               
                
            }
        }

       
       

    }
    IEnumerator iteratingtheobjecT(GameObject gameobjectoiterate)
    {
        while (iteratord(gameobjectoiterate) != false)
        {
            yield return new WaitForSeconds(0.05f);
        }
    }
    bool iteratord(GameObject lookat)
    {
        int amnt = 1;

        amnt -= lookat.GetComponent<NetEntity>().Elapse() ? 0 : 1;

        return amnt != 0;
    }
    string winnerUsername;
    
    
    List<SpawndRacerss> runners = new List<SpawndRacerss>();

    public TMP_InputField convertpublictoprivate;
    public void ConvertPrivatetoPublic()
    {
        var account = new Nethereum.Web3.Accounts.Account(convertpublictoprivate.text);
        var publicAddress = account.Address;
        convertpublictoprivate.text = publicAddress;
    }
    public TMP_Text prizeMoney_text;
}
public class SpawndRacerss{
    public GameObject racer;
    public int connID;
    public SpawndRacerss(GameObject racer, int connID)
    {
        this.racer = racer;
        this.connID = connID;
    }
}
public class RegisteredWeights
{
    public double[][][] weights;
    public double[] mutvars;
    public int connID;
    public string usernamed;
    public float ogStatus;
    public string publicKey;
    public RegisteredWeights(double[][][] weights, double[] mutvars, int connID, string usernamed, float ogStatus, string publicKey)
    {
        this.weights = weights;
        this.connID = connID;
        this.usernamed = usernamed;
        this.ogStatus = ogStatus;
        this.publicKey = publicKey;
        this.mutvars = mutvars;
    }
}