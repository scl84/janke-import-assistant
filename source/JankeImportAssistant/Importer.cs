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

        public List<PartViewModel> Import()
        {
            var partViewModels = new List<PartViewModel>();

            var importJsonText = OpenImport();
            if (string.IsNullOrEmpty(importJsonText)) return partViewModels;

            try
            {
                var partsList = JsonSerializer.Deserialize<List<Part>>(importJsonText, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<Part>();

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
                return new List<PartViewModel>();
            }

            return partViewModels;
           
        }

        private string? OpenImport()
        {
            OpenFileDialog openFileDialog = new();
            openFileDialog.Filter = "JavaScript Object Notation (*.json)|*.json";
            return openFileDialog.ShowDialog() == true ? File.ReadAllText(openFileDialog.FileName) : null;
        }

    }
}
