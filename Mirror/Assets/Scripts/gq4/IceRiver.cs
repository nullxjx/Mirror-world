using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceRiver : MonoBehaviour
{

    static public bool icebreakflag;
    private Animator m_anim;
    private bool icebreaksitutaion;
    // Start is called before the first frame update
    void Start()
    {
        m_anim = gameObject.GetComponent<Animator>();
        icebreaksitutaion = false;
    }

    // Update is called once per frame
    void Update()
    {
        icebreak();


    }

    void icebreak()
    {
        if (!icebreaksitutaion)
        {
            if (icebreakflag)
            {
                m_anim.SetBool("riverbreak",true);
                icebreaksitutaion = true;
            }
        }
    }
}
