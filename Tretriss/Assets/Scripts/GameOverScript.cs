using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScript : MonoBehaviour
{
    public static GameObject gameOverCanvas;
    public int test;
    static AudioSource audioGameover;

    public static void resetScore()
    
    { 
        //reset les parametres pour une nouvelle partie
        Score.scoreValue = 0;
        Score.lignDeleted = 0;
        Group.isGameOver = false;
        Score.fallTime = 0.8f;
    }

    public static void updateHighScore()
    {
       
    }
    
    public static void updateGameOverCanva()
    {
        if (Group.isGameOver == true)
        {
            gameOverCanvas.SetActive(true);
            audioGameover.PlayOneShot(audioGameover.clip);
        }
        else
            gameOverCanvas.SetActive(false);
    }
    
    void Start()
    {
        audioGameover = GetComponent<AudioSource>();
        gameOverCanvas = GameObject.Find("GameoverCanva");
    }

    // Update is called once per frame
    void Update()
    {
            updateGameOverCanva();
            // Debug.Log("test update GameoverScript");
    }
}
