using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManeger : MonoBehaviour
{
    [SerializeField]
    private string nextSceneName;

    bool modeFullScreen = true;

    void Start()
    {
    }

    void Update()
    {

        if (Input.GetKeyDown(KeyCode.F5)){
            modeFullScreen ^= !modeFullScreen;
        }

        Screen.SetResolution(1280, 720, modeFullScreen);

        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
