//-----------------------------------------------------------------------
// <copyright file="XmlFileHelper.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using UnityEngine;
    using UnityEditor;
    
    /// <summary>
    /// Represents helper operations on Scenes and Resolutions XML file.
    /// </summary>
    public static class XmlFileHelper
    {
        /// <summary>
        /// Path for saving, is relative to the Assets folder. 
        /// </summary>
        private static string resourcesDirectoryPath = "ScenesAndResolutions/Resources";

        /// <summary>
        /// Saves the specified <see cref="System.Xml.Linq.XElement"/> 
        /// to scenes and resolutions file.
        /// </summary>
        /// <param name="xmlFile"><see cref="ScenesAndResolutions.XmlFile"/> object.</param>
        /// <param name="xml"><see cref="System.Xml.Linq.XElement"/>  to save.</param>
        /// <returns><c>true</c> on success, <c>false</c> on failure.</returns>
        public static bool Save(this XmlFile xmlFile, XElement xml)
        {
            try
            {
                var path = XmlFileHelper.GetFilePathForSave();
                xml.Save(path);
                AssetDatabase.ImportAsset(XmlFileHelper.GetRelativeFilePathForSave());
                Log.Info(string.Format("Availbale scenes and resolutions were written to '{0}'.", path));
                return true;
            }
            catch (System.Exception ex)
            {
                if (ex is IOException ||
                    ex is PathTooLongException ||
                    ex is DirectoryNotFoundException ||
                    ex is System.NotSupportedException ||
                    ex is System.UnauthorizedAccessException ||
                    ex is System.FormatException ||
                    ex is System.ArgumentException ||
                    ex is System.InvalidOperationException ||
                    ex is XmlException)
                {
                    Log.Warning(string.Format("Failed to write availbale scenes and resolutions to file.\nException: {0}", ex.Message));
                    return false;
                }
                
                throw;
            }
        }
        
        /// <summary>
        /// Deletes scenes and resolutions file.
        /// </summary>
        public static void Delete(this XmlFile xmlFile)
        {
            try
            {
                var path = XmlFileHelper.GetFilePathForDelete();
                File.Delete(path);
                if (File.Exists(path))
                {
                    Log.Warning(string.Format("Failed to delete availbale scenes and resolutions file ('{0}').", path));
                }
                else
                {
                    Log.Info(string.Format("Availbale scenes and resolutions file ('{0}') was deleted.", path));
                }
            }
            catch (System.Exception ex)
            {
                if (ex is IOException ||
                    ex is PathTooLongException ||
                    ex is DirectoryNotFoundException ||
                    ex is System.NotSupportedException ||
                    ex is System.UnauthorizedAccessException ||
                    ex is System.FormatException ||
                    ex is System.ArgumentException ||
                    ex is System.InvalidOperationException)
                {
                    Log.Warning(string.Format("Failed to delete availbale scenes and resolutions file.\nException: {0}", ex.Message));
                }
                else
                {
                    throw;
                }
            }
        }
        #region Private methods
        /// <summary>
        /// Gets the file path for Save operation.
        /// </summary>
        /// <returns>The file path for save.</returns>
        private static string GetFilePathForSave()
        {
            var directory = Path.Combine(Application.dataPath, XmlFileHelper.resourcesDirectoryPath);
            Directory.CreateDirectory(directory);
            return Path.Combine(directory, XmlFile.FileName);
        }

        /// <summary>
        /// Gets the file path for Delete operation.
        /// </summary>
        /// <returns>The file path for delete.</returns>
        private static string GetFilePathForDelete()
        {
            var directory = Path.Combine(Application.dataPath, XmlFileHelper.resourcesDirectoryPath);
            return Path.Combine(directory, XmlFile.FileName);
        }
        
        /// <summary>
        /// Gets the relative file path.
        /// </summary>
        /// <returns>The relative file path.</returns>
        private static string GetRelativeFilePathForSave()
        {
            var directory = Path.Combine("Assets", XmlFileHelper.resourcesDirectoryPath);
            return Path.Combine(directory, XmlFile.FileName);
        }
        #endregion
    }
}
