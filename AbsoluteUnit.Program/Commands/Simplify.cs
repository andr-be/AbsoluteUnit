using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.UnitTypes;

namespace AbsoluteUnit.Program.Commands;

public class Simplify(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
{
    private Measurement Input { get; init; } = GetInput(commandGroup, measurementParser);

    public List<Measurement> Run()
    {
        var baseMeasurement = Input.ExpressInBaseUnits();
        var baseQuantity = baseMeasurement.Quantity * Math.Pow(10, Input.Exponent);

        var baseCounts = new BaseUnitCount(baseMeasurement);
        var simplifiedUnits = baseCounts.GetUnits();
        
        var prefixSimplified = PrefixSimplification(simplifiedUnits[0], baseQuantity);

        return [prefixSimplified];
    }

    private static Measurement PrefixSimplification(List<Unit> units, double quantity)
    {
        if (quantity is < 1e+3 and > 1e-3)
            return new Measurement(units, quantity);

        int exponentValue = 0;

        if (quantity > 1)
            while (quantity > 1)
            {
                quantity /= 1e3;
                exponentValue += 3;
            }
        
        else
            while (quantity < 1)
            {
                quantity *= 1e3;
                exponentValue -= 3;
            }
        
        units = SimplifyFirstUnit(units, exponentValue);

        return new Measurement(units, quantity);
    }

    private static List<Unit> SimplifyFirstUnit(List<Unit> units, int exponentValue)
    {
        units[0] = Unit.OfType
        (
            unitType: units[0].UnitType.UnitType, 
            prefix:   (SIPrefix.Prefixes)exponentValue, 
            exponent: units[0].Exponent
        );

        return units;
    }

    private static Measurement GetInput(CommandGroup commandGroup, IMeasurementParser measurementParser) => 
        measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);

    public override string ToString() => $"{commandGroup.CommandType}:\t{Input} represented in simplest form";
}

