using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Structures;

public record Unit(IUnitType UnitType, int Exponent = 1, SIPrefix? Prefix = null)
{
    public IUnitType UnitType { get; init; } = UnitType;
    public int Exponent { get; init; } = Exponent;
    public SIPrefix Prefix { get; init; } = Prefix ?? new();

    public override string ToString() =>
        $"{Prefix}{UnitType.Symbol}{(Exponent != 1 ? "^" + Exponent : "")}";

    public double ConversionFromBase() => UnitType.FromBase() * PrefixValue();

    public double ConversionToBase() => UnitType.ToBase() / PrefixValue();

    public double PrefixValue() => Math.Pow(10.0, Prefix.Prefix.Factor());

    public List<Unit> ExpressInBaseUnits() => MeasurementConverter.BaseConversion(this);

    public override int GetHashCode()
    {
        return HashCode.Combine(UnitType, Exponent, Prefix);
    }
}
