using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    [SerializeField]
    private GameObject[] tetriminos;

    public static int widthOfGrid = 10;
    public static int heightGrid = 20;

    public static Transform[,] grid = new Transform[widthOfGrid, heightGrid];

    public int pointsOneLine = 40;
    public int pointsTwoLine = 100;
    public int pointsThreeLine = 300;
    public int pointsFourLine = 1200;


    public float fall = 1.0f;

 public static int currentScore = 0;

    private int numRowsInTurn = 0;

    private GameObject previewTetrimino;
    private GameObject nextTetrimino;

    private bool gameStarted = false;

    public static int currentLevel = 0;
    public static int numLinesCleared = 0;

    private Vector2 previewTetriminoPosition = new Vector2(16.0f, 0.5f);
    // will check how many lines the player has cleared and will increment numLines cleared
    public void UpdateScore()
    {
        if (numRowsInTurn > 0)
        {
            if (numRowsInTurn == 1)
            {
                ClearedOneLine();
                numLinesCleared += 1;
            }
            else if (numRowsInTurn == 2)
            {
                ClearedTwoLine();
                numLinesCleared += 2;
            }
            else if (numRowsInTurn == 3)
            {
                ClearedThreeLine();
                numLinesCleared += 3;
            }
            else if (numRowsInTurn == 4)
            {
                ClearedFourLine();
                numLinesCleared += 4;
            }
            numRowsInTurn = 0;
        }
    }
    //when the user clears a specific number of lines the current score will be updated and points will be given
    public void ClearedOneLine()
    {
        currentScore += pointsOneLine + (currentLevel * 20);
    }
    public void ClearedTwoLine()
    {
        currentScore += pointsTwoLine + (currentLevel * 25);
    }
    public void ClearedThreeLine()
    {
        currentScore += pointsThreeLine + (currentLevel * 30); ;
    }
    public void ClearedFourLine()
    {
        currentScore += pointsFourLine + (currentLevel * 35);
    }
    // Use this for initialization
    void Start()
    {

        spawnNextTetrimino();
    }

    void Update()
    {
        //updates the score, level, and speed
        UpdateScore();
        updateLevel();
        updateSpeed();
    }
    //when the player clears ten lines they will move to the next level
    public void updateLevel()
    {
        currentLevel = numLinesCleared / 10;

    }
    //when the player levels up the tetrimino fall speed will be increased
    void updateSpeed()
    {
        fall = 1.0f - ((float)currentLevel * 0.1f);

    }
    //resets current score
    public void resetCurrentScore()
    {
        currentScore = 0;
    }
    //resets number of lines cleared
    public void resetNumLines()
    {
        numLinesCleared = 0;
    }
    //resets current level
    public void resetCurrentLevel()
    {
        currentLevel = 0;
    }
    // this method will check if the tetrimino piece is above the grid
    //it will check an individual cube in a tetrimino to check whether the 
    //object has gone outside of the grid

    public bool CheckIsAboveGrid(Tetrimino tetrimino)
    {
        for (int x = 0; x < widthOfGrid; ++x)
        {
            foreach (Transform mino in tetrimino.transform)
            {
                Vector2 pos = round(mino.position);
                if (pos.y > heightGrid - 1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    //when a tetrimino piece goes outside of the top of the grid the gameover scene will be called
    public void GameOver()
    {
        SceneManager.LoadScene("GameOver");
    }
    //check if a full row exists within the x-axis if there is one return true else false
    public bool IsSFullRowAt(int y)
    {
        for (int x = 0; x < widthOfGrid; x++)
        {
            if (grid[x, y] == null)
            {
                return false;
            }
        }

        //found full row
        numRowsInTurn++;

        return true;
    }
    //when a full row is found then the minos at that row will be deleted
    public void DeleteMinoAt(int y)
    {
        for (int x = 0; x < widthOfGrid; x++)
        {
            Destroy(grid[x, y].gameObject);
            grid[x, y] = null;
        }
    }
    //when a row is deleted any rows that were on top of that row will have to move down
    public void moveRowDown(int y)
    {
        for (int x = 0; x < widthOfGrid; x++)
        {
            if (grid[x, y] != null)
            {
                grid[x, y - 1] = grid[x, y];
                grid[x, y] = null;
                grid[x, y - 1].position += new Vector3(0, -1, 0);
            }
        }
    }
    //moves all rows down
    public void moveAllRowsDown(int y)
    {
        for (int i = y; i < heightGrid; ++i)
        {
            moveRowDown(i);
        }
    }
    //deletes an individual row and moves all rows on top of that row down
    public void DeleteRow()
    {
        for (int y = 0; y < heightGrid; ++y)
        {
            if (IsSFullRowAt(y))
            {
                DeleteMinoAt(y);
                moveAllRowsDown(y + 1);
                --y;
            }
        }
    }

    //will check to see if a tetrimino has moved outside the grid
    public void updateGrid(Tetrimino tetrimino)
    {
        for (int y = 0; y < heightGrid; ++y)
        {
            for (int x = 0; x < widthOfGrid; ++x)
            {
                if (grid[x, y] != null)
                {
                    if (grid[x, y].parent == tetrimino.transform)
                    {
                        grid[x, y] = null;

                    }
                }
            }
        }
        foreach (Transform mino in tetrimino.transform)
        {
            Vector2 pos = round(mino.position);
            if (pos.y < heightGrid)
            {
                grid[(int)pos.x, (int)pos.y] = mino;
            }
        }
    }
    
    public Transform GetTransformAtGridPosition(Vector2 pos)
    {

        if (pos.y > heightGrid - 1)
        {
            return null;
        }
        else
        {
            return grid[(int)pos.x, (int)pos.y];
        }
    }
    //spawns the next tetrimino at the top of the grid and previews the next tetrimino as well
    public void spawnNextTetrimino()
    {
        if (!gameStarted)
        {
            gameStarted = true;
            nextTetrimino = Instantiate(tetriminos[Random.Range(0, tetriminos.Length)], new Vector2(5.0f, 20.0f), Quaternion.identity);
            previewTetrimino = Instantiate(tetriminos[Random.Range(0, tetriminos.Length)], previewTetriminoPosition, Quaternion.identity);

            previewTetrimino.GetComponent<Tetrimino>().enabled = false;

        }
        else
        {
            previewTetrimino.transform.localPosition = new Vector2(5.0f, 20.0f);
            nextTetrimino = previewTetrimino;
            nextTetrimino.GetComponent<Tetrimino>().enabled = true;

            previewTetrimino = Instantiate(tetriminos[Random.Range(0, tetriminos.Length)], previewTetriminoPosition, Quaternion.identity);

            previewTetrimino.GetComponent<Tetrimino>().enabled = false;

        }

    }
    //checks if a tetrimino is inside the grid
    public bool insideGrid(Vector2 pos)
    {
        return ((int)pos.x >= 0 && (int)pos.x < widthOfGrid && (int)pos.y >= 0);
    }

    //rounds the position of a tetrimino to a whole number
    public Vector2 round(Vector2 pos)
    {
        return new Vector2(Mathf.Round(pos.x), Mathf.Round(pos.y));
    }

}