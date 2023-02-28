using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GoToSceneOnButtonPress : MonoBehaviour
{
    public string sceneName;
	public bool isShop;
	public bool isCompete;
	private void Start()
	{
		if(PlayerPrefs.GetInt("TOSAgreed") == 0)
		{
			SceneManager.LoadScene("TOSScene");
		}
        PlayerPrefs.SetInt("isShop", 0);
		PlayerPrefs.SetInt("Compete", 0);
		if(PlayerPrefs.GetInt("hasLoadedWeights") == 0)
		{
			StartCoroutine(createFiles());
		}
    }
	IEnumerator createFiles()
	{

        for (int i = 0; i < 2; i++)
        {
			try
			{
                FileStream fs = new FileStream(Application.dataPath + $"/PlayerDataddFile{i}.json", FileMode.CreateNew); //change to something more usable in future
            }
			catch
			{

			}
          

        }
		yield return new WaitForSeconds(0.1f);
        PlayerPrefs.SetInt("hasLoadedWeights", 1);
    }
	public void GoToScene(string name)
	{
        PlayerPrefs.SetInt("isShop", 0);
        PlayerPrefs.SetInt("Compete", 0);
        if (isShop)
		{
			PlayerPrefs.SetInt("isShop", 1);
			//remember to set to zero once in shop
		}
		if (isCompete)
		{
            PlayerPrefs.SetInt("Compete", 1);
        }
		SceneManager.LoadSceneAsync(name); 
	}
}
