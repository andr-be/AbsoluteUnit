using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Interfaces;

public interface IUnitFactory
{
    List<AbsUnit> BuildUnits(List<UnitGroup>? unitGroups = null);
}
