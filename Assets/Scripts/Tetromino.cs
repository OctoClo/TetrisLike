using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    private GameManager manager;
    
    // List of 3 Vector3 to move the tetromino depending on its direction (LEFT, RIGHT and UP)
    private Vector3[] move = { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;

    // Last time the tetromino falled
    private float previousFallTime;

    // Speed the tetromino is falling at (default : 1 unit per second)
    private float speed = 1f;

    private void Awake ()
    {
        manager = FindObjectOfType<GameManager>();
    }

    void Start ()
    {
		
	}

    void Update ()
    {
        ProcessInput();
	}

    void ProcessInput ()
    {
        // Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += move[LEFT];

            if (!CheckPosition())
                transform.position += move[RIGHT];
        }
            

        // Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += move[RIGHT];

            if (!CheckPosition())
                transform.position += move[LEFT];
        }

        // Rotate by 90 degres
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            transform.Rotate(0, 0, 90);

            if (!CheckPosition())
                transform.Rotate(0, 0, -90);
        }

        // Move up or fall if it's been 1 second since last fall
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Time.time - previousFallTime >= speed)
        {
            transform.position += move[UP];

            if (!CheckPosition())
                transform.position += move[DOWN];
            else
                previousFallTime = Time.time;
        }
    }

    private bool CheckPosition ()
    {
        foreach (Transform mino in transform)
        {
            Vector2 position = manager.Round(mino.position);
            if (!manager.IsInsideBoard(position))
                return false;
        }

        return true;
    }
}
