using Mirror;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class initaiterrr : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        StartCoroutine(starteR());

    }
    IEnumerator starteR()
    {
        yield return new WaitForSeconds(0.2f);
       // try
       // {
            NetworkManager.singleton.StartHost();
      ///  }
        //catch
       // {
            //NetworkManager.singleton.StartClient();
       // }
    }
       
    
}
