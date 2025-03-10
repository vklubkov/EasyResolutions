using System.Collections.Generic;

namespace EasyResolutions {
    public interface IEasyResolutionsPicker {
        public void AddScenes(List<string> scenes);

        public string PickResolution(string source);

        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight);

        public string PickResolution(string source,
                                     IEasyResolutionsComparer overrideComparer);

        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight,
                                     IEasyResolutionsComparer overrideComparer);

        public string PickResolution<T>(string source,
                                        T identifier);

        public string PickResolution<T>(string source,
                                        T identifier,
                                        int targetWidth,
                                        int targetHeight);

        public string PickResolution<T>(string source,
                                        T identifier,
                                        IEasyResolutionsComparer overrideComparer);

        public string PickResolution<T>(string source,
                                        T identifier,
                                        int targetWidth,
                                        int targetHeight,
                                        IEasyResolutionsComparer overrideComparer);

        public string PickResolution(string source,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity");

        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity");

        public string PickResolution(string source,
                                     IEasyResolutionsComparer overrideComparer,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity");

        public string PickResolution(string source,
                                     int targetWidth,
                                     int targetHeight,
                                     IEasyResolutionsComparer overrideComparer,
                                     Dictionary<string, List<IParsedString>> stringMap,
                                     string extension = "unity");
        }
}