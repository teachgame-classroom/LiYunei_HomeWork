using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TitleControler : MonoBehaviour
{
    private Animator titleBlinkAnim;
    private bool isStartButtonPressed;

    void Start()
    {
        titleBlinkAnim = GameObject.Find("StartCanvas/Panel/Text").GetComponent<Animator>();
    }

    void Update()
    {
        if (!isStartButtonPressed)
        {
            if (Input.GetKeyDown(KeyCode.N))
            {
                GetComponent<AudioSource>().Play();
                titleBlinkAnim.SetBool("Fast", true);
                isStartButtonPressed = true;
                Invoke("LoadScnen", 2f);
            }
        }
    }

    void LoadScnen()
    {
        SceneManager.LoadScene(1);
    }
}
