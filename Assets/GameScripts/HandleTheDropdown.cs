using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


using OPS.AntiCheat.Field;
using Mirror;


using Nethereum.Web3;
using Newtonsoft.Json;
using Nethereum.Hex.HexTypes;
using UnityEngine.SceneManagement;
using UnityEngine.Video;
using System.Globalization;
using Unity.VisualScripting;

using UnityEngine.UIElements;
using System.Runtime.Serialization;
using Nethereum.ABI.EIP712;
using System.Threading.Tasks;
using System.Numerics;
using Nethereum.Util;
using Nethereum.Model;
using Nethereum.Web3.Accounts;
using System.Security.Cryptography.X509Certificates;
using Org.BouncyCastle.Asn1.X9;
using Nethereum.RPC.Eth.DTOs;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.Hex.HexConvertors.Extensions;
using static UnityEngine.ParticleSystem;
using System.CodeDom;

[Serializable]
public class HandleTheDropdown : NetworkBehaviour//ipunobservable onphotonserializeview is not in this monobehavior
{


    List<GameObject> shopListObjects = new List<GameObject>();
    public List<UploadsInShop> shopList = new List<UploadsInShop>();

    int thechosen = 0;
    byte[] serializeData;
    void Start() {

            
        var y = Camera.main.gameObject.GetComponent<loadHandleTheDropdown>().loaderlist;
        contentObjectOfShopList = y[0].transform;
        contentObject3 = y[1];
        contentObject2 = y[2].transform;
        entertitleofsale = y[3].GetComponent<TMP_InputField>();
        enterCost = y[4].GetComponent<TMP_InputField>();
        enterPublicKey = y[5].GetComponent<TMP_InputField>();
        enterprivateKey = y[6].GetComponent<TMP_InputField>();
        enterCaptcha = y[7].GetComponent<TMP_InputField>();
        captchaObject = y[8];
        uploadObject = y[9];
        cotnetErrors = y[10].transform;
        confirmbuy = y[11];
        spawnPlace = y[12].transform;
        Cmdgeneratekey(NetworkClient.connection.connectionId);


       


    }



  

    void Cmdgeneratekey(int clientID)
    {
        if (NetworkServer.connections.ContainsKey(clientID))
        {
            if(!playerbuyTimers.ContainsKey(clientID))
            {
                if (forgottenweights.Exists(x => x.IPAddress == NetworkServer.connections[clientID].address))
                {
                    var y =forgottenweights.FindAll(x => x.IPAddress == NetworkServer.connections[clientID].address);
                    for (int i = 0; i < y.Count; i++)
                    {
                        TargetactuallyGiveWeights(NetworkServer.connections[clientID], y[i].weightsToGive);
                    }
                }
                playerbuyTimers[clientID] = 0; //will it stop
                if (clientID == 0)
                {
                    secretCode = Hashing.ToSHA256(UnityEngine.Random.Range(0, 10000000000000).ToString());
                    TargetGiveBackKey(NetworkServer.connections[0], secretCode);
                }

            }
            
        }

    }
    string secretCode;
    [TargetRpc]
    void TargetGiveBackKey(NetworkConnection conn, string secret)
    {
        secretCode = secret;
    }
    public Transform contentObjectOfShopList;
    float timer;
    public GameObject shopItemobject;
    public GameObject walker;

