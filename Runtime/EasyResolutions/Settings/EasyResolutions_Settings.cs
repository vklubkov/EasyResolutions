using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace EasyResolutions {
    [CreateAssetMenu(fileName = "EasyResolutions_Settings", menuName = "Easy Resolutions/Settings")]
    // ReSharper disable InconsistentNaming because it matches the asset name
    internal class EasyResolutions_Settings : ScriptableObject, ISettings, ISerializationCallbackReceiver {
        // ReSharper restore InconsistentNaming
#if UNITY_EDITOR
        public static string DefaultComparerPropertyName => nameof(_defaultComparer);
        public static string EpsilonForAspectRatioComparisonPropertyName => nameof(_epsilonForAspectRatioComparison);
        public static string StartingSceneGroupPropertyName => nameof(_startingSceneGroup);
        public static string SceneListPropertyName => nameof(_sceneList);
        public static string GroupComparerKeysPropertyName => nameof(_groupComparerKeys);
        public static string GroupComparerValuesPropertyName => nameof(_groupComparerValues);
        public static string StringMapsPropertyName => nameof(_stringMaps);

        public static string VersionPropertyName => nameof(_version);
        public static string BuildPlayerProcessorOrderPropertyName => nameof(_buildPlayerProcessorOrder);

        public static string RunStartingSceneInsteadOfOpenScenePropertyName =>
            nameof(_runStartingSceneInsteadOfActiveScene);

        public static string PickResolutionForRunningScenePropertyName => nameof(_pickResolutionForActiveScene);
        public static string ScenesFoldoutValuePropertyName => nameof(_scenesFoldoutValue);
        public static string GroupFoldoutKeysPropertyName => nameof(_groupFoldoutKeys);
        public static string GroupFoldoutValuesPropertyName => nameof(_groupFoldoutValues);
        public static string SceneFoldoutKeysPropertyName => nameof(_sceneFoldoutKeys);
        public static string SceneFoldoutValuesPropertyName => nameof(_sceneFoldoutValues);
#endif

        [SerializeField] EasyResolutionsComparer _defaultComparer;
        [SerializeField] double _epsilonForAspectRatioComparison = 0.008;
        [SerializeField] string _startingSceneGroup;
        [SerializeField, HideInInspector] List<ParsedScenePath> _sceneList;
        [SerializeField, HideInInspector] List<string> _groupComparerKeys;
        [SerializeField, HideInInspector] List<EasyResolutionsComparer> _groupComparerValues;
        [SerializeField] List<EasyResolutionsStringMapBase> _stringMaps;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] int _version;
        [SerializeField] int _buildPlayerProcessorOrder;
        [SerializeField] bool _runStartingSceneInsteadOfActiveScene;
        [SerializeField] bool _pickResolutionForActiveScene;
        [SerializeField, HideInInspector] bool _scenesFoldoutValue;
        [SerializeField, HideInInspector] List<string> _groupFoldoutKeys;
        [SerializeField, HideInInspector] List<bool> _groupFoldoutValues;
        [SerializeField, HideInInspector] List<string> _sceneFoldoutKeys;
        [SerializeField, HideInInspector] List<bool> _sceneFoldoutValues;

        public int BuildPlayerProcessorOrder => _buildPlayerProcessorOrder;
        public bool RunStartingSceneInsteadOfActiveScene => _runStartingSceneInsteadOfActiveScene;
        public bool PickResolutionForActiveScene => _pickResolutionForActiveScene;
#endif

        public double Epsilon => _epsilonForAspectRatioComparison;

        public IParsedString ZeroScene { get; private set; }
        public string StartingSceneGroup => _startingSceneGroup;
        public Dictionary<string, List<IParsedString>> Scenes { get; } = new();

        public IEasyResolutionsComparer DefaultComparer => _defaultComparer;
        public Dictionary<string, IEasyResolutionsComparer> Comparers { get; } = new();

        public List<EasyResolutionsStringMapBase> StringMaps => _stringMaps;

#if UNITY_EDITOR
        public static EasyResolutions_Settings Instance { get; private set; }
#endif

#if UNITY_EDITOR
        [InitializeOnLoadMethod]
        static void InitializeInstance() {
            ISettings.Instance = Instance = GetInstance();
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }

        static void OnPlayModeStateChanged(PlayModeStateChange state) => ISettings.Instance = Instance = GetInstance();

        static EasyResolutions_Settings GetInstance() {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            return preloadedAssets
                .FirstOrDefault(asset => asset is EasyResolutions_Settings) as EasyResolutions_Settings;
        }
#endif

        void OnEnable() {
#if UNITY_EDITOR
            // Initialize with default values
            if (_defaultComparer == null) {
                using var serializedObject = new SerializedObject(this);
                serializedObject.SetDefaultComparer();
                serializedObject.IncrementVersion();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            if (_sceneList == null || _sceneList.Count == 0) {
                using var serializedObject = new SerializedObject(this);
                serializedObject.ReRegisterScenes();
                serializedObject.IncrementVersion();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            if (string.IsNullOrEmpty(_startingSceneGroup) && _sceneList != null) {
                _startingSceneGroup = _sceneList.Count switch {
                    1 => _sceneList[0].Group,
                    > 1 => _sceneList[1].Group,
                    _ => _startingSceneGroup
                };

                using var serializedObject = new SerializedObject(this);
                serializedObject.IncrementVersion();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }
#else
            if (_defaultComparer == null) { // Should not happen, but let's handle this just in case.
                Debug.LogWarning("EasyResolutions: default comparer is not set. Defaulting to ResolutionFitComparer.");
                _defaultComparer = CreateInstance<EasyResolutions_DefaultComparer>();
            }

            ISettings.Instance ??= this as ISettings;
#endif
        }

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => RefreshScenesDictionary();

        void RefreshScenesDictionary() {
            Scenes.Clear();
            Comparers.Clear();

#if UNITY_EDITOR
            var oldGroupFoldoutValues = _groupFoldoutKeys
                .Zip(_groupFoldoutValues, (key, value) => (key, value))
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

            _groupFoldoutKeys.ClearPairedValues(_groupFoldoutValues);

            var oldSceneFoldoutValues = _sceneFoldoutKeys
                .Zip(_sceneFoldoutValues, (key, value) => (key, value))
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

            _sceneFoldoutKeys.ClearPairedValues(_sceneFoldoutValues);
#endif

            var oldGroupComparerValues = _groupComparerKeys
                .Zip(_groupComparerValues, (key, value) => (key, value))
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

            _groupComparerKeys.ClearPairedValues(_groupComparerValues);

            foreach (var sceneInfo in _sceneList) {
                if (sceneInfo.Index == 0)
                    ZeroScene = sceneInfo;

                if (Scenes.TryGetValue(sceneInfo.Group, out var groupedScenes))
                    groupedScenes.Add(sceneInfo);
                else {
                    Scenes.Add(sceneInfo.Group, new List<IParsedString> { sceneInfo });

                    var groupComparer = GetComparer(oldGroupComparerValues, sceneInfo.Group);
                    Comparers.Add(sceneInfo.Group, groupComparer);

#if UNITY_EDITOR
                    _groupFoldoutKeys.AddPairedValues(
                        _groupFoldoutValues, oldGroupFoldoutValues, sceneInfo.Group, false);
#endif

                    _groupComparerKeys.AddPairedValues(
                        _groupComparerValues, oldGroupComparerValues, sceneInfo.Group, null);
                }

#if UNITY_EDITOR
                _sceneFoldoutKeys.AddPairedValues(
                    _sceneFoldoutValues, oldSceneFoldoutValues, sceneInfo.Value, false);
#endif
            }
        }

        IEasyResolutionsComparer GetComparer(
            Dictionary<string, EasyResolutionsComparer> groupComparerValues, string groupPath) =>
            groupComparerValues.TryGetValue(groupPath, out var groupComparer) && groupComparer != null
                ? groupComparer
                : _defaultComparer;
    }
}