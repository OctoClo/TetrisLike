using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    public GameObject[] tetrominos;

    private int boardWidth = 10;
    private int boardHeigt = 19;

	void Start ()
    {
        SpawnNextTetromino();
	}
	
	void Update ()
    {
		
	}

    private void SpawnNextTetromino()
    {
        // Random index
        int index = Random.Range(0, tetrominos.Length);

        // Spawn a Tetromino at current position with no rotation
        Instantiate(tetrominos[index], transform.position, Quaternion.identity);
    }

    public bool IsInsideBoard (Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < boardWidth && (int)position.y < boardHeigt);
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
