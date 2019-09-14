using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour {
    public Text Score;
    public Text Level;
    public Text Lines;

    GameManager obj = new GameManager();

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        //constantly display and update the UI information
        UIInformation();
      
	}

    public void UIInformation()
    {
        //Change the score, level, and lines text on the UI
        Score.text = GameManager.currentScore.ToString();
        Level.text = GameManager.currentLevel.ToString();
        Lines.text = GameManager.numLinesCleared.ToString();
    }
}
