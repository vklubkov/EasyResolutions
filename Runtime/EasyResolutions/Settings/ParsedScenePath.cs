using System;
using UnityEngine;

namespace EasyResolutions {
    [Serializable]
    internal class ParsedScenePath : IParsedString {
#if UNITY_EDITOR
        public static string IndexPropertyName => nameof(_index);
        public static string FullPathPropertyName => nameof(_fullPath);
        public static string NameWithoutExtensionPropertyName => nameof(_nameWithoutExtension);
        public static string PathWithoutExtensionPropertyName => nameof(_pathWithoutExtension);
        public static string GroupPathPropertyName => nameof(_groupPath);
        public static string WidthPropertyName => nameof(_width);
        public static string HeightPropertyName => nameof(_height);

        [SerializeField] string _fullPath;
        [SerializeField] string _nameWithoutExtension;
#endif

        [SerializeField] int _index;
        [SerializeField] string _groupPath;
        [SerializeField] string _pathWithoutExtension;
        [SerializeField] int _width;
        [SerializeField] int _height;

#if UNITY_EDITOR
        public string FullPath => _fullPath;
        public string NameWithoutExtension => _nameWithoutExtension;
#endif

        public int Index => _index;
        public string Group => _groupPath;
        public string Value => _pathWithoutExtension;
        public int Width => _width;
        public int Height => _height;
        public string Extension { get; } = "unity";

        public ParsedScenePath(string scenePath, int sceneIndex = -1) {
            var sceneData = scenePath.PrepareSceneData(sceneIndex);

#if UNITY_EDITOR
            _fullPath = sceneData.FullPath;
            _nameWithoutExtension = sceneData.NameWithoutExtension;
#endif

            _index = sceneData.Index;
            _pathWithoutExtension = sceneData.PathWithoutExtension;
            _groupPath = sceneData.GroupPath;
            _width = sceneData.Width;
            _height = sceneData.Height;
        }
    }
}