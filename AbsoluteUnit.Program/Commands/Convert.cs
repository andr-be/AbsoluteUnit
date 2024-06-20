using AbsoluteUnit.Program.Factories;

namespace AbsoluteUnit.Program.Commands;

using AbsoluteUnit.Program.Interfaces;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;
using static AbsUnitBuilder;

public class Convert : ICommand
{
    public AbsMeasurement FromUnit { get; }
    public AbsMeasurement ToUnit { get; }
    public double ConversionFactor { get; }

    private CommandGroup CommandGroup { get; }

    public Convert(CommandGroup commandGroup, IMeasurementParser measurementParser)
    {
        CommandGroup = commandGroup;

        FromUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);

        ToUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[1], unitOnly: true);

        if (MeasurementConverter.IsValidConversion(FromUnit, ToUnit))
            ConversionFactor = MeasurementConverter.QuantityConversionFactor(FromUnit, ToUnit);

        else throw new ArgumentException($"Cannot convert from {FromUnit} to {ToUnit}");
    }

    public AbsMeasurement Execute() =>
        MeasurementConverter.ConvertMeasurement(FromUnit, ToUnit);
    //new(ToUnit.Units, FromUnit.Quantity * ConversionFactor, FromUnit.Exponent);

    public override string ToString() =>
        $"{CommandGroup.CommandType}:\tFrom: {FromUnit}\tTo: {string.Join(".", ToUnit.Units)}";
}

/// <summary>
/// Statically handles the logic required to convert between different AbsMeasurements
/// </summary>
public static class MeasurementConverter
{
    /// <summary>
    /// Given a valid from and to unit, will convert from one to another
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>a new AbsMeasurement in to's units, with a converted quantity</returns>
    public static AbsMeasurement ConvertMeasurement(AbsMeasurement from, AbsMeasurement to) =>
        new(to.Units, from.Quantity * QuantityConversionFactor(from, to), from.Exponent);

    /// <summary>
    /// Takes a single AbsMeasurement and converts it to the AbsMeasurement of the Base SI Conversion
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public static AbsMeasurement ExpressInBaseUnits(AbsMeasurement from) => new
    (
        from.Units.SelectMany(BaseConversion).ToList(),
        from.Quantity * BaseConversionFactor(from.Units),
        from.Exponent
    );

    /// <summary>
    /// Given a valid AbsUnit, will return the SI Base conversion case.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static List<AbsUnit> BaseConversion(AbsUnit unit)
    {
        if (unit.Unit is SIBase baseUnit)
            return UnitConverter.ConvertSIBase(baseUnit);

        if (unit.Unit is USCustomary customaryUnit)
            return UnitConverter.ConvertUSCustomary(customaryUnit);

        if (unit.Unit is SIDerived derivedUnit)
            return UnitConverter.ConvertSIDerived(derivedUnit);

        if (unit.Unit is Miscellaneous miscUnit)
            return UnitConverter.ConvertMiscellaneous(miscUnit);

        else throw new ArgumentException($"{unit} is not a supported AbsUnit!");
    }

    /// <summary>
    /// Given a valid from and to unit, will give you the amount you have to multiply by to generate the converted quantity
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns>factor to multiply from's quantity by to represent it in to's units</returns>
    public static double QuantityConversionFactor(AbsMeasurement from, AbsMeasurement to) =>
        BaseConversionFactor(to.Units) * BaseConversionFactor(from.Units);

    /// <summary>
    /// Returns the amount needed to multiply a unit by to convert to its base case.
    /// </summary>
    /// <param name="units"></param>
    /// <returns></returns>
    private static double BaseConversionFactor(List<AbsUnit> units) =>
        units.Select(u => u.ConversionToBase()).Aggregate((x, y) => x * y);

