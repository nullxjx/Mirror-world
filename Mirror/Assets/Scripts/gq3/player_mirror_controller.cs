using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class player_mirror_controller : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private BoxCollider2D boxCollider2d;
    private Animator animator;
    private GameObject player;

    private bool isJumping;
    private bool isFalling;
    private bool isGround;//是否落地
    private bool isTrunk;//是否在树干上
    private bool isTree;//是否在树上

    private float movemaxx;

    public float last_height;
    private float safe_height;

    private AudioSource music;
    private AudioClip jumpsound;
    private AudioClip runsound;
    private AudioClip climbsound;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        Vector2 position = GlobalHelper.instance.player_mirror_init_position;
        rigidbody2d.MovePosition(position);
        player = GameObject.Find("player");

        Debug.Log("player_mirror init pos: " + position);

        //初始化时镜子内的人物无法发生碰撞
        transform.GetComponent<BoxCollider2D>().enabled = false;
        SetGravity(0);//初始的重力为0

        last_height = transform.position.y;
        safe_height = 4.5f;//经过场景测试出来
        movemaxx = transform.position.x;
        InitialSounds();
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalHelper.instance.die_flag)
            return;

        if (GlobalHelper.instance.control == 0)
        {
            //跟随左边的人物运动
            float pos_x = 2 * GlobalHelper.instance.mirror_X - player.transform.position.x;
            Vector2 pos = new Vector2(pos_x, player.transform.position.y);
            rigidbody2d.MovePosition(pos);

            string clip = getCurrentClipInfo();
            animator.Play(clip);
        }
        else if (GlobalHelper.instance.control == 1)
        {
            CheckAirStatus();
            Run();
            Jump();

            CheckGrounded();
            SwitchAnimation();

            Flip();
            AdjustBoxCollider();
            MoveMaxX();
        }
    }

    void MoveMaxX()
    {
        if (transform.position.x > movemaxx)
            movemaxx = transform.position.x;
    }
    void InitialSounds()
    {
        music = gameObject.GetComponent<AudioSource>();
        music.playOnAwake = false;
        jumpsound = Resources.Load<AudioClip>("music/jump");
        runsound = Resources.Load<AudioClip>("music/run");
        climbsound = Resources.Load<AudioClip>("music/climb");
    }

    public void SetGravity(float value)
    {
        rigidbody2d.gravityScale = value;
    }

    public void SetTrunk(bool flag)
    {
        isTrunk = flag;
        last_height = transform.position.y;

        if (isTrunk)
        {
            animator.SetBool("climbIdle", true);
            animator.SetBool("run", false);
            animator.SetBool("jump", false);
            animator.SetBool("fall", false);
        }
        else
        {
            animator.SetBool("idle", true);
            animator.SetBool("climb", false);
            animator.SetBool("climbIdle", false);
        }
    }

    public string getCurrentClipInfo() // 获取当前执行的动画
    {
        AnimatorClipInfo[] m_CurrentClipInfo = player.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        return m_CurrentClipInfo[0].clip.name;
    }

    void Flip()
    {
        bool playHasXAxisSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
        if (playHasXAxisSpeed)
        {
            if (rigidbody2d.velocity.x > 0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (GlobalHelper.instance.control == 1)
                {
                    player.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }

            if (rigidbody2d.velocity.x < -0.1f)
            {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (GlobalHelper.instance.control == 1)
                {
                    player.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }
        }
    }

    void CheckAirStatus()
    {
        isJumping = animator.GetBool("jump");
        isFalling = animator.GetBool("fall");
    }

    void SwitchAnimation()
    {
        animator.SetBool("idle", false);
        if (animator.GetBool("jump"))
        {
            if (rigidbody2d.velocity.y < 0.0f)
            {
                animator.SetBool("jump", false);
                animator.SetBool("fall", true);
            }
        }
        else if (isGround || isTree)
        {
            animator.SetBool("fall", false);
            animator.SetBool("climb", false);
            animator.SetBool("idle", true);
        }
        else if (isTrunk)
        {
            animator.SetBool("jump", false);
            float y_speed = rigidbody2d.velocity.y;
            if (y_speed > 0.5 || y_speed < -0.5)
            {
                animator.SetBool("climb", true);
                animator.SetBool("climbIdle", false);
                if (!music.isPlaying)
                {
                    music.clip = climbsound;
                    music.Play();
                }
            }
            else
            {
                animator.SetBool("climb", false);
                animator.SetBool("climbIdle", true);
            }
        }
    }

    void Run()
    {
        float moveX = Input.GetAxisRaw("Horizontal");

        if (isTrunk)
        {
            if (moveX > 0)
                rigidbody2d.velocity = new Vector2(GlobalHelper.instance.RunSpeed, rigidbody2d.velocity.y);
            else if (moveX < 0)
                rigidbody2d.velocity = new Vector2(-GlobalHelper.instance.RunSpeed, rigidbody2d.velocity.y);
            else
                rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);

            return;
        }

        if (moveX > 0)
        {
            if (!music.isPlaying && (isGround || isTree))
            {
                music.clip = runsound;
                music.Play();
            }
            rigidbody2d.velocity = new Vector2(GlobalHelper.instance.RunSpeed, rigidbody2d.velocity.y);
            bool playHasXAxisSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
            animator.SetBool("run", playHasXAxisSpeed);
            animator.SetBool("idle", false);
        }
        else if (moveX < 0)
        {

            if (!music.isPlaying && (isGround || isTree))
            {
                music.clip = runsound;
                music.Play();
            }
            // Anim.CrossFade("playerrun", 0.1f, -1, 0);
            rigidbody2d.velocity = new Vector2(-GlobalHelper.instance.RunSpeed, rigidbody2d.velocity.y);
            bool playHasXAxisSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
            animator.SetBool("run", playHasXAxisSpeed);
            animator.SetBool("idle", false);
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("idle", true);
            // Anim.CrossFade("palyeridle", 0.1f, -1, 0);
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }

    void CheckGrounded()
    {
        isGround = rigidbody2d.IsTouchingLayers(LayerMask.GetMask("Road"));
        isTree = rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3tree"));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround || isTree)
            {
                music.clip = jumpsound;
                music.Play();
                
                animator.SetBool("jump", true);
                animator.SetBool("idle", false);
                
                Vector2 jumpVel = new Vector2(rigidbody2d.velocity.x, GlobalHelper.instance.JumpSpeed);
                rigidbody2d.velocity = Vector2.up * jumpVel;
                transform.SetParent(null, true);
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        float cur_height = transform.position.y;
        float deltaHeight = last_height - cur_height;
        //Debug.Log("deltaHeight: " + deltaHeight);
        if (deltaHeight > safe_height)
        {
            Die("too high!");
            Debug.Log("die");
        }
        else
        {
            last_height = cur_height;
        }

        if (rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3butterfly")))
        {
            Die("hit butterfly!");
        }

        if (rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3flower")))
        {   
            Win();
        }
    }

    void AdjustBoxCollider()
    {
        if (isTree)
        {
            boxCollider2d.offset = GlobalHelper.instance.rightTree_offset;
            boxCollider2d.size = new Vector2(GlobalHelper.instance.rightTree_size.x, GlobalHelper.instance.rightTree_size.y);
        }
        else {
            float x_speed = rigidbody2d.velocity.x;
            if (x_speed < 0)
            {
                if (x_speed < -0.1f)
                {
                    //往右移动
                    //Debug.Log("move left");
                    boxCollider2d.offset = GlobalHelper.instance.run_left_offset;
                    boxCollider2d.size = new Vector2(GlobalHelper.instance.run_size.x, GlobalHelper.instance.run_size.y);
                }
                else
                {
                    //Debug.Log("left idle");
                    //往左静止
                    boxCollider2d.offset = GlobalHelper.instance.idle_right_offset;
                    boxCollider2d.size = new Vector2(GlobalHelper.instance.idle_size.x, GlobalHelper.instance.idle_size.y);
                }
            }
            else
            {
                if (x_speed > 0.1f)
                {
                    //往左移动
                    //Debug.Log("move right");
                    boxCollider2d.offset = GlobalHelper.instance.run_right_offset;
                    boxCollider2d.size = new Vector2(GlobalHelper.instance.run_size.x, GlobalHelper.instance.run_size.y);
                }
                else
                {
                    //往右静止
                    //Debug.Log("right idle");
                    boxCollider2d.offset = GlobalHelper.instance.idle_left_offset;
                    boxCollider2d.size = new Vector2(GlobalHelper.instance.idle_size.x, GlobalHelper.instance.idle_size.y);
                }
            }
        }
        

        if (isJumping)
        {

        }
    }

    public void Die(string msg)
    {
        //Debug.Log("die: " + msg);
        //Dialog d = GameObject.Find("background").GetComponent<Dialog>();
        //Debug.Log(d.displayTime);
        //d.DisplayDialog("Sorry, you die!", transform.position);

        //GlobalHelper.instance.die_flag = true;
        //alllevelsInfor.changeInfor();
        for (int i = 3; i >= 0; i--)
        {
            if (movemaxx >= alllevelsInfor.xs[i])
            {
                transform.position = new Vector3(alllevelsInfor.xs[i], alllevelsInfor.ys[i], transform.position.z);
                //dieflag = false;
                //flipplayercontrol.dieflag = false;
                break;
            }
        }
    }

    public void Win() 
    {
        //Debug.Log("win!");
        //Dialog d = GameObject.Find("background").GetComponent<Dialog>();
        //Debug.Log(d.displayTime);
        //d.DisplayDialog("Congratulations, you win!", transform.position);

        //alllevelsInfor.currentLevel += 1;
        //alllevelsInfor.changeInfor();
        //alllevelsInfor.loadnewScene();
        //alllevelsInfor.changeInfor();

        GameObject.Find("MainCamera").GetComponent<Animator>().SetBool("Start", true);
    }
}
