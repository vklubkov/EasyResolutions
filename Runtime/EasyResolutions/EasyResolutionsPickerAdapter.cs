using System.Collections.Generic;

namespace EasyResolutions {
    /// Adapter for use with dependency injection.
    /// Bind this class in your container to inject it anywhere.
    public class EasyResolutionsPickerAdapter : IEasyResolutionsPicker {
        /// Adds scenes to the Scene Map of EasyResolutions_Settings.
        /// <param name="scenes">list of scene paths or names</param>
        public void AddScenes(List<string> scenes) => EasyResolutionsPicker.AddScenes(scenes);

        /// Picks the scene path for a scene in Build that best matches the current
        /// game window resolution. Uses the comparer specified in EasyResolutions_Settings
        /// for the group in the Scene Map, otherwise the default comparer.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public string PickResolution(string source) => source.PickResolution();

        /// Picks the scene path for a scene in Build that best matches the specified
        /// resolution. Uses the comparer specified in EasyResolutions_Settings for the
        /// group in the Scene Map, otherwise the default comparer.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public string PickResolution(string source, int targetWidth, int targetHeight) =>
            source.PickResolution(targetWidth, targetHeight);

        /// Picks the scene path for a scene in Build that best matches
        /// the current game window resolution.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public string PickResolution(string source, IEasyResolutionsComparer overrideComparer) =>
            source.PickResolution(overrideComparer);

        /// Picks the scene path for a scene in Build that best matches
        /// the specified resolution.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public string PickResolution(
            string source, int targetWidth, int targetHeight, IEasyResolutionsComparer overrideComparer) =>
            source.PickResolution(targetWidth, targetHeight, overrideComparer);

        /// Picks a string from a group in the String Map with the specified identifier
        /// that best matches the current game window resolution. Uses the comparer specified
        /// for the group in the Scene Map, otherwise the Comparer Override in that same
        /// Scene Map asset, otherwise the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution<T>(string source, T identifier) => source.PickResolution(identifier);

        /// Picks a string from a group in the String Map with the specified identifier
        /// that best matches the specified resolution. Uses the comparer specified for
        /// the group in the Scene Map, otherwise the Comparer Override in that same
        /// Scene Map asset, otherwise the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution<T>(string source, T identifier, int targetWidth, int targetHeight) =>
            source.PickResolution(identifier, targetWidth, targetHeight);

        /// Picks a string from a group in the String Map with the specified
        /// identifier that best matches the current game window resolution.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution<T>(string source, T identifier, IEasyResolutionsComparer overrideComparer) =>
            source.PickResolution(identifier, overrideComparer);

        /// Picks a string from a group in the String Map with the specified
        /// identifier that best matches the specified resolution.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution<T>(string source,
                                        T identifier,
                                        int targetWidth,
                                        int targetHeight,
                                        IEasyResolutionsComparer overrideComparer) =>
            source.PickResolution(identifier, targetWidth, targetHeight, overrideComparer);

        /// Picks a string from a group in the specified string dictionary
        /// that best matches the current game window resolution.
        /// Uses the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution(string source,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity") => source.PickResolution(stringMap, extension);

        /// Picks a string from a group in the specified string
        /// dictionary that best matches the specified resolution.
        /// Uses the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity") => 
            source.PickResolution(targetWidth, targetHeight, stringMap, extension);

        /// Picks a string from a group in the specified string dictionary
        /// that best matches the current game window resolution.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution(string source,
                                     IEasyResolutionsComparer overrideComparer,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity") => 
            source.PickResolution(overrideComparer, stringMap, extension);

        /// Picks a string from a group in the specified string
        /// dictionary that best matches the specified resolution.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight,
                                     IEasyResolutionsComparer overrideComparer,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity") =>
            source.PickResolution(targetWidth, targetHeight, overrideComparer, stringMap, extension);
    }
}