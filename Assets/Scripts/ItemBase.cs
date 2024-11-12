using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
abstract public class ItemBase : MonoBehaviour
{
    //移動速度。
    [SerializeField]
    protected float speed = 3;
    //画面サイズ確認用
    protected Camera camera_;
    //自身のサイズ確認用
    protected Collider2D collider_;
    
    //初期化処理
    void Start(){
        camera_ = Camera.main;
        collider_ = GetComponent<Collider2D>();
    }
    //更新処理
    protected virtual void Update(){
        //移動処理
        transform.Translate(Vector3.right * speed * Time.deltaTime);
        //画面外の確認
        //ワールド座標上のカメラ右端をカメラから算出
        float worldScreenRight =
          camera_.orthographicSize * camera_.aspect;
        //アイテムの当たり判定のサイズ
        float boundsSize = collider_.bounds.size.x;
        //当たり判定含め完全に画面外に出ていたらDestroy
        if(transform.position.x > worldScreenRight + boundsSize){
            Destroy(this.gameObject);
        }
    }
    //衝突判定
    private void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Explosion")){Get();}
    }
    //アイテムの取得処理抽象メゾット
    public abstract void Get();
}