      int[] layersf = new int[] { 9, 50, 20, 50, 5 };
     void Update()
    {
       
        transform.eulerAngles = new UnityEngine.Vector3(0, 0, 0);
        timer += Time.deltaTime;
        if (timer > 10)
        {
            if(PlayerPrefs.GetInt("Compete") != 1)
            {
                Cmdreloadshoplistmain(NetworkClient.connection.connectionId);
                timer = 0;
            }

           
            
        }



    }
    private void hi(int conn)
    {
        float lastCommandTime = playerCommandTimers[conn];
        if ((float)NetworkTime.time - lastCommandTime >= cooldownTime)
        {
            // Perform action
            if (conn != 0)
            {
                playerCommandTimers[conn] = (float)NetworkTime.time;
            }
        }
        else
        {
            NetworkServer.connections[conn].Disconnect();
        }
    }
    [Command (requiresAuthority = false)]
    void Cmdreloadshoplistmain(int conn)
    {
        
            List<string> uploadnames = new List<string>();
            List<string> publicKeys = new List<string>();
            List<string> creators = new List<string>();
            List<decimal> costs = new List<decimal>();
            List<float> speeds = new List<float>();
            shopList.Sort((a, b) => b.speed.CompareTo(a.speed));
            uploadnames = shopList.Select(x => x.thenameofupload).Take(100).ToList();
            publicKeys = shopList.Select(x => x.publicKey).Take(100).ToList();
            creators = shopList.Select(x => x.theCreator).Take(100).ToList();
            costs = shopList.Select(x => x.cost).Take(100).ToList();
            speeds = shopList.Select(x => x.speed).Take(100).ToList();
        List<int> mostBoughts = new List<int>();
        mostBoughts = shopList.Select(x => x.mostBought).Take(100).ToList();

        TargetRpcreloadShopListObjects(NetworkServer.connections[conn], uploadnames, publicKeys, creators, costs, speeds, mostBoughts);
        
       
       
    }
   
   



      [Command(requiresAuthority = false)]
    public void CmdSearch(int conn, string search)
    {
        
        
            // Perform action
           
            List<UploadsInShop> result = shopList.FindAll(x => x.thenameofupload.Contains(search));
            shopList.Sort((a, b) => b.mostBought.CompareTo(a.mostBought));
            List<string> uploadnames = new List<string>();
            List<string> publicKeys = new List<string>();
            List<string> creators = new List<string>();
            List<decimal> costs = new List<decimal>();
            List<float> speeds = new List<float>();
            uploadnames = result.Select(x => x.thenameofupload).Take(100).ToList();
            publicKeys = result.Select(x => x.publicKey).Take(100).ToList();
            creators = result.Select(x => x.theCreator).Take(100).ToList();
            costs = result.Select(x => x.cost).Take(100).ToList();
            speeds = result.Select(x => x.speed).Take(100).ToList();
            List<int> mostBoughts = new List<int>();
            mostBoughts = shopList.Select(x => x.mostBought).Take(100).ToList();
            Targetgivethesearchresult(NetworkServer.connections[conn], uploadnames, publicKeys, creators, costs, speeds, mostBoughts);
       
       
    }
    public GameObject contentObject3;
    List<GameObject> shopListObjects1 = new List<GameObject>();
    List<GameObject> shopListObjects2 = new List<GameObject>();
    List<GameObject> shopListObjects3 = new List<GameObject>();
    double[] fakemutvars;
    [TargetRpc]
    void Targetgivethesearchresult(NetworkConnection conn,   List<string> uploadnames, List<string> publicKeys, List<string> creators, List<decimal> costs, List<float> speeds, List<int> mostBoughts)
    {

        var combinedList = uploadnames.Select((n, i) => new UploadsInShop(n, fakeWeights, costs[i], creators[i], publicKeys[i], speeds[i], mostBoughts[i], fakemutvars)).ToList();
        foreach (var item in shopListObjects1)
        {
            Destroy(item);
            
        }
        shopListObjects1.Clear();
        //thetitle, thecreator, and the cost, public key is hidden in script cuz that is eyesore
        foreach (var item in combinedList)
        {
            var x = Instantiate(shopItemobject, contentObject3.transform);
            var y = x.GetComponent<ShopScript>();
            y.named = item.thenameofupload;
            y.nametext.text = "Name: " + item.thenameofupload;
            y.publicKey = item.publicKey;
            y.cost_text.text = "Cost: " + item.cost.ToString();
           y.theCost = item.cost;
            y.theCreator = item.theCreator;
            y.thisObjectsCreator = gameObject;
            y.speedText.text = "Speed:" + item.speed.ToString();
            y.downloadText.text = "Downloads: " + item.mostBought.ToString();
            shopListObjects1.Add(x);
        }
    }

