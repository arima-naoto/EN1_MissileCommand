using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;
using UnityEngine.Assertions;

public class Missile : MonoBehaviour
{
    // Explosionのプレハブ,MissileはExplosionを生成する
    [SerializeField]
    private Explosion explosionPrefab;
    // 移動速度
    [SerializeField]
    private float speed_;
    // 移動量
    private Vector3 velocity_;
    // レティクル保存用
    private GameObject reticle_;

    void Start()
    {
        
    }

    void Update()
    {
        //距離の二乗の計算。距離の比較的なので二乗のまま使う
        float distanceSpr = Vector3.SqrMagnitude(
            reticle_.transform.position- transform.position);

        Vector3 velocityDeltaTime = velocity_ * Time.deltaTime;

        float veloicityDistanceSqr = 
              Vector3.SqrMagnitude(velocityDeltaTime);
        //十分に遠かったら移動
        if(distanceSpr >= veloicityDistanceSqr){
            transform.position += velocityDeltaTime;
            return;
        }
        //近かったらレティクルをかなさって爆破
        transform.position = reticle_.transform.position;
        this.Explosion();
    }

    public void SetUp(GameObject reticle){
        //レティクルの格納
        reticle_ = reticle;

        //生成位置とレティクルが一致していたら即爆発
        if(reticle.transform.position != transform.position){
            //1.移動量の計算
            this.SetVelocity();
            //2.向きの計算
            this.LookAtReticle();
        }else{
            //3.爆発
            this.Explosion();
        }
    }

    private void SetVelocity(){
        //座標の差分を取り、ベクトルを算出
        Vector3 direction = (
            reticle_.transform.position - transform.position);

        //ベクトルがゼロでないことを確認
        Assert.IsTrue(direction != Vector3.zero);
        //ベクトルを正規化
        direction = direction.normalized;
        //ベクトルに移動速度をかけ移動量とする
        velocity_ = direction * speed_;
    }

    private void LookAtReticle(){
        //角度の算出。
        float angle = Mathf.Atan2(
            velocity_.y,velocity_.x
        ) * Mathf.Rad2Deg;

        //角度を90度傾ける
        angle -= 90; 

        //ゲームオブジェクトを回転させる。
        transform.rotation = Quaternion.Euler(0,0,angle);
    }

    private void Explosion(){
        //爆発の生成
        Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        //爆発と共にレティクルも消す
        Destroy(reticle_);
        //自身も消す
        Destroy(gameObject);
    }

}
