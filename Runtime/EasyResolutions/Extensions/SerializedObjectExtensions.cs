#if UNITY_EDITOR

using System.Linq;
using UnityEditor;

namespace EasyResolutions {
    public static class SerializedObjectExtensions {
        public static void SetDefaultComparer(this SerializedObject serializedObject) {
            using var defaultComparerProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.DefaultComparerPropertyName);

            defaultComparerProperty.objectReferenceValue =
                nameof(EasyResolutions_DefaultComparer)
                    .FindAssets<EasyResolutions_DefaultComparer>()
                    .FirstOrDefault();
        }

        public static void ReRegisterScenes(this SerializedObject serializedObject) {
            using var sceneListProperty = serializedObject.FindProperty(EasyResolutions_Settings.SceneListPropertyName);
            sceneListProperty.ClearArray();

            var scenePaths = EditorBuildSettings.scenes
                .Where(scene => scene.enabled)
                .Select(scene => scene.path)
                .ToArray();

            for (var i = 0; i < scenePaths.Length; i++) {
                sceneListProperty.InsertArrayElementAtIndex(i);
                var sceneInfo = sceneListProperty.GetArrayElementAtIndex(i);
                var sceneData = scenePaths[i].PrepareSceneData(i);
                sceneInfo.FindPropertyRelative(ParsedScenePath.IndexPropertyName).intValue = sceneData.Index;
                sceneInfo.FindPropertyRelative(ParsedScenePath.FullPathPropertyName).stringValue = sceneData.FullPath;

                sceneInfo.FindPropertyRelative(ParsedScenePath.PathWithoutExtensionPropertyName).stringValue =
                    sceneData.PathWithoutExtension;

                sceneInfo.FindPropertyRelative(ParsedScenePath.NameWithoutExtensionPropertyName).stringValue =
                    sceneData.NameWithoutExtension;

                sceneInfo.FindPropertyRelative(ParsedScenePath.GroupPathPropertyName).stringValue = sceneData.GroupPath;
                sceneInfo.FindPropertyRelative(ParsedScenePath.WidthPropertyName).intValue = sceneData.Width;
                sceneInfo.FindPropertyRelative(ParsedScenePath.HeightPropertyName).intValue = sceneData.Height;
            }
        }

        public static void IncrementVersion(this SerializedObject serializedObject) {
            using var versionProperty = serializedObject.FindProperty(EasyResolutions_Settings.VersionPropertyName);
            unchecked {
                versionProperty.intValue++;
            }
        }
    }
}

#endif