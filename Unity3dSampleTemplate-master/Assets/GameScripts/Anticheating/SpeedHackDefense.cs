using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

using Photon.Pun;
public class SpeedHackDefense : MonoBehaviourPunCallbacks
{


    public static float systemTime = 0F;
    public static float timer = 0F;
    public static bool kickPlayer = false;

    public float tolerance = 1.1f;

    int resetTime = 0;

    // UI Canvas
    //public Canvas antihack_Canvas;

    void Start()
    {
        RestartTimer(true);
        systemTime = System.DateTime.Now.Second;
        timer = System.DateTime.Now.Second;

    }
    void Update()
    {
        if (systemTime == 10F || systemTime == 50F)
        {
            timer = System.DateTime.Now.Second;
            resetTime = 0;
        }

        systemTime = System.DateTime.Now.Second;
        timer += Time.deltaTime;

        float result = timer - systemTime;
        if (result < 0)
        {
            result = result * -1;
        }

        if (result > tolerance && kickPlayer == true && systemTime > 10F)
        {
            KickPlayer();
        }

        if (result > 60 + tolerance && systemTime < 10F && kickPlayer == true)
        {
            KickPlayer();
        }
    }

    //If player got caught do this
    void KickPlayer()
    {
        PhotonNetwork.LeaveRoom();
        Console.WriteLine("Stop Hacking Using Speed Hacks And Ruining Others Game. Do you have any emotions or empathy, or should you even exist?");
        Application.Quit();
        //Application.LoadLevel("antiHackScreen");
    }


    public static void RestartTimer(bool kick)
    {
        timer = System.DateTime.Now.Second;
        kickPlayer = kick;
    }
}