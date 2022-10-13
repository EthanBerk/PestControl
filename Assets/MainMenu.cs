using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public Text highScore;


    private void Start()
    {
        Time.timeScale = 1;
        if (!PlayerPrefs.HasKey("High"))
        {
            PlayerPrefs.SetInt("High", 0);
        }

        highScore.text = "HI: " + PlayerPrefs.GetInt("High");
    }

    // Start is called before the first frame update
    public void exit()
    {
        Application.Quit();
    }

    public void StartButtun()
    {
        SceneManager.LoadScene(1);
    }
}