    public Transform contentObject2;

      [Command(requiresAuthority = false)]
    public void CmdGetTheMostpopular(int conn)
    {
        
            List<string> uploadnames = new List<string>();
            List<string> publicKeys = new List<string>();
            List<string> creators = new List<string>();
            List<decimal> costs = new List<decimal>();
            List<float> speeds = new List<float>();

            shopList.Sort((a, b) => b.mostBought.CompareTo(a.mostBought));
            uploadnames = shopList.Select(x => x.thenameofupload).Take(100).ToList();
            publicKeys = shopList.Select(x => x.publicKey).Take(100).ToList();
            creators = shopList.Select(x => x.theCreator).Take(100).ToList();
            costs = shopList.Select(x => x.cost).Take(100).ToList();
            speeds = shopList.Select(x => x.speed).Take(100).ToList();
            List<int> mostBoughts = new List<int>();
            mostBoughts = shopList.Select(x => x.mostBought).Take(100).ToList();
            TargetGetTheMostPopulars(NetworkServer.connections[conn], uploadnames, publicKeys, creators, costs, speeds, mostBoughts);
       
      
    }
    //gotta add this later and actually use this and make it more efficient so rpcreloadlistobjects can be refreshed by clients so we do not hog their memory
    [TargetRpc]
    void TargetGetTheMostPopulars(NetworkConnection conn,  List<string> uploadnames, List<string> publicKeys, List<string> creators, List<decimal> costs, List<float> speeds, List<int> mostBoghts)
    {
        var combinedList = uploadnames.Select((n, i) => new UploadsInShop(n, fakeWeights, costs[i], creators[i], publicKeys[i], speeds[i], mostBoghts[i], fakemutvars)).ToList();
        foreach (var item in shopListObjects2)
        {
            Destroy(item);
           
        }
        shopListObjects2.Clear();
        //thetitle, thecreator, and the cost, public key is hidden in script cuz that is eyesore
        foreach (var item in combinedList)
        {
            var x = Instantiate(shopItemobject, contentObject2.transform);
            var y = x.GetComponent<ShopScript>();
            y.named = item.thenameofupload;
            y.nametext.text = "Name: " + item.thenameofupload;
            y.publicKey = item.publicKey;
            y.cost_text.text = "Cost: " + item.cost.ToString();
            y.theCost = item.cost;
            y.theCreator = item.theCreator;
            y.thisObjectsCreator = gameObject;
            y.speedText.text = "Speed:" + item.speed.ToString();
            y.downloadText.text = "Downloads: " + item.mostBought.ToString();
            shopListObjects2.Add(x);
        }
    }

  
  


    public byte[] ConvertDataToBytes(double[][][] data)
        {
        // Convert the data to bytes
        // Example:
        BinaryFormatter bf = new BinaryFormatter();
            using (var stream = new MemoryStream())
            {
                bf.Serialize(stream, data);
                serializeData = stream.ToArray();
            }
        return serializeData;
        }

        public double[][][] ConvertBytesToData(byte[] bytes)
        {
            MemoryStream streamd = new MemoryStream(bytes);

            // Create a BinaryFormatter
            BinaryFormatter formatter = new BinaryFormatter();

            // Deserialize the object from the MemoryStream
            return (double[][][])formatter.Deserialize(streamd);
    }

      
    
     

        // custom message to send the encrypted data


        double[][][] fakeWeights;
    
