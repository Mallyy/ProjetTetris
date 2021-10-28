using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BtnBehavior : MonoBehaviour
{

	public void startGame()
	{
		SceneManager.LoadScene("SampleScene");
	}
    	public void quitGame()
    	{
    		Debug.Log("QUIT!");
    		Application.Quit();
    	}
}
