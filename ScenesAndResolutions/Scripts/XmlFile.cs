//-----------------------------------------------------------------------
// <copyright file="XmlFile.cs" company="">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.IO;
    using System.Xml;
    using System.Xml.Linq;
    using UnityEngine;

    /// <summary>
    /// Represents operations on Scenes and Resolutions XML file.
    /// </summary>
    public class XmlFile
    {
        /// <summary>
        /// The name of scenes and resolutions file.
        /// </summary>
        private static string fileName = "scenesandresolutions.xml";

        /// <summary>
        /// Gets or sets scenes and resolutions file name.
        /// </summary>
        /// <value>File name.</value>
        public static string FileName
        {
            get { return fileName; }
            set { fileName = value; }
        }

        /// <summary>
        /// Loads scenes and resolutions from file to a
        /// <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <returns><see cref="System.Xml.Linq.XElement"/> containing the 
        /// contents of the file on success, <c>null</c> on failure.</returns>
        public XElement Load()
        {
            try
            {
                var path = XmlFile.GetFilePathForLoad();
                var xmlData = Resources.Load<TextAsset>(path);
                if (xmlData == null)
                {
                    return null;
                }
                
                var xml = XElement.Parse(xmlData.text);
                Log.Info(string.Format("Availbale scenes and resolutions were loaded from '{0}' file.", path));
                return xml;
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
                    Log.Warning(string.Format("Failed to load availbale scenes and resolutions from file.\nException: {0}", ex.Message));
                    return null;
                }
                
                throw;
            }
        }
        #region Private methods
        /// <summary>
        /// Gets the file path for Load operation.
        /// </summary>
        /// <returns>The file path for load.</returns>
        private static string GetFilePathForLoad()
        {
            var directory = Path.GetDirectoryName(XmlFile.fileName) ?? string.Empty;
            var file = Path.GetFileNameWithoutExtension(XmlFile.fileName) ?? string.Empty;
            return Path.Combine(directory, file);
        }
        #endregion
    }
}
