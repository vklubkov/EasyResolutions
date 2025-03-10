using System.Collections.Generic;

namespace EasyResolutions {
    public interface ISettings {
        public static ISettings Instance { get; protected set; }

        public double Epsilon { get; }

        public IParsedString ZeroScene { get; }
        public string StartingSceneGroup { get; }
        public Dictionary<string, List<IParsedString>> Scenes { get; }

        public IEasyResolutionsComparer DefaultComparer { get; }
        public Dictionary<string, IEasyResolutionsComparer> Comparers { get; }

        public List<EasyResolutionsStringMapBase> StringMaps { get; }
    }
}