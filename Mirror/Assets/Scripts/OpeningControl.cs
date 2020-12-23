using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpeningControl : MonoBehaviour
{
    public void hideOpeningBG()
    {
        GameObject.Find("Canvas_UI/OpeningBG").SetActive(false);
        GameObject.Find("Canvas_UI/Text").SetActive(false);

        GameObject.Find("Canvas_UI/LeftMask").GetComponent<Animator>().SetBool("Start", true);
        GameObject.Find("Canvas_UI/RightMask").GetComponent<Animator>().SetBool("Start", true);

        Debug.Log("hide opening");
    }
}
