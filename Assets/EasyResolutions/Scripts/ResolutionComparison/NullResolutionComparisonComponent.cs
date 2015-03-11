//-----------------------------------------------------------------------
// <copyright file="NullResolutionComparisonComponent.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Represents the check of <see cref="EasyResolutions.Resolution"/>'s A and B for being <c>null</c>.
    /// </summary>
    public class NullResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Compares specified resolutions A and B to <c>null</c>.
        /// If both A and B are <c>null</c>, they are equal. If A is not 
        /// <c>null</c> and B is <c>null</c>, A is "bigger". If B is 
        /// not <c>null</c> and A is <c>null</c>, B is "bigger". If 
        /// they both aren't <c>null</c>, further comparison is necessary.
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

            if (resolutionA == null && resolutionB == null)
            {
                return true;
            }
            
            if (resolutionB == null)
            {
                result = -1;
                return true;
            }
            
            if (resolutionA == null)
            {
                result = 1;
                return true;
            }

            return false;
        }
    }
}