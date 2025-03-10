using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

namespace EasyResolutions {
    [CustomEditor(typeof(EasyResolutionsStringMap<>), true)]
    internal class EasyResolutionsStringMapEditor : EditorWithAutoUpdate {
        VisualElement _stringMap;

        protected override void OnRefreshUI() {
            CreateSettingsButton();
            CreateIdentifierField();
            CreateDefaultComparerField();
            CreateExtensionField();
            CreateStringList();
            CreateStringMap();
        }

        void CreateSettingsButton() {
            var button = new Button { style = { marginBottom = 10f } };
            Root.Add(button);

            var isAdded = UpdateSettingsButton(button);
            button.schedule
                .Execute(() => isAdded = UpdateSettingsButton(button))
                .Every(100);

            button.clicked += () => {
                var preloadedAssets = PlayerSettings.GetPreloadedAssets();

                var settings = preloadedAssets
                    .FirstOrDefault(asset => asset is EasyResolutions_Settings) as EasyResolutions_Settings;

                if (settings == null)
                    return;

                var settingsSerializedObject = new SerializedObject(settings);

                var stringMapsProperty =
                    settingsSerializedObject.FindProperty(EasyResolutions_Settings.StringMapsPropertyName);

                var elementsToRemove = new List<int>();
                for (var i = 0; i < stringMapsProperty.arraySize; i++) {
                    var arrayElementProperty = stringMapsProperty.GetArrayElementAtIndex(i);
                    if (arrayElementProperty.objectReferenceValue == target)
                        elementsToRemove.Add(i);
                }

                foreach (var index in elementsToRemove)
                    stringMapsProperty.DeleteArrayElementAtIndex(index);

                if (!isAdded) {
                    var newIndex = stringMapsProperty.arraySize;
                    stringMapsProperty.InsertArrayElementAtIndex(newIndex);
                    var stringMapProperty = stringMapsProperty.GetArrayElementAtIndex(newIndex);
                    stringMapProperty.objectReferenceValue = target;
                }

                settingsSerializedObject.ApplyModifiedProperties();
            };
        }

        bool UpdateSettingsButton(Button button) {
            var preloadedAssets = PlayerSettings.GetPreloadedAssets();

            var settings = preloadedAssets
                .FirstOrDefault(asset => asset is EasyResolutions_Settings) as EasyResolutions_Settings;

            if (settings == null) {
                button.SetEnabled(false);
                button.text = "Add to Settings";
                return false;
            }

            var stringMaps = settings.StringMaps;
            var isAdded = stringMaps.Contains(target);
            button.SetEnabled(true);

            button.text = isAdded
                ? "Remove from Settings"
                : "Add to Settings";

            return isAdded;
        }

        void CreateIdentifierField() {
            var identifierProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.IdentifierPropertyName);

            Root.AddPropertyField(identifierProperty);
        }

        void CreateDefaultComparerField() {
            var defaultComparerProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.ComparerOverridePropertyName);

