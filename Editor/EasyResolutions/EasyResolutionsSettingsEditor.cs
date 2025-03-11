using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyResolutions {
    [CustomEditor(typeof(EasyResolutions_Settings))]
    internal class EasyResolutionsSettingsEditor : EditorWithAutoUpdate {
        protected override void OnSetup() => EditorDataProvider.Bind(this);

        protected override void OnRefreshUI() {
            CreatePreloadedAssetsButton();
            CreateEditorSettings();
            CreateRuntimeSettings();
        }

        void CreatePreloadedAssetsButton() {
            var button = new Button { style = { marginBottom = 10f } };
            Root.Add(button);

            var isPreloaded = UpdatePreloadedAssetsButton(button);
            button.schedule
                .Execute(() => isPreloaded = UpdatePreloadedAssetsButton(button))
                .Every(100);

            button.clicked += () => {
                var preloadedAssets = PlayerSettings.GetPreloadedAssets().ToList();
                preloadedAssets.RemoveAll(asset => asset is EasyResolutions_Settings);

                if (!isPreloaded)
                    preloadedAssets.Add(target);

                PlayerSettings.SetPreloadedAssets(preloadedAssets.ToArray());
                RepaintEditorWindow("Project Settings");
            };
        }

        bool UpdatePreloadedAssetsButton(Button button) {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();
            var isPreloaded = preloadedAssets.Contains(target);

            button.text = isPreloaded
                ? "Remove from Preloaded Assets"
                : "Add to Preloaded Assets";

            return isPreloaded;
        }

        static void RepaintEditorWindow(string windowTitle) {
            var editorWindows = Resources.FindObjectsOfTypeAll<EditorWindow>();

            var targetEditorWindows = editorWindows
                .Where(editorWindow => editorWindow.titleContent.text == windowTitle)
                .ToArray();

            if (targetEditorWindows.Length == 0)
                return;

            foreach (var targetEditorWindow in targetEditorWindows)
                targetEditorWindow.Repaint();
        }

        void CreateEditorSettings() {
            var editorSettings = new VisualElement { style = { marginBottom = 10f } };
            Root.Add(editorSettings);

            var editorSettingsTitle =
                new Label("Editor Settings") { style = { unityFontStyleAndWeight = FontStyle.Bold } };

            editorSettings.Add(editorSettingsTitle);

            var buildPlayerProcessorOrderProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.BuildPlayerProcessorOrderPropertyName);

            editorSettings.AddPropertyField(buildPlayerProcessorOrderProperty);

            var runStartingSceneInsteadOfOpenSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.RunStartingSceneInsteadOfActiveScenePropertyName);

            editorSettings.AddPropertyField(runStartingSceneInsteadOfOpenSceneProperty);

            var pickResolutionForRunningSceneProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.PickResolutionForActiveScenePropertyName);

            editorSettings.AddPropertyField(pickResolutionForRunningSceneProperty);
        }

        void CreateRuntimeSettings() {
            var runtimeSettings = new VisualElement { style = { marginBottom = 10f } };
            Root.Add(runtimeSettings);

            var runtimeSettingsTitle =
                new Label("Runtime Settings") { style = { unityFontStyleAndWeight = FontStyle.Bold } };

            runtimeSettings.Add(runtimeSettingsTitle);

            var comparisonSectionTitle = new Label("Comparison") {
                style = {
                    paddingTop = 5f,
                    paddingLeft = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            runtimeSettings.Add(comparisonSectionTitle);

            CreateDefaultComparerField(runtimeSettings);
            CreateEpsilonForAspectRatioComparisonField(runtimeSettings);

            var scenesSectionTitle = new Label("Scenes") {
                style = {
                    paddingTop = 5f,
                    paddingLeft = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            runtimeSettings.Add(scenesSectionTitle);
            CreateZeroSceneLabel(runtimeSettings);
            CreateStartingSceneGroupDropdown(runtimeSettings);
            CreateSceneMap(runtimeSettings);

            var stringsSectionTitle = new Label("Strings") {
                style = {
                    paddingTop = 5f,
                    paddingLeft = 3f,
                    unityFontStyleAndWeight = FontStyle.Bold
                }
            };

            runtimeSettings.Add(stringsSectionTitle);
            CreateStringMapList(runtimeSettings);
        }

        void CreateDefaultComparerField(VisualElement parent) {
            var defaultComparerProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.DefaultComparerPropertyName);

            var visualElement = parent.AddPropertyField(defaultComparerProperty);
            visualElement.TrackPropertyValue(defaultComparerProperty, trackedProperty => {
                if (trackedProperty.objectReferenceValue != null)
                    return;

                serializedObject.SetDefaultComparer();
                serializedObject.ApplyModifiedProperties();
            });
        }

        void CreateEpsilonForAspectRatioComparisonField(VisualElement parent) {
            var epsilonForAspectRatioComparisonProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.EpsilonForAspectRatioComparisonPropertyName);

            parent.AddPropertyField(epsilonForAspectRatioComparisonProperty);
        }
        
        void CreateZeroSceneLabel(VisualElement parent) {
            var settings = (EasyResolutions_Settings)target;

            var horizontalGroup = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row,
                    paddingLeft = 3f
                }
            };

            var title = new Label("Zero Scene") {
                style = {
                    minWidth = 137, // To align with SceneGroupDropdown
                    unityTextAlign = TextAnchor.MiddleLeft
                }
            };

            var value = new Label(settings.ZeroScene?.Value ?? string.Empty) {
                style = {
                    unityTextAlign = TextAnchor.MiddleLeft,
                    overflow = Overflow.Hidden,
                },
                selection = { isSelectable = true }
            };

            horizontalGroup.Add(title);
            horizontalGroup.Add(value);
            parent.Add(horizontalGroup);
        }

        void CreateStartingSceneGroupDropdown(VisualElement parent) {
            var settings = (EasyResolutions_Settings)target;

            var startingSceneGroupProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.StartingSceneGroupPropertyName);

            var sceneGroups = settings.Scenes.Keys.ToList();

            var dropdown = new PopupField<string>("Starting Scene (group)", sceneGroups, 0) {
                value = startingSceneGroupProperty.stringValue
            };

            dropdown.RegisterValueChangedCallback(evt => {
                startingSceneGroupProperty.stringValue = evt.newValue;
                serializedObject.ApplyModifiedProperties();
            });

            parent.Add(dropdown);
        }

        void CreateSceneMap(VisualElement parent) {
            var settings = (EasyResolutions_Settings)target;
            var scenes = settings.Scenes;

            var groupComparerKeysProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.GroupComparerKeysPropertyName);

            var groupComparerValuesProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.GroupComparerValuesPropertyName);

            var scenesFoldoutValueProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.ScenesFoldoutValuePropertyName);

            var groupFoldoutKeysProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.GroupFoldoutKeysPropertyName);

            var groupFoldoutValuesProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.GroupFoldoutValuesPropertyName);

            var sceneFoldoutKeysProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.SceneFoldoutKeysPropertyName);

            var sceneFoldoutValuesProperty =
                serializedObject.FindProperty(EasyResolutions_Settings.SceneFoldoutValuesPropertyName);

            var sceneMapFoldout = parent.AddFoldout(scenesFoldoutValueProperty, "Scene Map");

            foreach (var (groupName, groupedScenes) in scenes) {
                var groupFoldout = sceneMapFoldout.AddItemFoldout(
                    groupFoldoutKeysProperty, groupFoldoutValuesProperty, groupName, " (group)");

                if (groupFoldout == null) {
                    Debug.LogWarning($"EasyResolutions: failed to draw group {groupName}");
                    continue;
                }

                var comparerOverrideField = groupFoldout.AddItemPropertyField(
                    groupComparerKeysProperty, groupComparerValuesProperty, groupName, "Comparer Override");

                if (comparerOverrideField == null)
                    Debug.LogWarning($"EasyResolutions: failed to draw the comparer override for group {groupName}");

                foreach (var scene in groupedScenes) {
                    var sceneInfo = (ParsedScenePath)scene;

                    var sceneFoldout = groupFoldout.AddItemFoldout(
                        sceneFoldoutKeysProperty, sceneFoldoutValuesProperty, scene.Value);

                    if (sceneFoldout == null) {
                        Debug.LogWarning($"EasyResolutions: failed to draw the foldout for scene {scene.Value}");
                        continue;
                    }

                    CreateSceneInfoElement(parent: sceneFoldout, sceneInfo);
                }
            }
        }

        void CreateSceneInfoElement(VisualElement parent, ParsedScenePath parsedScenePath) {
            var horizontalGroup = new VisualElement {
                style = {
                    flexDirection = FlexDirection.Row,
                    unityFontStyleAndWeight = FontStyle.Normal
                }
            };

            parent.Add(horizontalGroup);

            var leftPanel = new VisualElement {
                style = {
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderLeftWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = Color.gray,
                    borderBottomColor = Color.gray,
                    borderLeftColor = Color.gray,
                    borderRightColor = Color.gray,
                    unityTextAlign = TextAnchor.UpperRight,
                    flexGrow = 0,
                    backgroundColor = Color.white,
                    color = Color.black
                }
            };

            horizontalGroup.Add(leftPanel);

            var rightPanel = new VisualElement {
                style = {
                    borderTopWidth = 1,
                    borderBottomWidth = 1,
                    borderRightWidth = 1,
                    borderTopColor = Color.gray,
                    borderBottomColor = Color.gray,
                    borderRightColor = Color.gray,
                    flexGrow = 1,
                    backgroundColor = Color.white,
                    color = Color.black
                }
            };

            horizontalGroup.Add(rightPanel);

            var leftStringBuilder = new StringBuilder();
            var rightStringBuilder = new StringBuilder();

            leftStringBuilder.AppendLine("Group");
            rightStringBuilder.AppendLine(parsedScenePath.Group);

            leftStringBuilder.AppendLine("Scene Path");
            rightStringBuilder.AppendLine(parsedScenePath.Value);

            leftStringBuilder.AppendLine("Editor Path");
            rightStringBuilder.AppendLine(parsedScenePath.FullPath);

            var buildIndex = parsedScenePath.Index;
            if (buildIndex != -1) { // Should not be -1 for scenes in Build Settings, but let's handle this just in case
                leftStringBuilder.AppendLine("Scene Name");
                rightStringBuilder.AppendLine(parsedScenePath.NameWithoutExtension);

                leftStringBuilder.AppendLine("Build Index");
                rightStringBuilder.AppendLine(parsedScenePath.Index.ToString());
            }

            var width = parsedScenePath.Width;
            var height = parsedScenePath.Height;
            if (width != -1 && height != -1) {
                leftStringBuilder.AppendLine("Resolution");
                rightStringBuilder.AppendLine($"{width}x{height}");
            }

            leftPanel.Add(new Label(leftStringBuilder.ToString()));
            rightPanel.Add(new Label(rightStringBuilder.ToString()) { selection = { isSelectable = true } });
        }

        void CreateStringMapList(VisualElement parent) {
            var stringMapsProperty = serializedObject.FindProperty(EasyResolutions_Settings.StringMapsPropertyName);
            parent.AddPropertyField(stringMapsProperty);
        }

        protected override void OnCleanup() => EditorDataProvider.Unbind(this);
    }
}