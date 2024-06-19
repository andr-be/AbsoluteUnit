namespace AbsoluteUnit.Program;

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
        if (ValidConversion(FromUnit, ToUnit)) ConversionFactor = GetConversionFactor();
        else throw new ArgumentException($"Cannot convert from {FromUnit} to {ToUnit}");
    }

    private static bool ValidConversion(AbsMeasurement fromUnit, AbsMeasurement toUnit)
    {
        // really need to work out how I might assign 
        return true;
    }

    private double GetConversionFactor()
    {
        var toBase = FromUnit.Units
            .Select(u => u.ConversionToBase())
            .Aggregate((x, y) => x * y);

        var fromBase = ToUnit.Units
            .Select(u => u.ConversionFromBase())
            .Aggregate((x, y) => x * y);

        return toBase * fromBase;
    }

    public AbsMeasurement Execute() =>
        new(ToUnit.Units, FromUnit.Quantity * ConversionFactor, FromUnit.Exponent);

    public override string ToString() =>
        $"{CommandGroup.CommandType}:\t{FromUnit} -> {string.Join(".", ToUnit.Units)} == {Execute()}";
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
    public static AbsMeasurement ToBaseMeasurement(AbsMeasurement from) => new
    (
        from.Units.SelectMany(BaseConversion).ToList(),
        from.Quantity * BaseConversionFactor(from.Units),
        from.Exponent
    );

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
    public static bool ValidConversion(AbsMeasurement from, AbsMeasurement to) =>
        ToBaseMeasurement(new AbsMeasurement(from.Units)).Equals(ToBaseMeasurement(new AbsMeasurement(to.Units)));

    /// <summary>
    /// Given a valid AbsUnit, will return the SI Base conversion case.
    /// </summary>
    /// <param name="unit"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    /// <exception cref="ArgumentException"></exception>
    public static List<AbsUnit> BaseConversion(object unit)
    {
        if (unit is AbsUnit validUnit)
        {
            if (validUnit.Unit is SIBase baseUnit)
                return ConvertSIBase(baseUnit);
            
            if (validUnit.Unit is USCustomary customaryUnit)
                return ConvertUSCustomary(customaryUnit);
            
            if (validUnit.Unit is SIDerived derivedUnit)
                throw new NotImplementedException($"No BaseConversion implemented for {derivedUnit} (Derived units not implemented 19/06/24)");
            
            if (validUnit.Unit is Miscellaneous miscUnit)
                throw new NotImplementedException($"No BaseConversion implemented for {miscUnit} (Miscellaneous units not implemented 19/06/24)");

            throw new NotImplementedException($"No BaseConversion implemented for {validUnit}");
        }
        else throw new ArgumentException($"{unit} is not a supported AbsUnit!");
    }

    private static List<AbsUnit> ConvertUSCustomary(USCustomary customaryUnit)
    {
        return customaryUnit.Unit switch
        {
            // Length
            USCustomary.Units.Mil => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],
            USCustomary.Units.Inch => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],
            USCustomary.Units.Feet => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],
            USCustomary.Units.Yards => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],
            USCustomary.Units.Miles => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],

            // Mass
            USCustomary.Units.Ounce => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("g"))
                    .Build()
            ],
            USCustomary.Units.Pound => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("g"))
                    .Build()
            ],
            USCustomary.Units.Ton => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("g"))
                    .Build()
            ],

            // Volume
            USCustomary.Units.FluidOunce => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .WithExponent(3)
                    .Build()
            ],
            USCustomary.Units.Pint => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .WithExponent(3)
                    .Build()
            ],
            USCustomary.Units.Gallon => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .WithExponent(3)
                    .Build()
            ],

            // Temperature
            USCustomary.Units.Fahrenheit => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("k"))
                    .Build()
            ],
        };
    }

    private static List<AbsUnit> ConvertSIBase(SIBase baseUnit)
    {
        return baseUnit.Unit switch
        {
            // Base SI Units convert to themselves!
            SIBase.Units.Meter => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("m"))
                    .Build()
            ],
            SIBase.Units.Gram => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("g"))
                    .Build()
            ],
            SIBase.Units.Second => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("s"))
                    .Build()
            ],
            SIBase.Units.Ampere => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("A"))
                    .Build()
            ],
            SIBase.Units.Kelvin => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("K"))
                    .Build()
            ],
            SIBase.Units.Mole => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("mol"))
                    .Build()
            ],
            SIBase.Units.Candela => [
                new AbsUnitBuilder()
                    .WithUnit(new SIBase("cd"))
                    .Build()
            ],
        };
    }
}