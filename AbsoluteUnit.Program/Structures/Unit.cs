using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Program.Structures;

public class Unit(IUnitType UnitType, int Exponent = 1, SIPrefix? Prefix = null)
{
    public IUnitType UnitType { get; init; } = UnitType;
    public int Exponent { get; init; } = Exponent;
    public SIPrefix Prefix { get; init; } = Prefix ?? new();

    public static Unit OfType(object? unitType, SIPrefix.Prefixes? prefix = null, int exponent = 1)
    {
        ArgumentNullException.ThrowIfNull(unitType);

        IUnitType newType = unitType switch
        {
            SIBase.Units siBase => new SIBase(siBase),
            SIDerived.Units siDerived => new SIDerived(siDerived),
            USCustomary.Units usCustomary => new USCustomary(usCustomary),
            Miscellaneous.Units miscUnit => new Miscellaneous(miscUnit),
            _ => throw new NotImplementedException($"No Unit corresponding to {unitType}!"),
        };

        return new(newType, exponent, new(prefix??SIPrefix.Prefixes._None));
    }
    
    public double ConversionToBase() => UnitType.ToBase() * Prefix.Value;

    public double ConversionFromBase() => UnitType.FromBase() / Prefix.Value;
    
    public override string ToString() =>
        $"{Prefix}{UnitType.Symbol}{(Exponent != 1 ? "^" + Exponent : "")}";

    public override int GetHashCode() => HashCode.Combine(UnitType, Exponent, Prefix);

    public override bool Equals(object? obj)
    {
        if (obj is not Unit other) return false;

        var typeEqual = UnitType.Equals(other.UnitType);
        var exponentEqual = Exponent == other.Exponent;
        var prefixEqual = Prefix.Equals(other.Prefix);

        return typeEqual && exponentEqual && prefixEqual;
    }
}


public static class UnitListExtensions
{
    public static double AggregateToBaseConversionFactors(this List<Unit> units)
    {
        double aggregateFactor = 1.0;

        foreach(var unit in units)
            aggregateFactor *= Math.Pow(unit.ConversionToBase(), unit.Exponent);

        return aggregateFactor;
    }

    public static double AggregateFromBaseConversionFactors(this List<Unit> units)
    {
        double aggregateFactor = 1.0;

        foreach (var unit in units)
            aggregateFactor *= Math.Pow(unit.ConversionFromBase(), unit.Exponent);

        return aggregateFactor;
    }
}

public static class UnitExtensions
{
    public static List<Unit> ExpressInBaseUnits(this Unit unit) => unit.UnitType.ExpressInBaseUnits(unit);
}