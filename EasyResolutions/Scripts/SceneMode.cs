//-----------------------------------------------------------------------
// <copyright file="SceneResolutionPicker.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Scene scale mode enumeration.
    /// </summary>
    public enum SceneMode
    {
        /// <summary>
        /// Finds the scene, which best "fills" the game window. 
        /// This means, that the algorithm will find scenes with 
        /// nearest aspect ratios to the game window aspect ratio, 
        /// and prefer the one, which covers the whole game window
        /// rectangle (thus, the scene may be cropped), or at 
        /// least the one, which covers more area of the rectangle.
        /// </summary>
        Fill,
        
        /// <summary>
        /// Finds the scene, which best "fits" the game window.
        /// This means, that the algorithm will find scenes with 
        /// nearest aspect ratios to the game window aspect ratio, 
        /// and prefer the one, which completely fits into the
        /// game window rectangle, or at least the one, which 
        /// is cropped as little as possible.
        /// </summary>
        Fit
    }
}