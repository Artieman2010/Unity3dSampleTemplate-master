
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Unity.VisualScripting;

using System.Linq;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

public class ButtonScript1 : NetworkBehaviour
{
  
    public TMP_InputField enterusername;
    public TMP_InputField enterpass;
    public GameObject uploadObject;
    
    // Start is called before the first frame update
    void Start()
    {
        var y = Camera.main.gameObject.GetComponent<loadButtonScript1>().loaderlist;
        enterusername = y[0].GetComponent<TMP_InputField>();
        enterpass = y[1].GetComponent<TMP_InputField>();
        uploadObject = y[2];
        maintab = y[3];
        secondtab = y[4];
        searchTab = y[5];
        enterAnswer = y[6].GetComponent<TMP_InputField>();
        searchBar  = y[7].GetComponent<TMP_InputField>();
        shopCanvas = y[8];
        gameCanvas = y[9];
        submitweightsthing = y[10];
        captchaObject = y[11];
        createAccountObject = y[12];
        gameObject.GetComponent<CaptchaScript>().image = y[13].GetComponent<Image>();

        tabColors = Camera.main.gameObject.GetComponent<loadButtonScript1>().images;

        var x = Camera.main.gameObject.GetComponent<loadthebuttons>();
        var l = gameObject.GetComponent<CompetitionScriptRedo>();
        var d = gameObject.GetComponent<HandleTheDropdown>();
        var c = gameObject.GetComponent<LeaderboardList>();
        x.createButton.onClick.AddListener(createAccount);
        x.uploadButton.onClick.AddListener(d.UploadToShop);
        x.convertButton.onClick.AddListener(l.ConvertPrivatetoPublic);
        x.submitWeightsButton.onClick.AddListener(l.registerToWeightsButton);
        x.leaveButton.onClick.AddListener(LeaveButton);
        x.leaveButton2.onClick.AddListener(LeaveButton);
        x.resubmitWeights.onClick.AddListener(l.resubmitweightsbutton);
        x.createNewAccount.onClick.AddListener(resetCreateaccountObject);
        x.ogrankbutton.onClick.AddListener(c.setOGRAnkObjecttrue);
         x.setuploadbuttontrue.onClick.AddListener(UploadGameObjectTrue);
        x.submitCode.onClick.AddListener(c.submitCode);
        x.RegenerateCode.onClick.AddListener(c.generateCodebutton);
        x.copyCode.onClick.AddListener(c.copyFromClipboard);
        x.firstTab.onClick.AddListener(maintabshop);
        x.secondTab.onClick.AddListener(secondtabshop);
        x.searchTab.onClick.AddListener(searchtab);
        x.doSearch.onClick.AddListener(searchCall);
        x.dropdownpick.onValueChanged.AddListener(d.HandleInputData);

        foreach (var item in tabColors)
        {
            item.color = Color.white;
        }
        tabColors[0].color = Color.yellow;

    }

    // Update is called once per frame
    float timerkikik;
    void Update()
    {
        timerkikik += Time.deltaTime;
        if(timerkikik > 0.2)
        {
            if (createAccountObject.activeInHierarchy == false && uploadObject.activeInHierarchy == false)
            {
                captchaObject.SetActive(false);
            }
            else
            {
                captchaObject.SetActive(true);
                
            }
        }
       
    }
    public GameObject maintab;
    public GameObject secondtab;
    public GameObject searchTab;
    public List<Image> tabColors = new List<Image>();
    public TMP_InputField enterAnswer;
    public void maintabshop()
    {
        maintab.SetActive(true);
  
        searchTab.SetActive(false);
        secondtab.SetActive(false);
        foreach (var item in tabColors)
        {
            item.color = Color.white;
        }
        tabColors[0].color = Color.yellow;
    }
    public void secondtabshop()
    {
        maintab.SetActive(false);
        searchTab.SetActive(false);
        gameObject.GetComponent<HandleTheDropdown>().CmdGetTheMostpopular(NetworkClient.connection.connectionId);
            secondtab.SetActive(true);
        foreach (var item in tabColors)
        {
            item.color = Color.white;
        }
        tabColors[1].color = Color.yellow;



    }
   
    public TMP_InputField searchBar;
    public void searchtab()
    {
        maintab.SetActive(false);
            secondtab.SetActive(false);
        
        searchTab.SetActive(true);
        foreach (var item in tabColors)
        {
            item.color = Color.white;
        }
        tabColors[2].color = Color.yellow;
    }
    public void searchCall()
    {
        gameObject.GetComponent<HandleTheDropdown>().CmdSearch(NetworkClient.connection.connectionId, searchBar.text);
    }
    public void resetCreateaccountObject()
    {
        createAccountObject.SetActive(true);
        captchaObject.SetActive(true);
        gameObject.GetComponent<CaptchaScript>().CmdgetTheCaptcha(NetworkClient.connection.connectionId);
    }
    public void LeaveButton()
    {
        NetworkManager.singleton.StopClient();
        SceneManager.LoadScene(0);
    }
    public GameObject shopCanvas;
    public GameObject gameCanvas;
    public void createAccount()
    {
       
           
            
              
                gameObject.GetComponent<LeaderboardList>().CmdnewAcccount(enterusername.text, enterpass.text, enterAnswer.text, NetworkClient.connection.connectionId);
                
              
            
            
          
                PlayerPrefs.SetString("password", enterpass.text);
                PlayerPrefs.SetString("username", enterusername.text);
                createAccountObject.SetActive(false);
                PlayerPrefs.SetInt("accountCreated", 1);
                createAccountObject.SetActive(false);
                captchaObject.SetActive(false);
        if (PlayerPrefs.GetInt("Compete") == 2)
        {
            shopCanvas.SetActive(false);
                gameCanvas.SetActive(true);
            submitweightsthing.SetActive(true);

            PlayerPrefs.SetInt("Compete", 1);
        }
            
           
        
       
    }
    public GameObject submitweightsthing;
    List<ViewsandIndexa> sentImagesandViews = new List<ViewsandIndexa>();
    List<CaptchaCtor> bytes = new List<CaptchaCtor>();

    

    public GameObject captchaObject;
    public GameObject createAccountObject;
    public void UploadGameObjectTrue()
    {
        if (uploadObject.gameObject.activeSelf == false)
        {
            uploadObject.gameObject.SetActive(true);
            captchaObject.SetActive(true);
            gameObject.GetComponent<CaptchaScript>().CmdgetTheCaptcha(NetworkClient.connection.connectionId);
        }
        else
        {
            uploadObject.SetActive(false);
            if (createAccountObject.gameObject.activeSelf == true)
            {
                captchaObject.SetActive(true);
            }
        }
    }
    
}   
