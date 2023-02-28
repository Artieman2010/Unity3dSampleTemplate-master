using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ContinueAfterTOS : MonoBehaviour
{
    // Start is called before the first frame update
   public void IAGree()
    {
        PlayerPrefs.SetInt("TOSAgreed", 1);
        SceneManager.LoadScene(0);
    }
}
