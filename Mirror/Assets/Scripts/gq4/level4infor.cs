using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class level4infor : MonoBehaviour
{

    // Start is called before the first frame update



    static public GameObject fragment1;
    static public GameObject fragment2;
    static public GameObject successfragment;
    static public GameObject treebranch;
    static public bool getspiderweb;
    static public bool getstone;
    static public bool cangetstone;
    static public bool gettreebranch;
    static public bool getfisingpole;

    static public bool play1getfragment;
    static public bool play2getfragment;

    static public GameObject npcdialog;
    static public GameObject tipdialog;
    static public Text treebranchtext;
    static public Text spidernettext;

    void ShutDownDialog()
    {
        tipdialog.SetActive(false);
    }

    void Start()
    {

        fragment1 = GameObject.FindGameObjectWithTag("level4fragment1");
        fragment2 = GameObject.FindGameObjectWithTag("level4fragment2");
        successfragment = GameObject.FindGameObjectWithTag("level4fragment3");
        treebranch = GameObject.FindGameObjectWithTag("level4treebranch");
        npcdialog = GameObject.FindGameObjectWithTag("level4dialog");
        tipdialog = GameObject.FindGameObjectWithTag("level4startdialog");

        treebranchtext = GameObject.FindGameObjectWithTag("level4branchtext").GetComponent<Text>();
        spidernettext = GameObject.FindGameObjectWithTag("level4spidernettext").GetComponent<Text>();
        //itemInfordialog = GameObject.FindGameObjectWithTag("level4Iteminfor");
        //itemInfordialog.
        npcdialog.SetActive(false);
        fragment1.SetActive(false);
        fragment2.SetActive(false);
        successfragment.SetActive(false);
        treebranch.SetActive(true);

        getstone = false;
        getspiderweb = false;
        cangetstone = true;
        gettreebranch = false;
        getfisingpole = false;

        play1getfragment = false;
        play2getfragment = false;

        //tipdialog.SetActive(true);
        //Invoke("ShutDownDialog", 4f);
    }

    //static void  ShutDownDialog()
    //{
    //    npcdialog.SetActive(false);
    //}

    // Update is called once per frame
    void Update()
    {
        CheckInfor();
    }

    static void CheckInfor()
    {
        if (getspiderweb == true)
            spidernettext.text = "1";
        else
            spidernettext.text = "0";

        if (gettreebranch == true)
            treebranchtext.text = "1";
        else
            treebranchtext.text = "0";
    }
}
