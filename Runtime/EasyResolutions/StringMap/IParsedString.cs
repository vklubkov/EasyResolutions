namespace EasyResolutions {
    public interface IParsedString : IResolution {
        public string Group { get; }
        public string Value { get; }
        public string Extension { get; }
    }
}