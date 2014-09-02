//-----------------------------------------------------------------------
// <copyright file="UnityEngineResolutionHelper.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// <see cref="UnityEngine.Resolution"/> resolution helper.
    /// </summary>
    public static class UnityEngineResolutionHelper
    {
        /// <summary>
        /// Sets the specified width and height to the <see cref="UnityEngine.Resolution"/>.
        /// If at least one of the specified values <c>&lt;= 0</c>, 
        /// than both the width and height of the resolution are set to <c>0</c>.
        /// Otherwise, the specified values are used.
        /// </summary>
        /// <returns><see cref="UnityEngine.Resolution"/> with new width and height.</returns>
        /// <param name="resolution"><see cref="UnityEngine.Resolution"/> to set width and height.</param>
        /// <param name="width">Resolution width.</param>
        /// <param name="height">Resolution height.</param>
        public static UnityEngine.Resolution Set(this UnityEngine.Resolution resolution, int width, int height)
        {
            if (width > 0 && height > 0)
            {
                resolution.width = width;
                resolution.height = height;
            }
            else
            {
                resolution.width = 0;
                resolution.height = 0;
            }

            return resolution;
        }

        /// <summary>
        /// Gets the aspect ratio factor of the specified <see cref="UnityEngine.Resolution"/>.
        /// </summary>
        /// <returns>The aspect ratio factor of the specified <see cref="UnityEngine.Resolution"/>.</returns>
        /// <param name="resolution"><see cref="UnityEngine.Resolution"/> to compute aspect ratio for.</param>
        public static float GetAspectRatioFactor(this UnityEngine.Resolution resolution)
        {
            if (resolution.width <= 0 || resolution.height <= 0)
            {
                return 0f;
            }

            var width = (float)resolution.width;
            var height = (float)resolution.height;
            return (width > height) ? (width / height) : (height / width);
        }

        /// <summary>
        /// Determines whether the specified <see cref="UnityEngine.Resolution"/> has landscape orientation.
        /// </summary>
        /// <returns><c>true</c> if the specified <see cref="UnityEngine.Resolution"/>
        /// has landscape orientation; otherwise, <c>false</c>.</returns>
        /// <param name="resolution"><see cref="UnityEngine.Resolution"/> to check the orientation.</param>
        public static bool IsLandscape(this UnityEngine.Resolution resolution)
        {
            return (resolution.width <= 0 || resolution.height <= 0) || (resolution.width >= resolution.height);
        }

        /// <summary>
        /// Determines whether the specified <see cref="UnityEngine.Resolution"/> has portrait orientation.
        /// </summary>
        /// <returns><c>true</c> if the specified <see cref="UnityEngine.Resolution"/> 
        /// has portrait orientation; otherwise, <c>false</c>.</returns>
        /// <param name="resolution"><see cref="UnityEngine.Resolution"/> to check the orientation.</param>
        public static bool IsPortrait(this UnityEngine.Resolution resolution)
        {
            return (resolution.width <= 0 || resolution.height <= 0) || (resolution.width <= resolution.height);
        }
    }
}