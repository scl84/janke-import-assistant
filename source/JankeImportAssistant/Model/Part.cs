using CsvHelper.Configuration.Attributes;
using System;
using System.Collections.ObjectModel;

namespace JankeImportAssistant.Model
{
    public class Part
    {
        public Part(PartViewModel viewModel)
        {
            UserId = viewModel.UserId.ToString();
            PartNumber = viewModel.PartNumber;
            Description = viewModel.Description;
            Revision = viewModel.Revision;
            Multi = viewModel.Multi;
            Group = viewModel.Group;
            LeadTime = viewModel.LeadTime;
            Components = viewModel.Components;
            Labors = viewModel.Labors;
            InfoFilename = viewModel.GetInfoFileName();
        }

        [Index(0)]
        [Name("user_id")]
        public string? UserId { get; set; }

        [Index(1)]
        [Name("part_number")]
        public string PartNumber { get; set; }

        [Index(2)]
        [Name("part_code")]
        public string PartCode => PartNumber;

        [Index(3)]
        [Name("client_part_number")]
        public string ClientPartNumber => PartNumber;

        [Index(4)]
        [Name("drawing_number")]
        public string DrawingNumber => PartNumber;

        [Index(5)]
        [Name("description")]
        public string Description { get; set; }

        [Index(6)]
        [Name("revision")]
        public string Revision { get; set; }

        [Index(7)]
        [Name("multi")]
        public string Multi { get; set; }

        [Index(8)]
        [Name("unit")]
        public string Unit => "EA";

        [Index(9)]
        [Name("group")]
        public string Group { get; set; }

        [Index(10)]
        [Name("is_active")]
        [BooleanTrueValues("yes")]
        [BooleanFalseValues("no")]
        public bool IsActive => true;

        [Index(11)]
        [Name("lead_time")]
        public string LeadTime { get; set; }

        [Index(12)]
        [Name("manu")]
        [BooleanTrueValues("yes")]
        [BooleanFalseValues("no")]
        public bool IsManufactured => true;

        [Index(13)]
        [Name("components")]
        public ObservableCollection<Component> Components { get; } = new ObservableCollection<Component>();

        [Index(14)]
        [Name("labor")]
        public ObservableCollection<Labor> Labors { get; } = new ObservableCollection<Labor>();

        [Index(15)]
        [Name("info_description")]
        public string InfoDescription => "DRAWING";

        [Index(16)]
        [Name("info_revision")]
        public string InfoRevision => Revision;

        [Index(17)]
        [Name("info_jt-print")]
        [BooleanTrueValues("yes")]
        [BooleanFalseValues("no")]
        public bool InfoJtPrint => false;

        [Index(18)]
        [Name("info_q-email")]
        [BooleanTrueValues("yes")]
        [BooleanFalseValues("no")]
        public bool InfoQEmail => true;

        [Index(19)]
        [Name("info_filename")]
        public string InfoFilename { get; set; }
    }
}
