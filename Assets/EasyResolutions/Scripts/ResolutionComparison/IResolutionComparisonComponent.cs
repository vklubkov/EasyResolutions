//-----------------------------------------------------------------------
// <copyright file="IResolutionComparisonComponent.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Resolution comparison component interface.
    /// </summary>
    public interface IResolutionComparisonComponent
    {
        /// <summary>
        /// Compares the specified resolution A to resolution B and checks them both against the game resolution.
        /// The greater is the one, which best fits the game resolution.
        /// </summary>
        /// <returns><c>true</c>, if the comparison is finished, and
        /// <c>false</c>, if further comparison is necessary.</returns>
        /// <param name="result">The result of the comparison: 0 if resolutions are equal or if 
        /// the further comparison is necessary, -1 if A is "bigger", 1 if B is "bigger".</param>
        /// <param name="resolutionA">First resolution to compare.</param>
        /// <param name="resolutionB">Second resolution to compare.</param>
        /// <param name="gameResolution">Game resolution.</param>
        bool Compare(out int result, Resolution resolutionA, Resolution resolutionB, UnityEngine.Resolution gameResolution);
    }
}