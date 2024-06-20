using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Interfaces;

public interface IUnitGroupParser
{
    List<UnitGroup> ParseUnitGroups(string unitString);
}
