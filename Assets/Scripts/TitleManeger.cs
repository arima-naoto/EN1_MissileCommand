using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManeger : MonoBehaviour
{
   [SerializeField]
   private string nextSceneName;
    void Start()
    {
        Screen.SetResolution(1280,720,false);
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            SceneManager.LoadScene(nextSceneName);
        }
    }
}
