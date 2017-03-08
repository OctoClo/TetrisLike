using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    // Tetrominos that can be spawned later on
    public Tetromino[] tetrominos;

    // Some UI objects
    public GameObject pauseZone;
    public GameObject gameOverZone;
    public Text scoreText;
    public Button pauseButton;

    public Image character;
    public Sprite[] characters;
    public Text characterText;    
    public string[] characterTexts;

    // Dimensions of the board
    private static int BOARD_WIDTH = 10;
    private static int BOARD_HEIGHT = 19;

    // Array representing each cell of the board and its mino if any
    private static Transform[,] boardMinos = new Transform[BOARD_WIDTH, BOARD_HEIGHT];

    // Number of points for clearing a row
    private const int SCORE_BY_ROW_CLEARED = 10;

    // Score and number of rows cleared
    private int score;
    private int numberOfRowsCleared;

    // Speed the tetrominos are falling at (default : 1 unit per second)
    public float speed;

    // Number of rows to clear before increasing speed
    private const int NUMBER_OF_ROWS_TO_INCREASE_SPEED = 5;

    void Awake()
    {
        // Deactivate pause and game over zones
        pauseZone.SetActive(false);
        gameOverZone.SetActive(false);
    } 

    void Start ()
    {
        // Some initializations
        score = 0;
        numberOfRowsCleared = 0;

        UpdateCharacter();

        // Spawn the first tetromino
        SpawnNextTetromino();        
	}

    private void UpdateCharacter()
    {
        // Random character image
        int index = Random.Range(0, characters.Length);
        character.sprite = characters[index];

        // Random character text
        index = Random.Range(0, characters.Length);
        characterText.text = "\"" + characterTexts[index] + "\"";
    }

    private void UpdateScore ()
    {
        // Increase score and update UI
        score += SCORE_BY_ROW_CLEARED;
        scoreText.text = score.ToString();
    }

    public bool IsItTimeToIncreaseSpeed ()
    {
        // If 5 rows or more have been cleared, it's time to increase speed !
        if (numberOfRowsCleared >= NUMBER_OF_ROWS_TO_INCREASE_SPEED)
        {
            numberOfRowsCleared = 0;
            return true;
        }
        return false;
    }

    public void UpdateBoard (Tetromino tetromino)
    {
        // For each cell in the game board
        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            for (int y = 0; y < BOARD_HEIGHT; y++)
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
        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            // If it is null, then the row is not full
            if (boardMinos[x, y] == null)
                return false;
        }

        // Update the score and increase the number of rows cleared
        UpdateScore();
        numberOfRowsCleared++;
        if (IsItTimeToIncreaseSpeed())
            speed++;
        UpdateCharacter();

        return true;
    }

    private void DeleteRow (int y)
    {
        // For each cell in the given row, destroy and delete the mino ;-;
        for (int x = 0; x < BOARD_WIDTH; x++)
        {
            Destroy(boardMinos[x, y].gameObject);
            boardMinos[x, y] = null;
        }
    }

    private void MoveRowUp (int y)
    {
        // For each cell in the given row, translate their content into the above row
        for (int x = 0; x < BOARD_WIDTH; x++)
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
        for (int y = 0; y < BOARD_HEIGHT; y++)
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
        return ((int)position.x >= 0 && (int)position.x < BOARD_WIDTH && (int)position.y < BOARD_HEIGHT);
    }

    public void PauseGame ()
    {
        Time.timeScale = 0f;
        pauseZone.SetActive(true);
    }

    public void ResumeGame ()
    {
        Time.timeScale = 1f;
        pauseZone.SetActive(false);
    }

    public void GameOver ()
    {
        Time.timeScale = 1f;
        pauseButton.enabled = false;
        gameOverZone.SetActive(true);
    }

    public void Restart ()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void Menu ()
    {
        SceneManager.LoadScene("Menu");
    }

    public Vector2 Round (Vector2 position)
    {
        return new Vector2(Mathf.Round(position.x), Mathf.Round(position.y));
    }
}
