using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace EasyResolutions {
    public abstract class EasyResolutionsStringMapBase : ScriptableObject { }

    // ReSharper disable InconsistentNaming because it matches the asset name
    public abstract class EasyResolutionsStringMap<T> :
        // ReSharper restore InconsistentNaming
        EasyResolutionsStringMapBase, IStringMap<T>, ISerializationCallbackReceiver {
#if UNITY_EDITOR
        public static string IdentifierPropertyName => nameof(_identifier);
        public static string ComparerOverridePropertyName => nameof(_comparerOverride);
        public static string ExtensionPropertyName => nameof(_extension);
        public static string GroupComparerKeysPropertyName => nameof(_groupComparerKeys);
        public static string GroupComparerValuesPropertyName => nameof(_groupComparerValues);
        public static string StringListPropertyName => nameof(_stringList);
        public static string StringsFoldoutValuePropertyName => nameof(_stringsFoldoutValue);
        public static string GroupFoldoutKeysPropertyName => nameof(_groupFoldoutKeys);
        public static string GroupFoldoutValuesPropertyName => nameof(_groupFoldoutValues);
        public static string StringFoldoutKeysPropertyName => nameof(_stringFoldoutKeys);
        public static string StringFoldoutValuesPropertyName => nameof(_stringFoldoutValues);
#endif

        [SerializeField] T _identifier;
        [SerializeField] EasyResolutionsComparer _comparerOverride;
        [SerializeField] string _extension;
        [SerializeField] List<string> _stringList;
        [SerializeField, HideInInspector] List<string> _groupComparerKeys;
        [SerializeField, HideInInspector] List<EasyResolutionsComparer> _groupComparerValues;

#if UNITY_EDITOR
        [SerializeField, HideInInspector] bool _stringsFoldoutValue;
        [SerializeField, HideInInspector] List<string> _groupFoldoutKeys;
        [SerializeField, HideInInspector] List<bool> _groupFoldoutValues;
        [SerializeField, HideInInspector] List<string> _stringFoldoutKeys;
        [SerializeField, HideInInspector] List<bool> _stringFoldoutValues;
#endif

        public T Identifier => _identifier;
        public string Extension => _extension;
        public IEasyResolutionsComparer ComparerOverride => _comparerOverride;
        public Dictionary<string, List<IParsedString>> Strings { get; } = new();
        public Dictionary<string, IEasyResolutionsComparer> Comparers { get; } = new();

        public void OnBeforeSerialize() { }
        public void OnAfterDeserialize() => RefreshScenesDictionary();

        void RefreshScenesDictionary() {
            Strings.Clear();
            Comparers.Clear();

#if UNITY_EDITOR
            var oldGroupFoldoutValues = _groupFoldoutKeys
                .Zip(_groupFoldoutValues, (key, value) => (key, value))
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

            _groupFoldoutKeys.ClearPairedValues(_groupFoldoutValues);

            var oldStringFoldoutValues = _stringFoldoutKeys
                .Zip(_stringFoldoutValues, (key, value) => (key, value))
                .ToList();

            _stringFoldoutKeys.ClearPairedValues(_stringFoldoutValues);
#endif

            var oldGroupComparerValues = _groupComparerKeys
                .Zip(_groupComparerValues, (key, value) => (key, value))
                .ToDictionary(kvp => kvp.key, kvp => kvp.value);

            _groupComparerKeys.ClearPairedValues(_groupComparerValues);

            foreach (var originalName in _stringList) {
                var parsedString = new ParsedString(originalName, _extension);
                if (Strings.TryGetValue(parsedString.Group, out var groupedScenes))
                    groupedScenes.Add(parsedString);
                else {
                    Strings.Add(parsedString.Group, new List<IParsedString> { parsedString });

                    var groupComparer = GetComparer(oldGroupComparerValues, parsedString.Group);
                    Comparers.Add(parsedString.Group, groupComparer);

#if UNITY_EDITOR
                    _groupFoldoutKeys.AddPairedValues(
                        _groupFoldoutValues, oldGroupFoldoutValues, parsedString.Group, false);
#endif

                    _groupComparerKeys.AddPairedValues(
                        _groupComparerValues, oldGroupComparerValues, parsedString.Group, null);
                }

#if UNITY_EDITOR
                _stringFoldoutKeys.AddPairedValues(
                    _stringFoldoutValues, oldStringFoldoutValues, parsedString.Value, false);
#endif
            }
        }

        IEasyResolutionsComparer GetComparer(
            Dictionary<string, EasyResolutionsComparer> groupComparerValues, string groupPath) =>
            groupComparerValues.TryGetValue(groupPath, out var groupComparer) && groupComparer != null
                ? groupComparer
                : _comparerOverride;
    }

    [CreateAssetMenu(fileName = "EasyResolutions_StringMap", menuName = "Easy Resolutions/String Map")]
    // ReSharper disable InconsistentNaming because it matches the asset name
    public class EasyResolutions_StringMap : EasyResolutionsStringMap<string> { }
    // ReSharper restore InconsistentNaming
}