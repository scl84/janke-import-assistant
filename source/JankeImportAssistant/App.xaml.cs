using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace JankeImportAssistant
{
    public partial class App : Application
    {
        private const string ConfigMissingErrorMessage = "Missing configuration file";
        private const string ConfigInvalidErrorMessage = "Invalid configuration file, refer to manual";

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string rawJson = File.ReadAllText(BinaryPath() + @"\configuration.json");
            if (string.IsNullOrEmpty(rawJson)) throw new Exception(ConfigMissingErrorMessage);

            Configuration config =
                JsonSerializer.Deserialize<Configuration>(rawJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new Exception(ConfigInvalidErrorMessage);

            if (!config.IsValid()) throw new Exception(ConfigInvalidErrorMessage);

            var window = new MainWindow(config);
            window.Show();
        }

        private static string? BinaryPath()
        {
            // Get directory of the executable for this application
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
