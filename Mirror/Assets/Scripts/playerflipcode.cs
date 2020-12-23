using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class palyerflipcode : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D m_rg;
    private Animator myAnim;
    private BoxCollider2D my_feet;

    public int MoveSpeed;
    public int JumpSpeed;

    private float playerGravity;


    private bool isGround;
    void Start()
    {
        m_rg = gameObject.GetComponent<Rigidbody2D>();
        myAnim = gameObject.GetComponent<Animator>();
        my_feet = gameObject.GetComponent<BoxCollider2D>();
        myAnim.SetBool("Idle", true);
        playerGravity = m_rg.gravityScale;
    }

    // Update is called once per frame
    void Update()
    {
        //restart();
        Run();
        Flip();
        Jump();
        CheckGrounded();
        SwithAnimation();


    }
    //void restart()
    //{
    //    if (Input.GetButtonDown("restart"))
    //        SceneManager.LoadScene(0);

    //}
    void Run()
    {
        if (Input.GetAxisRaw("Horizontal") > 0)
        {
            m_rg.velocity = new Vector2(MoveSpeed, m_rg.velocity.y);
            bool playHasXAxisSpeed = Mathf.Abs(m_rg.velocity.x) > Mathf.Epsilon;
            myAnim.SetBool("Run", playHasXAxisSpeed);
            myAnim.SetBool("Idle", false);
        }
        else if (Input.GetAxisRaw("Horizontal") < 0)
        {
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
        isGround = my_feet.IsTouchingLayers(LayerMask.GetMask("Road"))
            || my_feet.IsTouchingLayers(LayerMask.GetMask("Lattern"));
        //Debug.Log(isGround);
    }
    void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGround)
            {
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
    void SwithAnimation()
    {
        myAnim.SetBool("Idle", false);

        //Debug.Log(myAnim.GetBool("Jump"));
        if (myAnim.GetBool("Jump"))
        {
            // Debug.Log("")
            Debug.Log(m_rg.velocity.y);
            if (m_rg.velocity.y < 0.0f)
            {
                myAnim.SetBool("Jump", false);
                myAnim.SetBool("Fall", true);
            }
        }
        else if (isGround)
        {
            myAnim.SetBool("Fall", false);
            myAnim.SetBool("Idle", true);
        }
    }
}