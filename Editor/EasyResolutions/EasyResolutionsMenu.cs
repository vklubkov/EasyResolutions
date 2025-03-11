using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyResolutions {
    internal static class EasyResolutionsMenu {
        const string _createSettingsAssetMenuItemName = "Tools/Easy Resolutions/Create Settings";
        const string _createStringMapAssetMenuItemName = "Tools/Easy Resolutions/Create String Map";
        const string _createComparerAssetMenuItemName = "Tools/Easy Resolutions/Create Comparer";
        const string _openSettingsAssetMenuItemName = "Tools/Easy Resolutions/Open Settings";

        const string _runStartingSceneInsteadOfActiveSceneMenuItemName =
            "Tools/Easy Resolutions/Run Starting Scene instead of Active Scene";
        
        const string _pickResolutionForActiveSceneMenuItemName =
            "Tools/Easy Resolutions/Pick Resolution for Active Scene";

        const string _createHelpMenuItemName = "Tools/Easy Resolutions/Help";

        [MenuItem(_createSettingsAssetMenuItemName, priority = 10)]
        static void CreateSettingsAsset() =>
            EditorApplication.ExecuteMenuItem("Assets/Create/Easy Resolutions/Settings");

        [MenuItem(_createStringMapAssetMenuItemName, priority = 20)]
        static void CreateStringMapAsset() =>
            EditorApplication.ExecuteMenuItem("Assets/Create/Easy Resolutions/String Map");

        [MenuItem(_createComparerAssetMenuItemName, priority = 30)]
        static void CreateComparerAsset() =>
            EditorApplication.ExecuteMenuItem("Assets/Create/Easy Resolutions/Comparer");

        [MenuItem(_openSettingsAssetMenuItemName, isValidateFunction: false, priority = 50)]
        static void OpenSettingsAsset() {
            var settings = GetSettings();
            if (settings == null)
                return;

            Selection.activeObject = settings;
        }

        [MenuItem(_openSettingsAssetMenuItemName, isValidateFunction: true)]
        static bool ValidateOpenSettingsAsset() => GetSettings() != null;

        [MenuItem(_runStartingSceneInsteadOfActiveSceneMenuItemName, isValidateFunction: false, priority: 70)]
        static void ToggleRunStartingSceneInsteadOfActiveScene() {
            var settings = GetSettings();
            if (settings == null)
                return;

            using var serializedObject = new SerializedObject(settings);

            using var runStartingSceneInsteadOfActiveSceneProperty = serializedObject.FindProperty(
                EasyResolutions_Settings.RunStartingSceneInsteadOfActiveScenePropertyName);

            runStartingSceneInsteadOfActiveSceneProperty.boolValue =
                !runStartingSceneInsteadOfActiveSceneProperty.boolValue;

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem(_runStartingSceneInsteadOfActiveSceneMenuItemName, isValidateFunction: true)]
        static bool ValidateRunStartingSceneInsteadOfActiveScene() {
            var settings = GetSettings();
            if (settings == null)
                return false;

            using var serializedObject = new SerializedObject(settings);

            using var runStartingSceneInsteadOfActiveSceneProperty = serializedObject.FindProperty(
                EasyResolutions_Settings.RunStartingSceneInsteadOfActiveScenePropertyName);

            var runStartingSceneInsteadOfActiveScene = runStartingSceneInsteadOfActiveSceneProperty.boolValue;
            Menu.SetChecked(_runStartingSceneInsteadOfActiveSceneMenuItemName, runStartingSceneInsteadOfActiveScene);
            return true;
        }
        
        [MenuItem(_pickResolutionForActiveSceneMenuItemName, isValidateFunction: false, priority: 80)]
        static void TogglePickResolutionForActiveScene() {
            var settings = GetSettings();
            if (settings == null)
                return;

            using var serializedObject = new SerializedObject(settings);
            
            using var pickResolutionForActiveSceneProperty = 
                serializedObject.FindProperty(EasyResolutions_Settings.PickResolutionForActiveScenePropertyName);
            
            pickResolutionForActiveSceneProperty.boolValue = !pickResolutionForActiveSceneProperty.boolValue;
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem(_pickResolutionForActiveSceneMenuItemName, isValidateFunction: true)]
        static bool ValidatePickResolutionForActiveScene() {
            var settings = GetSettings();
            if (settings == null)
                return false;

            using var serializedObject = new SerializedObject(settings);

            using var pickResolutionForActiveSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.PickResolutionForActiveScenePropertyName);

            var pickResolutionForActiveScene = pickResolutionForActiveSceneProperty.boolValue;
            Menu.SetChecked(_pickResolutionForActiveSceneMenuItemName, pickResolutionForActiveScene);
            return true;
        }

        static EasyResolutions_Settings GetSettings() {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            return preloadedAssets
                .FirstOrDefault(asset => asset is EasyResolutions_Settings) as EasyResolutions_Settings;
        }
        
        [MenuItem(_createHelpMenuItemName, priority = 100)]
        public static void Open() => Application.OpenURL("https://github.com/vklubkov/EasyResolutions");
    }
}