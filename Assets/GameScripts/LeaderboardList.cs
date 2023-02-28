using System.Collections.Generic;
using UnityEngine;

using System;
using System.Linq;
using TMPro;
using System.Security.Cryptography;
using System.Text;
using Mirror;
using Unity.VisualScripting;
using Google.Apis.Http;

public class LeaderboardList : NetworkBehaviour
{
    //  [SyncVar]
    public List<Score> scores = new List<Score>();
    
    public Transform contentObjectleaderboard;
    public GameObject leaderObject;
    List<GameObject> leaderboardObjeclist = new List<GameObject>();
   

    public GameObject createAccountObject;
    public GameObject ethersendobject;
   
    public GameObject captchaObject;
    // Start is called before the first frame update
    void Start()
    {
        var y = Camera.main.gameObject.GetComponent<loadLeaderboardList>().loaderlist;
        contentObjectleaderboard = y[0].transform;
       createAccountObject = y[1];
        ethersendobject = y[2];
        captchaObject = y[3];
        texttosubmit = y[4].GetComponent<TMP_InputField>();
        ogrankobject = y[5];
        copyText = y[6].GetComponent<TMP_Text>();

        
        if(PlayerPrefs.GetInt("accountCreated") != 1)
        {
            createAccountObject.SetActive(true);
            captchaObject.SetActive(true);
        }
        else
        {
            captchaObject.SetActive(false);
        }
       
            
        
        
      
       
     
       
        
        //call rpc if is host client that sends all the info needed to the client
    }
    /*public void callanRPC(Score score){
        Rpclololololo(score);
    }
    [ClientRpc]
    void Rpclololololo(Score score){
        AddScore(score);
    }*/
    
    public IEnumerable<Score> GetHighScores(){
     
    
        

        
       return scores.OrderByDescending(x => x.score);
       
    }
    // Update is called once per frame
    float timerkik;
   
    
  
    void Update()
    
    {


      
        timerkik += Time.deltaTime;
       
        if(timerkik > 10){


            if (PlayerPrefs.GetInt("Compete") != 1)
            {
                CmdCalltheListreloadleader(NetworkClient.connection.connectionId);

                timerkik = 0;
            }
           
            
        }
       
    }

    

    [Command (requiresAuthority = false)]
    public void CmdCalltheListreloadleader(int conn)
    {
      /*  if (calledAlready != true)
        {
            StartCoroutine(CheckForDuplicates());
                calledAlready = true;
        }*/
       

        List<string> username = new List<string>();
      
        List<float> score = new List<float>();
       
        username = scores.Select(x => x.name).Take(100).ToList();
       
        score = scores.Select(x => x.score).Take(100).ToList();
        
      

        TargetRpcListLeaderboardReload(NetworkServer.connections[conn], username, score);
    }
    [TargetRpc]
    void TargetRpcListLeaderboardReload(NetworkConnection conn, List<string> username, List<float> score)
    {
        Debug.Log("receivedListleader");
        //if(!NetworkClient.isHostClient)
        List<Score> scores = new List<Score>();
        scores = username.Select((n, i) => new Score(n, score[i], "", 0 )).ToList();
      
            foreach (var item in leaderboardObjeclist)
            {
                Destroy(item);
                
            }
            leaderboardObjeclist.Clear();
        foreach (var item in scores)
        {
            var x = Instantiate(leaderObject, contentObjectleaderboard);
           
            x.transform.GetChild(1).GetComponent<TMP_Text>().text = $"Name: {item.name}        Score: {item.score}";
            leaderboardObjeclist.Add(x);
        }
               
                    

             

            
           
        
        
           
            
        
        
    }
   
    public void AddScore(Score score){
        
       
            if (!scores.Exists(x => x.name == score.name))
            {
                score.pass = Hashing.ToSHA256(score.pass);
                score.score = 0;
                scores.Add(score);
                GetHighScores();

            Debug.Log(scores.Count);

        }
       
       
    }
    public void changetheEthercost(decimal whatever)
    {
       
        ethersendobject.transform.GetChild(4).gameObject.GetComponent<EtherTransferCoroutinesUnityWebRequest>().Amount = whatever;
        
    }
    public void sendtheEther()
    {
        ethersendobject.transform.GetChild(4).gameObject.GetComponent<EtherTransferCoroutinesUnityWebRequest>().TransferRequest();
    }
    public void theendreceiver(string playerreceiving)
    {
        ethersendobject.transform.GetChild(4).gameObject.GetComponent<EtherTransferCoroutinesUnityWebRequest>().endPayer = playerreceiving;
    }
   
    List<ViewsandIndexa> sentImagesandViews = new List<ViewsandIndexa>();
    List<CaptchaCtor> bytes = new List<CaptchaCtor>();
  

   

    [Command(requiresAuthority = false)]
    public void CmdnewAcccount(string username, string pass, string playerAnswer, int clientID)
    {
        Debug.Log("lol");
        
        
            
       
            // Perform action
          
                
                if (!scores.Exists(x => x.name == username))
                {

                    if (username.Length < 20 && username.Length > 0)
                    {
                        sentImagesandViews = gameObject.GetComponent<CaptchaScript>().sentImagesandViews;
                        bytes = gameObject.GetComponent<CaptchaScript>().bytes;


                        playerAnswer = playerAnswer.ToLower();

                        var h = sentImagesandViews.Find(x => x.connectionID == clientID);

                        if (playerAnswer == bytes[h.indexinlist].playerAnswer.ToLower())
                        {
                            Debug.Log("passedCreateAccountCaptcha");
                            AddScore(new Score(username, 0, pass, 0));
                            sentImagesandViews.Remove(h);






                        }
                        else
                        {
                            Targetdidnotpasscaptcha(NetworkServer.connections[clientID]);
                        }

                    }
                    else
                    {
                        TargetToolarget(NetworkServer.connections[clientID]);
                    }



                }
                else
                {
                    TargetgiveError(NetworkServer.connections[clientID]);
                }
            
          
        
       
        //add captcha here too


    }