    [TargetRpc]
    void TargetRpcreloadShopListObjects(NetworkConnection conn, List<string> uploadnames, List<string> publicKeys, List<string> creators, List<decimal> costs, List<float> speeds, List<int> mostboughts)
    {
        
        Debug.Log("wasgivenlist");

        var combinedList = uploadnames.Select((n, i) => new UploadsInShop(n, fakeWeights, costs[i], creators[i], publicKeys[i], speeds[i], mostboughts[i], fakemutvars)).ToList();
        foreach (var item in shopListObjects3)
        {
            
           Destroy(item);
            
        }
        shopListObjects3.Clear();
        //TEST AND ADD TO THE BLOCK CHECKER FOR IT TO BE RIGHT URL
        //thetitle, thecreator, and the cost, public key is hidden in script cuz that is eyesore
        foreach (var item in combinedList)
        {
          
            var x = Instantiate(shopItemobject, contentObjectOfShopList);
            var y = x.GetComponent<ShopScript>();
           y.named = item.thenameofupload;
            y.nametext.text = "Name: " + item.thenameofupload;
            y.publicKey = item.publicKey;
            y.cost_text.text = "Cost: " + item.cost.ToString();
            y.theCost = item.cost;
            y.theCreator = item.theCreator;
            y.thisObjectsCreator = gameObject;
            y.speedText.text = "Speed:" + item.speed.ToString();
            y.downloadText.text = "Downloads: " + item.mostBought.ToString();
            shopListObjects3.Add(x);
        }
      


    }
    public TMP_InputField entertitleofsale;
    public TMP_InputField enterCost;
    public TMP_InputField enterPublicKey;
    public TMP_InputField enterprivateKey;
  

    public TMP_InputField enterCaptcha;
    public void HandleInputData(int val){
        thechosen = val;
    }
    public GameObject captchaObject;
    public GameObject uploadObject;
    public void UploadToShop(){


        //CHECK TO MAKE SURE THE PARSING DOES NOT GIVE ERROR 
        Debug.Log("uploadbuttonclicked");


        StartCoroutine(dothejob());
          
        
    }
   

