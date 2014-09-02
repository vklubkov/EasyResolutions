//-----------------------------------------------------------------------
// <copyright file="OppositeEqualityResolutionComparisonComponent.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents equality check of dimensions of <see cref="ScenesAndResolutions.Resolution"/>'s 
    /// A and B against opposite dimensions of game resolution (widths of A and B against height 
    /// of game resolution and heights of A and B against width of game resolutions).
    /// </summary>
    public class OppositeEqualityResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Checks equality of the specified resolutions A and B against opposite dimensions of game resolution.
        /// If at least one of resolutions A and B is <c>null</c>, they are treated as equal. If the width of A 
        /// is equal to the height of game resolution and the height of A is equal to the width of game resolution, 
        /// the resolution A is "bigger". If the width of B is equal to the height of game resolution and the 
        /// height of B is equal to the width of game resolution, the resolution B is "bigger". Otherwise, 
        /// further comparison is necessary.
        /// </summary>
        /// <returns><c>true</c>, if the comparison is finished, and
        /// <c>false</c>, if further comparison is necessary.</returns>
        /// <param name="result">The result of the comparison: 0 if resolutions are equal or if 
        /// the further comparison is necessary, -1 if A is "bigger", 1 if B is "bigger".</param>
        /// <param name="resolutionA">First resolution to compare.</param>
        /// <param name="resolutionB">Second resolution to compare.</param>
        /// <param name="gameResolution">Game resolution.</param>
        public bool Compare(out int result, Resolution resolutionA, Resolution resolutionB, UnityEngine.Resolution gameResolution)
        {
            result = 0;

            if (resolutionA == null || resolutionB == null)
            {
                return true;
            }

            if (resolutionA.Equals(resolutionB))
            {
                return true;
            }

            if (resolutionA.Width == gameResolution.height && 
                resolutionA.Height == gameResolution.width)
            {
                result = -1;
                return true;
            }
                
            if (resolutionB.Width == gameResolution.height && 
                resolutionB.Height == gameResolution.width)
            {
                result = 1;
                return true;
            }

            return false;
        }
    }
}