﻿using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.UnitTypes;

public class Miscellaneous(Miscellaneous.Units unit) : IUnitType
{
    public enum Units
    {
        // Time
        Year,
        Month,
        Day,
        Hour,
        Minute,
    }

    public string Symbol => UnitType switch
    {
        Units.Year => "yr",
        Units.Month => "mth",
        Units.Day => "day",
        Units.Hour => "h",
        Units.Minute => "min",
        _ => throw new NotImplementedException($"{UnitType} has no Symbol implementation!")
    };

    public object UnitType { get; init; } = unit;

    public double FromBase(double value=1) => value / Conversion[(Units)UnitType];

    public double ToBase(double value=1) => value * Conversion[(Units)UnitType];

    public List<Unit> ExpressInBaseUnits(Unit unit) => (Units)UnitType switch
    {
        Units.Year or
        Units.Month or
        Units.Day or
        Units.Hour or
        Units.Minute => [SIBase.Second(unit.Exponent)],

        _ => throw new NotImplementedException($"No base unit conversion for {UnitType}")
    };

    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "yr", new Miscellaneous(Units.Year) },
        { "yrs", new Miscellaneous(Units.Year) },
        { "year", new Miscellaneous(Units.Year) },
        { "years", new Miscellaneous(Units.Year) },
        
        { "mth", new Miscellaneous(Units.Month) },
        { "mths", new Miscellaneous(Units.Month) },
        { "month", new Miscellaneous(Units.Month) },
        { "months", new Miscellaneous(Units.Month) },
        
        { "day", new Miscellaneous(Units.Day) },
        { "days", new Miscellaneous(Units.Day) },
        
        { "h", new Miscellaneous(Units.Hour) },
        { "hr", new Miscellaneous(Units.Hour) },
        { "hrs", new Miscellaneous(Units.Hour) },
        { "hour", new Miscellaneous(Units.Hour) },
        { "hours", new Miscellaneous(Units.Hour) },
        
        { "min", new Miscellaneous(Units.Minute) },
        { "mins", new Miscellaneous(Units.Minute) },
    };

    static readonly Dictionary<Units, double> Conversion = new()
    {
        { Units.Year,  31_536_000 },
        { Units.Month,  2_628_000 },
        { Units.Day,       86_400 },
        { Units.Hour,       3_600 },
        { Units.Minute,        60 },
    };

    public override bool Equals(object? obj)
    {
        if (obj is not Miscellaneous other) return false;

        var unitEquals = other.UnitType.Equals(UnitType);

        return unitEquals;
    }

    public override int GetHashCode() => HashCode.Combine(UnitType, Symbol);
}