    IEnumerator dothejob()
    {
        decimal.TryParse(enterCost.text, out decimal result);
        decimal thecostvalue = result;
        double[][][] weightsUpload;
        double[] mutUpload;
        BinaryFormatter bf = new BinaryFormatter();
        //change everything to numbers later on 
        using (FileStream fs = new FileStream(Application.dataPath + $"/dat/WeightSave{thechosen}.bin", FileMode.Open))
            weightsUpload = (double[][][])bf.Deserialize(fs);
        BinaryFormatter bf2 = new BinaryFormatter();
        using (FileStream fs2 = new FileStream(Application.dataPath + $"/dat/MutVars{thechosen}.bin", FileMode.Open))
            mutUpload = (double[])bf2.Deserialize(fs2);

        var y = ConvertDataToBytes(weightsUpload);
        var l = ConvertDouble1ToBytes(mutUpload);

        yield return new WaitForSeconds(0.2f);



        //var weightsUploadtrue = ConvertDataToBytes(weightsUpload);
        //gotta encrypt
        Cmdaddinshop(entertitleofsale.text, /*weightsUploadtrue,*/ y, thecostvalue, PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"), enterPublicKey.text, enterCaptcha.text, NetworkClient.connection.connectionId, l);
        captchaObject.SetActive(false);
        uploadObject.SetActive(false);
    }

    public byte[] ConvertDouble1ToBytes(double[] data)
    {
        // Convert the data to bytes
        // Example:
        BinaryFormatter bf = new BinaryFormatter();
        using (var stream = new MemoryStream())
        {
            bf.Serialize(stream, data);
            serializeData = stream.ToArray();
        }
        return serializeData;
    }

    public double[] ConvertBytesTodouble1(byte[] bytes)
    {
        MemoryStream streamd = new MemoryStream(bytes);

        // Create a BinaryFormatter
        BinaryFormatter formatter = new BinaryFormatter();

        // Deserialize the object from the MemoryStream
        return (double[])formatter.Deserialize(streamd);
    }


    List<CaptchaCtor> bytes = new List<CaptchaCtor>();
    List<ViewsandIndexa> sentImagesandViews = new List<ViewsandIndexa>();
    public override void OnStopClient()
    {
        SceneManager.LoadScene(0);
    }
    [Command(requiresAuthority = false)]
    void Cmdaddinshop(string thenameofadd, byte[] vectorArray, decimal cost, string theCreator, string thePass, string thePublickey, string playerAnswer, int clientID, byte[] mutvars)
    {
       
            // byte[] decryptedData = encryption.DecryptData(theactualuploadd);
            // Convert the bytes to data
            // float[][][] theactualupload = ConvertBytesToData(decryptedData);
            // Handle the data
            Debug.Log("receivedAddRequest");

            Debug.Log(gameObject.name);
            if (thenameofadd.Length > 0 && thenameofadd.Length < 20 && cost <= 100m && !shopList.Exists(x => x.thenameofupload == thenameofadd))
            {
                sentImagesandViews = gameObject.GetComponent<CaptchaScript>().sentImagesandViews;
                bytes = gameObject.GetComponent<CaptchaScript>().bytes;


                playerAnswer = playerAnswer.ToLower();

                var h = sentImagesandViews.Find(x => x.connectionID == clientID);

                if (playerAnswer == bytes[h.indexinlist].playerAnswer.ToLower())
                {

                    sentImagesandViews.Remove(h);
                    Debug.Log("passedCaptcha");
                    StartCoroutine(checkAnadd(thenameofadd, vectorArray, cost, theCreator, thePass, thePublickey, clientID, mutvars));


                }
                else
                {
                gameObject.GetComponent<LeaderboardList>().Targetdidnotpasscaptcha(NetworkServer.connections[clientID]);
                }

            }
            else
            {
                gameObject.GetComponent<LeaderboardList>().TargetToolarget(NetworkServer.connections[clientID]);
            }
        
       
      







    }
   IEnumerator checkAnadd(string thenameofadd, byte[] hid, decimal cost, string theCreator, string thePass, string thePublickey, int connID, byte[] mutvars)
    {
        var xd = ConvertBytesTodouble1(mutvars);
        var floatArray = ConvertBytesToData(hid);
        yield return new WaitForSeconds(0.02f);
        bool wehaveahit = true;
        var bruh = gameObject.GetComponent<LeaderboardList>().scores;
        /*CHANGE  THIS TO FALSE AND MAKE SURE ALL THE LOOPS ARE CALLED BEFORE THE WEHAVEAHIT IS TRUE*/
        foreach (var item in bruh)
        {
            if (item.pass == Hashing.ToSHA256(thePass) && item.name == theCreator)
            {
                wehaveahit = true;
            }
        }
        yield return new WaitForSeconds(bruh.Count * 0.02f);

        var k = shopList.Count(x => x.theCreator == theCreator);

        if (wehaveahit)
        {
            if (k <= 3)
            {
               
                  
                    UploadsInShop upload = new UploadsInShop(thenameofadd, floatArray, cost, theCreator, thePublickey, 0, 0, xd);
                    shopList.Add(upload);
                  
                    Targetrunthesimulation(NetworkServer.connections[0], hid, shopList.Count - 1, mutvars);
                   
                    Debug.Log(shopList.Count);
                

            }

        }
        else
        {
            gameObject.GetComponent<LeaderboardList>().TargetNeedsAnAccount(NetworkServer.connections[connID]);
        }

    }
   
    [TargetRpc]
    void Targetrunthesimulation(NetworkConnection conn, byte[] by, int index, byte[] mutvars)
    {
       
        StartCoroutine(walkingTest(index, by, mutvars));
    }
    public Transform spawnPlace;
    IEnumerator walkingTest( int index, byte[] d, byte[] mutvars)
    {
        var yd = ConvertBytesToData(d);
        var ld = ConvertBytesTodouble1(mutvars);
        NeuralNetwork net = new NeuralNetwork(layersf, yd);

        //change everything to numbers later on 
       
        net.weights = yd;
        net.mutatableVariables = ld;
        yield return new WaitForSeconds(0.1f);
        NetEntity y  = Instantiate(walker, spawnPlace ).GetComponent<NetEntity>();
        y.Init(net, 1, layersf[0], 10000, 1 );

        
        
        while(iteratord(y.gameObject) != false)
        {
            yield return new WaitForSeconds(0.05f);
        }




        /*  if(k < 0)
          {
              k *= -1f;
          }*/
        float hello = y.transform.GetChild(0).position.x;
        hello += 10f;
        float speed = hello / 2.1f;
        Debug.Log(speed);
        Destroy(y.gameObject);
        CmdsetSpeedInShopList(index, Mathf.RoundToInt(speed * 10f) / 10f, secretCode);


        

    }
    
    
    bool iteratord(GameObject lookat)
    {
        int amnt = 1;
        
            amnt -= lookat.GetComponent<NetEntity>().Elapse() ? 0 : 1;
     
        return amnt != 0;
    }
    [Command(requiresAuthority = false)]
    void CmdsetSpeedInShopList(int index, float speed, string secretCoded)
    {
        if(secretCode == secretCoded)
        {
            shopList[index].speed = speed;
        }
        
    }
    double[][][] shouldbesentweights;
      [Command(requiresAuthority = false)]
    void CmdExecuteBuyWeights(string theSeller, decimal theCost,  int connectionId,  string thename, string publicKeyTheir) 
    {


        //also gotta change all other playerfiles saving, training, with dropdown for three
        float lastCommandTime = playerbuyTimers[connectionId];
        if ((float)NetworkTime.time - lastCommandTime >= buyCooldown)
        {
            // Perform action
          //  if (connectionId != 0) I DISABLED THE PROTECTION CUZ THAT IS STUPID;
            {
              //  playerbuyTimers[connectionId] = (float)NetworkTime.time;
            }

            var result = shopList.FirstOrDefault(c => c.theCreator == theSeller && c.cost == theCost && c.thenameofupload == thename);
            if (result != null)
            {
                shouldbesentweights = result.theweightupload;
                
                
                checkformoney(connectionId, theCost, result.publicKey, shopList.IndexOf(result), publicKeyTheir);
            }
        }
        else
        {
            NetworkServer.connections[connectionId].Disconnect();
        }




        
                
                
                




            
     }

    public async void GetAverageBlockTimeForMonth()
    {
        Web3 _web3 = new Web3("https://mainnet.infura.io/v3/6282b01b48594c4f90c08acb9c92e169");
        // Get the latest block number
        var latestBlockNumber = await _web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();

        // Calculate the start block number (30 days or 1 month ago)
        var startBlockNumber = Math.Max(0, Convert.ToInt64(latestBlockNumber) - 17280); // 30 days * 24 hours * 60 minutes * 20 seconds = 17280 blocks

        // Get the start block and end block
        var startBlock = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(new HexBigInteger(startBlockNumber)));
        var endBlock = await _web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(new BlockParameter(new HexBigInteger(latestBlockNumber)));

        // Calculate the time difference between the start and end blocks
        var timeDiff = TimeSpan.FromSeconds(Convert.ToDouble(endBlock.Timestamp.Value) - Convert.ToDouble(startBlock.Timestamp.Value));

        // Calculate the number of blocks mined during the time period
        var blocksMined = latestBlockNumber.ToLong() - startBlockNumber;

        // Calculate the average block time (add 1 second buffer)
        var avgBlockTimeInSeconds = (timeDiff.TotalSeconds / blocksMined) + 1;
        ethertime =  (float)avgBlockTimeInSeconds;

       

    }
    float ethertime;
    IEnumerator checkformoney(int connectionID, decimal Amount, string endPayer, int index, string publicKey)
    {





       GetAverageBlockTimeForMonth();
        yield return new WaitForSeconds(1f);
        
        yield return new WaitForSeconds(ethertime);

        CheckTransaction checkit = new CheckTransaction();
        checkit.yourAddress = "0x58e8B9018E763B5C8146FE37e90D09EffaB975b3";
        checkit.publicAddress1 = publicKey;
        checkit.publicAddress2 = endPayer;
        checkit.amount = Amount;
        checkit.CheckLatestBlock();
      
        yield return new WaitForSeconds(302);
        if(checkit.wentOk == true)
        {
            gameObject.GetComponent<CompetitionScriptRedo>().amountToGive += Decimal.Multiply(Amount, 0.1m);
           var x = ConvertDataToBytes(shouldbesentweights);
            try
            {
                TargetactuallyGiveWeights(NetworkServer.connections[connectionID], x);
            }
            catch
            {
                forgottenweights.Add(new ForgottenWeights(NetworkServer.connections[connectionID].address, x));
            }
            shopList[index].mostBought += 1;
            checkit.wentOk = false;

           
         
        }
         
    }
    List<ForgottenWeights> forgottenweights = new List<ForgottenWeights>(); 
    [TargetRpc]
   void TargetactuallyGiveWeights(NetworkConnection conn,  byte[] x)
    {
       // byte[] decryptedData = encryption.DecryptData(x);
        // Convert the bytes to data
        double[][][] boughtWeights = ConvertBytesToData(x);
        // Handle the data
       
     
        

        StartCoroutine(check(boughtWeights, 0, null, 0, null, null));
           

        
       
    }
    public float cooldownTime = 0.1f; // Time in seconds between commands

