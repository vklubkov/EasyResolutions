using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
#endif

namespace EasyResolutions {
    public static class StringExtensions {
#if UNITY_EDITOR
        public static IList<T> FindAssets<T>(this string path) where T : Object {
            var guids = AssetDatabase.FindAssets($"t:{path}");
            var assetPaths = guids.Select(AssetDatabase.GUIDToAssetPath);
            var assets = assetPaths.Select(AssetDatabase.LoadAssetAtPath<T>);
            return assets.ToList();
        }
#endif

        public static (
            bool IsFirst, string FullPath, string NameWithoutExtension,
            int Index, string PathWithoutExtension,
            string GroupPath, int Width, int Height)
            PrepareSceneData(this string path, int index = -1) {
            var isFirst = index == 0;

            var fullPath = path.Replace("\\", "/");
            fullPath = fullPath.EndsWith(".unity")
                ? fullPath
                : fullPath + ".unity";

            var runtimePath = fullPath.StartsWith("Assets/") ? fullPath["Assets/".Length..] : fullPath;

            var lastDotIndex = runtimePath.LastIndexOf('.');
            var pathWithoutExtension = runtimePath[..lastDotIndex];

            var lastSlashIndex = runtimePath.LastIndexOf('/');
            var nameWithoutExtension = pathWithoutExtension[(lastSlashIndex + 1)..];

            var groupName = nameWithoutExtension.ParseResolution(out var width, out var height);

            var pathOnly = runtimePath[..(lastSlashIndex + 1)];
            var groupPath = pathOnly + groupName;

            return (isFirst, fullPath, nameWithoutExtension,
                index, pathWithoutExtension, groupPath, width, height);
        }

        public static string ParseResolution(this string nameWithoutExtension, out int width, out int height) {
            var nameTokens = nameWithoutExtension.Split('.');
            if (nameTokens.Length <= 1)
                return PrepareNoResolution(out width, out height);

            var resolutionTokens = nameTokens[^1].Split('x');
            if (resolutionTokens.Length != 2)
                return PrepareNoResolution(out width, out height);

            if (!int.TryParse(resolutionTokens[0], out width))
                return PrepareNoResolution(out width, out height);

            if (!int.TryParse(resolutionTokens[1], out height))
                return PrepareNoResolution(out width, out height);

            string PrepareNoResolution(out int width, out int height) {
                width = -1;
                height = -1;
                return nameWithoutExtension;
            }

            return string.Join(".", nameTokens.Take(nameTokens.Length - 1));
        }
    }
}