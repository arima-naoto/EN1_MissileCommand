using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Meteor : MonoBehaviour
{
    /// <summary>
    /// 最低落下速度
    /// </summary>
    [SerializeField]private float fallSpeedMin_ = 1;

    /// <summary>
    /// 最高落下速度
    /// </summary>
    [SerializeField]private float fallSpeedMax_ = 3;

    /// <summary>
    /// 爆発プレハブ。生成元から受け取る
    /// </summary>
    private Explosion explosionPrefab_;
    
    /// <summary>
    /// 地面のコライダー。生成元から受け取る
    /// </summary>
　　private BoxCollider2D groundCollider_;
    private Rigidbody2D rb_;
    private GameManeger gameManeger_;

    //スコアエフェクトプレハブ
    [SerializeField]
    ScoreEffect scoreEffectPrefab_;

    void Start()
    {
        rb_ = GetComponent<Rigidbody2D>();

        SetUpVelocity();
    }

   
    void Update()
    {
      
    }

    /// <summary>
    /// 生成元から必要な情報を引き継ぐ
    /// </summary>
    /// <param name="ground">地面</param>
    /// <param name="gameManeger">ゲームマネージャー</param>
    /// <param name="explosionPrefab">爆発プレハブ</param>
    public void SetUp(BoxCollider2D ground,GameManeger gameManeger,Explosion explosionPrefab){

        gameManeger_ = gameManeger;
        groundCollider_ = ground;
        explosionPrefab_ = explosionPrefab;

    }

    /// <summary>
    /// 移動量の設定
    /// </summary>
    private void SetUpVelocity(){
        //地面の上下左右の位置を取得
        float left = groundCollider_.bounds.center.x - groundCollider_.bounds.size.x / 2;
        float right = groundCollider_.bounds.center.x + groundCollider_.bounds.size.x / 2;
        float top = groundCollider_.bounds.center.y + groundCollider_.bounds.size.y / 2;
    
        float targetX = Mathf.Lerp(left,right,Random.Range(0.0f,1.0f));

        Vector3 target = new Vector3(targetX,top,0.0f);
        Vector3 direction = (target - transform.position).normalized;
        float fallSpeed = Random.Range(fallSpeedMin_,fallSpeedMax_);
        rb_.velocity = direction * fallSpeed;
    }

    private void OnTriggerEnter2D(Collider2D collision){
        Explosion explosion;
        if(collision.gameObject.CompareTag("Explosion") && collision.TryGetComponent(out explosion)){
            Explosion(explosion);
        } 
        else if(collision.gameObject.CompareTag("Ground")){
            Fall();
        }
    }

    /// <summary>
    /// 爆発
    /// </summary>
    private void Explosion(Explosion otherExplosion){

        //連鎖数の取得と加算
        int chainNum = otherExplosion.chainNum + 1;
        //連鎖数に応じたスコアを使用
        int score = chainNum * 100;
        ScoreEffect scoreEffect = Instantiate(scoreEffectPrefab_,
        transform.position,Quaternion.identity);

        scoreEffect.SetScore(score);

        //GameManegerにscore加算を通知
        gameManeger_.AddScore(score);

        //爆発を生成し
        Explosion explosion = Instantiate(explosionPrefab_,transform.position,Quaternion.identity);

        //生成したExplosionに連鎖数を設定
        explosion.chainNum = chainNum;

        //自身を消滅させる
        Destroy(gameObject);

    }

    /// <summary>
    /// 地面に落下
    /// </summary>
    private void Fall(){
        //GameManegerにダメージの通知
        gameManeger_.Damange(1);

        //自身は消滅
        Destroy(gameObject);
    }
}
