using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;

namespace JankeImportAssistant
{
    public partial class App : Application
    {
        private const string ConfigMissingErrorMessage = "Missing configuration file";
        private const string ConfigInvalidErrorMessage = "Invalid configuration file, refer to manual";

        protected override void OnStartup(StartupEventArgs e)
        {
            SetupUnhandledExceptionHandling();
            base.OnStartup(e);
            string rawJson;
            Configuration config;

            try
            {
                rawJson = File.ReadAllText(BinaryPath() + @"\configuration.json");
                config = JsonSerializer.Deserialize<Configuration>(rawJson, new JsonSerializerOptions { PropertyNameCaseInsensitive = true })
                    ?? throw new Exception(ConfigInvalidErrorMessage);
            }
            catch (FileNotFoundException)
            {
                throw new Exception(ConfigMissingErrorMessage);
            }
            catch (JsonException)
            {
                throw new Exception(ConfigInvalidErrorMessage);
            }

            if (!config.IsValid()) throw new Exception(ConfigInvalidErrorMessage);

            var window = new MainWindow(config);
            window.Show();
        }

        private static string? BinaryPath()
        {
            // Get directory of the executable for this application
            return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
        }

        // Unhandled exception behaviour modified from https://blog.danskingdom.com/Catch-and-display-unhandled-exceptions-in-your-WPF-app/
        private void SetupUnhandledExceptionHandling()
        {
            // Catch exceptions from all threads in the AppDomain.
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
                ShowUnhandledException(args.ExceptionObject as Exception);

            // Catch exceptions from each AppDomain that uses a task scheduler for async operations.
            TaskScheduler.UnobservedTaskException += (sender, args) =>
                ShowUnhandledException(args.Exception);

            // Catch exceptions from a single specific UI dispatcher thread.
            Dispatcher.UnhandledException += (sender, args) =>
            {
                // If we are debugging, let Visual Studio handle the exception and take us to the code that threw it.
                if (!Debugger.IsAttached)
                {
                    args.Handled = true;
                    ShowUnhandledException(args.Exception);
                }
            };
        }

        void ShowUnhandledException(Exception e)
        {
            var messageBoxTitle = "Janke Import Assistant";
            var messageBoxMessage = $"The following exception occurred:\n\n{e.Message}";
            var messageBoxButtons = MessageBoxButton.OK;

            // Let the user decide if the app should die or not (if applicable).
            if (MessageBox.Show(messageBoxMessage, messageBoxTitle, messageBoxButtons) == MessageBoxResult.OK)
            {
                Current.Shutdown(-1);
            }
        }
    }
}
