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
        public List<ComponentTypes>? ComponentTypes { get; set; }

        public bool IsValid()
        {
            if (UserId == null || UserId == 0) return false;
            if (string.IsNullOrEmpty(DrawingDirectory)) return false;
            if (UnitsOfMeasurement == null || UnitsOfMeasurement.Count == 0) return false;
            if (Colours == null || Colours.Count == 0) return false;
            if (WorkCentres == null || WorkCentres.Count == 0) return false;
            if (ComponentTypes == null || ComponentTypes.Count == 0) return false;

            return true;
        }
    }

    public abstract class ComboBoxSource
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string Label { get; set; }
        public string Code { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }

    public class Colours : ComboBoxSource { }
    public class WorkCentres : ComboBoxSource { }
    public class UnitsOfMeasurement : ComboBoxSource { }
    public class ComponentTypes : ComboBoxSource { }


}
