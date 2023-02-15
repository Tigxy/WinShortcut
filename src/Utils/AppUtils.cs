using System;

namespace win_short_cut.Utils {
    public static class AppUtils {
        /// <summary>
        /// Tries to retrieve the version of the current application.
        /// In case the app has been deployed via ClickOnce, it will retrieve the corresponding ClickOnce version number.
        /// </summary>
        /// <returns>The current applications version</returns>
        public static Version? GetAppVersion() {
            Version? currentVersion;
            bool.TryParse(System.Environment.GetEnvironmentVariable("ClickOnce_IsNetworkDeployed"), out bool isNetworkDeployed);
            if (isNetworkDeployed) {
                Version.TryParse(System.Environment.GetEnvironmentVariable("ClickOnce_CurrentVersion"), out currentVersion);
            }
            else {
                currentVersion = System.Reflection.Assembly.GetEntryAssembly()?.GetName().Version;
            }
            return currentVersion;
        }
    }
}
