using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using Debug = UnityEngine.Debug; // Explicitly use UnityEngine.Debug


public class ApplicationDebugger
{
    /// <summary>
    /// Logs when a class finishes initialization.
    /// </summary>
    public static void LogClassInitialization()
    {
        string className = new StackTrace().GetFrame(1).GetMethod().DeclaringType.Name;

        Debug.Log($"<color=green>[INITIALIZATION] Class: \"{className}\" has finished initialization.</color>");
    }

    /// <summary>
    /// Logs when a method completes execution.
    /// </summary>
    public static void LogMethodExecution()
    {
        StackFrame frame = new StackTrace().GetFrame(1); // Get the caller method info
        string className = frame.GetMethod().DeclaringType.Name;
        string methodName = frame.GetMethod().Name;

        Debug.Log($"<color=blue>[EXECUTION] Method: \"{methodName}\" in class: \"{className}\" has completed execution.</color>");
    }
}
