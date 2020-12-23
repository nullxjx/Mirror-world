using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation : MonoBehaviour
{
    private GameObject playerdialog;
    private GameObject npcdialog;

    // Start is called before the first frame update
    void Start()
    {
        playerdialog = GameObject.FindGameObjectWithTag("level1dialog");
        npcdialog = GameObject.FindGameObjectWithTag("level1npcdialog");
        playerdialog.SetActive(false);
        npcdialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowPlayerDialog()
    {
        playerdialog.SetActive(true);
        Invoke("ShowNPCDialog", 3f);
    }

    void ShowNPCDialog()
    {
        playerdialog.SetActive(false);
        npcdialog.SetActive(true);
        Invoke("HideNPCDialog", 3f);
    }

    void HideNPCDialog()
    {
        npcdialog.SetActive(false);
        Invoke("LoadNextScene", 2f);

    }

    void LoadNextScene() {
        alllevelsInfor.changeInfor();
        alllevelsInfor.loadnewScene();
    }
}