           Root.AddPropertyField(defaultComparerProperty);
        }

        void CreateExtensionField() {
            var extensionProperty = serializedObject.FindProperty(EasyResolutions_StringMap.ExtensionPropertyName);
            var extensionField = new PropertyField(extensionProperty);
            extensionField.BindProperty(extensionProperty);
            Root.Add(extensionField);

            extensionField.RegisterCallback<GeometryChangedEvent>(_ => {
                var textField = extensionField.Q<TextField>();
                textField?.RegisterCallback<FocusOutEvent>(_ => RecreateUI());
            });
        }

        void CreateStringList() {
            var stringListProperty = serializedObject.FindProperty(EasyResolutions_StringMap.StringListPropertyName);
            var stringListField = new PropertyField(stringListProperty);
            stringListField.BindProperty(stringListProperty);
            Root.Add(stringListField);

            var oldNameList = GetStringListFromProperty(stringListProperty);
            stringListField.TrackPropertyValue(stringListProperty, changedProperty => {
                var newNameList = GetStringListFromProperty(changedProperty);
                if (IsStructuralChange(oldNameList, newNameList)) {
                    oldNameList = newNameList;
                    RecreateUI();
                    return;
                }

                _stringMap.Clear();
                serializedObject.Update();
                CreateStringGroups();
            });
        }

        IList<string> GetStringListFromProperty(SerializedProperty property) {
            var array = new string[property.arraySize];
            for (var i = 0; i < property.arraySize; i++)
                array[i] = property.GetArrayElementAtIndex(i).stringValue;

            return array;
        }

        // Structural changes:
        // - add - non-equal count
        // - remove - non-equal count
        // - move - equal count, differences in > 1 elements

        // Non-structural changes:
        // - user edit - difference in 1 element
        // - no edit - 0 differences in elements
        bool IsStructuralChange(IList<string> oldList, IList<string> newList) =>
            oldList.Count != newList.Count || CountDifferences(oldList, newList) > 1;

        int CountDifferences(IList<string> oldList, IList<string> newList) {
            if (oldList.Count != newList.Count)
                throw new ArgumentException("EasyResolutions: oldList and newList must have same length");

            var differencesCount = 0;
            for (var i = 0; i < oldList.Count; i++) {
                var oldItem = oldList[i];
                var newItem = newList[i];
                if (oldItem != newItem)
                    differencesCount++;
            }

            return differencesCount;
        }
        
        void CreateStringMap() {
            var stringsFoldoutValueProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.StringsFoldoutValuePropertyName);

            _stringMap = Root.AddFoldout(stringsFoldoutValueProperty, "String Map");
            CreateStringGroups();
        }

        void CreateStringGroups() {
            var settings = (IStringMap)target;
            var stringGroups = settings.Strings;

            var groupComparerKeysProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.GroupComparerKeysPropertyName);

            var groupComparerValuesProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.GroupComparerValuesPropertyName);

            var groupFoldoutKeysProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.GroupFoldoutKeysPropertyName);

            var groupFoldoutValuesProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.GroupFoldoutValuesPropertyName);

            var stringFoldoutKeysProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.StringFoldoutKeysPropertyName);

            var stringFoldoutValuesProperty =
                serializedObject.FindProperty(EasyResolutions_StringMap.StringFoldoutValuesPropertyName);

            foreach (var (groupName, stringGroup) in stringGroups) {
                var groupFoldout = _stringMap.AddItemFoldout(
                    groupFoldoutKeysProperty, groupFoldoutValuesProperty, groupName, " (group)");

                if (groupFoldout == null) {
                    Debug.LogWarning($"EasyResolutions: failed to draw group {groupName}");
                    continue;
                }

                var comparerOverrideField = groupFoldout.AddItemPropertyField(
                    groupComparerKeysProperty, groupComparerValuesProperty, groupName, "Comparer Override");

                if (comparerOverrideField == null)
                    Debug.LogWarning($"EasyResolutions: failed to draw the comparer override for group {groupName}");

                foreach (var parsedString in stringGroup) {
                    var stringFoldout = groupFoldout.AddItemFoldout(
                         stringFoldoutKeysProperty, stringFoldoutValuesProperty, parsedString.Value, string.Empty);

                    if (stringFoldout == null) {
                        Debug.LogWarning(
                            $"EasyResolutions: failed to draw the foldout for string {parsedString.Value}");

                        continue;
                    }

                    CreateStringInfoElement(stringFoldout, parsedString);
                }
            }
        }
        
        void CreateStringInfoElement(VisualElement parent, IParsedString parsedString) {
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
            rightStringBuilder.AppendLine(parsedString.Group);

            leftStringBuilder.AppendLine("Value");
            rightStringBuilder.AppendLine(parsedString.Value);

            var width = parsedString.Width;
            var height = parsedString.Height;
            if (width != -1 && height != -1) {
                leftStringBuilder.AppendLine("Resolution");
                rightStringBuilder.AppendLine($"{width}x{height}");
            }

            leftPanel.Add(new Label(leftStringBuilder.ToString()));
            rightPanel.Add(new Label(rightStringBuilder.ToString()) { selection = { isSelectable = true } });
        }
    }
}