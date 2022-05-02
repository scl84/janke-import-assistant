﻿using System.Collections.ObjectModel;

namespace JankeImportAssistant.Model
{
    public class PartViewModel
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public PartViewModel(Configuration configuration, int currentRecord, int totalRecords)
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        {
            Configuration = configuration;
            CurrentRecord = currentRecord;
            TotalRecords = totalRecords;
        }

        public int? UserId => Configuration.UserId;
        public string PartNumber { get; set; }
        public string Description { get; set; }
        public string Revision { get; set; }
        public string Multi { get; set; }
        public string Group { get; set; }
        public string LeadTime { get; set; }
        public string? Colour { get; set; }
        public string? SurfaceArea { get; set; }
        public ObservableCollection<Component> Components { get; } = new ObservableCollection<Component>();
        public ObservableCollection<Labor> Labors { get; } = new ObservableCollection<Labor>();
        public Configuration Configuration { get; set; }
        public int CurrentRecord { get; set; }
        public int TotalRecords { get; set; }

        public string GetInfoFileName()
        {
            return $"{Configuration.DrawingDirectory}/{PartNumber}_{Revision}.pdf";
        }

        public bool IsCompletePart()
        {
            if (string.IsNullOrEmpty(PartNumber) ||
                string.IsNullOrEmpty(Description) ||
                string.IsNullOrEmpty(Revision) ||
                string.IsNullOrEmpty(Multi) ||
                string.IsNullOrEmpty(Group) ||
                string.IsNullOrEmpty(LeadTime) ||
                Components.Count < 1 ||
                Labors.Count < 1)
            {
                return false;
            }

            return true;
        }
    }
}
