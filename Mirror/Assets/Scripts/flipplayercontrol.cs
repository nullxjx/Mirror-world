using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class flipplayercontrol : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D m_rg;
    private Animator myAnim;
    private CapsuleCollider2D my_feet;

    private GameObject player1object;
    private Transform player1transform;
    private Animator player1Anim;
    private Rigidbody2D player1rg;


    public int MoveSpeed;
    public int JumpSpeed;
    public int Safeheight;

    private int ClimbSpeed;

    private float playerGravity;



    private bool isGround;
    private bool isLadder;

    private bool isClimbing;
    private bool isJumping;
    private bool isFalling;


    //private float lastHeight;
    private float currentHeight;
    private float dcurrentHeight;


    static public bool dieflag;
    //static public bool getmirrorgragment;
    static public bool mirror;
    static public float lastHeight;

    //private bool getstone;


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

        player1object = GameObject.Find("player1");
        player1transform = player1object.GetComponent<Transform>();
        player1Anim = player1object.GetComponent<Animator>();
        player1rg = player1object.GetComponent<Rigidbody2D>();

        

        myAnim.SetBool("Idle", true);
        playerGravity = m_rg.gravityScale;
        mirror = true;

        lastHeight = transform.position.y;


        dieflag = false;
        //getmirrorgragment = false;
        ClimbSpeed = 5;

        //getstone = false;

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
        //restart();
        //mirrorchange();
        if (mirror == false)
        {
            //mirrorchange();
            Run();
            //Flip();
            Jump();
            Climb();
            CheckGrounded();
            SwithAnimation();
            CheckLadder();
            GetStone();
            ThrowStone();
            Die();
        }
        else
        {
            myAnim.SetBool("Idle", player1Anim.GetBool("Idle"));
            myAnim.SetBool("Run", player1Anim.GetBool("Run"));
            myAnim.SetBool("Jump", player1Anim.GetBool("Jump"));
            myAnim.SetBool("Fall", player1Anim.GetBool("Fall"));
            myAnim.SetBool("Climbing", player1Anim.GetBool("Climbing"));
            myAnim.SetBool("GetStone", player1Anim.GetBool("GetStone"));
            myAnim.SetBool("GetFishpole", player1Anim.GetBool("GetFishpole"));
            myAnim.SetBool("Throwhook", player1Anim.GetBool("Throwhook"));
            m_rg.velocity = player1rg.velocity;
            //transform.position = new Vector3(player1transform.position.x, -player1transform.position.y-(float)(7.8), player1transform.position.z);
            transform.position = new Vector3(player1transform.position.x, -player1transform.position.y, player1transform.position.z);

        }
        Flip();
        //Die();

    }

    //void mirrorchange()
    //{
    //    if (Input.GetButtonDown("Changemirror") && playercontrol.hide == false)
    //    {
    //        if (mirror == true)
    //            mirror = false;
    //        else
    //            mirror = true;
    //    }
    //}

    void CheckLadder()
    {
        isLadder = my_feet.IsTouchingLayers(LayerMask.GetMask("Ladder"));
    }
    void Climb()
    {
        float moveY = Input.GetAxis("Vertical");
        if (isClimbing)
        {
            m_rg.velocity = new Vector2(m_rg.velocity.x, -moveY * ClimbSpeed);
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

                m_rg.velocity = new Vector2(m_rg.velocity.x, -moveY * ClimbSpeed);
                m_rg.gravityScale = 0.0f;
                lastHeight = transform.position.y;
                playercontrol.lastHeight = player1transform.position.y;
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
        if (isLadder && isGround)
            m_rg.gravityScale = playerGravity;
    }
    void Die()
    {
        //if (dieflag)
        //{
            
        //    playercontrol.dieflag = true;
        //}
         if(dieflag)
        {
            //alllevelsInfor.changeInfor();
            for (int i = 3; i >= 0; i--)
            {
                if (transform.position.x>= alllevelsInfor.xs[i])
                {
                    transform.position = new Vector3(alllevelsInfor.xs[i], -alllevelsInfor.ys[i], transform.position.z);
                    dieflag = false;
                    flipplayercontrol.dieflag = false;
                    break;
                }
            }

         
        }

    }
    void Run()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            if (!music.isPlaying && isGround)
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
            if (!music.isPlaying && isGround)
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
                m_rg.velocity = Vector2.down * jumpVel;
                transform.SetParent(null, true);
            }
            //else
            //{
            //    myAnim.SetBool("Jump", false);
            //}
        }
    }
    void GetStone()
    {
        if (!level4infor.getstone)
        {
            if (Input.GetButtonDown("communicateStone") && my_feet.IsTouchingLayers(LayerMask.GetMask("SmallStone")))
            {
                if (!level4infor.getstone)
                {
                    level4infor.getstone = true;
                    myAnim.SetBool("GetStone", true);
                }
            }
        }
    }

    void ThrowStone()
    {
        if (level4infor.getstone)
        {
            if (Input.GetButtonDown("communicateStone") && my_feet.IsTouchingLayers(LayerMask.GetMask("level4IceRiver")))
            {
                level4infor.getstone = false;
                myAnim.SetBool("GetStone", false);
                IceRiver.icebreakflag = true;
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
            Debug.Log(m_rg.velocity.y);
            if (m_rg.velocity.y > 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
                lastHeight = transform.position.y;
            }
        }
        else if (isGround || isLadder)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {

        if (!mirror)
        {
            currentHeight = transform.position.y;
            dcurrentHeight = player1transform.position.y;

            playercontrol.lastHeight = dcurrentHeight;


            if (-lastHeight + currentHeight > Safeheight)
                dieflag = true;
            else
                lastHeight = currentHeight;


            //river
            if (my_feet.IsTouchingLayers(LayerMask.GetMask("River")) || my_feet.IsTouchingLayers(LayerMask.GetMask("Monster")))
                dieflag = true;

            //Level1
            if (collision.gameObject == level1Infor.fragment2)
            {
                music.clip = getfragmentsound;
                music.Play();
                level1Infor.play2getfragment = true;
                level1Infor.fragment2.SetActive(false);
            }
            //if (my_feet.IsTouchingLayers(LayerMask.GetMask("Mirror")) && level1Infor.play2getfragment == true)
            if (level1Infor.play2getfragment == true)
            {

                player1transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
                playercontrol.lastHeight = -transform.position.y;
                playercontrol.hide = false;
                playercontrol.mirror = false;

                mirror = true;
                player1object.SetActive(true);
                level1Infor.play2getfragment = false;

            }

            //Level5
            if (my_feet.IsTouchingLayers(LayerMask.GetMask("Lattern")))
                transform.SetParent(collision.gameObject.transform);

            //Level4
            if (collision.gameObject == level4infor.fragment2)
            {
                music.clip = getfragmentsound;
                music.Play();
                level4infor.play2getfragment = true;
                level4infor.fragment2.SetActive(false);
            }
            //if (my_feet.IsTouchingLayers(LayerMask.GetMask("Mirror")) && level4infor.play2getfragment == true)
            if (level4infor.play2getfragment == true)
            {
                player1transform.position = new Vector3(transform.position.x, -transform.position.y, transform.position.z);
                playercontrol.lastHeight = -transform.position.y;
                playercontrol.hide = false;
                playercontrol.mirror = false;

                mirror = true;
                player1object.SetActive(true);
                level4infor.play2getfragment = false;

            }

            if (my_feet.IsTouchingLayers(LayerMask.GetMask("level4spiderweb")) && level4infor.getspiderweb == false)
            {
                //
                level4infor.getspiderweb = true;

                level4infor.fragment2.SetActive(true);

            }
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
    //    getmirrorgragment = true;
    //}
}

