namespace EasyResolutions {
    /// Parsing examples:<br/>
    /// - ("abc", "") -> { Group: "abc", Value: "abc", Width: -1, Height: -1, Extension: "" },<br/>
    /// - ("abc.txt", "") -> { Group: "abc.txt", Value: "abc.txt", Width: -1, Height: -1, Extension: "" },<br/>
    /// - ("abc.1366x768", "") -> { Group: "abc", Value: "abc.1366x768", Width: 1366, Height: 768, Extension: "" },<br/>
    /// - ("abc.1366x768.txt", "") -> { Group: "abc.1366x768.txt", Value: "abc.1366x768.txt", Width: -1, Height: -1, Extension: "" },<br/>
    /// - ("abc.txt", "txt") -> { Group: "abc", Value: "abc.txt", Width: -1, Height: -1, Extension: "txt" },<br/>
    /// - ("abc.1366x768.txt", "txt") -> { Group: "abc", Value: "abc.1366x768.txt", Width: 1366, Height: 768, Extension: "txt" },<br/>
    /// - ("abc", "txt") -> { Group: "abc", Value: "abc.txt", Width: -1, Height: -1, Extension: "txt" },<br/>
    /// - ("abc.1366x768", "txt") -> { Group: "abc", Value: "abc.1366x768.txt", Width: 1366, Height: 768, Extension: "txt" },<br/>
    /// - ("abc", "png") -> { Group: "abc", Value: "abc.png", Width: -1, Height: -1, Extension: "png" },<br/>
    /// - ("abc.txt", "png") -> { Group: "abc.txt", Value: "abc.txt.png", Width: -1, Height: -1, Extension: "png" },<br/>
    /// - ("abc.1366x768", "png") -> { Group: "abc", Value: "abc.1366x768.png", Width: 1366, Height: 768, Extension: "png" },<br/>
    /// - ("abc.1366x768.txt", "png") -> { Group: "abc.1366x768.txt", Value: "abc.1366x768.txt.png", Width: -1, Height: -1, Extension: "png" }
    public class ParsedString : IParsedString {
        /// Name of the group this string belongs to:
        /// Value minus Extension minus WidthxHeight.
        public string Group { get; }
        
        /// String with any `\`'s replaced with `/`'s
        /// and added Extension, if missing.
        public string Value { get; }
        
        /// Width or -1.
        public int Width { get; }
        
        /// Height or -1.
        public int Height { get; }

        /// Extension or empty string.
        public string Extension { get;}

        /// Constructor.
        /// <param name="value">any string</param>
        /// <param name="extension">optional extension</param>
        public ParsedString(string value, string extension) {
            var sanitizedString = value.Replace('\\', '/');

            string stringWithoutExtension;
            if (string.IsNullOrEmpty(extension)) {
                Value = stringWithoutExtension = sanitizedString;
                Extension = string.Empty;
            }
            else {
                var trimmedExtension = extension.TrimStart('.');
                Extension = trimmedExtension;
                var extensionWithDot = $".{trimmedExtension}";
                if (value.EndsWith(extensionWithDot)) {
                    Value = sanitizedString;
                    stringWithoutExtension = Value[..^extensionWithDot.Length];
                }
                else {
                    stringWithoutExtension = sanitizedString;
                    Value = stringWithoutExtension + "." + trimmedExtension;
                }
            }

            Group = stringWithoutExtension.ParseResolution(out var width, out var height);

            Width = width;
            Height = height;
        }
    }
}