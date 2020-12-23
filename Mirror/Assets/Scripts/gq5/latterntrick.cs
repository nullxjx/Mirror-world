using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class latterntrick : MonoBehaviour
{
    // Start is called before the first frame update
    public string latterntagC;

    private Animator myAnim;
    private Animator realAnim;
    private GameObject latternrealobject;
    public Transform latternrealTransform;
    private string lattertag;
    void Start()
    {

        lattertag = "lattern" + latterntagC;
        //m_playerTransform = GameObject.Find("player1").GetComponent<Transform>()
        latternrealobject = GameObject.Find(lattertag);

        latternrealTransform = latternrealobject.GetComponent<Transform>();
        realAnim = latternrealobject.GetComponent<Animator>();
        myAnim = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (realAnim.GetBool("Light"))
        {
            myAnim.SetBool("Light", true);
        }
        else
        {
            myAnim.SetBool("Light", false);
        }
    }
}
