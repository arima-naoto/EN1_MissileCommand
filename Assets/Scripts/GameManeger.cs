using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

public class GameManeger : MonoBehaviour
{
    private Camera mainCamera_;

    [SerializeField,Header("Prefab")]
    private Explosion explosionPrefab_;

    //レティクルのプレハブ
    [SerializeField]
    private GameObject reticlePrefab_;
    
    //ミサイルのプレハブ
    [SerializeField]
    private Missile missilePrefab;

    //隕石の生成
    [SerializeField]
    private Meteor meteorPrefab_;

    //アイテムのプレハブの登録用
    [SerializeField]List<ItemBase> items_;

    //隕石の生成関係
    [SerializeField,Header("MeteorSpawner")]
    //隕石がぶつかる地面
    private BoxCollider2D ground_;
    //隕石の生成の時間間隔
    [SerializeField] private float meteorInteval_ = 1;
    //隕石の生成までの時間
    private float meteorTimer_;
    [SerializeField] private List<Transform> spawnPosition_;

    [SerializeField,Header("Missile")]private List<Transform> missilePositions_;
    
    //スコアテキスト
    [SerializeField,Header("ScoreUISettings")]
    private ScoreText scoreText_;
    private static int score_;

    // ライフ関係
    [SerializeField,Header("LifeUISettings")]
    // ライフゲージ
    private LifeBar lifeBar_;
    //最大体力
    [SerializeField] private float maxLife_ = 10;
    //現在体力
    private float life_;

    [SerializeField,Header("ItemSetting")]
    //アイテム生成位置
    private Transform itemSpwanPoint_;
    [SerializeField]
    // アイテム生成の時間間隔
    private float itemSpawnInterval_ = 10;
    //アイテム生成タイマー
    private float itemTimer_ = 0;

    [SerializeField,Header("scene")]
    //次のシーン
    private string nextScene;

    //初期化処理
    void Start()
    {
        //「MainCamera」というタグを持つゲームオブジェクトを検索する
        GameObject mainCameraObject = 
          GameObject.FindGameObjectWithTag("MainCamera");

#if UNITY_EDITOR

        //NULL出ないことを確認する
        Assert.IsNotNull(mainCameraObject,"MainCameraが見つかりませんでした");

        //Cameraコンポーネントが存在し、取得できることを確認する
        Assert.IsTrue(mainCameraObject.TryGetComponent(out mainCamera_),
        "MainCameraにCameraコンポーネントがありません");

        Assert.IsTrue(
            spawnPosition_.Count > 0,
            "spawnPosition_に要素が一つもありません"
        );

        foreach(Transform t in spawnPosition_){
            Assert.IsNotNull(t,"spawnPosition_にNullが含まれています");
        }
#endif
        mainCameraObject.TryGetComponent(out mainCamera_);

        // 体力の初期化
        this.ResetLeft();

        score_ = 0;

    }

    //更新処理
    void Update()
    {
        //クリックした所にミサイルを生成
        if(Input.GetMouseButtonDown(0)){
            this.GenerateMissile();
        }
        //隕石タイマー更新
        UpdateMeteorTimer();
        //アイテムタイマーの更新
        UpdateItemTimer();

        // 現在の体力が無くなったら
        if(life_ <= 0){

            //シーンを次のシーンに移行する
            SceneManager.LoadScene(nextScene);
        }
    }

    #region 各オブジェクトの生成

    // 隕石
    private void GenerateMeteor()
    {

        int max = spawnPosition_.Count;
        int posIndex = Random.Range(0, max);

        Vector3 spawnPosition = spawnPosition_[posIndex].position;
        Meteor meteor =
        Instantiate(meteorPrefab_,
        spawnPosition, Quaternion.identity);
        meteor.SetUp(ground_, this, explosionPrefab_);
    }

    // ミサイル
    private void GenerateMissile()
    {
        //クリックしたスクリーン座標を取得し、ワールド座標に変換する
        Vector3 clickPosition =
        mainCamera_.ScreenToWorldPoint(Input.mousePosition);
        clickPosition.z = 0;

        Transform closestPosition = null;
        float closestDistance = float.MaxValue;

        foreach(Transform missilePos in missilePositions_){
            float distance = Vector3.Distance(clickPosition,missilePos.position);

            if(distance < closestDistance){
                closestDistance = distance;
                closestPosition = missilePos;
            }
        }

        if(closestPosition != null){
            GameObject reticle = Instantiate(reticlePrefab_, clickPosition, Quaternion.identity);
            Missile missile = Instantiate(missilePrefab, closestPosition.position, Quaternion.identity);
            missile.SetUp(reticle);
        }

    }
    
    // アイテム
    private ItemBase PickUpItem(){
        //アイテム数の取得と、アイテムが一つ以上登録されていることを確認する
        int itemPrefabNum = items_.Count;
        Assert.IsTrue(itemPrefabNum > 0);

        // ランダムでインデックスを取得し、そのインデックスのアイテムを返す
        int pickedupIndex = Random.Range(0,itemPrefabNum);
        ItemBase pickedupItem = items_[pickedupIndex];
        return pickedupItem;
    }

    #endregion

    #region 各オブジェクトの更新

    /// スコアの加算
    public void AddScore(int point)
    {
        score_ += point;
        scoreText_.SetScore(score_);
    }

    /// ライフをえらす
    public void Damange(int point)
    {
        life_ -= point;
        // UIの更新
        UpdateLifeBar();
    }

    /// 隕石タイマーの更新
    private void UpdateMeteorTimer()
    {
        meteorTimer_ -= Time.deltaTime;
        if (meteorTimer_ > 0) { return; }
        meteorTimer_ += meteorInteval_;
        GenerateMeteor();
    }



    /// ライフの初期化
    private void ResetLeft()
    {
        life_ = maxLife_;
        //UIの更新
        UpdateLifeBar();
    }

    /// ライフUIの更新
    private void UpdateLifeBar()
    {
        // 最大体力と現在体力の割合で何割かを算出する
        float lifeRatio = Mathf.Clamp01(life_ / maxLife_);
        // 割合をlifeBar_へ伝え、UIに反映してもらう
        lifeBar_.SetGuageRatio(lifeRatio);
    }

    private void UpdateItemTimer()
    {
        //タイマーを減らし、まだ残っていらた早期リターン
        itemTimer_ -= Time.deltaTime;
        if(itemTimer_ > 0) { return; }

        itemTimer_ += itemSpawnInterval_;
        //タイマーを減算させ、まだ残っていたら早期リターン
        ItemBase pickUpItem = PickUpItem();
        Instantiate(pickUpItem,
        itemSpwanPoint_.position,
        Quaternion.identity);  
    }

    public static int GetScore() {  return score_; }

    #endregion

}