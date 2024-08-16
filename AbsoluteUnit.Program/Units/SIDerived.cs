﻿using AbsoluteUnit.Program.Structures;
using static AbsoluteUnit.Program.Units.SIBase;

namespace AbsoluteUnit.Program.Units;

public class SIDerived(SIDerived.Units unit) : IUnitType
{
    public enum Units
    {
        Hertz,
        Radian,
        Steradian,
        Newton,
        Pascal,
        Joule,
        Watt,
        Coulomb,
        Volt,
        Farad,
        Ohm,
        Siemens,
        Weber,
        Tesla,
        Henry,
        Celsius,
        Lumen,
        Lux,
        Becquerel,
        Gray,
        Sievert,
        Katal,
    }

    public string Symbol => Unit switch
    {
        Units.Joule => "J",
        _ => throw new NotImplementedException("TODO: implement the rest of the SI Derived unit symbols"),
    };

    public object Unit { get; init; } = unit;

    public double ToBase(double value) =>
        (Units)Unit == Units.Celsius
            ? value + 273.15
            : value;

    public double FromBase(double value) =>
        (Units)Unit == Units.Celsius
            ? value - 273.15
            : value;

    // TODO: Create all of the derived unit strings...
    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "J", new SIDerived(Units.Joule) },
        { "joule", new SIDerived(Units.Joule) },
        { "joules", new SIDerived(Units.Joule) },
        // FUCK LOADS MORE COMING SOON!
    };

    public override bool Equals(object? obj) =>
        obj is SIDerived other &&
        Unit.Equals(other.Unit);

    public override int GetHashCode() => HashCode.Combine(Symbol, Unit);

    public List<Unit> ExpressInBaseUnits() => (Units)Unit switch
    {
        Units.Hertz => [Second(-1)],
        Units.Becquerel => [Second(-1)],

        Units.Newton => [Kilogram(), Meter(), Second(-2)],
        Units.Pascal => [Kilogram(), Meter(-1), Second(-2)],
        Units.Joule => [Kilogram(), Meter(2), Second(-2)],
        Units.Watt => [Kilogram(), Meter(2), Second(-3)],

        Units.Coulomb => [Second(), Ampere()],
        Units.Volt => [Kilogram(), Meter(-2), Second(-3), Ampere(-1)],
        Units.Farad => [Kilogram(-1), Meter(-2), Second(4), Ampere(2)],
        Units.Ohm => [Kilogram(), Meter(2), Second(-3), Ampere(-2)],

        Units.Siemens => [Kilogram(-1), Meter(-2), Second(3), Ampere(2)],
        Units.Weber => [Kilogram(), Meter(2), Second(-2), Ampere(-1)],
        Units.Tesla => [Kilogram(), Second(-2), Ampere(-1)],
        Units.Henry => [Kilogram(), Meter(2), Second(-2), Ampere(-2)],

        Units.Celsius => [Kelvin()],

        Units.Lumen or
        Units.Lux => [Candela(), Meter(-2)],

        Units.Gray or
        Units.Sievert => [Meter(2), Second(-2)],

        Units.Katal => [Second(-1), Mole()],

        Units.Radian or
        Units.Steradian => [],    // Unitless constant...?!

        _ => throw new NotImplementedException($"{Unit} not currently supported!")
    };
}
