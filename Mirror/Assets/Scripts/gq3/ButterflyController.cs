using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButterflyController : MonoBehaviour
{
    public float speed = 1;

    //轴向控制
    public bool vertical = true;

    private Rigidbody2D rigidbody2d;


    private int direction = 1;
    //方向改变时间间隔，常量
    public float changeTime = 3;
    //计时器
    private float timer;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        timer = changeTime;
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }


        Vector2 pos = rigidbody2d.position;

        if (vertical)
            pos.y += Time.deltaTime * speed * direction;
        else
            pos.x += Time.deltaTime * speed * direction;

        rigidbody2d.MovePosition(pos);
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        PlayerController3 player = collision.gameObject.GetComponent<PlayerController3>();

        if (player != null)
        {
            player.Die("hit butterfly");
        }
    }
}
