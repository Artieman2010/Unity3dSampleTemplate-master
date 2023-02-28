using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class ProcessDetector
{

    public static bool detected;
    public static List<string> currentlyRunningProcesses = new List<string>();
    public static string[] suspiciousprocessNames = { "Cheat", "Debugger", "x64dbg", "IDA Pro", "cheatengine-x86_64" };

    public static string ignore = AppDomain.CurrentDomain.FriendlyName;
    
    public static void UpdateProcList()
    {
        Process[] running = Process.GetProcesses();
        foreach (Process process in running)
        {
            currentlyRunningProcesses.Add(process.ProcessName);

        }
    }
    public static void FindProcesses()
    {
        foreach(string process in currentlyRunningProcesses)
        {
           
            if (process != ignore)
            {
                for (int i = 0; i < suspiciousprocessNames.Length; i++)
                {
                    if (process.Contains(suspiciousprocessNames[i]))
                    {
                        detected = true;
                        return;
                    }


                }
            }
        }
    }
}
public class DetectDebugger
{
    public static bool detected;
    public static string[] debuggers = { "ida64", "dbg64", "Debugger" };
    [DllImport("kernel132.dll", SetLastError = true, ExactSpelling = true)]
    public static extern bool CheckRemoteDebuggerPresent(IntPtr hProcess, ref bool isDebuggerPresent);
  
    public static void UpdateDetection()
    {

        for (int i = 0; i < debuggers.Length; i++)
        {
            Process[] debuggerrunning = Process.GetProcessesByName(debuggers[i]);
           if(debuggerrunning != null & debuggerrunning.Length > 0)
            {
                detected = true;
            }


        }
        bool isDebuggerPresent = false;
        CheckRemoteDebuggerPresent(Process.GetCurrentProcess().Handle, ref isDebuggerPresent);
        if (isDebuggerPresent)
        {
            detected = true;
        }
        if (Debugger.IsAttached)
        {
            detected = true;
        }
    }
}
