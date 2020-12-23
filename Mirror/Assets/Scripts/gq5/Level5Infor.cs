using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level5Infor : MonoBehaviour
{
    // Start is called before the first frame update
    static public bool templeflipbreakflag;
    static public bool templebreakflag;
    static public bool succeedflag;
    static public bool playercometotempleflag;

    static public GameObject camera1;
    static public GameObject camera2;
    static public GameObject camera3;

    static public GameObject tipdialog;
    static public GameObject succeeddialog;

    public GameObject pausePanel;

    void ShutDownDialog()
    {
        tipdialog.SetActive(false);
    }

    void Start()
    {
        templeflipbreakflag = false;
        templebreakflag = false;
        succeedflag = false;

        camera1 = GameObject.FindGameObjectWithTag("level5camera1");
        camera2 = GameObject.FindGameObjectWithTag("level5camera2");
        camera3 = GameObject.FindGameObjectWithTag("level5camera3");

        
        succeeddialog= GameObject.FindGameObjectWithTag("level5succeeddialog");

        pausePanel = GameObject.Find("Canvas_Success");
        pausePanel.SetActive(false);

        //tipdialog = GameObject.FindGameObjectWithTag("level5tipdialog");
        //tipdialog.SetActive(true);
        //Invoke("ShutDownDialog", 6f);

        succeeddialog.SetActive(false);

        camera1.SetActive(true);
        camera2.SetActive(true);
        camera3.SetActive(false); ;

    }

    // Update is called once per frame
    void Update()
    {
        Succeed();
    }

    void Succeed()
    {
        if(succeedflag)
        {
            camera1.SetActive(false);
            camera2.SetActive(false);
            camera3.SetActive(true);
            succeedflag = false;
            //succeeddialog.SetActive(true);
            //最后一关
            //大结局
            pausePanel.SetActive(true);
            //GameObject.Find("Canvas_UI/Panel").SetActive(true);
        }
    }
}
