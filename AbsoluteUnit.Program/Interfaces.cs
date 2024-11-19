using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public Measurement Input { get; init; }
        public abstract List<Measurement> Run();
    }

    public interface IMeasurementParser
    {
        Measurement ProcessMeasurement();
        Measurement ProcessMeasurement(string measurementString, bool unitOnly = false);
        MeasurementGroup GenerateMeasurementGroup(string measurementString, bool unitOnly = false);
    }

    public interface IUnitType
    {
        object UnitType { get; }
        string Symbol { get; }
        double ToBase(double value=1.0);
        double FromBase(double value=1.0);
        List<Unit> ExpressInBaseUnits(Unit unit);
    }

    public interface IUnitFactory
    {
        List<Unit> BuildUnits(List<UnitGroup>? unitGroups = null);
    }

    public interface IUnitGroupParser
    {
        List<UnitGroup> ParseUnitGroups(string unitString);
    }
}
