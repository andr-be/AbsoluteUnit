using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Factories;

public class TestUnitBuilder
{
    private SIPrefix Prefix = new(SIPrefix.Prefixes._None);
    private IUnitType UnitType = new SIBase(SIBase.Units.Meter);
    private int Exponent = 1;

    public Unit Build() => new(UnitType, Exponent, Prefix);

    public TestUnitBuilder WithPrefix(SIPrefix.Prefixes prefix)
    {
        Prefix = new(prefix);
        return this;
    }

    public TestUnitBuilder WithUnit(IUnitType unit)
    {
        UnitType = unit;
        return this;
    }

    public TestUnitBuilder WithExponent(int exponent)
    {
        Exponent = exponent;
        return this;
    }    
}
