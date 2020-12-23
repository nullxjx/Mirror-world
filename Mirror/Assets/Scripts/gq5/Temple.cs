using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Temple : MonoBehaviour
{
    // Start is called before the first frame update
    //static public bool templebreak;
    private GameObject lA;
    private Animator lAani;
    private GameObject lB;
    private Animator lBani;
    private GameObject lC;
    private Animator lCani;
    private GameObject lD;
    private Animator lDani;

    private Animator templeAni;

    //static public bool playercomeflag;

    private PolygonCollider2D templearea;
    void Start()
    {
        lA = GameObject.Find("latternA");
        lAani = lA.GetComponent<Animator>();
        lB = GameObject.Find("latternB");
        lBani = lB.GetComponent<Animator>();
        lC = GameObject.Find("latternC");
        lCani = lC.GetComponent<Animator>();
        lD = GameObject.Find("latternD");
        lDani = lD.GetComponent<Animator>();

        templeAni = gameObject.GetComponent<Animator>();
        templearea = gameObject.GetComponent<PolygonCollider2D>();
        templeAni.SetBool("Break", false);
       // playercomeflag = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Level5Infor.playercometotempleflag)
            Break();
    }

    void Break()
    {
        if (lAani.GetBool("Light") == false
            && lBani.GetBool("Light") == false
            && lCani.GetBool("Light") == false
            && lDani.GetBool("Light") == false && Level5Infor.templeflipbreakflag == true)
        {
            templeAni.SetBool("Break", true);
            //gameObject.GetComponent<Renderer>().enabled = false;
            Level5Infor.templebreakflag = true;
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
