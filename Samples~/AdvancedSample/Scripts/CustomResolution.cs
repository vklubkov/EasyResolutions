using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EasyResolutions.AdvancedSample {
    public class CustomResolution : MonoBehaviour {
        [SerializeField] EasyResolutionsComparer _comparer;
        [SerializeField] List<Texture2D> _textures;
        [SerializeField] RawImage _image;

        readonly Dictionary<string, Texture2D> _loadedTextures = new();
        readonly Dictionary<string, List<IParsedString>> _textureLookup = new();

        void Start() {
            foreach (var texture in _textures) {
                _loadedTextures.Add(texture.name, texture);
                var parsedString = new ParsedString(texture.name, string.Empty);
                if (_textureLookup.TryGetValue(parsedString.Group, out var stringGroup))
                    stringGroup.Add(parsedString);
                else
                    _textureLookup[parsedString.Group] = new List<IParsedString> { parsedString };
            }

            var selectedTextureName = "Background".PickResolution(
                (int)_image.rectTransform.rect.width,
                (int)_image.rectTransform.rect.height,
                _comparer, _textureLookup);

            if (_loadedTextures.TryGetValue(selectedTextureName, out var selectedTexture))
                _image.texture = selectedTexture;
        }
    }
}