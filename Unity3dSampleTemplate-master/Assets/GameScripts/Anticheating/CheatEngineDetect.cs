using System;

using System.Collections.Generic;

using System.Runtime.InteropServices;

using System.Text;

using UnityEngine;


//Script by Samarth Pradeep from SamsidParty

public class CheatEngineDetect : MonoBehaviour

{

#if UNITY_STANDALONE_WIN || UNITY_EDITOR_WIN


    //Native Code

    [DllImport("user32.dll", CharSet = CharSet.Unicode)]

    private static extern int GetWindowText(IntPtr hWnd, StringBuilder strText, int maxCount);


    [DllImport("user32.dll", CharSet = CharSet.Unicode)]

    private static extern int GetWindowTextLength(IntPtr hWnd);


    [DllImport("user32.dll")]

    private static extern bool EnumWindows(EnumWindowsProc enumProc, IntPtr lParam);


    public delegate bool EnumWindowsProc(IntPtr hWnd, IntPtr lParam);


    //Internal Methods
    private void Update()
    {
        
    }
    private static string GetWindowText(IntPtr hWnd)

    {

        int size = GetWindowTextLength(hWnd);

        if (size > 0)

        {

            var builder = new StringBuilder(size + 1);

            GetWindowText(hWnd, builder, builder.Capacity);

            return builder.ToString();

        }


        return String.Empty;

    }

    private static IEnumerable<IntPtr> FindWindows(EnumWindowsProc filter)

    {

        IntPtr found = IntPtr.Zero;

        List<IntPtr> windows = new List<IntPtr>();


        EnumWindows(delegate (IntPtr wnd, IntPtr param)

        {

            if (filter(wnd, param))

            {

                windows.Add(wnd);

            }


            return true;

        }, IntPtr.Zero);


        return windows;

    }

    private static IEnumerable<IntPtr> FindWindowsWithText(string titleText)

    {

        return FindWindows(delegate (IntPtr wnd, IntPtr param)

        {

            return GetWindowText(wnd).Contains(titleText);

        });

    }


    /// <summary>

    /// Detects whether cheat engine is running, works without admin

    /// </summary>

    /// <returns>True if cheat engine is running</returns>

    public static bool IsCheatEngineRunning()

    {

        var ce = FindWindowsWithText("Cheat Engine");

        foreach (var win in ce)

        {

            return true;

        }

        return false;

    }


#else


    /*public static bool IsCheatEngineRunning()

    {

        //Not supported on this platform

        return false;

    }*/ //might add comment from u


//
#endif
}

