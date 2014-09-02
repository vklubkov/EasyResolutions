//-----------------------------------------------------------------------
// <copyright file="EditorBuildSettingsHelper.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.IO;
    using System.Text;
    using System.Xml.Linq;
    using UnityEditor;

    /// <summary>
    /// Editor build settings helper.
    /// </summary>
    public static class EditorBuildSettingsHelper
    {
        /// <summary>
        /// Gets available  scenes and resolutions from editor build settings.
        /// Only scenes, which are enabled in build settings, are listed.
        /// </summary>
        /// <returns>Available scenes and resolutions.</returns>
        /// <param name="editorBuildSettings">Editor build settings object.</param>
        public static XElement GetScenesAndResolutions(this EditorBuildSettings editorBuildSettings)
        {
            if (editorBuildSettings == null)
            {
                return null;
            }

            var availableResolutions = new AvailableResolutions();
            var enabledScenes = new StringBuilder("Enabled scenes in Build Settings:\n");
            foreach (var scene in EditorBuildSettings.scenes)
            {
                if (scene.enabled)
                {
                    var sceneName = Path.GetFileNameWithoutExtension(scene.path) ?? string.Empty;
                    enabledScenes.AppendLine(sceneName);
                    availableResolutions.Add(sceneName);
                }
            }

            Log.Info(enabledScenes.ToString());
            return availableResolutions.ToXml();
        }
    }
}
