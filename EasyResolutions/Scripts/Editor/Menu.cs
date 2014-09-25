//-----------------------------------------------------------------------
// <copyright file="Menu.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace EasyResolutions
{
    using UnityEditor;
    
    /// <summary>
    /// Represents Easy Resolutions menu.
    /// </summary>
    public static class Menu
    {
        /// <summary>
        /// Generates the file with available scenes and resolutions, when 
        /// <c>Assets/Create/Easy Resolutions XML file</c> menu item is used.
        /// </summary>
        [MenuItem("Assets/Create/Easy Resolutions XML file", false, 10000)]
        public static void GenerateFile()
        {
            var buildSettings = new EditorBuildSettings();
            var root = buildSettings.GetScenesAndResolutions();
            if (root != null)
            {
                var xmlFile = new XmlFile();
                xmlFile.Save(root);
            }
        }
    }
}
