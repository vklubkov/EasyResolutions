//-----------------------------------------------------------------------
// <copyright file="SizeResolutionComparisonComponent.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents the comparison of sizes of <see cref="ScenesAndResolutions.Resolution"/>'s 
    /// A and B and their check against the size of the game resolution.
    /// </summary>
    public class SizeResolutionComparisonComponent : IResolutionComparisonComponent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ScenesAndResolutions.SizeResolutionComparisonComponent"/> class.
        /// </summary>
        public SizeResolutionComparisonComponent()
        {
            this.WidthComparer = new SingleDimensionComparer();
            this.HeightComparer = new SingleDimensionComparer();
        }

        /// <summary>
        /// Gets or sets the width comparer.
        /// </summary>
        public ISingleDimensionComparer WidthComparer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the height comparer.
        /// </summary>
        public ISingleDimensionComparer HeightComparer
        {
            get;
            set;
        }

        /// <summary>
        /// Compares the sizes of resolutions A and B and checks them against the size of game resolution.
        /// If at least one of resolutions A and B is <c>null</c>, they are treated as equal. If the results 
        /// of sizes comparison in two single dimensions are equal, this value is chosen as the comparison 
        /// result. If game resolution has landscape orientation or is square and the widths comparison 
        /// result is equality, the resolution with smaller height is "bigger". If game resolution has 
        /// landscape orientation or is square and the width comparison result isn't equality, the resolution 
        /// with smaller width is "bigger". If game resolution has portrait orientation and the heights 
        /// comparison result is equality, the resolution with smaller width is "bigger". If game resolution 
        /// has portrait orientation and the heights comparison result isn't equality, the resolution with 
        /// smaller height is "bigger". This <see cref="ScenesAndResolutions.IResolutionComparisonComponent"/> 
        /// is final, no further comparison necessary.
        /// </summary>
        /// <returns>Always <c>true</c>, which means, that the comparison is finished.</returns>
        /// <param name="result">The result of the comparison: 0 if resolutions are 
        /// equal, -1 if A is "bigger", 1 if B is "bigger".</param>
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

            if (this.WidthComparer == null || this.HeightComparer == null)
            {
                return true;
            }

            var widthResult = this.WidthComparer.Compare(resolutionA.Width, resolutionB.Width, gameResolution.width);
            var heightResult = this.HeightComparer.Compare(resolutionA.Height, resolutionB.Height, gameResolution.height);
            if (widthResult == heightResult)
            {
                result = widthResult;
                return true;
            }

            if (gameResolution.IsLandscape())
            {
                if (widthResult == 0)
                {
                    result = (resolutionA.Height <= resolutionB.Height) ? -1 : 1;
                }
                else
                {
                    result = (resolutionA.Width <= resolutionB.Width) ? -1 : 1;
                }
            }
            else
            {
                if (heightResult == 0)
                {
                    result = (resolutionA.Width <= resolutionB.Width) ? -1 : 1;
                }
                else
                {
                    result = (resolutionA.Height <= resolutionB.Height) ? -1 : 1;
                }
            }

            return true;
        }
    }
}