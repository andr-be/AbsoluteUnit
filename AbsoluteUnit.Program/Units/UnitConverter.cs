
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Units;
using static UnitFactory;

public static class UnitConverter
{
    /// <summary>
    /// Converts all SIBase units to SIBase units
    /// </summary>
    /// <param name="baseUnit">the SIBase unit</param>
    /// <returns>exactly what you put in</returns>
    public static List<Unit> ConvertSIBase(SIBase baseUnit) => [ new(baseUnit) ];

    /// <summary>
    /// Converts all USCustomary units to their SIBase Representation
    /// </summary>
    /// <param name="customaryUnit">the USCustomary unit you wish to convert to base</param>
    /// <returns>SI Base Representation</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<Unit> ConvertUSCustomary(USCustomary customaryUnit) => customaryUnit.Unit switch
    {
        // Length
        USCustomary.Units.Mil or
        USCustomary.Units.Inch or
        USCustomary.Units.Feet or
        USCustomary.Units.Yards or
        USCustomary.Units.Miles => [ Meter() ],

        // Mass
        USCustomary.Units.Ounce or
        USCustomary.Units.Pound or
        USCustomary.Units.Ton => [ Kilogram() ],

        // Volume
        USCustomary.Units.FluidOunce or
        USCustomary.Units.Pint or
        USCustomary.Units.Gallon => [ Meter(3) ],

        // Temperature
        USCustomary.Units.Fahrenheit => [ Kelvin() ],

        _ => throw new NotImplementedException($"No base conversion case implemented for {customaryUnit}")
    };

    /// <summary>
    /// Converts all SIDerived Units to their SIBase Representation
    /// </summary>
    /// <param name="derivedUnit">the SIDerived unit you wish to convert to base</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<Unit> ConvertSIDerived(SIDerived derivedUnit) => derivedUnit.Unit switch
    {
        SIDerived.Units.Hertz => [ Second(-1) ],
        SIDerived.Units.Becquerel => [ Second(-1) ],

        SIDerived.Units.Newton => [ Kilogram(), Meter(), Second(-2) ],
        SIDerived.Units.Pascal => [ Kilogram(), Meter(-1), Second(-2) ],
        SIDerived.Units.Joule => [ Kilogram(), Meter(2), Second(-2) ],
        SIDerived.Units.Watt => [ Kilogram(), Meter(2), Second(-3) ],

        SIDerived.Units.Coulomb => [ Second(), Ampere() ],
        SIDerived.Units.Volt => [ Kilogram(), Meter(-2), Second(-3), Ampere(-1) ],
        SIDerived.Units.Farad => [ Kilogram(-1), Meter(-2), Second(4), Ampere(2) ],
        SIDerived.Units.Ohm => [ Kilogram(), Meter(2), Second(-3), Ampere(-2) ],

        SIDerived.Units.Siemens => [ Kilogram(-1), Meter(-2), Second(3), Ampere(2) ],
        SIDerived.Units.Weber => [ Kilogram(), Meter(2), Second(-2), Ampere(-1) ],
        SIDerived.Units.Tesla => [ Kilogram(), Second(-2), Ampere(-1) ],
        SIDerived.Units.Henry => [ Kilogram(), Meter(2), Second(-2), Ampere(-2) ],

        SIDerived.Units.Celsius => [ Kelvin() ],

        SIDerived.Units.Lumen or
        SIDerived.Units.Lux => [ Candela(), Meter(-2) ],

        SIDerived.Units.Gray or
        SIDerived.Units.Sievert => [ Meter(2), Second(-2) ],

        SIDerived.Units.Katal => [ Second(-1), Mole() ],

        SIDerived.Units.Radian or
        SIDerived.Units.Steradian => [],    // Unitless constant...?!

        _ => throw new NotImplementedException($"{derivedUnit} not currently supported!")
    };

    /// <summary>
    /// Converts Miscellaneous units to their SIBase Representation
    /// </summary>
    /// <param name="miscUnit">the Miscellaneous unit you wish to convert to base</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<Unit> ConvertMiscellaneous(Miscellaneous miscUnit)
    {
        throw new NotImplementedException($"{miscUnit} has no base conversions implemented!");
    }
}