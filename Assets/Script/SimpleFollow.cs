using UnityEngine;
using System.Collections;

public class SimpleFollow : MonoBehaviour {

    Vector3 diff;

    //追いかける相手
    public GameObject target;
    public float followSpeed;

	// Use this for initialization
	void Start () {
        //追従距離の計算
        diff = target.transform.position - transform.position;
	
	}
	
	// Update is called once per frame
	void LateUpdate () {
        //現在のポジションと目標のポジションを一定の割合で縮めていく
        //遠いほど速く、近いほどゆっくりのスムーズな動き
        transform.position = Vector3.Lerp(
            transform.position,
            target.transform.position - diff,
            Time.deltaTime * followSpeed
        );
	}
}
