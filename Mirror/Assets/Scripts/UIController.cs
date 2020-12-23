using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIController : MonoBehaviour
{
    public GameObject pauseMenu;
    
    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0;//游戏停止
    }

    public void ResumeGame()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    public void GotoMainMenu() 
    {

        //Scene scene = SceneManager.GetActiveScene();
        //PlayerPrefs.SetString("lastChapter", scene.name);
        //Debug.Log("lastChapter: " + PlayerPrefs.GetString("lastChapter"));
        SceneManager.LoadScene(0);
    }

    public void QuitGame() {
        Application.Quit();
    }
}
