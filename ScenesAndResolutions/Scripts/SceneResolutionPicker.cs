//-----------------------------------------------------------------------
// <copyright file="SceneResolutionPicker.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Xml.Linq;

    /// <summary>
    /// Represents scene resolution picker. It is intended to select 
    /// the best resolution from available for the particular scene 
    /// and append it to a scene name.
    /// </summary>
    public static class SceneResolutionPicker
    {
        /// <summary>
        /// Gets or sets the object, which contains available resolutions, available for scenes.
        /// </summary>
        public static IAvailableResolutions AvailableResolutions
        {
            get;
            set;
        }

        /// <summary>
        /// Selects a resolution from available for the specified scene and appends it to the scene name.
        /// The one is chosen, which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name with resolution.</returns>
        /// <param name="sceneName">Scene name.</param>
        public static string PickResolution(this string sceneName)
        {
            return sceneName.PickResolution(UnityEngine.Screen.width, UnityEngine.Screen.height, SceneMode.Fill);
        }

        /// <summary>
        /// Selects a resolution from available for the specified scene and appends it to the scene name.
        /// The one is chosen, which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name with resolution.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="mode">Desired scene scale mode.</param>
        public static string PickResolution(this string sceneName, SceneMode mode)
        {
            return sceneName.PickResolution(UnityEngine.Screen.width, UnityEngine.Screen.height, mode);
        }

        /// <summary>
        /// Selects a resolution from available for the specified scene and appends it to the scene name.
        /// The one is chosen, which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name with resolution.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="gameWidth">Game resolution width.</param>
        /// <param name="gameHeight">Game resolution height.</param>
        public static string PickResolution(this string sceneName, int gameWidth, int gameHeight)
        {
            return sceneName.PickResolution(gameWidth, gameHeight, SceneMode.Fill);
        }

        /// <summary>
        /// Selects a resolution from available for the specified scene and appends it to the scene name.
        /// The one is chosen, which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name with resolution.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="gameWidth">Game resolution width.</param>
        /// <param name="gameHeight">Game resolution height.</param>
        /// <param name="mode">Desired scene scale mode.</param>
        public static string PickResolution(this string sceneName, int gameWidth, int gameHeight, SceneMode mode)
        {
            if (sceneName == null)
            {
                return null;
            }

            if (AvailableResolutions == null)
            {
                SceneResolutionPicker.LoadAvailableResolutions();
            }

            var gameResolution = new UnityEngine.Resolution().Set(gameWidth, gameHeight);
            var best = AvailableResolutions.GetBest(sceneName, gameResolution, mode);
            Log.Info(string.Format("Sorted available resolutions for game resolution {0}x{1} (the first is the best):\n{2}", gameResolution.width, gameResolution.height, AvailableResolutions.ResolutionsListToString()));
            Log.Info(string.Format("Best available scene is: '{0}'.", best));
            return best;
        }

        /// <summary>
        /// Loads available resolutions.
        /// </summary>
        private static void LoadAvailableResolutions()
        {
            var xmlFile = new XmlFile();
            var xml = xmlFile.Load();
            var availableResolutions = new AvailableResolutions();
            if (availableResolutions.FromXml(xml))
            {
                Log.Info(string.Format("Availbale scenes and resolutions XML was succesfully parsed.\nAvailable scenes and resolutions:\n{0}", availableResolutions));
            }
            else
            {
                Log.Warning(string.Format("Failed to parse availbale scenes and resolutions XML.\nAvailable scenes and resolutions:\n{0}", availableResolutions));
            }
            
            AvailableResolutions = availableResolutions;
        }
    }
}
