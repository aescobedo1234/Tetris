using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TetriminoType
{
    I, J, L, O, S, T, Z
};

public class Tetrimino : MonoBehaviour
{
    float fall = 0;
    private float fallSpeed;

    public bool rotate = true;
    public bool rotateLimit = false;

    public int myScore = 100;

    private float myScoreTime;
    // Use this for initialization
    void Start()
    {
        //defaault fall speed when the tetrimino is spawned at the top of the grid
        fallSpeed = GameObject.Find("TetriminoGrid").GetComponent<GameManager>().fall;
    }


    // Update is called once per frame
    void Update()
    {
        //check for user input and update the score
        userInput();
        UpdateMyScore();
    }
    //updates the player score
    void UpdateMyScore()
    {
        if (myScoreTime < 1)
        {
            myScoreTime += Time.deltaTime;
        }
        else
        {
            myScoreTime = 0;
            myScore = Mathf.Max(myScore - 10, 0);
        }
    }
    //check whether the player presses the left, right, up or down arrows
    void userInput()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            transform.position += new Vector3(1, 0, 0);

            if (validPosition())
            {
                FindObjectOfType<GameManager>().updateGrid(this);
            }
            else
            {
                transform.position += new Vector3(-1, 0, 0);
            }

        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            transform.position += new Vector3(-1, 0, 0);

            if (validPosition())
            {
                FindObjectOfType<GameManager>().updateGrid(this);
            }
            else
            {
                transform.position += new Vector3(1, 0, 0);
            }
        }
        //if user presses space move the tetrimino piece downard faster
        else if (Input.GetKeyDown(KeyCode.Space) || Time.time - fall >= fallSpeed)
        {

            transform.position += new Vector3(0, -1, 0);
            if (validPosition())
            {
                FindObjectOfType<GameManager>().updateGrid(this);
            }
            else
            {
                transform.position += new Vector3(0, 1, 0);

                FindObjectOfType<GameManager>().DeleteRow();

                if (FindObjectOfType<GameManager>().CheckIsAboveGrid(this))
                {
                    FindObjectOfType<GameManager>().GameOver();
                }

                enabled = false;

                GameManager.currentScore += myScore;

                FindObjectOfType<GameManager>().spawnNextTetrimino();
            }

            fall = Time.time;

        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            if (rotate)
            {

                if (rotateLimit)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }


                }
                else
                {

                    transform.Rotate(0, 0, 90);
                }
                if (validPosition())
                {
                    FindObjectOfType<GameManager>().updateGrid(this);
                }
                else
                {
                    if (rotateLimit)
                    {


                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }

                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            if (rotate)
            {

                if (rotateLimit)
                {
                    if (transform.rotation.eulerAngles.z >= 90)
                    {
                        transform.Rotate(0, 0, -90);
                    }
                    else
                    {
                        transform.Rotate(0, 0, 90);
                    }


                }
                else
                {

                    transform.Rotate(0, 0, 90);
                }
                if (validPosition())
                {
                    FindObjectOfType<GameManager>().updateGrid(this);
                }
                else
                {
                    if (rotateLimit)
                    {


                        if (transform.rotation.eulerAngles.z >= 90)
                        {
                            transform.Rotate(0, 0, -90);
                        }
                        else
                        {
                            transform.Rotate(0, 0, 90);
                        }

                    }
                    else
                    {
                        transform.Rotate(0, 0, -90);
                    }
                }
            }
        }




    }
    //check if a tetrimino is at a valid position within the grid
    bool validPosition()
    {
        foreach (Transform mino in transform)
        {
            Vector2 pos = FindObjectOfType<GameManager>().round(mino.position);

            if (FindObjectOfType<GameManager>().insideGrid(pos) == false)
            {
                return false;
            }
            if (FindObjectOfType<GameManager>().GetTransformAtGridPosition(pos) != null && FindObjectOfType<GameManager>().GetTransformAtGridPosition(pos).parent != transform)
            {
                return false;
            }
        }
        return true;
    }
}