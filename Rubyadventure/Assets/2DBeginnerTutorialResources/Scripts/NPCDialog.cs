using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NPCDialog : MonoBehaviour
{
    public GameObject dialogBox;
    public float displayTime = 4.0f;
    private float timerDisplay;
    public Text dialogtext;
    public AudioSource audioSource;
    public AudioClip doneTaskclip;
    private bool hasplayed;
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

    public void DisplayDialog()
    {
        timerDisplay = displayTime;
        dialogBox.SetActive(true);
        UIHealthBar.instance.hasTaken = true;
        if (UIHealthBar.instance.fixedNum>=4)
        {
            dialogtext.text = "Oh,good guy .You Ruby have done it well";
            if (!hasplayed)
            {
                audioSource.PlayOneShot(doneTaskclip);
                hasplayed = true;
            }
        }
    }
}
