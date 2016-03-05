using UnityEngine;
using System.Collections;
using UnityEngine.UI;


public class GameController : MonoBehaviour {

    public NejikoController nejiko;
    //scorelabel
    public Text scoreLabel;
    public LifePanel lifePanel;
	
	// Update is called once per frame
	void Update () {
        int score = CalcScore();
        //スコアの更新
        scoreLabel.text = "Score : " + score + "m";

        //ライフパネル更新
        lifePanel.UpdateLife(nejiko.Life());

        //ねじこのライフが０になったらゲームオーバー
        if(nejiko.Life() <= 0) {
            //updateを止める
            enabled = false;

            //ハイスコアを更新
            if(PlayerPrefs.GetInt("HighScore") < score) {
                PlayerPrefs.SetInt("HighScore", score);
            }

            //2秒後にreturnToTitleを呼び出す
            Invoke("ReturnToTitle", 2.0f);
        }
	
	}

    int CalcScore() {
        //ネジコの走行距離をスコアにする
        return (int)nejiko.transform.position.z;
    }

    void ReturnToTitle() {
        Application.LoadLevel("Title");
    }
}
