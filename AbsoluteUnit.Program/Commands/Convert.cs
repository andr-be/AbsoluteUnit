
using AbsoluteUnit.Program.Factories;
using AbsoluteUnit.Program.Structures;
using AbsoluteUnit.Program.Units;

namespace AbsoluteUnit.Program.Commands;
public class Convert : ICommand
{
    public Measurement FromUnit { get; }
    public Measurement ToUnit { get; }
    public double ConversionFactor { get; }

    private CommandGroup CommandGroup { get; }

    public Convert(CommandGroup commandGroup, IMeasurementParser measurementParser)
    {
        CommandGroup = commandGroup;

        FromUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[0]);

        ToUnit = measurementParser.ProcessMeasurement(commandGroup.CommandArguments[1], unitOnly: true);

        if (MeasurementConverter.IsValidConversion(FromUnit, ToUnit))
            ConversionFactor = FromUnit.QuantityConversionFactor(ToUnit);

        else throw new ArgumentException($"Cannot convert from {FromUnit} to {ToUnit}");
    }

    public Measurement Run() => FromUnit.ConvertTo(ToUnit);

    public override string ToString() =>
        $"{CommandGroup.CommandType}:\tFrom: {FromUnit}\tTo: {string.Join(".", ToUnit.Units)}";
}

/// <summary>
/// Statically handles the logic required to convert between different AbsMeasurements
/// </summary>
public static class MeasurementConverter
{
    /// <summary>
    /// Takes a single AbsMeasurement and converts it to the AbsMeasurement of the Base SI Conversion
    /// </summary>
    /// <param name="from"></param>
    /// <returns></returns>
    public static Measurement ExpressInBaseUnits(Measurement from) => new
    (
        from.Units.SelectMany(u => u.BaseConversion()).ToList(),
        from.Quantity * from.Units.AggregateConversionFactors(),
        from.Exponent
    );

    /// <summary>
    /// Given a valid AbsUnit, will return the SI Base conversion case.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static List<Unit> BaseConversion(Unit unit) => unit.UnitType switch
    {
        SIBase baseUnit => UnitConverter.ConvertSIBase(baseUnit),
        USCustomary customaryUnit => UnitConverter.ConvertUSCustomary(customaryUnit),
        SIDerived derivedUnit => UnitConverter.ConvertSIDerived(derivedUnit),
        Miscellaneous miscUnit => UnitConverter.ConvertMiscellaneous(miscUnit),
        _ => throw new ArgumentException($"{unit} is not a supported AbsUnit!")
    };

    /// <summary>
    /// Returns the amount needed to multiply a unit by to convert to its base case.
    /// </summary>
    /// <param name="units"></param>
    /// <returns></returns>
    private static double BaseConversionFactor(List<Unit> units) =>
        units.Select(u => u.ConversionToBase()).Aggregate((x, y) => x * y);

    /// <summary>
    /// Checks that the base conversions of two AbsMeasurements match, returning false if they do not.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public static bool IsValidConversion(Measurement from, Measurement to)
    {
        var fromUnitsAsBase = ExpressInBaseUnits(new Measurement(from.Units));
        var toUnitsAsBase = ExpressInBaseUnits(new Measurement(to.Units));

        return fromUnitsAsBase.Equals(toUnitsAsBase);
    }
}
