using System.Collections.Generic;


namespace JankeImportAssistant
{
    public class Configuration
    {
        public int? UserId { get; set; }
        public string? DrawingDirectory { get; set; }
        public List<UnitsOfMeasurement>? UnitsOfMeasurement { get; set; }
        public List<Colours>? Colours { get; set; }
        public List<Colours>? WorkCentres { get; set; }

        public bool IsValid()
        {
            if (UserId == null || UserId == 0) return false;
            if (string.IsNullOrEmpty(DrawingDirectory)) return false;
            if (UnitsOfMeasurement == null || UnitsOfMeasurement.Count == 0) return false;
            if (Colours == null || Colours.Count == 0) return false;
            if (WorkCentres == null || WorkCentres.Count == 0) return false;

            return true;
        }
    }

    public abstract class ComboBoxSource
    {
        public string Label { get; set; }
        public string Code { get; set; }
    }

    public class Colours : ComboBoxSource { }
    public class WorkCentres : ComboBoxSource { }
    public class UnitsOfMeasurement : ComboBoxSource { }


}
