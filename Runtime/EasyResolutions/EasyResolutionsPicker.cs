using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace EasyResolutions {
    public static class EasyResolutionsPicker {
#if UNITY_EDITOR
        static string ActiveScenePath {
            get => SessionState.GetString("EasyResolutions.ActiveScenePath", null);
            set => SessionState.SetString("EasyResolutions.ActiveScenePath", value);
        }

        static int ActiveSceneBuildIndex {
            get => SessionState.GetInt("EasyResolutions.ActiveSceneBuildIndex", -1);
            set => SessionState.SetInt("EasyResolutions.ActiveSceneBuildIndex", value);
        }

        static EasyResolutionsPicker() =>
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;

        static void OnPlayModeStateChanged(PlayModeStateChange change) {
            if (change != PlayModeStateChange.ExitingEditMode)
                return;

            ActiveScenePath = null;
            ActiveSceneBuildIndex = -1;
            EditorSceneManager.playModeStartScene = null;

            var settings = EasyResolutions_Settings.Instance;
            if (settings == null)
                return;

            // No need to change the scene when no need to pick resolution
            if (!settings.RunStartingSceneInsteadOfActiveScene && !settings.PickResolutionForActiveScene)
                return;

            var activeScene = SceneManager.GetActiveScene();
            var activeScenePath = activeScene.path;
            var activeSceneBuildIndex = activeScene.buildIndex;

            // The scene is probably not saved as asset,  no need to pick resolution
            if (!settings.RunStartingSceneInsteadOfActiveScene &&
                settings.PickResolutionForActiveScene &&
                (string.IsNullOrEmpty(activeScenePath) || activeSceneBuildIndex == -1))
                return;

            if (settings.ZeroScene is not ParsedScenePath scenePath)
                return;

            ActiveScenePath = activeScenePath;
            ActiveSceneBuildIndex = activeSceneBuildIndex;
            EditorSceneManager.playModeStartScene = AssetDatabase.LoadAssetAtPath<SceneAsset>(scenePath.FullPath);
        }
#endif

        // Override the starting scene
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        static void OnBeforeSceneLoad() {
#if UNITY_EDITOR
            var settings = EasyResolutions_Settings.Instance;
#else
            var settings = ISettings.Instance;
#endif

            if (settings == null) {
                Debug.LogWarning("EasyResolutions: settings not provided. " +
                                 "Please check Preloaded Assets in Project Settings.");

                return;
            }

#if UNITY_EDITOR
            if (settings.RunStartingSceneInsteadOfActiveScene) {
                // Do nothing if Starting Scene is missing for some reason.
                if (string.IsNullOrEmpty(settings.StartingSceneGroup)) {
                    Debug.LogWarning("EasyResolutions: Starting Scene group not provided. " +
                                     "Please check your Easy Resolutions Settings.");

                    return;
                }

                SceneManager.LoadScene(settings.StartingSceneGroup.PickResolution());
                return;
            }

            // No need to do anything, load Active scene normally
            if (!settings.PickResolutionForActiveScene)
                return;

            var activeScenePath = ActiveScenePath;
            var buildIndex = ActiveSceneBuildIndex;

            // The scene is likely not saved as asset, do nothing
            if (string.IsNullOrEmpty(activeScenePath) || buildIndex == -1)
                return;

            var indexNotInBuild = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .ToArray().Length;

            // If not in Build, can't pick resolution, do nothing.
            if (buildIndex == indexNotInBuild)
                return;

            var sceneInfo = new ParsedScenePath(activeScenePath, buildIndex);
            SceneManager.LoadScene(sceneInfo.Group.PickResolution());
#else
            // Do nothing if Starting Scene is missing for some reason.
            if (string.IsNullOrEmpty(settings.StartingSceneGroup)) {
                Debug.LogWarning("EasyResolutions: Starting Scene group not provided. " +
                                 "Please check your Easy Resolutions Settings.");

                return;
            }

            // In Build, Unity automatically loads scene with index 0 no matter what.
            // So no need to do anything if it is selected as a Starting Scene.
            if (settings.ZeroScene.Group == settings.StartingSceneGroup)
                return;

            SceneManager.LoadScene(settings.StartingSceneGroup.PickResolution());
#endif
        }

        /// Adds scenes to the Scene Map of EasyResolutions_Settings.
        /// <param name="scenes">list of scene paths or names</param>
        public static void AddScenes(List<string> scenes) {
            if (scenes == null || scenes.Count == 0) {
                Debug.LogWarning("EasyResolutions: no scenes to add.");
                return;
            }

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return;

            foreach (var scene in scenes) {
                var scenePath = new ParsedScenePath(scene);
                if (settings.Scenes.TryGetValue(scenePath.Group, out var group))
                    group.Add(scenePath);
                else
                    settings.Scenes.Add(scenePath.Group, new List<IParsedString> { scenePath });
            }
        }

        /// Picks the scene path for a scene in Build that best matches the current
        /// game window resolution. Uses the comparer specified in EasyResolutions_Settings
        /// for the group in the Scene Map, otherwise the default comparer.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public static string PickResolution(this string source) => source.PickResolution(Screen.width, Screen.height);

        /// Picks the scene path for a scene in Build that best matches the specified
        /// resolution. Uses the comparer specified in EasyResolutions_Settings for the
        /// group in the Scene Map, otherwise the default comparer.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public static string PickResolution(this string source,
                                            int targetWidth,
                                            int targetHeight) {
            if (!ValidateParameters(source, targetWidth, targetHeight))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, settings.Scenes, "unity");
            if (group == null || group.Count == 0)
                return source;

            var groupName = group[0].Group;
            var comparer = GetComparer(groupName, settings.Comparers, settings.DefaultComparer);
            if (!ValidateGroupComparer<string>(null, groupName, comparer))
                return source;

            return PickBestResolution(targetWidth, targetHeight, comparer, group, settings.Epsilon);
        }

        /// Picks the scene path for a scene in Build that best matches
        /// the current game window resolution.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public static string PickResolution(this string source,
                                            IEasyResolutionsComparer overrideComparer) {
            if (!ValidateParameters(source, overrideComparer))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, settings.Scenes, "unity");
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(Screen.width, Screen.height, overrideComparer, group, settings.Epsilon);
        }

        /// Picks the scene path for a scene in Build that best matches
        /// the specified resolution.
        /// <param name="source">ideally, the key of a group in the Scene Map,
        /// but can be the path or the name of a scene in that group.</param>
        /// <param name="targetWidth">desired width</param>
        /// <param name="targetHeight">desired height</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked scene path, or source if unable to pick</returns>
        public static string PickResolution(this string source,
                                            int targetWidth,
                                            int targetHeight,
                                            IEasyResolutionsComparer overrideComparer) {
            if (!ValidateParameters(source, targetWidth, targetHeight, overrideComparer))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, settings.Scenes, "unity");
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(targetWidth, targetHeight, overrideComparer, group, settings.Epsilon);
        }

        /// Picks a string from a group in the String Map with the specified identifier
        /// that best matches the current game window resolution. Uses the comparer specified
        /// for the group in the Scene Map, otherwise the Comparer Override in that same
        /// Scene Map asset, otherwise the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public static string PickResolution<T>(this string source, T identifier) =>
            source.PickResolution(identifier, Screen.width, Screen.height);

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
        public static string PickResolution<T>(this string source,
                                               T identifier,
                                               int targetWidth,
                                               int targetHeight) {
            if (!ValidateParameters(source, targetWidth, targetHeight))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var stringMap = GetStringMap(identifier, settings);
            if (!ValidateStringMap(stringMap, identifier))
                return source;

            var group = GetGroup(source, stringMap.Strings, stringMap.Extension);
            if (group == null || group.Count == 0)
                return source;

            var groupName = group[0].Group;
            var comparer = GetComparer(groupName, stringMap.Comparers, stringMap.ComparerOverride, settings.DefaultComparer);
            if (!ValidateGroupComparer(identifier, groupName, comparer))
                return source;

            return PickBestResolution(targetWidth, targetHeight, comparer, group, settings.Epsilon);
        }

        /// Picks a string from a group in the String Map with the specified
        /// identifier that best matches the current game window resolution.
        /// <param name="source">ideally, the key of a group in the String Map,
        /// but can be a string in that group.</param>
        /// <param name="identifier">String Map asset identifier</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public static string PickResolution<T>(this string source,
                                               T identifier,
                                               IEasyResolutionsComparer overrideComparer) {
            if (!ValidateParameters(source, overrideComparer))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var stringMap = GetStringMap(identifier, settings);
            if (!ValidateStringMap(stringMap, identifier))
                return source;

            var group = GetGroup(source, stringMap.Strings, stringMap.Extension);
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(Screen.width, Screen.height, overrideComparer, group, settings.Epsilon);
        }

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
        public static string PickResolution<T>(this string source,
                                               T identifier,
                                               int targetWidth,
                                               int targetHeight,
                                               IEasyResolutionsComparer overrideComparer) {
            if (!ValidateParameters(source, targetWidth, targetHeight, overrideComparer))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var stringMap = GetStringMap(identifier, settings);
            if (!ValidateStringMap(stringMap, identifier))
                return source;

            var group = GetGroup(source, stringMap.Strings, stringMap.Extension);
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(targetWidth, targetHeight, overrideComparer, group, settings.Epsilon);
        }

        /// Picks a string from a group in the specified string dictionary
        /// that best matches the current game window resolution.
        /// Uses the default comparer of EasyResolutions_Settings.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public static string PickResolution(this string source,
                                            Dictionary<string, List<IParsedString>> stringMap,
                                            string extension = "unity") {
            if (!ValidateParameters(source, stringMap))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, stringMap, extension);
            if (group == null || group.Count == 0)
                return source;

            var groupName = group[0].Group;
            var comparer = settings.DefaultComparer;
            if (!ValidateGroupComparer<string>(null, groupName, comparer))
                return source;

            return PickBestResolution(Screen.width, Screen.height, comparer, group, settings.Epsilon);
        }

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
        public static string PickResolution(this string source,
                                            int targetWidth,
                                            int targetHeight,
                                            Dictionary<string, List<IParsedString>> stringMap,
                                            string extension = "unity") {
            if (!ValidateParameters(source, targetWidth, targetHeight, stringMap))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, stringMap, extension);
            if (group == null || group.Count == 0)
                return source;

            var groupName = group[0].Group;
            var comparer = settings.DefaultComparer;
            if (!ValidateGroupComparer<string>(null, groupName, comparer))
                return source;

            return PickBestResolution(targetWidth, targetHeight, comparer, group, settings.Epsilon);
        }

        /// Picks a string from a group in the specified string dictionary
        /// that best matches the current game window resolution.
        /// <param name="source">ideally, the key of a group in the groups
        /// dictionary, but can be a string in that group.</param>
        /// <param name="overrideComparer">an object implementing the
        /// IEasyResolutionsComparer interface, is used as resolution comparer</param>
        /// <param name="stringMap">a dictionary containing groups of strings</param>
        /// <param name="extension">optional extension, should match the extension of `name`, if any</param>
        /// <returns>Picked string or source if unable to pick</returns>
        public static string PickResolution(this string source,
                                            IEasyResolutionsComparer overrideComparer,
                                            Dictionary<string, List<IParsedString>> stringMap,
                                            string extension = "unity") {
            if (!ValidateParameters(source, overrideComparer, stringMap))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, stringMap, extension);
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(Screen.width, Screen.height, overrideComparer, group, settings.Epsilon);
        }

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
        public static string PickResolution(this string source,
                                            int targetWidth,
                                            int targetHeight,
                                            IEasyResolutionsComparer overrideComparer,
                                            Dictionary<string, List<IParsedString>> stringMap,
                                            string extension = "unity") {
            if (!ValidateParameters(source, targetWidth, targetHeight, overrideComparer, stringMap))
                return source;

            var settings = ISettings.Instance;
            if (!ValidateSettings(settings))
                return source;

            var group = GetGroup(source, stringMap, extension);
            if (group == null || group.Count == 0)
                return source;

            return PickBestResolution(targetWidth, targetHeight, overrideComparer, group, settings.Epsilon);
        }

        static bool ValidateParameters(string name, int targetWidth, int targetHeight,
            IEasyResolutionsComparer comparer, Dictionary<string, List<IParsedString>> nameGroups) =>
            ValidateParameters(name, targetWidth, targetHeight, comparer) && ValidateParameters(nameGroups);

        static bool ValidateParameters(
            string name, int targetWidth, int targetHeight, IEasyResolutionsComparer comparer) =>
            ValidateParameters(name, targetWidth, targetHeight) && ValidateParameters(comparer);

        static bool ValidateParameters(
            string name, IEasyResolutionsComparer comparer, Dictionary<string, List<IParsedString>> nameGroups) =>
            ValidateParameters(name, comparer) && ValidateParameters(nameGroups);

        static bool ValidateParameters(string name, IEasyResolutionsComparer comparer) =>
            ValidateParameters(name) && ValidateParameters(comparer);

        static bool ValidateParameters(string name, Dictionary<string, List<IParsedString>> nameGroups) =>
            ValidateParameters(name) && ValidateParameters(nameGroups);

        static bool ValidateParameters(
            string name, int targetWidth, int targetHeight, Dictionary<string, List<IParsedString>> nameGroups) =>
            ValidateParameters(name, targetWidth, targetHeight) && ValidateParameters(nameGroups);

        static bool ValidateParameters(string name, int targetWidth, int targetHeight) =>
            ValidateParameters(name) && ValidateParameters(targetWidth, targetHeight);

        static bool ValidateParameters(string name) {
            if (!string.IsNullOrEmpty(name))
                return true;

            Debug.LogWarning("EasyResolutions: name not specified.");
            return false;
        }

        static bool ValidateParameters(int targetWidth, int targetHeight) {
            if (targetWidth > 0 && targetHeight > 0)
                return true;

            Debug.LogWarning($"EasyResolutions: target resolution `{targetWidth}x{targetHeight}` is invalid.");
            return false;
        }

        static bool ValidateParameters(IEasyResolutionsComparer comparer) {
            if (comparer != null)
                return true;

            Debug.LogWarning("EasyResolutions: comparer not specified.");
            return false;
        }

        static bool ValidateParameters(Dictionary<string, List<IParsedString>> nameGroups) {
            if (nameGroups != null && nameGroups.Count != 0)
                return true;

            Debug.LogWarning("EasyResolutions: nameGroups dictionary not specified.");
            return false;
        }

        static bool ValidateSettings(ISettings settings) {
            if (settings != null)
                return true;

            Debug.LogWarning(
                "EasyResolutions: settings are not provided or not yet initialized." +
                "Please check Preloaded Assets list in Project Settings. If you use " +
                "EasyResolutions before the first scene is loaded, please do this not " +
                "early than in `RuntimeInitializeLoadType.AfterAssembliesLoaded` " +
                "(https://docs.unity3d.com/6000.0/Documentation/ScriptReference/RuntimeInitializeLoadType.html).");

            return false;
        }

        static List<IParsedString> GetGroup(
            string name, Dictionary<string, List<IParsedString>> stringGroups, string extension) {
            if (string.IsNullOrEmpty(name))
                return null;

            if (stringGroups.TryGetValue(name, out var group))
                return group;

            extension ??= string.Empty;
            if (!extension.StartsWith("."))
                extension = $".{extension}";

            if (name.EndsWith(extension))
                name = name[..^extension.Length];

            name = name.ParseResolution(out _, out _);
            if (stringGroups.TryGetValue(name, out group))
                return group;

            name = stringGroups.Keys.FirstOrDefault(key => key.EndsWith(name));
            if (name != null && stringGroups.TryGetValue(name, out group))
                return group;

            return null;
        }

        static IEasyResolutionsComparer GetComparer(
            string groupName, Dictionary<string, IEasyResolutionsComparer> groupComparers,
            IEasyResolutionsComparer defaultComparer) {
            var groupComparer = groupComparers?.GetValueOrDefault(groupName);
            if (groupComparer != null)
                return groupComparer;

            return defaultComparer;
        }

        static IEasyResolutionsComparer GetComparer(
            string groupName, Dictionary<string, IEasyResolutionsComparer> groupComparers,
            IEasyResolutionsComparer stringMapComparer, IEasyResolutionsComparer defaultComparer) {
            var groupComparer = groupComparers?.GetValueOrDefault(groupName);
            if (groupComparer != null)
                return groupComparer;

            if (stringMapComparer != null)
                return stringMapComparer;

            return defaultComparer;
        }

        static bool ValidateGroupComparer<T>(T identifier, string groupName, IEasyResolutionsComparer comparer) {
            if (comparer != null)
                return true;

            Debug.LogWarning(identifier == null
                ? $"EasyResolutions: comparer not found for group `{groupName}`."
                : $"EasyResolutions: comparer not found for group `{groupName}` in `{identifier}` string map.");

            return false;
        }

        static string PickBestResolution(int targetWidth,
                                         int targetHeight,
                                         IEasyResolutionsComparer comparer,
                                         List<IParsedString> stringGroup,
                                         double epsilon) {
            if (stringGroup.Count == 1)
                return stringGroup[0].Value;

            comparer.Target = (targetWidth, targetHeight);
            comparer.Epsilon = epsilon;
            stringGroup.Sort(comparer);
            return stringGroup[0].Value;
        }

        static IStringMap<T> GetStringMap<T>(T identifier, ISettings settings) {
            foreach (var stringMapBase in settings.StringMaps) {
                if (stringMapBase is IStringMap<T> stringMap &&
                    EqualityComparer<T>.Default.Equals(identifier, stringMap.Identifier)) {
                    return stringMap;
                }
            }

            return null;
        }

        static bool ValidateStringMap<T>(IStringMap<T> stringMap, T identifier) {
            if (stringMap != null)
                return true;

            Debug.LogWarning($"EasyResolutions: string map with identifier `{identifier}` not found.");
            return false;
        }
    }
}