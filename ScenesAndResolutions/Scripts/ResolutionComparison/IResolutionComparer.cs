//-----------------------------------------------------------------------
// <copyright file="IResolutionComparer.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Collections.Generic;

    /// <summary>
    /// Resolution comparer interface.
    /// </summary>
    public interface IResolutionComparer : IComparer<Resolution>
    {
        /// <summary>
        /// Gets or sets the game resolution.
        /// This property is always set automatically right before sorting happens 
        /// in <see cref="ScenesAndResolutions.AvailableResolutions"/>.
        /// </summary>
        UnityEngine.Resolution GameResolution
        {
            get;
            set;
        }
    }
}