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
                {
                    if (boardMinos[x, y].parent == tetromino.transform)
                        // Delete it, because we're moving that tetromino
                        boardMinos[x, y] = null;
                }
            }
        }

        // Then, update the current position
        foreach (Transform mino in tetromino.transform)
        {
            Vector2 position = Round(mino.position);

            if (position.y >= 0)
                boardMinos[(int)position.x, (int)position.y] = mino;
        }
    }

    public Transform getMinoAtPosition (Vector2 position)
    {
        if (position.y < 0)
            return null;
        else
            return boardMinos[(int)position.x, (int)position.y];
    }

    private bool IsRowFull (int y)
    {
        // For each cell in the given row
        for (int x = 0; x < boardWidth; x++)
        {
            // If it is null, then the row is not full
            if (boardMinos[x, y] == null)
                return false;
        }

        return true;
    }

    private void DeleteRow (int y)
    {
        // For each cell in the given row, destroy and delete the mino ;-;
        for (int x = 0; x < boardWidth; x++)
        {
            Destroy(boardMinos[x, y].gameObject);
            boardMinos[x, y] = null;
        }
    }

    private void MoveRowUp (int y)
    {
        // For each cell in the given row, translate their content into the above row
        for (int x = 0; x < boardWidth; x++)
        {
            if (boardMinos[x, y] != null)
            {
                boardMinos[x, y + 1] = boardMinos[x, y];
                boardMinos[x, y] = null;
                boardMinos[x, y + 1].position += new Vector3(0, 1, 0);
            }
        }
    }

    private void MoveAllRowsUp (int startingRow)
    {
        for (int currentRow = startingRow; currentRow >= 0; currentRow--)
            MoveRowUp(currentRow);
    }

    public void CheckIfWeShouldDeleteSomeRow ()
    {
        // For each row, if it is full, then delete it and move up all rows below it
        for (int y = 0; y < boardHeight; y++)
        {
            if (IsRowFull(y))
            {
                DeleteRow(y);
                MoveAllRowsUp(y - 1);
                y--;
            }
        }
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
        // /!\ Do not check if it's above 0 in y
        return ((int)position.x >= 0 && (int)position.x < boardWidth && (int)position.y < boardHeight);
    }

    public void GameOver ()
    {
        Application.LoadLevel("GameOver");
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
