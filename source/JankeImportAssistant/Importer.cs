using JankeImportAssistant.Model;
using Microsoft.Win32;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Globalization;

namespace JankeImportAssistant
{
    public class Importer
    {
        private readonly Configuration configuration;

        public Importer(Configuration configuration)
        {
            this.configuration = configuration;
        }

        public ImportResult Import()
        {
            var partViewModels = new List<PartViewModel>();

            var importJsonText = OpenImport();
            if (string.IsNullOrEmpty(importJsonText)) return new ImportResult(ImportStatus.Cancel);

            try
            {
                var partsList = JsonSerializer.Deserialize<List<Part>>(importJsonText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Part>();

                var duplicatePartNumbers = partsList.GroupBy(y => y.PartNumber).Where(x => x.Count() > 1).Select(z => z.Key).ToList();
                if (duplicatePartNumbers != null && duplicatePartNumbers.Any())
                {
                    var message = $"Import contains duplicate parts, correct source file before proceeding\n\nPart Numbers:\n{string.Join(",", duplicatePartNumbers)}";
                    return new ImportResult(ImportStatus.Error, null, message);
                }

                for (int i = 0; i < partsList.Count; i++)
                {
                    var partViewModel = new PartViewModel
                    {
                        Configuration = configuration,
                        PartNumber = partsList[i].PartNumber,
                        Description = partsList[i].Description,
                        Revision = partsList[i].Revision,
                        Multi = partsList[i].Multi,
                        Group = partsList[i].Group,
                        LeadTime = partsList[i].LeadTime,
                        CurrentRecord = i + 1,
                        TotalRecords = partsList.Count
                    };

                    partsList[i].Labors.ToList().ForEach(l => partViewModel.Labors.Add(l));
                    partsList[i].Components.ToList().ForEach(l =>
                    {
                        if (configuration.Colours!.Exists(c => c.Code.Equals(l.Build)))
                        {
                            partViewModel.Colour = l.Build;
                            partViewModel.SurfaceArea = (decimal.Parse(l.Quantity, CultureInfo.InvariantCulture) / App.PaintCoefficient).ToString();
                        }
                        else if (!App.ZincUndercoatCode.Equals(l.Build))
                        {
                            partViewModel.Components.Add(l);
                        }
                    });

                    partViewModels.Add(partViewModel);
                }
            } catch (JsonException)
            {
                return new ImportResult(ImportStatus.Error, null, "Unable to import parts, check file validity and try again");
            }

            return new ImportResult(ImportStatus.Ok, partViewModels, null);
           
        }

        private string? OpenImport()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
            return openFileDialog.ShowDialog() == true ? File.ReadAllText(openFileDialog.FileName) : null;
        }

    }

    public class ImportResult
    {
        public ImportResult(ImportStatus status)
        {
            Status = status;
        }

        public ImportResult(ImportStatus status, List<PartViewModel>? parts, string? message)
        {
            Status = status;
            Parts = parts;
            Message = message;
        }

        public ImportStatus Status { get; }
        public List<PartViewModel>? Parts { get; }
        public string? Message { get; }

    }

    public enum ImportStatus
    {
        Ok,
        Error,
        Cancel
    }
}
