namespace AbsoluteUnit.Program;

public interface IUnit
{
    object Unit { get; }
    string Symbol { get; }
    double ToBase(double value);
    double FromBase(double value);
}

public class AbsUnit(IUnit unit, int exponent, SIPrefix prefix)
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
    public SIPrefix Prefix { get; set; } = prefix;
}

public class UnitFactory
{
    public List<UnitGroup> UnitGroups { get; private set; }

    public UnitFactory(List<UnitGroup> unitGroups) => UnitGroups = unitGroups;

    public UnitFactory(UnitGroup unitGroup) => UnitGroups = [unitGroup];

    private List<Dictionary<string, object>> ValidSymbols = 
    [ 
        SIBase.ValidUnitStrings, 
        SIDerived.ValidUnitStrings,
        USCustomary.ValidUnitStrings, 
        Miscellaneous.ValidUnitStrings,
    ];

    public List<AbsUnit> BuildUnits()
    {
        PropagateExponents();
        ValidateSymbols();
        EvaluatePrefixes();

        return UnitGroups
            .Select(g => CreateUnit(g))
            .ToList();
    }

    private void PropagateExponents()
    {
        GroupLikeSymbols();
        UnitGroups = UnitGroups
            .Where(ug => ug.Operation == UnitGroup.UnitOperation.Divide)
            .Count() switch
        {
            0 => UnitGroups,
            1 => SimplePropagation(UnitGroups),
            _ => ComplexPropagation(UnitGroups),
        };
    }

    private static List<UnitGroup> SimplePropagation(List<UnitGroup> input)
    {
        bool reachedDivisionSign = false;

        return input
            .Select(current =>
            {
                if (current.Operation == UnitGroup.UnitOperation.Divide)
                    reachedDivisionSign = true;

                return reachedDivisionSign 
                    ? current with { Exponent = current.Exponent * -1 } 
                    : current;
            })
            .ToList();
    }

    private List<UnitGroup> ComplexPropagation(List<UnitGroup> input)
    {
        return input
            .Select(current =>
            {
                return (current.Operation == UnitGroup.UnitOperation.Divide)
                    ? current = current with { Exponent = current.Exponent * -1 }
                    : current;
            })
            .ToList();
    }

    private void GroupLikeSymbols()
    {
        var groupedUnits = UnitGroups.GroupBy(ug => ug.UnitSymbol)
            .Select(group =>
            {
                var operation = group.First().Operation;
                var symbol = group.Key;
                var exponent = group.Sum(ug => ug.Exponent);
                return new UnitGroup(operation, symbol, exponent);
            })
            .ToList();
        UnitGroups = groupedUnits;
    }

    private void ValidateSymbols()
    {
        foreach (var unit in UnitGroups)
        {
            bool valid = false;
            var current = unit.UnitSymbol;

            foreach (var dict in ValidSymbols)
            {
                if (dict.ContainsKey(current))
                {
                    valid = true;
                    break;
                }
            }

            if (!valid) 
                throw new KeyNotFoundException($"{unit.UnitSymbol} is not a supported unit!");
        }
    }

    private void EvaluatePrefixes()
    {
        return;
    }

    private AbsUnit CreateUnit(UnitGroup group)
    {
        foreach (var stringDict in ValidSymbols)
        {
            var prefix = TryGetPrefix(group.UnitSymbol[0]);
            stringDict.TryGetValue(group.UnitSymbol, out var unit);
            if (unit is not null)
                return new AbsUnit((IUnit)unit, group.Exponent, prefix);
        }
        throw new KeyNotFoundException($"{group.UnitSymbol} not found in key database...");
    }

    private static SIPrefix TryGetPrefix(char firstChar)
    {
        SIPrefix.ValidPrefixStrings.TryGetValue($"{firstChar}", out var parsedPrefix);
        return parsedPrefix;
    }
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

    Dictionary<Units, double> Conversion = new()
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
        Units.Pound => "lbs",
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

public class SIPrefix(SIPrefix.Prefix prefix)
{
    Prefix _prefix = prefix;

    public enum Prefix
    {
        Quetta = 30,
        Ronna = 27,
        Yotta = 24,
        Zetta = 21,
        Exa = 18,
        Peta = 15,
        Tera = 12,
        Giga = 9,
        Mega = 6,
        Kilo = 3,
        Hecto = 2,
        Deca = 1,
        // Base case is 0!
        Deci = -1,
        Centi = -2,
        Milli = -3,
        Micro = -6,
        Nano = -9,
        Pico = -12,
        Femto = -15,
        Atto = -18,
        Zepto = -21,
        Yocto = -24,
        Ronto = -27,
        Quecto = -30
    }

    public static readonly Dictionary<string, SIPrefix> ValidPrefixStrings = new()
    {
        { "Q", new SIPrefix(Prefix.Quetta) },
        { "R", new SIPrefix(Prefix.Ronna) },
        { "Y", new SIPrefix(Prefix.Yotta) },
        { "Z", new SIPrefix(Prefix.Zetta) },
        { "E", new SIPrefix(Prefix.Exa) },
        { "P", new SIPrefix(Prefix.Peta) },
        { "T", new SIPrefix(Prefix.Tera) },
        { "G", new SIPrefix(Prefix.Giga) },
        { "M", new SIPrefix(Prefix.Mega) },
        { "k", new SIPrefix(Prefix.Kilo) },
        { "h", new SIPrefix(Prefix.Hecto) },
        { "da", new SIPrefix(Prefix.Deca) },
        { "d", new SIPrefix(Prefix.Deci) },
        { "c", new SIPrefix(Prefix.Centi) },
        { "m", new SIPrefix(Prefix.Milli) },
        { "µ", new SIPrefix(Prefix.Micro) },
        { "n", new SIPrefix(Prefix.Nano) },
        { "p", new SIPrefix(Prefix.Pico) },
        { "f", new SIPrefix(Prefix.Femto) },
        { "a", new SIPrefix(Prefix.Atto) },
        { "z", new SIPrefix(Prefix.Zepto) },
        { "y", new SIPrefix(Prefix.Yocto) },
        { "r", new SIPrefix(Prefix.Ronto) },
        { "q", new SIPrefix(Prefix.Quecto) },
    };
}

public static class PrefixExtensions
{
    public static double Factor(this SIPrefix.Prefix prefix) => (int)prefix;
}
