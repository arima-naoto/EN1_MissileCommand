using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(TMP_Text))]
public class ResultScoreText : MonoBehaviour
{

    private int score_;
    private TMP_Text scoreText_;

    void Start()
    {
        score_ = 0;
        scoreText_ = GetComponent<TMP_Text>();
    }

    void Update()
    {

    }

    public void SetScore(int score)
    {
        score_ = score;
        UpdateScoreText();
    }

    private void UpdateScoreText()
    {
        scoreText_.text = $"SCORE :{score_: 00000000}";
    }
}
