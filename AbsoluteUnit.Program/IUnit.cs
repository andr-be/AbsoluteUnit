namespace AbsoluteUnit.Program;

public interface IUnit
{
    object Unit { get; }
    string Symbol { get; }
    double ToBase(double value);
    double FromBase(double value);
}

public class SIBase(SIBase.Units unit) : IUnit
{
    public object Unit { get; } = unit;

    public enum Units
    {
        Meter,
        Gram,
        Second,
        Ampere,
        Kelvin,
        Mole,
        Candela
    }

    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "m", new SIBase(Units.Meter) },
        { "g", new SIBase(Units.Gram) },
        { "s", new SIBase(Units.Second) },
        { "A", new SIBase(Units.Ampere) },
        { "K", new SIBase(Units.Kelvin) },
        { "mole", new SIBase(Units.Mole) },
        { "cd", new SIBase(Units.Candela) },
    };

    public string Symbol => Unit switch
    {
        Units.Meter => "m",
        Units.Gram => "g",
        Units.Second => "s",
        Units.Ampere => "A",
        Units.Kelvin => "K",
        Units.Mole => "mole",
        Units.Candela => "cd",
        _ => throw new NotImplementedException(),
    };

    public double ToBase(double value) => value;

    public double FromBase(double value) => value;
}

public class SIDerived : IUnit
{
    public string Symbol => throw new NotImplementedException();

    public object Unit => throw new NotImplementedException();

    public double FromBase(double value)
    {
        throw new NotImplementedException();
    }

    public double ToBase(double value)
    {
        throw new NotImplementedException();
    }

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

    public static readonly Dictionary<string, object> ValidUnitStrings = [];
}

public class USCustomary(USCustomary.Units unit) : IUnit
{
    public object Unit { get; } = unit;

    readonly Dictionary<Units, double> Conversion = new()
    {
        { Units.Mil, 25.4e-6 },
        { Units.Inch, 25.4e-3 },
        { Units.Feet, 0.3048 },
        { Units.Yards, 0.9144 },
        { Units.Miles, 1.609344e3 },

        { Units.Ounce, 28.349523125 },
        { Units.Pound, 453.59237 },
        { Units.Ton, 1016.0469088e3 },

        { Units.FluidOunce, 29.5735295625e-3 },
        { Units.Pint, 0.473176473 },
        { Units.Gallon, 3.785411784 },
    };

    double FahrenheitToCelcius(double value) =>
        (value - 32) * (5.0 / 9.0);

    double CelciusToFahrenheit(double value) =>
        (value * (9.0 / 5.0)) + 32;

    public string Symbol => Unit switch
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

    public double FromBase(double value) => Unit switch
    {
        Units.Fahrenheit => CelciusToFahrenheit(value),
        _ => value / Conversion[unit]
    };

    public double ToBase(double value) => Unit switch
    {
        Units.Fahrenheit => FahrenheitToCelcius(value),
        _ => value * Conversion[unit],
    };

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
}

public class Miscellaneous : IUnit
{
    public string Symbol => throw new NotImplementedException();

    public object Unit => throw new NotImplementedException();

    public double FromBase(double value)
    {
        throw new NotImplementedException();
    }

    public double ToBase(double value)
    {
        throw new NotImplementedException();
    }

    public static readonly Dictionary<string, object> ValidUnitStrings = [];
}
