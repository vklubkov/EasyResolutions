//-----------------------------------------------------------------------
// <copyright file="FitAspectRatioResolutionComparisonComponent.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents the comparison of <see cref="ScenesAndResolutions.Resolution"/>'s A and B aspect ratios,
    /// and their check against the game resolution aspect ratio. , to find the one, which best "fills" the
    /// game window. This means, that the algorithm will prefer the one, which completely fits into the game 
    /// window rectangle, or at least the one, which is cropped as little as possible, over others. 
    /// </summary>
    public class FitAspectRatioResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Compares the aspect ratios of specified resolutions A and B and checks them against the 
        /// aspect ratio of game resolution. If at least one of resolutions A and B is <c>null</c>, 
        /// they are treated as equal. If the aspect ratio factors of A and B are nearly equal, 
        /// further comparison is necessary. If aspect ratio factor of A is nearly equal to the 
        /// aspect ratio factor of game resolution, A is "bigger"; and, if aspect ratio factor 
        /// of B is nearly equal to the aspect ratio factor of game resolution, B is "bigger". 
        /// If aspect ratio factor's of A and B both are greater or both are lower than the 
        /// aspect ratio factor of game resolution, the resolution with aspect ratio factor, which is 
        /// closer to the aspect ratio factor of game resolution, is "bigger". If resolution
        /// A, B and game resolution all have portrait orientation, the resolution with greater 
        /// aspect ratio factor is "bigger". If game resolution is landscape, and both A and B are 
        /// portrait, the resolution with smaller aspect ratio is "bigger". If game resolution is
        /// portrait and both A and B are landscape, the resolution with smaller aspect ratio is 
        /// "bigger". If resolution A, B and game resolution all are landscape, the resolution with
        /// smaller aspect ratio is "bigger". Otherwise, further comparison is necessary. Note, 
        /// that aspect ratios of landscape and portrait resolutions of same sizes (in opposite 
        /// dimensions, of course) are treated as equal.
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
            
            var aspectA = resolutionA.GetAspectRatioFactor();
            var aspectB = resolutionB.GetAspectRatioFactor();
            var gameAspect = gameResolution.GetAspectRatioFactor();
            
            if (aspectA.NearlyEquals(aspectB, FloatHelper.EpsilonForComparisonOfAspectRatioFactors))
            {
                return false;
            }
            
            if (aspectA.NearlyEquals(gameAspect, FloatHelper.EpsilonForComparisonOfAspectRatioFactors))
            {
                result = -1;
                return true;
            }
            
            if (aspectB.NearlyEquals(gameAspect, FloatHelper.EpsilonForComparisonOfAspectRatioFactors))
            {
                result = 1;
                return true;
            }
            
            if ((aspectB <= aspectA && aspectA <= gameAspect) || 
                (aspectB >= aspectA && aspectA >= gameAspect))
            {
                result = -1;
                return true;
            }
            
            if ((aspectA <= aspectB && aspectB <= gameAspect) || 
                (aspectA >= aspectB && aspectB >= gameAspect))
            {
                result = 1;
                return true;
            }
            
            bool bothAAndBArePortrait = resolutionA.IsPortrait() && resolutionB.IsPortrait();
            bool needGreaterAspect = gameResolution.IsPortrait() && bothAAndBArePortrait;
            if (needGreaterAspect)
            {
                result = (aspectA >= aspectB) ? -1 : 1;
                return true;
            }
            
            bool bothAAndBAreLandscape = resolutionA.IsLandscape() && resolutionB.IsLandscape();
            bool needSmallerAspect = 
                (gameResolution.IsLandscape() && bothAAndBAreLandscape) || 
                (gameResolution.IsLandscape() && bothAAndBArePortrait) ||
                (gameResolution.IsPortrait() && bothAAndBAreLandscape);
            if (needSmallerAspect)
            {
                result = (aspectA <= aspectB) ? -1 : 1;
                return true;
            }

            return false;
        }
    }
}