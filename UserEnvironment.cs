using Microsoft.Win32;

namespace CopilotTokenTracker
{
    /// <summary>
    ///  Reads and writes user environment variables straight from <c>HKCU\Environment</c>.
    ///  <para>
    ///   <see cref="Environment.SetEnvironmentVariable(string, string, EnvironmentVariableTarget)"/>
    ///   follows its registry write with a <c>SendMessageTimeout(HWND_BROADCAST, WM_SETTINGCHANGE, ...)</c>
    ///   so Explorer and other shells pick the change up. That call waits on every top-level window
    ///   in the session (roughly a second each for any that are busy), which can stall the UI for
    ///   several seconds. These settings are only consumed by this app, so the broadcast is skipped.
    ///  </para>
    /// </summary>
    internal static class UserEnvironment
    {
        private const string EnvironmentKey = "Environment";

        public static string? Get(string name)
        {
            try
            {
                using var key = Registry.CurrentUser.OpenSubKey(EnvironmentKey);
                return key?.GetValue(name) as string;
            }
            catch
            {
                return null;
            }
        }

        public static void Set(string name, string value)
        {
            using var key = Registry.CurrentUser.CreateSubKey(EnvironmentKey);

            if (key is null)
            {
                throw new InvalidOperationException($@"Could not open HKCU\{EnvironmentKey}.");
            }

            if (string.IsNullOrEmpty(value))
            {
                key.DeleteValue(name, throwOnMissingValue: false);
            }
            else
            {
                key.SetValue(name, value, RegistryValueKind.String);
            }

            // Keep this process in sync; the registry write alone does not refresh the block.
            Environment.SetEnvironmentVariable(name, value, EnvironmentVariableTarget.Process);
        }
    }
}
