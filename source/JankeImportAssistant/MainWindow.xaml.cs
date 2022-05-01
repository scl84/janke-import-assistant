using JankeImportAssistant.Model;
using System.Collections.Generic;
using System.Windows;


namespace JankeImportAssistant
{    public partial class MainWindow : Window
    {
        private readonly Configuration _configuration;
        private List<Part> _partList;
        private Part _part;
        private int _index = 0;

        public MainWindow(Configuration config) : base()
        {
            _configuration = config;
            _partList = new List<Part>();
            _part = new Part(_configuration);
            DataContext = _part;
            InitializeComponent();
        }
    }
}
