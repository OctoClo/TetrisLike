using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

    // Tetrominos that can be spawned later on
    public Tetromino[] tetrominos;

    // Dimensions of the board
    private static int boardWidth = 10;
    private static int boardHeight = 19;

    // Array representing each cell of the board and its mino if any
    private static Transform[,] boardMinos = new Transform[boardWidth, boardHeight];

	void Start ()
    {
        // Spawn the first tetromino
        SpawnNextTetromino();
	}
	
	public void UpdateBoard (Tetromino tetromino)
    {
        // For each cell in the game board
        for (int x = 0; x < boardWidth; x++)
        {
            for (int y = 0; y < boardHeight; y++)
            {
                // If the cell's mino's parent (i.e. tetromino) equals the one we were given
                if (boardMinos[x, y] != null)
                    if (boardMinos[x, y].parent == tetromino.transform)
                        // Delete it, because we're moving that tetromino
                        boardMinos[x, y] = null;
            }
        }

        // Then, update the current position
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 position = Round(mino.position);

            if (position.y > 0)
                boardMinos[(int)position.x, (int)position.y] = mino;
        }
    }

    public Transform getMinoAtPosition (Vector2 position)
    {
        if (position.y <= 0)
            return null;
        else
            return boardMinos[(int)position.x, (int)position.y];
    }

    public void SpawnNextTetromino()
    {
        // Random index
        int index = Random.Range(0, tetrominos.Length);

        // Spawn a Tetromino at current position with no rotation
        Instantiate(tetrominos[index], transform.position, Quaternion.identity);
    }

    public bool IsInsideBoard (Vector2 position)
    {
        return ((int)position.x >= 0 && (int)position.x < boardWidth && (int)position.y < boardHeight);
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
