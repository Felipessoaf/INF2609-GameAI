using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasScript : MonoBehaviour {
    private static CanvasScript instance;

    public GameObject GameOverScreen, VictoryScreen;

    public void Awake()
    {
        if (instance == null) instance = this;
    }

    public static CanvasScript GetInstance()
    {
        return instance;
    }

    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Time.timeScale = 1;
    }

    public void GameOver()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        GameOverScreen.SetActive(true);
    }

    public void Victory()
    {
        Cursor.visible = true;
        Time.timeScale = 0;
        VictoryScreen.SetActive(true);
    }
}
