
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

        if (FromUnit.IsValidConversion(ToUnit))
            ConversionFactor = FromUnit.QuantityConversionFactor(ToUnit);

        else throw new ArgumentException($"Cannot convert from {FromUnit} to {ToUnit}");
    }

    public Measurement Run() => FromUnit.ConvertTo(ToUnit);

    public override string ToString() =>
        $"{CommandGroup.CommandType}:\tFrom: {FromUnit}\tTo: {string.Join(".", ToUnit.Units)}";
}
