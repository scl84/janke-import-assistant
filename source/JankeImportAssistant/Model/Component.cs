using CsvHelper.Configuration.Attributes;
using System;

namespace JankeImportAssistant.Model
{
    public class Component
    {
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
