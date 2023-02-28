using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OPS.AntiCheat.Field;
using TMPro;
public class ShopScript : MonoBehaviour
{
    public ProtectedString named;

    public ProtectedString publicKey;
    public ProtectedDecimal theCost;
    public ProtectedString theCreator;
    public GameObject thisObjectsCreator;

    public TMP_Text cost_text;
    public TMP_Text creator_text;
    public TMP_Text nametext;
    public TMP_Text speedText;
    public TMP_Text downloadText;
    
    // Start is called before the first frame update
    void Start()
    {
        nametext.text = named;
   
        creator_text.text = "Creator Name: " + theCreator.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void bought()
    {
       
       
       

        thisObjectsCreator.GetComponent<HandleTheDropdown>().givetheparameters(publicKey, theCost, theCreator, named);
       
    }
}
