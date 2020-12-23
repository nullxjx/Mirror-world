using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Dialog : MonoBehaviour
{
    public GameObject dialogBox;
    public float displayTime = 4.0f;
    private float timerDisplay;
    public Text dialogText;


    // Start is called before the first frame update
    void Start()
    {
        dialogBox.SetActive(false);
        timerDisplay = -1;
    }

    // Update is called once per frame
    void Update()
    {
        if (timerDisplay >= 0)
        {
            timerDisplay -= Time.deltaTime;
            if (timerDisplay < 0)
            {
                dialogBox.SetActive(false);
            }
        }
    }

    public void DisplayDialog(string txt, Vector2 pos)
    {
        dialogBox.transform.position = pos;

        timerDisplay = displayTime;
        dialogText.text = txt;
        dialogBox.SetActive(true);
    }
}