    private Dictionary<int, float> playerCommandTimers = new Dictionary<int, float>();

    public float buyCooldown = 0.5f;
    private Dictionary<int, float> playerbuyTimers = new Dictionary<int, float>();
    IEnumerator check(double[][][] boughtWeights, int action, string seller, decimal theCost,  ProtectedString name, string publicAddress)
    {
        BinaryFormatter bf = new BinaryFormatter();

        for (int i = 0; i < 3; i++)
        {
            bool actuallySomethinghere = false;
            double[][][] checkWeights;
            using (FileStream fs = new FileStream(Application.dataPath + $"/PlayerDataddFile{i.ToString()}.json", FileMode.Open))
                checkWeights = (double[][][])bf.Deserialize(fs);

            for (int l = 0; l < checkWeights.Length; l++)
            {
                for (int k = 0; k < checkWeights[l].Length; k++)
                {
                    for (int j = 0; j < checkWeights[l][k].Length; j++)
                    {
                        if (checkWeights[l][k][j] != 0)
                        {
                            actuallySomethinghere = true;

                        }
                    }
                }

            }
          
            if (action == 0)
            {
                if (actuallySomethinghere != true) //make sure is called after for loop if not, then use corountine, same for serialization
                {
                    //to check if this is happenning, make sure it is not saving it in the first playerdatafile

                    using (FileStream fs = new FileStream(Application.dataPath + $"/PlayerDataddFile{i.ToString()}.json", FileMode.Create))
                        bf.Serialize(fs, boughtWeights);
                    break;

                }
            }
            else if (action == 1)
            {
                if(actuallySomethinghere == true)
                {
                    var x = Instantiate(errorObject, cotnetErrors);
                    x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "You used up all your saves.";
                    Destroy(x, 3f);
                    Debug.Log("used emall");
                }
                else
                {
                    CmdExecuteBuyWeights(seller, theCost, NetworkClient.connection.connectionId, name, publicAddress);
                    yield return new WaitForSeconds(0.5f);
                    //processingTransactionloading.gameObject.SetActive(false);
                }

            }
            
        }
        yield return null;
    }
    public Transform cotnetErrors;
    public GameObject errorObject;
    double[][][] randomweightss;

  
        public void buyTheWeights(string seller, decimal theCost, ProtectedString publicKey, ProtectedString name)
        {
        var account = new Nethereum.Web3.Accounts.Account(enterprivateKey.text);
            var publicAddress = account.Address;
       // processingTransactionloading.gameObject.SetActive(true);
       // processingTransactionloading.Play();
        StartCoroutine(check(randomweightss, 1, seller, theCost,  name, publicAddress));

             //gottamaketransactionandcheckit
        }
    public GameObject confirmbuy;
    string readypublickey;
    decimal readycost;
    string thereadycreatorl;
        string namedready;
    public void givetheparameters(string publicKey, decimal theCost, string theCreator, string named)
    {
        readypublickey = publicKey;
        readycost = theCost;
        thereadycreatorl = theCreator;
        namedready = named;
        confirmbuy.SetActive(true);
        StartCoroutine(mustbeclickedwithin3secs());
    }
    IEnumerator mustbeclickedwithin3secs()
    {
        yield return new WaitForSeconds(3f);
        confirmbuy.SetActive(false);
    }
    public void callthebuying()
    {
        therealbuy(readypublickey, readycost, thereadycreatorl, namedready);
        confirmbuy.SetActive(false);
    }
    public void therealbuy(string selleraddress, decimal theAmount, string theCreator, string named)
    {
       var y = GetComponent<LeaderboardList>().ethersendobject.transform.GetChild(4).gameObject.GetComponent<EtherTransferCoroutinesUnityWebRequest>();
        y.PrivateKey = enterprivateKey.text;

    
        y.AddressTo = selleraddress;

        y.Amount = System.Decimal.Multiply(theAmount, 0.7m);

        y.TransferRequest();

        y.AddressTo = "0x58e8B9018E763B5C8146FE37e90D09EffaB975b3";

        y.Amount = System.Decimal.Multiply(theAmount, 0.3m);
        y.TransferRequest();
       buyTheWeights(theCreator, theAmount, selleraddress, named);

    }
    


}
public class UploadsInShop{
    public string thenameofupload;
    public double[][][] theweightupload;
    public double[] mutvars;
    public decimal cost;
    public string theCreator;
    public string publicKey;
    public float speed;
    public int mostBought;
    public UploadsInShop(string thenameofupload, double[][][] theweightupload, decimal cost, string theCreator, string publicKey, float speed, int mostBought, double[] mutvars)
    {
        this.thenameofupload = thenameofupload;
        this.theweightupload = theweightupload;
        this.cost = cost;
        this.theCreator = theCreator;
        this.publicKey = publicKey;
        this.speed = speed;
        this.mostBought = mostBought;
        this.mutvars = mutvars;
    }
}


