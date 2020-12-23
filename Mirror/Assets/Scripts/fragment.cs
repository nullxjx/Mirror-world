using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fragment : MonoBehaviour
{
    // Start is called before the first frame update

    public bool successflag;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        //if (collision.gameObject.tag == "player1")
        //{
            
        //    //playercontrol.getfragment();
        //}
        //else if (collision.gameObject.tag == "player2")
        //{
        //    //flipplayercontrol.getfragment();
        //}
        if(successflag)
        {
            //alllevelsInfor.currentLevel += 1;
            if (alllevelsInfor.currentLevel != 2)
            {
                Debug.Log("get to npc");
                GameObject.Find("Camera_main").GetComponent<Animator>().SetBool("Start", true);

            }
            //else {
            //    alllevelsInfor.changeInfor();
            //    alllevelsInfor.loadnewScene();
            //}
        }
        //gameObject.SetActive(false);
    }
}
