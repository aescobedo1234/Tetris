using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour {


    
    GameManager obj = new GameManager();

public void PlayAgain()
    {
        //reset the player's scores when the player presses play again
        obj.resetCurrentScore();
        obj.resetCurrentLevel();
        obj.resetNumLines();

        //load the game scene after resetting scores
        SceneManager.LoadScene("Game");
       
    }
}
