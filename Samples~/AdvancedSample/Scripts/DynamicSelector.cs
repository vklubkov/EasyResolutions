using System.Collections.Generic;
using EasyResolutions;
using UnityEngine;

namespace EasyResolutions.AdvancedSample {
    public class DynamicSelector : MonoBehaviour {
        [SerializeField] EasyResolutionsComparer _comparer;
        [SerializeField] GameObject[] _gameObjects;

        readonly Dictionary<string, List<IParsedString>> _gameObjectsLookup = new();

        int _width;
        int _height;

        void Start() {
            foreach (var go in _gameObjects) {
                var parsedString = new ParsedString(go.name, string.Empty);
                if (_gameObjectsLookup.TryGetValue(parsedString.Group, out var stringGroup))
                    stringGroup.Add(parsedString);
                else
                    _gameObjectsLookup[parsedString.Group] = new List<IParsedString> { parsedString };
            }

            Select();
        }

        void Select() {
            var selectedGameObjectName = "Canvas".PickResolution(_comparer, _gameObjectsLookup);
            foreach (var go in _gameObjects)
                go.SetActive(go.name == selectedGameObjectName);
        }

        void Update() {
            if (_width == Screen.width && _height == Screen.height)
                return;

            Select();
        }
    }
}