    [TargetRpc]
    void TargetgiveError(NetworkConnection conn)
    {
        var bruh = gameObject.GetComponent<HandleTheDropdown>();
        var trans = bruh.cotnetErrors;
        var objecttospawn = bruh.errorObject;
        var x = Instantiate(objecttospawn, trans);
        x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Username already exists.";
        Destroy(x, 3f);
    }
    [TargetRpc]
   public void Targetdidnotpasscaptcha(NetworkConnection conn)
    {
        var bruh = gameObject.GetComponent<HandleTheDropdown>();
        var trans = bruh.cotnetErrors;
        var objecttospawn = bruh.errorObject;
        var x = Instantiate(objecttospawn, trans);
        x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Captcha failed.";
        Destroy(x, 3f);
    }
    [TargetRpc]
    public void TargetToolarget(NetworkConnection conn)
    {
        var bruh = gameObject.GetComponent<HandleTheDropdown>();
        var trans = bruh.cotnetErrors;
        var objecttospawn = bruh.errorObject;
        var x = Instantiate(objecttospawn, trans);
        x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Invalid input.";
        Destroy(x, 3f);
    }
    [TargetRpc]
    public void TargetNeedsAnAccount(NetworkConnection conn)
    {
        var bruh = gameObject.GetComponent<HandleTheDropdown>();
        var trans = bruh.cotnetErrors;
        var objecttospawn = bruh.errorObject;
        var x = Instantiate(objecttospawn, trans);
        x.transform.GetChild(2).gameObject.GetComponent<TMP_Text>().text = "Need an account.";
        Destroy(x, 3f);
    }
    [Command (requiresAuthority = false)]
    void CmdReferralProgram(int connID, string Coded, string username)
    {
        if(codes.Exists(x => x.code == Coded))
        {
            if (codes.Find( x => x.code == Coded).IPAddress != NetworkServer.connections[connID].address)
            {
                scores.Find(x => x.name == username).ogStatus++; //maybe all give it to person who generates code
                scores.Find(x => x.name == codes.Find(x => x.code == Coded).username).ogStatus++;
                codes.Remove(codes.Find(x => x.code == Coded));
            }
        }
    }
    public TMP_InputField texttosubmit;
    public void submitCode()
    {
        CmdReferralProgram(NetworkClient.connection.connectionId, texttosubmit.text, PlayerPrefs.GetString("username"));
    }
    public GameObject ogrankobject;
    public void setOGRAnkObjecttrue()
    {
        if(ogrankobject.activeInHierarchy == false)
        {
            ogrankobject.SetActive(true);
        }
        else
        {
            ogrankobject.SetActive(false);
        }
    }
   
    [TargetRpc]
    void TargetgiveCode(NetworkConnection conn, string text)
    {
        copyText.text = text;
    }
    [Command(requiresAuthority = false)]
    void CmdGenerateRefCode(int connID, string username, string password)
    {
       if(connID != 0)
        {
            if (scores.Exists(x => x.name == username))
            {

                if (scores.Find(x => x.name == username).pass == Hashing.ToSHA256(password))
                {

                    if (!codes.Exists(x => x.IPAddress == NetworkServer.connections[connID].address))
                    {

                        var random = new System.Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        var randomString = new string(Enumerable.Repeat(chars, 20)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        codes.Add(new Code(randomString, NetworkServer.connections[connID].address, username));
                        TargetgiveCode(NetworkServer.connections[connID], randomString);
                    }
                    else
                    {
                        var random = new System.Random();
                        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                        var randomString = new string(Enumerable.Repeat(chars, 20)
                          .Select(s => s[random.Next(s.Length)]).ToArray());
                        codes.Find(x => x.IPAddress == NetworkServer.connections[connID].address).code = randomString;
                        TargetgiveCode(NetworkServer.connections[connID], randomString);
                    }
                }

            }
            else
            {
                TargetNeedsAnAccount(NetworkServer.connections[connID]);
            }
        }
           
        
       


    }
    public void generateCodebutton()
    {
        CmdGenerateRefCode(NetworkClient.connection.connectionId, PlayerPrefs.GetString("username"), PlayerPrefs.GetString("password"));
    }
    public TMP_Text copyText;
    public void copyFromClipboard()
    {
        TextEditor textEditor = new TextEditor();
        textEditor.text = copyText.text;
        textEditor.SelectAll();
        textEditor.Copy();
    }
    List<Code> codes = new List<Code>();

}
public class Code
{
    public string code;
    public string IPAddress;
    public string username;
    public Code(string code, string iPAddress, string username)
    {
        this.code = code;
        IPAddress = iPAddress;
        this.username = username;
    }
}
[Serializable]
public class Score{
    public string name;
    public float score;

    public string pass;
    public float ogStatus;
    public Score (string name, float score, string pass, float ogStatus)
    {
        this.name = name;
        this.score = score;
        this.pass = pass;
        this.ogStatus = ogStatus;
    }
}
[Serializable]
public class ScoreData{
    public List<Score> scores;
    public ScoreData(){
        scores = new List<Score>();
    }
}

public static class Hashing{

     public static string ToSHA256(string s)
    {
        var sb = new StringBuilder();
        using var sha256 = SHA256.Create();
        byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(s));

        for (int i = 0; i < bytes.Length; i++)
        {
            sb.Append(bytes[i].ToString("x2"));
        }
        return sb.ToString();
    }   
}