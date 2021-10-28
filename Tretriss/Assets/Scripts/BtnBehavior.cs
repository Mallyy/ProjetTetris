using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnBehavior : MonoBehaviour
{

	public void startGame()
	{
		SceneManager.LoadScene("SampleScene");
		GameOverScript.resetScore();
	}

	public void accueil()
	{
		SceneManager.LoadScene("Scenes/ScèneMenue");
	}
    	public void quitGame()
    	{
    		Debug.Log("QUIT!");
    		Application.Quit();
    	}
}
