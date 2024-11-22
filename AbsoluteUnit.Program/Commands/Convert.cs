
using AbsoluteUnit.Program.Parsers.ParserGroups;
using AbsoluteUnit.Program.Structures;

namespace AbsoluteUnit.Program.Commands;
public class Convert : ICommand
{
    public Measurement Input { get; init; }
    public Measurement ToUnit { get; }
    public double ConversionFactor { get; }

    private CommandGroup CommandGroup { get; }
    private bool StandardForm { get; } = false;

    public Convert(CommandGroup commandGroup, IMeasurementParser measurementParser)
    {
        CommandGroup = commandGroup;

        try
        {
            Input = measurementParser
                .ProcessMeasurement(commandGroup.CommandArguments[0]);
        }
        catch (KeyNotFoundException e)
        {
            throw new CommandError(ErrorCode.InvalidCommand, $"{commandGroup.CommandArguments[0]} is an unrecognised unit.", e);
        }

        ToUnit = measurementParser
            .ProcessMeasurement(commandGroup.CommandArguments[1], unitOnly: true);
        
        StandardForm = commandGroup.Flags.ContainsKey(Flag.StandardForm);

        if (Input.IsLegalConversion(ToUnit))
            ConversionFactor = Measurement.QuantityConversionFactor(Input, ToUnit);

        else throw new CommandError(ErrorCode.InvalidConversion, $"Cannot convert from {Input} to {ToUnit}");
    }

    public List<Measurement> Run() => [Input.ConvertTo(ToUnit, StandardForm)];

    public override string ToString() =>
        $"{CommandGroup.CommandType}:\tFrom: {Input}\tTo: {string.Join(".", ToUnit.Units)}";
}
