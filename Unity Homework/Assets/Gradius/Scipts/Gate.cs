using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gate : MonoBehaviour
{
    public float openOffset;
    public float closeOffset;
    private float openDistance;
    private float cloaseDistance;

    private GameObject player;
    private Animator anim;

    void Start()
    {
        player = GameObject.Find("Vic Viper");
        anim = GetComponent<Animator>();
        anim.SetBool("Open", false);
    }

    void Update()
    {
        if (IsPlayerCloseEnough())
        {
            anim.SetBool("Open", true);
        }
        else
        {
            anim.SetBool("Open", false);
        }
    }

    private bool IsPlayerCloseEnough()
    {
        if (player != null)
        {
            bool playerPosXClose = false;
            bool playerPosYClose = false;
            if (player.transform.position.x > transform.position.x)
            {
                playerPosXClose =(player.transform.position.x -transform.position.x) < openOffset;
                
            }
            else
            {
                playerPosXClose = (transform.position.x - player.transform.position.x) < openOffset;
            }
            if(player.transform.position.y > transform.position.y)
            {
                playerPosYClose = (player.transform.position.y - transform.position.y) < openOffset;
            }
            else
            {
                playerPosYClose = (transform.position.y-player.transform.position.y) < openOffset;
            }

            return (playerPosXClose == true && playerPosYClose == true);
        }
        return false;
    }
}
