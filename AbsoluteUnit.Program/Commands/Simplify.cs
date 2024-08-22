using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;
using System.Security.Cryptography;

namespace AbsoluteUnit.Program.Commands;

public class Simplify(CommandGroup commandGroup, IMeasurementParser measurementParser) : ICommand
{
    private CommandGroup CommandGroup { get; init; } = commandGroup;

    private Measurement Input { get; init; } = GetInput(commandGroup, measurementParser);

    public Measurement Run()
    {
        var baseRepresentation = Input.ExpressInBaseUnits();
        var baseCounts = new BaseUnitCount(baseRepresentation);
        var simplifiedUnits = baseCounts.GetUnits();

        return new Measurement(simplifiedUnits, baseRepresentation.Quantity);
    }

    private static Measurement GetInput(CommandGroup commandGroup, IMeasurementParser measurementParser) => 
        measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);
}

struct BaseUnitCount
{
    public int Meter { get; }
    public int Kilogram { get; }
    public int Second { get; }
    public int Ampere { get; }
    public int Kelvin { get; }
    public int Mole { get; }
    public int Candela { get; }

    public int Complexity { get; set; } = 0;

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
            if (unit.UnitType.Unit is SIBase.Units baseUnit)
            {
                switch (baseUnit)
                {
                    case SIBase.Units.Meter: Meter++; break;
                    case SIBase.Units.Gram: Kilogram++; break;
                    case SIBase.Units.Second: Second++; break;
                    case SIBase.Units.Ampere: Ampere++; break;
                    case SIBase.Units.Kelvin: Kelvin++; break;
                    case SIBase.Units.Mole: Mole++; break;
                    case SIBase.Units.Candela: Candela++; break;
                }
            }
        }
    }

    public BaseUnitCount Subtract

    public List<Unit> GetUnits()
    {
        return [];
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
        { SIDerived.Units.Radian,    new() },
        { SIDerived.Units.Steradian, new() },
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
}
