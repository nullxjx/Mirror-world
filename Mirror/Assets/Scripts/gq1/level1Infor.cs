using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class level1Infor : MonoBehaviour
{
    // Start is called before the first frame update

    //static public float lastedx;
    //static public float lastedy;
    //static public float[] xs;
    //static public float[] ys;
    static public GameObject fragment1;
    static public GameObject fragment2;
    static public GameObject fragment3;

    static public bool play1getfragment;
    static public bool play2getfragment;

    static public GameObject db1;
    static public GameObject db2;

    static public GameObject tipdialog;

    void ShutDownDialog()
    {
        tipdialog.SetActive(false);
    }
    void Start()
    {
        fragment1 = GameObject.FindGameObjectWithTag("level1fragment1");
        fragment2 = GameObject.FindGameObjectWithTag("level1fragment2");
        fragment3= GameObject.FindGameObjectWithTag("level1fragment3");
        db1 = GameObject.FindGameObjectWithTag("level1db1");
        db2 = GameObject.FindGameObjectWithTag("level1db2");


        //下面的三行代码不需要了 by XJX
        //tipdialog = GameObject.FindGameObjectWithTag("level1dialog");
        //tipdialog.SetActive(true);
        //Invoke("ShutDownDialog", 4f);


        //xs = new float[4];
        //ys = new float[4];
        //xs[0] = -85F;
        //ys[0] = 3F;
        //xs[1] = F;
        //ys[1] = 14F;
        //xs[2] = 32F;
        //ys[2] = 11F;
        //xs[3] = 32F;
        //ys[3] = 11F;

        play1getfragment = false;
        play2getfragment = false;

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
