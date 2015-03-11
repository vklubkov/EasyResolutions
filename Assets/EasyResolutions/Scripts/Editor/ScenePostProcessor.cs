//-----------------------------------------------------------------------
// <copyright file="ScenePostProcessor.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    using UnityEditor;
    using UnityEngine;
    
    /// <summary>
    /// Represents the scene post-processor, which automatically 
    /// saves available scenes and resolutions to file, when you
    /// run a scene in the Editor.
    /// </summary>
    public static class ScenePostProcessor
    {
        /// <summary>
        /// Indicates, whether available Easy Resolutions XML file 
        /// is up-to-date. Thus, prevents it from being created multiple 
        /// times (post processing runs for each loaded scene).
        /// </summary>
        private static bool isScenesFileUpToDate;
        
        /// <summary>
        /// Runs post-processing for a scene.
        /// </summary>
        [UnityEditor.Callbacks.PostProcessScene]
        public static void PostProcessScene()
        {
            if (!isScenesFileUpToDate && Application.isEditor)
            {
                var buildSettings = new EditorBuildSettings();
                var xml = buildSettings.GetScenesAndResolutions();
                if (xml != null)
                {
                    var xmlFile = new XmlFile();
                    if (xmlFile.Save(xml))
                    {
                        isScenesFileUpToDate = true;
                    }
                }
            }
        }
    }
}
