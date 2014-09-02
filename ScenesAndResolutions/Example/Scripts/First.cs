//-----------------------------------------------------------------------
// <copyright file="First.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ScenesAndResolutions;
using UnityEngine;

/// <summary>
/// A script for the very first scene in the project. It is only 
/// intended to load the next scene of appropriate resolution.
/// You may add such script as a component to the camera.
/// </summary>
public class First : MonoBehaviour
{
    /// <summary>
    /// Immediately loads the next scene of appropriate 
    /// resolution on component initialization.
    /// </summary>
    private void Start()
    {
        // Don't use `PickResolution` in `Awake` method, because 
        // scene post-process steps take place after `Awake` is invoked.
        Application.LoadLevel("TestScene".PickResolution(SceneMode.Fill));
    }
}
