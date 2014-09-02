//-----------------------------------------------------------------------
// <copyright file="ZeroResolutionComparisonComponent.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents the check of <see cref="ScenesAndResolutions.Resolution"/>'s A and B for being zero resolutions.
    /// </summary>
    public class ZeroResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Checks the specified resolutions A and B for being zero and compares one to another.
        /// If at least one of resolutions A and B is <c>null</c>, they are treated as equal.
        /// If they both are zero, they are treated as equal. If A is zero and B isn't, A is 
        /// "bigger". If B is zero and A isn't, B is "bigger". Otherwise, if they both aren't 
        /// zero, further comparison is necessary.
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
            
            if (resolutionA.IsZero() && resolutionB.IsZero())
            {
                return true;
            }
            
            if (resolutionA.IsZero())
            {
                result = -1;
                return true;
            }
            
            if (resolutionB.IsZero())
            {
                result = 1;
                return true;
            }
            
            return false;
        }
    }
}