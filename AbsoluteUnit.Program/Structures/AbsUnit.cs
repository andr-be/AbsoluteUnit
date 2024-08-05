using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Structures;

public record AbsUnit(IUnit Unit, int Exponent = 1, SIPrefix? Prefix = null)
{
    public IUnit Unit { get; init; } = Unit;
    public int Exponent { get; init; } = Exponent;
    public SIPrefix Prefix { get; init; } = Prefix ?? new();

    public override string ToString() =>
        $"{Prefix}{Unit.Symbol}{(Exponent != 1 ? "^" + Exponent : "")}";

    public double ConversionFromBase() => Unit.FromBase() * PrefixValue();

    public double ConversionToBase() => Unit.ToBase() / PrefixValue();

    public double PrefixValue() => Math.Pow(10.0, Prefix.Prefix.Factor());

    public List<AbsUnit> ExpressInBaseUnits() => MeasurementConverter.BaseConversion(this);

    public override int GetHashCode()
    {
        return HashCode.Combine(Unit, Exponent, Prefix);
    }
}
