using System.Collections.Generic;

namespace EasyResolutions {
    internal interface IStringMap {
        public string Extension { get; }
        public IEasyResolutionsComparer ComparerOverride { get; }
        public Dictionary<string, List<IParsedString>> Strings { get; }
        public Dictionary<string, IEasyResolutionsComparer> Comparers { get; }
    }

    internal interface IStringMap<out T> : IStringMap {
        public T Identifier { get; }
    }
}