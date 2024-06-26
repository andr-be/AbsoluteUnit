﻿using AbsoluteUnit.Program.Interfaces;

namespace AbsoluteUnit.Program.Units;

public class SIDerived : IUnit
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

    public string Symbol => throw new NotImplementedException();

    public object Unit => throw new NotImplementedException();

    public double ToBase(double value) =>
        (Units)Unit == Units.Celsius
            ? value + 273.15
            : value;

    public double FromBase(double value) =>
        (Units)Unit == Units.Celsius
            ? value - 273.15
            : value;

    public static readonly Dictionary<string, object> ValidUnitStrings = [];

    public override bool Equals(object? obj) =>
        obj is SIDerived other &&
        Unit.Equals(other.Unit);
}
