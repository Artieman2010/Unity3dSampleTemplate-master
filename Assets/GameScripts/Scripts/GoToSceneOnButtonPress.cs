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
		
		Directory.CreateDirectory(Application.persistentDataPath + "/dat");
		//create new files
        for (int i = 0; i < 2; i++)
        {
			try
			{
                FileStream fs = new FileStream(Application.persistentDataPath + $"/WeightSave{i}.json", FileMode.CreateNew);
				FileStream fsd = new FileStream(Application.persistentDataPath + $"/dat/WeightSaveMeta{i}.mta", FileMode.CreateNew);
				FileStream fs2 = new FileStream(Application.persistentDataPath + $"/dat/MutVars{i}.bin", FileMode.CreateNew);



                    //change to something more usable in future
            }
			catch
			{

			}
          

        }
		FileStream fsl = new FileStream(Application.persistentDataPath + "/dat/hist.txt", FileMode.CreateNew);

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
	public void Quit()
	{
		Application.Quit();
	}
}
