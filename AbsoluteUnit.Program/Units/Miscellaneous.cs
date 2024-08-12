using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Units;

public class Miscellaneous : IUnitType
{
    public string Symbol => throw new NotImplementedException();

    public object Unit => throw new NotImplementedException();

    public double FromBase(double value)
    {
        throw new NotImplementedException();
    }

    public double ToBase(double value)
    {
        throw new NotImplementedException();
    }

    public List<Unit> ExpressInBaseUnits()
    {
        throw new NotImplementedException();
    }

    public static readonly Dictionary<string, object> ValidUnitStrings = [];
}