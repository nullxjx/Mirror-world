using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Templeflip : MonoBehaviour
{
    // Start is called before the first frame update
    //static public bool templeflipbreak;
    private GameObject lAflip;
    private Animator lAflipani;
    private GameObject lBflip;
    private Animator lBflipani;
    private GameObject lCflip;
    private Animator lCflipani;
    private GameObject lDflip;
    private Animator lDflipani;

    private Animator templeAni;


    //static public bool playercomeflag;
    private PolygonCollider2D templearea;
    void Start()
    {
        //templeflipbreak = false;
        lAflip = GameObject.Find("latternAflip");
        lAflipani = lAflip.GetComponent<Animator>();
        lBflip = GameObject.Find("latternBflip");
        lBflipani = lBflip.GetComponent<Animator>();
        lCflip = GameObject.Find("latternCflip");
        lCflipani = lCflip.GetComponent<Animator>();
        lDflip = GameObject.Find("latternDflip");
        lDflipani = lDflip.GetComponent<Animator>();

        templeAni = gameObject.GetComponent<Animator>();

        templearea = gameObject.GetComponent<PolygonCollider2D>();
        //templeflipbreak = false;
        templeAni.SetBool("Break", false);



    }

    // Update is called once per frame
    void Update()
    {
        if(Level5Infor.playercometotempleflag)
            Break();
    }

    void Break()
    {
        if (lAflipani.GetBool("Light") == false
            && lBflipani.GetBool("Light") == false
            && lCflipani.GetBool("Light") == false
            && lDflipani.GetBool("Light") == false)
        {
            templeAni.SetBool("Break", true);
            Level5Infor.templeflipbreakflag = true;
            //gameObject.GetComponent<Renderer>().enabled = false;
            //templeflipbreak = true;
        }


    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (templearea.IsTouchingLayers(LayerMask.GetMask("Player")))
         {
            Level5Infor.playercometotempleflag = true;
        }

    }
}
