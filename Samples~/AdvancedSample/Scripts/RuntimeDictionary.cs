using System.Collections.Generic;
using UnityEngine;

namespace EasyResolutions.AdvancedSample {
    public class RuntimeDictionary : MonoBehaviour {
        [SerializeField] EasyResolutionsComparer _comparer;
        [SerializeField] GameObject[] _prefabs;

        readonly Dictionary<string, GameObject> _loadedPrefabs = new();
        readonly Dictionary<string, List<IParsedString>> _prefabLookup = new();

        void Start() {
            foreach (var prefab in _prefabs) {
                _loadedPrefabs.Add(prefab.name, prefab);
                var parsedString = new ParsedString(prefab.name, string.Empty);
                if (_prefabLookup.TryGetValue(parsedString.Group, out var stringGroup))
                    stringGroup.Add(parsedString);
                else
                    _prefabLookup[parsedString.Group] = new List<IParsedString> { parsedString };
            }

            var selectedPrefabName = "Canvas".PickResolution(_comparer, _prefabLookup);
            if (_loadedPrefabs.TryGetValue(selectedPrefabName, out var selectedPrefab))
                Instantiate(selectedPrefab);
        }
    }
}