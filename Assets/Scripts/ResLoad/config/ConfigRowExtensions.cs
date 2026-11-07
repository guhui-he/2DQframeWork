using System;
using System.Collections.Generic;
using System.Globalization;

namespace Game
{
    public static class ConfigRowExtensions
    {
        public static string GetString(this Dictionary<string, string> row, string key, string defaultValue = "")
            => row.GetValueOrDefault(key, defaultValue);

        public static int GetInt(this Dictionary<string, string> row, string key)
            => int.TryParse(row.GetValueOrDefault(key), out var v) ? v : 0;

        public static float GetFloat(this Dictionary<string, string> row, string key)
            => float.TryParse(row.GetValueOrDefault(key), NumberStyles.Float, CultureInfo.InvariantCulture, out var v) ? v : 0f;

        public static bool GetBool(this Dictionary<string, string> row, string key)
            => bool.TryParse(row.GetValueOrDefault(key), out var v) && v;

        public static T GetEnum<T>(this Dictionary<string, string> row, string key, T defaultValue) where T : struct, Enum
        {
            string raw = row.GetValueOrDefault(key);
            if (string.IsNullOrWhiteSpace(raw) || raw.Trim() == "-")
                return defaultValue;

            if (Enum.TryParse<T>(raw.Trim(), ignoreCase: true, out var value) && Enum.IsDefined(typeof(T), value))
                return value;

            return defaultValue;
        }
    }
}
