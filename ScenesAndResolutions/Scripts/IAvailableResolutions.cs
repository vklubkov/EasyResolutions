//-----------------------------------------------------------------------
// <copyright file="IAvailableResolutions.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    /// <summary>
    /// Represents available scenes and resolutions interface.
    /// </summary>
    public interface IAvailableResolutions
    {
        /// <summary>
        /// Gets the specified scene name with a resolution appended,
        /// which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name string with the best resolution appended.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="gameResolution">Game resolution.</param>
        /// <param name="mode">Desired scene scale mode.</param>
        string GetBest(string sceneName, UnityEngine.Resolution gameResolution, SceneMode mode);

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the list of available resolutions.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the list of available resolutions.</returns>
        string ResolutionsListToString();
    }
}
