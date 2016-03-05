using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TitleController : MonoBehaviour {

    public Text HighScoreLabel;

    public void Start() {
        //highscoreを表示
        HighScoreLabel.text = "High Score : " + PlayerPrefs.GetInt("HighScore") + "m";

    }

	public void OnStartButtonClicked() {
        Application.LoadLevel("Main");
    } 
}
