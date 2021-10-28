using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
    private Text textScore;
    private Text textLign;
    public static int level = 1;
    public static int lignDeleted = 0; 
    public static  float fallTime = 0.8f;
    public static void scoring(int RowDeleted)
    {
        int scoreTemp = 0;
        switch (RowDeleted)
        {
            case 0 :
                //Debug.Log("no rows deleted");
                break;
            case 1 : 
                scoreTemp = 40 * level;
                lignDeleted++;
                updateFallTime();
                break;
            case 2 :
                scoreTemp = 100 * level;
                lignDeleted += 2;
                updateFallTime();
                break;
            case 3 : 
                scoreTemp = 300 * level;
                lignDeleted += 3;
                updateFallTime();
                break;
            case 4 :
                scoreTemp = 1200 * level;
                lignDeleted += 4;
                updateFallTime();
                break; 
            default:
                scoreTemp = 0;
                Debug.Log("error score attribution . scoringMethod");
                break;
        }
        scoreValue += scoreTemp;
        
       //Debug.Log("score : ");
    }

    public static void scoringSoftLanding()
    {
        scoreValue += 5; 
    }

    // Start is called before the first frame update
    void Start()
    {
        textScore = GameObject.Find("Score").GetComponent<Text>();
        textLign = GameObject.Find("LignCompter").GetComponent<Text>();
    }
    
    // Update is called once per frame
    void Update()
    {
        textLign.text = "Lignes : " + lignDeleted;
        textScore.text = "Score : " + scoreValue;
    }
    public static void updateFallTime()
    {
        fallTime = 0.8f / Score.lignDeleted;
        Debug.Log(fallTime);
    }


}
