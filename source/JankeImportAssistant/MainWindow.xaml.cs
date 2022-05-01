using System.Windows;


namespace JankeImportAssistant
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly Configuration _configuration;

        public MainWindow()
        {
            InitializeComponent();
        }

        public MainWindow(Configuration config) : base()
        {
            _configuration = config;
        }
    }
}
