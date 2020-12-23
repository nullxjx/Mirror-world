using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PlayerController3 : MonoBehaviour
{
    private Rigidbody2D rigidbody2d;
    private Animator animator;
    private BoxCollider2D boxCollider2d;

    private GameObject swing;
    private GameObject player_mirror;

    private Vector2 lookDirection = new Vector2(1, 0);
    private Vector3 scale;

    private bool isJumping;
    private bool isFalling;
    private bool isGround;//是否落地
    private bool isSwing;//是否在秋千上
    private bool isTrunk;//是否在树干上
    private bool isTree;//是否在树上

    private float MAX_ANGLE = 50;
    private float MIN_ANGLE = -50;
    private float maxAngle;
    private float minAngle;
    private float circleSpeed = 0;
    private float maxSpeed = 6;
    private bool direction = true;//true向右，false向左

    private bool switch_mirror = false;

    public float last_height;
    private float safe_height;

    private AudioSource music;
    private AudioClip jumpsound;
    private AudioClip runsound;
    private AudioClip climbsound;
    private AudioClip swingsound;



    private float movemaxx;
    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        boxCollider2d = GetComponent<BoxCollider2D>();
        swing = GameObject.Find("swing");
        player_mirror = GameObject.Find("player_mirror");

        maxAngle = MAX_ANGLE;
        minAngle = MIN_ANGLE;

        last_height = transform.position.y;
        safe_height = 4.5f;//经过场景测试出来
        scale = transform.localScale;
        Debug.Log("initScale: " + scale);

        InitBoxCollider();
        InitialSounds();
        movemaxx = transform.position.x; 
    }

    // Update is called once per frame
    void Update()
    {
        if (GlobalHelper.instance.die_flag) {
            UpdateSwing();
            FadeOverTime();
            return;
        }

        if (GlobalHelper.instance.control == 0)
        {
            CheckAirStatus();
            Run();
            Jump();

            CheckScale();
            CheckGrounded();
            SwitchAnimation();

            Flip();
            UpdateSpeed();
            PlaySwing();
            FadeOverTime();
            AdjustBoxCollider();
            MoveMaxX();
        }
        else {
            UpdateSwing();
            FadeOverTime();

            //跟随右边运动
            float pos_x = 2 * GlobalHelper.instance.mirror_X - player_mirror.transform.position.x;
            Vector2 pos = new Vector2(pos_x, player_mirror.transform.position.y);
            rigidbody2d.MovePosition(pos);

            string clip = getCurrentClipInfo();
            animator.Play(clip);
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
        swingsound = Resources.Load<AudioClip>("music/swing");
    }

    void CheckScale() {
        if (!isSwing && transform.localScale != scale) {
            Debug.Log("恢复形变");
            rigidbody2d.transform.localScale = scale;
        }
    }

    public void SetGravity(float value, string funcName = "") {
        rigidbody2d.gravityScale = value;
        Debug.Log(funcName + " " + value);
    }

    public void SetTrunk(bool flag) {
        isTrunk = flag;
        last_height = transform.position.y; 

        if (isTrunk)
        {
            animator.SetBool("climbIdle", true);
            animator.SetBool("run", false);
            animator.SetBool("jump", false);
            animator.SetBool("fall", false);
        }
        else {
            animator.SetBool("idle", true);
            animator.SetBool("climb", false);
            animator.SetBool("climbIdle", false);
        }
    }

    public string getCurrentClipInfo() // 获取当前执行的动画
    {
        AnimatorClipInfo[] m_CurrentClipInfo = player_mirror.GetComponent<Animator>().GetCurrentAnimatorClipInfo(0);
        return m_CurrentClipInfo[0].clip.name;
    }

    void UpdateSpeed()
    {
        if (Input.GetButtonDown("communicateStone") && isSwing)
        {
            maxAngle = MAX_ANGLE;
            minAngle = MIN_ANGLE;
            if (circleSpeed < maxSpeed)
                circleSpeed += 0.5f;
        }
    }

    void FadeOverTime() {
        if (circleSpeed > 0.5f)
            circleSpeed -= 0.01f;

        if (maxAngle > 0)
            maxAngle -= 0.01f;
        if(minAngle < 0f)
            minAngle += 0.01f;
    }

    void PlaySwing() {    
        if (isSwing)
        {
            if (!music.isPlaying && circleSpeed > 0.5f)
            {
                music.clip = swingsound;
                music.Play();
            }

            UpdateSwing();
        }
        else if(swing.transform.eulerAngles.z != 0)
        {
            UpdateSwing();
        }
    }

    void UpdateSwing() 
    {
        if (maxAngle < 0.008455329) return;
        if (direction)
        {
            Vector3 angle_ = new Vector3(0, 0, circleSpeed);
            swing.transform.Rotate(angle_);
            if (swing.transform.eulerAngles.z < 100 && swing.transform.eulerAngles.z > maxAngle)
            {
                direction = false;
            }
        }
        else
        {
            Vector3 angle_ = new Vector3(0, 0, -circleSpeed);
            swing.transform.Rotate(angle_);
            if (swing.transform.eulerAngles.z > 200 && (swing.transform.eulerAngles.z - 360) < minAngle)
            {
                direction = true;
            }
        }
    }

    void Flip()
    {
        bool playHasXAxisSpeed = Mathf.Abs(rigidbody2d.velocity.x) > Mathf.Epsilon;
        if (playHasXAxisSpeed)
        {
            if (rigidbody2d.velocity.x > 0.1f) {
                transform.localRotation = Quaternion.Euler(0, 0, 0);
                if (GlobalHelper.instance.control == 0) 
                { 
                    player_mirror.transform.localRotation = Quaternion.Euler(0, 0, 0);
                }
            }

            if (rigidbody2d.velocity.x < -0.1f) {
                transform.localRotation = Quaternion.Euler(0, 180, 0);
                if (GlobalHelper.instance.control == 0)
                {
                    player_mirror.transform.localRotation = Quaternion.Euler(0, 180, 0);
                }
            }   
        }
    }

    void CheckAirStatus()
    {
        isJumping = animator.GetBool("jump");
        isFalling = animator.GetBool("fall");
    }

    void SwitchAnimation() {
        animator.SetBool("idle", false);
        if (animator.GetBool("jump"))
        {
            if (rigidbody2d.velocity.y < 0.0f)
            {
                animator.SetBool("jump", false);
                animator.SetBool("fall", true);
            }
        }
        else if (isGround || isSwing || isTree)
        {
            animator.SetBool("fall", false);
            animator.SetBool("climb", false);
            animator.SetBool("idle", true);
        }
        else if (isTrunk) {
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
            else {
                animator.SetBool("climb", false);
                animator.SetBool("climbIdle", true);
            }
        }
    }

    void Run()
    {
        if (!isSwing) transform.SetParent(null);//离开秋千
        

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
            //Debug.Log("Run idle false: " + GetSysTime());
        }
        else
        {
            animator.SetBool("run", false);
            animator.SetBool("idle", true);
            // Anim.CrossFade("palyeridle", 0.1f, -1, 0);
            rigidbody2d.velocity = new Vector2(0, rigidbody2d.velocity.y);
        }
    }

    string GetSysTime() {
        int hour = DateTime.Now.Hour;
        int minute = DateTime.Now.Minute;
        int second = DateTime.Now.Second;
        int miniSec = DateTime.Now.Minute;

        string txt = string.Format("{0:D2}:{1:D2}:{2:D2}:{3:D4}", hour, minute, second, miniSec);
        return txt; 
    }

    void CheckGrounded()
    {
        isGround = rigidbody2d.IsTouchingLayers(LayerMask.GetMask("Road"));
        isTree = rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3tree"));
        isSwing = rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3swing"));
    }

    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround || isSwing || isTree)
            {
                animator.SetBool("jump2idle", false);//去掉会在jump和idle动画直接来回切换

                var st = new System.Diagnostics.StackTrace();
                SetGravity(1.0f, st.GetFrame(0).ToString());
                
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

    void InitBoxCollider()
    {
        boxCollider2d.offset = GlobalHelper.instance.idle_right_offset;
        boxCollider2d.size = new Vector2(GlobalHelper.instance.idle_size.x, GlobalHelper.instance.idle_size.y);

        Vector2 position = GlobalHelper.instance.player_init_position;
        rigidbody2d.MovePosition(position);
        Debug.Log("player init pos: " + position);

        AdjustBoxCollider();
    }

    void AdjustBoxCollider()
    {
        float x_speed = rigidbody2d.velocity.x;
        if (x_speed > 0) {
            if (x_speed > 0.1f)
            {
                //往右移动
                boxCollider2d.offset = GlobalHelper.instance.run_right_offset;
                boxCollider2d.size = new Vector2(GlobalHelper.instance.run_size.x, GlobalHelper.instance.run_size.y);
            }
            else {
                //往左静止
                boxCollider2d.offset = GlobalHelper.instance.idle_left_offset;
                boxCollider2d.size = new Vector2(GlobalHelper.instance.idle_size.x, GlobalHelper.instance.idle_size.y);
            }
        }
        else
        {
            if (x_speed < -0.1f)
            {
                //往左移动
                boxCollider2d.offset = GlobalHelper.instance.run_left_offset;
                boxCollider2d.size = new Vector2(GlobalHelper.instance.run_size.x, GlobalHelper.instance.run_size.y);
            }
            else
            {
                //往右静止
                boxCollider2d.offset = GlobalHelper.instance.idle_right_offset;
                boxCollider2d.size = new Vector2(GlobalHelper.instance.idle_size.x, GlobalHelper.instance.idle_size.y);
            }
        }

        if (isJumping) { 

        }
    }

    public void Die(string msg) {
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

    void OnCollisionEnter2D(Collision2D collision)
    {
        float cur_height = transform.position.y;
        float deltaHeight = last_height - cur_height;
        Debug.Log("deltaHeight: " + deltaHeight);
        if (deltaHeight > safe_height)
        {
            Die("too high!");
            Debug.Log("die");
        }
        else 
        {
            last_height = cur_height;
        }

        if (rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3swing"))) {
            if (collision.contacts[0].normal.y < 0)
            {
                Debug.Log("碰到秋千下面");
                animator.SetBool("jump2idle", true);
            }
            else {
                Debug.Log("碰到秋千上面");
                isSwing = true;
                animator.SetBool("swing", true);
                transform.SetParent(collision.gameObject.transform);
            }
        }

        if (rigidbody2d.IsTouchingLayers(LayerMask.GetMask("level3fragment")))
        {
            transform.SetParent(GameObject.Find("swing").transform);

            Debug.Log("destroy mirror_fragment...");
            Destroy(GameObject.Find("mirror_fragment"));

            switch_mirror = true;
        }

        if (rigidbody2d.IsTouchingLayers(LayerMask.GetMask("Road")))
        {
            animator.SetBool("swing", false);
            animator.SetBool("idle", true);
            if (switch_mirror)
            {
                var st = new System.Diagnostics.StackTrace();
                SetGravity(0.0f, st.GetFrame(0).ToString());//镜子外人物没有重力
                transform.GetComponent<BoxCollider2D>().enabled = false;//镜子外人物无法发生碰撞

                GlobalHelper.instance.control = 1;//捡到镜子碎片后由右边的人物控制
                player_mirror.transform.GetComponent<BoxCollider2D>().enabled = true;//镜子内人物可以发生碰撞
                player_mirror.GetComponent<Rigidbody2D>().gravityScale = 1.0f;//镜子内人物重力恢复1

                GlobalHelper.instance.camera_tag = "player_mirror";

                animator.SetBool("run", false);
                animator.SetBool("swing", false);
                animator.SetBool("jump", false);
                animator.SetBool("fall", false);
                animator.SetBool("climb", false);
                animator.SetBool("climbIdle", false);
            }
        }   
    }
}
