//-----------------------------------------------------------------------
// <copyright file="TestSceneRotation.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
using ScenesAndResolutions;
using UnityEngine;

/// <summary>
/// Reloads TestScene, when window resolution changes.
/// </summary>
public class TestScene : MonoBehaviour
{
    /// <summary>
    /// The old width.
    /// </summary>
    private int oldWidth;

    /// <summary>
    /// The old height.
    /// </summary>
    private int oldHeight;

    /// <summary>
    /// Stores initial width and height values.
    /// </summary>
    private void Start()
    {
        this.oldWidth = Screen.width;
        this.oldHeight = Screen.height;
    }

    /// <summary>
    /// Checks the resolution on each frame and
    /// updates the scene, if necessary.
    /// </summary>
    private void Update()
    {
        if (this.oldWidth != Screen.width || 
            this.oldHeight != Screen.height)
        {
            this.oldWidth = Screen.width;
            this.oldHeight = Screen.height;
            Application.LoadLevel("TestScene".PickResolution(SceneMode.Fill));
        }
    }
}
