//-----------------------------------------------------------------------
// <copyright file="ResolutionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Collections.Generic;

    /// <summary>
    /// <see cref="ScenesAndResolutions.Resolution"/>'s comparer. Provides 
    /// a way to customize the <see cref="ScenesAndResolutions.Resolution"/>'s
    /// comparison using the comparison components.
    /// </summary>
    public class ResolutionComparer : IResolutionComparer
    {
        /// <summary>
        /// The list of comparison components.
        /// </summary>
        private List<IResolutionComparisonComponent> comparisonComponents = new List<IResolutionComparisonComponent>();

        /// <summary>
        /// Gets or sets the <see cref="UnityEngine.Resolution"/>, 
        /// which represents the current game resolution.
        /// </summary>
        public UnityEngine.Resolution GameResolution
        {
            get;
            set;
        }

        /// <summary>
        /// Appends the specified comparison component.
        /// </summary>
        /// <returns><c>true</c>, if component was successfully appended, <c>false</c> otherwise.</returns>
        /// <param name="comparisonComponent">Comparison component to append.</param>
        public bool AppendComparisonComponent(IResolutionComparisonComponent comparisonComponent)
        {
            if (comparisonComponent == null)
            {
                return false;
            }

            this.comparisonComponents.Add(comparisonComponent);
            return true;
        }
        
        /// <summary>
        /// Compares the specified scene resolutions depending on the 
        /// game resolution, which was specified as property.
        /// </summary>
        /// <returns>-1 indicates, that resolution A best fits the game resolution,
        /// 1 indicates, that resolution B best fits the game resolution,
        /// 0 indicates, that resolutions A and B are equal.</returns>
        /// <param name="resolutionA">The first resolution to compare.</param>
        /// <param name="resolutionB">The second resolution to compare.</param>
        public int Compare(Resolution resolutionA, Resolution resolutionB)
        {
            foreach (var component in this.comparisonComponents)
            { 
                int result;
                if (component.Compare(out result, resolutionA, resolutionB, this.GameResolution))
                {
                    return result;
                }
            }

            return 0;
        }
    }
}