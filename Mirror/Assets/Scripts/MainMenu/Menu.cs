using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public GameObject aboutMenu;
    public GameObject chapterMenu;

    public void UIEnable() {
        GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
    }

    public void ShowUI() {
        GameObject obj = GameObject.FindGameObjectWithTag("GameMenu");
        Debug.Log(obj);
        obj.SetActive(true);
    }

    public void PlayGame() {
        Time.timeScale = 1;
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        alllevelsInfor.changeInfor(1);
        alllevelsInfor.loadnewScene();
    }

    public void QuitGame() {
        Application.Quit();
    }

    public void ShowChapter() {
        chapterMenu.SetActive(true);
    }

    public void HideChapter()
    {
        chapterMenu.SetActive(false);
    }

    public void ContinueGame() {
        Time.timeScale = 1;
        //string lastChapter = PlayerPrefs.GetString("lastChapter");
        //alllevelsInfor.changeInfor();
        //Application.LoadLevel(lastChapter);
        alllevelsInfor.loadnewScene();
    }

    public void ShowAboutMenu() {
        Time.timeScale = 1;
        aboutMenu.SetActive(true);
        Invoke("HideAboutMenu", 3f);
    }

    public void HideAboutMenu()
    {
        aboutMenu.SetActive(false);
        Debug.Log("hide about");
    }


    public void LoadMoon()
    {
        Time.timeScale = 1;
        alllevelsInfor.changeInfor(4);
        alllevelsInfor.loadnewScene();
        //SceneManager.LoadScene(4);
    }

    public void LoadFlower()
    {
        Time.timeScale = 1;
        alllevelsInfor.changeInfor(2);
        alllevelsInfor.loadnewScene();
        // SceneManager.LoadScene(2);
    }

    public void LoadSnow()
    {
        Time.timeScale = 1;
        alllevelsInfor.changeInfor(3);
        alllevelsInfor.loadnewScene();
        //SceneManager.LoadScene(3);
    }

    public void LoadMirror()
    {
        Time.timeScale = 1;
        alllevelsInfor.changeInfor(1);
        alllevelsInfor.loadnewScene();
        // SceneManager.LoadScene(1);
    }
}
