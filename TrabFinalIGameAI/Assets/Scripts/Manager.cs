using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Manager : MonoBehaviour {

    public GameObject VictoryCanvas;
    public GameObject GameOverCanvas;

    private void Awake()
    {
        Time.timeScale = 1;
    }

    public void Victory()
    {
        VictoryCanvas.SetActive(true);
    }

    public void GameOver()
    {
        Time.timeScale = 0;
        GameOverCanvas.SetActive(true);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
