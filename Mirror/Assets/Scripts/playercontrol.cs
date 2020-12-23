using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class playercontrol : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D m_rg;
    private Animator myAnim;
    private CapsuleCollider2D my_feet;

    public int MoveSpeed;
    public int JumpSpeed;
    public int Safeheight;

    private int ClimbSpeed;

    private float playerGravity;

    private GameObject player2object;
    private Transform player2transform;
    private Animator player2Anim;
    private Rigidbody2D player2rg;

    private bool isGround;
    private bool isLadder;

    private bool isClimbing;
    private bool isJumping;
    private bool isFalling;



    //private float lastHeight;
    private float currentHeight;
    private float dcurrentHeight;
    private float movemaxx;

    static public bool dieflag;


    //static public bool getmirrorgragment;
    static public bool hide;
    static public bool mirror;
    static public float lastHeight;

    //private bool getstone;

    // private bool getinriver;



    private AudioSource music;
    private AudioClip jumpsound;
    private AudioClip runsound;
    private AudioClip climbsound;
    private AudioClip getfragmentsound;
    private AudioClip throwstonesound;
    private AudioClip getstonesound;


    void Start()
    {
        m_rg = gameObject.GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
        my_feet = gameObject.GetComponent<CapsuleCollider2D>();
        myAnim.SetBool("Idle", true);
        playerGravity = m_rg.gravityScale;

        mirror = false;
        player2object = GameObject.Find("player2");
        player2transform = player2object.GetComponent<Transform>();
        player2Anim = player2object.GetComponent<Animator>();
        player2rg = player2object.GetComponent<Rigidbody2D>(); 


        lastHeight = transform.position.y;
        dieflag = false;
        hide = false;
        //getmirrorgragment = false;


        ClimbSpeed = 5;
        movemaxx = transform.position.x;
        Debug.Log("movemaxx:"+movemaxx);


   
        InitialSounds();




    }
    void InitialSounds()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.playOnAwake = false;
        jumpsound = Resources.Load<AudioClip>("music/jump");
        runsound = Resources.Load<AudioClip>("music/run");
        climbsound = Resources.Load<AudioClip>("music/climb");
        getfragmentsound = Resources.Load<AudioClip>("music/fragment");
        throwstonesound = Resources.Load<AudioClip>("music/throwstone");
        getstonesound = Resources.Load<AudioClip>("music/getstone");
    }
    // Update is called once per frame
    void Update()
    {

        if (hide == false)
        {
            restart();
            //mirrorchange();
            if (mirror == false)
            {
                CheckAirStatus();
                Run();
                Jump();
                Climb();
                CheckGrounded();
                CheckLadder();
                SwithAnimation();
                GetStone();
                ThrowStone();
                GetFishingPole();
                ThrowHook();
                Die();

                //Getmirrorfragment();
            }
            else
            {
                myAnim.SetBool("Idle", player2Anim.GetBool("Idle"));
                myAnim.SetBool("Run", player2Anim.GetBool("Run"));
                myAnim.SetBool("Jump", player2Anim.GetBool("Jump"));
                myAnim.SetBool("Fall", player2Anim.GetBool("Fall"));
                myAnim.SetBool("Climbing", player2Anim.GetBool("Climbing"));
                myAnim.SetBool("GetStone", player2Anim.GetBool("GetStone"));
                m_rg.velocity = player2rg.velocity;
                //transform.position = new Vector3(player2transform.position.x, -player2transform.position.y - (float)(7.8), player2transform.position.z);
                transform.position = new Vector3(player2transform.position.x, -player2transform.position.y , player2transform.position.z);
            }

            Flip();
            MoveMaxX();
        }
        restart();
       


    }
    void MoveMaxX()
    {
        if (transform.position.x > movemaxx)
            movemaxx = transform.position.x;
    }
    //void mirrorchange()
    //{
    //    if (Input.GetButtonDown("Changemirror"))
    //    {
    //        if (mirror == true)
    //            mirror = false;
    //        else
    //            mirror = true;
    //    }
    //}
    void restart()
    {
        if (Input.GetButtonDown("restart"))
            SceneManager.LoadScene(0);

    }
    void Die()
    {
        if(dieflag)
        {
           // alllevelsInfor.changeInfor();
            for (int i = 3; i >= 0; i--)
            {
                if (movemaxx >= alllevelsInfor.xs[i])
                {
                    transform.position = new Vector3(alllevelsInfor.xs[i], alllevelsInfor.ys[i], transform.position.z);
                    dieflag = false;
                    flipplayercontrol.dieflag = false;
                    break;
                }
            }

         
        }

    }

    void CheckLadder()
    {
        isLadder = my_feet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
    void Climb()
    {
        float moveY= Input.GetAxis("Vertical");
        if(isClimbing)
        {
            m_rg.velocity = new Vector2(m_rg.velocity.x, moveY * ClimbSpeed);
        }
        if (isLadder)
        {
            if (moveY > 0.5f || moveY < -0.5f)
            {
                if (!music.isPlaying)
                {
                    music.clip = climbsound;
                    music.Play();
                }
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Run", false);
                myAnim.SetBool("Climbing", true);

                m_rg.velocity = new Vector2(m_rg.velocity.x, moveY * ClimbSpeed);
                m_rg.gravityScale = 0.0f;
                lastHeight = transform.position.y;
                flipplayercontrol.lastHeight = player2transform.position.y;
            }
            else
            {
                if (isJumping || isFalling)
                    myAnim.SetBool("Climbing", false);
                else
                {
                    myAnim.SetBool("Climbing", false);
                    m_rg.velocity = new Vector2(m_rg.velocity.x, 0.0f);
                }
            }
        }
        else
        {
            myAnim.SetBool("Climbing", false);
            m_rg.gravityScale = playerGravity;

        }
        if(isLadder&& isGround)
            m_rg.gravityScale = playerGravity;
      
    }
    void Run()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (!music.isPlaying&& isGround)
            {
                music.clip = runsound;
                music.Play();
            }
            m_rg.velocity = new Vector2(MoveSpeed, m_rg.velocity.y);
            bool playHasXAxisSpeed = Mathf.Abs(m_rg.velocity.x) > Mathf.Epsilon;
            myAnim.SetBool("Run", playHasXAxisSpeed);
            myAnim.SetBool("Idle", false);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
            if (!music.isPlaying&& isGround)
            {
                music.clip = runsound;
                music.Play();
            }
            // Anim.CrossFade("playerrun", 0.1f, -1, 0);
            m_rg.velocity = new Vector2(-MoveSpeed, m_rg.velocity.y);
            bool playHasXAxisSpeed = Mathf.Abs(m_rg.velocity.x) > Mathf.Epsilon;
            myAnim.SetBool("Run", playHasXAxisSpeed);
            myAnim.SetBool("Idle", false);
        }
        else
        {
            myAnim.SetBool("Run", false);
            myAnim.SetBool("Idle", true);
            // Anim.CrossFade("palyeridle", 0.1f, -1, 0);
            m_rg.velocity = new Vector2(0, m_rg.velocity.y);
        }
    }

    void Flip()
    {
        bool playHasXAxisSpeed = Mathf.Abs(m_rg.velocity.x) > Mathf.Epsilon;
        if (playHasXAxisSpeed)
        {
            if (m_rg.velocity.x > 0.1f)
                transform.localRotation = Quaternion.Euler(0, 0, 0);
            if (m_rg.velocity.x < -0.1f)
                transform.localRotation = Quaternion.Euler(0, 180, 0);
        }
    }
    void CheckGrounded()
    {
        isGround = my_feet.IsTouchingLayers(LayerMask.GetMask("Mirror"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Road"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Lattern"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Cloudy"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Stone"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Mountain"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("level4IceRiver"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("SmallStone"));
        //Debug.Log(isGround);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround||isLadder)
            {
                music.clip = jumpsound;
                music.Play();
                myAnim.SetBool("Jump", true);
                myAnim.SetBool("Idle", false);
                Vector2 jumpVel = new Vector2(m_rg.velocity.x, JumpSpeed);
                m_rg.velocity = Vector2.up * jumpVel;

                transform.SetParent(null, true);

            }
        }
    }

    void GetStone()
    {
        if (level4infor.cangetstone&&level4infor.getstone==false)
        {
            if (Input.GetButtonDown("communicateStone") &&my_feet.IsTouchingLayers(LayerMask.GetMask("SmallStone")))
            {

                music.clip = getstonesound;
                music.Play();
                level4infor.getstone = true;
                myAnim.SetBool("GetStone", true);
            }
        }
    }

    void ThrowStone()
    {
        if(level4infor.getstone)
        {
            if(Input.GetButtonDown("communicateStone") &&my_feet.IsTouchingLayers(LayerMask.GetMask("level4IceRiver")))
            {
                music.clip = throwstonesound;
                music.Play();
                level4infor.getstone = false;
                myAnim.SetBool("GetStone", false);
                IceRiver.icebreakflag = true;
                level4infor.fragment1.SetActive(true);
                level4infor.cangetstone = false;
            }
        }
    }


    void ShutDownDialog()
    {
        level4infor.npcdialog.SetActive(false);
    }
    void GetFishingPole()
    {
        if (Input.GetButtonDown("communicateStone") && my_feet.IsTouchingLayers(LayerMask.GetMask("level4npc")))
        {
            level4infor.npcdialog.SetActive(true);
            Invoke("ShutDownDialog", 3f);
            if (level4infor.getfisingpole == false && level4infor.getspiderweb == true && level4infor.gettreebranch == true)
            {
                level4infor.getfisingpole = true;
                level4infor.getspiderweb = false;
                level4infor.gettreebranch = false;

                myAnim.SetBool("GetFishpole", true);         
            }
        }
    }

    void ThrowHook()
    {
        if(level4infor.getfisingpole&& my_feet.IsTouchingLayers(LayerMask.GetMask("level4IceRiver")))
        {
            if (Input.GetButtonDown("communicateStone"))
            {
                myAnim.SetBool("Throwhook", true);
                level4infor.successfragment.SetActive(true);
                myAnim.SetBool("GetFishpole", false);
            }
        }
    }
    void SwithAnimation()
    {
        myAnim.SetBool("Idle", false);

        //Debug.Log(myAnim.GetBool("Jump"));
        if (myAnim.GetBool("Jump"))
        {
            // Debug.Log("")
            //Debug.Log(m_rg.velocity.y);
            if (m_rg.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
                lastHeight = transform.position.y;
            }
        }
        else if (isGround|| isLadder)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        currentHeight = transform.position.y;
        dcurrentHeight = player2transform.position.y;

        flipplayercontrol.lastHeight = dcurrentHeight;
        if (lastHeight - currentHeight > Safeheight)
            dieflag = true;
        else 
            lastHeight = currentHeight;
       
        //river
        if(my_feet.IsTouchingLayers(LayerMask.GetMask("River"))|| my_feet.IsTouchingLayers(LayerMask.GetMask("Monster")))
            dieflag = true;


        //level1
        if (collision.gameObject == level1Infor.fragment1)
        {
            music.clip = getfragmentsound;
            music.Play();
            level1Infor.play1getfragment = true;
            level1Infor.fragment1.SetActive(false);
        }
        // if (my_feet.IsTouchingLayers(LayerMask.GetMask("Mirror")) && level1Infor.play1getfragment == true)
        if (level1Infor.play1getfragment == true)
        {

            hide = true;
            gameObject.SetActive(false);

            level1Infor.db1.SetActive(false);
            level1Infor.db2.SetActive(false);
            mirror = true;
            flipplayercontrol.mirror = false;
            level1Infor.play1getfragment = false;

        }

        //level2
        if (my_feet.IsTouchingLayers(LayerMask.GetMask("Lattern")))
            transform.SetParent(collision.gameObject.transform);



        //level4
        if (collision.gameObject == level4infor.fragment1)
        {
            music.clip = getfragmentsound;
            music.Play();
            level4infor.play1getfragment = true;
            level4infor.fragment1.SetActive(false);
        }

        //if (my_feet.IsTouchingLayers(LayerMask.GetMask("level4IceRiver")) && level4infor.play1getfragment == true)
        if (level4infor.play1getfragment == true)
        {
            //getmirrorgragment = true;
            hide = true;
            gameObject.SetActive(false);
            mirror = true;
            flipplayercontrol.mirror = false;
            level4infor.play1getfragment = false;

        }
        if(my_feet.IsTouchingLayers(LayerMask.GetMask("level4treebranch")))
        {
            level4infor.gettreebranch = true;
            level4infor.treebranch.SetActive(false);
        }


 

    }

    void CheckAirStatus()
    {
        isJumping = myAnim.GetBool("Jump");
        isFalling = myAnim.GetBool("Fall");
        isClimbing = myAnim.GetBool("Climbing");
  
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        currentHeight = transform.position.y;
        lastHeight = currentHeight;
    }


    //static public void getfragment()
    //{
    //    //getmirrorgragment = true;
    //}
}