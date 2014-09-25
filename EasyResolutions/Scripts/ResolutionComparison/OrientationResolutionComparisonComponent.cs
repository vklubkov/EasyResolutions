//-----------------------------------------------------------------------
// <copyright file="OrientationResolutionComparisonComponent.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    /// <summary>
    /// Represents the comparison of orientations of <see cref="EasyResolutions.Resolution"/>'s 
    /// A and B against the orientation of the game resolution.
    /// </summary>
    public class OrientationResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Compares orientations of resolutions A and B against orientation of the game resolution.
        /// If at least one of resolutions A and B is <c>null</c>, they are treated as equal.
        /// If game resolution has landscape orientation, A has landscape orientation and B has 
        /// portrait orientation - A is "bigger". If game resolution has portrait orientation, 
        /// A has portrait orientation and B has landscape orientation - A is "bigger". If game 
        /// resolution has landscape orientation, B has landscape orientation and A has portrait 
        /// orientation - B is "bigger". If game resolution has portrait orientation, B has portrait 
        /// orientation and A has landscape orientation - B is "bigger". Otherwise, further comparison 
        /// is necessary.
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

            if ((!gameResolution.IsPortrait() && resolutionA.IsLandscape() && !resolutionB.IsLandscape()) ||
                (!gameResolution.IsLandscape() && resolutionA.IsPortrait() && !resolutionB.IsPortrait()))
            {
                result = -1;
                return true;
            }
            
            if ((!gameResolution.IsPortrait() && !resolutionA.IsLandscape() && resolutionB.IsLandscape()) ||
                (!gameResolution.IsLandscape() && !resolutionA.IsPortrait() && resolutionB.IsPortrait()))
            {
                result = 1;
                return true;
            }
           
            return false;
        }
    }
}