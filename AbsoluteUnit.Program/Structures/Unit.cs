using AbsoluteUnit.Program.Commands;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Structures;

public record Unit(IUnitType UnitType, int Exponent = 1, SIPrefix? Prefix = null)
{
    public IUnitType UnitType { get; init; } = UnitType;
    public int Exponent { get; init; } = Exponent;
    public SIPrefix Prefix { get; init; } = Prefix ?? new();

    public double ConversionFromBase() => UnitType.FromBase() * PrefixValue();

    public double ConversionToBase() => UnitType.ToBase() / PrefixValue();
    
    public double PrefixValue() => Math.Pow(10.0, Prefix.Prefix.Factor());

    public override string ToString() =>
        $"{Prefix}{UnitType.Symbol}{(Exponent != 1 ? "^" + Exponent : "")}";

    public override int GetHashCode() => HashCode.Combine(UnitType, Exponent, Prefix);

    public static Unit BuildUnit
    (
        IUnitType unitType,
        int exponent = 1,
        SIPrefix.Prefixes prefix = SIPrefix.Prefixes._None

    ) => new(unitType, exponent, new(prefix));
}


public static class UnitListExtensions
{
    public static double AggregateConversionFactors(this List<Unit> units) => units
        .Select(u => u.ConversionToBase())
        .Aggregate((x, y) => x * y);
}

public static class UnitExtensions
{
    public static List<Unit> BaseConversion(this Unit unit) => unit.UnitType.ExpressInBaseUnits();
}