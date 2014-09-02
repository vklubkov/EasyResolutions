//-----------------------------------------------------------------------
// <copyright file="FloatHelper.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    //using System;
    
    /// <summary>
    /// Helper class for <c>float</c> type.
    /// </summary>
    public static class FloatHelper
    {
        /// <summary>
        /// Gets the epsilon for comparison of aspect ratio factors. 
        /// 0.008f seems to be a good choice (because of variety of inaccurate 16:9 resolutions).
        /// </summary>
        public static float EpsilonForComparisonOfAspectRatioFactors
        {
            get { return 0.008f; }
        }

        /// <summary>
        /// Checks whether two float values are nearly equal. 
        /// This is the simplest possible check. It is good enough
        /// for the comparison of screen resolution aspect ratio factors, 
        /// but may fail in some special cases. For better implementation
        /// you may want to look at
        /// http://stackoverflow.com/questions/3874627/floating-point-comparison-functions-for-c-sharp
        /// </summary>
        /// <returns><c>true</c>, if two <c>float</c>'s are nearly equal, <c>false</c> otherwise.</returns>
        /// <param name="valueA">The first value to compare.</param>
        /// <param name="valueB">The second value to compare.</param>
        /// <param name="epsilon">Epsilon value.</param>
        public static bool NearlyEquals(this float valueA, float valueB, float epsilon)
        {
            return System.Math.Abs(valueA - valueB) < epsilon;
        }
    }
}