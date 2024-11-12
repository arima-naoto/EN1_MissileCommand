using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class LifeBar : MonoBehaviour
{
    // Sliderの割合
    private float ratio_;

    //Sliderコンポーネント
    private Slider slider_;
   
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    private void Awake(){

        // Sliderコンポーネントの取得
        slider_ = GetComponent<Slider>();

    }

    // Sliderの割合を設定
    public void SetGuageRatio(float ratio){
        //0～1の範囲で切り詰める
        ratio_ = Mathf.Clamp01(ratio);

        //UIに反映
        slider_.value = ratio_;
    }

}
