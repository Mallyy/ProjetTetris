using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Group : MonoBehaviour
{
    // Time since last gravity tick
    float lastFall = 0;
    public static bool isGameOver = false; 
    AudioSource audioFall;

    //Touch variable
    Vector2 initialPosition = Vector2.zero;
    float touchBeginTime;
    readonly float swipeMaxDuration = 1f;
    
    bool isValidGridPos() {        
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);

            // Not inside Border?
            if (!Playfield.insideBorder(v))
                return false;

            // Block in grid cell (and not part of same group)?
            if (Playfield.grid[(int)v.x, (int)v.y] != null &&
                Playfield.grid[(int)v.x, (int)v.y].parent != transform)
                return false;
        }
        return true;
    }
    
    void updateGrid() {
        // Remove old children from grid
        for (int y = 0; y < Playfield.h; ++y)
        for (int x = 0; x < Playfield.w; ++x)
            if (Playfield.grid[x, y] != null)
                if (Playfield.grid[x, y].parent == transform)
                    Playfield.grid[x, y] = null;

        // Add new children to grid
        foreach (Transform child in transform) {
            Vector2 v = Playfield.roundVec2(child.position);
            Playfield.grid[(int)v.x, (int)v.y] = child;
        }        
    }
    // Start is called before the first frame update
    void Start()
    {
        audioFall = GetComponent<AudioSource>();
        // Default position not valid? Then it's game over
        if (!isValidGridPos()) {
            Debug.Log("GAME OVER");
            Destroy(gameObject);
            isGameOver = true;
            GameOverScript.updateGameOverCanva();

        }
    }

    // Update is called once per frame
    void Update()
    {
        //Mobile inputs
        if (Input.touchCount > 0)
        {
            var touch = Input.GetTouch(0);
            
            if (touch.phase == TouchPhase.Began)
            {
                TouchBegan(touch);
            }
            else if (touch.phase == TouchPhase.Ended)
            {
                var touchDuration = Time.time - touchBeginTime;

                if (touchDuration < swipeMaxDuration)
                {
                    HandleMove(touch);
                }
            }
        }
        
        //Keyboard
        // Move Left
        else if (Input.GetKeyDown(KeyCode.LeftArrow)) {
            moveLeft();
        }

        // Move Right
        else if (Input.GetKeyDown(KeyCode.RightArrow)) {
            moveRight();
        }

        // Rotate
        else if (Input.GetKeyDown(KeyCode.UpArrow)) {
            rotate();
        }

        // Move Downwards and Fall
        else if (Input.GetKeyDown(KeyCode.DownArrow) ||
                 Time.time - lastFall >= 1) {
            //update score if valid 
            // increment score 
            if(Input.GetKeyDown(KeyCode.DownArrow))
            {
                Score.scoringSoftLanding();
            }
            fall();
        }

    }

    void TouchBegan(Touch touch)
    {
        initialPosition = touch.position;
        touchBeginTime = Time.time;
    }
    void HandleMove(Touch touch)
    {
        Vector2 diff = touch.position - initialPosition;
        if (Math.Abs(diff.x) > Math.Abs(diff.y))
        {
            if (diff.x > 0)
            {
                moveRight();
            }
            else if(diff.x < 0)
            {
                moveLeft();
            }
        }
        else
        {
            if (diff.y > 0)
            {
                rotate();
            }
            else if (diff.y < 0) //swipe bas
            {
                fall();
                Score.scoringSoftLanding();
            }
        }
    }
    
    void moveLeft()
    {
        // Modify position
        transform.position += new Vector3(-1, 0, 0);
       
        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            transform.position += new Vector3(1, 0, 0);
    }

    void moveRight()
    {
        // Modify position
        transform.position += new Vector3(1, 0, 0);
       
        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            transform.position += new Vector3(-1, 0, 0);
    }

    void rotate()
    {
        transform.Rotate(0, 0, -90);
       
        // See if valid
        if (isValidGridPos())
            // It's valid. Update grid.
            updateGrid();
        else
            // It's not valid. revert.
            transform.Rotate(0, 0, 90);
    }

    void fall()
    {
        // Modify position
        transform.position += new Vector3(0, -1, 0);

        // See if valid
        if (isValidGridPos()) {
            // It's valid. Update grid.
            updateGrid();
            audioFall.Play(0);
        } else {
            // It's not valid. revert.
            transform.position += new Vector3(0, 1, 0);

            // Clear filled horizontal lines
            Playfield.deleteFullRows();

            // Spawn next Group
            FindObjectOfType<Spawner>().spawnNext();

            // Disable script
            enabled = false;
        }

        lastFall = Time.time;
    }
    
}