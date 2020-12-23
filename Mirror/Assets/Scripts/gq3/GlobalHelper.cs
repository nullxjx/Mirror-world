using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalHelper : MonoBehaviour
{
    public Vector2 player_init_position = new Vector2(-7f, -3.950255f);
    public float mirror_X = 3.85f;
    public Vector2 player_mirror_init_position;

    public float scale = 0.09267534f;

    //动态调整BoxCollider的大小和位置
    public Vector2 idle_right_offset = new Vector2(3.047779f, -6.301541f);
    public Vector2 idle_left_offset = new Vector2(-2.98f, -6.301541f);
    public Vector2 idle_size = new Vector2(3.016654f, 2.321558f);

    public Vector2 run_right_offset = new Vector2(0.3230515f, -5.595082f);
    public Vector2 run_left_offset = new Vector2(-0.32f, -5.595082f);
    public Vector2 run_size = new Vector2(11.7139f, 2.321558f);

    public Vector2 rightTree_offset = new Vector2(-2.766898f, -0.4620028f);
    public Vector2 rightTree_size = new Vector2(2.701047f, 13.68564f);

    public Vector2 on_swing_offset = new Vector2(0.1899998f, 0.2322603f);
    public Vector2 on_swing_size = new Vector2(4.392888f, 2.568117f);
    public Vector2 on_swing_pos = new Vector2(0.25f, 0.03999995f);

    public Vector2 swing_offset = new Vector2(0.05247068f, -0.1829675f);
    public Vector2 swing_size = new Vector2(4.266482f, 1.647488f);

    public float RunSpeed = 5;//跑动速度
    public float JumpSpeed = 5.2f;//跳跃速度
    public float ClimbSpeed = 5;//爬树速度


    public int control = 0;//0表示玩家控制左边人物(境外)，1表示玩家控制右边人物(镜内)


    public string camera_tag = "player";

    public bool die_flag = false;

    public static GlobalHelper instance { get; private set; }

    static public GameObject tipdialog;

    private void Awake()
    {
        instance = this; 
    }

    void ShutDownDialog()
    {
        tipdialog.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {

        //tipdialog= GameObject.FindGameObjectWithTag("level3tipdialog");
        //tipdialog.SetActive(true);
        //Invoke("ShutDownDialog", 4f);

        //Debug.Log("mirror_X: " + mirror_X);
        float player_mirror_X = 2 * mirror_X - player_init_position.x;
        player_mirror_init_position = new Vector2(player_mirror_X, player_init_position.y);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
