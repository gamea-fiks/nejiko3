using UnityEngine;
using System.Collections;

public class NejikoController : MonoBehaviour {

    const int MinLane = -2;
    const int MaxLane = 2;
    const float LaneWidth = 1.0f;
    const int DefaultLife = 3;
    const float StunDuration = 0.5f;

    CharacterController controller;
    Animator animator;

    //移動する方向
    Vector3 moveDirection = Vector3.zero;
    int targetLane;
    int life = DefaultLife;
    float recoverTime = 0.0f;

    public float gravity;
    public float speedZ;
    public float speedX;
    public float speedJump;
    //前進加速度のパラメーター
    public float accelerationZ;

    //ライフ取得関数
    public int Life() {
        return life;
    }

    //気絶判定
    public bool IsStan() {
        return recoverTime > 0.0f || life <= 0;
    }  


    // Use this for initialization
    void Start () {
        //コンポーネントの取得
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        /*
        初期ソース

        //地上にいる場合のみ操作を行う
        //接地判定
        if(controller.isGrounded) {
            //入力で移動を行う
           if(Input.GetAxis("Vertical") > 0.0f) {
                moveDirection.z = Input.GetAxis("Vertical") * speedZ;
            } else {
                moveDirection.z = 0;
            }

            //方向転換
            transform.Rotate(0, Input.GetAxis("Horizontal") * 3, 0);

            //ジャンプ
            if (Input.GetButton("Jump")) {
                moveDirection.y = speedJump;
                animator.SetTrigger("jump");
            }

        }
        */

        //デバッグ用
        if (Input.GetKeyDown("left")) MoveToLeft();
        if (Input.GetKeyDown("right")) MoveToRight();
        if (Input.GetKeyDown("space")) Jump();

        //気絶時の行動
        if(IsStan()) {
            //動きを止め気絶からの復帰カウントを進める
            moveDirection.x = 0.0f;
            moveDirection.z = 0.0f;
            recoverTime -= Time.deltaTime;
        } else {
            //徐々に加速しZ方向に常に前進　　前進
            float acceleratedZ = moveDirection.z + (accelerationZ * Time.deltaTime);
            //Mathf.Clamp関数でspeedZ以上にならないよう制限
            //Mathf.Clamp(a,b,c) b < a < c の値に収める関数
            moveDirection.z = Mathf.Clamp(acceleratedZ, 0, speedZ);

            //X方向は目標のポジションまでの差分の割合で速度を計算  横移動
            //遠いほど速く移動
            float ratioX = (targetLane * LaneWidth - transform.position.x) / LaneWidth;
            moveDirection.x = ratioX * speedX;

        }

        //重力分の力を毎フレーム追加
        moveDirection.y -= gravity * Time.deltaTime;

        //移動実行
        Vector3 globalDirection = transform.TransformDirection(moveDirection);
        controller.Move(globalDirection * Time.deltaTime);

        //移動後、接地していたらY方向の速度はリセット
        if (controller.isGrounded) moveDirection.y = 0;

        //速度が0以上なら走るフラグをtrueにする
        animator.SetBool("run", moveDirection.z > 0.0f);


    }

    public void MoveToLeft() {
        //気絶時の入力キャンセル
        if (IsStan()) return;
        //目標レーンの変更
        if (controller.isGrounded && targetLane > MinLane) targetLane--;
    }

    public void MoveToRight() {
        //気絶時の入力キャンセル
        if (IsStan()) return;
        //目標レーンの変更
        if (controller.isGrounded && targetLane < MaxLane) targetLane++;
    }

    public void Jump() {
        //気絶時の入力キャンセル
        if (IsStan()) return;
        if (controller.isGrounded) {
            moveDirection.y = speedJump;
            //ジャンプトリガー発動
            animator.SetTrigger("jump");
        }
        
    }

    //CharacterControllerにコリジョン（衝突判定）が生じたときの処理
    void OnControllerColliderHit(ControllerColliderHit hit) {
        if (IsStan()) return;

        //ヒット処理
        if(hit.gameObject.tag == "Robo") {
            //ライフを減らして気絶状態へ移行
            life--;
            recoverTime = StunDuration;

            //ダメージトリガー判定
            animator.SetTrigger("damage");

            //ヒットしたオブジェクトは削除
            Destroy(hit.gameObject);
        }

    }


}
