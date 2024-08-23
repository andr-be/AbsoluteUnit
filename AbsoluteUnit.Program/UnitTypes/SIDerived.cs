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

    public string Symbol => UnitType switch
    {
        Units.Joule => "J",
        Units.Newton => "N",
        _ => throw new NotImplementedException("TODO: implement the rest of the SI Derived unit symbols"),
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

    // TODO: Create all of the derived unit strings...
    public static readonly Dictionary<string, object> ValidUnitStrings = new()
    {
        { "J", new SIDerived(Units.Joule) },
        { "joule", new SIDerived(Units.Joule) },
        { "joules", new SIDerived(Units.Joule) },
        // FUCK LOADS MORE COMING SOON!
        { "N", new SIDerived(Units.Newton) },
        { "Newton", new SIDerived(Units.Newton) },
        { "Newtons", new SIDerived(Units.Newton) },
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

            Units.Siemens => 
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
