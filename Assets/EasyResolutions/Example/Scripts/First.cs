//-----------------------------------------------------------------------
// <copyright file="First.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using UnityEngine;
using EasyResolutions;

/// <summary>
/// A script for the very first scene in the project. It is only 
/// intended to load the next scene with appropriate resolution.
/// You may add such script as a component to the camera.
/// </summary>
public class First : MonoBehaviour
{
    /// <summary>
    /// Immediately loads the next scene with appropriate 
    /// resolution on component initialization.
    /// </summary>
    private void Start()
    {
        // Don't use `PickResolution` in `Awake` method, because 
        // scene post-process steps take place after `Awake` is invoked.
        Application.LoadLevel("TestScene".PickResolution(SceneMode.Fill));
    }
}
