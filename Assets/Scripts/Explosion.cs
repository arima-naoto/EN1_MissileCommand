using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    private float maxLifrTime_ = 1;
    private float time_ = 0;
    public int chainNum = 0;

    private Vector3 scaleSpeed = new Vector3(1.0f,1.0f,1.0f);
    private float fadeSpeed = 1.0f;
   
  
    void Start()
    {
        time_ = maxLifrTime_;
    }

    void Update()
    {
        Renderer renderer = GetComponent<Renderer>();
        Color currentColor = renderer.material.color;
        currentColor.a -= fadeSpeed * Time.deltaTime;
        currentColor.a = Mathf.Clamp(currentColor.a, 0, 1);
        renderer.material.color = currentColor;

        //生成されたら、倍率を上げる
        transform.localScale += scaleSpeed * Time.deltaTime;

        time_ -= Time.deltaTime;
        if(time_ > 0) { return; }
        Destroy(gameObject);
    }
}
