using System.Collections.Generic;
using System.Linq;

namespace EasyResolutions {
    // ReSharper disable InconsistentNaming as these extensions are for specifically IList
    public static class IListExtensions {
        // ReSharper restore InconsistentNaming
        public static void ClearPairedValues<T>(this IList<string> keys, IList<T> values) {
            keys.Clear();
            values.Clear();
        }

        public static void AddPairedValues<T>(
            this IList<string> keys, IList<T> values, IDictionary<string, T> old, string key, T defaultValue) {
            keys.Add(key);
            values.Add(old.TryGetValue(key, out var value) ? value : defaultValue);
        }

        public static void AddPairedValues<T>(
            this IList<string> keys, IList<T> values, IList<(string Key, T Value)> old, string key, T defaultValue) {
            keys.Add(key);

            var found = old.FirstOrDefault(kvp => kvp.Key == key);
            if (found.Key == null)
                values.Add(defaultValue);
            else {
                values.Add(found.Value);
                var index = old.IndexOf(found);
                old.RemoveAt(index);
            }
        }
    }
}