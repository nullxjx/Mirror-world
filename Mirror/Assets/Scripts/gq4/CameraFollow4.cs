using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow4 : MonoBehaviour
{
    // Start is called before the first frame update

    public string tag;
    private Transform m_playerTransform;


    //设定一个角色能看到的最远值
    //public float Ahead;

    //设置一个摄像机要移动到的点
    private Vector3 targetPos;

    public float smooth;

    //private float devHeight = 9.6f;
    private float devWidth = 6.4f;

    private bool dc;

    void Start()
    {
        m_playerTransform = GameObject.FindGameObjectWithTag(tag).GetComponent<Transform>();
        float screenHeight = Screen.height;

        //Debug.Log("screenHeight = " + screenHeight);

        //this.GetComponent<Camera>().orthographicSize = screenHeight / 200.0f;

        float orthographicSize = this.GetComponent<Camera>().orthographicSize;

        float aspectRatio = Screen.width * 1.0f / Screen.height;

        float cameraWidth = orthographicSize * 2 * aspectRatio;

        //Debug.Log("cameraWidth = " + cameraWidth);

        if (cameraWidth < devWidth)
        {
            orthographicSize = devWidth / (2 * aspectRatio);
            Debug.Log("new orthographicSize = " + orthographicSize);
            this.GetComponent<Camera>().orthographicSize = orthographicSize;
        }
        //targetPos = new Vector3(m_playerTransform.position.x + Ahead, m_playerTransform.position.y, gameObject.transform.position.z);
        //dc = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(m_playerTransform!=null)
            targetPos = new Vector3(m_playerTransform.position.x, 0, transform.position.z);        //targetPos = new Vector3(m_playerTransform.position.x, m_playerTransform.position.y, gameObject.transform.position.z);
        transform.position = Vector3.Lerp(transform.position, targetPos, smooth * Time.deltaTime);
        //transform.position = Vector3.Move(transform.position, targetPos);
    }


}
