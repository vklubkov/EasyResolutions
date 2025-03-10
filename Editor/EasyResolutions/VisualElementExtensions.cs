using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;

namespace EasyResolutions {
    internal static class VisualElementExtensions {
        public static VisualElement AddPropertyField(this VisualElement parent, SerializedProperty property) {
            var propertyField = new PropertyField(property);
            propertyField.BindProperty(property);
            parent.Add(propertyField);
            return propertyField;
        }
        
        public static VisualElement AddItemPropertyField(
            this VisualElement parent,
            SerializedProperty keysArrayProperty,
            SerializedProperty valuesArrayProperty,
            string key,
            string label) {
            var valueIndex = keysArrayProperty.FindKeyIndex(key);
            if (valueIndex == -1)
                return null;

            var valueProperty = valuesArrayProperty.GetArrayElementAtIndex(valueIndex);
            var propertyField = new PropertyField(valueProperty) { label = label };
            propertyField.BindProperty(valueProperty);
            parent.Add(propertyField);
            return propertyField;
        }

        public static VisualElement AddFoldout(
            this VisualElement parent,
            SerializedProperty foldoutValueProperty,
            string title) {
            var foldout = new Foldout {
                text = title,
                value = foldoutValueProperty.boolValue,

                // For some reason Unity can't properly offset elements in Inspector,
                // different elements can be displayed with different offsets.
                // Offsets can also differ depending on the hierarchy levels and
                // types of parent elements. In Unity 6 these offsets also seem
                // to not be consistent between different Editor versions.
                //
                // Make a Foldout have same offset as foldouts drawn as PropertyFields.
                // But this should not be expected to be universal.
                style = { marginLeft = 12 }
            };

            foldout.BindProperty(foldoutValueProperty);
            parent.Add(foldout);
            return foldout;
        }

        public static VisualElement AddItemFoldout(
            this VisualElement parent,
            SerializedProperty foldoutKeysProperty,
            SerializedProperty foldoutValuesProperty,
            string key,
            string suffix = "") {
            var foldoutValueIndex = foldoutKeysProperty.FindKeyIndex(key);
            if (foldoutValueIndex == -1)
                return null;

            var foldoutValueProperty = foldoutValuesProperty.GetArrayElementAtIndex(foldoutValueIndex);

            var foldout = new Foldout {
                text = key + suffix,
                value = foldoutValueProperty.boolValue
            };

            foldout.BindProperty(foldoutValueProperty);
            parent.Add(foldout);
            return foldout;
        }
        
        static int FindKeyIndex(this SerializedProperty array, string key) {
            for (var i = 0; i < array.arraySize; i++) {
                var keyProperty = array.GetArrayElementAtIndex(i);
                var elementKey = keyProperty.stringValue;
                if (elementKey == key)
                    return i;
            }

            return -1;
        }
    }
}