using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tetromino : MonoBehaviour {

    // Tetromino not allowed to rotate : O
    public bool allowedToRotate = true;

    // Tetrominos that rotate only twice : I, S and Z
    public bool rotateTwiceOnly = false;

    // GameManager instance to manage move checking
    private GameManager manager;
    
    // List of 3 Vector3 to move the tetromino depending on its direction (LEFT, RIGHT, UP and DOWN)
    private Vector3[] move = { new Vector3(-1, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, -1, 0) };
    private const int LEFT = 0;
    private const int RIGHT = 1;
    private const int UP = 2;
    private const int DOWN = 3;

    // Last time the tetromino fell
    private float previousFallTime;

    // Speed the tetromino is falling at (default : 1 unit per second)
    private float speed = 1f;

    void Awake ()
    {
        // Get the instance of GameManager
        manager = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        // If the tetromino spawns in an invalid place, it means game over
        if (!ValidPosition())
        {
            Destroy(gameObject);
            manager.GameOver();
        }

        previousFallTime = Time.time;
    }

    void Update ()
    {
        ProcessInput();
	}

    void ProcessInput ()
    {
        // Arrow left = Move left
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += move[LEFT];

            // If the tetromino is allowed to stay, update the board according to its new position
            if (ValidPosition())
                manager.UpdateBoard(this);

            // Else, abort, abort !
            else
                transform.position += move[RIGHT];
        }
            

        // Arrow right = Move right
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += move[RIGHT];

            // If the tetromino is allowed to stay, update the board according to its new position
            if (ValidPosition())
                manager.UpdateBoard(this);

            // Else, abort, abort !
            else
                transform.position += move[LEFT];
        }

        // Arrow down = Rotate by 90 degres
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (allowedToRotate)
            {
                // If the tetromino rotates only twice, change its rotation based on its current rotation
                if (rotateTwiceOnly)
                    transform.Rotate(0, 0, (transform.rotation.eulerAngles.z >= 90) ? -90 : 90);
                else
                    transform.Rotate(0, 0, 90);

                // If the tetromino is allowed to stay, update the board according to its new position
                if (ValidPosition())
                    manager.UpdateBoard(this);

                // Else, abort, abort !
                else
                {
                    if (rotateTwiceOnly)
                        transform.Rotate(0, 0, (transform.rotation.eulerAngles.z >= 90) ? -90 : 90);
                    else
                        transform.Rotate(0, 0, -90);
                }   
            }
        }

        // Arrow up | n seconds (1 unit / speed) since last fall = Move up
        else if (Input.GetKeyDown(KeyCode.UpArrow) || Time.time - previousFallTime >= (1 / speed))
        {
            transform.position += move[UP];

            // If the tetromino is allowed to stay, update the board according to its new position
            if (ValidPosition())
                manager.UpdateBoard(this);

            // Else, abort, abort ! It means the tetromino has reached the top border, disable it and spawn a new tetromino
            else
            {
                transform.position += move[DOWN];
                manager.CheckIfWeShouldDeleteSomeRow();
                enabled = false;
                manager.SpawnNextTetromino();
            }

            previousFallTime = Time.time;
        }
    }

    private bool ValidPosition ()
    {
        // For each mino in the tetromino
        foreach (Transform mino in transform)
        {
            Vector2 position = manager.Round(mino.position);

            // Check if it is inside the board
            if (!manager.IsInsideBoard(position))
                return false;

            // And if its parent is us : else it means it's hitting some other tetromino
            else if (manager.getMinoAtPosition(position) != null)
            {
                if (manager.getMinoAtPosition(position).parent != transform)
                    return false;
            } 
        }

        return true;
    }
}
