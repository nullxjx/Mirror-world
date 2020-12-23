using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimation3 : MonoBehaviour
{
    private GameObject playerdialog;

    // Start is called before the first frame update
    void Start()
    {
        playerdialog = GameObject.FindGameObjectWithTag("level3tipdialog");
        playerdialog.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void ShowDialog()
    {
        playerdialog.SetActive(true);
        Invoke("HideDialog", 3f);
    }

    void HideDialog()
    {
        playerdialog.SetActive(false);
        Invoke("LoadNextScene", 2f);

    }

    void LoadNextScene()
    {
        alllevelsInfor.changeInfor();
        alllevelsInfor.loadnewScene();
    }
}
