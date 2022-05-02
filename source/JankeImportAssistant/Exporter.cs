using CsvHelper;
using JankeImportAssistant.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Xml.Serialization;

namespace JankeImportAssistant
{
    public class Exporter
    {
        private List<PartViewModel> _partViewModelList;

        public Exporter(List<PartViewModel> partViewModelList)
        {
            _partViewModelList = partViewModelList;
        }

        public ExportResult Export()
        {
            var partList = new List<Part>();
            foreach (var partViewModel in _partViewModelList)
            {
                if (!partViewModel.IsCompletePart()) continue;
                partList.Add(BuildPart(partViewModel));
            }

            var outputFile = SaveExport();
            if (string.IsNullOrEmpty(outputFile)) return new ExportResult(false, "No valid file path selected for export");

            var orderedParts = OrderPartsByDependencies(partList);
            if (orderedParts.Count < 1) return new ExportResult(false, "Invalid part hierarchy");

            var fileType = Path.GetExtension(outputFile);

            switch (fileType)
            {
                case ".json":
                    ExportToJson(outputFile, orderedParts);
                    break;
                case ".xml":
                    ExportToXml(outputFile, orderedParts);
                    break;
                case ".csv":
                default:
                    ExportToCsv(outputFile, orderedParts);
                    break;
            }

            return new ExportResult(true, null);
        }

        private string? SaveExport()
        {
            SaveFileDialog saveFileDialog = new()
            {
                DefaultExt = ".csv",
                Filter = "Comma separated value (*.csv)|*.csv|JavaScript Object Notation|*.json|eXtensible Markup Language|*.xml"
            };

            return saveFileDialog.ShowDialog() == true ? saveFileDialog.FileName : null;
        }

        private Part BuildPart(PartViewModel partViewModel)
        {
            if (!string.IsNullOrEmpty(partViewModel.Colour) && !string.IsNullOrEmpty(partViewModel.SurfaceArea))
            {
                partViewModel.Components.Add(new Component("M", partViewModel.Colour, "Kg", "1"));
                partViewModel.Components.Add(new Component("M", "8310-0000", "Kg", "1"));
            }

            return new Part(partViewModel);
        }

        private List<Part> OrderPartsByDependencies(List<Part> parts)
        {
            var results = new List<Part>();
            while (parts.Count > 0)
            {
                var filteredParts = parts.Where(part =>part.Components.All(component => "M".Equals(component.Type) || results.Exists(result => result.PartCode.Equals(component.Build)))).ToList();

                if (filteredParts.Count == 0) return results;

                filteredParts.ForEach(dependencySatisfiedPart =>
                {
                    results.Add(dependencySatisfiedPart);
                    parts.Remove(dependencySatisfiedPart);
                });
            }

            return results;
        }

        private void ExportToCsv(string outputFile, List<Part> orderedParts)
        {
            using (var writer = new StreamWriter(outputFile))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteHeader<Part>();
                csv.WriteHeader<Component>();
                csv.WriteHeader<Labor>();
                csv.NextRecord();

                foreach (var part in orderedParts)
                {
                    foreach (var component in part.Components)
                    {
                        foreach (var labor in part.Labors)
                        {
                            csv.WriteRecord(part);
                            csv.WriteRecord(component);
                            csv.WriteRecord(labor);
                            csv.NextRecord();
                        }
                    }
                }
            }
        }

        private void ExportToJson(string outputFile, List<Part> orderedParts)
        {
            string json = JsonSerializer.Serialize(orderedParts);
            File.WriteAllText(outputFile, json);
        }

        private void ExportToXml(string outputFile, List<Part> orderedParts)
        {
            XmlSerializer writer = new(typeof(List<Part>), new XmlRootAttribute("Parts"));
            FileStream file = File.Create(outputFile);

            writer.Serialize(file, orderedParts);
            file.Close();
        }
    }

    public class ExportResult
    {
        public ExportResult(bool success, string? message)
        {
            Success = success;
            Message = message;
        }

        public bool Success { get; }
        public string? Message { get; }
    }
}
 