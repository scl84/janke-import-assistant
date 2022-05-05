using System;
using System.Collections.Generic;
using System.Windows;
using JankeImportAssistant.Model;

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
                return;
            }

            _index = _partList.Count;
            _part = new PartViewModel(_configuration, _index + 1, _partList.Count + 1);
            _partList.Add(_part);
            DataContext = _part;
        }
        private void Import(object sender, RoutedEventArgs e)
        {
            var importer = new Importer(_configuration);
            var importedData = importer.Import();

            if (importedData == null || importedData.Count == 0)
            {
                MessageBox.Show("Unable to import parts, check file validity and try again", "Import Parts", MessageBoxButton.OK, MessageBoxImage.Warning, MessageBoxResult.No);
                return;
            }

            _partList = importedData;
            _part = importedData[0];
            _index = 0;
            DataContext = _part;
        }


        private void Export(object sender, RoutedEventArgs e)
        {
            var exporter = new Exporter(_partList);
            var result = exporter.Export();

            switch (result.Status)
            {
                case ExportStatus.Cancel:
                    return;
                case ExportStatus.Ok:
                    _partList.Clear();
                    _part = new PartViewModel(_configuration, 1, 1);
                    _partList.Add(_part);
                    DataContext = _part;
                    return;
                case ExportStatus.Error when !string.IsNullOrEmpty(result.Message):
                    MessageBox.Show(result.Message, "Export Issue", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.No);
                    break;
                default:
                    throw new Exception("Export failed with unknown status");
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
