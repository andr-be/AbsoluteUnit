using AbsoluteUnit.Program;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Interfaces
{
    public interface IMeasurementParser
    {
        AbsMeasurement ProcessMeasurement();
        AbsMeasurement ProcessMeasurement(string measurementString, bool unitOnly = false);
        MeasurementGroup GenerateMeasurementGroup(string measurementString, bool unitOnly = false);
    }
}
