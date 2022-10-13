using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndScreen : MonoBehaviour
{
    // Start is called before the first frame update
    public void exit()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(0);
    }

    public void reset()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(1);
    }
}