public class CheckTransaction
{
    public string publicAddress1 = "0x..."; // The first public key you want to check is publicAddress1
    public string publicAddress2 = "0x..."; // The second public key you want to check is publicAddress2
    public decimal amount = 0.1m; // The amount you want to check for
    public string yourAddress = "";
    public bool wentOk = false;


    public async void CheckLatestBlock()
    {
        // Connect to the Ethereum network
      

        // Get the latest block number

        // Get the block at the latest block number

        var x = await checkerer();
        if (x)
        {
            wentOk = true;
        }
        else
        {
            wentOk = false;
        }
    }
    public async Task<bool> checkerer()
    {
        Web3 web3 = new Web3("https://mainnet.infura.io/v3/6282b01b48594c4f90c08acb9c92e169");
        var latestBlockNumber = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();
        var block = await web3.Eth.Blocks.GetBlockWithTransactionsByNumber.SendRequestAsync(latestBlockNumber);
        await Task.Delay(TimeSpan.FromMinutes(5));
        var latestBlockNumber2 = await web3.Eth.Blocks.GetBlockNumber.SendRequestAsync();


        if((Convert.ToInt64(latestBlockNumber2) - Convert.ToInt64(latestBlockNumber)) > 12)
        {
            var filteredTransactions = block.Transactions.Where(t => t.From == publicAddress1 && t.Value.Value >= Web3.Convert.ToWei(amount)
            && block.Transactions.Any(u => u.From == publicAddress1 && u.To == yourAddress && u.Value.Value >= Web3.Convert.ToWei(Decimal.Multiply(0.3m, amount)))
            && block.Transactions.Any(u => u.From == publicAddress1 && u.To == publicAddress2 && u.Value.Value >= Web3.Convert.ToWei(Decimal.Multiply(amount, 0.7m)))).ToList();
            return filteredTransactions.Count > 0;
        }
        else
        {
            return false;
        }
        // Filter transactions to check if there's a transaction from publicAddress1 to both publicAddress2 and yourAddress with the desired amount
        
    }
}
public class ForgottenWeights
{
    public string IPAddress;
    public byte[] weightsToGive;
    public ForgottenWeights(string IPAddress, byte[] weightsToGive)
    {
        this.IPAddress = IPAddress;
        this.weightsToGive = weightsToGive;
    }
}
//
