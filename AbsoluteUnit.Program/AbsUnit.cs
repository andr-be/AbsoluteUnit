using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program;

public class AbsUnit(
    IUnit unit, 
    int exponent = 1, 
    SIPrefix? prefix = null
    )
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
    public SIPrefix Prefix { get; set; } = prefix ?? new(SIPrefix.Prefixes._None);

    public override string ToString() => 
        $"{Prefix}{Unit.Symbol}{((Exponent != 1) ? "^" + Exponent : "")}";

    public double ConversionFromBase() => Unit.FromBase(1) * PrefixValue();

    public double ConversionToBase() => Unit.ToBase(1) / PrefixValue();

    public double PrefixValue() => Math.Pow(10.0, Prefix.Prefix.Factor());

    public List<AbsUnit> ExpressInBaseUnits() => MeasurementConverter.BaseConversion(this);

    public override bool Equals(object? obj)
    {
        if (obj is AbsUnit other)
        {
            return other.Unit.Equals(Unit)
                && other.Exponent.Equals(Exponent)
                && other.Prefix.Equals(Prefix);
        }
        return false;
    }
}
