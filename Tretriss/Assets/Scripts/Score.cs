using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public static int scoreValue = 0;
    private Text textScore; 
    public static int level = 1;
    
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
                break;
            case 2 :
                scoreTemp = 100 * level;
                break;
            case 3 : 
                scoreTemp = 300 * level;
                break;
            case 4 :
                scoreTemp = 1200 * level;
                break; 
            default:
                scoreTemp = 0;
                Debug.Log("error score attribution . scoringMethod");
                break;
        }
        Score.scoreValue += scoreTemp; 
       //Debug.Log("score : ");
    }

    public static void scoringSoftLanding()
    {
        scoreValue += 5; 
    }
    
    // Start is called before the first frame update
    void Start()
    {
        textScore = GetComponent<Text>();    }

    // Update is called once per frame
    void Update()
    {
        
        textScore.text = "Score : " + scoreValue;
    }
}
