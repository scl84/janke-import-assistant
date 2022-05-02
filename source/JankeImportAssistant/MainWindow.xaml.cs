using JankeImportAssistant.Model;
using System.Collections.Generic;
using System.Windows;


namespace JankeImportAssistant
{    public partial class MainWindow : Window
    {
        private readonly Configuration _configuration;
        private List<PartViewModel> _partList = new();
        private PartViewModel _part;
        private int _index = 0;

        public MainWindow(Configuration config) : base()
        {
            _configuration = config;
            _part = new PartViewModel(_configuration, 1, 1);
            _partList.Add(_part);
            DataContext = _part;
            InitializeComponent();
        }

        private void AddPart(object sender, RoutedEventArgs e)
        {
            if (!_part.IsCompletePart())
            {
                MessageBox.Show("Complete this part before adding a new one", "Add Part", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.No);
            }

            _index = _partList.Count;
            _part = new PartViewModel(_configuration, _index + 1, _partList.Count + 1);
            _partList.Add(_part);
            DataContext = _part;
        }

        private void Export(object sender, RoutedEventArgs e)
        {
            var exporter = new Exporter(_partList);
            var result = exporter.Export();

            if (ExportStatus.Cancel.Equals(result.Status)) return;

            if (ExportStatus.Ok.Equals(result.Status))
            {
                _partList.Clear();
                _part = new PartViewModel(_configuration, 1, 1);
                _partList.Add(_part);
                DataContext = _part;
                return;
            }

            if (ExportStatus.Error.Equals(result.Status) && !string.IsNullOrEmpty(result.Message))
            {
                MessageBox.Show(result.Message, "Export Issue", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
            }
        }


        private void Previous(object sender, RoutedEventArgs e)
        {
            if (_index == 0)
            {
                MessageBox.Show("No further records", "Part History", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
                return;
            }

            _index--;
            _part = _partList[_index];
            _part.TotalRecords = _partList.Count;
            DataContext = _part;
        }

        private void Next(object sender, RoutedEventArgs e)
        {
            if (_index == _partList.Count - 1)
            {
                MessageBox.Show("No further records", "Part History", MessageBoxButton.OK, MessageBoxImage.Information, MessageBoxResult.No);
                return;
            }

            _index++;
            _part = _partList[_index];
            DataContext = _part;
        }
    }
}
