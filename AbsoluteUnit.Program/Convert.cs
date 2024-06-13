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

public static class MeasurementConverter
{
    public static AbsMeasurement ToBaseMeasurement(AbsMeasurement from) =>
        new(ToBaseUnits(from.Units), ToBaseQuantity(from.Units, from.Quantity), from.Exponent);
    
    public static AbsUnit ToBaseUnit(AbsUnit from) =>
        new(BaseConversion(from.Unit.Unit), from.Exponent, from.Prefix);

    private static double ToBaseQuantity(List<AbsUnit> units, double quantity) => 
        units.Select(u => u.ConversionToBase()).Aggregate((x, y) => x * y);

    private static List<AbsUnit>? ToBaseUnits(List<AbsUnit> units) => 
        units.Select(ToBaseUnit).ToList();

    public static bool ValidConversion(AbsMeasurement from, AbsMeasurement to) =>
        ToBaseMeasurement(new AbsMeasurement(from.Units))
        .Equals(ToBaseMeasurement(new AbsMeasurement(to.Units)));

    public static SIBase BaseConversion(object unit) => unit switch
    {
        // Base SI Units convert to themselves!
        SIBase.Units.Meter => new(SIBase.Units.Meter),
        SIBase.Units.Gram => new(SIBase.Units.Gram),
        SIBase.Units.Second => new(SIBase.Units.Second),
        SIBase.Units.Ampere => new(SIBase.Units.Ampere),
        SIBase.Units.Kelvin => new(SIBase.Units.Kelvin),
        SIBase.Units.Mole => new(SIBase.Units.Mole),
        SIBase.Units.Candela => new(SIBase.Units.Candela),
        
        // US Customary Length
        USCustomary.Units.Mil => new(SIBase.Units.Meter),
        USCustomary.Units.Inch => new(SIBase.Units.Meter),
        USCustomary.Units.Feet => new(SIBase.Units.Meter),
        USCustomary.Units.Yards => new(SIBase.Units.Meter),
        USCustomary.Units.Miles => new(SIBase.Units.Meter),
        // US Customary Mass
        USCustomary.Units.Ounce => new(SIBase.Units.Gram),
        USCustomary.Units.Pound => new(SIBase.Units.Gram),
        USCustomary.Units.Ton => new(SIBase.Units.Gram),
        // US Customary Volume
        USCustomary.Units.FluidOunce => new(SIBase.Units.Meter),
        USCustomary.Units.Pint => new(SIBase.Units.Meter),
        USCustomary.Units.Gallon => new(SIBase.Units.Meter),
        // US Customary Temperature
        USCustomary.Units.Fahrenheit => new(SIBase.Units.Kelvin),
    };
}