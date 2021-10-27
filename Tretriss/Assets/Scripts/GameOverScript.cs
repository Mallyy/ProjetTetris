using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public static GameObject gameOverCanvas;
    public int test;
    public static void resetScore()
    {
        Score.scoreValue = 0;
    }

    public static void updateHighScore()
    {
        
    }
    public static void updateGameOverCanva()
    {
        if (Group.isGameOver == true)
        {
            gameOverCanvas.SetActive(true);
        }
        else
            gameOverCanvas.SetActive(false);
    }
    
    void Start()
    {
        gameOverCanvas = GameObject.Find("GameoverCanva");
    }

    // Update is called once per frame
    void Update()
    {
            updateGameOverCanva();
            // Debug.Log("test update GameoverScript");
    }
}