readonly struct BaseUnitCount : IEnumerable
{
    public int Meter { get; }
    public int Kilogram { get; }
    public int Second { get; }
    public int Ampere { get; }
    public int Kelvin { get; }
    public int Mole { get; }
    public int Candela { get; }

    public readonly int Complexity =>
        Math.Abs(Meter) +
        Math.Abs(Kilogram) +
        Math.Abs(Second) +
        Math.Abs(Ampere) +
        Math.Abs(Kelvin) +
        Math.Abs(Mole) +
        Math.Abs(Candela);

    /// <summary>
    /// Generate a BaseUnitCount from integer values
    /// </summary>
    /// <param name="meter">meters exponent</param>
    /// <param name="kilogram">kilograms exponent</param>
    /// <param name="second">second exponent</param>
    /// <param name="ampere">amps exponent</param>
    /// <param name="kelvin">kelvin exponent</param>
    /// <param name="mole">mole exponent</param>
    /// <param name="candela">candela exponent</param>
    public BaseUnitCount(
        int meter = 0,
        int kilogram = 0,
        int second = 0,
        int ampere = 0,
        int kelvin = 0,
        int mole = 0,
        int candela = 0)
    {
        Meter = meter;
        Kilogram = kilogram;
        Second = second;
        Ampere = ampere;
        Kelvin = kelvin;
        Mole = mole;
        Candela = candela;
    }

    /// <summary>
    /// Generate a BaseUnitCount from a Measurement
    /// </summary>
    /// <param name="baseRepresentation"></param>
    public BaseUnitCount(Measurement baseRepresentation)
    {
        foreach (var unit in baseRepresentation.Units)
        {
            if (unit.UnitType.UnitType is SIBase.Units baseUnit)
            {
                switch (baseUnit)
                {
                    case SIBase.Units.Meter:   Meter += unit.Exponent; 
                        break;
                    case SIBase.Units.Gram:    Kilogram += unit.Exponent; 
                        break;
                    case SIBase.Units.Second:  Second += unit.Exponent; 
                        break;
                    case SIBase.Units.Ampere:  Ampere += unit.Exponent; 
                        break;
                    case SIBase.Units.Kelvin:  Kelvin += unit.Exponent; 
                        break;
                    case SIBase.Units.Mole:    Mole += unit.Exponent; 
                        break;
                    case SIBase.Units.Candela: Candela += unit.Exponent; 
                        break;
                }
            }
        }
    }

    /// <summary>
    /// Generate a BaseUnitCount from an array of Integers
    /// </summary>
    /// <param name="counts"></param>
    public BaseUnitCount(int[] counts)
    {
        if (counts.Length != 7) throw new ArgumentException("Counts can only be initialised via array of size 7!");

        this = new BaseUnitCount(counts[0], counts[1], counts[2], counts[3], counts[4], counts[5], counts[6]);
    }

    public IEnumerator<int> GetEnumerator()
    {
        yield return Meter;
        yield return Kilogram;
        yield return Second;
        yield return Ampere;
        yield return Kelvin;
        yield return Mole;
        yield return Candela;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public int[] ToArray() => [Meter, Kilogram, Second, Ampere, Kelvin, Mole, Candela];

    public bool CanSubtract(BaseUnitCount other) =>
        this.Meter >= other.Meter
     && this.Kilogram >= other.Kilogram
     && this.Second >= other.Second
     && this.Ampere >= other.Ampere
     && this.Kelvin >= other.Kelvin
     && this.Mole >= other.Mole
     && this.Candela >= other.Candela
     && SubtractCount(other).Complexity <= this.Complexity;

    public readonly BaseUnitCount SubtractCount(BaseUnitCount other) => new
    (
        this.Meter - other.Meter, 
        this.Kilogram - other.Kilogram, 
        this.Second - other.Second, 
        this.Ampere - other.Ampere,
        this.Kelvin - other.Kelvin,
        this.Mole - other.Mole,
        this.Candela - other.Candela
    );

    public readonly BaseUnitCount AddCount(BaseUnitCount other) => new
    (
        this.Meter + other.Meter,
        this.Kilogram + other.Kilogram,
        this.Second + other.Second,
        this.Ampere + other.Ampere,
        this.Kelvin + other.Kelvin,
        this.Mole + other.Mole,
        this.Candela + other.Candela
    );

    public List<List<Unit>> GetUnits() => RecursivelyGetUnits(this, [], 0);

    private static List<List<Unit>> RecursivelyGetUnits(BaseUnitCount unitCount, List<Unit> currentPath, int depth)
    {
        const int MaxDepth = 10; // Don't want a stack overflow now...
        List<List<Unit>> solutions = [];

        // Base case: if the unit is fully simplified (all zeros) or max depth reached
        if (unitCount.Complexity == 0 || depth >= MaxDepth)
        {
            currentPath.AddRange(ConvertBaseUnitCountToUnits(unitCount));
            solutions.Add(new List<Unit>(currentPath));
            return solutions;
        }

        // Otherwise, run through every complex unit and recurse if it's possible to remove a unit from the chunk
        bool simplified = false;
        foreach (var complexUnit in SimplifyUtilities.GetComplexityOrder())
        {
            var candidateUnitCount = SimplifyUtilities.DerivedBaseCounts[complexUnit];
            if (unitCount.CanSubtract(candidateUnitCount))
            {
                simplified = true;
                var remainingUnitCount = unitCount.SubtractCount(candidateUnitCount);
                var newUnit = Unit.OfType(complexUnit);
                var newPath = new List<Unit>(currentPath) { newUnit };
                solutions.AddRange(RecursivelyGetUnits(remainingUnitCount, newPath, depth + 1));
            }
        }

        // If no complex units could be subtracted, add the remaining base units to the path
        if (!simplified)
        {
            currentPath.AddRange(ConvertBaseUnitCountToUnits(unitCount));
            solutions.Add(currentPath);
        }

        return solutions;
    }

    private static List<Unit> ConvertBaseUnitCountToUnits(BaseUnitCount count)
    {
        var units = new List<Unit>();
        if (count.Meter != 0) units.Add(Unit.OfType(SIBase.Units.Meter, exponent: count.Meter));
        if (count.Kilogram != 0) units.Add(Unit.OfType(SIBase.Units.Gram, SIPrefix.Prefixes.Kilo, count.Kilogram));
        if (count.Second != 0) units.Add(Unit.OfType(SIBase.Units.Second, exponent: count.Second));
        if (count.Ampere != 0) units.Add(Unit.OfType(SIBase.Units.Ampere, exponent: count.Ampere));
        if (count.Kelvin != 0) units.Add(Unit.OfType(SIBase.Units.Kelvin, exponent: count.Kelvin));
        if (count.Mole != 0) units.Add(Unit.OfType(SIBase.Units.Mole, exponent: count.Mole));
        if (count.Candela != 0) units.Add(Unit.OfType(SIBase.Units.Candela, exponent: count.Candela));
        return units;
    }
}

internal static class SimplifyUtilities
{
    public static Dictionary<SIDerived.Units, BaseUnitCount> DerivedBaseCounts => new()
    {
        { SIDerived.Units.Hertz,     new(second:-1) },
        { SIDerived.Units.Becquerel, new(second:-1) },

        { SIDerived.Units.Newton,    new(kilogram:1, meter:1, second:-2)},
        { SIDerived.Units.Pascal,    new(kilogram:1, meter:-1, second:-2)},
        { SIDerived.Units.Joule,     new(kilogram:1, meter:2, second:-2) },
        { SIDerived.Units.Watt,      new(kilogram:1, meter:2, second:-3) },

        { SIDerived.Units.Coulomb,   new(second:1, ampere:1) },
        { SIDerived.Units.Volt,      new(kilogram:1, meter:-2, second:-3, ampere:-1) },
        { SIDerived.Units.Farad,     new(kilogram:-1, meter:-2, second:4, ampere:2) },
        { SIDerived.Units.Ohm,       new(kilogram:1, meter:2, second:-3, ampere:-2) },

        { SIDerived.Units.Siemens,   new(kilogram:-1, meter:-2, second:3, ampere:2) },
        { SIDerived.Units.Weber,     new(kilogram:1, meter:2, second:-2, ampere:-1) },
        { SIDerived.Units.Tesla,     new(kilogram:1, second:-2, ampere:-1) },
        { SIDerived.Units.Henry,     new(kilogram:1, meter:2, second:-2, ampere:-2) },

        { SIDerived.Units.Celsius,   new(kelvin:1) },

        { SIDerived.Units.Lumen,     new(meter:-2, candela:1) },
        { SIDerived.Units.Lux,       new(meter:-2, candela:1) },

        { SIDerived.Units.Gray,      new(meter:2, second:-2) },
        { SIDerived.Units.Sievert,   new(meter:2, second:-2) },

        { SIDerived.Units.Katal,     new(mole:1, second:-1) },
    };

    public static Dictionary<SIBase.Units, BaseUnitCount> SIBaseCounts => new()
    {
        { SIBase.Units.Meter,   new(meter:1) },
        { SIBase.Units.Gram,    new(kilogram:1) },
        { SIBase.Units.Second,  new(second:1) },
        { SIBase.Units.Kelvin,  new(kelvin:1) },
        { SIBase.Units.Ampere,  new(ampere:1) },
        { SIBase.Units.Mole,    new(mole:1) },
        { SIBase.Units.Candela, new(candela:1) },
    };

    public static List<SIDerived.Units> GetComplexityOrder()
    {
        var order = DerivedBaseCounts.Keys.ToList();

        order.Sort((a, b) => DerivedBaseCounts[b].Complexity.CompareTo(DerivedBaseCounts[a].Complexity));

        return order;
    }
}
