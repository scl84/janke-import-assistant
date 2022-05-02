using CsvHelper.Configuration.Attributes;

namespace JankeImportAssistant.Model
{
    public class Labor
    {
// Parameterless constructor for serialisation
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Labor() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Labor(string workCentre, string setupTime, string productionTime, string description)
        {
            WorkCentre = workCentre;
            SetupTime = setupTime;
            ProductionTime = productionTime;
            Description = description;
        }

        [Index(0)]
        [Name("labor_wc")]
        public string WorkCentre { get; set; }

        [Index(1)]
        [Name("labor_set")]
        public string SetupTime { get; set; }

        [Index(3)]
        [Name("labor_pro")]
        public string ProductionTime { get; set; }

        [Index(4)]
        [Name("labor_description")]
        public string Description { get; set; }

    }
}
