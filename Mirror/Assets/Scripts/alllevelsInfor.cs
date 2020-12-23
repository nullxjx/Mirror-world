
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//存放全局关卡信息
public class alllevelsInfor: MonoBehaviour
{
    static public int currentLevel=0;

    static public float[] xs=new float[4];
    static public float[] ys= new float[4];

    //static public AudioSource music;
    //static public AudioClip jump;

    void Start()
    {
        changeInfor();
        //loadResources();
    }
    static public void loadnewScene()
    {
        //加载新场景
        //string scene = "unity" + currentLevel;
        //Application.LoadLevel(scene);//
          
        SceneManager.LoadScene(currentLevel);
    }


    static public void changeInfor(int sceneindex=-1)
    {
        if (sceneindex != -1)
            currentLevel = sceneindex;
        else
            currentLevel += 1;
        if (currentLevel==1)
        {
            //镜关
            xs[0] = -85F;
            ys[0] = 11F;
            xs[1] = -65F;
            ys[1] =28F;
            xs[2] = -35F;
            ys[2] = 12F;
            xs[3] = 12F;
            ys[3] = 26F;
        }
        else if(currentLevel==2)
        {
            //花关
            xs[0] = -5F;
            ys[0] = -4F;
            xs[1] = -5F;
            ys[1] = -4F;
            xs[2] = -5F;
            ys[2] = -4F;
            xs[3] = 15F;
            ys[3] = -4F;
            //雪关
        }
        else if(currentLevel==3)
        {
            //雪关

        }
        else if (currentLevel == 4)
        {
            //月关
            xs[0] = -14F;
            ys[0] = 20F;
            xs[1] = 8.5F;
            ys[1] = 16F;
            xs[2] = 16F;
            ys[2] = 16F;
            xs[3] = 31F;
            ys[3] = 14F;
        }
    }
    
    public alllevelsInfor()
    {

    }

}
