using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyResolutions {
    [InitializeOnLoad]
    internal class EditorDataProvider : AssetPostprocessor {
        static readonly HashSet<EasyResolutionsSettingsEditor> _settingsEditors = new();

        static EditorDataProvider() {
            Update();
            EditorBuildSettings.sceneListChanged += OnSceneListChanged;
        }

        static void OnSceneListChanged() => Update();

        static void OnPostprocessAllAssets(string[] importedAssets,
                                           string[] deletedAssets,
                                           string[] movedAssets,
                                           string[] movedFromAssetPaths) {
            var wasSceneMoved = movedAssets.Any(asset => asset.EndsWith(".unity"));
            if (!wasSceneMoved)
                return;

            Update();
        }
        
        public static void Update() {
            var settingsAsset = nameof(EasyResolutions_Settings).FindAssets<EasyResolutions_Settings>();
            if (settingsAsset.Count == 0) {
                Debug.LogWarning("EasyResolutions: settings not found. " +
                                 "Please check Preloaded Assets in Project Settings.");

                return;
            }

            foreach (var sceneAsset in settingsAsset) {
                using var serializedObject = new SerializedObject(sceneAsset);
                serializedObject.SetDefaultComparer();
                serializedObject.ReRegisterScenes();
                serializedObject.IncrementVersion();
                serializedObject.ApplyModifiedPropertiesWithoutUndo();
            }

            foreach (var settingsEditor in _settingsEditors)
                settingsEditor.RecreateUI();
        }

        public static void Bind(EasyResolutionsSettingsEditor settingsEditor) =>
            _settingsEditors.Add(settingsEditor);

        public static void Unbind(EasyResolutionsSettingsEditor settingsEditor) =>
            _settingsEditors.Remove(settingsEditor);
    }
}