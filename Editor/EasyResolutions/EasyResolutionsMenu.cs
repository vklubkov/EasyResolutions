using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EasyResolutions {
    internal static class EasyResolutionsMenu {
        const string _createSettingsAssetMenuItemName = "Tools/Easy Resolutions/Create Settings";
        const string _createStringMapAssetMenuItemName = "Tools/Easy Resolutions/Create String Map";
        const string _createComparerAssetMenuItemName = "Tools/Easy Resolutions/Create Comparer";
        const string _openSettingsAssetMenuItemName = "Tools/Easy Resolutions/Open Settings";

        const string _runStartingSceneInsteadOfOpenSceneMenuItemName =
            "Tools/Easy Resolutions/Run Starting Scene instead of Open Scene";
        
        const string _pickResolutionForRunningSceneMenuItemName =
            "Tools/Easy Resolutions/Pick Resolution for Running Scene";

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

        [MenuItem(_runStartingSceneInsteadOfOpenSceneMenuItemName, isValidateFunction: false, priority: 70)]
        static void ToggleRunStartingSceneInsteadOfOpenScene() {
            var settings = GetSettings();
            if (settings == null)
                return;

            using var serializedObject = new SerializedObject(settings);

            using var runStartingSceneInsteadOfOpenSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.RunStartingSceneInsteadOfOpenScenePropertyName);

            runStartingSceneInsteadOfOpenSceneProperty.boolValue =
                !runStartingSceneInsteadOfOpenSceneProperty.boolValue;

            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem(_runStartingSceneInsteadOfOpenSceneMenuItemName, isValidateFunction: true)]
        static bool ValidateRunStartingSceneInsteadOfOpenScene() {
            var settings = GetSettings();
            if (settings == null)
                return false;

            using var serializedObject = new SerializedObject(settings);

            using var runStartingSceneInsteadOfOpenSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.RunStartingSceneInsteadOfOpenScenePropertyName);

            var runStartingSceneInsteadOfOpenScene = runStartingSceneInsteadOfOpenSceneProperty.boolValue;
            Menu.SetChecked(_runStartingSceneInsteadOfOpenSceneMenuItemName, runStartingSceneInsteadOfOpenScene);
            return true;
        }
        
        [MenuItem(_pickResolutionForRunningSceneMenuItemName, isValidateFunction: false, priority: 80)]
        static void TogglePickResolutionForRunningScene() {
            var settings = GetSettings();
            if (settings == null)
                return;

            using var serializedObject = new SerializedObject(settings);
            
            using var pickResolutionForRunningSceneProperty = 
                serializedObject.FindProperty(EasyResolutions_Settings.PickResolutionForRunningScenePropertyName);
            
            pickResolutionForRunningSceneProperty.boolValue = !pickResolutionForRunningSceneProperty.boolValue;
            serializedObject.ApplyModifiedProperties();
        }

        [MenuItem(_pickResolutionForRunningSceneMenuItemName, isValidateFunction: true)]
        static bool ValidatePickResolutionForRunningScene() {
            var settings = GetSettings();
            if (settings == null)
                return false;

            using var serializedObject = new SerializedObject(settings);

            using var pickResolutionForRunningSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.PickResolutionForRunningScenePropertyName);

            var pickResolutionForRunningScene = pickResolutionForRunningSceneProperty.boolValue;
            Menu.SetChecked(_pickResolutionForRunningSceneMenuItemName, pickResolutionForRunningScene);
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