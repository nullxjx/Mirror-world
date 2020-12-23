using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoonControl : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rg;
    public bool up;
    //static public bool falldown;
    private bool hasfalled;
    void Start()
    {
        rg = gameObject.GetComponent<Rigidbody2D>();
        rg.gravityScale = 0;
        hasfalled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (hasfalled == false)
        {
            if (up == true && Level5Infor.templebreakflag == true)
            {
                rg.gravityScale = 1;
                hasfalled = true;
            }
            else if (up == false && Level5Infor.templeflipbreakflag == true)
            {
                rg.gravityScale = -1;
                hasfalled = true;
            }
        }

    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if(up)
            Level5Infor.succeedflag = true;
    }
}
