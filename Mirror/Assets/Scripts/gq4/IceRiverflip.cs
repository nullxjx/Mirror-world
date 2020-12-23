using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRiverflip : MonoBehaviour
{
    // Start is called before the first frame update
    private GameObject iceriver;
    private Animator iceriveranim;
    private Animator m_anim;
    void Start()
    {
        iceriver = GameObject.Find("iceriver");
        iceriveranim=iceriver.GetComponent<Animator>();
        m_anim=gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        m_anim.SetBool("riverbreak", iceriveranim.GetBool("riverbreak"));
    }
}
