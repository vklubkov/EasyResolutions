//-----------------------------------------------------------------------
// <copyright file="EqualityResolutionComparisonComponent.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents the check of resolutions A and B for being equal one to another and to game resolution.
    /// </summary>
    public class EqualityResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Checks the resolutions A and B for being equal and checks them for equality against the game resolution.
        /// If at least one of resolutions A and B is <c>null</c>, they are treated as equal. If A is equal 
        /// to B, they are (surprise!) equal. If A is equal to game resolution, and B isn't, A is "bigger". 
        /// If B is equal to game resolution, and A isn't, B is "bigger". If nothing of the above is correct, 
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
            
            if (resolutionA.Width == gameResolution.width &&
                resolutionA.Height == gameResolution.height)
            {
                result = -1;
                return true;
            }
            
            if (resolutionB.Width == gameResolution.width &&
                resolutionB.Height == gameResolution.height)
            {
                result = 1;
                return true;
            }
            
            return false;
        }
    }
}