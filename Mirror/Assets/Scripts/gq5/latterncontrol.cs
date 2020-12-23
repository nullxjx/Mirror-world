using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class latterncontrol : MonoBehaviour
{
    // Start is called before the first frame update
    private Animator myAnim;
    private BoxCollider2D ltcollider;

    private bool colliderPlayer;
   
    public bool initialstate;
    public bool moveflag;
    public float moveUnit;
    public int moveleftd;
    public int moverightd;

    public bool ABCDflag;
    private bool directionleft;
    public int movespeed;
    private Vector3 StartPosition;
    private Vector3 EndPositionleft;
    private Vector3 EndPositionright;
    private bool firstMove;

    

    void Start()
    {
        myAnim = gameObject.GetComponent<Animator>();
        ltcollider = gameObject.GetComponent<BoxCollider2D>();

        myAnim.SetBool("Light", initialstate);
        StartPosition = transform.position;
        EndPositionleft = StartPosition;
        EndPositionright = StartPosition;
        EndPositionleft.x = StartPosition.x- moveleftd*moveUnit;
        EndPositionright.x= StartPosition.x + moverightd*moveUnit;
        directionleft = true;
       // firstMove = true;
       // movespeed = 1;
        //if (moveflag)
        //    latternmove();

    }

    // Update is called once per frame
    void Update()
    {
        // latternmove();
        //if (firstMove)
        //{
        //    latternmove();
        //    firstMove = false;
        // }

        checkDirection();
    }
    void checkDirection()
    {
        if (moveflag)
        {
            latternmove();
            //latternmove();
            if (directionleft == true)
            {
                if (transform.position.x <= EndPositionleft.x+0.5)
                {
                    directionleft = false;
                    //latternmove();
                }
            }
            else
            {
            if (transform.position.x >= EndPositionright.x-0.5)
                {
                    directionleft = true;
                   // latternmove();
                }
            }
        }
    }
    void latternmove()
    {
        float step = movespeed * Time.deltaTime;
        if (directionleft)
            transform.position = Vector3.MoveTowards(transform.position, EndPositionleft, step);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPositionright, step);
        }
    }
    void CheckPlayer()
    {
        colliderPlayer = ltcollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        //Debug.Log(isGround);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        CheckPlayer();
        if(colliderPlayer)
        {
            if(myAnim.GetBool("Light"))
            {
                myAnim.SetBool("Light", false);
            }
            else
            {
                myAnim.SetBool("Light", true);
            }
            if (ABCDflag)
            {
                if (moveflag == true)
                    moveflag = false;
                else
                    moveflag = true;
            }
        }



    }
}
