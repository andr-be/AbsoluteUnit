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

public class MeasurementConverter
{
    public static AbsMeasurement ToBaseMeasurement(AbsMeasurement from)
    {
        var newUnits = GetBaseUnits(from.Units);
        var newQuantity = QuantityConversion(from.Units, from.Quantity);
        
        return new(newUnits, newQuantity, from.Exponent);
    }

    private static double QuantityConversion(List<AbsUnit> units, double quantity)
    {
        var newQuantity = quantity;

        foreach (var unit in units)
            newQuantity *= unit.ConversionToBase();

        return newQuantity;
    }

    private static List<AbsUnit>? GetBaseUnits(List<AbsUnit> units)
    {
        List<AbsUnit> convertedUnits = [];

        foreach (var unit in units)
            convertedUnits.Add(ToBaseUnit(unit));

        return convertedUnits;
    }

    public static AbsUnit ToBaseUnit(AbsUnit from) => 
        new(BaseConversion(from.Unit.Unit), from.Exponent, from.Prefix);

    public static SIBase BaseConversion(object unit) => unit switch
    {
        USCustomary.Units.Mil => new(SIBase.Units.Meter),
        USCustomary.Units.Inch => new(SIBase.Units.Meter),
        USCustomary.Units.Feet => new(SIBase.Units.Meter),
        USCustomary.Units.Yards => new(SIBase.Units.Meter),
        USCustomary.Units.Miles => new(SIBase.Units.Meter),

        USCustomary.Units.Ounce => new(SIBase.Units.Gram),
        USCustomary.Units.Pound => new(SIBase.Units.Gram),
        USCustomary.Units.Ton => new(SIBase.Units.Gram),

        USCustomary.Units.FluidOunce => new(SIBase.Units.Meter),
        USCustomary.Units.Pint => new(SIBase.Units.Meter),
        USCustomary.Units.Gallon => new(SIBase.Units.Meter),

        USCustomary.Units.Fahrenheit => new(SIBase.Units.Kelvin),
    };
}