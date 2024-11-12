using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(TMP_Text))]
public class TimerText : MonoBehaviour
{
    private TMP_Text timerText;
    private float remainingTimer = 60;

    [SerializeField]private string nextScene;

    void Start()
    {
        timerText = GetComponent<TMP_Text>();
        timerText.text = remainingTimer.ToString("0");
    }

    void Update()
    {
        if(remainingTimer > 0){
            remainingTimer -= Time.deltaTime;
            timerText.text = Mathf.Ceil(remainingTimer).ToString("0");
        } else {
            SceneManager.LoadScene(nextScene);
        }
    }
}