    /// <summary>
    /// Checks that the base conversions of two AbsMeasurements match, returning false if they do not.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static bool IsValidConversion(AbsMeasurement from, AbsMeasurement to)
    {
        var fromUnitsAsBase = ExpressInBaseUnits(new AbsMeasurement(from.Units));
        var toUnitsAsBase = ExpressInBaseUnits(new AbsMeasurement(to.Units));

        return fromUnitsAsBase.Equals(toUnitsAsBase);
    }
}

public static class UnitConverter
{
    /// <summary>
    /// Converts all SIBase units to SIBase units
    /// </summary>
    /// <param name="baseUnit">the SIBase unit</param>
    /// <returns>exactly what you put in</returns>
    public static List<AbsUnit> ConvertSIBase(SIBase baseUnit) =>
    [
        new AbsUnitBuilder()
            .WithUnit(baseUnit)
            .Build()
    ];

    /// <summary>
    /// Converts all USCustomary units to their SIBase Representation
    /// </summary>
    /// <param name="customaryUnit">the USCustomary unit you wish to convert to base</param>
    /// <returns>SI Base Representation</returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<AbsUnit> ConvertUSCustomary(USCustomary customaryUnit) => customaryUnit.Unit switch
    {
        // Length
        USCustomary.Units.Mil or
        USCustomary.Units.Inch or
        USCustomary.Units.Feet or
        USCustomary.Units.Yards or
        USCustomary.Units.Miles => [Meter()],

        // Mass
        USCustomary.Units.Ounce or
        USCustomary.Units.Pound or
        USCustomary.Units.Ton => [Kilogram()],

        // Volume
        USCustomary.Units.FluidOunce or
        USCustomary.Units.Pint or
        USCustomary.Units.Gallon => [Meter(3)],

        // Temperature
        USCustomary.Units.Fahrenheit => [Kelvin()],

        _ => throw new NotImplementedException($"No base conversion case implemented for {customaryUnit}")
    };

    /// <summary>
    /// Converts all SIDerived Units to their SIBase Representation
    /// </summary>
    /// <param name="derivedUnit">the SIDerived unit you wish to convert to base</param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static List<AbsUnit> ConvertSIDerived(SIDerived derivedUnit) => derivedUnit.Unit switch
    {
        SIDerived.Units.Hertz => [Second(-1)],

        SIDerived.Units.Newton => [Kilogram(), Meter(), Second(-2)],

        SIDerived.Units.Pascal => [Kilogram(), Meter(-1), Second(-2)],

        SIDerived.Units.Joule => [Kilogram(), Meter(2), Second(-2)],

        SIDerived.Units.Watt => [Kilogram(), Meter(2), Second(-3)],

        SIDerived.Units.Coulomb => [Second(), Ampere()],

        SIDerived.Units.Volt => [Kilogram(), Meter(-2), Second(-3), Ampere(-1)],

        SIDerived.Units.Farad => [Kilogram(-1), Meter(-2), Second(4), Ampere(2),],

        SIDerived.Units.Ohm => [Kilogram(), Meter(2), Second(-3), Ampere(-2)],

        SIDerived.Units.Siemens => [Kilogram(-1), Meter(-2), Second(3), Ampere(2)],

        SIDerived.Units.Weber => [Kilogram(), Meter(2), Second(-2), Ampere(-1)],

        SIDerived.Units.Tesla => [Kilogram(), Second(-2), Ampere(-1)],

        SIDerived.Units.Henry => [Kilogram(), Meter(2), Second(-2), Ampere(-2)],

        SIDerived.Units.Celsius => [Kelvin()],

        SIDerived.Units.Lumen or
        SIDerived.Units.Lux => [Candela(), Meter(-2)],

        SIDerived.Units.Becquerel => [Second(-1)],

        SIDerived.Units.Gray or
        SIDerived.Units.Sievert => [Meter(2), Second(-2)],

        SIDerived.Units.Katal => [Second(-1), Mole()],

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
    public static List<AbsUnit> ConvertMiscellaneous(Miscellaneous miscUnit)
    {
        throw new NotImplementedException($"{miscUnit} has no base conversions implemented!");
    }
}