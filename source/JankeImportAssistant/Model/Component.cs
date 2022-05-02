using CsvHelper.Configuration.Attributes;

namespace JankeImportAssistant.Model
{
    public class Component
    {
// Parameterless constructor for serialisation
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public Component() { }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        public Component(string type, string build, string unit, string quantity)
        {
            Type = type;
            Build = build;
            Unit = unit;
            Quantity = quantity;
        }

        [Index(0)]
        [Name("component_type")]
        public string Type { get; set; }

        [Index(1)]
        [Name("component_build")]
        public string Build { get; set; }

        [Index(3)]
        [Name("component_unit")]
        public string Unit { get; set; }

        [Index(4)]
        [Name("component_quantity")]
        public string Quantity { get; set; }
    }
}
