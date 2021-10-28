using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BtnBehavior : MonoBehaviour
{
    	public void quitGame()
    	{
    		Debug.Log("QUIT!");
    		Application.Quit();
    	}
}
