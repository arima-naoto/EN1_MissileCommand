using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClearManeger : MonoBehaviour
{
    [SerializeField]
    private string nextScene;

    [SerializeField, Header("ScoreUISetting")]
    private ResultScoreText scoreText;
    private int score;

    void Start()
    {

    }
    void Update()
    {
        this.SetScore();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SceneManager.LoadScene(nextScene);
        }
    }

    private void SetScore()
    {
        score = GameManeger.GetScore();
        scoreText.SetScore(score);
    }
}
