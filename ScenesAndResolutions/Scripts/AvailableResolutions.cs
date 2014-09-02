//-----------------------------------------------------------------------
// <copyright file="AvailableResolutions.cs" company="https://github.com/marked-one/ScenesAndResolutionsPicker">
//     Copyright © 2014 Vladimir Klubkov. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace ScenesAndResolutions
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Xml;
    using System.Xml.Linq;

    /// <summary>
    /// Represents resolutions available for scenes.
    /// </summary>
    public class AvailableResolutions : IAvailableResolutions
    {
        /// <summary>
        /// Dictionary containing scenes with a HashSet of available resolutions for each one.
        /// </summary>
        private Dictionary<string, HashSet<Resolution>> scenesWithResolutions = new Dictionary<string, HashSet<Resolution>>();

        /// <summary>
        /// All resolutions, available for scenes.
        /// </summary>
        private List<Resolution> availableResolutions = new List<Resolution>();

        /// <summary>
        /// Initializes a new instance of the 
        /// <see cref="ScenesAndResolutions.AvailableResolutions"/> 
        /// class: creates the default resolution comparers.
        /// </summary>
        public AvailableResolutions()
        {
            this.FillComparer = new FillResolutionComparer();
            this.FitComparer = new FitResolutionComparer();
        }

        /// <summary>
        /// Gets or sets the fill resolution comparer.
        /// </summary>
        /// <value>The fill resolution comparer.</value>
        public IResolutionComparer FillComparer
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the fit resolution comparer.
        /// </summary>
        /// <value>The fit resolution comparer.</value>
        public IResolutionComparer FitComparer
        {
            get;
            set;
        }

        /// <summary>
        /// Adds a scene with resolution, which is specified as a string of the following format:
        /// <code> {scene name}.{width}x{height} </code>, example:
        /// <example>"TestScene.1366x768"</example>
        /// </summary>
        /// <returns><c>true</c>, if a scene with resolution was successfully added, <c>false</c> otherwise.</returns>
        /// <param name="sceneWithResolution">String containing scene name and its resolution.</param>
        public bool Add(string sceneWithResolution)
        { 
            if (string.IsNullOrEmpty(sceneWithResolution))
            {
                return false;
            }

            return this.ParseSceneWithResolution(sceneWithResolution);
        }

        /// <summary>  
        /// Add a scene with resolution, which is specified as scene name string and integer width and height.
        /// </summary>
        /// <returns><c>true</c>, if a scene with resolution was successfully added, <c>false</c> otherwise.</returns>
        /// <param name="sceneName">String containing scene name.</param>
        /// <param name="width">Scene resolution width.</param>
        /// <param name="height">Scene resolution height.</param>
        public bool Add(string sceneName, int width, int height)
        { 
            if (string.IsNullOrEmpty(sceneName))
            {
                return false;
            }

            var resolution = new Resolution(width, height);
            this.AddSceneWithResolution(sceneName, resolution);
            this.AddResolution(resolution);
            return true;
        }

        /// <summary>
        /// Gets the specified scene name with a resolution appended,
        /// which best fits the specified game resolution.
        /// </summary>
        /// <returns>Scene name string with the best resolution appended.</returns>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="gameResolution">Game resolution.</param>
        /// <param name="mode">Desired scene scale mode.</param>
        public string GetBest(string sceneName, UnityEngine.Resolution gameResolution, SceneMode mode)
        {
            if (!this.scenesWithResolutions.ContainsKey(sceneName))
            {
                return sceneName;
            }

            var comparer = this.GetComparer(mode);
            if (comparer != null)
            {
                comparer.GameResolution = gameResolution;
                this.availableResolutions.Sort(comparer);
            }

            var resolution = this.availableResolutions.FirstOrDefault(item => 
            {
                return this.scenesWithResolutions[sceneName].Contains(item);
            });

            if (resolution == null)
            {
                return sceneName;
            }

            return string.Format("{0}{1}", sceneName, resolution.ToExtension());
        }

        /// <summary>
        /// Deserializes <see cref="ScenesAndResolutions.AvailableResolutions"/> from <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <returns><c>true</c>, if deserialization was successful, <c>false</c> otherwise.</returns>
        /// <param name="root"><see cref="System.Xml.Linq.XElement"/> with available resolutions.</param>
        public bool FromXml(XElement root)
        {
            try
            {
                return this.ParseXml(root);
            }
            catch (System.Exception ex)
            {
                if (ex is System.FormatException ||
                    ex is System.ArgumentException ||
                    ex is System.InvalidOperationException ||
                    ex is XmlException)
                {
                    return false;
                }

                throw;
            }
        }

        /// <summary>
        /// Serializes <see cref="ScenesAndResolutions.AvailableResolutions"/> to XML.
        /// </summary>
        /// <returns><see cref="System.Xml.Linq.XElement"/> containing available resolutions, 
        /// if write was successful, <c>null</c> otherwise.</returns>
        public XElement ToXml()
        {
            try
            {
                return this.BuildXml();
            }
            catch (System.Exception ex)
            {
                if (ex is System.FormatException ||
                    ex is System.ArgumentException ||
                    ex is System.InvalidOperationException ||
                    ex is XmlException)
                {
                    return null;
                }

                throw;
            }
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents the current <see cref="ScenesAndResolutions.AvailableResolutions"/>.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the current <see cref="ScenesAndResolutions.AvailableResolutions"/>.</returns>
        public override string ToString()
        {
            var xml = this.ToXml();
            return (xml == null) ? string.Empty : xml.ToString();
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents 
        /// the list of the available resolutions.
        /// </summary>
        /// <returns>A <see cref="System.String"/> that represents the 
        /// list of the available resolutions.</returns>
        public string ResolutionsListToString()
        {
            return string.Join(",\n", this.availableResolutions.Select(resolution => resolution.ToString()).ToArray());
        }

        #region Private methods
        /// <summary>
        /// Parses the specified string containing scene name with resolution.
        /// </summary>
        /// <returns><c>true</c>, if the string containing the scene name with 
        /// resolution was successfully parsed, <c>false</c> otherwise.</returns>
        /// <param name="sceneWithResolution">String containing scene name and its resolution.</param>
        private bool ParseSceneWithResolution(string sceneWithResolution)
        {
            var tokens = sceneWithResolution.Split('.');
            if (tokens.Length < 1)
            {
                return false;
            }

            var sceneName = tokens[0];
            var resolution = this.GetResolutionFromTokens(tokens);
            this.AddSceneWithResolution(sceneName, resolution);
            this.AddResolution(resolution);
            return true;
        }

        /// <summary>
        /// Gets the resolution from tokens.
        /// </summary>
        /// <returns>The extracted resolution.</returns>
        /// <param name="tokens">Tokens, possibly containing the resolution.</param>
        private Resolution GetResolutionFromTokens(string[] tokens)
        {
            for (var i = 1; i < tokens.Length; i++)
            {
                var resolution = new Resolution();
                if (resolution.FromExtension(tokens[i]))
                {
                    return resolution;
                }
            }
            
            return new Resolution();
        }

        /// <summary>
        /// Adds the resolution for the specified scene, but only if this resolution 
        /// is not already present among the resolutions, available for this scene.
        /// </summary>
        /// <param name="sceneName">Scene name.</param>
        /// <param name="resolution">Scene resolution.</param>
        private void AddSceneWithResolution(string sceneName, Resolution resolution)
        {
            if (!this.scenesWithResolutions.ContainsKey(sceneName))
            {
                this.scenesWithResolutions.Add(sceneName, new HashSet<Resolution>());
            }
            
            if (!this.scenesWithResolutions[sceneName].Contains(resolution))
            {
                this.scenesWithResolutions[sceneName].Add(resolution);
            }
        }

        /// <summary>
        /// Adds the specified resolution to resolutions list, 
        /// but only if it is not present in the list already.
        /// </summary>
        /// <param name="resolution">Resolution to add.</param>
        private void AddResolution(Resolution resolution)
        {
            if (!this.availableResolutions.Contains(resolution))
            {
                this.availableResolutions.Add(resolution);
            }
        }

        /// <summary>
        /// Selects the appropriate comparer depending on the specified scene mode.
        /// </summary>
        /// <returns>The chosen comparer.</returns>
        /// <param name="mode">Scene mode.</param>
        private IResolutionComparer GetComparer(SceneMode mode)
        {
            if (mode == SceneMode.Fill)
            {
                return this.FillComparer;
            }

            if (mode == SceneMode.Fit)
            {
                return this.FitComparer;
            }

            return null;
        }

        /// <summary>
        /// Parses XML data.
        /// </summary>
        /// <returns><c>true</c>, successfully parsed, <c>false</c> otherwise.</returns>
        /// <param name="root">Root <see cref="System.Xml.Linq.XElement"/>.</param>
        private bool ParseXml(XElement root)
        {
            if (root == null)
            {
                return false;
            }

            if (!"ScenesAndResolutions".Equals(root.Name.LocalName))
            {
                return false;
            }
            
            var scenes = root.Element("Scenes");
            if (!this.ScenesDictionaryFromXml(scenes))
            {
                this.scenesWithResolutions.Clear();
                return false;
            }
            
            var resolutions = root.Element("Resolutions");
            if (!this.ResolutionsListFromXml(resolutions))
            {
                this.scenesWithResolutions.Clear();
                this.availableResolutions.Clear();
                return false;
            }
            
            return true;
        }

        /// <summary>
        /// Reads scenes dictionary from <see cref="System.Xml.Linq.XContainer"/>.
        /// </summary>
        /// <returns><c>true</c>, if read was successful, <c>false</c> otherwise.</returns>
        /// <param name="scenesElement"><see cref="System.Xml.Linq.XContainer"/> containing scenes dictionary.</param>
        private bool ScenesDictionaryFromXml(XContainer scenesElement)
        {
            if (scenesElement == null)
            {
                return false;
            }

            var sceneElements = scenesElement.Elements("Scene");
            if (sceneElements == null)
            {
                return false;
            }

            foreach (var sceneElement in sceneElements)
            {
                var nameAttribute = sceneElement.Attribute("name");
                if (nameAttribute == null)
                {
                    return false;
                }

                var name = nameAttribute.Value ?? string.Empty;
                if (!name.Any())
                {
                    return false;
                }

                var resolutionElements = sceneElement.Elements("Resolution");
                if (resolutionElements == null)
                {
                    return false;
                }

                foreach (var resolutionElement in resolutionElements)
                {
                    var resolution = new Resolution();
                    if (!resolution.FromXml(resolutionElement))
                    {
                        return false;
                    }
                
                    this.AddSceneWithResolution(name, resolution);
                }
            }
            
            return true;
        }

        /// <summary>
        /// Reads resolutions list from <see cref="System.Xml.Linq.XContainer"/>.
        /// </summary>
        /// <returns><c>true</c>, if read was successful, <c>false</c> otherwise.</returns>
        /// <param name="resolutionsElement"><see cref="System.Xml.Linq.XContainer"/> containing list of resolutions.</param>
        private bool ResolutionsListFromXml(XContainer resolutionsElement)
        {
            if (resolutionsElement == null)
            {
                return false;
            }
            
            var resolutionElements = resolutionsElement.Elements("Resolution");
            if (resolutionElements == null)
            {
                return false;
            }
            
            foreach (var resolutionElement in resolutionElements)
            {
                var resolution = new Resolution();
                if (!resolution.FromXml(resolutionElement))
                {
                    return false;
                }
                
                this.AddResolution(resolution);
            }
            
            return true;
        }

        /// <summary>
        /// Builds the xml representation of available resolution.
        /// </summary>
        /// <returns><see cref="System.Xml.Linq.XElement"/> containing the representation of available resolutions.</returns>
        private XElement BuildXml()
        {
            var root = new XElement("ScenesAndResolutions");
            var scenes = this.ScenesDictionaryToXml();
            var resolutions = this.ResolutionsListToXml();
            root.Add(scenes);
            root.Add(resolutions);
            return root;
        }
            
        /// <summary>
        /// Writes scenes dictionary to <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <returns><see cref="System.Xml.Linq.XElement"/> containing scenes and their resolutions.</returns>
        private XElement ScenesDictionaryToXml()
        {
            var scenesElement = new XElement("Scenes");
            foreach (var key in this.scenesWithResolutions.Keys)
            {
                var sceneElement = new XElement("Scene", new XAttribute("name", key));
                var sceneResolutions = this.scenesWithResolutions[key];
                foreach (var resolution in sceneResolutions)
                {
                    var resoltuionElement = resolution.ToXml();
                    sceneElement.Add(resoltuionElement);
                }

                scenesElement.Add(sceneElement);
            }

            return scenesElement;
        }
        
        /// <summary>
        /// Writes resolutions list to <see cref="System.Xml.Linq.XElement"/>.
        /// </summary>
        /// <returns><see cref="System.Xml.Linq.XElement"/> containing list of resolutions.</returns>
        private XElement ResolutionsListToXml()
        {
            var resolutionsElement = new XElement("Resolutions");
            foreach (var resolution in this.availableResolutions)
            {
                var resoltuionElement = resolution.ToXml();
                resolutionsElement.Add(resoltuionElement);
            }

            return resolutionsElement;
        }
        #endregion
    }
}
