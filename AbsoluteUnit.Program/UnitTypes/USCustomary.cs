﻿
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.UnitTypes;

public class USCustomary(USCustomary.Units unit) : IUnitType
{
    public static USCustomary Pound() => new(Units.Pound);
    public static USCustomary Inch() => new(Units.Inch);
    public static USCustomary Feet() => new(Units.Feet);
    public static USCustomary Mile() => new(Units.Miles);
    public static USCustomary Fahrenheit() => new(Units.Fahrenheit);
    public object UnitType { get; init; } = unit;

    public enum Units
    {
        // Length
        Mil,
        Inch,
        Feet,
        Yards,
        Miles,
        // Weight
        Ounce,
        Pound,
        Ton,
        // Volume
        FluidOunce,
        Pint,
        Gallon,
        // Temperature
        Fahrenheit
    }


    public string Symbol => UnitType switch
    {
        Units.Mil => "thou",
        Units.Inch => "in",
        Units.Feet => "ft",
        Units.Yards => "yd",
        Units.Miles => "mi",
        Units.Ounce => "oz",
        Units.Pound => "lb",
        Units.Ton => "ton",
        Units.FluidOunce => "floz",
        Units.Pint => "pt",
        Units.Gallon => "gal",
        Units.Fahrenheit => "°F",
        _ => throw new InvalidDataException($"Invalid Unit: {unit}")
    };

    public double FromBase(double value=1.0) => UnitType switch
    {
        Units.Fahrenheit => KelvinToFahrenheit(value),
        _ => value / Conversion[unit]
    };

    public double ToBase(double value=1.0) => UnitType switch
    {
        Units.Fahrenheit => FahrenheitToKelvin(value),
        _ => 1 / FromBase(value),
    };

    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "mil", new USCustomary(Units.Mil) },
        { "thou", new USCustomary(Units.Mil) },

        { "inch", new USCustomary(Units.Inch) },
        { "in", new USCustomary(Units.Inch) },
        { "\"", new USCustomary(Units.Inch) },
        { "in.", new USCustomary(Units.Inch) },

        { "ft", new USCustomary(Units.Feet) },
        { "feet", new USCustomary(Units.Feet) },
        { "foot", new USCustomary(Units.Feet) },
        { "\'", new USCustomary(Units.Feet) },

        { "yd", new USCustomary(Units.Yards) },
        { "yds", new USCustomary(Units.Yards) },
        { "yards", new USCustomary(Units.Yards) },

        { "mi", new USCustomary(Units.Miles) },
        { "mile", new USCustomary(Units.Miles) },
        { "miles", new USCustomary(Units.Miles) },

        { "oz", new USCustomary(Units.Ounce) },
        { "ounce", new USCustomary(Units.Ounce) },

        { "lb", new USCustomary(Units.Pound) },
        { "lbs", new USCustomary(Units.Pound) },
        { "pounds", new USCustomary(Units.Pound) },

        { "ton", new USCustomary(Units.Ton) },
        { "tons", new USCustomary(Units.Ton) },

        { "floz", new USCustomary(Units.FluidOunce) },
        { "fl oz", new USCustomary(Units.FluidOunce) },

        { "pt", new USCustomary(Units.Pint) },
        { "pint", new USCustomary(Units.Pint) },

        { "gal", new USCustomary(Units.Gallon) },
        { "gallon", new USCustomary(Units.Gallon) },
        { "gallons", new USCustomary(Units.Gallon) },

        { "F", new USCustomary(Units.Fahrenheit) },
        { "°F", new USCustomary(Units.Fahrenheit) },
        { "degF", new USCustomary(Units.Fahrenheit) },
    };

    public override bool Equals(object? obj) =>
        obj is USCustomary other &&
        UnitType.Equals(other.UnitType);

    static double FahrenheitToKelvin(double value) =>
        (value - 32) * (5.0 / 9.0) + 273.15;

    static double KelvinToFahrenheit(double value) =>
        value - 273.15 * (9.0 / 5.0) + 32;

    public override int GetHashCode()
    {
        return HashCode.Combine(UnitType, Symbol);
    }

    public List<Unit> ExpressInBaseUnits(Unit unit) => (Units)UnitType switch
    {
        Units.Mil or
        Units.Inch or
        Units.Feet or
        Units.Yards or
        Units.Miles => [SIBase.Meter(unit.Exponent)],

        Units.Ounce or 
        Units.Pound or 
        Units.Ton => [SIBase.Kilogram(unit.Exponent)],

        Units.FluidOunce or
        Units.Pint or 
        Units.Gallon => [SIBase.Meter(3 * unit.Exponent)],

        Units.Fahrenheit => [SIBase.Kelvin(unit.Exponent)],

        _ => throw new NotImplementedException($"No base conversion case implemented for {UnitType}"),
    };

    static readonly Dictionary<Units, double> Conversion = new()
    {
        { Units.Mil, 25.4e-6 },
        { Units.Inch, 25.4e-3 },
        { Units.Feet, 0.3048 },
        { Units.Yards, 0.9144 },
        { Units.Miles, 1609.344 },

        { Units.Ounce, 0.02834952 },
        { Units.Pound, 0.45359237 },
        { Units.Ton, 907.18474 },

        { Units.FluidOunce, 2.957e-5 },
        { Units.Pint, 4.7318e-4 },
        { Units.Gallon, 3.78541e-3 },
    };
}
