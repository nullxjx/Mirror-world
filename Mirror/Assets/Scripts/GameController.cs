using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController instance { get; private set; }

    private void Awake()
    {
        instance = this;
    }

    public void ShowUI()
    {
        GameObject obj = GameObject.FindGameObjectWithTag("GameMenu");
        Debug.Log(obj);
        obj.SetActive(true);

        //GameObject.Find("Canvas/MainMenu/UI").SetActive(true);
    }
}
