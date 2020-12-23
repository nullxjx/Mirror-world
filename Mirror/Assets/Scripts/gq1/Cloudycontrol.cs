using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cloudycontrol : MonoBehaviour
{
    // Start is called before the first frame update
    public int moveUnit;
    public int movelength;
    public int movespeed;

    public bool moveflag;
    private bool directionup;

    private BoxCollider2D cloudycollider;

    private bool colliderPlayer;

    private Vector3 StartPosition;
    private Vector3 EndPositiondown;
    private Vector3 EndPositionup;


    private GameObject nextCloudyObject;
    private Animator nextCloudyObjecanim;
    public string nextCloudyindex;
    public bool showflag;
    public bool firstdisabled;


    public bool followflag;
    public string followcouldyname;
    private GameObject followCloudy;
    private Transform followCloudytransform;
    void Start()
    {
        if (firstdisabled)
        {
            gameObject.GetComponent<Renderer>().enabled = false;
            //return;
        }
        //else
        //    gameObject.SetActive(true);
        //moveflag = false;
        //canmove = false;
        cloudycollider = gameObject.GetComponent<BoxCollider2D>();
        StartPosition = transform.position;
        EndPositiondown = StartPosition;
        EndPositionup = StartPosition;
        EndPositiondown.y = StartPosition.y - movelength * moveUnit;
        EndPositionup.y = StartPosition.y + movelength * moveUnit;
        directionup = true;
        if (showflag)
        {
            nextCloudyObject = GameObject.FindGameObjectWithTag("showcloudy" + nextCloudyindex);
            nextCloudyObjecanim = nextCloudyObject.GetComponent<Animator>();
        }

        if(followflag)
        {
            followCloudy = GameObject.FindGameObjectWithTag(followcouldyname);
            followCloudytransform = followCloudy.GetComponent<Transform>();
        }


    }

    // Update is called once per frame
    void Update()
    {
        CheckDirection();
    }



    void CheckDirection()
    {
        if(moveflag)
        {
            cloudymove();
            if(directionup==true)
            {
                if (transform.position.y >= EndPositionup.y-1)
                    directionup = false;
            }
            else
            {
                if (transform.position.y <= EndPositiondown.y+1)
                    directionup = true;
            }
        }

        if(followflag)
        {
            transform.position = new Vector3(followCloudytransform.position.x, -followCloudytransform.position.y, followCloudytransform.position.z);
        }
    }

    void cloudymove()
    {
        float step = movespeed * Time.deltaTime;
        if (directionup)
            transform.position = Vector3.MoveTowards(transform.position, EndPositionup, step);
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, EndPositiondown, step);
        }
    }
    void CheckPlayer()
    {
        //colliderPlayer = cloudycollider.IsTouchingLayers(LayerMask.GetMask("Player"));
        //Debug.Log(isGround);
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (showflag)
        {
            nextCloudyObject.GetComponent<Renderer>().enabled = true;
            nextCloudyObjecanim.SetBool("Show", true);
        }
    }
}
