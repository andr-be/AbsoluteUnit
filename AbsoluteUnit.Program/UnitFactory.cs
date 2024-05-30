namespace AbsoluteUnit.Program;

public interface IUnit
{
    string Symbol { get; }
    double ToBase(double value);
    double FromBase(double value);
}

public class AbsUnit(IUnit unit, int exponent)
{
    public IUnit Unit { get; set; } = unit;
    public int Exponent { get; set; } = exponent;
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

    private List<UnitGroup> SimplePropagation(List<UnitGroup> input)
    {
        bool toDenominator = false;

        return input
            .Select(current =>
            {
                if (current.Operation == UnitGroup.UnitOperation.Divide)
                    toDenominator = true;

                return toDenominator 
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
            stringDict.TryGetValue(group.UnitSymbol, out var unit);
            if (unit is not null)
                return new AbsUnit((IUnit)unit, group.Exponent);
        }
        throw new KeyNotFoundException($"{group.UnitSymbol} not found in key database...");
    }
}

public class SIBase(SIBase.Unit unit) : IUnit
{
    private Unit _unit { get; set; } = unit;

    public enum Unit
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
        { "m", new SIBase(Unit.Meter) },
        { "g", new SIBase(Unit.Gram) },
        { "s", new SIBase(Unit.Second) },
        { "A", new SIBase(Unit.Ampere) },
        { "K", new SIBase(Unit.Kelvin) },
        { "mole", new SIBase(Unit.Mole) },
        { "cd", new SIBase(Unit.Candela) },
    };

    public string Symbol => unit switch
    {
        Unit.Meter => "m",
        Unit.Gram => "g",
        Unit.Second => "s",
        Unit.Ampere => "A",
        Unit.Kelvin => "K",
        Unit.Mole => "mole",
        Unit.Candela => "cd",
        _ => throw new NotImplementedException(),
    };

    public double ToBase(double value) => value;

    public double FromBase(double value) => value;
}


public class SIDerived : IUnit
{
    public string Symbol => throw new NotImplementedException();

    public double FromBase(double value)
    {
        throw new NotImplementedException();
    }

    public double ToBase(double value)
    {
        throw new NotImplementedException();
    }

    public enum Unit
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

public class USCustomary : IUnit
{
    public string Symbol => throw new NotImplementedException();

    public double FromBase(double value)
    {
        throw new NotImplementedException();
    }

    public double ToBase(double value)
    {
        throw new NotImplementedException();
    }

    public enum Unit
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

    public static readonly Dictionary<string, object> ValidUnitStrings = [];
}

public class Miscellaneous : IUnit
{
    public string Symbol => throw new NotImplementedException();

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

public class Prefix
{
    public enum SI
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

    private static readonly Dictionary<string, SI> PrefixStrings = new()
    {
        { "Q", SI.Quetta },
        { "R", SI.Ronna },
        { "Y", SI.Yotta },
        { "Z", SI.Zetta },
        { "E", SI.Exa },
        { "P", SI.Peta },
        { "T", SI.Tera },
        { "G", SI.Giga },
        { "M", SI.Mega },
        { "k", SI.Kilo },
        { "h", SI.Hecto },
        { "da", SI.Deca },
        { "d", SI.Deci },
        { "c", SI.Centi },
        { "m", SI.Milli },
        { "µ", SI.Micro },
        { "n", SI.Nano },
        { "p", SI.Pico },
        { "f", SI.Femto },
        { "a", SI.Atto },
        { "z", SI.Zepto },
        { "y", SI.Yocto },
        { "r", SI.Ronto },
        { "q", SI.Quecto },
    };
}

public static class PrefixExtensions
{
    public static double Factor(this Prefix.SI prefix) => (int)prefix;
}
