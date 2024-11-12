using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class ScoreEffect : MonoBehaviour
{
    //上昇速度
    [SerializeField]
    float upSpeed_ = 1; 

    //消滅までの時間
    [SerializeField]
    float aliveTime_ = 1;

    //カウンター
    float alivedTimer_ = 0;

    void Start()
    {
        
    }

    void Update()
    {
        alivedTimer_ += Time.deltaTime;
        if(alivedTimer_ > aliveTime_){Destroy(gameObject);}
        //上方向へ上昇
        transform.Translate(Vector3.up * upSpeed_ * Time.deltaTime);
    }

    //表示する数字の書き換え
    public void SetScore(int score){
        //数字は何度も書き換えないので、GetComponentして
        //そのまま書き換え、変数にも記録しない
        GetComponent<TMP_Text>().text = 
        score.ToString();
    }
}
