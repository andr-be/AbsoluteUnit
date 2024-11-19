using AbsoluteUnit.Program.Structures;
using static AbsoluteUnit.Program.UnitTypes.SIBase;

namespace AbsoluteUnit.Program.UnitTypes;

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
        Siemen,
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

    public string Symbol => UnitType switch
    {
        // Intentionally left messy to remind you to clean up...
        Units.Hertz => "Hz",
        Units.Radian => "r",
        Units.Steradian => "sr",
        Units.Newton => "N",
        Units.Pascal => "Pa",
        Units.Joule => "J",
        Units.Watt => "W",
        Units.Coulomb => "C",
        Units.Volt => "V",
        Units.Farad => "F",
        Units.Ohm => "Ω",
        Units.Siemen => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
        Units.Weber => "Wb",
        Units.Tesla => "T",
        Units.Henry => "H",
        Units.Celsius => "°C",
        Units.Lumen => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
        Units.Lux => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
        Units.Becquerel => "Bq",
        Units.Gray => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
        Units.Sievert => "sv",
        Units.Katal => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
        _ => throw new NotImplementedException($"TODO: Implement " + nameof(UnitType)),
    };

    public object UnitType { get; init; } = unit;

    public double ToBase(double value) =>
        (Units)UnitType == Units.Celsius
            ? value + 273.15
            : value;

    public double FromBase(double value) =>
        (Units)UnitType == Units.Celsius
            ? value - 273.15
            : value;

    // TODO: Finish all of the derived unit strings...
    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        // Joules
        { "J", new SIDerived(Units.Joule) },
        { "Joule", new SIDerived(Units.Joule) },
        { "Joules", new SIDerived(Units.Joule) },
        { "joule", new SIDerived(Units.Joule) },
        { "joules", new SIDerived(Units.Joule) },

        // Newton
        { "N", new SIDerived(Units.Newton) },
        { "Newton", new SIDerived(Units.Newton) },
        { "Newtons", new SIDerived(Units.Newton) },
        { "newton", new SIDerived(Units.Newton) },
        { "newtons", new SIDerived(Units.Newton) },

        // Pascal
        { "Pa", new SIDerived(Units.Pascal) },
        { "Pascal", new SIDerived(Units.Pascal) },
        { "Pascals", new SIDerived(Units.Pascal) },
        { "pascal", new SIDerived(Units.Pascal) },
        { "pascals", new SIDerived(Units.Pascal) },
        
        // Hertz
        { "Hz", new SIDerived(Units.Hertz) },
        { "Hertz", new SIDerived(Units.Hertz) },
        { "hz", new SIDerived(Units.Hertz) },
        { "hertz", new SIDerived(Units.Hertz) },
        
        // Celsius
        { "°C", new SIDerived(Units.Celsius) },
        { "°c", new SIDerived(Units.Celsius) },
        { "Celsius", new SIDerived(Units.Celsius) },
        { "celsius", new SIDerived(Units.Celsius) },
        { "degrees C", new SIDerived(Units.Celsius) },
        { "Degrees C", new SIDerived(Units.Celsius) },

        // Watt
        { "W", new SIDerived(Units.Watt) },
        { "Watt", new SIDerived(Units.Watt) },
        { "Watts", new SIDerived(Units.Watt) },
        { "watt", new SIDerived(Units.Watt) },
        { "watts", new SIDerived(Units.Watt) },

        // Volt
        { "V", new SIDerived(Units.Volt) },
        { "v", new SIDerived(Units.Volt) },
        { "Volt", new SIDerived(Units.Volt) },
        { "Volts", new SIDerived(Units.Volt) },
        { "volt", new SIDerived(Units.Volt) },
        { "volts", new SIDerived(Units.Volt) },

        // Ohm
        { "Ω", new SIDerived(Units.Ohm) },
        { "Ohm", new SIDerived(Units.Ohm) },
        { "Ohms", new SIDerived(Units.Ohm) },
        { "ohm", new SIDerived(Units.Ohm) },
        { "ohms", new SIDerived(Units.Ohm) },

        // Tesla
        { "T", new SIDerived(Units.Tesla) },
        { "Tesla", new SIDerived(Units.Tesla) },
        { "tesla", new SIDerived(Units.Tesla) },

        // Becquerel
        { "Bq", new SIDerived(Units.Becquerel) },
        { "Becquerel", new SIDerived(Units.Becquerel) },
        { "Becquerels", new SIDerived(Units.Becquerel) },
        { "becquerel", new SIDerived(Units.Becquerel) },
        { "becquerels", new SIDerived(Units.Becquerel) },
        
        // Radian
        { "r", new SIDerived(Units.Radian) },
        { "Radian", new SIDerived(Units.Radian) },
        { "Radians", new SIDerived(Units.Radian) },
        { "radian", new SIDerived(Units.Radian) },
        { "radians", new SIDerived(Units.Radian) },
        
        // Farad
        { "F", new SIDerived(Units.Farad) },
        { "Farad", new SIDerived(Units.Farad) },
        { "Farads", new SIDerived(Units.Farad) },
        { "farad", new SIDerived(Units.Farad) },
        { "farads", new SIDerived(Units.Farad) },

        // Coulomb
        { "C", new SIDerived(Units.Coulomb) },
        { "Coulomb", new SIDerived(Units.Coulomb) },
        
        // Steradian
        { "Steradian", new SIDerived(Units.Steradian) },

        // Siemen
        { "Siemen", new SIDerived(Units.Siemen) },
        { "Siemens", new SIDerived(Units.Siemen) },
        
        // Sievert
        { "sv", new SIDerived(Units.Sievert) },
        { "Sievert", new SIDerived(Units.Sievert) },
        { "Sieverts", new SIDerived(Units.Sievert) },
        
        // Weber
        { "Weber", new SIDerived(Units.Weber) },

        // Henry
        { "Henry", new SIDerived(Units.Henry) },
        
        // Lumen
        { "Lumen", new SIDerived(Units.Lumen) },

        // Lux
        { "Lux", new SIDerived(Units.Lux) },

        // Gray
        { "Gray", new SIDerived(Units.Gray) },
        
        // Katal
        { "Katal", new SIDerived(Units.Katal) },
        
    };

    public override bool Equals(object? obj) =>
        obj is SIDerived other &&
        UnitType.Equals(other.UnitType);

    public override int GetHashCode() => HashCode.Combine(Symbol, UnitType);

    public List<Unit> ExpressInBaseUnits(Unit unit)
    {
        var e = unit.Exponent;
        
        return (Units)UnitType switch
        {
            Units.Hertz =>     [Second(-1 * e)],
            Units.Becquerel => [Second(-1 * e)],

            Units.Newton => 
            [
                Kilogram(e), 
                Meter(e),      
                Second(-2 * e)
            ],

            Units.Pascal => 
            [
                Kilogram(e), 
                Meter(-1 * e), 
                Second(-2 * e)
            ],

            Units.Joule =>  
            [
                Kilogram(e), 
                Meter(2 * e),  
                Second(-2 * e)
            ],

            Units.Watt =>   
            [
                Kilogram(e), 
                Meter(2 * e),  
                Second(-3 * e)
            ],

            Units.Coulomb => 
            [
                Second(e),        
                Ampere(e)
            ],

            Units.Volt =>    
            [
                Kilogram(e),      
                Meter(-2 * e), 
                Second(-3 * e), 
                Ampere(-1 * e)
            ],

            Units.Farad => 
            [
                Kilogram(-1 * e), 
                Meter(-2 * e), 
                Second(4 * e),  
                Ampere(2 * e)
            ],

            Units.Ohm =>
            [
                Kilogram(e),
                Meter(2 * e),
                Second(-3 * e),
                Ampere(-2 * e)
            ],

            Units.Siemen => 
            [
                Kilogram(-1 * e), 
                Meter(-2 * e), 
                Second(3 * e), 
                Ampere(2 * e)
            ],

            Units.Weber => 
            [
                Kilogram(e), 
                Meter(2 * e), 
                Second(-2 * e), 
                Ampere(-1 * e)
            ],

            Units.Tesla => 
            [
                Kilogram(e), 
                Second(-2 * e), 
                Ampere(-1 * e)
            ],

            Units.Henry => 
            [
                Kilogram(e), 
                Meter(2 * e), 
                Second(-2 * e),
                Ampere(-2 * e)
            ],

            Units.Celsius => [Kelvin(e)],

            Units.Lumen or
            Units.Lux => [Candela(e), Meter(-2)],

            Units.Gray or
            Units.Sievert => [Meter(2), Second(-2)],

            Units.Katal => [Second(-1), Mole(e)],

            Units.Radian or
            Units.Steradian => [],    // Unitless constant...?!

            _ => throw new NotImplementedException($"{UnitType} not currently supported!")
        };
    }
}
