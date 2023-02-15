using System;
using System.Collections.Generic;
using System.Linq;

namespace win_short_cut.Utils {
    public static class Environment {
        public const string VAR_NAME_PATH = "PATH";

        private const EnvironmentVariableTarget DEFAULT_TARGET = EnvironmentVariableTarget.User;
        public static string[] GetEnvVar(string variable, EnvironmentVariableTarget target = DEFAULT_TARGET) {
            var value = System.Environment.GetEnvironmentVariable(variable, target);
            if (value == null)
                return Array.Empty<string>();

            return value.Split(';');
        }

        public static bool SetEnvVar(string variable, IEnumerable<string> value, EnvironmentVariableTarget target = DEFAULT_TARGET) {
            System.Environment.SetEnvironmentVariable(variable, string.Join(';', value), target);
            return true;
        }

        public static bool IsInEnvVar(string variable, string path, EnvironmentVariableTarget target = DEFAULT_TARGET) {
            return GetEnvVar(variable, target).Contains(path);
        }

        public static bool RemoveFromEnvVar(string variable, string path, EnvironmentVariableTarget target = DEFAULT_TARGET) {
            return SetEnvVar(variable, GetEnvVar(variable, target).Where(s => !s.Equals(path)), target);
        }

        public static bool AddToEnvVar(string variable, string path, EnvironmentVariableTarget target = DEFAULT_TARGET) {
            // prevent adding the same path multiple times
            if (IsInEnvVar(variable, path, target))
                return true;

            return SetEnvVar(variable, GetEnvVar(variable, target).Concat(new string[] { path }), target);
        }

        public static string[] GetPath() => GetEnvVar(VAR_NAME_PATH);

        public static bool IsInPath(string path) => IsInEnvVar(VAR_NAME_PATH, path);

        public static bool RemoveFromPath(string path) => RemoveFromEnvVar(VAR_NAME_PATH, path);

        public static bool AddToPath(string path) => AddToEnvVar(VAR_NAME_PATH, path);
    }
}
