using System;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Windows;

namespace JankeImportAssistant
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            string rawJson = File.ReadAllText(BinaryPath() + @"\configuration.json");
            if (string.IsNullOrEmpty(rawJson)) throw new Exception("Missing configuration file");

            Configuration config =
                JsonSerializer.Deserialize<Configuration>(rawJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                ?? throw new Exception("Invalid configuration file");

            if (!config.IsValid()) throw new Exception("Invalid configuration file, refer to manual");

            var window = new MainWindow(config);
            window.Show();
        }

        private static string? BinaryPath()
        {
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }
    }
}
