using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class StageGenerator : MonoBehaviour {

    const int StageTipSize = 30;

    //現在のステージチップ
    int currentTipIndex;

    //ターゲットキャラの指定
    public Transform character;
    //ステージチップの配列
    public GameObject[] stageTips;
    //自動生成開始インデックス
    public int startTipIndex;
    //自動先読み個数
    public int preInstantiate;
    //生成済ステージチップの保持リスト
    public List<GameObject> generatedStageList = new List<GameObject>();

	// Use this for initialization
	void Start () {
        //初期化
        currentTipIndex = startTipIndex - 1;
        UpdateStage(preInstantiate);

    }
	
	// Update is called once per frame
	void Update () {
        //キャラの位置から現在のステージチップのインデックスを計算
        int charaPositionIndex = (int)(character.position.z / StageTipSize);
        //次のステージチップに入ったらステージ更新処理を行う
        if(charaPositionIndex + preInstantiate > currentTipIndex) {

            UpdateStage(charaPositionIndex + preInstantiate);
        }
	
	}

    //指定のindexまでのステージチップを生成して、管理下に置く
    void UpdateStage(int toTipIndex) {
        if (toTipIndex <= currentTipIndex) return;

        //指定のステージチップまでを生成
        for(int i = currentTipIndex + 1; i <= toTipIndex; i++) {
            GameObject stageObject = GenerateStage(i);

            //生成したステージチップを管理リストに追加
            generatedStageList.Add(stageObject);
        }

        //ステージ保持上限内になるまで古いステージを削除
        while (generatedStageList.Count > preInstantiate + 2) DestroyOldestStage();

        currentTipIndex = toTipIndex;
    }

    //指定のインデックス位置にStageオブジェクトをランダムに生成
    GameObject GenerateStage(int tipIndex) {
        //次のステージをランダムに選択
        int nextStageTip = Random.Range(0, stageTips.Length);

        //ステージ生成処理
        GameObject stageObject = (GameObject)Instantiate(
            stageTips[nextStageTip],
            new Vector3(0, 0, tipIndex * StageTipSize),
            Quaternion.identity
        );

        return stageObject;

    }

    //古いステージを削除
    void DestroyOldestStage() {
        GameObject oldStage = generatedStageList[0];
        generatedStageList.RemoveAt(0);
        Destroy(oldStage);
    }


}
