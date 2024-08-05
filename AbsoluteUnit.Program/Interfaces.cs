using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program
{
    public interface ICommand
    {
        public abstract Measurement Run();
    }

    public interface IMeasurementParser
    {
        Measurement ProcessMeasurement();
        Measurement ProcessMeasurement(string measurementString, bool unitOnly = false);
        MeasurementGroup GenerateMeasurementGroup(string measurementString, bool unitOnly = false);
    }
    public interface IUnit
    {
        object Unit { get; }
        string Symbol { get; }
        double ToBase(double value=1.0);
        double FromBase(double value=1.0);
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
